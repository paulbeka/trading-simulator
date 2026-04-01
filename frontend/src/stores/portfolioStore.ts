import { create } from "zustand";

export type Position = {
  ticker: string;
  quantity: number;
  avgEntryPrice: number;
  pnl?: number;
  price?: number;
};

type PortfolioState = {
  positions: Record<string, Position>;
  cashBalance: number;

  setPositions: (positions: Position[]) => void;
  updatePosition: (symbol: string, data: Partial<Position>) => void;
  setCashBalance: (balance: number) => void;
};

export const usePortfolioStore = create<PortfolioState>((set) => ({
  positions: {},
  cashBalance: 0,

  setPositions: (positionsArray) =>
    set((state) => ({
      positions: Object.fromEntries(
        positionsArray.map((p) => {
          const existing = state.positions[p.ticker];

          return [
            p.ticker,
            {
              ...p,
              pnl: existing?.pnl,
              price: existing?.price,
            },
          ];
        })
      ),
    })),

  updatePosition: (symbol, data) =>
    set((state) => ({
      positions: {
        ...state.positions,
        [symbol]: {
          ...state.positions[symbol],
          ...data,
          ticker: symbol,
        },
      },
    })),

  setCashBalance: (balance) =>
    set(() => ({
      cashBalance: balance,
    })),
}));