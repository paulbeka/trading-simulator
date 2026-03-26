import React, { useState } from "react";
import { Box, Typography, Paper, IconButton, Button, TextField } from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import StockSearcher from "./StockSearcher";
import type { Ticker } from "./StockSearcher";
import { subscribe, unsubscribe } from "../../websocket/subscriptions";
import { useMarketStore } from "../../stores/marketStore";
import { usePortfolioStore } from "../../stores/portfolioStore"; // ✅ NEW
import { buyStock, sellStock } from "../../api/api";

const TickerSelector: React.FC = () => {
  const [selected, setSelected] = useState<Ticker[]>([]);
  const [quantities, setQuantities] = useState<Record<string, number>>({});
  const prices = useMarketStore((state) => state.prices);

  const updatePosition = usePortfolioStore((state) => state.updatePosition); // ✅ NEW

  const handleSelect = async (ticker: Ticker) => {
    setSelected((prev) => {
      if (prev.find((t) => t.ticker === ticker.ticker)) return prev;
      return [...prev, ticker];
    });

    setQuantities((prev) => ({
      ...prev,
      [ticker.ticker]: 1,
    }));

    await subscribe(ticker.ticker);
  };

  const handleDelete = async (tickerToDelete: string) => {
    setSelected((prev) =>
      prev.filter((t) => t.ticker !== tickerToDelete)
    );

    setQuantities((prev) => {
      const updated = { ...prev };
      delete updated[tickerToDelete];
      return updated;
    });

    await unsubscribe(tickerToDelete);
  };

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
    if (!price || isNaN(price)) return;
    try {
      const res = await buyStock(ticker, quantity, price);

      updatePosition(res.ticker, {
        price: Number(res.price),
      });

    } catch (e) {
      console.error(e);
    }
  };

  const handleSell = async (ticker: string, quantity: number, price: number) => {
    if (!price || isNaN(price)) return;
    try {
      const res = await sellStock(ticker, quantity, price);

      updatePosition(res.ticker, {
        price: Number(res.price),
      });

    } catch (e) {
      console.error(e);
    }
  };

  return (
    <Box>
      <StockSearcher onSelect={handleSelect} />

      <Box sx={{ mt: 3 }}>
        {selected.length === 0 ? (
          <Typography variant="body2" color="text.secondary">
            No tickers selected yet
          </Typography>
        ) : (
          selected.map((ticker) => {
            const price = prices[ticker.ticker];
            const quantity = quantities[ticker.ticker] || 1;
            const total =
              price && !isNaN(price) ? (price * quantity).toFixed(2) : null;

            return (
              <Paper
                key={ticker.ticker}
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
                  <Typography variant="h6">{ticker.ticker}</Typography>
                  <Typography variant="body2" color="text.secondary">
                    {ticker.name}
                  </Typography>
                </Box>

                <Typography sx={{ minWidth: 100 }}>
                  {price ?? "Loading..."}
                </Typography>

                <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                  <Button
                    size="small"
                    variant="outlined"
                    onClick={() => updateQuantity(ticker.ticker, -1)}
                  >
                    -
                  </Button>

                  <TextField
                    size="small"
                    type="number"
                    value={quantity}
                    onChange={(e) =>
                      handleQuantityChange(ticker.ticker, e.target.value)
                    }
                    sx={{ width: 80 }}
                    inputProps={{ min: 1 }}
                  />

                  <Button
                    size="small"
                    variant="outlined"
                    onClick={() => updateQuantity(ticker.ticker, 1)}
                  >
                    +
                  </Button>
                </Box>

                <Typography sx={{ minWidth: 120 }}>
                  Total: {total ? `$${total}` : "—"}
                </Typography>

                <Box sx={{ display: "flex", gap: 1 }}>
                  <Button
                    variant="contained"
                    color="success"
                    onClick={() =>
                      handleBuy(ticker.ticker, quantity, Number(price))
                    }
                  >
                    Buy
                  </Button>
                  <Button
                    variant="contained"
                    color="error"
                    onClick={() =>
                      handleSell(ticker.ticker, quantity, Number(price))
                    }
                  >
                    Sell
                  </Button>
                </Box>

                <IconButton
                  onClick={() => handleDelete(ticker.ticker)}
                  color="error"
                  sx={{ marginLeft: "auto" }}
                >
                  <DeleteIcon />
                </IconButton>
              </Paper>
            );
          })
        )}
      </Box>
    </Box>
  );
};

export default TickerSelector;