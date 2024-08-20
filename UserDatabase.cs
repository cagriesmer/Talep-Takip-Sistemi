using MySql.Data.MySqlClient;
using System.Configuration;

namespace TalepTakip1
{
    public class UserDatabase
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

        public void KayitOl(User user)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Users (userName, userRole, userPassword) VALUES (@userName, @userRole, @userPassword)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userName", user.userName);
                    cmd.Parameters.AddWithValue("@userRole", user.userRole);
                    cmd.Parameters.AddWithValue("@userPassword", user.userPassword);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
