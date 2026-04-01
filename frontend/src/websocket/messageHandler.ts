import { useMarketStore } from "../stores/marketStore";
import { usePortfolioStore } from "../stores/portfolioStore";
import type { PriceUpdateMessage } from "./messages.types";

export function handlePnL(msg: any) {
  const ticker = msg.ticker ?? msg.Ticker;
  const price = msg.price ?? msg.Price;
  const positionPnL = msg.positionPnL ?? msg.PositionPnL;

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