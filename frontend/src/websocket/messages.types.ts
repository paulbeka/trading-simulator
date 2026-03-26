export type PnLUpdateMessage = {
  user: string;
  ticker: string;
  price: number;
  positionPnL: number;
  positionDelta: number;
  totalPnL: number;
  totalDelta: number;
};

export type PriceUpdateMessage = {
  type: "price_update";
  ticker: string;
  price: number;
};

export type IncomingMessage = PnLUpdateMessage | PriceUpdateMessage;