import type { WeeklyBoardRowDto } from './types';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'https://localhost:7192';

async function getJson<T>(path: string): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`);
  if (!response.ok) {
    throw new Error(`${path} -> HTTP ${response.status}`);
  }
  return (await response.json()) as T;
}

export const apiClient = {
  getHealth: () => getJson<{ status: string }>('/api/health'),
  getWeeklyBoard: (programDayId: number, workoutWeekId: number) =>
    getJson<WeeklyBoardRowDto[]>(`/api/program-days/${programDayId}/weekly-board?workoutWeekId=${workoutWeekId}`),
};
