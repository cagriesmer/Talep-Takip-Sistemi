using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows.Forms;

namespace TalepTakip.Models
{
    internal class UserRepository
    {
        private string connectionString;

        // Veritabanı bağlantı cümlesini al
        public UserRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        }

        // Yeni kullanıcı ekle
        public bool AddUser(string username, string hashedPassword, string name, string surname)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Kullanıcı adının mevcut olup olmadığını kontrol et
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE userName = @userName";
                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@userName", username);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        // Kullanıcı adı mevcut, kullanıcı eklenemez
                        MessageBox.Show("Bu kullanıcı adı zaten mevcut. Lütfen başka bir kullanıcı adı deneyin.");
                        return false;
                    }
                }

                // Kullanıcı adı mevcut değilse yeni kullanıcı ekle
                string query = "INSERT INTO Users (userName, userPassword, name, surname) VALUES (@userName, @userPassword, @name, @surname)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userName", username);
                    cmd.Parameters.AddWithValue("@userPassword", hashedPassword);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@surname", surname);
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        // Kullanıcı adına göre kullanıcı bilgilerini getir
        public User GetUserByUsername(string username)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT userName, userPassword, userRole, name, surname FROM Users WHERE userName = @userName";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userName", username);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                userName = reader["userName"].ToString(),
                                userPassword = reader["userPassword"].ToString(),
                                userRole = reader["userRole"].ToString(),
                                name = reader["name"].ToString(),
                                surname = reader["surname"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }


        // Kullanıcı adına göre kullanıcı rolünü getir
        public bool UpdateUserRole(string userName, string roleName)
        {
            string updateQuery = "UPDATE users SET userRole = @roleName WHERE userName = @userName";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand(updateQuery, conn))
                    {
                        command.Parameters.AddWithValue("@roleName", roleName);
                        command.Parameters.AddWithValue("@userName", userName);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda mesaj göster
                    MessageBox.Show("Veritabanı güncelleme hatası: " + ex.Message);
                    return false;
                }
            }
        }
        
        // Tüm kullanıcıları getir
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            string query = "SELECT userID, userName, userRole, name, surname FROM users";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    id = Convert.ToInt32(reader["userID"]),
                                    userName = reader["userName"].ToString(),
                                    userRole = reader["userRole"].ToString(),
                                    name = reader["name"].ToString(),
                                    surname = reader["surname"].ToString()
                                };
                                users.Add(user);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda mesaj göster
                    MessageBox.Show("Veritabanı hatası: " + ex.Message);
                }
            }

            return users;
        }

        // Yeni talep ekle
        public bool AddRequest(string userName, string reqDescription, string reqTitle, DateTime reqDate, string state, string fileName, byte[] fileData, string fileType)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO requests (userName, reqDate, reqDescription, reqTitle, state, fileName, fileData, fileType) VALUES (@userName, @reqDate, @reqDescription, @reqTitle, @state, @fileName, @fileData, @fileType)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userName", userName);
                    cmd.Parameters.AddWithValue("@reqDescription", reqDescription);
                    cmd.Parameters.AddWithValue("@reqTitle", reqTitle);
                    cmd.Parameters.AddWithValue("@reqDate", reqDate);
                    cmd.Parameters.AddWithValue("@state", "Beklemede");
                    cmd.Parameters.AddWithValue("@fileName", fileName != null ? Path.GetFileName(fileName) : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@fileData", fileData != null ? fileData : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@fileType", fileType != null ? fileType : (object)DBNull.Value);
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        // Kullanıcı adına göre talepleri getir
        public List<TalepTakip.Models.Request> GetRequestsByUser(string userName)
        {
            List<TalepTakip.Models.Request> requests = new List<TalepTakip.Models.Request>();
            string query = "SELECT * FROM requests WHERE userName = @userName";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userName", userName);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            requests.Add(new Request
                            {
                                ReqId = reader["requestID"].ToString(),
                                UserName = reader["userName"].ToString(),
                                ReqDescription = reader["reqDescription"].ToString(),
                                ReqTitle = reader["reqTitle"].ToString(),
                                ReqDate = Convert.ToDateTime(reader["reqDate"]),
                                State = reader["state"].ToString(),
                                FileName = reader["fileName"].ToString(),
                                Description = reader["description"].ToString(),
                                CompDate = reader["compDate"].ToString()

                            });
                        }
                    }
                }
            }
            return requests;
        }

        // Tüm talepleri getir
        public List<TalepTakip.Models.Request> GetAllRequests()
        {
            List<TalepTakip.Models.Request> requests = new List<TalepTakip.Models.Request>();
            string query = "SELECT * FROM requests";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            requests.Add(new Request
                            {
                                ReqId = reader["requestID"].ToString(),
                                UserName = reader["userName"].ToString(),
                                ReqDescription = reader["reqDescription"].ToString(),
                                ReqTitle = reader["reqTitle"].ToString(),
                                ReqDate = Convert.ToDateTime(reader["reqDate"]),
                                State = reader["state"].ToString(),
                                FileName = reader["fileName"].ToString(),
                                Description = reader["description"].ToString(),
                                CompDate = reader["compDate"].ToString()
                            });
                        }
                    }
                }
            }

            return requests;
        }

        // Dosya indirme
        public byte[] DownloadFile(string fileName)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT fileName, fileData, fileType FROM requests WHERE FileName = @FileName";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FileName", fileName);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string fileNameD = reader["fileName"].ToString();
                            string fileType = reader["fileType"].ToString();
                            byte[] fileData = (byte[])reader["fileData"];

                            // Dosyayı kaydetme işlemi
                            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                            {
                                saveFileDialog.FileName = fileNameD;
                                saveFileDialog.Filter = "All files (*.*)|*.*";
                                saveFileDialog.DefaultExt = fileType;


                                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    string dosyaYolu = saveFileDialog.FileName;

                                    // İndirilenler klasörüne dosyayı kaydet
                                    File.WriteAllBytes(dosyaYolu, fileData);
                                    MessageBox.Show("Dosya indirildi: " + dosyaYolu);

                                    // Dosyayı aç ve göster
                                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                                    {
                                        FileName = dosyaYolu,
                                        UseShellExecute = true
                                    });
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        // Kullanıcı bilgilerini güncelle
        public bool UpdateUser(User user)
        {
            string updateQuery = "UPDATE users SET userPassword = @userPassword, userRole = @userRole, name = @name, surname = @surname WHERE userName = @userName";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.Add("@userPassword", MySqlDbType.VarChar).Value = user.userPassword;
                        cmd.Parameters.Add("@userRole", MySqlDbType.VarChar).Value = user.userRole;
                        cmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = user.name;
                        cmd.Parameters.Add("@surname", MySqlDbType.VarChar).Value = user.surname;
                        cmd.Parameters.Add("@userName", MySqlDbType.VarChar).Value = user.userName;

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda mesaj göster
                    MessageBox.Show("Veritabanı güncelleme hatası: " + ex.Message);
                    return false;
                }
            }
        }

        // Talep durumunu güncelle
        public void UpdateRequestState(string requestId, string state, string description, string compDate)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Requests SET state = @state, description = @description, compDate = @compDate WHERE requestID = @requestID";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@state", state);
                    cmd.Parameters.AddWithValue("@requestID", requestId);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@compDate", compDate);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Talep " + state.ToString() + " olarak kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Talep esnasında yüklenen dosyayı veritabanına kaydet
        public bool SaveFileToDatabase(string fileName, byte[] fileData)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO files (FileName, FileData) VALUES (@FileName, @FileData)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FileName", Path.GetFileName(fileName));
                        cmd.Parameters.AddWithValue("@FileData", fileData);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    // Hata yönetimi
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
        }
        // Talep silme
        public bool DeleteRequest(string requestId)
        {
            // Kullanıcıya onay kutusu göster
            DialogResult result = MessageBox.Show("Talep silinecektir. Bu işlem geri alınamaz! Onaylıyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            // Kullanıcı onaylarsa talebi sil
            if (result == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM requests WHERE requestID = @requestID";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@requestID", requestId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            else
            {
                // Kullanıcı onaylamazsa işlem iptal edilir
                MessageBox.Show("Talep silme işlemi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }
        // Kullanıcı silme
        public bool DeleteUser(string username)
        {
            // Kullanıcıya onay kutusu göster
            DialogResult result = MessageBox.Show("Kullanıcı silinecektir. Bu işlem geri alınamaz! Onaylıyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            // Kullanıcı onaylarsa kullanıcıyı sil
            if (result == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM users WHERE userName = @userName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userName", username);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            else
            {
                // Kullanıcı onaylamazsa işlem iptal edilir
                MessageBox.Show("Kullanıcı silme işlemi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }
        public List<Role> GetAllRoles()
        {
            List<Role> roles = new List<Role>();
            string query = "SELECT roleID, roleName FROM roles";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Role role = new Role
                                {
                                    roleID = Convert.ToInt32(reader["roleID"]),
                                    roleName = reader["roleName"].ToString(),
                                };
                                roles.Add(role);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda mesaj göster
                    MessageBox.Show("Veritabanı hatası: " + ex.Message);
                }
            }

            return roles;
        }

    }
}
