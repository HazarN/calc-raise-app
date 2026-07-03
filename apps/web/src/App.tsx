import { useEffect, useState } from 'react';
import { apiClient } from './api/client';
import type { WeeklyBoardRowDto, WeeklySpreadReportDto } from './api/types';
import { WeeklyBoardTable } from './components/WeeklyBoardTable';
import { HeatmapGrid } from './components/HeatmapGrid';

type ConnectionStatus = 'checking' | 'connected' | 'unreachable';
type ViewTab = 'board' | 'spread';

// v0 demo: tek program günü, tek hafta. Program/hafta seçimi CR-0005'in sonraki adımı.
const DEMO_PROGRAM_DAY_ID = 1;
const DEMO_WORKOUT_WEEK_ID = 1;

function App() {
  const [status, setStatus] = useState<ConnectionStatus>('checking');
  const [tab, setTab] = useState<ViewTab>('board');
  const [rows, setRows] = useState<WeeklyBoardRowDto[]>([]);
  const [spread, setSpread] = useState<WeeklySpreadReportDto>({ cells: [] });

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
    apiClient
      .getWeeklySpread(DEMO_WORKOUT_WEEK_ID)
      .then(setSpread)
      .catch(() => setSpread({ cells: [] }));
  }, [status]);

  return (
    <main className="mx-auto max-w-5xl p-6">
      <h1 className="mb-1 text-2xl font-semibold">Calc.</h1>
      <p className="mb-4 text-sm text-gray-500">
        Backend:{' '}
        {status === 'checking' && 'kontrol ediliyor…'}
        {status === 'connected' && <span className="text-green-600">bağlı</span>}
        {status === 'unreachable' && (
          <span className="text-red-600">erişilemiyor (server/ çalışıyor mu? bkz. server/README.md)</span>
        )}
      </p>

      <div className="mb-4 flex gap-2 border-b border-gray-200">
        <TabButton label="Haftalık Tablo" active={tab === 'board'} onClick={() => setTab('board')} />
        <TabButton label="Haftalık Yayılım" active={tab === 'spread'} onClick={() => setTab('spread')} />
      </div>

      {tab === 'board' ? <WeeklyBoardTable rows={rows} /> : <HeatmapGrid report={spread} />}
    </main>
  );
}

function TabButton({ label, active, onClick }: { label: string; active: boolean; onClick: () => void }) {
  return (
    <button
      type="button"
      onClick={onClick}
      className={`px-3 py-2 text-sm font-medium ${
        active ? 'border-b-2 border-gray-900 text-gray-900' : 'text-gray-500'
      }`}
    >
      {label}
    </button>
  );
}

export default App;
