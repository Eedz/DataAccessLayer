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
                                TranslationText = (string)rdr["Translation"],
                                Bilingual = (bool)rdr["Bilingual"]
                            };
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
        /// Returns the list of languages used by a survey.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<string> GetLanguages(StudyWave s)
        {
            List<string> langs = new List<string>();
            string query = "SELECT Lang FROM Translations.FN_GetWaveLanguages(@wid)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();
                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@wid", s.WaveID);

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
                    int i = 0;
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
