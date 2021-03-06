﻿using Oracle.ManagedDataAccess.Client;
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
            PlayerThread();
        }

        private void PlayerThread()
        {
            Task t = Task.Run(() => {
                while (true)
                {
                    switch (variables.currentPlayer)
                    {
                        case 1:
                            LBL_TOURAQUELJOUEUR.Text = "Joueur 1";
                            break;
                        case 2:
                            LBL_TOURAQUELJOUEUR.Text = "Joueur 2";
                            break;
                        case 3:
                            LBL_TOURAQUELJOUEUR.Text = "Joueur 3";
                            break;
                        case 4:
                            LBL_TOURAQUELJOUEUR.Text = "Joueur 4";
                            break;
                    }                  
                }            
            });
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
            string query = "truncate table joueurcrack";
            //clear table 
            OracleCommand cmd = new OracleCommand(query, objConn);
            cmd.ExecuteNonQuery();


            cmd = new OracleCommand();
            cmd.Connection = objConn;
            cmd.CommandText = "Update questions set flag ='X' where flag='Y'";
            cmd.ExecuteNonQuery();

        }
        private void HideAllCats()
        {
            LBL_Yellow.Hide();
            LBL_Green.Hide();
            LBL_Red.Hide();
            LBL_White.Hide();
            LBL_Blue.Hide();
        }

        
        //works
        private void ShowQuestion(string x)
        {
            //x = category
            try
            {
                OracleCommand cmd = new OracleCommand();

                cmd.Connection = objConn;
                cmd.CommandText = "GESTIONQUESTION.afficherquestion";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("enonce", OracleDbType.Varchar2, 32767);
                cmd.Parameters["enonce"].Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.Add("pcat", OracleDbType.Char);
                cmd.Parameters["pcat"].Value = x;

                cmd.ExecuteNonQuery();
                string bval = cmd.Parameters["enonce"].Value.ToString();

                lb_question.Text = bval;
                lb_question.Visible = true;
                ShowReponses(bval);

            }
            catch (Exception se)
            {
                MessageBox.Show(se.Message.ToString());
            }

        }
         private void MakeAllLabelsVisible()
        {
            LBL_Blue.Visible = true;
            LBL_Green.Visible = true;
            LBL_Red.Visible = true;
            LBL_Yellow.Visible = true;
        }
        //works
        private void ShowReponses(string x)
        {
            //x = question
            try
            {
                OracleCommand cmd = new OracleCommand();

                //get question number ---- works
                cmd.Connection = objConn;
                cmd.CommandText = "GESTIONQUESTION.getquestion";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("numquestion", OracleDbType.Char, 8);
                cmd.Parameters["numquestion"].Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.Add("enonce", OracleDbType.Varchar2, 255);
                cmd.Parameters["enonce"].Value = "" + x + "";

                
                cmd.ExecuteNonQuery();
                string numquestion = cmd.Parameters["numquestion"].Value.ToString();
                //remove whitespace
                numquestion = numquestion.Replace(" ", "");

                cmd = new OracleCommand();
                cmd.Connection = objConn;
                cmd.CommandText = "Update questions set flag ='Y' where numquestion='" + numquestion + "'";
                cmd.ExecuteNonQuery();

                cmd = new OracleCommand();
                //get answers --- works
                cmd.Connection = objConn;
                cmd.CommandText = "GESTIONQUESTION.getreponce";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("resultat", OracleDbType.RefCursor);
                cmd.Parameters["resultat"].Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.Add("pnumquestion", OracleDbType.Char);
                cmd.Parameters["pnumquestion"].Value = numquestion.ToString();

                //no more error----------
                OracleDataReader oraread;
                oraread = cmd.ExecuteReader();
                while (oraread.Read())
                {               
                    variables.answers.Add(oraread.GetString(1));
                    //getstring(0) aka answer's number into answersNum list to check for correct answer later
                    variables.answersNum.Add(oraread.GetString(0));
                }
                //changing buttons text to answers
                for(int i = 0; i < variables.answers.Count; ++i)
                {
                    switch (i)
                    {
                        case 0:
                            BTN_CHOIX1.Text = variables.answers[i];
                            BTN_CHOIX1.Visible = true;
                            break;
                        case 1:
                            BTN_CHOIX2.Text = variables.answers[i];
                            BTN_CHOIX2.Visible = true;
                            break;
                        case 2:
                            BTN_CHOIX3.Text = variables.answers[i];
                            BTN_CHOIX3.Visible = true;
                            BTN_CHOIX4.Visible = false;
                            break;
                        case 3:
                            BTN_CHOIX4.Text = variables.answers[i];
                            BTN_CHOIX4.Visible = true;
                            break;                    
                    }
                }
                variables.answers.Clear();
            }
            catch (Exception se)
            {
                MessageBox.Show(se.Message.ToString());
            }
            
        }

        private void EnablePicBoxes()
        {
            PB_Blue.Enabled = true;
            PB_Green.Enabled = true;
            PB_RED.Enabled = true;
            PB_Yellow.Enabled = true;
        }

        private void DisablePicBoxes()
        {
            PB_Blue.Enabled = false;
            PB_Green.Enabled = false;
            PB_RED.Enabled = false;
            PB_Yellow.Enabled = false;
        }

        private void BTN_Tourner_Click(object sender, EventArgs e)
        {
            EnableAnswers();
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
                    if (i == rInt - 1)
                        ShowQuestion("R");
                }
                if (j == 2)
                {
                    HideAllCats();
                    LBL_White.Font = new Font("Times New Roman", 12, FontStyle.Bold);
                    LBL_White.Show();
                    EnablePicBoxes();
                    MakeAllLabelsVisible();




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
        private void Winner()
        {
            MessageBox.Show("Player " + variables.currentPlayer + " wins! The others lose!");
            this.Close();
            
        }

        private void DisableAnswers()
        {
            BTN_Tourner.Enabled = true;
            BTN_CHOIX1.Enabled = false;
            BTN_CHOIX2.Enabled = false;
            BTN_CHOIX3.Enabled = false;
            BTN_CHOIX4.Enabled = false;
        }

        private void EnableAnswers()
        {
            BTN_Tourner.Enabled = false;
            BTN_CHOIX1.Enabled = true;
            BTN_CHOIX2.Enabled = true;
            BTN_CHOIX3.Enabled = true;
            BTN_CHOIX4.Enabled = true;
        }
        private void BTN_CHOIX1_Click(object sender, EventArgs e)
        {
            DisableAnswers();
            OracleCommand cmd = new OracleCommand();

            //get question number ---- works
            cmd.Connection = objConn;
            cmd.CommandText = "GESTIONQUESTION.bonnereponse";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("estbonne", OracleDbType.Char, 1);
            cmd.Parameters["estbonne"].Direction = ParameterDirection.ReturnValue;

            cmd.Parameters.Add("numreponse", OracleDbType.Char);
            cmd.Parameters["numreponse"].Value = variables.answersNum[0];

            cmd.ExecuteNonQuery();
            string boss = cmd.Parameters["estbonne"].Value.ToString();

            if(boss == "Y")
            {
                label1.Text = "Bonne réponse!";
                switch (variables.currentPlayer)
                {
                    case 1:
                        progressBar1.PerformStep();
                        if(progressBar1.Value == 5)
                        {
                            Winner();
                        }
                        break;
                    case 2:
                        progressBar2.PerformStep();
                        if (progressBar2.Value == 5)
                        {
                            Winner();
                        }
                        break;
                    case 3:
                        progressBar3.PerformStep();
                        if (progressBar3.Value == 5)
                        {
                            Winner();
                        }
                        break;
                    case 4:
                        progressBar4.PerformStep();
                        if (progressBar4.Value == 5)
                        {
                            Winner();
                        }
                        break;
                }
            }
            else
            {
                label1.Text = "Mauvaise réponse!";
                if (variables.currentPlayer != variables.Players)
                {
                    variables.currentPlayer++;
                }
                else
                {
                    variables.currentPlayer = 1;
                }

            }
            variables.answersNum.Clear();
        }

        private void BTN_CHOIX2_Click(object sender, EventArgs e)
        {
            DisableAnswers();
            OracleCommand cmd = new OracleCommand();

            //get question number ---- works
            cmd.Connection = objConn;
            cmd.CommandText = "GESTIONQUESTION.bonnereponse";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("estbonne", OracleDbType.Char, 1);
            cmd.Parameters["estbonne"].Direction = ParameterDirection.ReturnValue;

            cmd.Parameters.Add("numreponse", OracleDbType.Char);
            cmd.Parameters["numreponse"].Value = variables.answersNum[1];

            cmd.ExecuteNonQuery();
            string boss = cmd.Parameters["estbonne"].Value.ToString();

            if (boss == "Y")
            {
                label1.Text = "Bonne réponse!";
                switch (variables.currentPlayer)
                {
                    case 1:
                        progressBar1.PerformStep();
                        if (progressBar1.Value == 5)
                        {
                            Winner();
                        }
                        break;
                    case 2:
                        progressBar2.PerformStep();
                        if (progressBar2.Value == 5)
                        {
                            Winner();
                        }
                        break;
                    case 3:
                        progressBar3.PerformStep();
                        if (progressBar3.Value == 5)
                        {
                            Winner();
                        }
                        break;
                    case 4:
                        progressBar4.PerformStep();
                        if (progressBar4.Value == 5)
                        {
                            Winner();
                        }
                        break;
                }
            }
            else
            {
                label1.Text = "Mauvaise réponse!";
                if (variables.currentPlayer != variables.Players)
                {
                    variables.currentPlayer++;
                }
                else
                {
                    variables.currentPlayer = 1;
                }

            }
            variables.answersNum.Clear();
        }

        private void BTN_CHOIX3_Click(object sender, EventArgs e)
        {
            DisableAnswers();
            OracleCommand cmd = new OracleCommand();

            //get question number ---- works
            cmd.Connection = objConn;
            cmd.CommandText = "GESTIONQUESTION.bonnereponse";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("estbonne", OracleDbType.Char, 1);
            cmd.Parameters["estbonne"].Direction = ParameterDirection.ReturnValue;

            cmd.Parameters.Add("numreponse", OracleDbType.Char);
            cmd.Parameters["numreponse"].Value = variables.answersNum[2];

            cmd.ExecuteNonQuery();
            string boss = cmd.Parameters["estbonne"].Value.ToString();

            if (boss == "Y")
            {
                label1.Text = "Bonne réponse!";
                switch (variables.currentPlayer)
                {
                    case 1:
                        progressBar1.PerformStep();
                        if (progressBar1.Value == 5)
                        {
                            Winner();
                        }
                        break;
                    case 2:
                        progressBar2.PerformStep();
                        if (progressBar2.Value == 5)
                        {
                            Winner();
                        }
                        break;
                    case 3:
                        progressBar3.PerformStep();
                        if (progressBar3.Value == 5)
                        {
                            Winner();
                        }
                        break;
                    case 4:
                        progressBar4.PerformStep();
                        if (progressBar4.Value == 5)
                        {
                            Winner();
                        }
                        break;
                }
            }
            else
            {
                label1.Text = "Mauvaise réponse!";
                if (variables.currentPlayer != variables.Players)
                {
                    variables.currentPlayer++;
                }
                else
                {
                    variables.currentPlayer = 1;
                }

            }
            variables.answersNum.Clear();
        }

        private void BTN_CHOIX4_Click(object sender, EventArgs e)
        {
            DisableAnswers();
            OracleCommand cmd = new OracleCommand();

            //get question number ---- works
            cmd.Connection = objConn;
            cmd.CommandText = "GESTIONQUESTION.bonnereponse";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("estbonne", OracleDbType.Char, 1);
            cmd.Parameters["estbonne"].Direction = ParameterDirection.ReturnValue;

            cmd.Parameters.Add("numreponse", OracleDbType.Char);
            cmd.Parameters["numreponse"].Value = variables.answersNum[3];

            cmd.ExecuteNonQuery();
            string boss = cmd.Parameters["estbonne"].Value.ToString();

            if (boss == "Y")
            {
                label1.Text = "Bonne réponse!";
                switch (variables.currentPlayer)
                {
                    case 1:
                        progressBar1.PerformStep();
                        if (progressBar1.Value == 5)
                        {
                            Winner();
                        }
                        break;
                    case 2:
                        progressBar2.PerformStep();
                        if (progressBar2.Value == 5)
                        {
                            Winner();
                        }
                        break;
                    case 3:
                        progressBar3.PerformStep();
                        if (progressBar3.Value == 5)
                        {
                            Winner();
                        }
                        break;
                    case 4:
                        progressBar4.PerformStep();
                        if (progressBar4.Value == 5)
                        {
                            Winner();
                        }
                        break;
                }
            }
            else
            {
                label1.Text = "Mauvaise réponse!";
                if (variables.currentPlayer != variables.Players)
                {
                    variables.currentPlayer++;
                }
                else
                {
                    variables.currentPlayer = 1;
                }

            }
            variables.answersNum.Clear();
        }

        private void PB_Green_Click(object sender, EventArgs e)
        {
            ShowQuestion("V");
            DisablePicBoxes();
        }

        private void PB_RED_Click(object sender, EventArgs e)
        {
            ShowQuestion("R");
            DisablePicBoxes();
        }

        private void PB_Blue_Click(object sender, EventArgs e)
        {
            ShowQuestion("B");
            DisablePicBoxes();

        }

        private void PB_Yellow_Click(object sender, EventArgs e)
        {
            ShowQuestion("J");
            DisablePicBoxes();
        }

    
    }
}
