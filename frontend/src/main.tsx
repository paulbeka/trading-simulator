import { createRoot } from 'react-dom/client'
import { ThemeProvider } from "@mui/material/styles";
import './index.css'
import App from './App.tsx'
import { theme } from "./theme/theme.ts";
import CssBaseline from "@mui/material/CssBaseline";

createRoot(document.getElementById('root')!).render(
  <ThemeProvider theme={theme}>
    <CssBaseline />
    <App />
  </ThemeProvider>
)
