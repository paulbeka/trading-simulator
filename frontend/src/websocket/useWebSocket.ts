import { useEffect } from "react";
import { getConnection } from "../websocket/websocketClient";
import { handlePnL, handlePrice } from "./messageHandler";
import type { PnLUpdateMessage, PriceUpdateMessage } from "./messages.types";

export function useWebSocket() {
  useEffect(() => {
    let isMounted = true;

    async function setup() {
      const conn = await getConnection();
      if (!isMounted) return;

      conn.on("PnLUpdate", (msg: PnLUpdateMessage) => {
        handlePnL(msg);
      });

      conn.on("ReceiveTickerUpdate", (msg: PriceUpdateMessage) => {
        handlePrice(msg);
      });
    }

    setup();

    return () => {
      isMounted = false;

      getConnection().then(conn => {
        conn.off("PnLUpdate");
        conn.off("PriceUpdate");
      });
    };
  }, []);
}