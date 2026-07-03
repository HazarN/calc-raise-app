import { useEffect, useState } from 'react';
import { StatusBar } from 'expo-status-bar';
import { FlatList, SafeAreaView, StyleSheet, Text, View } from 'react-native';
import { apiClient } from './src/api/client';
import type { WeeklyBoardRowDto } from './src/api/types';
import { WeeklyBoardCard } from './src/components/WeeklyBoardCard';

type ConnectionStatus = 'checking' | 'connected' | 'unreachable';

// v0 demo: tek program günü, tek hafta. Program/hafta seçimi CR-0006'nın sonraki adımı.
const DEMO_PROGRAM_DAY_ID = 1;
const DEMO_WORKOUT_WEEK_ID = 1;

export default function App() {
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
    <SafeAreaView style={styles.container}>
      <Text style={styles.title}>Calc.</Text>
      <Text style={styles.status}>
        Backend ({apiClient.baseUrl}):{' '}
        {status === 'checking' && 'kontrol ediliyor…'}
        {status === 'connected' && <Text style={styles.connected}>bağlı</Text>}
        {status === 'unreachable' && (
          <Text style={styles.unreachable}>erişilemiyor (telefon ve backend aynı Wi-Fi'da mı?)</Text>
        )}
      </Text>

      {rows.length === 0 ? (
        <Text style={styles.empty}>Bu program günü için henüz egzersiz tanımlanmamış.</Text>
      ) : (
        <FlatList
          data={rows}
          keyExtractor={(row) => String(row.programExerciseId)}
          renderItem={({ item }) => <WeeklyBoardCard row={item} />}
          contentContainerStyle={styles.list}
        />
      )}
      <StatusBar style="auto" />
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    paddingHorizontal: 16,
    paddingTop: 12,
  },
  title: {
    fontSize: 24,
    fontWeight: '600',
  },
  status: {
    fontSize: 13,
    color: '#6b7280',
    marginTop: 4,
    marginBottom: 12,
  },
  connected: {
    color: '#16a34a',
  },
  unreachable: {
    color: '#dc2626',
  },
  empty: {
    fontSize: 14,
    color: '#6b7280',
    fontStyle: 'italic',
  },
  list: {
    paddingBottom: 24,
  },
});
