import { create } from "zustand";

export type Position = {
  pnl: number;
  price: number;
};

type PortfolioState = {
  positions: Record<string, Position>;
  updatePosition: (symbol: string, data: Partial<Position>) => void;
};

export const usePortfolioStore = create<PortfolioState>((set) => ({
  positions: {},

  updatePosition: (symbol, data) =>
    set((state) => ({
      positions: {
        ...state.positions,
        [symbol]: {
          ...state.positions[symbol],
          ...data,
        },
      },
    })),
}));
