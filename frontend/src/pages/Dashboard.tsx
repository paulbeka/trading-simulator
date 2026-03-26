import React from "react";
import { Box } from "@mui/material";
import TickerSelector from "../components/watchlist/TickerSelector";
import { useWebSocket } from "../websocket/useWebSocket";
import AssetList from "../components/assetlist/AssetList";
import TotalPnl from "../components/totalpnl/TotalPnl";

const Dashboard: React.FC = () => {
  useWebSocket();

  return (
    <Box sx={{ p: 3 }}>
      <TotalPnl />
      <hr />
      <AssetList />
      <hr />
      <TickerSelector />
    </Box>
  );
};

export default Dashboard;