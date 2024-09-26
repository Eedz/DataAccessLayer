using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ITCLib;
using Dapper;

namespace ITCLib
{
    /// <summary>
    /// Static class for interacting with the Database.
    /// </summary>
    public static partial class DBAction
    {
#if DEBUG
        static string connectionString = ConfigurationManager.ConnectionStrings["ISISConnectionStringTest"].ConnectionString;
#else
        static string connectionString = ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString;
#endif

        public static string GetConnectionString()
        {
            return connectionString;
        }

        /// <summary>
        /// Returns the list of people.
        /// </summary>
        /// <returns></returns>
        public static List<Person> GetPeople()
        {
            List<Person> people = new List<Person>();

            string sql = "SELECT ID, Init AS FirstName, LastName, Email, Active, PraccID, username, HomePhoneNo, OfficeNo, SMG, PM, Analyst, Pracc AS Praccer, Firm, " +
                        "Programmer, CountryTeam, Admin, RA AS ResearchAssistant, Dissem AS Dissemination, PI AS Investigator, Stat AS Statistician, Institution, CommentEntry AS Entry, " +
                        "PraccEntry, VarNameNotify AS VarNameChangeNotify FROM qryIssueInit ORDER BY Name;" +
                        "SELECT P.ID, P.PersonnelID, p.CountryID AS StudyID, C.ID, C.Country AS StudyName, C.ISO_Code FROM qryPersonnelCountry AS P LEFT JOIN tblCountryCode AS C ON P.CountryID = C.ID ORDER BY C.Country;" +
                        "SELECT ID, PersonnelID, CommentType, Comment FROM qryPersonnelComments;" +
                        "SELECT PR.ID, PersonnelID, RoleID, R.ID, Role AS RoleName FROM tblPersonnelRoles AS PR LEFT JOIN tblPersonnel AS P ON PR.PersonnelID = P.ID LEFT JOIN tblRoles AS R ON PR.RoleID = R.ID;"; 

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var results = db.QueryMultiple(sql);

                people = results.Read<Person>().ToList();
                List<PersonnelStudy> countries = results.Read<PersonnelStudy, Study, PersonnelStudy>((personnelstudy, study) =>
                {
                    personnelstudy.StudyName = study;
                    return personnelstudy;
                }, splitOn: "StudyID").ToList();
                List<PersonnelComment> comments = results.Read<PersonnelComment>().ToList();
                List<PersonnelRole> roles = results.Read<PersonnelRole, Role, PersonnelRole>((personnelrole, role) =>
                {
                    personnelrole.RoleName = role;
                    return personnelrole;
                }, splitOn: "RoleID").ToList();

                foreach (Person p in people)
                {
                    p.AssociatedStudies = countries.Where(x => x.PersonnelID == p.ID).ToList();
                    p.PersonnelComments = comments.Where(x => x.PersonnelID == p.ID).ToList();
                    p.Roles = roles.Where(x => x.PersonnelID == p.ID).ToList();
                }

            }

            return people;
        }

        public static List<Role> GetRoles()
        {
            List<Role> roles = new List<Role>();

            string sql = "SELECT ID, Role AS RoleName FROM tblRoles ORDER BY RoleName;";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                roles = db.Query<Role>(sql).ToList();
            }

