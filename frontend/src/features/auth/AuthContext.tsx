import { createContext, useContext, useMemo, useState } from 'react';
import { clearToken, getStoredToken, login, storeToken } from './authService';

interface AuthContextValue {
  isAuthenticated: boolean;
  signIn: (username: string, password: string) => Promise<void>;
  signOut: () => void;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [token, setToken] = useState<string | null>(() => getStoredToken());

  const value = useMemo<AuthContextValue>(
    () => ({
      isAuthenticated: Boolean(token),
      signIn: async (username: string, password: string) => {
        const response = await login(username, password);
        storeToken(response.token);
        setToken(response.token);
      },
      signOut: () => {
        clearToken();
        setToken(null);
      }
    }),
    [token]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }

  return context;
}
