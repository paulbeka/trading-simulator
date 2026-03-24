import './App.css'
import { Routes, Route, BrowserRouter } from "react-router-dom";
import { Navigate } from "react-router-dom";
import { useAuthStore } from './auth/authStore';
import ProtectedRoute from "./auth/ProtectedRoute";
import Login from './pages/LoginPage';
import Dashboard from './pages/Dashboard';
import RegisterPage from './pages/RegisterPage';
import BasePage from './pages/BasePage';
import AboutPage from './pages/AboutPage';


function App() {
  const { user } = useAuthStore();

  return (
    <BrowserRouter>
      <Routes>
        <Route element={<BasePage />}>
          <Route path="/" element={user
          ? <Navigate to="/dashboard" replace />
          : <Navigate to="/login" replace />} />
          
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<RegisterPage />} />

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
  )
}

export default App
