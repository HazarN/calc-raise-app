import type { WeeklyBoardRowDto } from '../api/types';
import { SetList } from './SetList';

const columns = [
  'Hareket Bölgesi',
  'Egzersiz',
  'Method',
  'Hedef Set/Tekrar',
  'Bu Hafta',
  'Geçen Hafta',
  'All-Time PR',
] as const;

export function WeeklyBoardTable({ rows }: { rows: WeeklyBoardRowDto[] }) {
  if (rows.length === 0) {
    return <p className="text-gray-500 italic">Bu program günü için henüz egzersiz tanımlanmamış.</p>;
  }

  return (
    <table className="w-full border-collapse text-sm">
      <thead>
        <tr className="border-b border-gray-300 text-left">
          {columns.map((column) => (
            <th key={column} className="px-3 py-2 font-semibold">
              {column}
            </th>
          ))}
        </tr>
      </thead>
      <tbody>
        {rows.map((row) => (
          <tr key={row.programExerciseId} className="border-b border-gray-200 align-top">
            <td className="px-3 py-2">{row.primaryMuscleGroupName}</td>
            <td className="px-3 py-2 font-medium">{row.exerciseName}</td>
            <td className="px-3 py-2">{row.methodName ?? '-'}</td>
            <td className="px-3 py-2">
              {row.targetSetsMin}-{row.targetSetsMax} set × {row.targetRepsMin}-{row.targetRepsMax} tekrar
            </td>
            <td className="px-3 py-2">
              <SetList sets={row.currentWeekSets} />
            </td>
            <td className="px-3 py-2 text-gray-500">
              <SetList sets={row.previousWeekSets} />
            </td>
            <td className="px-3 py-2">
              {row.allTimePersonalRecord ? (
                <SetList sets={row.allTimePersonalRecord.sets} />
              ) : (
                <span className="text-gray-400">-</span>
              )}
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
