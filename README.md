# Talep Takip Sistemi
Bu proje, C# Windows Forms kullanılarak geliştirilmiş bir Telep Takip Sistemi uygulamasıdır. Telefonla iletilen talep, rica veya isteklerin kaydının tutulamaması nedeniyle bu programa ihtiyaç duyulmuştur. Program kurumdaki ortak sunucuda bulunan MySQL veritabanına bağlanarak diğer kullanıcılar tarafından kullanılabilmektedir.

## 🔹 Özellikler
- Program içerisinde "Personel" ve "Yönetici" rolleri bulunmaktadır. Bu rollere göre programda yapılabilecek işlemler değişmektedir.
  ### Personel:
- Program üzerinden yeni talep iletebilir, mevcut taleplerinin durumunu görüntüleyebilir.
  ### Yönetici:
- Programda tüm talepleri görüntüleyebilir, bir talebi silebilir, veya "Tamamla" butonu ile tamamlayabilir.
- Tamamlama işlemi esnasında açıklama girebilir.
- Kullanıcı listesini görüntüleyebilir. Yeni kullanıcı ekleyebilir, mevcut bir kullanıcıyı düzenleyebilir ya da silebilir.
- Personel için tanımlanan işlemlerin hepsini yönetici de yapabilir.

## 🔧 Kurulum
1. Projeyi GitHub'dan veya ZIP olarak indirin.
2. Visual Studio'da TalepTakip.sln dosyasını açın.
3. Gerekli NuGet paketlerini ekleyin.
4. İstediğiniz veritabanına App.config dosyası içindeki connection string ile bağlantısını yapın.
5. Projeyi derleyin ve çalıştırın.

Projede .NET Framework 4.5 kullanılmıştır. Ayrıca 4.7.2 veya 4.8 sürümleri ile de uyumlu çalışabilir.

## 💻 Kullanılan Teknolojiler
- Visual Studio 2022
- .NET Framework 4.5
- MySQL veritabanı, şu tablolar ile: requests, roles, states ve users.

## 📦 Bağımlılıklar
- **NuGet Paketleri:**
  - Guna.UI2.WinForms: Gelişmiş UI bileşenleri için.
  - BouncyCastle.Cryptography: Kriptografik fonksiyonlar için.
  - MySql.Data: MySQL veritabanı ile etkileşim için.

## 📸 Ekran Görüntüleri
- Giriş ekranı
<img src = "Images/1.png"/>
- Personel ana sayfa
<img src = "Images/2.png"/>
- Talep girişi
<img src = "Images/3.png"/>
- Yönetici ana sayfa
<img src = "Images/1.png"/>

## 📧 İletişim
cagriesmer7@gmail.com
