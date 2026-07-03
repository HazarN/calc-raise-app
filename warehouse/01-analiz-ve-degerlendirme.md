# Calc. — Analiz ve Değerlendirme Notları

> `startingPrompt.txt` dosyasını okudum. Bu doküman, senin isteğin üzerine yaptığım derin analizin, araştırma bulgularının, itiraz ettiğim/netleştirmek istediğim noktaların ve önerdiğim ilerleyiş planının tamamı. Kod yazmadım — hâlâ planlama aşamasındayız.

---

## 1. Genel Değerlendirme

Fikir bütün olarak çok sağlam ve mühendislik açısından "yapılabilir" bir proje. Tek gerçek risk alanları: (a) PR/progressive-overload matematiğindeki convention'ın bilimsel doğruluğu, (b) "cebimden hiç para çıkmasın" hedefinin üç ayrı frontend + gerçek bir backend ile birlikte ne kadar gerçekçi olduğu, (c) haftalık veri modelinin (bu hafta / geçen hafta / all-time PR) aslında göründüğünden bir parça daha karmaşık olması. Aşağıda hepsini tek tek açıyorum.

---

## 2. PR (Personal Record) Matematiği — Araştırma Sonucu Geri Bildirim

Senin önerdiğin convention:
1. Set sayısı > tekrar sayısından daha önemli bir PR metriği (2×6 > 1×12)
2. Aynı set/tekrar'da ağırlık daha yüksekse o kazanır (60kg 2×6 > 65kg 1×8 — dikkat: burada set sayısı da farklı, aşağıda ayrıca değineceğim)

**Araştırma ne diyor:** Egzersiz bilimi literatüründe (RP Strength, Stronger by Science, vb.) PR'lar tek bir "set > tekrar > ağırlık" hiyerarşisiyle değil, çoğunlukla **volume load / tonnage** (`set × tekrar × ağırlık`) ya da **tahmini 1RM** (Epley/Brzycki gibi formüllerle) üzerinden karşılaştırılır. Popüler antrenman takip uygulamaları (Hevy, Strong) da tek bir "en iyi" değeri zorlamak yerine **birden fazla paralel PR türü** tutar: Max Ağırlık, Max Tekrar, Max Hacim (bir seansta), Tahmini 1RM.

Senin verdiğin örneği bu formülle test edelim:
- 60kg, 2×6 → hacim = 60×2×6 = **720**
- 65kg, 1×8 → hacim = 65×1×8 = **520**

