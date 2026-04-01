import { Typography, Box } from "@mui/material";
import { usePortfolioStore } from "../../stores/portfolioStore";
import { useEffect } from "react";
import { getAccountBalance } from "../../api/api";

const TotalPnl = () => {
  const positions = usePortfolioStore((state) => state.positions);
  const cashBalance = usePortfolioStore((state) => state.cashBalance);
  
  const setCashBalance = usePortfolioStore((state) => state.setCashBalance);

  const totalPnl = Object.values(positions).reduce(
    (sum, pos) => sum + (pos.pnl || 0),
    0
  );

  useEffect(() => {
    const fetchBalance = async () => {
      const balance = await getAccountBalance();
      setCashBalance(balance); 
    };

    fetchBalance();
  }, [positions]);

  return (
    <Box>
      <Typography>
        Cash: ${cashBalance.toFixed(2)} | Total PnL: ${totalPnl.toFixed(2)}
      </Typography>
    </Box>
  );
};

export default TotalPnl;