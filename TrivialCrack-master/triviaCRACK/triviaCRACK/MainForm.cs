using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

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
           
        }

        private void Connect()
        {
            objConn = new OracleConnection(con);
            objConn.Open();
            MessageBox.Show(objConn.State.ToString());

        }
        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void HideAllCats()
        {
            LBL_Yellow.Hide();
            LBL_Green.Hide();
            LBL_Red.Hide();
            LBL_White.Hide();
            LBL_Blue.Hide();
        }

        private void BTN_Tourner_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            int rInt = r.Next(0, 50); //for ints
          
            for(int i = 0; i < rInt; i++)
            {
                int j = r.Next(0, 5);
                if (j == 0)
                {
                    HideAllCats();
                    LBL_Green.Show();
                }
                if (j == 1)
                {
                    HideAllCats();
                    LBL_Red.Show();
                }
                if (j == 2)
                {
                    HideAllCats();
                    LBL_White.Show();
                }
                if (j == 3)
                {
                    HideAllCats();
                    LBL_Blue.Show();
                }
                if (j == 4)
                {
                    HideAllCats();
                    LBL_Yellow.Show();
                }


            }
        }
    }
}
