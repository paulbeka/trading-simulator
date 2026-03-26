import React, { useEffect, useState } from "react";
import {
  Box,
  Typography,
  Paper,
  Button,
  TextField,
} from "@mui/material";
import { getPositions, buyStock, sellStock } from "../../api/api";
import type { Position } from "../../api/api.types";
import { useMarketStore } from "../../stores/marketStore";
import { subscribe, unsubscribe } from "../../websocket/subscriptions";

const AssetList: React.FC = () => {
  const [positions, setPositions] = useState<Position[]>([]);
  const [quantities, setQuantities] = useState<Record<string, number>>({});
  const prices = useMarketStore((state) => state.prices);

  // Fetch positions
  useEffect(() => {
    const fetchPositions = async () => {
      try {
        const data = await getPositions();
        setPositions(data);

        data.forEach(async (p) => {
          setQuantities((prev) => ({
            ...prev,
            [p.ticker]: 1,
          }));

          await subscribe(p.ticker);
        });
      } catch (e) {
        console.error(e);
      }
    };

    fetchPositions();

    return () => {
      positions.forEach((p) => unsubscribe(p.ticker));
    };
  }, []);

  const updateQuantity = (ticker: string, delta: number) => {
    setQuantities((prev) => ({
      ...prev,
      [ticker]: Math.max(1, (prev[ticker] || 1) + delta),
    }));
  };

  const handleQuantityChange = (ticker: string, value: string) => {
    const num = parseInt(value, 10);
    setQuantities((prev) => ({
      ...prev,
      [ticker]: isNaN(num) || num < 1 ? 1 : num,
    }));
  };

  const handleBuy = async (ticker: string, quantity: number, price: number) => {
    try {
      await buyStock(ticker, quantity, price);
    } catch (e) {
      console.error(e);
    }
  };

  const handleSell = async (ticker: string, quantity: number, price: number) => {
    try {
      await sellStock(ticker, quantity, price);
    } catch (e) {
      console.error(e);
    }
  };

  return (
    <Box>
      <Typography variant="h5" sx={{ mb: 2 }}>
        Your Portfolio
      </Typography>

      {positions.length === 0 ? (
        <Typography color="text.secondary">
          No assets yet
        </Typography>
      ) : (
        positions.map((p) => {
          const price = prices[p.ticker];
          const quantity = quantities[p.ticker] || 1;

          const totalValue =
            price && !isNaN(price) ? price * p.quantity : null;

          const pnl =
            price && !isNaN(price)
              ? (price - p.avgEntryPrice) * p.quantity
              : null;

          return (
            <Paper
              key={p.ticker}
              sx={{
                p: 2,
                mb: 1,
                borderRadius: 2,
                display: "flex",
                alignItems: "center",
                gap: 2,
                flexWrap: "wrap",
              }}
            >
              <Box sx={{ minWidth: 120 }}>
                <Typography variant="h6">{p.ticker}</Typography>
                <Typography variant="body2" color="text.secondary">
                  Qty: {p.quantity}
                </Typography>
              </Box>

              <Box sx={{ minWidth: 140 }}>
                <Typography>
                  Price: {price ?? "Loading..."}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Avg: {p.avgEntryPrice}
                </Typography>
              </Box>

              <Box sx={{ minWidth: 160 }}>
                <Typography>
                  Value: {totalValue ? `$${totalValue.toFixed(2)}` : "—"}
                </Typography>
                <Typography
                  color={
                    pnl && pnl >= 0 ? "success.main" : "error.main"
                  }
                >
                  PnL:{" "}
                  {pnl ? `$${pnl.toFixed(2)}` : "—"}
                </Typography>
              </Box>

              <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                <Button
                  size="small"
                  variant="outlined"
                  onClick={() => updateQuantity(p.ticker, -1)}
                >
                  -
                </Button>

                <TextField
                  size="small"
                  type="number"
                  value={quantity}
                  onChange={(e) =>
                    handleQuantityChange(p.ticker, e.target.value)
                  }
                  sx={{ width: 80 }}
                  inputProps={{ min: 1 }}
                />

                <Button
                  size="small"
                  variant="outlined"
                  onClick={() => updateQuantity(p.ticker, 1)}
                >
                  +
                </Button>
              </Box>

              <Box sx={{ display: "flex", gap: 1 }}>
                <Button
                  variant="contained"
                  color="success"
                  onClick={() =>
                    handleBuy(p.ticker, quantity, Number(price))
                  }
                >
                  Buy
                </Button>

                <Button
                  variant="contained"
                  color="error"
                  onClick={() =>
                    handleSell(p.ticker, quantity, Number(price))
                  }
                >
                  Sell
                </Button>
              </Box>
            </Paper>
          );
        })
      )}
    </Box>
  );
};

export default AssetList;