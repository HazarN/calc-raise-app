# Calc. Raise Weightlifting Tracker Program

Gelişmiş bir progressive-overload / haftalık antrenman takip uygulaması.

Planlama ve süreç dokümanları için bkz. [`warehouse/`](./warehouse):
- [`warehouse/startingPrompt.txt`](./warehouse/startingPrompt.txt) — orijinal proje talebi
- [`warehouse/backlog.md`](./warehouse/backlog.md) — CR (change request) backlog'u ve karar günlüğü

Analiz/ihtiyaç dokümanları ve kullanıcı yanıtları `warehouse/chats/` altında tutulur — bazen kimlik bilgisi (connection string vb.) içerebildiğinden bu klasör `.gitignore`'da, repo'ya gitmez.

## Repo Yapısı

```
apps/
  web/        React (web)
  mobile/     React Native + Expo
  desktop/    Electron (web app'i sarmalar)
packages/
  design-tokens/   Ortak renk/spacing/typography tokenları
  api-client/      Backend'den üretilen tip-güvenli API client
server/       ASP.NET Core backend (Domain / Application / Infrastructure / Api)
warehouse/    Planlama, analiz, backlog dokümanları
```

## Branch Modeli

`develop` (aktif geliştirme) → `release-X.X` → `main` (production).
Talep branch'leri: `(feature|bugfix|hotfix)_CR-XXXX_(persona)`.
