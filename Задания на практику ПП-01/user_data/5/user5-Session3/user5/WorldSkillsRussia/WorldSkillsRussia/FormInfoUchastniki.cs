using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WorldSkillsRussia
{
    public partial class FormInfoUchastniki : Form
    {
        DataBase _dataBase = new DataBase();
        string iduserGlob;
        string FIO_Glob;
        public FormInfoUchastniki(string iduser, string FIO)
        {
            InitializeComponent();
            iduserGlob = iduser;
            FIO_Glob = FIO;
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            string query = $"select * from users_mark where id_user = {iduser}";
            SqlCommand sqlCommand = new SqlCommand(query, _dataBase.getConnection());
            dataAdapter.SelectCommand = sqlCommand;
            dataAdapter.Fill(dataTable);
            string infoExpert = "";
            foreach (DataRow row in dataTable.Rows)
            {
                var cells = row.ItemArray;
                foreach (var cell in cells)
                {
                    infoExpert += cell + ",";
                }
            }
            string[] fullInfoAboutExpert = infoExpert.Split(',');
            string skill = fullInfoAboutExpert[7];
            string championat = fullInfoAboutExpert[10];

            dataAdapter = new SqlDataAdapter();
            dataTable = new DataTable();
            query = $"select * from skill where id = {skill}";
            sqlCommand = new SqlCommand(query, _dataBase.getConnection());
            dataAdapter.SelectCommand = sqlCommand;
            dataAdapter.Fill(dataTable);
            string infoSkill = "";
            foreach (DataRow row in dataTable.Rows)
            {
                var cells = row.ItemArray;
                foreach (var cell in cells)
                {
                    infoSkill += cell + ",";
                }
            }
            string[] fullInfoAboutskill = infoSkill.Split(',');
            string titleSkll = fullInfoAboutskill[1];
            labelTitleSkill.Text = "Компетенция: " + titleSkll;

            dataAdapter = new SqlDataAdapter();
            dataTable = new DataTable();
            query = $"select * from competition where id = {championat}";
            sqlCommand = new SqlCommand(query, _dataBase.getConnection());
            dataAdapter.SelectCommand = sqlCommand;
            if (championat == "" || championat == null)
            {
                this.Close();
                return;
            }
            dataAdapter.Fill(dataTable);
            string infoCompetition = "";
            foreach (DataRow row in dataTable.Rows)
            {
                var cells = row.ItemArray;
                foreach (var cell in cells)
                {
                    infoCompetition += cell + ",";
                }
            }
            string[] fullInfoAboutCompet = infoCompetition.Split(',');
            string titleChampionate = fullInfoAboutCompet[1];
            labelTitleChamp.Text = titleChampionate;
        }

        private void butBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butVozrastCenz_Click(object sender, EventArgs e)
        {
            VozrastnCenzForm vozrastnCenzForm = new VozrastnCenzForm(labelTitleSkill.Text, labelTitleChamp.Text);
            vozrastnCenzForm.ShowDialog();
        }
    }
}