Bu örnekte senin sezgin (2×6'nın daha büyük PR olması) volume-load ile de örtüşüyor — yani sezgin rastgele değil, hacim mantığına denk düşüyor. **Ama** katı bir "önce set sayısına bak" kuralı bazı durumlarda hacimle çelişir. Örnek:
- 60kg, 3×3 → hacim = 540
- 60kg, 1×10 → hacim = 600

Burada senin kuralına göre 3×3 (daha fazla set) kazanır, ama gerçek mekanik iş (hacim) açısından 1×10 daha büyüktür. Yani "set sayısı her zaman üstündür" kuralı bazı gerçek senaryolarda progressive-overload'ın asıl ölçütü olan toplam hacimle ters düşebilir.

**Önerim:** Tek bir keyfi lexicographic sıralama (set > tekrar > ağırlık) yerine, **birincil karşılaştırma metriği olarak Volume Load (tonaj)** kullanmak. Bu hem matematiksel olarak tutarlı hem de senin verdiğin örnekle uyumlu. Eşitlik durumunda (aynı tonaj) ikincil kriter olarak "daha az sette / daha yüksek ağırlıkla yapılmış olması" tercih edilebilir (çünkü nispeten daha yüksek yoğunluk = strength-odaklı ilerleme sinyali). Ayrıca ileride egzersiz bazında "hedef" (hipertrofi vs. kuvvet) tanımlarsak, PR karşılaştırma metriğini o hedefe göre ağırlıklandırabiliriz (kuvvet hedefli hareketlerde tahmini 1RM'yi öne çıkarmak gibi). Şimdilik MVP için **tek metrik: volume load** öneriyorum — basit, açıklanabilir, "minimum bakım maliyeti" felsefene de uygun.

Bu bir düzeltme değil, bir **iyileştirme önerisi** — istersen orijinal kuralınla da ilerleyebiliriz, ama az önceki 3×3 vs 1×10 örneği gibi kenar durumlarda garip sonuçlar üretebileceğini bilerek karar vermeni isterim.

**Kaynaklar:**
- [Stronger by Science — The New Approach to Training Volume](https://www.strongerbyscience.com/the-new-approach-to-training-volume/)
- [SPC Performance Lab — Quantifying Training Volume](https://www.spcperformancelab.com.au/strength-training-advice/methods-of-quantifying-training-volume-for-muscle-hypertrophy/)
- [Training Volume & Workout Volume Calculator](https://traincalc.com/calculators/workout-volume)

---

## 3. Veri Modeli — Fark Edilmesi Gereken Bir Kavramsal İnce Nokta

Prompt'ta şu satır dikkatimi çekti: *"...bu haftanın ve geçen haftanın verilerini kontrol ederek en yüksek loadun bulunduğu kolonu yazacak all-time PR kolonu..."*

Burada iki farklı kavram birbirine karışıyor gibi görünüyor:
- **"Geçen hafta" kolonu** → literal olarak bir önceki haftanın ham verisi (read-only, haftalık kaydırılan bir pencere).
- **"All-time PR" kolonu** → tanımı gereği *tüm geçmiş* üzerinden en yüksek değeri temsil etmeli, sadece son 2 haftayı değil.

Eğer "all-time PR" her hafta yalnızca "bu hafta vs. geçen hafta" karşılaştırmasıyla güncelleniyorsa, matematiksel olarak bu **doğru çalışır** (running max: `PR_bu_hafta = max(PR_önceki, bu_haftaki_veri)`), **ama ancak** "PR_önceki" değeri her zaman doğru şekilde taşınırsa. Yani veri modelinde 3 ayrı kavramsal alan olmalı:

| Alan | Davranış | Kaynak |
|---|---|---|
| Bu hafta girilen set/tekrar | Editable | Kullanıcı girişi |
| Geçen hafta | Read-only, literal önceki hafta snapshot'ı | Bir önceki haftanın "bu hafta" verisi |
| All-time PR | Read-only, running max | `max(önceki All-time PR, bu haftaki veri)` |

Bunu tek bir tabloda "üç görünür kolon" olarak tutmak UI açısından doğru, ama **veritabanında fiziksel olarak 3 ayrı sütun tutmak yerine**, ham geçmiş verinin (her hafta her egzersiz için girilen set/tekrar/ağırlık logu) tek bir normalize tabloda saklanmasını, "geçen hafta" ve "all-time PR"ın bu log üzerinden **sorgu/view** ile hesaplanmasını öneririm. Bu hem normalizasyon kurallarına uyar (senin de istediğin gibi) hem de ileride "3 hafta önce ne yapmıştım" gibi soruları bedavaya çözer — çünkü veri zaten kayıp olmadan duruyor.

---

## 4. Heat-map / Haftalık Yayılım Görünümü

Fikir güzel ve teknik olarak basit (bir renk-yoğunluk matrisi). Küçük bir öneri: satırları "kas grubu", sütunları "haftanın günü" yapıp, hücre rengini o gün o kas grubuna düşen **toplam hacim**le boyamak; ayrıca aynı kas grubu 48 saatten az arayla tekrar ediyorsa hücreyi ayrı bir uyarı rengiyle (örn. kırmızı kenarlık) işaretlemek — böylece "yeterince dinlendi mi" sorusuna görsel olarak direkt cevap verir, sadece egzersiz isimlerini yazmaktan daha fazla bilgi taşır. Set sayılarını da hücrede küçük bir sayı olarak göstermek (senin de dediğin gibi) hacim kontrolü için iyi bir ek.

---

## 5. Progressive Overload Katsayı Algoritması — İlk Öneri

Kas grubu gelişim hızının farklı olduğu doğru (MEV/MAV/MRV — Minimum/Maximum Adaptive/Maximum Recoverable Volume literatürü bunu destekliyor: büyük kas grupları [sırt, bacak] küçüklere [biceps, triceps] göre daha yüksek hacim kaldırabiliyor ve farklı SRA — Stimulus/Recovery/Adaptation — eğrilerine sahip). MVP için basit ve açıklanabilir bir başlangıç öneriyorum:

- Her kas grubu için haftalık toplam hacmi (o gruba bağlı tüm egzersizlerin volume load toplamı) hesapla.
- `İlerleme Katsayısı (%) = (bu_hafta_hacim - son_N_hafta_ortalama_hacim) / son_N_hafta_ortalama_hacim × 100`
- Ardışık kaç haftadır pozitif ilerleme olduğunu bir "streak" sayacıyla göster.
- İleride: MEV/MAV/MRV bantlarını kullanıcının kendi geçmişinden öğrenerek (ör. performans platoya girdiğinde otomatik "deload öner") daha akıllı hale getirilebilir; bunun için opsiyonel bir "zorluk/yorgunluk hissi" (basit 1-5 RPE benzeri) alanı eklemek büyük fayda sağlar — ama bu bir v2 fikri, MVP'yi şişirmesin.

Bu algoritmayı ayrı bir "agent" olarak yönetme fikrin doğru — ilerleyen sprintlerde bunu bağımsız evrilebilir bir modül olarak tasarlayacağım.

**Kaynaklar:**
- [RP Strength — Training Volume Landmarks](https://rpstrength.com/blogs/articles/training-volume-landmarks-muscle-growth)
- [MaxFit — MEV, MAV, MRV Guide](https://maxfit.ee/en/blog/training-volume-landmarks)

---

## 6. Tech Stack Fizibilitesi

### 6.1. "3 Frontend" Aslında Kısmen 2 Codebase

Electron, React web uygulamasını neredeyse aynen sarmalayabilir (aynı component'ler, aynı CSS). Yani gerçek ayrım **(Web + Desktop paylaşımlı bir React codebase)** ve **(Mobil — React Native/Expo, tamamen farklı render katmanı)** şeklinde ikiye iner. Bu, "3 ayrı tasarım kalıbı" yükünü ciddi şekilde azaltır.

Öneri: Monorepo (pnpm/Turborepo) yapısı:
- `apps/web` — React
- `apps/desktop` — ince bir Electron shell, `apps/web`'i içeri alır
- `apps/mobile` — React Native + Expo
- `packages/design-tokens` — ortak renk paleti, spacing, typography (JSON/TS olarak, hem CSS hem RN tarafında tüketilebilir)
- `packages/api-client` — backend'den (Swagger/OpenAPI) otomatik üretilen tip-güvenli API client — 3 yerde ayrı ayrı fetch kodu yazmayı engeller (DRY)

Web ve mobilde ortak "utility-first" yaklaşım için Tailwind CSS (web/desktop) + NativeWind (React Native'de Tailwind class syntax) kombinasyonu, tasarım dilini pratikte gerçekten ortaklaştırır.

### 6.2. "Retro Tasarım" — Netleştirme Gerekiyor

"Retro" geniş bir kavram. Somut kütüphane seçimi buna bağlı:
- 90'lar Windows/masaüstü estetiği → `98.css` / `XP.css`
- 8-bit/16-bit oyun konsolu estetiği → `NES.css`
- 70-80'ler poster/varsayılan spor salonu (kalın tipografi, sıcak turuncu/kahve tonları) estetiği → Tailwind üzerine özel bir tema, hazır kütüphane yerine kendi tasarım tokenlarımızı yazmak

Hangisine daha yakın bir görsel referans aklında var, birkaç örnek/moodboard paylaşabilirsen tasarım tokenlarını ona göre kurarım.

### 6.3. "Cebimden Hiç Para Çıkmasın" — Gerçekçilik Kontrolü

Bu hedef **kişisel/tek-kullanıcılı bir uygulama** için gerçekçi, ama bazı sınırlarla:

| Katman | Ücretsiz Seçenek | Sınırlama |
|---|---|---|
| Web hosting | Vercel / Cloudflare Pages / Netlify | Statik SPA için pratikte sınırsız, tek kullanıcı için sorun yok |
| Backend hosting (.NET) | Azure App Service (F1 Free) veya Fly.io free allowance | Soğuk başlangıç (cold start), CPU/RAM kotası düşük — tek kullanıcı için yeterli |
| Veritabanı | Neon (Postgres) free tier | 100 CU-saat/ay, 0.5 GB depolama, proje bazlı — tek kullanıcılık bir antrenman logu için fazlasıyla yeterli |
| Desktop dağıtım | Electron, doğrudan `.exe`/`.dmg` paylaşımı | Kod imzalama (code signing) sertifikası ücretli; imzasız paket Windows/Mac'te "bilinmeyen yayıncı" uyarısı verir ama çalışır — sorun değilse maliyet $0 |
| Mobil dağıtım | Expo Go / EAS internal build | Uygulamayı App Store/Play Store'a **yayınlamak** istersen Apple Developer $99/yıl + Google Play $25 (tek seferlik) kaçınılmaz. Sadece kendi telefonuna kurmak istiyorsan (sideload/Expo Go) $0 |
| CI/CD | GitHub Actions free tier | Public/az kullanımlı private repo için yeterli |
| DDoS/CDN koruma | Cloudflare (ücretsiz proxy) | Bonus: hem güvenlik hem $0 |

**Sonuç:** Tek kullanıcılı, mağazalara yayınlanmayan bir "kişisel takip aracı" olarak kalırsa gerçekten $0 maliyetle sürdürülebilir. Eğer ileride telefonun app store'una gerçek anlamda yayınlamak ya da başka kullanıcılara açmak istersen, o noktada mağaza ücretleri ve muhtemelen daha yüksek DB/compute kotası gibi maliyetler devreye girer. **Açık soru:** Bu proje sadece senin kişisel kullanımın için mi, yoksa ileride başkalarının da kullanabileceği bir ürün mü olacak? Bu, auth/multi-tenancy karmaşıklığını baştan tasarlayıp tasarlamayacağımızı belirler.

---

## 7. Backend Mimarisi ve Güvenlik

- **Katmanlama:** Controller → Service → Repository + Unit of Work, EF Core ile. MediatR/CQRS gibi ek soyutlamalar bu ölçekte muhtemelen erken optimizasyon olur — "minimum bakım maliyeti" felsefene göre başta düz katmanlı ilerleyip ihtiyaç çıkarsa evriltmeyi öneririm.
- **SQL Injection:** EF Core parametreli sorgular kullandığı sürece doğal olarak korunuyoruz; tek dikkat noktası `FromSqlRaw`/`ExecuteSqlRaw` içinde string interpolation kullanmamak (her zaman parametre placeholder'ı).
- **DoS/DDoS:** Gerçek bir DDoS'u tek başına backend kodu durduramaz — bunun gerçekçi ve **ücretsiz** çözümü Cloudflare'i proxy olarak önüne koymak (rate limiting + temel DDoS mitigasyonu dahil, $0). Ayrıca ASP.NET Core'un yerleşik `RateLimiter` middleware'i ile endpoint bazlı sınırlama ekleyeceğiz.
- **Auth:** Tek kullanıcılıysa bile en azından basit bir JWT tabanlı giriş öneririm (API'nin halka açık kalmaması için).

---

## 8. Git-Flow Değerlendirmesi

Önerdiğin `develop` + `release-X.X` + `(type)_CR-XXXX_(agent)` branch modeli mantıklı ve senin ölçeğin için hafif/yeterli. Küçük eklemeler öneriyorum:
- `main`/`production` branch'i en tepede tutmak (release'lerin merge edildiği, her zaman deploy-edilebilir hâl).
- Branch tipi listesine `chore` (bağımlılık güncelleme, konfig, CI değişikliği gibi kod-olmayan işler için) eklemek isteyebilirsin — zorunlu değil.
- CR numaralarını ve durumlarını tutacağım `warehouse/backlog.md` dosyasını, ilk somut taleplerimiz netleşince (bkz. §10) oluşturacağım.

---

## 9. Agile Süreç — Bundan Sonra Nasıl Çalışacağım

- Taleplerini `CR-XXXX` numarasıyla `warehouse/backlog.md`'ye ekleyeceğim (To Do / In Progress / Done sütunlarıyla).
- Görev tipine göre kendimi bir "persona" olarak konumlandıracağım (ör. Frontend Web/Desktop, Mobile, Backend, DB/Perf, Progressive-Overload Algoritma) ve branch isimlerinde bu persona adı geçecek. Bağımsız ve paralelleştirilebilir işlerde (ör. aynı anda hem mobil hem backend işi varsa) gerçekten paralel alt-agent'lar (Agent tool) kullanarak ilerleyeceğim.
- Hız konusunda: önce küçük, uçtan uca çalışan bir **demo** (tek egzersiz, tek hafta, temel tablo + PR hesaplama) ile başlamayı, sonra heat-map ve progressive-overload katsayısı gibi katmanları üstüne eklemeyi öneriyorum — büyük patlama yerine artımlı teslim.

---

## 10. Açık Sorular (Cevaplarına Göre Backlog'u Şekillendireceğim)

1. **PR metriği:** Kendi "set > tekrar > ağırlık" kuralınla mı devam edelim, yoksa §2'de önerdiğim "volume load (tonaj)" tabanlı yaklaşımı mı tercih edersin?
2. **Kapsam:** Bu uygulama sadece senin kişisel kullanımın için mi, yoksa ileride başka kullanıcılara da açılabilir mi? (Auth/multi-tenancy tasarımını etkiler.)
3. **"Retro" tasarım referansı:** Hangi döneme/estetiğe yakın? (90'lar masaüstü / 8-bit oyun konsolu / vintage spor salonu posteri / başka?)
4. **Method (uygulanan metod) alanı:** Drop-set, rest-pause, myo-reps gibi sabit bir liste mi olacak, yoksa serbest metin mi?
5. **Hareket bölgesi genişletmesi:** "Daha bilimsel hesaplamalar" derken, birincil/ikincil kas katılımı yüzdesi, hareket paterni (itme/çekme/squat/hinge), ekipman tipi gibi alanları şimdiden mi ekleyelim, yoksa v2'ye mi bırakalım?
6. **İlk demo kapsamı:** Demo'yu sadece web'de mi göreyim, yoksa baştan üç platformu da mı hedefleyelim?

---

## 11. Önerilen Sonraki Adım

Yukarıdaki açık sorulara cevap verdiğinde, `warehouse/backlog.md` dosyasını ilk somut CR'larla (muhtemelen: proje iskeleti/monorepo kurulumu, veritabanı şeması v1, temel tablo ekranı demo'su) oluşturup Agile döngüsüne başlayacağım.
