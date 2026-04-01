import React, { useEffect, useState } from "react";
import {
  Box,
  Typography,
  Paper,
  Button,
  TextField,
} from "@mui/material";
import { getPositions, buyStock, sellStock } from "../../api/api";
import { useMarketStore } from "../../stores/marketStore";
import { usePortfolioStore } from "../../stores/portfolioStore";
import { subscribe, unsubscribe } from "../../websocket/subscriptions";
import { getConnection } from "../../websocket/websocketClient";

const AssetList: React.FC = () => {
  // ❌ removed local positions state
  const positions = usePortfolioStore((state) => state.positions);
  const setPositions = usePortfolioStore((state) => state.setPositions);

  const [quantities, setQuantities] = useState<Record<string, number>>({});

  const prices = useMarketStore((state) => state.prices);

  useEffect(() => {
    let activeTickers: string[] = [];
    let mounted = true;

    const setup = async () => {
      try {
        await getConnection();

        const data = await getPositions();
        if (!mounted) return;

        // ✅ store instead of local state
        setPositions(data);

        activeTickers = data.map((p) => p.ticker);

        for (const p of data) {
          setQuantities((prev) => ({
            ...prev,
            [p.ticker]: 1,
          }));

          try {
            await subscribe(p.ticker);
          } catch (e) {
            console.error("Subscribe failed", p.ticker, e);
          }
        }
      } catch (e) {
        console.error(e);
      }
    };

    setup();

    return () => {
      mounted = false;
      for (const ticker of activeTickers) {
        unsubscribe(ticker);
      }
    };
  }, [setPositions]);

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

  const handleBuy = async (ticker: string, quantity: number, price?: number) => {
    if (!price) return;
    try {
      await buyStock(ticker, quantity, price);

      // ✅ refresh store
      const data = await getPositions();
      setPositions(data);
    } catch (e) {
      console.error(e);
    }
  };

  const handleSell = async (ticker: string, quantity: number, price?: number) => {
    if (!price) return;
    try {
      await sellStock(ticker, quantity, price);

      // ✅ refresh store
      const data = await getPositions();
      setPositions(data);
    } catch (e) {
      console.error(e);
    }
  };

  const positionArray = Object.values(positions);

  return (
    <Box>
      <Typography variant="h5" sx={{ mb: 2 }}>
        Your Portfolio
      </Typography>

      {positionArray.length === 0 ? (
        <Typography color="text.secondary">
          No assets yet
        </Typography>
      ) : (
        positionArray.map((p) => {
          const price = prices[p.ticker];
          const pnl = p.pnl;
          const quantity = quantities[p.ticker] || 1;

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
                  Value: {price ? `$${(price * p.quantity).toFixed(2)}` : "—"}
                </Typography>
                <Typography
                  color={
                    pnl !== undefined && pnl >= 0
                      ? "success.main"
                      : "error.main"
                  }
                >
                  PnL: {pnl !== undefined ? `$${pnl.toFixed(2)}` : "—"}
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
                    handleBuy(p.ticker, quantity, price)
                  }
                >
                  Buy
                </Button>

                <Button
                  variant="contained"
                  color="error"
                  onClick={() =>
                    handleSell(p.ticker, quantity, price)
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