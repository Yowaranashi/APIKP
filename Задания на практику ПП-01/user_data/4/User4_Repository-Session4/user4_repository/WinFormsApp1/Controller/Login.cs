using Microsoft.Data.SqlClient;
using WinFormsApp1.config;
using WinFormsApp1.Controller.expert;
using WinFormsApp1.Controller.organizator;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WinFormsApp1
{
    public partial class Login : Form
    {

        public static Session Session { get; set; }
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "Server=WM-SQL-SERVER\\SQLEXPRESS01;Database=db_bsn_user4;User ID=bsn_user4;Password=ItNTAX;TrustServerCertificate=True";
                SqlConnection connection = new SqlConnection(connectionString);
                string zapros = "Select * From dbo.Users Where PIN=" + textBox1.Text + ";";
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(zapros, connection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    string password = (string)reader["password"];
                    if (password == textBox2.Text)
                    {
                        if (Convert.ToInt32(reader["ID role"]) == 6)
                        {
                            Session = new Session(this);
                            Session.UserFIO = (string)reader["FIO"];
                            OrganizatorForm form = new OrganizatorForm();
                            form.ShowDialog();
                        }
                        if (Convert.ToInt32(reader["ID role"]) == 2)
                        {
                            ExpertFormOpen(reader);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль");
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверный логин или пароль");
            }

        }

        public void ExpertFormOpen(SqlDataReader reader)
        {
            if (reader["чемпионат"] == null)
            {
                MessageBox.Show("Эксперт не учавствует ни в одном чемпионате");
            }
            Session = new Session(this);
            Session.UserFIO = (string)reader["FIO"];

            string connectionString = "Server=WM-SQL-SERVER\\SQLEXPRESS01;Database=db_bsn_user4;User ID=bsn_user4;Password=ItNTAX;TrustServerCertificate=True";
            SqlConnection connection = new SqlConnection(connectionString);
            string zapros = "Select Skill.title as SkillTit, Competition.title as CompetitionTit, Competition.id as id From Users, Competition, Skill Where Users.PIN = " + textBox1.Text + " and Users.skill = Skill.id and Users.чемпионат = Competition.id;";
            connection.Open();
            SqlCommand sqlCommand = new SqlCommand(zapros, connection);
            SqlDataReader reader1 = sqlCommand.ExecuteReader();
            reader1.Read();
            Session.SkillName = (string)reader1["SkillTit"];
            Session.CempionatName = (string)reader1["CompetitionTit"];
            Session.CempoinatId = Convert.ToInt32(reader1["id"]);
            ExpertForm form = new ExpertForm();
            form.ShowDialog();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}