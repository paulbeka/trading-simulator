import { AppBar, Toolbar, Box, Typography } from "@mui/material";
import { Link, useLocation } from "react-router-dom";
import { NavButton } from "./NavButton";

const Navbar = () => {
  const location = useLocation();

  const navItems = [
    { name: "Dashboard", link: "/dashboard" },
    { name: "About", link: "/about" },
  ];

  return (
    <AppBar
      position="sticky"
      elevation={0}
      sx={(theme) => ({
        background: "rgba(20,20,20,0.7)",
        backdropFilter: "blur(10px)",
        borderBottom: `1px solid ${theme.palette.divider}`,
      })}
    >
      <Toolbar
        sx={{
          maxWidth: "1200px",
          width: "100%",
          margin: "0 auto",
          display: "flex",
          justifyContent: "space-between",
        }}
      >
        <Typography
          variant="h6"
          sx={{ textDecoration: "none", color: "white" }}
        >
          Trading Simulator
        </Typography>

        <Box sx={{ display: "flex", gap: 3 }}>
          {navItems.map((item) => {
            const isActive = location.pathname === item.link;

            return (
              <NavButton
								key={item.name}
								link={item.link}
								active={isActive}
							>
								{item.name}
							</NavButton>
            );
          })}
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar;