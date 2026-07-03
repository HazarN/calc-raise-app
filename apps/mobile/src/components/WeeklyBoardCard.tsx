import { StyleSheet, Text, View } from 'react-native';
import type { WeeklyBoardRowDto } from '../api/types';
import { SetList } from './SetList';

export function WeeklyBoardCard({ row }: { row: WeeklyBoardRowDto }) {
  return (
    <View style={styles.card}>
      <Text style={styles.exerciseName}>{row.exerciseName}</Text>
      <Text style={styles.subtitle}>
        {row.primaryMuscleGroupName}
        {row.methodName ? ` · ${row.methodName}` : ''}
      </Text>
      <Text style={styles.target}>
        Hedef: {row.targetSetsMin}-{row.targetSetsMax} set × {row.targetRepsMin}-{row.targetRepsMax} tekrar
      </Text>

      <View style={styles.columns}>
        <View style={styles.column}>
          <Text style={styles.columnLabel}>Bu Hafta</Text>
          <SetList sets={row.currentWeekSets} />
        </View>
        <View style={styles.column}>
          <Text style={styles.columnLabel}>Geçen Hafta</Text>
          <SetList sets={row.previousWeekSets} />
        </View>
        <View style={styles.column}>
          <Text style={styles.columnLabel}>All-Time PR</Text>
          {row.allTimePersonalRecord ? (
            <SetList sets={row.allTimePersonalRecord.sets} />
          ) : (
            <Text style={styles.empty}>-</Text>
          )}
        </View>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  card: {
    borderWidth: 1,
    borderColor: '#e5e7eb',
    borderRadius: 8,
    padding: 12,
    marginBottom: 10,
  },
  exerciseName: {
    fontSize: 16,
    fontWeight: '600',
  },
  subtitle: {
    fontSize: 13,
    color: '#6b7280',
    marginTop: 2,
  },
  target: {
    fontSize: 13,
    color: '#374151',
    marginTop: 6,
  },
  columns: {
    flexDirection: 'row',
    marginTop: 10,
    gap: 12,
  },
  column: {
    flex: 1,
  },
  columnLabel: {
    fontSize: 11,
    fontWeight: '700',
    color: '#9ca3af',
    textTransform: 'uppercase',
    marginBottom: 4,
  },
  empty: {
    fontSize: 14,
    color: '#9ca3af',
  },
});
