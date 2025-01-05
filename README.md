# Process Monitor - Gerçek Zamanlı İşlem İzleme Sistemi

Bu proje, Windows işletim sisteminde çalışan işlemleri (processes) gerçek zamanlı olarak izleyip, WebSocket teknolojisi üzerinden başka bir bilgisayara aktaran bir sistemdir.

## Teknolojiler

- **C# (.NET 8)**: Windows işlemlerini izlemek için
- **Node.js**: WebSocket sunucusu için
- **WebSocket**: Gerçek zamanlı veri iletimi için

## Proje Yapısı

### 1. ProcessMonitor (C# Console App)

- `Program.cs`: Ana uygulama kodu
- `ProcessMonitor.csproj`: Proje yapılandırması
- Özellikler:
  - Windows işlemlerini izleme
  - İşlem bilgilerini toplama (ad, ID, başlangıç zamanı, bellek kullanımı)
  - WebSocket üzerinden veri gönderme
  - Otomatik yeniden bağlanma

### 2. WebRTCServer (Node.js)

- `server.js`: WebSocket sunucu kodu
- `package.json`: Bağımlılıklar ve yapılandırma
- Özellikler:
  - WebSocket bağlantılarını yönetme
  - JSON formatında veri alma
  - Tarih bazlı loglama
  - Otomatik log klasörü oluşturma

## Kurulum

### Gereksinimler

- .NET 8.0 SDK
- Node.js ve npm

### ProcessMonitor (C#)

```bash
cd ProcessMonitor
dotnet restore
dotnet run
```

### WebRTCServer (Node.js)

```bash
cd WebRTCServer
npm install
node server.js
```

## Kullanım

1. Önce WebSocket sunucusunu başlatın:

```bash
cd WebRTCServer
npm install
node server.js
```

2. Ardından ProcessMonitor'u başlatın:

```bash
cd ProcessMonitor
dotnet restore
dotnet run
```

## Veri Formatı

İşlem bilgileri JSON formatında gönderilir:

```json
{
  "ProcessName": "notepad.exe",
  "Id": 1234,
  "StartTime": "2024-01-01 10:00:00",
  "MemoryUsage": 123456789
}
```

## Log Dosyaları

- Konum: `WebRTCServer/logs/`
- Format: `process_log_YYYY-MM-DD.json`
- İçerik: Timestamp ve işlem listesi

## Güvenlik Notları

- Proje şu anda yerel ağda test amaçlı kullanım için tasarlanmıştır
- Üretim ortamı için:
  - SSL/TLS eklenmeli
  - Kimlik doğrulama yapılmalı
  - Veri şifreleme uygulanmalı

## Hata Yönetimi

- Bağlantı kopması durumunda otomatik yeniden bağlanma
- İşlem bilgisi alınamadığında null değer döndürme
- Log yazma hatalarını yakalama ve raporlama

## Geliştirme Önerileri

- Web arayüzü eklenmesi
- Veritabanı entegrasyonu
- İşlem filtreleme özellikleri
- Gerçek zamanlı grafik ve istatistikler
- Çoklu istemci desteği
- İşlem kaynaklı uyarı sistemi

## Lisans

MIT
