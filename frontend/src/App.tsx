import './App.css'
import { Routes, Route, BrowserRouter } from "react-router-dom";
import ProtectedRoute from "./auth/ProtectedRoute";
import Login from './pages/LoginPage';
import Dashboard from './pages/Dashboard';

function App() {

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />

        <Route
          path="/dashboard"
          element={
            <ProtectedRoute>
              <Dashboard />
            </ProtectedRoute>
          }
        />
      </Routes>
    </BrowserRouter>
  )
}

export default App
