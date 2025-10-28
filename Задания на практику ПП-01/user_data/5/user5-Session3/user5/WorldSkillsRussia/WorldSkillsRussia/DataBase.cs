using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WorldSkillsRussia
{
    class DataBase
    {
        SqlConnection _sqlConnection = new SqlConnection(@"Data Source=WM-SQL-SERVER\SQLEXPRESS01;Initial Catalog=db_bsn_user5;Persist Security Info=True;User ID=bsn_user5;Password=MVOYdY");

        public void openConnection()
        {
            if (_sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                _sqlConnection.Open();
            }
        }
        public void closeConnection()
        {
            if (_sqlConnection.State == System.Data.ConnectionState.Open)
            {
                _sqlConnection.Close();
            }
        }
        public SqlConnection getConnection()
        {
            return _sqlConnection;
        }
    }
}
