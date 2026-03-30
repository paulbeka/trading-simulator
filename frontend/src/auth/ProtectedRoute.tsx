import { Navigate } from "react-router-dom";
import { useAuthStore } from "./authStore";
import Loading from "../components/util/Loading";

const ProtectedRoute = ({ children }: { children: React.ReactNode }) => {
  const authChecked = useAuthStore((s) => s.authChecked);
  const user = useAuthStore((s) => s.user);

  if (!authChecked) {
    return <Loading />;
  }

  if (!user) {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
};

export default ProtectedRoute;