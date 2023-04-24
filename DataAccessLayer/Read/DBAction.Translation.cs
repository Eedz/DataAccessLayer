using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
namespace ITCLib
{
    partial class DBAction
    {
        //
        // Translations
        //
        // 

        #region Reviewed
        /// <summary>
        /// Returns a list of translations for a particular survey and language.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static List<Translation> GetSurveyTranslation(string survey, string language)
        {
            List<Translation> ts = new List<Translation>();

            string sql = "SELECT ID, QID, Survey, VarName, [Translation] AS TranslationText, LitQ, Bilingual, " +
                "LanguageID, LanguageID AS ID, Lang AS LanguageName, Abbrev, ISOAbbrev, NonLatin, RTL, PreferredFont " +
                "FROM qryTranslation WHERE Survey = @survey AND Lang = @language;";
                
            var parameters = new { survey, language };

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                ts = db.Query<Translation, Language, Translation>(sql, (translation, lang) =>
                {
                    translation.LanguageName = lang;
                    return translation;
                }, parameters, splitOn: "LanguageID").ToList();

            }

            return ts;
        }

        /// <summary>
        /// Returns a list of translations for a particular survey, varname and language.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="varname"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static Translation GetSurveyTranslation(string survey, string varname, string language)
        {
            Translation t;

            string sql = "SELECT ID, QID, Survey, VarName, [Translation] AS TranslationText, LitQ, Bilingual, " +
                "LanguageID, LanguageID AS ID, Lang AS LanguageName, Abbrev, ISOAbbrev, NonLatin, RTL, PreferredFont " +
                "FROM qryTranslation WHERE Survey = @survey AND VarName =@varname AND Lang = @language;";

            var parameters = new { survey, varname, language };

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                t = db.Query<Translation, Language, Translation>(sql, (translation, lang) =>
                {
                    translation.LanguageName = lang;
                    return translation;
                }, parameters, splitOn: "LanguageID").FirstOrDefault();

            }

            return t;
        }

        /// <summary>
        /// Returns the list of languages.
        /// </summary>
        /// <returns></returns>
        public static List<Language> GetLanguages()
        {
            List<Language> languages;

            string sql = "SELECT ID, [Lang] AS LanguageName, [Abbrev], [ISOAbbrev], [NonLatin], [PreferredFont], [RTL] " +
                "FROM tblLanguage ORDER BY Lang;";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                languages = db.Query<Language>(sql).ToList();
            }

            return languages;
        }

        /// <summary>
        /// Returns the list of languages.
        /// </summary>
        /// <returns></returns>
        public static List<Language> ListLanguages()
        {
            List<Language> languages;

            string sql = "SELECT ID, [Lang] AS LanguageName, [Abbrev], [ISOAbbrev], [NonLatin], [PreferredFont], [RTL] " +
                "FROM tblLanguage ORDER BY Lang;";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                languages = db.Query<Language>(sql).ToList();
            }

            return languages;
        }

        /// <summary>
        /// Returns the list of languages used by a survey.
        /// </summary>
        /// <param name="surv"></param>
        /// <returns></returns>
        public static List<Language> ListLanguages(Survey surv)
        {
            List<Language> languages = new List<Language>();
            string sql = "SELECT ID, Lang AS LanguageName, Abbrev, ISOAbbrev, NonLatin, PreferredFont, RTL " +
                "FROM Translations.FN_GetLanguagesSID(@sid) ORDER BY Lang;";
            var parameters = new { sid = surv.SID };
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                languages = db.Query<Language>(sql, parameters).ToList();
            }

            return languages;
        }

        /// <summary>
        /// Returns the list of languages used by surveys in a wave.
        /// </summary>
        /// <param name="wave"></param>
        /// <returns></returns>
        public static List<Language> ListLanguages(StudyWaveRecord wave)
        {
            List<Language> languages = new List<Language>();
            string sql = "SELECT ID, Lang AS LanguageName, Abbrev, ISOAbbrev, NonLatin, PreferredFont, RTL " +
                "FROM Translations.FN_GetLanguagesWID(@wid) ORDER BY Lang;";

            var parameters = new { wid = wave.ID };
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                languages = db.Query<Language>(sql, parameters).ToList();
            }

            return languages;
        }

        /// <summary>
        /// Returns the list of survey language records for a survey.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<SurveyLanguage> GetSurveyLanguages(Survey s)
        {
            List<SurveyLanguage> languages = new List<SurveyLanguage>();
            string sql = "SELECT SL.ID, SL.SurvID, " +
                "SL.LanguageID, L.ID, L.Lang AS LanguageName, L.Abbrev, L.ISOAbbrev, L.NonLatin, L.PreferredFont, L.RTL " +
                "FROM qrySurveyLanguages AS SL INNER JOIN qryLanguage AS L ON SL.LanguageID = L.ID WHERE SurvID = @sid";

            var parameters = new { sid = s.SID };

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                languages = db.Query<SurveyLanguage, Language, SurveyLanguage>(sql, (survLang, lang) =>
                {
                    survLang.SurvLanguage = lang;
                    return survLang;
                }, parameters, splitOn: "LanguageID").ToList();
                
               
            }
            return languages;
        }

        /// <summary>
        /// Returns the list of translations for a single question.
        /// </summary>
        /// <param name="QID"></param>
        /// <returns></returns>
        public static List<Translation> GetQuestionTranslations (int QID)
        {
            List<Translation> list = new List<Translation>();
            string sql = "SELECT ID, QID, Survey, VarName, [Translation] AS TranslationText, LitQ, Bilingual, " +
                "LanguageID, LanguageID AS ID, Lang AS LanguageName, Abbrev, ISOAbbrev, NonLatin, RTL, PreferredFont " +
                "FROM qryTranslation WHERE QID = @qid;";

            var parameters = new { qid = QID };

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                list = db.Query<Translation, Language, Translation> (sql, (translation, language) => {
                        translation.LanguageName = language;
                        return translation;
                    }, parameters, splitOn: "LanguageID").ToList();
            }

            return list;
        }

        /// <summary>
        /// Returns the translation for a single question in a specified language.
        /// </summary>
        /// <param name="QID"></param>
        /// <returns></returns>
        public static Translation GetQuestionTranslations(int QID, string language)
        {
            Translation translation;
            string sql = "SELECT ID, QID, Survey, VarName, [Translation] AS TranslationText, LitQ, Bilingual, " +
                "LanguageID, LanguageID AS ID, Lang AS LanguageName, Abbrev, ISOAbbrev, NonLatin, RTL, PreferredFont " +
                "FROM qryTranslation WHERE QID = @qid AND Lang = @language;";

            var parameters = new { qid = QID, language };

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                translation = db.Query<Translation, Language, Translation>(sql, (t, lang) => {
                    t.LanguageName = lang;
                    return t;
                }, parameters, splitOn: "LanguageID").FirstOrDefault();
            }

            return translation;
        }
        #endregion

        //
        // Fill Methods
        //

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
                if (!DBNull.Value.Equals(r["Lang"]))
                {
                    t.LanguageName = new Language() { LanguageName = (string)r["Lang"] };
                }
                if (!DBNull.Value.Equals(r["LitQ"])) t.LitQ = (string)r["LitQ"];

                SurveyQuestion q = s.QuestionByID(t.QID);

                t.Survey = q.SurveyCode;
                t.VarName = q.VarName.VarName;

                q.Translations.Add(t);
            }
        }
    }
}
