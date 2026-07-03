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
| 7 | Retro tasarım | Referans görsel geldi (`warehouse/retro-design.jfif`): kalın siyah (2-3px) çerçeveli kartlar, düz (gradyansız) pastel renk bloklu diyagonal arka plan (gök mavisi / şeftali / nane yeşili), krem kart zemini, kalın mono/geometrik başlık tipografisi + sade sans body, "stat rozeti" kutuları (büyük sayı + küçük etiket), hap (pill) şeklinde arama/buton. Neo-brutalist/Y2K-pop tarzı — 90'lar Windows'tan çok "kalın çerçeveli flat-pastel dashboard" stiline yakın. `@calc/design-tokens` bu paletle doldurulacak (CR-0005 devamı). |
| 8 | Dağıtım/maliyet | v1 tamamen ücretsiz: Windows'ta imzasız `.exe`, web'de domainsiz, mobilde Expo Go. Gerçek store yayını yok. |
| 9 | Demo kapsamı | 3 arayüz de hedefleniyor, ağırlıklı test Expo Go + iOS. |
| 10 | Backend erişimi (v1) | Neon (Postgres, free tier) **canlıda** — bkz. CR-0003 notu. API hosting (backend'in deploy edileceği yer) **karar bekliyor / duraklatıldı**: Fly.io/Koyeb/Railway elendi (bkz. not aşağıda), Azure App Service F1 denendi ve kullanıcı vazgeçti (hesap/kota sürtünmesi — France Central'da kota bulundu ama Web App oluşturma adımında durduruldu, oluşturulan kaynaklar silindi). Şimdilik backend yerel makinede (`dotnet run`) çalıştırılıp test ediliyor; gerçek hosting kararı sonraya bırakıldı. |

---

## Sprint 0 — Temel / Demo İskeleti

| CR | Başlık | Persona | Durum |
|---|---|---|---|
| CR-0001 | Monorepo iskeleti (pnpm workspaces, `apps/web`, `apps/mobile`, `apps/desktop`, `packages/design-tokens`, `packages/api-client`), git init, `.gitignore`, temel README | DevOps | **Done** |
| CR-0002 | Backend .NET Core Web API iskeleti — Controller/Service/Repository/UoW katmanları, PostgreSQL bağlantısı, EF Core kurulumu | Backend | **Done** |
| CR-0003 | Veritabanı şeması v1 + migration: `MuscleGroup`, `Exercise`, `ExerciseMuscleGroup` (Primary/Secondary + %), `Method`, `ProgramDay`, `ProgramExercise`, `WorkoutWeek`, `WorkoutSetLog` (RIR dahil) | Backend / DB | **Done** |
| CR-0004 | PR/Volume-Load hesaplama servisi + "bu hafta / geçen hafta / all-time PR" sorgu-view'ları | Backend | **Done** |
| CR-0005 | Web — Vite/React/Tailwind iskeleti + haftalık tablo demo ekranı | Frontend (Web) | **Done (v0 demo)** — bkz. not aşağıda |
| CR-0006 | Mobile (Expo) — ana tablo ekranının RN versiyonu, Expo Go üzerinde test | Mobile | To Do |
| CR-0007 | Desktop (Electron) shell — web app'i sarmalama | Frontend (Desktop) | To Do |
| CR-0008 | Progressive-overload katsayı servisi (genel + kas grubu bazlı) | Algoritma | To Do |
| CR-0009 | Haftalık yayılım / heat-map görünümü | Frontend (Web) | To Do |
| CR-0010 | Backend deployment/hosting kararı ve kurulumu | DevOps | **Paused** — bkz. karar günlüğü #10 |

**Not (CR-0002/0003/0004):** Üçü de aynı `feature_CR-0002_backend` branch'inde tek commit olarak `develop`'a merge edildi — iskelet, şema ve ilk gerçek özellik (PR hesaplama) birbirinden ayrıştırılamayacak kadar sıkı bağlıydı. `dotnet build` 0 hata ile geçti; migration (`InitialCreate`) tasarım-zamanı factory ile üretildi, henüz gerçek bir Neon veritabanına uygulanmadı (bkz. ihtiyaçlar dosyası).

**Not (Node.js/pnpm):** Bu makinede Node.js/npm/pnpm hiç kurulu değildi, CR-0005/0006/0007/0009'u bloke ediyordu. winget ile Node.js LTS + `npm install -g pnpm` kurularak çözüldü — artık bloke değil.

**Not (CR-0005):** `apps/web` gerçek bir Vite+React+TS+Tailwind projesi; `pnpm --filter @calc/web build` ve `dev` başarıyla çalışıyor, dev server'ın servis ettiği HTML shell doğrulandı. Ancak backend'e gerçek bir Neon bağlantı dizesi tanımlı olmadığı için (`Program.cs` bağlantı dizesi yoksa kasıtlı olarak fail-fast yapıyor) **tam uçtan-uca akış (API'den veri çekme) henüz canlı test edilemedi** — bkz. `warehouse/02-ihtiyaclar-ve-bloklar.md`. API tipleri/client'ı şimdilik el yazması (`apps/web/src/api/`); CR-0004 kapsamında NSwag ile `@calc/api-client`'a taşınacak.

Sprint 0'ın hedefi: tek egzersiz / tek hafta ölçeğinde uçtan uca çalışan bir demo (CR-0001 → CR-0006 minimum uygulanabilir demo, CR-0007/8/9 demo sonrası eklenir).
