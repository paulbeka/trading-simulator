import { Box, Typography } from "@mui/material";

const Footer = () => {
  return (
    <Box
      component="footer"
      sx={(theme) => ({
        mt: "auto",
        background: "rgba(20,20,20,0.7)",
        backdropFilter: "blur(10px)",
        borderTop: `1px solid ${theme.palette.divider}`,
        color: "white",
      })}
    >
      <Box
        sx={{
          maxWidth: "1200px",
          width: "100%",
          margin: "0 auto",
          py: 2,
          px: 2,
          textAlign: "center",
        }}
      >
        <Typography variant="body2">
          © {new Date().getFullYear()} Paul Bekaert
        </Typography>
      </Box>
    </Box>
  );
};

export default Footer;