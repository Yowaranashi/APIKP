using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1.Controller.expert
{
    public partial class ExpertForm : Form
    {
        public ExpertForm()
        {
            InitializeComponent();

            label1.Text = Login.Session.CempionatName + "\nКомпетенция: " + Login.Session.SkillName;
            string wellcomeText;

            if (DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 11)
            {
                wellcomeText = "Доброе утро, ";
            }
            else if (DateTime.Now.Hour >= 11 && DateTime.Now.Hour <= 17)
            {
                wellcomeText = "Добрый день, ";
            }
            else
            {
                wellcomeText = "Доброй ночи, ";
            }
            wellcomeText += Login.Session.UserFIO; //фио организатора
            label2.Text = wellcomeText + "\nДень С-2";

            groupBox1.Visible = false;
            groupBox3.Visible = false;
            groupBox2.Visible = false;
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox6.Visible = false;
            groupBox7.Visible = false;
            groupBox8.Visible = false;
            groupBox9.Visible = false;
            ViewUchast();
            ViewExpert();
            ViewExpert2();
        }

        public void ViewUchast()
        {
            dataGridView2.ColumnCount = 7;
            dataGridView2.Columns[0].Name = "№";
            dataGridView2.Columns[1].Name = "Фамилия";
            dataGridView2.Columns[2].Name = "Имя";
            dataGridView2.Columns[3].Name = "Отчество";
            dataGridView2.Columns[4].Name = "Дата рождения";
            dataGridView2.Columns[5].Name = "Полных лет";
            dataGridView2.Columns[6].Name = "Статус подтверждения";

            string connectionString = "Server=WM-SQL-SERVER\\SQLEXPRESS01;Database=db_bsn_user4;User ID=bsn_user4;Password=ItNTAX;TrustServerCertificate=True";
            SqlConnection connection = new SqlConnection(connectionString);
            string zapros = "Select * From dbo.Users Where чемпионат = " + Login.Session.CempoinatId + " and [ID role] = 1;";
            connection.Open();
            SqlCommand sqlCommand = new SqlCommand(zapros, connection);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                int i = 0;
                while (reader.Read())
                {
                    dataGridView2.RowCount++;
                    dataGridView2.Rows[i].Cells[0].Value = reader["#"];
                    dataGridView2.Rows[i].Cells[1].Value = reader["FIO"].ToString().Split(' ')[0];
                    dataGridView2.Rows[i].Cells[2].Value = reader["FIO"].ToString().Split(' ')[1];
                    dataGridView2.Rows[i].Cells[3].Value = reader["FIO"].ToString().Split(' ')[2];
                    dataGridView2.Rows[i].Cells[4].Value = reader["Дата рождения"].ToString();
                    dataGridView2.Rows[i].Cells[5].Value = DateTime.Now.Year - Convert.ToDateTime(reader["Дата рождения"]).Year;
                    i++;
                }

            }
            connection.Close();
        }

        public void ViewExpert()
        {
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "№";
            dataGridView1.Columns[1].Name = "Фамилия";
            dataGridView1.Columns[2].Name = "Имя";
            dataGridView1.Columns[3].Name = "Отчество";
            dataGridView1.Columns[4].Name = "ФИО участника";
            dataGridView1.Columns[5].Name = "Статус подтверждения";

            string connectionString = "Server=WM-SQL-SERVER\\SQLEXPRESS01;Database=db_bsn_user4;User ID=bsn_user4;Password=ItNTAX;TrustServerCertificate=True";
            SqlConnection connection = new SqlConnection(connectionString);
            string zapros = "Select * From dbo.Users Where чемпионат = " + Login.Session.CempoinatId + " and [ID role] = 2;";
            connection.Open();
            SqlCommand sqlCommand = new SqlCommand(zapros, connection);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                int i = 0;
                while (reader.Read())
                {
                    dataGridView1.RowCount++;
                    dataGridView1.Rows[i].Cells[0].Value = reader["#"];
                    dataGridView1.Rows[i].Cells[1].Value = reader["FIO"].ToString().Split(' ')[0];
                    dataGridView1.Rows[i].Cells[2].Value = reader["FIO"].ToString().Split(' ')[1];
                    dataGridView1.Rows[i].Cells[3].Value = reader["FIO"].ToString().Split(' ')[2];
                    i++;
                }

            }
            connection.Close();
        }

        public void ViewExpert2()
        {
            dataGridView3.ColumnCount = 2;
            dataGridView4.AutoResizeColumns();
            dataGridView3.Columns[0].Name = "Эксперт";
            dataGridView3.Columns[1].Name = "Дата рождения";

            dataGridView4.ColumnCount = 2;
            dataGridView4.AutoResizeColumns();
            dataGridView4.Columns[0].Name = "Эксперт";
            dataGridView4.Columns[1].Name = "Дата рождения";

            dataGridView5.ColumnCount = 2;
            dataGridView5.AutoResizeColumns();
            dataGridView5.Columns[0].Name = "Эксперт";
            dataGridView5.Columns[1].Name = "Дата рождения";

            dataGridView6.ColumnCount = 2;
            dataGridView6.AutoResizeColumns();
            dataGridView6.Columns[0].Name = "Эксперт";
            dataGridView6.Columns[1].Name = "Дата рождения";

            string connectionString = "Server=WM-SQL-SERVER\\SQLEXPRESS01;Database=db_bsn_user4;User ID=bsn_user4;Password=ItNTAX;TrustServerCertificate=True";
            SqlConnection connection = new SqlConnection(connectionString);
            string zapros = "Select * From dbo.Users Where чемпионат = " + Login.Session.CempoinatId + " and [ID role] = 2;";
            connection.Open();
            SqlCommand sqlCommand = new SqlCommand(zapros, connection);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                int i = 0;
                while (reader.Read())
                {
                    dataGridView3.RowCount++;
                    dataGridView3.Rows[i].Cells[0].Value = reader["FIO"];
                    dataGridView3.Rows[i].Cells[1].Value = reader["дата рождения"].ToString();

                    dataGridView4.RowCount++;
                    dataGridView4.Rows[i].Cells[0].Value = reader["FIO"];
                    dataGridView4.Rows[i].Cells[1].Value = reader["дата рождения"].ToString();

                    dataGridView5.RowCount++;
                    dataGridView5.Rows[i].Cells[0].Value = reader["FIO"];
                    dataGridView5.Rows[i].Cells[1].Value = reader["дата рождения"].ToString();

                    dataGridView6.RowCount++;
                    dataGridView6.Rows[i].Cells[0].Value = reader["FIO"];
                    dataGridView6.Rows[i].Cells[1].Value = reader["дата рождения"].ToString();
                    i++;
                }

            }
            connection.Close();
        }
        private void expertUprav_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox3.Visible = false;
            groupBox2.Visible = false;
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox6.Visible = false;
            groupBox7.Visible = false;
            groupBox8.Visible = false;
            groupBox1.Visible = true;
            groupBox9.Visible = false;
        }

        private void chempionatOptions_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox3.Visible = false;
            groupBox2.Visible = false;
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox6.Visible = false;
            groupBox7.Visible = false;
            groupBox8.Visible = false;
            groupBox2.Visible = true;
            groupBox9.Visible = false;
        }

        private void chempionat_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox3.Visible = false;
            groupBox2.Visible = false;
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox6.Visible = false;
            groupBox7.Visible = false;
            groupBox8.Visible = false;
            groupBox3.Visible = true;
            groupBox9.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox3.Visible = false;
            groupBox2.Visible = false;
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox6.Visible = false;
            groupBox7.Visible = false;
            groupBox8.Visible = false;
            groupBox5.Visible = true;
            groupBox9.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox3.Visible = false;
            groupBox2.Visible = false;
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox6.Visible = false;
            groupBox7.Visible = false;
            groupBox8.Visible = false;
            groupBox4.Visible = true;
            groupBox9.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox3.Visible = false;
            groupBox2.Visible = false;
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox6.Visible = false;
            groupBox7.Visible = false;
            groupBox8.Visible = false;
            groupBox6.Visible = true;
            groupBox9.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox3.Visible = false;
            groupBox2.Visible = false;
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox6.Visible = false;
            groupBox7.Visible = false;
            groupBox8.Visible = false;
            groupBox7.Visible = true;
            groupBox9.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox3.Visible = false;
            groupBox2.Visible = false;
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox6.Visible = false;
            groupBox7.Visible = false;
            groupBox8.Visible = false;
            groupBox8.Visible = true;
            groupBox9.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox3.Visible = false;
            groupBox2.Visible = false;
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox6.Visible = false;
            groupBox7.Visible = false;
            groupBox8.Visible = false;
            groupBox9.Visible = true;
        }
    }
}
