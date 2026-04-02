import { Typography, Box } from "@mui/material";
import { usePortfolioStore } from "../../stores/portfolioStore";
import { useEffect } from "react";
import { getAccountBalance } from "../../api/api";

const formatNumber = (num: number) =>
  num.toLocaleString("en-US", {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  });

const getColor = (num: number) => (num >= 0 ? "#4caf50" : "#f44336");

const TotalPnl = () => {
  const positions = usePortfolioStore((state) => state.positions);
  const cashBalance = usePortfolioStore((state) => state.cashBalance);
  const setCashBalance = usePortfolioStore((state) => state.setCashBalance);

  const totalPnl = Object.values(positions).reduce(
    (sum, pos) => sum + (pos.pnl || 0),
    0
  );

  const realisedPnl = cashBalance - 100000;
  const total = realisedPnl + totalPnl;

  useEffect(() => {
    const fetchBalance = async () => {
      const balance = await getAccountBalance();
      setCashBalance(balance);
    };

    fetchBalance();
  }, [positions]);

  return (
    <Box>
      <Typography sx={{ fontWeight: 500 }}>
        Cash: ${formatNumber(cashBalance)} |{" "}
        Unrealised PnL:{" "}
        <span style={{ color: getColor(totalPnl) }}>
          ${formatNumber(totalPnl)}
        </span>{" "}
        | Realised PnL:{" "}
        <span style={{ color: getColor(realisedPnl) }}>
          ${formatNumber(realisedPnl)}
        </span>{" "}
        | Total PnL:{" "}
        <span style={{ color: getColor(total) }}>
          ${formatNumber(total)}
        </span>
      </Typography>
    </Box>
  );
};

export default TotalPnl;