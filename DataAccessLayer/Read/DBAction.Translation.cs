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
        // Translations
        //
        // 

        /// <summary>
        /// Returns a list of translations for a particular survey and language.
        /// </summary>
        /// <param name="SurvID"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static List<Translation> GetSurveyTranslation(int SurvID, string language)
        {
            List<Translation> ts = new List<Translation>();
            Translation t;
            string query = "SELECT * FROM Translations.FN_GetSurveyTranslations(@sid, @language)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@sid", SurvID);
                sql.SelectCommand.Parameters.AddWithValue("@language", language);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            t = new Translation
                            {
                                ID = (int)rdr["ID"],
                                QID = (int)rdr["QID"],
                                Language = (string)rdr["Lang"],
                                LanguageName = new Language() { LanguageName = (string)rdr["Lang"] },
                                TranslationText = (string)rdr["Translation"],
                                Bilingual = (bool)rdr["Bilingual"]
                            };

                            if (t.Language.Equals("Arabic") || t.Language.Equals("Hebrew"))
                                t.LanguageName.RTL = true;
                                
                            ts.Add(t);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return ts;
        }

        /// <summary>
        /// Returns a list of translations for a particular survey and language.
        /// </summary>
        /// <param name="SurvID"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static Translation GetSurveyTranslation(string survey, string varname, string language)
        {
            Translation t = new Translation();

            string query = "SELECT * FROM qryTranslation WHERE Survey= @survey AND VarName =@varname AND Lang= @language";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", survey);
                sql.SelectCommand.Parameters.AddWithValue("@varname", varname);
                sql.SelectCommand.Parameters.AddWithValue("@language", language);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            t.ID = (int)rdr["ID"];
                            t.QID = (int)rdr["QID"];
                            t.Language = rdr.SafeGetString("Lang");
                            t.TranslationText = rdr.SafeGetString("Translation");
                            t.Bilingual = (bool)rdr["Bilingual"];
                           
                     
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return t;
        }

        /// <summary>
        /// Returns the list of all languages used by surveys.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<string> GetLanguages()
        {
            List<string> langs = new List<string>();
            string query = "SELECT Lang FROM qryTranslation GROUP BY Lang ORDER BY Lang";

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
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Lang")))
                                langs.Add((string)rdr["Lang"]);

                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            return langs;
        }

        /// <summary>
        /// Returns the list of languages used by a survey.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<string> GetLanguages(Survey s)
        {
            List<string> langs = new List<string>();
            string query = "SELECT Lang FROM Translations.FN_GetSurveyLanguages(@sid)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();
                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@sid", s.SID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Lang")))
                                langs.Add((string)rdr["Lang"]);

                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            return langs;
        }

        /// <summary>
        /// Returns the list of all languages used by surveys.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<Language> ListLanguages()
        {
            List<Language> langs = new List<Language>();
            string query = "SELECT * FROM Translations.FN_GetLanguages() Lang ORDER BY Lang";

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
                            Language l = new Language();
                            l.ID = (int)rdr["ID"];
                            l.LanguageName = rdr.SafeGetString("Lang");
                            l.Abbrev = rdr.SafeGetString("Abbrev");
                            l.ISOAbbrev = rdr.SafeGetString("ISOAbbrev");
                            l.NonLatin = (bool)rdr["NonLatin"];
                            l.PreferredFont = rdr.SafeGetString("PreferredFont");
                            l.RTL = (bool)rdr["RTL"];

                            langs.Add(l);

                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            return langs;
        }

        /// <summary>
        /// Returns the list of all languages used by surveys.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<Language> ListLanguages(Survey surv)
        {
            List<Language> langs = new List<Language>();
            string query = "SELECT * FROM Translations.FN_GetLanguagesSID(@sid) ORDER BY Lang";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();
                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@sid", surv.SID);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Language l = new Language();
                            l.ID = (int)rdr["ID"];
                            l.LanguageName = rdr.SafeGetString("Lang");
                            l.Abbrev = rdr.SafeGetString("Abbrev");
                            l.ISOAbbrev = rdr.SafeGetString("ISOAbbrev");
                            l.NonLatin = (bool)rdr["NonLatin"];
                            l.PreferredFont = rdr.SafeGetString("PreferredFont");
                            l.RTL = (bool)rdr["RTL"];

                            langs.Add(l);

                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            return langs;
        }

        /// <summary>
        /// Returns the list of all languages used by surveys.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<Language> ListLanguages(StudyWaveRecord wave)
        {
            List<Language> langs = new List<Language>();
            string query = "SELECT * FROM Translations.FN_GetLanguagesWID(@wid) ORDER BY Lang";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();
                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@wid", wave.ID);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Language l = new Language();
                            l.ID = (int)rdr["ID"];
                            l.LanguageName = rdr.SafeGetString("Lang");
                            l.Abbrev = rdr.SafeGetString("Abbrev");
                            l.ISOAbbrev = rdr.SafeGetString("ISOAbbrev");
                            l.NonLatin = (bool)rdr["NonLatin"];
                            l.PreferredFont = rdr.SafeGetString("PreferredFont");
                            l.RTL = (bool)rdr["RTL"];

                            langs.Add(l);

                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            return langs;
        }

        /// <summary>
        /// Returns the list of languages used by a survey.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<SurveyLanguage> GetSurveyLanguages(Survey s)
        {
            List<SurveyLanguage> langs = new List<SurveyLanguage>();
            string query = "SELECT * FROM qrySurveyLanguages AS SL INNER JOIN qryLanguage AS L ON SL.LanguageID = L.ID WHERE SurvID = @sid";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();
                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@sid", s.SID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            SurveyLanguage sl = new SurveyLanguage();
                            sl.ID = (int)rdr["ID"];
                            sl.SurvID = (int)rdr["SurvID"];
                            sl.SurvLanguage = new Language()
                            {
                                ID = (int)rdr["ID"],
                                LanguageName = rdr.SafeGetString("Lang"),
                                Abbrev = rdr.SafeGetString("Abbrev"),
                                ISOAbbrev = rdr.SafeGetString("ISOAbbrev"),
                                NonLatin = (bool)rdr["NonLatin"],
                                PreferredFont = rdr.SafeGetString("PreferredFont"),
                                RTL = (bool)rdr["RTL"]


                        };

                            langs.Add(sl); 
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            return langs;
        }

        /// <summary>
        /// Returns the list of languages used by a survey.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<string> GetLanguages(StudyWaveRecord s)
        {
            List<string> langs = new List<string>();
            string query = "SELECT Lang FROM Translations.FN_GetWaveLanguages(@wid) GROUP BY Lang";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();
                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@wid", s.ID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Lang")))
                                langs.Add((string)rdr["Lang"]);

                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            return langs;
        }

        /// <summary>
        /// Returns the list of translations for a single question.
        /// </summary>
        /// <param name="QID"></param>
        /// <returns></returns>
        public static List<Translation> GetQuestionTranslations(int QID)
        {
            Translation t;
            List<Translation> list = new List<Translation>();
            string query = "SELECT * FROM Translations.FN_GetQuestionTranslations(@qid, null)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@qid", QID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            t = new Translation
                            {
                                ID = (int)rdr["ID"],
                                QID = (int)rdr["QID"],
                                Survey = (string)rdr["Survey"],
                                VarName = (string)rdr["VarName"],
                                Language = (string)rdr["Lang"],
                                LanguageName = new Language() { LanguageName = (string)rdr["Lang"] },
                                TranslationText = (string)rdr["Translation"],
                                Bilingual = (bool)rdr["Bilingual"]
                            };

                            if (t.Language.Equals("Arabic") || t.Language.Equals("Hebrew"))
                                t.LanguageName.RTL = true;

                            list.Add(t);
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
        /// Returns the list of translations for a single question.
        /// </summary>
        /// <param name="QID"></param>
        /// <returns></returns>
        public static List<TranslationRecord> GetQuestionTranslationRecords(int QID)
        {
            TranslationRecord t;
            List<TranslationRecord> list = new List<TranslationRecord>();
            string query = "SELECT * FROM Translations.FN_GetQuestionTranslations(@qid, null)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@qid", QID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            t = new TranslationRecord
                            {
                                ID = (int)rdr["ID"],
                                QID = (int)rdr["QID"],
                                Survey = (string)rdr["Survey"],
                                VarName = (string)rdr["VarName"],
                                Language = (string)rdr["Lang"],
                                LanguageName = new Language() { LanguageName = (string)rdr["Lang"] },
                                TranslationText = (string)rdr["Translation"],
                                Bilingual = (bool)rdr["Bilingual"]
                            };

                            if (t.Language.Equals("Arabic") || t.Language.Equals("Hebrew"))
                                t.LanguageName.RTL = true;

                            list.Add(t);
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
        /// Returns the list of translations for a single question.
        /// </summary>
        /// <param name="QID"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static List<Translation> GetQuestionTranslations(int QID, string language)
        {
            Translation t;
            List<Translation> list = new List<Translation>();
            string query = "SELECT * FROM Translations.FN_GetQuestionTranslations(@qid, @language)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@qid", QID);
                sql.SelectCommand.Parameters.AddWithValue("@language", language);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            t = new Translation
                            {
                                ID = (int)rdr["ID"],
                                QID = (int)rdr["QID"],
                                Language = (string)rdr["Lang"],
                                TranslationText = (string)rdr["Translation"],
                                Bilingual = (bool)rdr["Bilingual"]
                            };

                            list.Add(t);
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
        /// Returns a list of questions from a backup database.
        /// </summary>
        /// <remarks>
        /// This could be achieved by changing the FROM clause in GetSurveyTable but often there are columns that don't exist in the backups, due to 
        /// their age and all the changes that have happened to the database over the years. 
        /// </remarks>
        public static void FillBackupTranslation(Survey s, DateTime backup, List<string> langs)
        {
            DataTable rawTable;
            BackupConnection bkp = new BackupConnection(backup);
            string select = "SELECT * ";
            string where = "Survey = '" + s.SurveyCode + "' AND Lang IN ('" + string.Join("','", langs) + "')";

            if (bkp.Connected)
            {
                Console.Write("unzipped");
                rawTable = bkp.GetTranslationData(select, where);
            }
            else
            {
                // could not unzip backup/7zip not installed etc. 
                return;
            }

            foreach (DataRow r in rawTable.Rows)
            {
                Translation t = new Translation();
                t.ID = (int)r["ID"];
                t.QID = (int)Math.Floor((double)r["QID"]);
                if (!DBNull.Value.Equals(r["Translation"])) t.TranslationText = (string)r["Translation"];
                if (!DBNull.Value.Equals(r["Lang"])) t.Language = (string)r["Lang"];
                if (!DBNull.Value.Equals(r["LitQ"])) t.LitQ = (string)r["LitQ"];

                SurveyQuestion q = s.QuestionByID(t.QID);

                t.Survey = q.SurveyCode;
                t.VarName = q.VarName.VarName;

                q.Translations.Add(t);
            }
        }


        //
        // Fill Methods
        //

        /// <summary>
        /// Populates the provided Survey's questions with translations.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="language"></param>
        public static void FillTranslationsBySurvey(Survey s, string language)
        {
            Translation t;
            string query = "SELECT * FROM Translations.FN_GetSurveyTranslations(@sid, @language)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();
                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@sid", s.SID);
                sql.SelectCommand.Parameters.AddWithValue("@language", language);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            t = new Translation
                            {
                                ID = (int)rdr["ID"],
                                QID = (int)rdr["QID"],
                                Language = (string)rdr["Lang"],
                                TranslationText = (string)rdr["Translation"],
                                Bilingual = (bool)rdr["Bilingual"]
                            };
                            s.QuestionByID(t.QID).Translations.Add(t);

                        }
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }

        }

        
    }
}
