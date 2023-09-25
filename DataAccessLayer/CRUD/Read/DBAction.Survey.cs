using System;
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
        ///  Return a list of surveys that changed on the target date.
        /// </summary>
        /// <param name="targetDate"></param>
        public static List<Survey> GetChangedSurveys(DateTime targetDate)
        {
            List<Survey> changed;
            string sql = "SELECT A.ID AS SID, A.Survey AS SurveyCode FROM FN_getChangedSurveys(@date) AS A GROUP BY A.ID, A.Survey;";

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@date", targetDate);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                changed = db.Query<Survey>(sql, parameters).ToList();
            }
            return changed;
        }

        /// <summary>
        /// Returns the date of last update (or null) for the given survey.
        /// </summary>
        /// <param name="s"></param>
        public static DateTime? GetSurveyLastUpdate(Survey s)
        {
            DateTime? lastupdate;
            string sql = "SELECT MAX(LastUpdate) FROM [dbo].[FN_getLastSurveyChangeDates] (@survey);";

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@survey", s.SurveyCode);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                lastupdate = (DateTime?)db.ExecuteScalar(sql, parameters);
            }
            return lastupdate;
        }

        /// <summary>
        ///  Return a list of surveys in the renumber list.
        /// </summary>
        public static List<KeyValuePair<int, string>> GetRenumberedSurveys()
        {
            List<KeyValuePair<int, string>> surveys = new List<KeyValuePair<int, string>>();
            string sql = "SELECT SurvID, Survey FROM qryRenumberedSurveys ORDER BY Survey";
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var results = db.Query(sql).Select(x => x as IDictionary<string, object>);
                foreach (IDictionary<string, object> row in results)
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
        public static List<Survey> GetAllSurveysInfo()
        {
            List<Survey> surveys = new List<Survey>();

            string sql = "SELECT ID AS SID, Survey AS SurveyCode, ISO_Code AS SurveyCodePrefix, SurveyTitle AS Title, CountryCode, Locked, " +
                        "EnglishRouting, HideSurvey, ReRun, NCT, Wave, WaveID, " +
                        "SurveyFileName AS WebName, ISISCreationDate AS CreationDate, " +
                        "CohortID, CohortID AS ID, Cohort, CohortCode AS Code," +
                        "ModeID, ModeID AS ID, ModeLong AS Mode, ModeAbbrev " +
                        "FROM qrySurveyInfo ORDER BY ISO_Code, Wave, Survey;" +
                    "SELECT SL.ID, SL.SurvID, SL.LanguageID, L.ID, L.Lang AS LanguageName, L.Abbrev, L.ISOAbbrev, L.NonLatin, L.PreferredFont, L.RTL " +
                        "FROM qrySurveyLanguages AS SL INNER JOIN qryLanguage AS L ON SL.LanguageID = L.ID;" +
                    "SELECT ID, SurvID, UserStateID, UserStateID AS ID, UserState AS UserStateName FROM qrySurveyUserStates;" +
                    "SELECT ID, SurvID, ProductID, ProductID AS ID, Product AS ProductName FROM qrySurveyProducts;";
                

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var results = db.QueryMultiple(sql);
                surveys = results.Read<Survey, SurveyCohort, SurveyMode, Survey>(
                (survey, cohort, mode) =>
                {
                    survey.Cohort = cohort;
                    survey.Mode = mode;
                    return survey;
                }, "CohortID, ModeID"
                ).ToList();

                var languages = results.Read<SurveyLanguage, Language, SurveyLanguage>((survlang, lang) =>
                {
                    survlang.SurvLanguage = lang;
                    return survlang;
                }, "LanguageID");
                var userstates = results.Read<SurveyUserState, UserState, SurveyUserState>((survstate, state) =>
                {
                    survstate.State = state;
                    return survstate;
                }, "UserStateID");
                var products = results.Read<SurveyScreenedProduct, ScreenedProduct, SurveyScreenedProduct>((survproduct, product) =>
                {
                    survproduct.Product = product;
                    return survproduct;
                }, "ProductID");

                foreach (Survey survey in surveys)
                {
                    survey.LanguageList = languages.Where(x => x.SurvID == survey.SID).ToList();
                    survey.UserStates = userstates.Where(x=>x.SurvID == survey.SID).ToList();
                    survey.ScreenedProducts = products.Where(x=>x.SurvID ==survey.SID).ToList();
                }
            }

            return surveys;
        }

        /// <summary>
        /// Returns the list of survey IDs that are considered locked.
        /// </summary>
        /// <returns></returns>
        public static List<int> GetLockedSurveys()
        {
            List<int> list = new List<int>();
            string sql = "SELECT SurvID FROM tblAutoLock";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                list = db.Query<int>(sql).ToList();
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
            
            string sql = "SELECT ID, Mode, ModeAbbrev FROM tblMode ORDER BY Mode";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                modes = db.Query<SurveyMode>(sql).ToList();
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

            string sql = "SELECT ID, Cohort, Code, WebName FROM tblCohort ORDER BY Cohort";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                cohorts = db.Query<SurveyCohortRecord>(sql).ToList();
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

            string sql = "SELECT ID, [Group] AS UserGroup, [Code], WebName FROM tblGroup ORDER BY [Group]";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                groups = db.Query<SurveyUserGroup>(sql).ToList();
            }
            return groups;
        }

        /// <summary>
        /// Unlocks a survey for the specified time interval.
        /// </summary>
        /// <returns></returns>
        public static int UnlockSurvey(string survey, int interval)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@survey", survey);
            parameters.Add("@interval", interval);

            int result = 0;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                result = db.ExecuteScalar<int>("proc_unlockSurvey", parameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        /// <summary>
        /// Locks a survey.
        /// </summary>
        /// <returns></returns>
        public static int LockSurvey(Survey s)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@survey", s.SurveyCode);

            int result = 0;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                result = db.ExecuteScalar<int>("proc_lockSurvey", parameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        /// <summary>
        /// Returns a list of survey codes that contain the varname.
        /// </summary>
        /// <param name="varname"></param>
        /// <returns></returns>
        public static List<string> GetVarNameSurveys(string varname)
        {
            List<string> surveyList = new List<string>();
            var parameters = new { varname };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                surveyList = db.Query<string>("SELECT Survey FROM qrySurveyQuestions WHERE VarName = @varname", parameters).ToList();
            }

            return surveyList;
        }

        /// <summary>
        /// Returns a list of survey codes that contain the refvarname.
        /// </summary>
        /// <param name="refvarname"></param>
        /// <returns></returns>
        public static List<string> GetRefVarNameSurveys(string refvarname)
        {
            List<string> surveyList = new List<string>();
            var parameters = new { refvarname };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                surveyList = db.Query<string>("SELECT Survey FROM qrySurveyQuestions WHERE refVarName = @refvarname", parameters).ToList();
            }

            return surveyList;
        }

        /// <summary>
        /// Returns the list of surveys that have been unlocked by a user.
        /// </summary>
        /// <returns></returns>
        public static List<LockedSurveyRecord> GetUserLockedSurveys()
        {
            List<LockedSurveyRecord> records = new List<LockedSurveyRecord>();

            string query = "SELECT ID, SurvID, Survey AS SurveyCode, UnlockedBy, Name, UnlockedFor, UnlockedAt " +
                "FROM qryUnlockedSurveys ORDER BY Survey";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                records = db.Query<LockedSurveyRecord>(query).ToList();
            }
            return records;
        }

        public static List<SurveyImage> GetSurveyImagesFromFolder(Survey survey)
        {
            string folder = @"\\psychfile\psych$\psych-lab-gfong\SMG\Survey Images\" +
                survey.SurveyCodePrefix + @" Images\" + survey.SurveyCode;

            if (!System.IO.Directory.Exists(folder))
                return new List<SurveyImage>();

            var files = System.IO.Directory.EnumerateFiles(folder, "*.*", System.IO.SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".png") || s.EndsWith(".jpg"));

            List<SurveyImage> images = new List<SurveyImage>();

            int id = 0;
            foreach (var file in files) 
            {
                int iWidth = 0;
                int iHeight = 0;
                using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(file))
                {
                    iWidth = bmp.Width;
                    iHeight = bmp.Height;
                    iWidth = (int)Math.Round((decimal)iWidth * 9525);
                    iHeight = (int)Math.Round((decimal)iHeight * 9525);
                }

                string filename = file.Substring(file.LastIndexOf(@"\") + 1);
                if (images.Any(x => x.ImageName.Equals(filename)))
                    continue;

                string varname = string.Empty;
                string language = string.Empty;
                string countries = string.Empty;
                string description = string.Empty;

                string[] parts = file.Split('_');

                if (parts.Length == 5)
                {
                    varname = parts[1];
                    language = parts[2];
                    countries = parts[3];
                    description = parts[4];
                }
                else
                {
                    if (file.IndexOf('_') > 0)
                    {
                        int first_ = file.IndexOf('_') + 1;
                        int second_ = file.IndexOf('_', first_);

                        if (second_ == -1 || first_ == -1)
                        {
                            varname = file.Substring(file.LastIndexOf(@"\") + 1);
                            description = file.Substring(file.LastIndexOf(@"\") + 1);
                        }
                        else
                        {
                            varname = file.Substring(first_, second_ - first_);
                            description = file.Substring(second_ + 1);
                        }
                    }
                }

                SurveyImage img = new SurveyImage()
                {
                    ID = id,
                    ImageName = file.Substring(file.LastIndexOf(@"\") + 1),
                    ImagePath = file,
                    Width = iWidth,
                    Height= iHeight,
                    Description = description,
                    VarName = varname,
                    Language = language,
                    Country = countries,
                };
                images.Add(img);
                id++;
            }

            return images;
        }
    }
}
