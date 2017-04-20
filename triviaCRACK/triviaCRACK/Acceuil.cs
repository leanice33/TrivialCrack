using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace triviaCRACK
{
    public partial class F_Acceuil : Form
    {
        public F_Acceuil()
        {
            InitializeComponent();
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            variables.Players = 2;
            MainForm form1 = new MainForm();
            form1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            variables.Players = 3;
            MainForm form1 = new MainForm();
            form1.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            variables.Players = 4;
            MainForm form1 = new MainForm();
            form1.Show();
        }
    }
}
