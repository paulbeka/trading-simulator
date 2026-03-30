import "./App.css";
import { Routes, Route, BrowserRouter, Navigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { useAuthStore } from "./auth/authStore";
import ProtectedRoute from "./auth/ProtectedRoute";

import Login from "./pages/LoginPage";
import Dashboard from "./pages/Dashboard";
import RegisterPage from "./pages/RegisterPage";
import BasePage from "./pages/BasePage";
import AboutPage from "./pages/AboutPage";
import Loading from "./components/util/Loading";

function App() {
  const user = useAuthStore((s) => s.user);
  const checkAuth = useAuthStore((s) => s.checkAuth);
  const authChecked = useAuthStore((s) => s.authChecked);

  const [hydrated, setHydrated] = useState(useAuthStore.persist.hasHydrated());

  useEffect(() => {
    const unsub = useAuthStore.persist.onFinishHydration(() => {
      setHydrated(true);
    });

    if (useAuthStore.persist.hasHydrated()) {
      setHydrated(true);
    }

    return () => unsub();
  }, []);

  useEffect(() => {
    if (hydrated) {
      checkAuth();
    }
  }, [hydrated, checkAuth]);

  if (!hydrated || !authChecked) {
    return <Loading />;
  }

  return (
    <BrowserRouter>
      <Routes>
        <Route element={<BasePage />}>
          <Route
            path="/"
            element={
              user ? (
                <Navigate to="/dashboard" replace />
              ) : (
                <Navigate to="/login" replace />
              )
            }
          />

          <Route
            path="/login"
            element={
              user ? <Navigate to="/dashboard" replace /> : <Login />
            }
          />

          <Route
            path="/register"
            element={
              user ? <Navigate to="/dashboard" replace /> : <RegisterPage />
            }
          />

          <Route path="/about" element={<AboutPage />} />

          <Route
            path="/dashboard"
            element={
              <ProtectedRoute>
                <Dashboard />
              </ProtectedRoute>
            }
          />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;