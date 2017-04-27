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
    public partial class Username : Form
    {
        public Username()
        {
            InitializeComponent();
            //increment current player to choose name
            variables.CurrentChoice++;
            label1.Text = "Entrez le nom pour le joueur " + variables.CurrentChoice;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrWhiteSpace(textBox1.Text.ToString()))
            {
                //add name to list
                variables.names.Add(textBox1.Text.ToString());
                this.Close();
            }
        }
    }
}
