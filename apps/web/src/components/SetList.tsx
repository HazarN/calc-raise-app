import type { SetLogDto } from '../api/types';

export function SetList({ sets, emptyLabel = '-' }: { sets: SetLogDto[]; emptyLabel?: string }) {
  if (sets.length === 0) {
    return <span className="text-gray-400">{emptyLabel}</span>;
  }

  return (
    <ul className="space-y-0.5">
      {sets.map((set) => (
        <li key={set.setNumber} className="whitespace-nowrap">
          {set.weightKg}kg × {set.reps}
        </li>
      ))}
    </ul>
  );
}
