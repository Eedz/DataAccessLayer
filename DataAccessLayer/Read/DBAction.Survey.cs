﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
namespace ITCLib
{
    partial class DBAction
    {
        //
        // Surveys
        //

        /// <summary>
        ///  Return a list of surveys to be generated.
        /// </summary>
        /// <param name="allSurveys"></param>
        public static List<Survey> GetChangedSurveys(DateTime targetDate)
        {
            List<Survey> changed;
            Survey s;

            changed = new List<Survey>();

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                string query = "";

                sql.SelectCommand = new SqlCommand();

                query = "SELECT A.ID, A.Survey, B.SurveyTitle " +
                    "FROM FN_getChangedSurveys(@date) AS A INNER JOIN tblStudyAttributes AS B ON A.Survey = B.Survey " +
                    "GROUP BY A.ID, A.Survey, B.SurveyTitle";

                sql.SelectCommand.Parameters.AddWithValue("@date", targetDate);
                sql.SelectCommand.Connection = conn;
                sql.SelectCommand.CommandText = query;

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            s = new Survey
                            {
                                SID = (int)rdr["ID"],
                                SurveyCode = rdr["Survey"].ToString(),
                            };
                            changed.Add(s);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }

                return changed;
            }
        }

        /// <summary>
        ///  Return a list of surveys in the renumber list.
        /// </summary>
        /// <param name="allSurveys"></param>
        public static List<KeyValuePair<int, string>> GetRenumberedSurveys()
        {
            List<KeyValuePair<int, string>> surveys = new List<KeyValuePair<int, string>>();
            string sql = "SELECT SurvID, Survey FROM qryRenumberedSurveys ORDER BY Survey";
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                var results = db.Query(sql).Select(x=>x as IDictionary<string,object>);
                foreach (IDictionary<string,object> row in results)
                {
                    surveys.Add(new KeyValuePair<int, string>(Int32.Parse(row["SurvID"].ToString()), row["Survey"].ToString()));
                }
            }
            return surveys;
        }

        /// <summary>
        /// Returns the list of surveys in the database in alpha order.
        /// </summary>
        /// <returns></returns>
        public static List<SurveyRecord> GetAllSurveysInfoD()
        {

            List<SurveyRecord> surveys = new List<SurveyRecord>();

            string sql = "SELECT ID AS SID, Survey AS SurveyCode, ISO_Code AS SurveyCodePrefix, SurveyTitle AS Title, CountryCode, Locked, EnglishRouting, HideSurvey, ReRun, NCT, Wave, WaveID, " +
                "SurveyFileName AS WebName, Languages, CreationDate " +
                "CohortID, CohortID AS ID, Cohort, CohortCode AS Code," +
                "Mode AS ModeID, Mode AS ID, ModeLong AS Mode, ModeAbbrev " +
                "FROM Surveys.FN_ListAllSurveys() ORDER BY ISO_Code, Wave, Survey";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                surveys = db.Query<SurveyRecord, SurveyCohort, SurveyMode, SurveyRecord>(sql, 
                (survey, cohort, mode) =>
                {
                    survey.Cohort = cohort;
                    survey.Mode = mode;
                    return survey;
                },
                splitOn: "CohortID, ModeID"
                ).ToList();
            }

            return surveys;

        }

        /// <summary>
        /// Returns the list of surveys in the database in alpha order.
        /// </summary>
        /// <returns></returns>
        public static List<SurveyRecord> GetAllSurveysInfo()
        {
            
            List<SurveyRecord> surveys = new List<SurveyRecord>();
            string query = "SELECT * FROM Surveys.FN_ListAllSurveys() ORDER BY ISO_Code, Wave, Survey";

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

                            SurveyRecord s = new SurveyRecord();

                            s.SID = (int)rdr["ID"];
                            s.SurveyCode = (string)rdr["Survey"];
                            s.SurveyCodePrefix = (string)rdr["ISO_Code"];
                            s.Title = rdr.SafeGetString("SurveyTitle");
                            s.CountryCode = (string)rdr["CountryCode"];
                            s.Locked = (bool)rdr["Locked"];
                            s.EnglishRouting = (bool)rdr["EnglishRouting"];
                            s.HideSurvey = (bool)rdr["HideSurvey"];
                            s.ReRun = (bool)rdr["ReRun"];
                            s.NCT = (bool)rdr["NCT"];
                            s.Wave = (double)rdr["Wave"];
                            s.WaveID = (int)rdr["WaveID"];
                            s.WebName = rdr.SafeGetString("SurveyFileName");
                            s.CreationDate = rdr.SafeGetDate("CreationDate");
                            

                            // language
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Languages"))) s.Languages = (string)rdr["Languages"];

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

                            s.LanguageList = GetSurveyLanguages(s);
                            s.UserStates = GetUserStates(s);
                            s.ScreenedProducts = GetScreenProducts(s);

                            surveys.Add(s);
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
        public static List<SurveyRecord> GetSurveys(int waveID)
        {
            List<SurveyRecord> surveys = new List<SurveyRecord>();
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
        public static SurveyRecord GetSurveyInfo(string code)
        {
            SurveyRecord s;
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
                        s = new SurveyRecord
                        {
                            SID = (int)rdr["ID"],
                            SurveyCode = (string)rdr["Survey"],
                            SurveyCodePrefix = (string)rdr["ISO_Code"],
                            Title = rdr.SafeGetString("SurveyTitle"),
                            CountryCode = (string)rdr["CountryCode"],
                            Locked = (bool)rdr["Locked"],
                            EnglishRouting = (bool)rdr["EnglishRouting"],
                            HideSurvey = (bool)rdr["HideSurvey"],
                            ReRun = (bool)rdr["ReRun"],
                            NCT = (bool)rdr["NCT"],
                            ITCSurvey = (bool)rdr["ITCSurvey"],
                            Wave = (double)rdr["Wave"],
                            WaveID = (int)rdr["WaveID"],
                            WebName = rdr.SafeGetString("SurveyFileName"),
                            CreationDate = rdr.SafeGetDate("ISISCreationDate")
                        };

                        // language
                        if (!rdr.IsDBNull(rdr.GetOrdinal("Languages"))) s.Languages = (string)rdr["Languages"];

                        // cohort
                        if (!rdr.IsDBNull(rdr.GetOrdinal("Cohort")))
                        {
                            s.Cohort = new SurveyCohort((int)rdr["CohortID"], (string)rdr["Cohort"]);                         
                            if (!rdr.IsDBNull(rdr.GetOrdinal("CohortCode")))
                                s.Cohort.Code = (string)rdr["CohortCode"];
                            else
                                s.Cohort.Code = "";
                        }
                        else
                        {
                            s.Cohort = new SurveyCohort();
                        }

                        s.Group.ID = (int)rdr["GroupID"];
                        s.Group.UserGroup = rdr.SafeGetString("GroupName");

                        // mode
                        if (!rdr.IsDBNull(rdr.GetOrdinal("Mode")))
                            s.Mode = new SurveyMode((int)rdr["Mode"], (string)rdr["ModeLong"], (string)rdr["ModeAbbrev"]);


                        // check for corrected wordings
                        s.HasCorrectedWordings = HasCorrectedWordings(s.SurveyCode);
                        s.LanguageList = GetSurveyLanguages(s);
                        s.UserStates = GetUserStates(s);
                        s.ScreenedProducts = GetScreenProducts(s);
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            

            return s;
        }

        // <summary>
        /// Returns the list of screened products for a survey.
        /// </summary>
        /// <returns></returns>
        public static List<SurveyScreenedProduct> GetScreenProducts(Survey survey)
        {
            List<SurveyScreenedProduct> products = new List<SurveyScreenedProduct>();
            string query = "SELECT * FROM qrySurveyProducts WHERE SurvID=@survID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survID", survey.SID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            products.Add(new SurveyScreenedProduct(){
                                ID = (int)rdr["ID"],
                                SurvID = (int)rdr["SurvID"],
                                Product = new ScreenedProduct((int)rdr["ProductID"], rdr.SafeGetString("Product"))
                            });
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return products;
        }

        // <summary>
        /// Returns the list of user states for a survey.
        /// </summary>
        /// <returns></returns>
        public static List<SurveyUserState> GetUserStates(Survey survey)
        {
            List<SurveyUserState> states = new List<SurveyUserState>();
            string query = "SELECT * FROM qrySurveyUserStates WHERE SurvID=@survID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survID", survey.SID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            states.Add(new SurveyUserState()
                            {
                                ID = (int)rdr["ID"],
                                SurvID = (int)rdr["SurvID"],
                                State = new UserState((int)rdr["UserStateID"], rdr.SafeGetString("UserState"))
                            });
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return states;
        }

        /// <summary>
        /// Returns true if the surey has corrected wordings.
        /// </summary>
        /// <param name="surveyCode"></param>
        /// <returns></returns>
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
                            NCT = (bool)rdr["NCT"],
                            Wave = (double)rdr["Wave"]
                        };

                        // language
                        if (!rdr.IsDBNull(rdr.GetOrdinal("Languages"))) s.Languages = (string)rdr["Languages"];

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


                        // check for corrected wordings
                        s.HasCorrectedWordings = HasCorrectedWordings(s.SurveyCode);
                        s.LanguageList = GetSurveyLanguages(s);
                        s.UserStates = GetUserStates(s);
                        s.ScreenedProducts = GetScreenProducts(s);
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

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
        /// Returns the list of survey IDs that are considered locked.
        /// </summary>
        /// <returns></returns>
        public static List<int> GetLockedSurveys()
        {
            List<int> list = new List<int>();
            string query = "SELECT SurvID FROM tblAutoLock";

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
                            list.Add((int)rdr["SurvID"]);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }

            }

            return list;
        }

        /// <summary>
        /// Returns the list of survey modes in the database.
        /// </summary>
        /// <returns></returns>
        public static List<SurveyMode> GetModeInfo()
        {
            List<SurveyMode> modes = new List<SurveyMode>();
            SurveyMode m;
            string query = "SELECT * FROM Surveys.FN_GetSurveyModes() ORDER BY Mode";

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
        public static List<SurveyCohortRecord> GetCohortInfo()
        {
            List<SurveyCohortRecord> cohorts = new List<SurveyCohortRecord>();
            SurveyCohortRecord c;
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
                            c = new SurveyCohortRecord((int)rdr["ID"], (string)rdr["Cohort"]);

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

        public static string GetTempPrefix(Survey s)
        {
            string prefix = "";
            string query = "SELECT ReservedPrefix FROM qryReservedPrefixes WHERE RegionID = (SELECT RegionID FROM qrySUrveyInfo WHERE Survey=@survey)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", s.SurveyCode);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            prefix = rdr.SafeGetString("ReservedPrefix");
                        }

                    }
                }
                catch (Exception)
                {

                }
            }
            return prefix;
        }

        public static List<string> GetVarNameSurveys(string varname)
        {
            List<string> surveyList = new List<string>();
            string query = "SELECT Survey FROM qrySurveyQuestions WHERE VarName =@varname";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@varname", varname);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            surveyList.Add(rdr.SafeGetString("Survey"));
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            return surveyList;
        }

        public static List<string> GetRefVarNameSurveys(string refvarname)
        {
            List<string> surveyList = new List<string>();
            string query = "SELECT Survey FROM qrySurveyQuestions WHERE refVarName =@refvarname";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@refvarname", refvarname);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            surveyList.Add(rdr.SafeGetString("Survey"));
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            return surveyList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<LockedSurveyRecord> GetUserLockedSurveys()
        {
            List<LockedSurveyRecord> records = new List<LockedSurveyRecord>();
            
            string query = "SELECT * FROM qryUnlockedSurveys ORDER BY Survey";

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
                            LockedSurveyRecord r  = new LockedSurveyRecord
                            {
                                ID = (int)rdr["ID"],
                                SurvID = (int)rdr["SurvID"],
                                SurveyCode = (string)rdr["Survey"],
                                UnlockedBy = (int)rdr["UnlockedBy"],
                                Name = (string)rdr["Name"],
                                UnlockedFor = (int)rdr["UnlockedFor"],
                                UnlockedAt = (DateTime)rdr["UnlockedAt"]

                            };

                            records.Add(r);

                        }

                    }
                }
                catch (Exception)
                {

                }
            }
            return records;
        }

    }
}
