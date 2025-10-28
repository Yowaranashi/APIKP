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
    public partial class FormUchastnikiForOrganizator : Form
    {
        DataBase _dataBase = new DataBase();
        int idChampGlob = 0;
        public FormUchastnikiForOrganizator(int idChamp)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            idChampGlob = idChamp;
        }

        private void FormUchastniki_Load(object sender, EventArgs e)
        {
            dataGVUchastniki.Columns.Add("id", "№");
            dataGVUchastniki.Columns.Add("Familiya", "Фамилия");
            dataGVUchastniki.Columns.Add("Name", "Имя");
            dataGVUchastniki.Columns.Add("Otchestvo", "Отчество");
            dataGVUchastniki.Columns.Add("Compet", "Компетенция");
            dataGVUchastniki.Columns.Add("Role", "Роль");
            dataGVUchastniki.Columns.Add("Status", "Статус согласования");

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            string query = $"select title from skill";
            SqlCommand sqlCommand = new SqlCommand(query, _dataBase.getConnection());
            dataAdapter.SelectCommand = sqlCommand;
            dataAdapter.Fill(dataTable);
            string infoComp = "";
            foreach (DataRow row in dataTable.Rows)
            {
                var cells = row.ItemArray;
                foreach (var cell in cells)
                {
                    infoComp += cell + ",";
                }
            }
            string[] competitions = infoComp.Split(',');
            comboBCompetition.Items.AddRange(competitions);

            dataAdapter = new SqlDataAdapter();
            dataTable = new DataTable();
            query = $"select role from role";
            sqlCommand = new SqlCommand(query, _dataBase.getConnection());
            dataAdapter.SelectCommand = sqlCommand;
            dataAdapter.Fill(dataTable);
            string infoRole = "";
            foreach (DataRow row in dataTable.Rows)
            {
                var cells = row.ItemArray;
                foreach (var cell in cells)
                {
                    infoRole += cell + ",";
                }
            }
            string[] rols = infoRole.Split(',');
            comboBRole.Items.AddRange(rols);

            dataAdapter = new SqlDataAdapter();
            dataTable = new DataTable();
            query = $"select id_user, FIO, ID_role, skill from users_mark where championate = {idChampGlob}";
            sqlCommand = new SqlCommand(query, _dataBase.getConnection());
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

                SqlDataAdapter dataAdapterRole = new SqlDataAdapter();
                DataTable dataTableRole = new DataTable();
                string queryForRole = $"select role from role where id_role = {infoUser[2]}";
                SqlCommand sqlCommandRole = new SqlCommand(queryForRole, _dataBase.getConnection());
                dataAdapterRole.SelectCommand = sqlCommandRole;
                dataAdapterRole.Fill(dataTableRole);
                string answer = "";
                foreach (DataRow rowRole in dataTableRole.Rows)
                {
                    var answerNonExtension = rowRole.ItemArray;
                    foreach (var cellRole in answerNonExtension)
                    {
                        answer = cellRole.ToString();
                    }
                }
                SqlDataAdapter dataAdapterCompet = new SqlDataAdapter();
                DataTable dataTableCompet = new DataTable();
                string queryForCompet = $"select title from skill where id = {infoUser[3]}";
                SqlCommand sqlCommandCompet = new SqlCommand(queryForCompet, _dataBase.getConnection());
                dataAdapterCompet.SelectCommand = sqlCommandCompet;
                dataAdapterCompet.Fill(dataTableCompet);
                string compet = "";
                foreach (DataRow rowCompet in dataTableCompet.Rows)
                {
                    var answerNonExtension = rowCompet.ItemArray;
                    foreach (var cellCompet in answerNonExtension)
                    {
                        compet = cellCompet.ToString();
                    }
                }
                dataGVUchastniki.Rows.Add(infoUser[0], FIO[0], FIO[1], FIO[2], compet, answer);
            }

        }

        private void comboBCompetition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBFilter.Checked)
            {
                for (int i = 0; i < dataGVUchastniki.Rows.Count; i++)
                {
                    dataGVUchastniki.Rows[i].Visible = true;
                }
                for (int i = 0; i < dataGVUchastniki.Rows.Count; i++)
                {
                    if (dataGVUchastniki.Rows[i].Cells[4].Value.ToString() != comboBCompetition.Text)
                    {
                        dataGVUchastniki.Rows[i].Visible = false;
                    }
                    if (dataGVUchastniki.Rows[i].Cells[5].Value.ToString() != comboBRole.Text)
                    {
                        dataGVUchastniki.Rows[i].Visible = false;
                    }
                }
            }
        }

        private void comboBRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBFilter.Checked)
            {
                for (int i = 0; i < dataGVUchastniki.Rows.Count; i++)
                {
                    dataGVUchastniki.Rows[i].Visible = true;
                }
                for (int i = 0; i < dataGVUchastniki.Rows.Count; i++)
                {
                    if (dataGVUchastniki.Rows[i].Cells[4].Value.ToString() != comboBCompetition.Text)
                    {
                        dataGVUchastniki.Rows[i].Visible = false;
                    }
                    if (dataGVUchastniki.Rows[i].Cells[5].Value.ToString() != comboBRole.Text)
                    {
                        dataGVUchastniki.Rows[i].Visible = false;
                    }
                }
            }
        }

        private void checkBFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBFilter.Checked)
            {
                for (int i = 0; i < dataGVUchastniki.Rows.Count; i++)
                {
                    dataGVUchastniki.Rows[i].Visible = true;
                }
            }
            else
            {
                for (int i = 0; i < dataGVUchastniki.Rows.Count; i++)
                {
                    if (dataGVUchastniki.Rows[i].Cells[4].Value.ToString() != comboBCompetition.Text)
                    {
                        dataGVUchastniki.Rows[i].Visible = false;
                    }
                    if (dataGVUchastniki.Rows[i].Cells[5].Value.ToString() != comboBRole.Text)
                    {
                        dataGVUchastniki.Rows[i].Visible = false;
                    }
                }
            }
        }
    }
}
