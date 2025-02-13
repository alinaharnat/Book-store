using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Bookstore.Forms
{
     class DataBase
    {
        
        private string connectionString = @"Data Source=DESKTOP-SEJLSPK; Initial Catalog=BookstoreDB; Integrated Security=True";

        public SqlConnection getSqlConnection()
        {
            return new SqlConnection(connectionString);
        }
       
        public void openConnection() 

        {
            var sqlConnection = new SqlConnection(connectionString);
            if (sqlConnection.State == System.Data.ConnectionState.Closed) 
            {
                sqlConnection.Open();
            }
        }

        public void closeConnection()
        {
            var sqlConnection = new SqlConnection(connectionString);
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }
      
    }
}
