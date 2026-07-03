# Calc. — Backend (server/)

ASP.NET Core 8 Web API. Katmanlar: `CalcRaise.Domain` (entity'ler) → `CalcRaise.Application` (arayüzler, DTO'lar, PR/hacim hesaplama ve haftalık tablo servisleri) → `CalcRaise.Infrastructure` (EF Core + Npgsql, repository/UoW implementasyonları) → `CalcRaise.Api` (controller'lar, DI wiring).

## Bağlantı Dizesi Kurulumu (Neon)

Gerçek bağlantı dizesi **hiçbir zaman appsettings.json'a yazılmaz** (repo'ya sızmasın diye). Yerelde `dotnet user-secrets` kullan:

```bash
cd src/CalcRaise.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:CalcRaise" "Host=...neon.tech;Database=calcraise;Username=...;Password=...;SslMode=Require"
```

Diğer ortamlarda (CI/CD, hosting) `ConnectionStrings__CalcRaise` environment variable'ı ile aynı değer set edilir.

## Migration'ları Uygulama

Migration dosyaları, gerçek bir DB bağlantısı olmadan `CalcRaiseDbContextFactory` (design-time factory) üzerinden üretilir. Gerçek Neon veritabanına uygulamak için:

```bash
dotnet tool install --global dotnet-ef   # ilk seferde
cd server
dotnet ef database update --project src/CalcRaise.Infrastructure --startup-project src/CalcRaise.Api
```

## Çalıştırma

```bash
cd server/src/CalcRaise.Api
dotnet run
```

Swagger UI: `https://localhost:<port>/swagger` (yalnızca Development ortamında açık).

## Şema Özeti (CR-0003)

`MuscleGroup`, `Method`, `Exercise`, `ExerciseMuscleGroup` (Primary/Secondary + yüzde), `ProgramDay`, `ProgramExercise` (hedef set/tekrar aralığı), `WorkoutWeek`, `WorkoutSetLog` (RIR dahil, immutable log). "Bu hafta / geçen hafta / all-time PR" ayrı fiziksel kolonlar değil — hepsi `WorkoutSetLog` üzerinden `WeeklyBoardService` tarafından sorgulanıp hesaplanır (bkz. `warehouse/01-analiz-ve-degerlendirme.md` §3).
