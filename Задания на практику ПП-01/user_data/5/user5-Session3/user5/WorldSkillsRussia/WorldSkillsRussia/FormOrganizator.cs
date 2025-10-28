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
    public partial class FormOrganizator : Form
    {
        int currentChamp = 0;
        DataBase _dataBase = new DataBase();
        public FormOrganizator(string FIO, string role)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            //Настройка времени
            if (DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 11)
            {
                labelInfoAboutPerson.Text = $"Доброе утро, {FIO} (организатор)";
            }
            else if (DateTime.Now.Hour >= 18 && DateTime.Now.Hour <= 23)
            {
                labelInfoAboutPerson.Text = $"Добрый вечер, {FIO} (организатор)";
            }
            else
            {
                labelInfoAboutPerson.Text = $"Добрый день, {FIO} (организатор)";
            }
        }

        private void butQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormOrganizator_Load(object sender, EventArgs e)
        {
            //Выборка всех доступных чампионатов
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            string query = $"select title from competition";

            SqlCommand sqlCommand = new SqlCommand(query, _dataBase.getConnection());
            dataAdapter.SelectCommand = sqlCommand;
            dataAdapter.Fill(dataTable);
            string allChamps = "";
            foreach (DataRow row in dataTable.Rows)
            {
                var cells = row.ItemArray;
                foreach (var cell in cells)
                {
                    allChamps += cell + ",";
                }
            }
            string[] champs = allChamps.Split(',');
            comboBTitleChamp.Items.AddRange(champs);

            //Выборка всех доступных компетенций 
            dataAdapter = new SqlDataAdapter();
            dataTable = new DataTable();
            query = $"select title from skill";
            sqlCommand = new SqlCommand(query, _dataBase.getConnection());
            dataAdapter.SelectCommand = sqlCommand;
            dataAdapter.Fill(dataTable);
            string allSkills = "";
            foreach (DataRow row in dataTable.Rows)
            {
                var cells = row.ItemArray;
                foreach (var cell in cells)
                {
                    allSkills += cell + ",";
                }
            }
            string[] skills = allSkills.Split(',');
            comboBCompetition.Items.AddRange(skills);
        }

        private void comboBTitleChamp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Выборка и вывод всей информации о выбранном пользователем чемпионате
            string championateName = comboBTitleChamp.Text;
            tBNameChamp.Text = championateName;
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            string query = $"select * from competition where title = '{championateName}'";
            SqlCommand sqlCommand = new SqlCommand(query, _dataBase.getConnection());
            dataAdapter.SelectCommand = sqlCommand;
            dataAdapter.Fill(dataTable);
            string allInfo = "";
            foreach (DataRow row in dataTable.Rows)
            {
                var cells = row.ItemArray;
                foreach (var cell in cells)
                {
                    allInfo += cell + ",";
                }
            }
            string[] ComponentsUser = allInfo.Split(',');
            int idChamp = int.Parse(ComponentsUser[0]);
            currentChamp = idChamp;
            DateTime dateStart = DateTime.Parse(ComponentsUser[2].ToString());
            DateTime dateEnd = DateTime.Parse(ComponentsUser[3].ToString());
            dateTPChampStart.Value = dateStart;
            dateTPChampEnd.Value = dateEnd;

            //Выборка информации о компетенции для выбранного чемпионата
            query = $"select top 1 skill_id from competition_skill where competition_id = {idChamp}";
            sqlCommand = new SqlCommand(query, _dataBase.getConnection());
            dataAdapter.SelectCommand = sqlCommand;
            dataAdapter.Fill(dataTable);
            string skillInfo = "";
            foreach (DataRow row in dataTable.Rows)
            {
                var cells = row.ItemArray;
                foreach (var cell in cells)
                {
                    skillInfo += cell + ",";
                }
            }
            string[] skills = skillInfo.Split(',');
            int id_skill = int.Parse(skills[0]);
            
            query = $"select id, title, [количество участников в команде], [для расчетов], [площадь на рабочее место (кв#м)], " +
                $"[площадь комнаты оценки на одну экспертную группу (кв# м)], " +
                $"[площадь склада (кв# м)], " +
                $"[зона брифинга (кв# м)] from skill where id = '{id_skill}'";
            sqlCommand = new SqlCommand(query, _dataBase.getConnection());
            dataAdapter.SelectCommand = sqlCommand;
            DataTable dataTableSkill = new DataTable();
            dataAdapter.Fill(dataTableSkill);
            string infoAboutSkill = "";
            foreach (DataRow row in dataTableSkill.Rows)
            {
                var cells = row.ItemArray;
                foreach (var cell in cells)
                {
                    infoAboutSkill += cell + ",";
                }
            }
            string[] newSkills = infoAboutSkill.Split(',');
            string titile = newSkills[1];
            comboBCompetition.Text = titile;
            numericUDUchastnikov.Value = decimal.Parse(newSkills[2]);
            numericUDExperts.Value = decimal.Parse(newSkills[3]);
            decimal sq = decimal.Parse(newSkills[4]) * numericUDUchastnikov.Value + decimal.Parse(newSkills[5]) + decimal.Parse(newSkills[6])
                + decimal.Parse(newSkills[7]) * 2;
            numericUDSq.Value = sq;
        }

        private void butChampionate_Click(object sender, EventArgs e)
        {
            if (currentChamp == 0)
            {
                MessageBox.Show("Чемпионат не выбран", "Ошибка!", MessageBoxButtons.OK);
                return;
            }
            else
            {
                FormUchastnikiForOrganizator formUchastniki = new FormUchastnikiForOrganizator(currentChamp);
                formUchastniki.ShowDialog();
                this.Activate();
            }
        }

        private void butOptionChampionate_Click(object sender, EventArgs e)
        {
            if (currentChamp == 0)
            {
                MessageBox.Show("Чемпионат не выбран", "Ошибка!", MessageBoxButtons.OK);
                return;
            }
            else
            {
                FormSettingChamp formSettingChampFormSettingChamp = new FormSettingChamp(currentChamp);
                formSettingChampFormSettingChamp.ShowDialog();
            }
        }
    }
}
