import React from "react";
import { Box } from "@mui/material";
import TickerSelector from "../components/stock/TickerSelector";

const Dashboard: React.FC = () => {

  return (
    <Box sx={{ p: 3 }}>
      <TickerSelector />
    </Box>
  );
};

export default Dashboard;