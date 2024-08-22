# Talep Takip Sistemi
Bu proje, C# Windows Forms kullanılarak geliştirilmiş bir Telep Takip Sistemi uygulamasıdır. Telefonla iletilen talep, rica veya isteklerin kaydının tutulamaması nedeniyle bu programa ihtiyaç duyulmuştur. Program kurumdaki ortak sunucuda bulunan MySQL veritabanına bağlanarak diğer kullanıcılar tarafından kullanılabilmektedir.

## 🔹 Özellikler
- Program içerisinde "Personel" ve "Yönetici" rolleri bulunmaktadır. Bu rollere göre programda yapılabilecek işlemler değişmektedir.
  # Personel:
- Program üzerinden yeni talep iletebilir, mevcut taleplerinin durumunu görüntüleyebilir.
  # Yönetici:
- Programda tüm talepleri görüntüleyebilir, bir talebi silebilir, veya "Tamamla" butonu ile tamamlayabilir.
- Tamamlama işlemi esnasında açıklama girebilir.
- Kullanıcı listesini görüntüleyebilir. Yeni kullanıcı ekleyebilir, mevcut bir kullanıcıyı düzenleyebilir ya da silebilir.
- Personel için tanımlanan işlemlerin hepsini yönetici de yapabilir.
- 

## 🔧 Kurulum
1. Projeyi GitHub'dan veya ZIP olarak indirin.
2. Visual Studio'da TalepTakip.sln dosyasını açın.
3. Gerekli NuGet paketlerini ekleyin.
4. Projeyi derleyin ve çalıştırın.

Projede .NET Framework 4.5 kullanılmıştır. Ayrıca 4.7.2 veya 4.8 sürümleri ile de uyumlu çalışabilir.

## 💻 Kullanılan Teknolojiler
- Visual Studio 2022
- .NET Framework 4.5
- MySQL veritabanı, şu tablolar ile:

## 📦 Bağımlılıklar
- **NuGet Paketleri:**
  - Guna.UI2.WinForms: Gelişmiş UI bileşenleri için.
  - BouncyCastle.Cryptography: Kriptografik fonksiyonlar için.
  - iTextSharp: PDF dosyaları oluşturmak ve dışa aktarmak için.
  - System.Data.SqlClient: SQL veritabanı ile etkileşim için.

## 📸 Ekran Görüntüleri

<img src="https://github.com/user-attachments/assets/16a2cfaa-257c-4075-8a61-6301bc7e768e" alt="1" width="800"/>
<img src="https://github.com/user-attachments/assets/f7bc47ed-2f88-42c6-88b6-4d6c73f43078" alt="2" width="800"/>
<img src="https://github.com/user-attachments/assets/0dad5953-cdf3-4d58-a680-6d7c0b19d713" alt="3" width="800"/>
<img src="https://github.com/user-attachments/assets/9b131086-7a2a-460a-b3c7-1aac02f96866" alt="3" width="800"/>

## 📧 İletişim
0zaferguler@gmail.com
