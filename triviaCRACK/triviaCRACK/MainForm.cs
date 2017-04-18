﻿using System;
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
            DrawIt();
        }

        private void DrawIt()
        {
            System.Drawing.Graphics graphics = this.CreateGraphics();
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(
               50, 50, 150, 150);
            graphics.DrawEllipse(System.Drawing.Pens.Black, rectangle);
            graphics.DrawRectangle(System.Drawing.Pens.Red, rectangle);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawIt();
        }
    }
}
