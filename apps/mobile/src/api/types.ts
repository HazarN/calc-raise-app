// Geçici el yazması tipler — @calc/web/src/api/types.ts ile aynı şekil.
// CR-0004 kapsamında backend Swagger şemasından @calc/api-client'a taşınacak.

export interface SetLogDto {
  setNumber: number;
  reps: number;
  weightKg: number;
}

export interface WeeklyExerciseRecordDto {
  workoutWeekId: number;
  totalVolume: number;
  sets: SetLogDto[];
}

export interface WeeklyBoardRowDto {
  programExerciseId: number;
  exerciseName: string;
  primaryMuscleGroupName: string;
  methodName: string | null;
  targetSetsMin: number;
  targetSetsMax: number;
  targetRepsMin: number;
  targetRepsMax: number;
  currentWeekSets: SetLogDto[];
  previousWeekSets: SetLogDto[];
  allTimePersonalRecord: WeeklyExerciseRecordDto | null;
}
