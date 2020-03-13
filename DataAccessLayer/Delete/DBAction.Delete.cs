using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ITCLib
{
    public static partial class DBAction
    {
        public static int DeleteQuestion (string varname, string survey)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteQuestion", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.DeleteCommand.Parameters.AddWithValue("@survey", survey);
                sql.DeleteCommand.Parameters.AddWithValue("@varname", varname);
                
                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
            
        }


        public static int DeleteRegion (int regionID)
        {
            return 1;
        }

        public static int DeleteStudy(int studyID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteStudy", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.DeleteCommand.Parameters.AddWithValue("@studyID", studyID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;

        }


        public static int DeleteNote(int noteID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteNote", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.DeleteCommand.Parameters.AddWithValue("@sID", noteID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;

        }

        public static int DeleteSurveyCheckRecord(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteSurveyCheck", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;

        }

        public static int DeleteSurveyCheckRef(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteSurveyCheckRefSurv", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;

        }
    }
}
