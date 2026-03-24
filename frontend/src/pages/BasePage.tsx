import { Outlet } from "react-router-dom";
import Navbar from "../components/navbar/Navbar";
import styles from "./CSS/BasePage.module.css";
import { useAuthStore } from "../auth/authStore";
import { useLocation } from "react-router-dom";
import Footer from "../components/footer/Footer";

const BasePage = () => {
  const user = useAuthStore((state) => state.user);
  const location = useLocation();

  const hideNavbar = ["/login", "/register"].includes(location.pathname);
  const isLoggedIn = user != null;

  return (
    <div className={styles.base_page}>
      {isLoggedIn && !hideNavbar && <Navbar />}

      <main className="content">
        <Outlet />
      </main>

      <Footer />
    </div>
  );
};

export default BasePage;