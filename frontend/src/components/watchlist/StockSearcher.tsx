import React, { useState, useEffect } from "react";
import {
  TextField,
  Autocomplete,
  CircularProgress,
  Box,
} from "@mui/material";
import { useTheme } from "@mui/material/styles";
import api from "../../api/api";

export interface Ticker {
  ticker: string;
  name: string;
  primary_exchange: string; 
  type: string;
}

interface StockSearcherProps {
  onSelect?: (ticker: Ticker) => void;
}

const StockSearcher: React.FC<StockSearcherProps> = ({ onSelect }) => {
  const theme = useTheme();

  const [options, setOptions] = useState<Ticker[]>([]);
  const [inputValue, setInputValue] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(false);

  useEffect(() => {
    if (!inputValue) {
      setOptions([]);
      return;
    }

    const timeout = setTimeout(() => {
      fetchTickers(inputValue);
    }, 300);

    return () => clearTimeout(timeout);
  }, [inputValue]);

  const fetchTickers = async (query: string): Promise<void> => {
    try {
      setLoading(true);
      const res = await api.get<Ticker[]>(
        `/api/tickers/search?q=${query}`
      );
      setOptions(res.data);
    } catch (err) {
      console.error("Ticker search error:", err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box sx={{ width: "100%", maxWidth: 400 }}>
      <Autocomplete<Ticker, false, false, true>
        freeSolo
        options={options}
        filterOptions={(x) => x}
        loading={loading}
        getOptionLabel={(option) =>
          typeof option === "string"
            ? option
            : `${option.ticker} - ${option.name}`
        }
        onInputChange={(_, value: string) => setInputValue(value)}
        onChange={(_, value) => {
          if (typeof value === "object" && value !== null) {
            onSelect?.(value);
            setInputValue("");
          }
        }}
        sx={{
          "& .MuiOutlinedInput-root": {
            borderRadius: theme.shape.borderRadius,
            backgroundColor: theme.palette.background.paper,
            color: theme.palette.text.primary,
          },
        }}
        renderInput={(params) => (
          <TextField
            {...params}
            placeholder="Search stocks (AAPL...)"
            variant="outlined"
            fullWidth
            InputProps={{
              ...params.InputProps,
              endAdornment: (
                <>
                  {loading && (
                    <CircularProgress
                      size={18}
                      sx={{ color: theme.palette.primary.main, mr: 1 }}
                    />
                  )}
                  {params.InputProps.endAdornment}
                </>
              ),
            }}
          />
        )}
        renderOption={(props, option) => (
          <li {...props}>
            <Box sx={{ display: "flex", flexDirection: "column" }}>
              <span style={{ fontWeight: 600 }}>
                {option.ticker}
              </span>
              <span
                style={{
                  fontSize: "0.85rem",
                  color: theme.palette.text.secondary,
                }}
              >
                {option.name} • {option.primary_exchange}
              </span>
            </Box>
          </li>
        )}
      />
    </Box>
  );
};

export default StockSearcher;