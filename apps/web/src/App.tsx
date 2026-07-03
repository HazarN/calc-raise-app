import { useEffect, useState } from 'react';
import { apiClient } from './api/client';
import type { WeeklyBoardRowDto } from './api/types';
import { WeeklyBoardTable } from './components/WeeklyBoardTable';

type ConnectionStatus = 'checking' | 'connected' | 'unreachable';

// v0 demo: tek program günü, tek hafta. Program/hafta seçimi CR-0005'in sonraki adımı.
const DEMO_PROGRAM_DAY_ID = 1;
const DEMO_WORKOUT_WEEK_ID = 1;

function App() {
  const [status, setStatus] = useState<ConnectionStatus>('checking');
  const [rows, setRows] = useState<WeeklyBoardRowDto[]>([]);

  useEffect(() => {
    apiClient
      .getHealth()
      .then(() => setStatus('connected'))
      .catch(() => setStatus('unreachable'));
  }, []);

  useEffect(() => {
    if (status !== 'connected') {
      return;
    }
    apiClient
      .getWeeklyBoard(DEMO_PROGRAM_DAY_ID, DEMO_WORKOUT_WEEK_ID)
      .then(setRows)
      .catch(() => setRows([]));
  }, [status]);

  return (
    <main className="mx-auto max-w-5xl p-6">
      <h1 className="mb-1 text-2xl font-semibold">Calc.</h1>
      <p className="mb-6 text-sm text-gray-500">
        Backend:{' '}
        {status === 'checking' && 'kontrol ediliyor…'}
        {status === 'connected' && <span className="text-green-600">bağlı</span>}
        {status === 'unreachable' && (
          <span className="text-red-600">erişilemiyor (server/ çalışıyor mu? bkz. server/README.md)</span>
        )}
      </p>
      <WeeklyBoardTable rows={rows} />
    </main>
  );
}

export default App;
