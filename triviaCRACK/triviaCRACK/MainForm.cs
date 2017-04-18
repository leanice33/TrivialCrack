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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            
        }

      
        private void DrawCircle()
        {
            System.Drawing.Graphics graphics = this.CreateGraphics();
            graphics.DrawEllipse(System.Drawing.Pens.Black, 200, 0, 400, 400);
            graphics.DrawLine(Pens.Black, 200, 200, 600, 200);
            graphics.DrawLine(Pens.Black, 400, 0, 400, 400);
        }
      

        private void button1_Click(object sender, EventArgs e)
        {
         
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            DrawCircle();
        }
    }
}
