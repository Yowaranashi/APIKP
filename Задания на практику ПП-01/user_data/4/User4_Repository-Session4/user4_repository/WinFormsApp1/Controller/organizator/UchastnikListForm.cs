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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsApp1.Controller.organizator
{
    public partial class UchastnikListForm : Form
    {
        public UchastnikListForm()
        {
            InitializeComponent();

            dataGridView1.ColumnCount = 8;
            dataGridView1.Columns[0].Name = "№";
            dataGridView1.Columns[1].Name = "Фамилия";
            dataGridView1.Columns[2].Name = "Имя";
            dataGridView1.Columns[3].Name = "Отчество";
            dataGridView1.Columns[4].Name = "Компетенция";
            dataGridView1.Columns[5].Name = "Роль";
            dataGridView1.Columns[6].Name = "Сатус согласования";
            dataGridView1.Columns[7].Name = "Согласование";

            setUchstnik();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void setUchstnik()
        {
            string connectionString = "Server=WM-SQL-SERVER\\SQLEXPRESS01;Database=db_bsn_user4;User ID=bsn_user4;Password=ItNTAX;TrustServerCertificate=True";
            SqlConnection connection = new SqlConnection(connectionString);
            string zapros = "Select * From dbo.Users;";
            connection.Open();
            SqlCommand sqlCommand = new SqlCommand(zapros, connection);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                int i = 0;
                while (reader.Read())
                {
                    dataGridView1.Rows[i].Cells[0].Value = reader["#"];
                    dataGridView1.Rows[i].Cells[1].Value = reader["FIO"].ToString().Split(' ')[0];
                    dataGridView1.Rows[i].Cells[2].Value = reader["FIO"].ToString().Split(' ')[1];
                    dataGridView1.Rows[i].Cells[3].Value = reader["FIO"].ToString().Split(' ')[2];
                    i++;
                }

            }
            connection.Close();
        }
    }
}
