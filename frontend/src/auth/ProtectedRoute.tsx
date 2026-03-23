import { Navigate } from "react-router-dom";
import { useAuthStore } from "./authStore";
import { useEffect, useState } from "react";

const ProtectedRoute = ({ children }: any) => {
  const token = useAuthStore((s) => s.token);
  const [hydrated, setHydrated] = useState(false);

  useEffect(() => {
    const unsub = useAuthStore.persist.onFinishHydration(() => {
      setHydrated(true);
    });

    if (useAuthStore.persist.hasHydrated()) {
      setHydrated(true);
    }

    return () => unsub();
  }, []);

  if (!hydrated) return null;

  if (!token) return <Navigate to="/login" replace />;

  return children;
};

export default ProtectedRoute;
