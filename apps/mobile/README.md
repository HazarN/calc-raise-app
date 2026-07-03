# @calc/mobile

React Native + Expo istemcisi.

```bash
pnpm --filter @calc/mobile start
```

Terminaldeki QR kodu **Expo Go** uygulamasıyla (iOS) okut. Telefon ve backend'i çalıştıran bilgisayarın **aynı Wi-Fi ağında** olması gerekir — backend hosting kararı henüz verilmedi (bkz. `warehouse/backlog.md` CR-0010), o yüzden şu an tek yol bu.

`src/api/client.ts`, Metro bundler'ın kendi LAN adresinden (Expo'nun `hostUri`'si) otomatik olarak backend adresini türetir — elle bir IP girmen gerekmez. Otomatik bulma işe yaramazsa `app.json` içine şunu ekleyip kendi IP'ni yazabilirsin:

```json
{ "expo": { "extra": { "apiBaseUrlOverride": "http://192.168.1.23:5080" } } }
```

Backend'in (`server/`) ayrı bir terminalde, **http** üzerinden (https değil — telefon self-signed sertifikaya güvenmez) çalışıyor olması gerekir:

```bash
cd server/src/CalcRaise.Api
ASPNETCORE_URLS=http://0.0.0.0:5080 dotnet run --no-launch-profile
```

(`0.0.0.0` önemli — sadece `localhost`'a bağlarsa telefon erişemez.)

`src/api/` içindeki tipler/client, `@calc/web` ile aynı geçici el yazmasıdır; CR-0004'te `@calc/api-client`'a taşınacak.
