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
    public partial class FormExpert : Form
    {
        DataBase _dataBase = new DataBase();
        string idExpert = "";
        string FIO_Expert = "";
        string idCampionGlob = "";
        public FormExpert(string idEx, string FIO, string idRole, string idChamp)
        {
            InitializeComponent();
            idExpert = idEx;
            idCampionGlob = idChamp;
            FIO_Expert = FIO;
            StartPosition = FormStartPosition.CenterScreen;
            //Настройка времени
            if (DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 11)
            {
                labelInfoAboutPerson.Text = $"Доброе утро, {FIO} (Эксперт)";
            }
            else if (DateTime.Now.Hour >= 18 && DateTime.Now.Hour <= 23)
            {
                labelInfoAboutPerson.Text = $"Добрый вечер, {FIO} (Эксперт)";
            }
            else
            {
                labelInfoAboutPerson.Text = $"Добрый день, {FIO} (Эксперт)";
            }

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            string query = $"select * from users_mark where id_user = {idEx}";
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
            labelTitleComp.Text = "Компетенция: " + titleSkll;

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

        private void butListUchastnic_Click(object sender, EventArgs e)
        {
            panelExpert.Visible = false;
            panelUch.Visible = true;
            dataGVExpert.Columns.Clear();
            dataGVUchastniki.Columns.Clear();
            dataGVUchastniki.Columns.Add("id", "№");
            dataGVUchastniki.Columns.Add("Familiya", "Фамилия");
            dataGVUchastniki.Columns.Add("Name", "Имя");
            dataGVUchastniki.Columns.Add("Otchestvo", "Отчество");
            dataGVUchastniki.Columns.Add("DateBirth", "Дата рождения");
            dataGVUchastniki.Columns.Add("YearsOld", "Полных лет");
            dataGVUchastniki.Columns.Add("Status", "Статус подтверждения");

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            string query = $"select id_user, FIO, Data_birth from users_mark where championate = {idCampionGlob} and ID_role = 1";
            SqlCommand sqlCommand = new SqlCommand(query, _dataBase.getConnection());
            dataAdapter.SelectCommand = sqlCommand;
            dataAdapter.Fill(dataTable);
            string infoAboutUser = "";
            foreach (DataRow row in dataTable.Rows)
            {
                infoAboutUser = "";
                var cells = row.ItemArray;
                foreach (var cell in cells)
                {
                    infoAboutUser += cell + ",";
                }
                string[] infoUser = infoAboutUser.Split(',');
                string[] FIO = infoUser[1].Split(' ');
                string dateBirth = infoUser[2];

                int years = DateTime.Now.Year - DateTime.Parse(dateBirth).Year;
                
                dataGVUchastniki.Rows.Add(infoUser[0], FIO[0], FIO[1], FIO[2], infoUser[2], years);
            }
        }

        private void butListExpert_Click(object sender, EventArgs e)
        {
            panelExpert.Visible = true;
            panelUch.Visible = false;
            dataGVExpert.Columns.Clear();
            dataGVExpert.Columns.Add("id", "№");
            dataGVExpert.Columns.Add("Familiya", "Фамилия");
            dataGVExpert.Columns.Add("Name", "Имя");
            dataGVExpert.Columns.Add("Otchestvo", "Отчество");
            dataGVExpert.Columns.Add("FamilIO", "ФИО участника");
            dataGVExpert.Columns.Add("Status", "Статус подтверждения");

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            string query = $"select id_user, FIO from users_mark where championate = {idCampionGlob} and ID_role = 2";
            SqlCommand sqlCommand = new SqlCommand(query, _dataBase.getConnection());
            dataAdapter.SelectCommand = sqlCommand;
            dataAdapter.Fill(dataTable);
            string infoAboutUser = "";
            foreach (DataRow row in dataTable.Rows)
            {
                infoAboutUser = "";
                var cells = row.ItemArray;
                foreach (var cell in cells)
                {
                    infoAboutUser += cell + ",";
                }
                string[] infoUser = infoAboutUser.Split(',');
                string[] FIO = infoUser[1].Split(' ');
                string FamilIO = FIO[0] + " " + FIO[1][0] + "." + FIO[1][0];
                dataGVExpert.Rows.Add(infoUser[0], FIO[0], FIO[1], FIO[2], FamilIO);
            }
        }

        private void butQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
