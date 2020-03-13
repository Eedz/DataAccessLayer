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
    partial class DBAction
    {
        //
        // Surveys
        //

        /// <summary>
        /// Returns the list of survey codes in the database in alpha order.
        /// </summary>
        /// <returns></returns>
        public static List<Survey> GetAllSurveys()
        {
            List<Survey> surveys = new List<Survey>();
            string query = "SELECT Survey FROM Surveys.FN_ListAllSurveys() ORDER BY ISO_Code, Wave, Survey";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            surveys.Add(GetSurveyInfo((string)rdr["Survey"]));
                        }
                    }
                }
                catch (Exception)
                {

                }

            }

            return surveys;
        }

        /// <summary>
        /// Returns the list of survey codes in the specified wave in alpha order.
        /// </summary>
        /// <param name="waveID"></param>
        /// <returns></returns>
        public static List<Survey> GetSurveys(int waveID)
        {
            List<Survey> surveys = new List<Survey>();
            string query = "SELECT Survey FROM Surveys.FN_ListWaveSurveys(@waveID) ORDER BY Survey";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@waveID", waveID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            surveys.Add(GetSurveyInfo((string)rdr["Survey"]));
                        }
                    }
                }
                catch (Exception)
                {

                }

            }

            return surveys;
        }

        /// <summary>
        /// Returns a Survey object with the provided survey code.
        /// </summary>
        /// <param name="code">A valid survey code. Null is returned if the survey code is not found in the database.</param>
        /// <returns></returns>
        public static Survey GetSurveyInfo(string code)
        {
            Survey s;
            string query = "SELECT * FROM Surveys.FN_GetSurveyInfo (@survey)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", code);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        rdr.Read();
                        s = new Survey
                        {
                            SID = (int)rdr["ID"],
                            SurveyCode = (string)rdr["Survey"],
                            SurveyCodePrefix = (string)rdr["ISO_Code"],
                            Title = (string)rdr["SurveyTitle"],
                            CountryCode = (string)rdr["CountryCode"],
                            Locked = (bool)rdr["Locked"],
                            EnglishRouting = (bool)rdr["EnglishRouting"],
                            HideSurvey = (bool)rdr["HideSurvey"],
                            ReRun = (bool)rdr["ReRun"],
                            NCT = (bool)rdr["NCT"]
                        };

                        // language
                        if (!rdr.IsDBNull(rdr.GetOrdinal("Languages"))) s.Languages = (string)rdr["Languages"];

                        // group
                        if (!rdr.IsDBNull(rdr.GetOrdinal("GroupName")))
                            s.Group = new SurveyUserGroup((int)rdr["GroupID"], (string)rdr["GroupName"]);
                        else
                            s.Group = new SurveyUserGroup();

                        // cohort
                        if (!rdr.IsDBNull(rdr.GetOrdinal("Cohort")))
                        {
                            s.Cohort = new SurveyCohort((int)rdr["CohortID"], (string)rdr["Cohort"]);                         
                            if (!rdr.IsDBNull(rdr.GetOrdinal("CohortCode")))
                                s.Cohort.Code = (string)rdr["CohortCode"];
                            else
                                s.Cohort.Code = "";
                        }

                        // mode
                        if (!rdr.IsDBNull(rdr.GetOrdinal("Mode")))
                            s.Mode = new SurveyMode((int)rdr["Mode"], (string)rdr["ModeLong"], (string)rdr["ModeAbbrev"]);

                        

                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            // check for corrected wordings
            s.HasCorrectedWordings = HasCorrectedWordings(s.SurveyCode);

            return s;
        }

        public static bool HasCorrectedWordings(string surveyCode)
        {
            bool result;
            string query = "SELECT Surveys.FN_HasCorrectedWordings (@survey)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", surveyCode);
                try
                {
                    result = (bool) sql.SelectCommand.ExecuteScalar();
}
                catch (Exception)
                {
                    result = false;
                }

            }

            return result;

        }

        /// <summary>
        /// Creates a Survey object with the provided ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Survey GetSurveyInfo(int ID)
        {
            Survey s;
            string query = "SELECT * FROM Surveys.FN_GetSurveyInfoByID (@sid)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@sid", ID);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        rdr.Read();
                        s = new Survey
                        {
                            SID = (int)rdr["ID"],
                            SurveyCode = (string)rdr["Survey"],
                            SurveyCodePrefix = (string)rdr["ISO_Code"],
                            Title = (string)rdr["SurveyTitle"],
                            CountryCode = (string)rdr["CountryCode"],
                            Locked = (bool)rdr["Locked"],
                            EnglishRouting = (bool)rdr["EnglishRouting"],
                            HideSurvey = (bool)rdr["HideSurvey"],
                            ReRun = (bool)rdr["ReRun"],
                            NCT = (bool)rdr["NCT"]
                        };

                        // language
                        if (!rdr.IsDBNull(rdr.GetOrdinal("Languages"))) s.Languages = (string)rdr["Languages"];

                        // group
                        if (!rdr.IsDBNull(rdr.GetOrdinal("GroupName")))
                            s.Group = new SurveyUserGroup((int)rdr["GroupID"], (string)rdr["GroupName"]);
                        else
                            s.Group = new SurveyUserGroup();

                        // cohort
                        if (!rdr.IsDBNull(rdr.GetOrdinal("Cohort")))
                        {
                            s.Cohort = new SurveyCohort((int)rdr["CohortID"], (string)rdr["Cohort"]);
                            if (!rdr.IsDBNull(rdr.GetOrdinal("CohortCode")))
                                s.Cohort.Code = (string)rdr["CohortCode"];
                            else
                                s.Cohort.Code = "";
                        }

                        // mode
                        if (!rdr.IsDBNull(rdr.GetOrdinal("Mode")))
                            s.Mode = new SurveyMode((int)rdr["Mode"], (string)rdr["ModeLong"], (string)rdr["ModeAbbrev"]);
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            // check for corrected wordings
            s.HasCorrectedWordings = HasCorrectedWordings(s.SurveyCode);

            return s;
        }

        /// <summary>
        /// Returns the list of survey codes for active and past surveys in alpha order.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSurveyList()
        {
            List<string> surveyCodes = new List<string>();
            string query = "SELECT Survey FROM Surveys.FN_ListSurveys() ORDER BY ISO_Code, Wave, Survey";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            surveyCodes.Add((string)rdr["Survey"]);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }

            }

            return surveyCodes;
        }

        /// <summary>
        /// Returns the survey code for a particular Question ID.
        /// </summary>
        /// <param name="qid">Valid Question ID.</param>
        /// <returns>Survey Code as string, empty string if Question ID is invalid.</returns>
        public static string GetSurveyCodeByQID(int qid)
        {
            string surveyCode= "";
            string query = "SELECT Surveys.FN_SurveyByQID (@qid)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@qid", qid);
                try
                {
                    surveyCode = (string)sql.SelectCommand.ExecuteScalar();
                }
                catch (Exception)
                {
                    surveyCode = "";
                }

            }

            return surveyCode;
        }

        /// <summary>
        /// Returns the list of survey modes in the database.
        /// </summary>
        /// <returns></returns>
        public static List<SurveyMode> GetModeInfo()
        {
            List<SurveyMode> modes = new List<SurveyMode>();
            SurveyMode m;
            string query = "SELECT * FROM Surveys.FN_GetSurveyModes ORDER BY Mode";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            m = new SurveyMode((int)rdr["ID"], (string)rdr["Mode"], (string)rdr["ModeAbbrev"]);


                            modes.Add(m);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }
            return modes;
        }

        /// <summary>
        /// Returns the list of survey cohorts in the database.
        /// </summary>
        /// <returns></returns>
        public static List<SurveyCohort> GetCohortInfo()
        {
            List<SurveyCohort> cohorts = new List<SurveyCohort>();
            SurveyCohort c;
            string query = "SELECT * FROM Surveys.FN_GetCohortInfo() ORDER BY Cohort";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            c = new SurveyCohort((int)rdr["ID"], (string)rdr["Cohort"]);

                            c.Code = (string)rdr["Code"];
                            c.WebName = (string)rdr["WebName"];

                            cohorts.Add(c);

                        }

                    }
                }
                catch (Exception)
                {

                }
            }
            return cohorts;
        }

        /// <summary>
        /// Returns the list of survey groups in the database.
        /// </summary>
        /// <returns></returns>
        public static List<SurveyUserGroup> GetGroupInfo()
        {
            List<SurveyUserGroup> groups = new List<SurveyUserGroup>();
            SurveyUserGroup g;
            string query = "SELECT * FROM Surveys.FN_GetGroupInfo() ORDER BY [Group]";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            g = new SurveyUserGroup
                            {
                                ID = (int)rdr["ID"],
                                UserGroup = (string)rdr["Group"],
                                Code = (string)rdr["Code"],
                                WebName = (string)rdr["WebName"],

                            };

                            groups.Add(g);

                        }

                    }
                }
                catch (Exception)
                {

                }
            }
            return groups;
        }


    }
}
