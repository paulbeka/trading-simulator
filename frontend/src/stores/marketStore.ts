import { create } from "zustand";

type MarketState = {
  prices: Record<string, number>;
  setPrice: (symbol: string, price: number) => void;
};

export const useMarketStore = create<MarketState>((set) => ({
  prices: {},

  setPrice: (symbol, price) =>
  set((state) => {
    const newState = {
      prices: {
        ...state.prices,
        [symbol]: price,
      },
    };

    return newState;
  }),
}));