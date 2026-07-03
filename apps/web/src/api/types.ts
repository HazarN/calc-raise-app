// Geçici el yazması tipler. CR-0004 kapsamında backend Swagger şemasından
// @calc/api-client içine NSwag ile otomatik üretilecek, bu dosya kalkacak.

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
