export type TradeRequest = {
  ticker: string;
  quantity: number;
  price: number;
  side: "buy" | "sell";
};

export type Position = {
  ticker: string;
  quantity: number;
  avgEntryPrice: number;
  updatedAt: string;
};