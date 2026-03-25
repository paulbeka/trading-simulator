import * as signalR from "@microsoft/signalr";
import { BASE_URL } from "../config/Config";
import { getSubscribedSymbols } from "./subscriptions";

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

      const symbols = getSubscribedSymbols();
      for (const symbol of symbols) {
        await connection!.invoke("SubscribeTicker", symbol);
      }
    });

    await connection.start();
    console.log("[SignalR] Connected");
  }

  return connection;
}