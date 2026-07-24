const API_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:8080';

export interface LoginResponse {
  token: string;
}

export async function login(username: string, password: string): Promise<LoginResponse> {
  const response = await fetch(`${API_URL}/api/auth/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ username, password })
  });

  if (!response.ok) {
    throw new Error('Invalid username or password.');
  }

  return response.json();
}

export function getStoredToken(): string | null {
  return localStorage.getItem('authToken');
}

export function storeToken(token: string): void {
  localStorage.setItem('authToken', token);
}

export function clearToken(): void {
  localStorage.removeItem('authToken');
}
