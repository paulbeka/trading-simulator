import axios from "axios";
import { useAuthStore } from "../auth/authStore";
import { BASE_URL } from "../config/Config";
import type { TradeRequest, Position } from "./api.types";

const api = axios.create({
  baseURL: BASE_URL,
});

api.interceptors.request.use((config) => {
  const token = useAuthStore.getState().token;

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

const executeTrade = async (data: TradeRequest) => {
  const response = await api.post(`/api/trades/${data.side.toLowerCase()}`, {
    ticker: data.ticker,
    quantity: data.quantity,
    price: data.price,
    side: data.side,
  });

  return response.data;
};

export const buyStock = async (ticker: string, quantity: number, price: number) => {
  return executeTrade({
    ticker,
    quantity,
    price,
    side: "buy",
  });
};

export const sellStock = async (ticker: string, quantity: number, price: number) => {
  return executeTrade({
    ticker,
    quantity,
    price,
    side: "sell",
  });
};

export const getPositions = async (): Promise<Position[]> => {
  const response = await api.get("/api/trades/positions");
  return response.data;
};

export const getAccountBalance = async (): Promise<number> => {
  const response = await api.get("/api/trades/balance");
  return response.data.cashBalance;
};

export default api;
