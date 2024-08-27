using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TalepTakip.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TalepTakip
{
    public partial class Home : Form
    {
        private string uName;
        private UserService userService;
        private NotificationService notificationService;
        private List<TalepTakip.Models.Request> requests;

        public Home(string uName)
        {
            InitializeComponent();
            this.uName = uName;
            userService = new UserService(new UserRepository());
        }

        // Çıkış yap
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Giriş formunu aç
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
            // Bildirim servisini durdur
            if (notificationService != null)
            {
                notificationService.Stop();
            }
        }
        // Ana sayfa yüklendiğinde
        public void Home_Load(object sender, EventArgs e)
        {
            // Hoş geldiniz mesajını ayarla
            guna2HtmlLabel1.Text = $"Hoşgeldiniz {uName}";

            // Kullanıcının rolünü al
            User currentUser = userService.GetUserByUsername(uName);
            List<TalepTakip.Models.Request> requests; // Talepleri tutacak liste

            if (currentUser.userRole == "Personel")
            {
                // Personel ise sadece kendi gönderdiği talepleri listele
                requests = userService.GetRequestsByUser(uName);
                // Personel rolü için kayıt ekle butonunu gizle
                guna2Button3.Visible = false;
                // Personel rolü için onayla butonunu gizle
                guna2DataGridView1.Columns["Column6"].Visible = false;

            }
            else if (currentUser.userRole == "Yönetici" || currentUser.userRole == "Müdür")
            {
                // Yönetici veya müdür ise tüm talepleri listele
                requests = userService.GetAllRequests();
                // Yönetici ve müdür rolü için kayıt ekle butonunu göster
                guna2Button3.Visible = true;
                // Yönetici ve müdür rolü için onayla butonunu göster
                guna2DataGridView1.Columns["Column6"].Visible = true;

                // Bildirim servisini başlat
                notificationService = new NotificationService(userService);
                notificationService.Start();
            }
            else
            {
                // Diğer roller için boş liste döndür
                requests = new List<TalepTakip.Models.Request>();
                // Diğer roller için kayıt ekle butonunu gizle
                guna2Button3.Visible = false;
                // Diğer roller için onayla butonunu gizle
                guna2DataGridView1.Columns["Column6"].Visible = false;
            }

            // Talepleri DataGridView'e ekle
            foreach (var request in requests)
            {
                int rowIndex = guna2DataGridView1.Rows.Add();
                guna2DataGridView1.Rows[rowIndex].Cells["Column1"].Value = request.UserName; // Mevcut "kullanıcı adı" sütununa ekle
                guna2DataGridView1.Rows[rowIndex].Cells["Column2"].Value = request.ReqTitle; // Mevcut "talep başlığı" sütununa ekle
                guna2DataGridView1.Rows[rowIndex].Cells["Column3"].Value = request.ReqDescription; // Mevcut "talep açıklaması" sütununa ekle
                guna2DataGridView1.Rows[rowIndex].Cells["Column4"].Value = request.ReqDate; // Mevcut "tarih" sütununa ekle
                guna2DataGridView1.Rows[rowIndex].Cells["Column5"].Value = request.State; // Mevcut "Durum" sütununa ekle
                guna2DataGridView1.Rows[rowIndex].Cells["Column6"].Value = "Tamamla"; // Mevcut "Durum" sütununa ekle
                guna2DataGridView1.Rows[rowIndex].Cells["Column7"].Value = request.ReqId; // Talep Id al, gizli sütuna ekle
                guna2DataGridView1.Rows[rowIndex].Cells["Column8"].Value = request.FileName; // Mevcut "Dosya Adı" sütununa ekle
                guna2DataGridView1.Rows[rowIndex].Cells["Column9"].Value = request.Description; // Mevcut "Açıklama" sütununa ekle
                guna2DataGridView1.Rows[rowIndex].Cells["Column10"].Value = request.CompDate; // Mevcut "Tamamlanma Tarihi" sütununa ekle

                // Talep durumuna göre satır rengini ayarla
                if (request.State == "Beklemede")
                {
                    guna2DataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                }
            }

            // DataSource'u ayarladıktan veya verileri yükledikten sonra: Tarihe göre sırala
            guna2DataGridView1.Sort(guna2DataGridView1.Columns["Column4"], System.ComponentModel.ListSortDirection.Descending);
        }
        // DataGridView'de onayla butonuna tıklandığında
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Geçerli bir hücreye tıklanıp tıklanmadığını kontrol edin
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Tıklanan hücrenin "Column6" olup olmadığını kontrol edin (Onayla butonu)
                if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "Column6")
                {
                    // Seçilen satırın talep durumunu al
                    var cell = guna2DataGridView1.Rows[e.RowIndex].Cells["Column5"];
                    // Seçilen satırın talep Id'sini al
                    var requestIdcell = guna2DataGridView1.Rows[e.RowIndex].Cells["Column7"];
                    // Seçilen satıra yazılan açıklamayı al
                    var descriptionCell = guna2DataGridView1.Rows[e.RowIndex].Cells["Column9"];
                    // Seçilen satırın tamamlanma tarihini al
                    var compDateCell = guna2DataGridView1.Rows[e.RowIndex].Cells["Column10"];

                    // Eğer durum ve talep Id'si alınabiliyorsa
                    if (cell != null && cell.Value != null && requestIdcell != null && requestIdcell.Value != null)
                    {
                        string requestId = requestIdcell.Value.ToString(); // Talep Id'sini al
                        string state = cell.Value.ToString(); // Durumu al
                        string description = descriptionCell.Value.ToString(); // Açıklamayı al
                        string compDate = compDateCell.Value.ToString(); // Tamamlanma zamanını al
                        
                        // Eğer açıklama yoksa
                        if (description == "")
                        {
                            description = "Açıklama yok";
                        }

                        // Eğer durum "Tamamlandı" ve Açıklama yok ise
                        if (state == "Tamamlandı" && description == "Açıklama yok")
                        {
                            // "Tamamlandı" durumundaki satır için buton tıklanamaz
                            MessageBox.Show("Bu talep zaten tamamlandı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        else
                        {
                            // Eğer tamamamlanma tarihi yoksa, o anın tarihini ekle
                            if (string.IsNullOrEmpty(compDate))
                            {
                                compDate = DateTime.Now.ToString();
                                compDateCell.Value = compDate;
                            }

                            // Talebi onayla
                            userService.ApproveRequest(requestId, description, compDate);
                            // Durumu "Tamamlandı" olarak güncelle
                            cell.Value = "Tamamlandı";
                            descriptionCell.Value = description;
                            // Tamamlandı olan satırı sarıdan beyaza çevir
                            int rowIndex = guna2DataGridView1.Rows[e.RowIndex].Index;
                            guna2DataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.White;
                            
                            // Yeniden yükle
                            this.Refresh();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Talep durumu alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                // Tıklanan hücrenin "Column8" olup olmadığını kontrol edin (Dosya indirme butonu)
                if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "Column8")
                {
                    // Seçilen satırın talep dosya adını al
                    var cell = guna2DataGridView1.Rows[e.RowIndex].Cells["Column8"];
                    if (cell != null && cell.Value != null)
                    {
                        string fileName = cell.Value.ToString();
                        // Dosyayı indir
                        userService.DownloadFile(fileName);
                    }
                    else
                    {
                        MessageBox.Show("Dosya adı alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Yöneticinin Talep açıklaması eklemesi
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            guna2DataGridView1.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;

            // Geçerli bir hücreye tıklanıp tıklanmadığını kontrol edin
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Tıklanan hücrenin "Column9" olup olmadığını kontrol edin (Açıklama hücresi)
                if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "Column9")
                {
                    // Kullanıcının rolünü al
                    User currentUser = userService.GetUserByUsername(uName);

                    if (currentUser.userRole == "Yönetici" || currentUser.userRole == "Müdür")
                    {
                        // Açıklamayı düzenleme moduna al
                        guna2DataGridView1.Rows[e.RowIndex].Cells["Column9"].ReadOnly = false;
                    }
                }
            }
        }
        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Uygulamayı kapat
            Application.Exit();
            // Bildirim servisini durdur
            if (notificationService != null)
            {
                notificationService.Stop();
            }
        }
        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            // Talep formunu aç
            Talep talepForm = new Talep(uName);
            talepForm.Show();
            this.Hide();
        }
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            // Kullanıcı formunu aç
            Users userForm = new Users(uName);
            userForm.Show();
            this.Hide();
        }

        // Talebi sil
        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Kullanıcının rolünü al
            User currentUser = userService.GetUserByUsername(uName);
            // Eğer rolü "Yönetici" veya "Müdür" ise
            if (currentUser.userRole == "Yönetici" || currentUser.userRole == "Müdür")
            {
                //seçilen talebin id'sini al
                var requestId = guna2DataGridView1.SelectedRows[0].Cells["Column7"].Value.ToString();
                //talebi sil
                userService.DeleteRequest(requestId);
                // Listeyi yeniden yükle
                this.Hide();
                Home homeForm = new Home(uName);
                homeForm.Show();
            }
            else
            {
                MessageBox.Show("Bu işlemi yapmaya yetkiniz yok.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {
            About aboutForm = new About();
            aboutForm.Show();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            // Listeyi yeniden yükle
            Home homeForm = new Home(uName);
            homeForm.Show();
            this.Hide();    
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            // Eğer requests listesi doluysa dışa aktar işlemine başla
            //if (requests != null && requests.Count > 0)
            //{
                // Excel dosyasına yazma
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Requests");

                    // Başlık satırlarını oluştur
                    ws.Cells[1, 1].Value = "Talep ID";
                    ws.Cells[1, 2].Value = "Talep Başlığı";
                    ws.Cells[1, 3].Value = "Talep Açıklaması";
                    ws.Cells[1, 4].Value = "Kullanıcı Adı";
                    ws.Cells[1, 5].Value = "Durum";
                    ws.Cells[1, 6].Value = "Talep Tarihi";
                    ws.Cells[1, 7].Value = "Dosya Adı";
                    ws.Cells[1, 8].Value = "Açıklama";


                    // Listeyi satır satır Excel'e yaz
                    for (int i = 0; i < requests.Count; i++)
                    {
                        ws.Cells[i + 2, 1].Value = requests[i].ReqId;
                        ws.Cells[i + 2, 2].Value = requests[i].ReqTitle;
                        ws.Cells[i + 2, 3].Value = requests[i].ReqDescription;
                        ws.Cells[i + 2, 4].Value = requests[i].UserName;
                        ws.Cells[i + 2, 5].Value = requests[i].State;
                        ws.Cells[i + 2, 6].Value = requests[i].ReqDate;
                        ws.Cells[i + 2, 6].Value = requests[i].FileName;
                        ws.Cells[i + 2, 7].Value = requests[i].Description;
                    }

                    // Dosyayı kaydetme
                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "Excel Files|*.xlsx";
                        saveFileDialog.Title = "Excel Dosyası Kaydet";

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            FileInfo fi = new FileInfo(saveFileDialog.FileName);
                            pck.SaveAs(fi);
                            MessageBox.Show("Veriler Excel'e başarıyla aktarıldı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            //}
            //else
            //{
            //    MessageBox.Show("Dışa aktarılacak veri yok.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }
    }
}
