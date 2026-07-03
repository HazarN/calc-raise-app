import Constants from 'expo-constants';
import type { WeeklyBoardRowDto } from './types';

const API_PORT = 5080;

/**
 * Backend hosting kararı henüz verilmedi (bkz. backlog.md CR-0010, Paused), yani şu an
 * Expo Go'nun backend'e ulaşabildiği tek yol aynı Wi-Fi ağı (LAN). Metro bundler'ın kendi
 * LAN adresini (Constants.expoConfig.hostUri, örn. "192.168.1.23:8081") kullanıp portu
 * bizim API portumuzla değiştiriyoruz — böylece geliştirici IP'yi elle güncellemek zorunda
 * kalmıyor. Gerçek bir hosting kurulunca bu, sabit bir URL'e dönüşecek.
 */
function resolveApiBaseUrl(): string {
  const override = Constants.expoConfig?.extra?.apiBaseUrlOverride as string | undefined;
  if (override) {
    return override;
  }

  const hostUri = Constants.expoConfig?.hostUri;
  const lanHost = hostUri?.split(':')[0];
  if (!lanHost) {
    throw new Error(
      'API adresi belirlenemedi: Constants.expoConfig.hostUri boş. ' +
        'app.json > expo.extra.apiBaseUrlOverride ile elle bir adres tanımla.',
    );
  }

  return `http://${lanHost}:${API_PORT}`;
}

const API_BASE_URL = resolveApiBaseUrl();

async function getJson<T>(path: string): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`);
  if (!response.ok) {
    throw new Error(`${path} -> HTTP ${response.status}`);
  }
  return (await response.json()) as T;
}

export const apiClient = {
  baseUrl: API_BASE_URL,
  getHealth: () => getJson<{ status: string }>('/api/health'),
  getWeeklyBoard: (programDayId: number, workoutWeekId: number) =>
    getJson<WeeklyBoardRowDto[]>(`/api/program-days/${programDayId}/weekly-board?workoutWeekId=${workoutWeekId}`),
};
