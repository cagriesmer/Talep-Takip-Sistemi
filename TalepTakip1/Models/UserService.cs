using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalepTakip.Models;

namespace TalepTakip.Models
{
    internal class UserService
    {
        private UserRepository userRepository;

        public UserService(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        // Yeni Kullanıcı kaydı
        public bool Register(string username, string password, string name, string surname)
        {
            return userRepository.AddUser(username, password, name, surname);
        }

        // Kullanıcı rolü güncelleme
        public bool UpdateUserRole(string userName, string roleName)
        {
            return userRepository.UpdateUserRole(userName, roleName);
        }

        // Kullanıcı girişi
        public bool Login(string username, string password)
        {
            // Kullanıcıyı veritabanından al
            var user = userRepository.GetUserByUsername(username);

            if (user == null)
            {
                return false;
            }

            // Şifreyi doğrula
            if (string.IsNullOrEmpty(user.userPassword))
            {
                throw new InvalidOperationException("Kullanıcı şifresi veritabanında bulunamadı.");
            }

            try
            {
                // Şifre doğrulama
                bool isVerified = BCrypt.Net.BCrypt.Verify(password, user.userPassword);
                return isVerified;
            }
            catch (BCrypt.Net.SaltParseException ex)
            {
                MessageBox.Show("Şifre doğrulama sırasında bir hata oluştu. Lütfen tekrar deneyin.\n" + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bilinmeyen bir hata oluştu.\n" + ex.Message);
                return false;
            }
        }

        // Tüm kullanıcıları getir
        public List<User> GetAllUsers()
        {
            return userRepository.GetAllUsers();
        }


        // Yeni talep ekleme
        public bool AddRequest(string userName, string reqDescription, string reqTitle, string reqDate, string state, string fileName, byte[] fileData, string fileType)
        {
            return userRepository.AddRequest(userName, reqDescription, reqTitle, reqDate, state, fileName, fileData, fileType);
        }
        
        // Kullanıcıya ait talepleri getir
        public List<TalepTakip.Models.Request> GetRequestsByUser(string userName)
        {
            return userRepository.GetRequestsByUser(userName);
        }

        // Tüm talepleri getir
        public List<TalepTakip.Models.Request> GetAllRequests()
        {
            return userRepository.GetAllRequests();
        }

        // Kullanıcı adına göre kullanıcı getir
        public User GetUserByUsername(string username)
        {
            return userRepository.GetUserByUsername(username);
        }

        // Kullanıcı bilgilerini güncelleme
        public bool UpdateUser(User user)
        {
            return userRepository.UpdateUser(user);
        }

        // Talep silme
        public bool DeleteRequest(string requestId)
        {
            return userRepository.DeleteRequest(requestId);
        }

        // Talep durumunu güncelleme
        public void ApproveRequest(string requestId, string description, string compDate)
        {
            userRepository.UpdateRequestState(requestId, "Tamamlandı", description, compDate);
        }

        // Talep esnasında yükenen dosyayı veritabanına kaydetme
        public bool SaveFileToDatabase(string fileName, byte[] fileData)
        {
            return userRepository.SaveFileToDatabase(fileName, fileData);
        }

        // Veritabanındaki dosyayı bilgisayara indirme
        public byte[] DownloadFile(string fileName)
        {
            return userRepository.DownloadFile(fileName);
        }
        // Kullanıcı Silme
        public bool DeleteUser(string username)
        {
            return userRepository.DeleteUser(username);
        }
        public List<Role> GetAllRoles()
        {
            return userRepository.GetAllRoles();
        }
    }
}
