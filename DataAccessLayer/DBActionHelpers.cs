using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ITCLib
{
    public static class DBActionHelpers
    {
        public static bool IsServerConnected()
        {
            using (var conn = new SqlConnection(DBAction.GetConnectionString()))
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
    }
}
