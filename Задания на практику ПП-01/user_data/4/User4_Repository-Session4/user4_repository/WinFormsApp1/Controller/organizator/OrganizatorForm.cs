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
using WinFormsApp1.config;

namespace WinFormsApp1.Controller.organizator
{
    public partial class OrganizatorForm : Form
    {
        public OrganizatorForm()
        {
            InitializeComponent();
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
            wellcome.Text = wellcomeText;



            chempionat.BackColor = Color.Gray;
            chempionatOptions.BackColor = Color.Gray;
            expertUprav.BackColor = Color.Gray;
            Protokol.BackColor = Color.Gray;

            groupBox1.Visible = false;

            setCempionats();
        }

        public void setCempionats()
        {
            string connectionString = "Server=WM-SQL-SERVER\\SQLEXPRESS01;Database=db_bsn_user4;User ID=bsn_user4;Password=ItNTAX;TrustServerCertificate=True";
            SqlConnection connection = new SqlConnection(connectionString);
            string zapros = "Select * From dbo.Competition;";
            connection.Open();
            SqlCommand sqlCommand = new SqlCommand(zapros, connection);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    comboBox3.Items.Add((string)reader["title"]);
                }

            }
            connection.Close();
        }

        private void chempionat_Click(object sender, EventArgs e)
        {
            chempionat.BackColor = Color.BlueViolet;
            chempionatOptions.BackColor = Color.Gray;
            expertUprav.BackColor = Color.Gray;
            Protokol.BackColor = Color.Gray;
            groupBox1.Visible = true;
        }

        private void chempionatOptions_Click(object sender, EventArgs e)
        {
            chempionat.BackColor = Color.Gray;
            chempionatOptions.BackColor = Color.BlueViolet;
            expertUprav.BackColor = Color.Gray;
            Protokol.BackColor = Color.Gray;
            groupBox1.Visible = false;
        }

        private void expertUprav_Click(object sender, EventArgs e)
        {
            chempionat.BackColor = Color.Gray;
            chempionatOptions.BackColor = Color.Gray;
            expertUprav.BackColor = Color.BlueViolet;
            Protokol.BackColor = Color.Gray;
            groupBox1.Visible = false;
        }

        private void Protokol_Click(object sender, EventArgs e)
        {
            chempionat.BackColor = Color.Gray;
            chempionatOptions.BackColor = Color.Gray;
            expertUprav.BackColor = Color.Gray;
            Protokol.BackColor = Color.BlueViolet;
            groupBox1.Visible = false;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Login.Session.CempionatName = comboBox3.Text;
            ChempionatForm form = new ChempionatForm();
            form.ShowDialog();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void exit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
