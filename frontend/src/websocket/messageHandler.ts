import { useMarketStore } from "../stores/marketStore";
import { usePortfolioStore } from "../stores/portfolioStore";
import type { PnLUpdateMessage, PriceUpdateMessage } from "./messages.types";

export function handlePnL(msg: PnLUpdateMessage) {
  const { ticker, price, positionPnL } = msg;

  usePortfolioStore.getState().updatePosition(ticker, {
    pnl: positionPnL,
    price,
  });

  useMarketStore.getState().setPrice(ticker, price);
}

export function handlePrice(msg: PriceUpdateMessage) {
  const { ticker, price } = msg;

  useMarketStore.getState().setPrice(ticker, price);
}