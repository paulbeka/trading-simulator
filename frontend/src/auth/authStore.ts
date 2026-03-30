import { create } from "zustand";
import { persist } from "zustand/middleware";

type User = {
  id: string;
  email?: string;
};

type AuthState = {
  user: User | null;
  token: string | null;
  authChecked: boolean;
  setAuth: (user: User, token: string) => void;
  logout: () => void;
  checkAuth: () => Promise<void>;
};

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      user: null,
      token: null,
      authChecked: false,

      setAuth: (user, token) => {
        set({
          user,
          token,
          authChecked: true,
        });
      },

      logout: () => {
        set({
          user: null,
          token: null,
          authChecked: true,
        });
      },

      checkAuth: async () => {
        const { token } = get();

        if (!token) {
          set({
            user: null,
            authChecked: true,
          });
          return;
        }

        try {
          const response = await fetch("/api/users/me", {
            method: "GET",
            headers: {
              Authorization: `Bearer ${token}`,
            },
          });

          if (!response.ok) {
            set({
              user: null,
              token: null,
              authChecked: true,
            });
            return;
          }

          const user = await response.json();

          set({
            user,
            authChecked: true,
          });
        } catch {
          set({
            user: null,
            token: null,
            authChecked: true,
          });
        }
      },
    }),
    {
      name: "auth-storage",
    }
  )
);