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
            InitiatePlayers(variables.Players);
        }


        private void InitiatePlayers(int x)
        {
            Username choose;
            //open a new dialog for each player x = number of players
            switch (x)
            {
                case 2:
                    for (int i = 0; i < 2; ++i)
                    {
                        choose = new Username();
                        choose.ShowDialog();
                    }
                    GB_P3.Visible = false;
                    GB_P4.Visible = false;
                    break;
                case 3:
                    for (int i = 0; i < 3; ++i)
                    {
                        choose = new Username();
                        choose.ShowDialog();
                    }
                    GB_P4.Visible = false;
                    break;
                case 4:
                    for (int i = 0; i < 4; ++i)
                    {
                        choose = new Username();
                        choose.ShowDialog();
                    }
                    break;
            }
            //prepare orcl command
            OracleCommand oraAjout = new OracleCommand("JOUEUR", objConn);
            oraAjout.CommandText = "JOUEUR.initialize";
            oraAjout.CommandType = CommandType.StoredProcedure;
            //add player to database for each one
            for (int i = 0; i < x; ++i)
            {
                OracleParameter orapamNum = new OracleParameter("pnom", OracleDbType.Varchar2);
                orapamNum.Direction = ParameterDirection.Input;
                orapamNum.Value = variables.names[i].ToString();
                oraAjout.Parameters.Add(orapamNum);
                oraAjout.ExecuteNonQuery();
                oraAjout.Parameters.Clear();
                //change groupboxes names to the actual player's name
                switch (i)
                {
                    case 0:
                        GB_P1.Text = variables.names[0];
                        break;
                    case 1:
                        GB_P2.Text = variables.names[1];
                        break;
                    case 2:
                        GB_P3.Text = variables.names[2];
                        break;
                    case 3:
                        GB_P4.Text = variables.names[3];
                        break;
                }
            }
        }
        private void Connect()
        {
            objConn = new OracleConnection(con);
            objConn.Open();
            MessageBox.Show(objConn.State.ToString());

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

            for (int i = 0; i < rInt; i++)
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
