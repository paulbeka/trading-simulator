import React, { useState } from "react";
import { Box, Typography, Paper, IconButton } from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import StockSearcher from "./StockSearcher";
import type { Ticker } from "./StockSearcher";

const TickerSelector: React.FC = () => {
  const [selected, setSelected] = useState<Ticker[]>([]);

  const handleSelect = (ticker: Ticker) => {
    setSelected((prev) => {
      if (prev.find((t) => t.ticker === ticker.ticker)) return prev;
      return [...prev, ticker];
    });
  };

  const handleDelete = (tickerToDelete: string) => {
    setSelected((prev) =>
      prev.filter((t) => t.ticker !== tickerToDelete)
    );
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
          selected.map((ticker) => (
            <Paper
              key={ticker.ticker}
              sx={{
                p: 2,
                mb: 1,
                borderRadius: 2,
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
              }}
            >
              <Box>
                <Typography variant="h6">
                  {ticker.ticker}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  {ticker.name} • {ticker.primary_exchange} • {ticker.type}
                </Typography>
              </Box>

              <IconButton
                onClick={() => handleDelete(ticker.ticker)}
                color="error"
              >
                <DeleteIcon />
              </IconButton>
            </Paper>
          ))
        )}
      </Box>
    </Box>
  );
};

export default TickerSelector;