export type PnLUpdateMessage = {
  User: string;
  Ticker: string;
  Price: number;
  PositionPnL: number;
  PositionDelta: number;
  TotalPnL: number;
  TotalDelta: number;
};

export type PriceUpdateMessage = {
  type: "price_update";
  ticker: string;
  price: number;
};

export type IncomingMessage = PnLUpdateMessage | PriceUpdateMessage;