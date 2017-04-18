using Oracle.ManagedDataAccess.Client;
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
        string con = "Data Source=(DESCRIPTION="
        + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)"
        + "(HOST=mercure.clg.qc.ca)(PORT=1521)))"
        + "(CONNECT_DATA=(SERVICE_NAME=ORCL.clg.qc.ca)));"
        + "User Id=mouranie;Password=ORACLE1";
        OracleConnection objConn;
        public MainForm()
        {
            InitializeComponent();
            Connect();
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

        private void Connect()
        {
            objConn = new OracleConnection(con);
            objConn.Open();
            MessageBox.Show(objConn.State.ToString());

        }
        private void button1_Click(object sender, EventArgs e)
        {
            DrawIt();
        }
    }
}
