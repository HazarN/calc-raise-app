# Calc. — Backlog

> CR numaraları sıralı ve kalıcıdır (silinen/iptal olan bir CR numarası tekrar kullanılmaz, durumu "Cancelled" olarak işaretlenir). Branch adlandırması: `(feature|bugfix|hotfix)_CR-XXXX_(persona)`.

## Karar Günlüğü (01-analiz-ve-degerlendirme_cevabım.txt üzerinden)

| # | Konu | Karar |
|---|---|---|
| 1 | PR metriği | Volume load (set×tekrar×ağırlık) esas alınacak. |
| 2 | RIR / Failure | `WorkoutSetLog`'a opsiyonel `RIR` alanı eklenecek. **v1'de PR/hacim hesaplamasını etkilemeyecek** — sadece bilgi/gösterge amaçlı kaydedilecek. İleride "etkin hacim" (RIR-ağırlıklı hacim) gibi bir v2 metriği için temel oluşturur. *(Bu benim önerdiğim varsayılan — sen "belki dahil edilir" demiştin, kesinleşmemişti; itiraz edersen v1'e de dahil ederiz.)* |
| 3 | Haftalık veri modeli | Onaylandı, UI mantığı değişmeyecek — sadece DB'de normalize log + view. |
| 4 | Method alanı | Onaylandı: tekli, opsiyonel alan. Süperset/dropset gibi zincirleme yapılar **v1'de yok**. |
| 5 | Progressive-overload katsayısı | Genel katsayının yanında **kas grubu bazlı** ayrı katsayılar da (bacak, ön omuz, triceps vb.) hesaplanıp gösterilecek. |
| 6 | Kas katılım verisi | `ExerciseMuscleGroup` ilişkisine Birincil/İkincil tip + yüzde alanı eklenecek. Hareket paterni/ekipman tipi gibi ek alanlar v1'de yok. |
| 7 | Retro tasarım | Kullanıcı ileride ilgili frontend persona'ya referans görsel verecek. Tasarım tokenları o görsel gelene kadar placeholder/nötr tutulacak. |
| 8 | Dağıtım/maliyet | v1 tamamen ücretsiz: Windows'ta imzasız `.exe`, web'de domainsiz, mobilde Expo Go. Gerçek store yayını yok. |
| 9 | Demo kapsamı | 3 arayüz de hedefleniyor, ağırlıklı test Expo Go + iOS. |

| 10 | Backend erişimi (v1) | **Cloud'dan başlanacak:** Neon (Postgres, free tier) + ücretsiz bir API hosting (Azure App Service F1 / Fly.io free allowance arasında CR-0002'de karar verilecek). Expo Go/iPhone dahil her cihazdan Wi-Fi bağımlılığı olmadan erişim sağlanacak. |

---

## Sprint 0 — Temel / Demo İskeleti

| CR | Başlık | Persona | Durum |
|---|---|---|---|
| CR-0001 | Monorepo iskeleti (pnpm workspaces, `apps/web`, `apps/mobile`, `apps/desktop`, `packages/design-tokens`, `packages/api-client`), git init, `.gitignore`, temel README | DevOps | To Do |
| CR-0002 | Backend .NET Core Web API iskeleti — Controller/Service/Repository/UoW katmanları, PostgreSQL bağlantısı, EF Core kurulumu | Backend | To Do |
| CR-0003 | Veritabanı şeması v1 + migration: `MuscleGroup`, `Exercise`, `ExerciseMuscleGroup` (Primary/Secondary + %), `Method`, `ProgramDay`, `ProgramExercise`, `WorkoutSetLog` (RIR dahil) | Backend / DB | To Do |
| CR-0004 | PR/Volume-Load hesaplama servisi + "bu hafta / geçen hafta / all-time PR" sorgu-view'ları | Backend | To Do |
| CR-0005 | Web — ana tablo ekranı (CRUD + PR kolonları) | Frontend (Web) | To Do |
| CR-0006 | Mobile (Expo) — ana tablo ekranının RN versiyonu, Expo Go üzerinde test | Mobile | To Do |
| CR-0007 | Desktop (Electron) shell — web app'i sarmalama | Frontend (Desktop) | To Do |
| CR-0008 | Progressive-overload katsayı servisi (genel + kas grubu bazlı) | Algoritma | To Do |
| CR-0009 | Haftalık yayılım / heat-map görünümü | Frontend (Web) | To Do |

Sprint 0'ın hedefi: tek egzersiz / tek hafta ölçeğinde uçtan uca çalışan bir demo (CR-0001 → CR-0006 minimum uygulanabilir demo, CR-0007/8/9 demo sonrası eklenir).