            return roles;
        }

        /// <summary>
        /// Returns the list of people.
        /// </summary>
        /// <returns></returns>
        public static List<Person> GetAllPeople()
        {
            List<Person> people = new List<Person>();

            string sql = "SELECT ID, Name, Init AS FirstName, LastName, Email, Active, PraccID, username, HomePhoneNo, OfficeNo, SMG, PM, Analyst, Pracc AS Praccer, Firm, " +
                        "Programmer, CountryTeam, Admin, RA AS ResearchAssistant, Dissem AS Dissemination, PI AS Investigator, Stat AS Statistician, Institution, CommentEntry AS Entry, " +
                        "PraccEntry, VarNameNotify AS VarNameChangeNotify FROM qryIssueInit ORDER BY Name;";
                       
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                people = db.Query<Person>(sql).ToList();
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
            string sql = "SELECT ID, Product AS ProductName FROM qryScreenedProducts ORDER BY Product";
            
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                products = db.Query<ScreenedProduct>(sql).ToList();
            }
            return products;
        }

        /// <summary>
        /// Returns the list of user states.
        /// </summary>
        /// <returns></returns>
        public static List<UserState> GetUserStates()
        {
            List<UserState> states = new List<UserState>();

            string sql = "SELECT ID, UserState AS UserStateName FROM qryUserStates ORDER BY UserState";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                states = db.Query<UserState>(sql).ToList();
            }

            return states;
        }

        /// <summary>
        /// Returns the list of regions.
        /// </summary>
        /// <returns></returns>
        public static List<Region> GetRegionInfo()
        {
            List<Region> regions = new List<Region>();

            string sql = "SELECT R.ID, R.Region AS RegionName, P.ReservedPrefix AS TempVarPrefix FROM tblRegion AS R LEFT JOIN tblReservedPrefixes AS P ON R.ID = P.RegionID ORDER BY R.ID";

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                regions = db.Query<Region>(sql).ToList();
            }
            return regions;
        }

        /// <summary>
        /// Returns the list of studies.
        /// </summary>
        /// <returns></returns>
        public static List<Study> GetStudyInfo()
        {
            List<Study> studies = new List<Study>();
            
            string sql = "SELECT ID, Country AS CountryName, Study AS StudyName, CountryCode, ISO_Code, AgeGroup, Cohort, Languages, RegionID FROM tblCountryCode ORDER BY Study";

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                studies = db.Query<Study>(sql).ToList();
            }

            return studies;
        }

        /// <summary>
        /// Returns the studies for a particular region.
        /// </summary>
        /// <returns></returns>
        public static List<Study> GetStudyInfo(int regionID)
        {
            List<Study> studies = new List<Study>();

            string sql = "SELECT ID, Country AS CountryName, Study AS StudyName, CountryCode, ISO_Code, AgeGroup, Cohort, Languages, RegionID FROM tblCountryCode " + 
                "WHERE RegionID = @regionID ORDER BY Study";

            var parameters = new { regionID };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                studies = db.Query<Study>(sql, parameters).ToList();
            }

            return studies;
        }

        /// <summary>
        /// Returns a list of survey waves.
        /// </summary>
        /// <returns></returns>
        public static List<StudyWave> GetWaveInfo()
        {
            List<StudyWave> waves = new List<StudyWave>();

            string sql = "SELECT W.ID, Wave, ISO_Code, Countries, EnglishRouting, W.CountryID AS StudyID " +
                "FROM tblProjectWaves AS W LEFT JOIN tblCountryCode AS C ON W.CountryID = C.ID " +
                "ORDER BY ISO_Code, Wave;" +
                "SELECT WaveID, StudyWave, Country, CountryID, FieldworkStart, FieldworkEnd FROM qryFieldworkDates WHERE NOT CountryID IS NULL";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var results = db.QueryMultiple(sql);

                waves = results.Read<StudyWave>().ToList();
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

            string sql = "SELECT WaveID AS ID, Wave, ISO_Code FROM qryStudyWaves WHERE CountryID = @studyID;";

            var parameters = new { studyID };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                waves = db.Query<StudyWaveRecord>(sql, parameters).ToList();
            }

            return waves;
        }

        /// <summary>
        /// Returns the list of VarName prefixes.
        /// </summary>
        /// <returns></returns>
        public static List<VariablePrefix> GetVariablePrefixes()
        {
            List<VariablePrefix> prefixes = new List<VariablePrefix>();

            string sql = "SELECT ID, Prefix, PrefixName, ProductType, RelatedPrefixes, DomainName AS Description, Comments, InactiveDomain AS Inactive FROM qryDomainList ORDER BY Prefix;" +
                "SELECT ID, PrefixID, VarNumLow AS Lower, VarNumHigh AS Upper, Description FROM qryDomainRanges ORDER BY VarNumLow;" +
                "SELECT R.ID, R.PrefixID, R.RelatedPrefixID, D.Prefix AS RelatedPrefix FROM qryDomainListRelated AS R INNER JOIN qryDomainList AS D ON R.RelatedPrefixID = D.ID;";

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                var results = db.QueryMultiple(sql);

                prefixes = results.Read<VariablePrefix>().ToList();
                var ranges = results.Read<VariableRange>().ToList();
                var parallels = results.Read <ParallelPrefix>().ToList();

                foreach (ParallelPrefix parallel in parallels)
                {
                    var pr = prefixes.FirstOrDefault(x => x.ID == parallel.PrefixID);      
                    if (pr != null)
                        pr.ParallelPrefixes.Add(parallel);
                }

                foreach (VariableRange range in ranges)
                {
                    var pr = prefixes.FirstOrDefault(x => x.ID == range.PrefixID);
                    if (pr != null)
                        pr.Ranges.Add(range);
                }
            }
            return prefixes;
        }

        /// <summary>
        /// Returns the list of Survey Processing Records, complete with dates and notes
        /// </summary>
        /// <returns></returns>
        public static List<SurveyProcessingRecord> GetSurveyProcessingRecords()
        {
            List<SurveyProcessingRecord> records = new List<SurveyProcessingRecord>();

            string sql = "SELECT ID, NA AS NotApplicable, Done, SurvID, SurvID AS SID, Survey AS SurveyCode, Stage, Stage AS ID, StageName FROM qrySurveyProcessing ORDER BY Survey, NewID;" +
                "SELECT ID, StageID, StageDate, EntryDate, StageInit, StageInit AS ID, EnteredBy AS Name, StageContact, StageContact AS ID, ContactName AS Name  FROM qrySurveyProcessingDates;" +
                "SELECT ID, StageID AS DateID, CommentDate AS NoteDate, Note, EnteredBy, EnteredBy AS ID, EnteredByName AS Name FROM qrySurveyProcessingNotes;";

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                var results = db.QueryMultiple(sql);

                records = results.Read<SurveyProcessingRecord, Survey, SurveyProcessingStage, SurveyProcessingRecord>((record, survey, stage) =>
                {
                    record.SurveyID = survey;
                    record.Stage = stage;
                    return record;
                }, splitOn: "SurvID, Stage").ToList();

                var dates = results.Read<SurveyProcessingDate, Person, Person, SurveyProcessingDate>((date, person1, person2) =>
                {
                    date.EnteredBy = person1;
                    date.Contact = person2;
                    return date;
                }, splitOn: "StageInit, StageContact");

                var notes = results.Read<SurveyProcessingNote, Person, SurveyProcessingNote>((note, person1) =>
                {
                    note.Author = person1;
                    return note;
                }, splitOn: "EnteredBy").ToList();

                foreach (SurveyProcessingDate d in dates)
                {
                    d.Notes = notes.Where(x => x.DateID == d.ID).ToList();

                }
                var dateswithnots = dates.Where(x => x.Notes.Count > 0);
                foreach (SurveyProcessingRecord r in records)
                {
                    r.StageDates = dates.Where(x => x.StageID == r.ID).ToList();
                    
                }
                var stageswithdates = records.Where(x => x.StageDates.Count > 0);
            }

            return records;
        }

        /// <summary>
        /// Returns the list of Survey Processing Records, complete with dates and notes, for the specified survey
        /// </summary>
        /// <param name="survID"></param>
        /// <returns></returns>
        public static List<SurveyProcessingRecord> GetSurveyProcessingRecords(int survID)
        {
            List<SurveyProcessingRecord> records = new List<SurveyProcessingRecord>();

            string sql = "SELECT ID, NA AS NotApplicable, Done, SurvID, SurvID AS SID, Survey AS SurveyCode, Stage, Stage AS ID, StageName FROM qrySurveyProcessing " +
                            "WHERE SurvID = @survID ORDER BY Survey, NewID;" +
                "SELECT ID, StageID, StageDate, EntryDate, StageInit, StageInit AS ID, EnteredBy AS Name, StageContact, StageContact AS ID, ContactName AS Name  FROM qrySurveyProcessingDates;" +
                "SELECT ID, StageID AS DateID, CommentDate AS NoteDate, Note, EnteredBy, EnteredBy AS ID, EnteredByName AS Name FROM qrySurveyProcessingNotes;";

            var parameters = new { survID };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                var results = db.QueryMultiple(sql, parameters);

                records = results.Read<SurveyProcessingRecord, Survey, SurveyProcessingStage, SurveyProcessingRecord>((record, survey, stage) =>
                {
                    record.SurveyID = survey;
                    record.Stage = stage;
                    return record;
                }, splitOn: "SurvID, Stage").ToList();

                var dates = results.Read<SurveyProcessingDate, Person, Person, SurveyProcessingDate>((date, person1, person2) =>
                {
                    date.EnteredBy = person1;
                    date.Contact = person2;
                    return date;
                }, splitOn: "StageInit, StageContact");

                var notes = results.Read<SurveyProcessingNote, Person, SurveyProcessingNote>((note, person1) =>
                {
                    note.Author = person1;
                    return note;
                }, splitOn: "EnteredBy").ToList();

                foreach (SurveyProcessingDate d in dates)
                {
                    d.Notes = notes.Where(x => x.DateID == d.ID).ToList();

                }

                foreach (SurveyProcessingRecord r in records)
                {
                    r.StageDates = dates.Where(x => x.StageID == r.ID).ToList();

                }
            }

            return records;
        }

        /// <summary>
        /// Returns a data table of questions matching all of the specified criteria.
        /// </summary>
        /// <param name="varnames"></param>
        /// <param name="prep"></param>
        /// <param name="prei"></param>
        /// <param name="prea"></param>
        /// <param name="litq"></param>
        /// <param name="psti"></param>
        /// <param name="pstp"></param>
        /// <param name="respname"></param>
        /// <param name="nrname"></param>
        /// <param name="translation"></param>
        /// <param name="varlabel"></param>
        /// <param name="domain"></param>
        /// <param name="topic"></param>
        /// <param name="content"></param>
        /// <param name="product"></param>
        /// <param name="englishRouting"></param>
        /// <param name="lang"></param>
        /// <param name="showProjects"></param>
        /// <param name="surveysFilter"></param>
        /// <param name="itconly"></param>
        /// <returns></returns>
        public static DataTable GetHarmonyData(List<string> varnames, bool prep, bool prei, bool @prea, bool @litq, bool @psti, bool pstp, bool respname, bool nrname,
                bool translation, bool varlabel, bool domain, bool topic, bool content, bool product, bool englishRouting, string lang, bool showProjects, List<string> surveysFilter,
                bool itconly)
        {
            DataTable records = new DataTable();

            string query = "proc_ReportHarmony3";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
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
