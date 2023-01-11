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
    
    
    /// <summary>
    /// Static class for interacting with the Database. TODO create stored procedures on server for each of these
    /// </summary>
    public static partial class DBAction
    {
        /// <summary>
        /// Returns the list of people.
        /// </summary>
        /// <returns></returns>
        public static List<PersonRecord> GetPeople()
        {
            List<PersonRecord> people = new List<PersonRecord>();

            string sql = "SELECT ID, Name, Init AS FirstName, LastName, Email, Active, PraccID, username, HomePhoneNo, OfficeNo, SMG, PM, Analyst, Pracc AS Praccer, Firm, " +
                        "Programmer, CountryTeam, Admin, RA AS ResearchAssistant, Dissem AS Dissemination, PI AS Investigator, Stat AS Statistician, Institution, CommentEntry AS Entry, " +
                        "PraccEntry, VarNameNotify AS VarNameChangeNotify FROM qryIssueInit ORDER BY Name;" +
                        "SELECT P.ID, p.CountryID AS StudyID, P.PersonnelID, C.Country AS StudyName, C.ISO_Code FROM qryPersonnelCountry AS P LEFT JOIN tblCountryCode AS C ON P.CountryID = C.ID ORDER BY C.Country;" +
                        "SELECT ID, PersonnelID, CommentType, Comment FROM qryPersonnelComments;"; 

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                var results = db.QueryMultiple(sql);

                people = results.Read<PersonRecord>().ToList();
                List<PersonnelStudyRecord> countries = results.Read<PersonnelStudyRecord>().ToList();
                List<PersonnelCommentRecord> comments = results.Read<PersonnelCommentRecord>().ToList();

                foreach (PersonnelStudyRecord p in countries)
                    p.NewRecord = false;
                foreach (PersonnelCommentRecord p in comments)
                    p.NewRecord = false;

                foreach (PersonRecord p in people)
                {
                    p.NewRecord = false;
                    p.AssociatedStudies = countries.Where(x => x.PersonnelID == p.ID).ToList();
                    p.PersonnelComments = comments.Where(x => x.PersonnelID == p.ID).ToList();
                }

            }

            return people;
        }

        /// <summary>
        /// Returns the list of screened products.
        /// </summary>
        /// <returns></returns>
        public static List<ScreenedProduct> GetScreenProducts()
        {
            List<ScreenedProduct> products = new List<ScreenedProduct>();
            string query = "SELECT * FROM qryScreenedProducts ORDER BY Product";

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
                            products.Add(new ScreenedProduct((int)rdr["ID"], rdr.SafeGetString("Product")));
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

        /// <summary>
        /// Returns the list of screened products.
        /// </summary>
        /// <returns></returns>
        public static List<ScreenedProduct> GetScreenProductsD()
        {
            List<ScreenedProduct> products = new List<ScreenedProduct>();
            string sql = "SELECT ID, Product FROM qryScreenedProducts ORDER BY Product";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                products = db.Query<ScreenedProduct>(sql).ToList();
            }

            return products;
        }

        // <summary>
        /// Returns the list of user states.
        /// </summary>
        /// <returns></returns>
        public static List<UserStateRecord> GetUserStates()
        {
            List<UserStateRecord> states = new List<UserStateRecord>();
            string query = "SELECT * FROM qryUserStates ORDER BY UserState";

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
                            states.Add(new UserStateRecord((int)rdr["ID"], rdr.SafeGetString("UserState")));
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

        // <summary>
        /// Returns the list of user states.
        /// </summary>
        /// <returns></returns>
        public static List<UserStateRecord> GetUserStatesD()
        {
            List<UserStateRecord> states = new List<UserStateRecord>();
            string sql = "SELECT ID, UserState FROM qryUserStates ORDER BY UserState";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                states = db.Query<UserStateRecord>(sql).ToList();
            }

            return states;
        }

        /// <summary>
        /// Unlocks a survey for the specified time interval.
        /// </summary>
        /// <returns></returns>
        public static int UnlockSurvey(Survey s, int interval)
        {
            int result = 0;
            string query = "proc_unlockSurvey";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.SelectCommand.Parameters.AddWithValue("@survey", s.SurveyCode);
                sql.SelectCommand.Parameters.AddWithValue("@interval", interval);

                try
                {
                    result = (int)sql.SelectCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    result = 1;
                }
            }

            return result;
        }

        /// <summary>
        /// Unlocks a survey for the specified time interval.
        /// </summary>
        /// <returns></returns>
        public static int UnlockSurvey(string s, int interval)
        {
            int result = 0;
            string query = "proc_unlockSurvey";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.SelectCommand.Parameters.AddWithValue("@survey", s);
                sql.SelectCommand.Parameters.AddWithValue("@interval", interval);

                try
                {
                    result = (int)sql.SelectCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    result = 1;
                }
            }

            return result;
        }

        /// <summary>
        /// Locks a survey.
        /// </summary>
        /// <returns></returns>
        public static int LockSurvey(Survey s)
        {
            int result = 0;
            string query = "proc_lockSurvey";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.SelectCommand.Parameters.AddWithValue("@survey", s.SurveyCode);

                try
                {
                    result = (int)sql.SelectCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    result = 1;
                }
            }

            return result;
        }



        /// <summary>
        /// Returns the list of regions.
        /// </summary>
        /// <returns></returns>
        public static List<RegionRecord> GetRegionInfo()
        {
            List<RegionRecord> regions = new List<RegionRecord>();
            RegionRecord r;
            string query = "SELECT R.ID, R.Region, P.ReservedPrefix FROM FN_GetAllRegions() AS R LEFT JOIN tblReservedPrefixes AS P ON R.ID = P.RegionID ORDER BY R.ID";

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
                            r = new RegionRecord
                            {
                                ID = (int)rdr["ID"],
                                RegionName = (string)rdr["Region"],
                                TempVarPrefix = rdr.SafeGetString("ReservedPrefix")

                            };

                            regions.Add(r);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return regions;
        }

        /// <summary>
        /// Returns the list of studies.
        /// </summary>
        /// <returns></returns>
        public static List<StudyRecord> GetStudyInfo()
        {
            List<StudyRecord> studies = new List<StudyRecord>();
            StudyRecord s;
            string query = "SELECT * FROM FN_GetStudyInfo() ORDER BY Study";

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
                            s = new StudyRecord();

                            s.ID = (int)rdr["ID"];
                            s.CountryName = rdr.SafeGetString("Country");
                            s.StudyName = rdr.SafeGetString("Study");
                            
                            if (Int32.TryParse(rdr.SafeGetString("CountryCode"), out int cc))  
                                s.CountryCode = cc;
                            
                            s.ISO_Code = rdr.SafeGetString("ISO_Code");
                            s.AgeGroup = rdr.SafeGetString("AgeGroup");
                            s.Cohort = (int)rdr["Cohort"];
                            s.Languages = rdr.SafeGetString("Languages");
                            s.RegionID = (int)rdr["RegionID"];

                            studies.Add(s);
                        }

                    }
                }
                catch (Exception)
                {
                    
                }
            }

            return studies;
        }

        /// <summary>
        /// Returns the studies for a particular region.
        /// </summary>
        /// <param name="regionID"></param>
        /// <returns></returns>
        public static List<StudyRecord> GetStudies(int regionID)
        {
            List<StudyRecord> studies = new List<StudyRecord>();
            StudyRecord s;
            string query = "SELECT * FROM FN_GetStudies(@regionID)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@regionID", regionID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            s = new StudyRecord
                            {
                                ID = (int)rdr["ID"],
                                CountryName = rdr.SafeGetString("Country"),
                                StudyName = rdr.SafeGetString("Study"),
                                CountryCode = Int32.Parse(rdr.SafeGetString("CountryCode")),
                                ISO_Code = rdr.SafeGetString("ISO_Code"),
                                AgeGroup = rdr.SafeGetString("AgeGroup")
                            };

                            studies.Add(s);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return studies;
        }

        /// <summary>
        /// Returns a list of survey waves.
        /// </summary>
        /// <returns></returns>
        public static List<StudyWaveRecord> GetWaveInfo()
        {
            List<StudyWaveRecord> waves = new List<StudyWaveRecord>();

            string sql = "SELECT W.ID, Wave, ISO_Code, Countries, EnglishRouting, W.CountryID AS StudyID " +
                "FROM tblProjectWaves AS W LEFT JOIN tblCountryCode AS C ON W.CountryID = C.ID " +
                "ORDER BY ISO_Code, Wave;" +
                "SELECT WaveID, StudyWave, Country, CountryID, FieldworkStart, FieldworkEnd FROM qryFieldworkDates";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                var results = db.QueryMultiple(sql);

                waves = results.Read<StudyWaveRecord>().ToList();
                var fieldworks = results.Read().Select(x => x as IDictionary<string, object>).ToList();

                foreach (IDictionary<string, object> row in fieldworks)
                {
                    var w = waves.Where(x => x.ID == (int)row["WaveID"]).FirstOrDefault();
                    if (w == null) continue;
                    Fieldwork f = new Fieldwork()
                    {
                        Country = new ITCCountry() { CountryName = (string)row["Country"] },
                        Start = (DateTime?)row["FieldworkStart"],
                        End = (DateTime?)row["FieldworkEnd"]
                    };
                    w.FieldworkDates.Add(f);
                }
            }

            return waves;
        }

        /// <summary>
        /// Returns the list of waves for a study.
        /// </summary>
        /// <param name="studyID"></param>
        /// <returns></returns>
        public static List<StudyWaveRecord> GetWaves(int studyID)
        {
            List<StudyWaveRecord> waves = new List<StudyWaveRecord>();
            StudyWaveRecord w;
            string query = "SELECT * FROM FN_GetWavesByStudy(@studyID)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@studyID", studyID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            w = new StudyWaveRecord
                            {
                                ID = (int)rdr["WaveID"],
                                ISO_Code = (string)rdr["ISO_Code"],
                                Wave = (double)rdr["Wave"]
                            };

                            waves.Add(w);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return waves;
        }

        /// <summary>
        /// Returns the list of VarName prefixes.
        /// </summary>
        /// <returns></returns>
        public static List<VariablePrefixRecord> GetVarPrefixes()
        {
            List<VariablePrefixRecord> prefixes = new List<VariablePrefixRecord>();
            
            string query = "SELECT * FROM qryDomainList ORDER BY Prefix";

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
                            VariablePrefixRecord p = new VariablePrefixRecord
                            {
                                ID = (int)rdr["ID"],
                                Prefix = rdr.SafeGetString("Prefix"),
                                PrefixName = rdr.SafeGetString("PrefixName"),
                                ProductType = rdr.SafeGetString("ProductType"),
                                RelatedPrefixes = rdr.SafeGetString("RelatedPrefixes"),
                                Description = rdr.SafeGetString("DomainName"),
                                Comments = rdr.SafeGetString("Comments"),
                                Inactive = (bool)rdr["InactiveDomain"]
                            };

                            prefixes.Add(p);
                        }

                    }
                }
                catch (Exception)
                {

                }

                query = "SELECT * FROM qryDomainRanges ORDER BY VarNumLow";
                sql.SelectCommand = new SqlCommand(query, conn);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            VariablePrefixRecord p = prefixes.Where(x => x.ID == (int)rdr["PrefixID"]).FirstOrDefault();
                            VariableRangeRecord range = new VariableRangeRecord();
                            range.ID = (int)rdr["ID"];
                            range.PrefixID = (int)rdr["PrefixID"];
                            range.Lower =rdr.SafeGetString("VarNumLow");
                            range.Upper = rdr.SafeGetString("VarNumHigh");
                            range.Description = rdr.SafeGetString("Description");
                            range.NewRecord = false;

                            if (p != null)
                                p.Ranges.Add(range);
                        }
                    }
                }
                catch (Exception)
                {

                }

                query = "SELECT R.*, D.Prefix FROM qryDomainListRelated AS R INNER JOIN qryDomainList AS D ON R.RelatedPrefixID = D.ID";
                sql.SelectCommand = new SqlCommand(query, conn);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            VariablePrefixRecord p = prefixes.Where(x => x.ID == (int)rdr["PrefixID"]).FirstOrDefault();
                            ParallelPrefixRecord parallel = new ParallelPrefixRecord();
                            parallel.ID = (int)rdr["ID"];
                            parallel.PrefixID = (int)rdr["PrefixID"];
                            parallel.RelatedID = (int)rdr["RelatedPrefixID"];
                            parallel.Prefix = rdr.SafeGetString("Prefix");
                            parallel.NewRecord = false;
                            
                            if (p != null)
                                p.ParallelPrefixes.Add(parallel);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            return prefixes;
        }

        /// <summary>
        /// Returns the list of bug reports.
        /// </summary>
        /// <returns></returns>
        public static List<BugReport> GetBugReports()
        {
            List<BugReport> bugs = new List<BugReport>();
            BugReport br;
            string query = "SELECT * FROM qryBugReports ORDER BY ID";

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
                            br = new BugReport
                            {
                                ID = (int)rdr["ID"],
                                Title =rdr.SafeGetString("Title"),
                                Description = rdr.SafeGetString("Description"),
                                Application = rdr.SafeGetString("Application"),
                                Form = rdr.SafeGetString("Form"),
                                
                                Survey = rdr.SafeGetString("Survey"),
                                
                            };
                            
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Priority")))
                                br.Priority = (PriorityLevel)rdr["Priority"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("SubDate")))
                                br.BugDate = (DateTime)rdr["SubDate"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("SubInit")))
                                br.Submitter = new Person((int)rdr["SubInit"]);
                            if (!rdr.IsDBNull(rdr.GetOrdinal("FixDate")))
                                br.FixDate = (DateTime)rdr["FixDate"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("FixInit")))
                                br.Fixer = new Person((int)rdr["FixInit"]);
                            if (!rdr.IsDBNull(rdr.GetOrdinal("ResDate")))
                                br.ResolutionDate = (DateTime)rdr["ResDate"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("ResInit")))
                                br.Resolver = new Person((int)rdr["ResInit"]);

                            br.Responses = DBAction.GetBugResponses(br.ID);
                            bugs.Add(br);
                        }

                    }
                }
                catch (Exception)
                {
                    
                }
            }
            

            return bugs;
        }

        /// <summary>
        /// Returns the list of responses for a bug report.
        /// </summary>
        /// <returns></returns>
        public static List<BugResponse> GetBugResponses(int bugID)
        {
            List<BugResponse> responses = new List<BugResponse>();
            BugResponse br;
            string query = "SELECT * FROM qryBugResponses WHERE BugID = " + bugID + " ORDER BY RespDate DESC";

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
                            br = new BugResponse
                            {
                                ID = (int)rdr["ID"],
                                BugID = (int)rdr["BugID"],
                                Response = (string)rdr["Response"],
                                ResponseDate = (DateTime)rdr["RespDate"],
                                Responder = new Person((int)rdr["User"])
                            };
                            
                            responses.Add(br);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return responses;
        }




        

        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SurveyProcessingRecord> GetSurveyProcessingRecords()
        {
            List<SurveyProcessingRecord> records = new List<SurveyProcessingRecord>();

            string query = "SELECT * FROM qrySurveyProcessing ORDER BY Survey, NewID";

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
                            SurveyProcessingRecord record = new SurveyProcessingRecord();
                            record.ID = (int)rdr["ID"];
                            record.SurveyID = new Survey() { SID = (int)rdr["SurvID"], SurveyCode = rdr.SafeGetString("Survey") };
                            record.Stage = new SurveyProcessingStage() { ID = (int)rdr["Stage"], StageName = rdr.SafeGetString("StageName") };
                            record.NotApplicable = (bool)rdr["NA"];
                            record.Done = (bool)rdr["Done"];

                            record.StageDates = GetSurveyProcessingDates(record.ID);

                            records.Add(record);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return records;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SurveyProcessingRecord> GetSurveyProcessingRecords(int survID)
        {
            List<SurveyProcessingRecord> records = new List<SurveyProcessingRecord>();

            string query = "SELECT * FROM qrySurveyProcessing WHERE SurvID = @survID ORDER BY NewID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survID", survID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            SurveyProcessingRecord record = new SurveyProcessingRecord();
                            record.ID = (int)rdr["ID"];
                            record.SurveyID = new Survey() { SID = (int)rdr["SurvID"], SurveyCode = rdr.SafeGetString("Survey") };
                            record.Stage = new SurveyProcessingStage() { ID = (int)rdr["Stage"], StageName = rdr.SafeGetString("StageName") };
                            record.NotApplicable = (bool)rdr["NA"];
                            record.Done = (bool)rdr["Done"];

                            record.StageDates = GetSurveyProcessingDates(record.ID);

                            records.Add(record);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return records;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SurveyProcessingDate> GetSurveyProcessingDates(int stageID)
        {
            List<SurveyProcessingDate> records = new List<SurveyProcessingDate>();

            string query = "SELECT * FROM qrySurveyProcessingDates WHERE StageID = @stageID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@stageID", stageID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            SurveyProcessingDate record = new SurveyProcessingDate();
                            record.ID = (int)rdr["ID"];
                            record.StageID = (int)rdr["StageID"];
                            record.StageDate = rdr.SafeGetDate("StageDate");
                            record.EntryDate = rdr.SafeGetDate("EntryDate");
                            record.EnteredBy = new Person(rdr.SafeGetString("EnteredBy"),(int)rdr["StageInit"]);
                            record.Contact = new Person(rdr.SafeGetString("ContactName"), (int)rdr["StageContact"]);
                            record.Notes = GetSurveyProcessingNotes(record.ID);

                            records.Add(record);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return records;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SurveyProcessingNote> GetSurveyProcessingNotes(int dateID)
        {
            List<SurveyProcessingNote> records = new List<SurveyProcessingNote>();

            string query = "SELECT * FROM qrySurveyProcessingNotes WHERE StageID = @stageID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@stageID", dateID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            SurveyProcessingNote record = new SurveyProcessingNote();
                            record.ID = (int)rdr["ID"];
                            record.DateID = (int)rdr["StageID"];
                            record.NoteDate = rdr.SafeGetDate("CommentDate");
                            record.Author = new Person(rdr.SafeGetString("EnteredByName"), (int)rdr["EnteredBy"]);
                            record.Note = rdr.SafeGetString("Note");

                            records.Add(record);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return records;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetHarmonyData(List<string> varnames, bool prep, bool prei, bool @prea, bool @litq, bool @psti, bool pstp, bool respname, bool nrname,
                bool translation, bool varlabel, bool domain, bool topic, bool content, bool product, bool englishRouting, string lang, bool showProjects, List<string> surveysFilter,
                bool itconly)
        {
            DataTable records = new DataTable();

            string query = "proc_ReportHarmony3";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                if (varnames.Count==0)
                    sql.SelectCommand.Parameters.AddWithValue("@varList", DBNull.Value);
                else 
                    sql.SelectCommand.Parameters.AddWithValue("@varList", string.Join(",", varnames));

                sql.SelectCommand.Parameters.AddWithValue("@prep", prep);
                sql.SelectCommand.Parameters.AddWithValue("@prei", prei);
                sql.SelectCommand.Parameters.AddWithValue("@prea", prea);
                sql.SelectCommand.Parameters.AddWithValue("@litq", litq);
                sql.SelectCommand.Parameters.AddWithValue("@psti", psti);
                sql.SelectCommand.Parameters.AddWithValue("@pstp", pstp);
                sql.SelectCommand.Parameters.AddWithValue("@respname", respname);
                sql.SelectCommand.Parameters.AddWithValue("@nrname", nrname);
                sql.SelectCommand.Parameters.AddWithValue("@translation", translation);
                sql.SelectCommand.Parameters.AddWithValue("@varlabel", varlabel);
                sql.SelectCommand.Parameters.AddWithValue("@domain", domain);
                sql.SelectCommand.Parameters.AddWithValue("@topic", topic);
                sql.SelectCommand.Parameters.AddWithValue("@content", content);
                sql.SelectCommand.Parameters.AddWithValue("@product", product);
                sql.SelectCommand.Parameters.AddWithValue("@englishRouting", englishRouting);
                sql.SelectCommand.Parameters.AddWithValue("@lang", lang);
                sql.SelectCommand.Parameters.AddWithValue("@showProjects", showProjects);
                if (surveysFilter.Count ==0)
                    sql.SelectCommand.Parameters.AddWithValue("@surveysFilter", DBNull.Value);
                else 
                    sql.SelectCommand.Parameters.AddWithValue("@surveysFilter", string.Join(",", surveysFilter));

                sql.SelectCommand.Parameters.AddWithValue("@itconly", itconly);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        for (int i = 0; i < rdr.FieldCount; i++)
                            records.Columns.Add(rdr.GetName(i), System.Type.GetType("System.String"));

                        while (rdr.Read())
                        {

                            DataRow newrow = records.NewRow();
                            object[] values = new object[rdr.FieldCount];
                            rdr.GetValues(values);
                            newrow.ItemArray = values;
                            records.Rows.Add(newrow);

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
