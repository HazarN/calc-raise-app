const fs = require('node:fs');
const path = require('node:path');

const source = path.join(__dirname, '..', '..', 'web', 'dist');
const destination = path.join(__dirname, '..', 'web-dist');

if (!fs.existsSync(source)) {
  console.error(`@calc/web henüz build edilmemiş: ${source} bulunamadı. Önce "pnpm --filter @calc/web build" çalıştır.`);
  process.exit(1);
}

fs.rmSync(destination, { recursive: true, force: true });
fs.cpSync(source, destination, { recursive: true });
console.log(`Kopyalandı: ${source} -> ${destination}`);
