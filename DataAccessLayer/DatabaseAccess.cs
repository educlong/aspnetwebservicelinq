using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace DataAccessLayer
{
    public class DatabaseAccess
    {
        protected string strConnection="Server=DESKTOP-M3M3TQ4\\SQLEXPRESS;Database=Db_ProductManagement;User Id=sa;pwd=1001";
        protected SqlConnection sqlConnection = null;
        public void OpenConnection()
        {
            if (sqlConnection == null)
                sqlConnection = new SqlConnection(strConnection);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }
        public void CloseConnection()
        {
            if (sqlConnection != null && sqlConnection.State == ConnectionState.Open)
                sqlConnection.Close();
        }
    }
}
