import { StyleSheet, Text, View } from 'react-native';
import type { SetLogDto } from '../api/types';

export function SetList({ sets, emptyLabel = '-' }: { sets: SetLogDto[]; emptyLabel?: string }) {
  if (sets.length === 0) {
    return <Text style={styles.empty}>{emptyLabel}</Text>;
  }

  return (
    <View>
      {sets.map((set) => (
        <Text key={set.setNumber} style={styles.setLine}>
          {set.weightKg}kg × {set.reps}
        </Text>
      ))}
    </View>
  );
}

const styles = StyleSheet.create({
  setLine: {
    fontSize: 14,
  },
  empty: {
    fontSize: 14,
    color: '#9ca3af',
  },
});
