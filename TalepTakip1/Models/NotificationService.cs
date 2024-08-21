using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TalepTakip.Models
{
    internal class NotificationService
    {
        public void NotifyUser(string title, string message)
        {
            NotifyIcon notifyIcon = new NotifyIcon();
            notifyIcon.Visible = true;
            //notifyIcon.Icon = SystemIcons.Information;
            notifyIcon.BalloonTipTitle = title;
            notifyIcon.BalloonTipText = message;
            notifyIcon.ShowBalloonTip(3000);
            // Bildirim gösterildikten sonra notifyIcon'u gizle
            notifyIcon.BalloonTipClosed += (sender, args) => notifyIcon.Dispose();
        }
    }
}
