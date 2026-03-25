import React from "react";
import { Box } from "@mui/material";
import TickerSelector from "../components/stock/TickerSelector";
import { useWebSocket } from "../websocket/useWebSocket";

const Dashboard: React.FC = () => {
  useWebSocket();

  return (
    <Box sx={{ p: 3 }}>
      <TickerSelector />
    </Box>
  );
};

export default Dashboard;