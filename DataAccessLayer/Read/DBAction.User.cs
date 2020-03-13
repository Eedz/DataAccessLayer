using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ITCLib
{
    public static partial class DBAction
    {
        //
        // Users
        //

        /// <summary>
        /// Returns the user profile for the specified username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static UserPrefs GetUser(string username)
        {
            UserPrefs u;
            string query = "SELECT * FROM Users.FN_GetUserPrefs (@username)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@username", username);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        rdr.Read();
                        u = new UserPrefs
                        {
                            userid = (int)rdr["PersonnelID"],
                            Username = (string)rdr["username"],
                            accessLevel = (AccessLevel)rdr["AccessLevel"],
                            ReportPath = (string)rdr["ReportFolder"],
                            reportPrompt = (bool)rdr["ReportPrompt"],
                            wordingNumbers = (bool)rdr["WordingNumbers"],
                            commentDetails = (int)rdr["CommentDetails"]
                        };
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
            
            return u;
        }

        /// <summary>
        /// Saves a filter for the provided user.
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="filter"></param>
        /// <param name="position"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int SaveSession(string formName, string filter, int position, UserPrefs user)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();
                sql.UpdateCommand = new SqlCommand("proc_saveSession", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@formname", formName);
                sql.UpdateCommand.Parameters.AddWithValue("@filter", filter);
                sql.UpdateCommand.Parameters.AddWithValue("@position", position);
                sql.UpdateCommand.Parameters.AddWithValue("@personnelID", user.userid);

                try
                {
                    sql.UpdateCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;

        }

        //
        // Fill Methods
        //

        /// <summary>
        /// Populates the Survey Entry filters for the provided user.
        /// </summary>
        /// <param name="user"></param>
        public static void FillUserSurveyFilters(UserPrefs user)
        {
  
            string query = "SELECT * FROM Users.FN_GetUserFilters(@id)";
            string se1 = "4C1"; string seb1 = "4C1"; string seg1 = "4C1";
            string se2 = "4C1"; string seb2 = "4C1"; string seg2 = "4C1";
            string se3 = "4C1"; string seb3 = "4C1"; string seg3 = "4C1";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@id", user.userid);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if ((string)rdr["FormName"] == "frmSurveyEntry")
                                se1 = (string)rdr["Filter"];
                            else if ((string)rdr["FormName"] == "frmSurveyEntry2")
                                se2 = (string)rdr["Filter"];
                            else if ((string)rdr["FormName"] == "frmSurveyEntry3")
                                se3 = (string)rdr["Filter"];

                            if ((string)rdr["FormName"] == "sfrmSurveyEntryBrown")
                                seb1 = (string)rdr["Filter"];
                            else if ((string)rdr["FormName"] == "sfrmSurveyEntryBrown2")
                                seb2 = (string)rdr["Filter"];
                            else if ((string)rdr["FormName"] == "sfrmSurveyEntryBrown3")
                                seb3 = (string)rdr["Filter"];

                            if ((string)rdr["FormName"] == "sfrmSurveyEntryGreen")
                                seg1 = (string)rdr["Filter"];
                            else if ((string)rdr["FormName"] == "sfrmSurveyEntryGreen2")
                                seg2 = (string)rdr["Filter"];
                            else if ((string)rdr["FormName"] == "sfrmSurveyEntryGreen3")
                                seg3 = (string)rdr["Filter"];


                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            user.SurveyEntryCodes.Add(se1);
            user.SurveyEntryCodes.Add(se2);
            user.SurveyEntryCodes.Add(se3);

            user.SurveyEntryBrown.Add(seb1);
            user.SurveyEntryBrown.Add(seb2);
            user.SurveyEntryBrown.Add(seb3);

            user.SurveyEntryGreen.Add(seg1);
            user.SurveyEntryGreen.Add(seg2);
            user.SurveyEntryGreen.Add(seg3);

        }

        
        
    }
}
