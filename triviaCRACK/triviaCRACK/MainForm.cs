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
    public partial class    MainForm : Form
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

        private void InitiatePlayers(int x)
        {
            Username choose;
            //open a new dialog for each player x = number of players
            switch (x)
            {
                case 2:
                    for(int i = 0; i < 2; ++i)
                    {
                        choose = new Username();
                        choose.ShowDialog();
                    }
                    break;
                case 3:
                    for (int i = 0; i < 3; ++i)
                    {
                        choose = new Username();
                        choose.ShowDialog();
                    }
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
            oraAjout.CommandType = CommandType.StoredProcedure;            //add player to database for each one            for (int i = 0; i < x; ++i)
            {
                OracleParameter orapamNum = new OracleParameter("pnom", OracleDbType.Varchar2);
                orapamNum.Direction = ParameterDirection.Input;
                orapamNum.Value = variables.names[i].ToString();
                oraAjout.Parameters.Add(orapamNum);
                oraAjout.ExecuteNonQuery();
                oraAjout.Parameters.Clear();
            }
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
