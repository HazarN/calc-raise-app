# @calc/web

React (Vite + TS + Tailwind) web istemcisi.

```bash
cp .env.example .env      # ilk seferde, gerekirse VITE_API_BASE_URL'i değiştir
pnpm --filter @calc/web dev
```

Backend'in (`server/`) ayrı bir terminalde `dotnet run` ile ayakta olması gerekir — bkz. `server/README.md`.

`src/api/` içindeki tipler ve client geçici el yazmasıdır; CR-0004'te backend Swagger şemasından `@calc/api-client`'a otomatik üretime geçilecek.
