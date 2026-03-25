import { useMarketStore } from "../stores/marketStore";
import { usePortfolioStore } from "../stores/portfolioStore";
import type { PnLUpdateMessage, PriceUpdateMessage } from "./messages.types";

export function handlePnL(msg: PnLUpdateMessage) {
  const { symbol, pnl, price } = msg;

  usePortfolioStore.getState().updatePosition(symbol, {
    pnl,
    price,
  });

  useMarketStore.getState().setPrice(symbol, price);
}

export function handlePrice(msg: PriceUpdateMessage) {
  const { ticker, price } = msg;

  useMarketStore.getState().setPrice(ticker, price);
}