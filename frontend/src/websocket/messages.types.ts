export type PnLUpdateMessage = {
  type: "pnl_update";
  symbol: string;
  pnl: number;
  price: number;
};

export type PriceUpdateMessage = {
  type: "price_update";
  ticker: string;
  price: number;
};

export type IncomingMessage = PnLUpdateMessage | PriceUpdateMessage;