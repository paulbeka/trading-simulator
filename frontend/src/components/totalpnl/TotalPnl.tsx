import { Typography, Box } from "@mui/material";
import { usePortfolioStore } from "../../stores/portfolioStore";

const TotalPnl = () => {
  const positions = usePortfolioStore((state) => state.positions);

  const totalPnl = Object.values(positions).reduce(
    (sum, pos) => sum + (pos.pnl || 0),
    0
  );

  return (
    <Box>
      <Typography variant="h6">
        Total PnL: ${totalPnl.toFixed(2)}
      </Typography>
    </Box>
  );
};

export default TotalPnl;