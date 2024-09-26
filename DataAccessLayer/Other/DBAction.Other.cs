using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace ITCLib
{
    /// <summary>
    /// Procedures that are not strictly SELECT or single INSERT, UPDATE or DELETE statements
    /// </summary>
    public static partial class DBAction
    {
        public static int CopySurvey(string source, string destination)
        {
            string sql = "proc_copySurvey";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@survey", source);
            parameters.Add("@newSurvey", destination);

            int rowsAffected;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }

            if (rowsAffected == 0)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Copies comments for a certain question to the 'deleted' comments table.
        /// </summary>
        /// <param name="QID"></param>
        /// <returns></returns>
        public static int BackupComments(int QID)
        {
            string sql = "proc_backupComments";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@QID", QID);

            int rowsAffected;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }

            if (rowsAffected == 0)
                return 1;
            else
                return 0;
        }
    }
}
