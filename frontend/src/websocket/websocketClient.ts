import * as signalR from "@microsoft/signalr";
import { BASE_URL } from "../config/Config";
import { getSubscribedSymbols } from "./subscriptions";
import { useAuthStore } from "../auth/authStore";

let connection: signalR.HubConnection | null = null;

export async function getConnection(): Promise<signalR.HubConnection> {
  if (!connection) {
    connection = new signalR.HubConnectionBuilder()
      .withUrl(`${BASE_URL}pnlHub`, {
        accessTokenFactory: () => localStorage.getItem("token") || "",
      })
      .withAutomaticReconnect()
      .build();

    connection.onclose(() => {
      console.log("[SignalR] Disconnected");
    });

    connection.onreconnected(async () => {
      console.log("[SignalR] Reconnected");

      const user = useAuthStore.getState().user;

      if (user?.id) {
        await connection!.invoke("Subscribe", user.id);
        console.log("[SignalR] Re-subscribed to user:", user.id);
      }

      const symbols = getSubscribedSymbols();
      for (const symbol of symbols) {
        await connection!.invoke("SubscribeTicker", symbol);
      }
    });

    await connection.start();
    console.log("[SignalR] Connected");

    const user = useAuthStore.getState().user;

    if (user?.id) {
      await connection.invoke("Subscribe", user.id);
      console.log("[SignalR] Subscribed to user:", user.id);
    }

    const symbols = getSubscribedSymbols();
    for (const symbol of symbols) {
      await connection.invoke("SubscribeTicker", symbol);
    }
  }

  return connection;
}