import { useMarketStore } from "../stores/marketStore";
import { usePortfolioStore } from "../stores/portfolioStore";
import type { PnLUpdateMessage, PriceUpdateMessage } from "./messages.types";

export function handlePnL(msg: PnLUpdateMessage) {
  usePortfolioStore.getState().updatePosition(msg.Ticker, {
    pnl: msg.PositionPnL,
    price: msg.Price,
  });

  useMarketStore.getState().setPrice(msg.Ticker, msg.Price);
}

export function handlePrice(msg: PriceUpdateMessage) {
  const { ticker, price } = msg;

  useMarketStore.getState().setPrice(ticker, price);
}