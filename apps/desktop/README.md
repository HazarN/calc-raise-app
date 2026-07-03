# @calc/desktop

Electron shell — `@calc/web`'i olduğu gibi sarmalar, ayrı bir UI kodu yok.

## Geliştirme

`@calc/web`'in dev server'ı ayrı bir terminalde çalışıyor olmalı (Electron ona bağlanır):

```bash
pnpm --filter @calc/web dev      # terminal 1
pnpm --filter @calc/desktop dev  # terminal 2
```

## Paketleme (imzasız .exe)

```bash
pnpm --filter @calc/desktop dist
```

`web/dist`'i build edip `desktop/web-dist`'e kopyalar, ardından `electron-builder` ile `apps/desktop/release/` altına **imzasız, portable tek bir .exe** üretir (bkz. backlog.md karar #8 — kod imzalama sertifikası yok, Windows "bilinmeyen yayıncı" uyarısı verir ama çalışır).
