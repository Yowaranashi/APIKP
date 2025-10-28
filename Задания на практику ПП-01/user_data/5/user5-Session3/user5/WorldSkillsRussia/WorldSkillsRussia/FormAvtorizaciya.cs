using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldSkillsRussia
{
    public partial class FormAvtorizaciya : Form
    {
        DataBase _dataBase = new DataBase();
        public FormAvtorizaciya()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void butLogin_Click(object sender, EventArgs e)
        {
            if (tBLogin.Text == "" || tBPassword.Text == "")
            {
                MessageBox.Show("Ошибка! Некоторые поля не заполнены.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int PIN = 0;
            if (!int.TryParse(tBLogin.Text, out PIN))
            {
                MessageBox.Show("Ошибка! Введена строка, ожидалось число.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            string query = $"select * from users_mark where PIN = '{PIN}' and [password] = '{tBPassword.Text}'";

            SqlCommand sqlCommand = new SqlCommand(query, _dataBase.getConnection());
            dataAdapter.SelectCommand = sqlCommand;
            dataAdapter.Fill(dataTable);
            if (dataTable.Rows.Count == 1)
            {
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
                string iduser = ComponentsUser[0];
                string FIO = ComponentsUser[1];
                string idRole = ComponentsUser[6];
                string idChamp = ComponentsUser[10];
                MessageBox.Show("Здравствуйте, " + FIO + ".", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (!checkBRemMe.Checked)
                {
                    tBLogin.Text = "";
                    tBPassword.Text = "";
                }
                this.Hide();
                if (int.Parse(idRole) == 6)
                {
                    FormOrganizator fromOrgan = new FormOrganizator(FIO, idRole);
                    fromOrgan.ShowDialog();
                }
                if (int.Parse(idRole) == 2 || int.Parse(idRole) == 3 || int.Parse(idRole) == 4 || int.Parse(idRole) == 5)
                {
                    FormExpert formExpert = new FormExpert(iduser, FIO, idRole, idChamp);
                    try
                    {
                        formExpert.ShowDialog();
                    }
                    catch
                    {
                        MessageBox.Show("У Вас нет ближайших чемпионатов.");
                    }
                }
                if (int.Parse(idRole) == 1)
                {
                    FormInfoUchastniki formInfoUchastniki = new FormInfoUchastniki(iduser, FIO);
                    try
                    {
                        formInfoUchastniki.ShowDialog();
                    }
                    catch
                    {
                        MessageBox.Show("У Вас нет ближайших чемпионатов. Обратитесь за информацией к организатору.");
                    }
                }
                this.Show();
            }
            else
            {
                MessageBox.Show("Пользователь не найдет.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }
    }
}
