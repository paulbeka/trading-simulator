import { getConnection } from "./websocketClient";

const subscribedSymbols = new Set<string>();

export async function subscribe(symbol: string) {
  if (subscribedSymbols.has(symbol)) return;

  const conn = await getConnection();

  await conn.invoke("SubscribeTicker", symbol);

  subscribedSymbols.add(symbol);
  console.log("[SignalR] Subscribed:", symbol);
}

export async function unsubscribe(symbol: string) {
  if (!subscribedSymbols.has(symbol)) return;

  const conn = await getConnection();

  await conn.invoke("UnsubscribeTicker", symbol);

  subscribedSymbols.delete(symbol);
  console.log("[SignalR] Unsubscribed:", symbol);
}

export function getSubscribedSymbols(): string[] {
  return Array.from(subscribedSymbols);
}