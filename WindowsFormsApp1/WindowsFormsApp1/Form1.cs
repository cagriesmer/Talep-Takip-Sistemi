using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 yusuf = new Form2();
            yusuf.Show();
   
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SoundPlayer player = new SoundPlayer("C:\\Users\\cagri\\Downloads\\videoplayback.wav");
            player.Play();


        }
    }
}
