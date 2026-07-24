import { getStoredToken } from '../auth/authService';

const API_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:8080';

export interface Question {
  position: number;
  text: string;
}

export async function fetchQuestions(): Promise<Question[]> {
  const token = getStoredToken();

  const response = await fetch(`${API_URL}/api/questions`, {
    headers: {
      Authorization: `Bearer ${token}`
    }
  });

  if (!response.ok) {
    throw new Error('Failed to load questions.');
  }

  return response.json();
}

export interface ResponseAnswer {
  position: number;
  answer: string;
}

export async function submitResponses(responses: ResponseAnswer[]): Promise<void> {
  const token = getStoredToken();

  const response = await fetch(`${API_URL}/api/responses`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`
    },
    body: JSON.stringify({ responses })
  });

  if (!response.ok) {
    throw new Error('Failed to submit responses.');
  }
}
