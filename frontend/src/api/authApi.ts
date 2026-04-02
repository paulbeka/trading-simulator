import api from "./api";

export const login = async (email: string, password: string) => {
  const res = await api.post("/api/Auth/login", { email, password });
  return res.data;
};

export const register = async (email: string, password: string) => {
  const res = await api.post("/api/Auth/register", { email, password });
  return res.data;
}

export const getMe = async (token: string) => {
  const res = await api.get("/api/users/me", {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  return res.data;
};
