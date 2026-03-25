import axios from "axios";
import { useAuthStore } from "../auth/authStore";
import { BASE_URL } from "../config/Config";

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

type TradeRequest = {
  ticker: string;
  quantity: number;
  price: number;
  side: "buy" | "sell";
};

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

export default api;
