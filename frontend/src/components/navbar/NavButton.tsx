import React from "react";
import { Button } from "@mui/material";
import { styled } from "@mui/material/styles";
import { useNavigate } from "react-router-dom";

interface NavButtonProps {
  active?: boolean;
  link: string;
  children: React.ReactNode;
}

const StyledNavButton = styled(Button, {
  shouldForwardProp: (prop) => prop !== "active",
})<{ active?: boolean }>(({ theme, active }) => ({
  color: active ? theme.palette.common.white : theme.palette.grey[400],
  fontWeight: 500,
  position: "relative",
  textTransform: "none",
  fontSize: "1rem",
  padding: "6px 8px",

  "&::after": {
    content: '""',
    position: "absolute",
    left: 0,
    bottom: 0,
    width: "100%",
    height: "2px",
    backgroundColor: theme.palette.common.white,
    transform: active ? "scaleX(1)" : "scaleX(0)",
    transformOrigin: "left",
    transition: "transform 0.3s ease",
  },

  "&:hover::after": {
    transform: "scaleX(1)",
  },

  "&:hover": {
    color: theme.palette.common.white,
    backgroundColor: "transparent",
  },
}));

export const NavButton = ({ link, active, children }: NavButtonProps) => {
  const navigate = useNavigate();

  return (
    <StyledNavButton active={active} onClick={() => navigate(link)}>
      {children}
    </StyledNavButton>
  );
};