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
        //doesn't work ??
        private void ShowQuestion(string x)
        {
            try
            {
                //wrong parametre type??
                OracleCommand Oracmd = new OracleCommand("GESTIONQUESTION", objConn);
                Oracmd.CommandText = "GESTIONQUESTION.afficherquestion";
                Oracmd.CommandType = CommandType.StoredProcedure;
                //sending char to function
                OracleParameter OraDesc = new OracleParameter("pcat", OracleDbType.Char);
                OraDesc.Value = x;
                OraDesc.Direction = ParameterDirection.Input;
                Oracmd.Parameters.Add(OraDesc);

                //function returns int question number
                OracleParameter orapamres = new OracleParameter("RES",
                OracleDbType.Int32);
                orapamres.Direction = ParameterDirection.Output;
                Oracmd.Parameters.Add(orapamres);
                //TODO call function getquestion, send it the number we got from the first function
                //and replace lb_question.Text by the question and make it visible

                OracleDataReader Oraread = Oracmd.ExecuteReader();
                while (Oraread.Read())
                {
                    variables.numQuestion = Oraread.GetString(0);
                }


            }
            catch (Exception se)
            {
                MessageBox.Show(se.Message.ToString());
            }
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
                    if(i== rInt - 1)
                        ShowQuestion("V");
                }
                if (j == 1)
                {
                    HideAllCats();
                    LBL_Red.Show();
                    //todo R category
                    if (i == rInt - 1)
                        ShowQuestion("V");
                }
                if (j == 2)
                {
                    HideAllCats();
                    LBL_White.Show();
                    //todo random
                    if (i == rInt - 1)
                        ShowQuestion("V");
                }
                if (j == 3)
                {
                    HideAllCats();
                    LBL_Blue.Show();
                    if (i == rInt - 1)
                        ShowQuestion("B");
                }
                if (j == 4)
                {
                    HideAllCats();
                    LBL_Yellow.Show();
                    if (i == rInt - 1)
                        ShowQuestion("J");
                }


            }
        }
    }
}
