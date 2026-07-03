# Calc. — İhtiyaçlar ve Açık Bloklar

> Devam edebilmem için senden gereken adımlar ve şu an bilinen sınırlamalar. Elimden geleni kendim hallettim (aşağıda "Kendim Çözdüm" bölümü), geri kalanlar senin hesap/karar gerektiren adımların.

## Senden Gereken Adımlar

### 1. GitHub deposu (öncelik: orta)
Bu makinede `gh` CLI kurulu/yetkilendirilmiş değil, yani senin adına bir GitHub deposu oluşturup push edemiyorum. İki seçenek:
- **A)** GitHub'da boş bir repo oluştur (README/gitignore eklemeden) ve bana adını/URL'sini ver, ben `git remote add origin ...` ile bağlayıp `git push` ederim.
- **B)** `gh` CLI'yi kurup `gh auth login` ile giriş yaparsan, repo oluşturma ve push işini tamamen ben yaparım.

Bu olmadan da yerelde geliştirmeye devam edebiliyorum (git repo zaten yerelde `develop`/`main` branch'leriyle çalışıyor), sadece uzak yedekleme/paylaşım yok.

### 2. Neon (Postgres) hesabı + bağlantı dizesi (öncelik: yüksek)
Backend'i (`server/`) gerçekten çalıştırıp uçtan uca test edebilmem için bir Neon bağlantı dizesine ihtiyacım var — şu an migration'lar üretildi (`InitialCreate`) ama hiçbir gerçek veritabanına uygulanmadı, `dotnet run` bağlantı dizesi olmadan **kasıtlı olarak** başlamıyor (yanlışlıkla boş/yanlış bir DB'ye yazmayı önlemek için).

Adımlar:
1. [neon.tech](https://neon.tech) üzerinde ücretsiz bir proje oluştur (bkz. karar günlüğü #10 — cloud'dan başlama kararı).
2. Bağlantı dizesini (`postgresql://...`) bana ilet — ben bunu senin adına `dotnet user-secrets` ile yerel makineye (repo'ya değil) kaydedip migration'ı uygularım.

### 3. Backend hosting kararı ve hesabı (öncelik: orta, Neon'dan sonra)
Expo Go/iPhone'dan backend'e her yerden erişim için bir cloud hosting gerekiyor (karar günlüğü #10). Azure App Service (F1 free) ile Fly.io free allowance arasında bir tercih yapman gerekecek — ikisi de $0, farkları:
- **Azure F1:** Microsoft hesabı yeterli, .NET ile "native" entegrasyon, ama F1 tier'ın soğuk başlangıç/CPU kotası biraz daha kısıtlı.
- **Fly.io:** Docker imajı gerektirir (zaten yapmamız gereken bir şey), free allowance biraz daha esnek.

Bu bir sonraki backend CR'ında (deploy) netleştirilecek — şimdilik acil değil, yerel geliştirme sürüyor.

### 4. "Retro" tasarım referansı (öncelik: düşük, CR-0005 devamı için gerekli)
Karar günlüğü #7'de not düşülmüştü: web arayüzünün gerçek görsel kimliği için bir ekran görüntüsü/moodboard bekliyorum. Gelene kadar `apps/web` nötr (Tailwind default) bir görünümde kalacak.

---

## Kendim Çözdüm

- **Node.js/npm/pnpm bu makinede hiç kurulu değildi** — winget ile Node.js LTS (v24.18.0) kurdum ve `npm install -g pnpm` ile pnpm 9.12.0'ı ekledim. Bu, CR-0005/0006/0007/0009'u bloke ediyordu, artık bloke değil.
- `dotnet-ef` global aracını kurdum, migration'ı gerçek bir DB olmadan (design-time factory ile) ürettim.

## Bilinen Sınırlama

Bu oturumun terminal araçları her komutta PATH'i Windows registry'sinden taze okumuyor gibi görünüyor (yeni kurulan Node.js/pnpm bazen "command not found" verebiliyor) — bunu her seferinde PATH'i elle tazeleyerek aşıyorum. Kendi terminalinde/IDE'nde bu sorun olmaz, PATH kalıcı olarak (Machine+User seviyesinde) güncellendi.
