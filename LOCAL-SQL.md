# Yerel SQL Server

SQL Server 2022 Developer, Docker üzerinde çalışır. Veriler
`dotnet-web-api_sqlserver-data` volume'unda tutulur. Port yalnızca
`127.0.0.1:1433` üzerinden erişilebilir.

## Başlat / durdur

Proje kökünde, Docker Desktop açıkken:

```sh
export PATH="/Applications/Docker.app/Contents/Resources/bin:$PATH"
docker compose --env-file .env.sqlserver -f compose.sqlserver.yml up -d
docker compose --env-file .env.sqlserver -f compose.sqlserver.yml stop
```

`stop` verileri korur. `down -v` veritabanı volume'unu siler.

## API'yi çalıştır

```sh
cd shopapp.webapi
python3 ../scripts/with-local-db.py dotnet run
```

Adres: http://localhost:4200/api/products

Web UI için aynı komutu `shopapp.webui` klasöründen çalıştırabilirsin.
İki uygulama aynı `shopappdb` veritabanına bağlanır. İşlemi Ctrl+C ile durdur.
Normal `dotnet run`, yerel bağlantıyı yüklemez; yukarıdaki yardımcı komutu kullan.

## Bağlantı

- Sunucu: `127.0.0.1,1433`
- Kimlik doğrulama: SQL Server Authentication
- Kullanıcı: `sa` (yalnızca yerel geliştirme)
- Veritabanı: `shopappdb`
- Parola: `.env.sqlserver` dosyasındaki `MSSQL_SA_PASSWORD`
- Yerel sunucu sertifikasına güven: etkin

`.env.sqlserver` Git tarafından yok sayılır ve yalnızca dosya sahibi okuyabilir.
Bu dosyayı paylaşma. Yardımcı script bağlantıyı
`ConnectionStrings__MsSqlConnection` ortam değişkeniyle yükler;
projenin mevcut appsettings dosyaları ve SQL Server sağlayıcısı korunur.

## Migration'lar

İlk kurulumda mevcut `ShopContext` ve `ApplicationContext` migration'ları
uygulandı. ShopContext migration'ı 5 örnek ürün içerir.
Web UI açılışta mevcut migration'ları kontrol eder; API bunu yapmaz.

Yeni, boş bir volume oluşturursan EF Core 3.1 CLI ile aşağıdaki işlemleri
yapabilir veya Web UI'ın mevcut migration başlangıcını kullanabilirsin:

```sh
cd shopapp.webapi
python3 ../scripts/with-local-db.py dotnet ef database update --project ../shopapp.data --startup-project . --context ShopContext
cd ../shopapp.webui
python3 ../scripts/with-local-db.py dotnet ef database update --context ApplicationContext
```

Bu komutlar uyumlu `dotnet-ef` aracının kurulu olmasını gerektirir.
İlk kurulumda araç geçici olarak `/private/tmp/shopapp-ef-tools` içine kuruldu;
bu yol kalıcı kurulum değildir.

Microsoft kurulum kaynağı:
https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16
