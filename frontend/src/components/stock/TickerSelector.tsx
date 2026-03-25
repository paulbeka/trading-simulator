import React, { useState } from "react";
import { Box, Typography, Paper, IconButton, Button, TextField } from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";

type Props = {
  ticker: string;
  name: string;
  exchange: string;
  type: string;
  price?: number;
  quantityHeld?: number;
  onDelete: () => void;
  onBuy: (qty: number) => void;
  onSell: (qty: number) => void;
};

const TickerRow: React.FC<Props> = ({
  ticker,
  name,
  exchange,
  type,
  price,
  quantityHeld = 0,
  onDelete,
  onBuy,
  onSell,
}) => {
  const [qty, setQty] = useState(1);

  const total = price ? qty * price : 0;

  return (
    <Paper
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
        <Typography variant="h6">{ticker}</Typography>
        <Typography variant="caption" color="text.secondary">
          {name}
        </Typography>
      </Box>

      <Typography sx={{ width: 80 }}>
        {price ?? "Loading..."}
      </Typography>

      <Typography sx={{ width: 100 }}>
        Held: {quantityHeld}
      </Typography>

      <TextField
        type="number"
        size="small"
        value={qty}
        onChange={(e) => setQty(Math.max(1, Number(e.target.value)))}
        sx={{ width: 80 }}
      />

      <Typography sx={{ width: 120 }}>
        {price ? `€${total.toFixed(2)}` : "-"}
      </Typography>

      <Button
        variant="contained"
        color="success"
        onClick={() => onBuy(qty)}
        disabled={!price}
      >
        Buy
      </Button>

      <Button
        variant="contained"
        color="warning"
        onClick={() => onSell(qty)}
        disabled={!price || quantityHeld === 0}
      >
        Sell
      </Button>

      <IconButton onClick={onDelete} color="error">
        <DeleteIcon />
      </IconButton>
    </Paper>
  );
};

export default TickerRow;