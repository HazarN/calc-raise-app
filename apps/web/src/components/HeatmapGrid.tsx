import type { WeeklySpreadCellDto, WeeklySpreadReportDto } from '../api/types';

// System.DayOfWeek ham değerleri (Sunday=0) — Pazartesi ile başlayan görüntüleme sırası.
const DAY_ORDER = [1, 2, 3, 4, 5, 6, 0];
const DAY_LABELS: Record<number, string> = {
  0: 'Pazar',
  1: 'Pazartesi',
  2: 'Salı',
  3: 'Çarşamba',
  4: 'Perşembe',
  5: 'Cuma',
  6: 'Cumartesi',
};

export function HeatmapGrid({ report }: { report: WeeklySpreadReportDto }) {
  const { cells } = report;

  if (cells.length === 0) {
    return <p className="text-gray-500 italic">Bu hafta için henüz planlanmış bir egzersiz yok.</p>;
  }

  const muscleGroupNames = [...new Set(cells.map((c) => c.muscleGroupName))].sort((a, b) => a.localeCompare(b, 'tr'));
  const cellByKey = new Map<string, WeeklySpreadCellDto>(cells.map((c) => [`${c.muscleGroupName}-${c.dayOfWeek}`, c]));
  const maxVolume = Math.max(1, ...cells.map((c) => c.totalVolume));

  return (
    <div className="overflow-x-auto">
      <table className="w-full border-collapse text-sm">
        <thead>
          <tr>
            <th className="px-2 py-2 text-left">Kas Grubu</th>
            {DAY_ORDER.map((day) => (
              <th key={day} className="px-2 py-2 text-center font-semibold">
                {DAY_LABELS[day]}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {muscleGroupNames.map((muscleGroupName) => (
            <tr key={muscleGroupName}>
              <td className="px-2 py-2 font-medium whitespace-nowrap">{muscleGroupName}</td>
              {DAY_ORDER.map((day) => {
                const cell = cellByKey.get(`${muscleGroupName}-${day}`);
                return (
                  <td key={day} className="p-1 align-top">
                    <HeatmapCell cell={cell} maxVolume={maxVolume} />
                  </td>
                );
              })}
            </tr>
          ))}
        </tbody>
      </table>
      <p className="mt-3 text-xs text-gray-500">
        Renk yoğunluğu o günün hacmini gösterir. Kırmızı çerçeve: aynı kas grubu 48 saatten az arayla tekrar
        ediliyor.
      </p>
    </div>
  );
}

function HeatmapCell({ cell, maxVolume }: { cell: WeeklySpreadCellDto | undefined; maxVolume: number }) {
  if (!cell) {
    return <div className="min-w-[110px] rounded border border-gray-100 p-2 text-center text-gray-300">-</div>;
  }

  const intensity = cell.totalVolume > 0 ? Math.max(0.15, cell.totalVolume / maxVolume) : 0;
  const style = { backgroundColor: `rgba(34, 197, 94, ${intensity})` };

  return (
    <div
      className={`min-w-[110px] rounded border p-2 ${cell.insufficientRestWarning ? 'border-2 border-red-500' : 'border-gray-200'}`}
      style={style}
    >
      <ul className="text-xs leading-tight">
        {cell.exerciseNames.map((name) => (
          <li key={name}>{name}</li>
        ))}
      </ul>
      {cell.totalSets > 0 && <p className="mt-1 text-xs font-semibold">{cell.totalSets} set</p>}
    </div>
  );
}
