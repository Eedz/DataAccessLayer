using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ITCLib;

namespace DataAccessLayer
{
    public static class DBActionOLD
    {
#if DEBUG
        static string connectionString = ConfigurationManager.ConnectionStrings["ISISConnectionStringTest"].ConnectionString;
#else
        static string connectionString = ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString;
#endif
       
       

      

      
    }
}
