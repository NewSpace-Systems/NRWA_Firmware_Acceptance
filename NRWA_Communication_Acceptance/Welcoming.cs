using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NRWA_Communication_Acceptance
{
    public partial class Welcoming : Form
    {
        public Welcoming()
        {
            InitializeComponent();
        }

        private void Welcoming_Load(object sender, EventArgs e)
        {
            this.Show();
            System.Threading.Thread.Sleep(2000);
            NRWA_FirmVer nRWA_FirmVer = new NRWA_FirmVer();
            nRWA_FirmVer.Show(this);
            this.Hide();
        }
    }
}
