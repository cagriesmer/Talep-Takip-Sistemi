using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalepTakip1.Models;

namespace TalepTakip1
{
    public partial class Talep : Form
    {
        private string uName;
        private UserService userService;
        private string filePath;
        private byte[] fileData;
        private string fileType;

        // Talep formu açıldığında kullanıcı adını al
        public Talep(string uName)
        {
            InitializeComponent();
            this.uName = uName;
            userService = new UserService(new UserRepository());

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Talep_KeyDown);

        }

        // Enter tuşuna basıldığında button1_Click olayını tetikle
        private void Talep_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Enter tuşuna basıldığında button1_Click olayını tetikliyoruz
                guna2Button2_Click(sender, e);

                // Enter tuşuna basıldığında bip sesi çıkmasını engelliyoruz
                e.SuppressKeyPress = true;
            }
        }

        // Talep formunu gönder, bilgileri veritabanına kaydet
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string userName = uName; // Giriş yapan kullanıcının adı
            string reqDescription = guna2TextBox1.Text; // TextBox içindeki açıklama
            string reqTitle = guna2TextBox2.Text; // TextBox içindeki Başlık
            DateTime reqDate = DateTime.Now; // Şu anki zamanı al
            string state = "Beklemede"; // Başlangıç durumu
            string filePath = this.filePath;
            byte[] fileData = this.fileData;
            string fileType = this.fileType;

            if (string.IsNullOrWhiteSpace(reqDescription) || string.IsNullOrWhiteSpace(reqTitle))
            {
                MessageBox.Show("Başlık ve açıklama kutularının doldurulması gerekmektedir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool isSuccess = userService.AddRequest(userName, reqDescription, reqTitle, reqDate, state, filePath, fileData, fileType);

            if (isSuccess)
            {
                MessageBox.Show("Talep başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                guna2TextBox1.Text = "";
                guna2TextBox2.Text = "";
                filePath = null;
                fileData = null;
                fileType = null;
                guna2HtmlLabel5.Visible = false;

                notifyIcon1.ShowBalloonTip(3000);
                notifyIcon1.Visible = true;
            }
            else
            {
                MessageBox.Show("Talep kaydedilirken bir hata oluştu!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Ana sayfaya dön
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Home homeForm = new Home(uName);
            homeForm.Show();
            this.Hide();
        }

        // Talep formunu kapat
        private void Talep__FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void Talep_Load(object sender, EventArgs e)
        {

        }

        // Dosya yükleme işlemi
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {

                    this.filePath = ofd.FileName; // Dosya yolunu al

                    // Dosya boyutunu kontrol et (10 MB = 10 * 1024 * 1024 byte)
                    const int maxFileSize = 10 * 1024 * 1024;
                    FileInfo fileInfo = new FileInfo(filePath);
                    if (fileInfo.Length > maxFileSize)
                    {
                        MessageBox.Show("Dosya boyutu 10 MB'ı geçemez.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Dosya boyutu uygunsa dosya verilerini al
                    this.fileData = File.ReadAllBytes(filePath);
                    this.fileType = Path.GetExtension(filePath);

                    // Dosya yükleme bilgilerini güncelle
                    guna2HtmlLabel5.Text = $"{Path.GetFileName(filePath)} dosyası yüklendi.";
                    guna2HtmlLabel5.Visible = true;
                }
            }
        }

        private void guna2HtmlLabel8_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }
    }
}
