using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TalepTakip.Models
{
    internal class NotificationService
    {
        private Timer notificationTimer;
        private UserService userService;
        private int previousRequestCount = 0;

        public NotificationService(UserService userService)
        {
            this.userService = userService;
            notificationTimer = new Timer();
            notificationTimer.Interval = 10000; // 10 saniye
            notificationTimer.Tick += NotificationTimer_Tick;
        }

        // Servisi başlat
        public void Start()
        {
            var requests = userService.GetAllRequests();
            previousRequestCount = requests.Count;
            notificationTimer.Start();
        }

        // Servisi durdur
        public void Stop()
        {
            notificationTimer.Stop();
        }

        // Timer Tick olayı
        private void NotificationTimer_Tick(object sender, EventArgs e)
        {
            var requests = userService.GetAllRequests();
            int currentRequestCount = requests.Count;

            if (currentRequestCount > previousRequestCount)
            {
                int newRequestCount = currentRequestCount - previousRequestCount;
                if (newRequestCount > 0)
                {
                    ShowNotification("Yeni Talep", "Yeni bir talep eklendi.");
                    previousRequestCount = currentRequestCount;
                }
            }
        }

        // Bildirim gösterme fonksiyonu
        private void ShowNotification(string title, string text)
        {
            
            NotifyIcon notifyIcon = new NotifyIcon
            {
                Visible = true,
                Icon = SystemIcons.Information,
                BalloonTipTitle = title,
                BalloonTipText = text
            };
            notifyIcon.ShowBalloonTip(3000);
        }
    }
}
