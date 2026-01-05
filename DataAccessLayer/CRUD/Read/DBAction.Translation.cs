using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace ITCLib
{
    partial class DBAction
    {
        //
        // Translations
        //
        // 
         
        public static Translation GetTranslation (int ID)
        {
            Translation t;
            string sql = "SELECT ID, QID, Survey, VarName, [Translation] AS TranslationText, LitQ, Bilingual, LastUpdate, " +
                "LanguageID, LanguageID AS ID, Lang AS LanguageName, Abbrev, ISOAbbrev, NonLatin, RTL, PreferredFont " +
                "FROM qryTranslation WHERE ID =@ID";

            var parameters = new { ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                t = db.Query<Translation, Language, Translation>(sql, (translation, lang) =>
                {
                    translation.LanguageName = lang;
                    return translation;
                }, parameters, splitOn: "LanguageID").ToList().FirstOrDefault();

            }

            return t;
        }
        
        /// <summary>
        /// Returns a list of translations for a particular survey and language.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static List<Translation> GetSurveyTranslation(string survey, string language)
        {
            List<Translation> ts = new List<Translation>();

            string sql = "SELECT ID, QID, Survey, VarName, [Translation] AS TranslationText, LitQ, Bilingual, LastUpdate, " +
                "LanguageID, LanguageID AS ID, Lang AS LanguageName, Abbrev, ISOAbbrev, NonLatin, RTL, PreferredFont " +
                "FROM qryTranslation WHERE Survey = @survey AND Lang = @language;";
                
            var parameters = new { survey, language };

            using (IDbConnection db = new SqlConnection(connectionString))
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

            string sql = "SELECT ID, QID, Survey, VarName, [Translation] AS TranslationText, LitQ, Bilingual, LastUpdate, " +
                "LanguageID, LanguageID AS ID, Lang AS LanguageName, Abbrev, ISOAbbrev, NonLatin, RTL, PreferredFont " +
                "FROM qryTranslation WHERE Survey = @survey AND VarName =@varname AND Lang = @language;";

            var parameters = new { survey, varname, language };

            using (IDbConnection db = new SqlConnection(connectionString))
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

            using (IDbConnection db = new SqlConnection(connectionString))
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

            using (IDbConnection db = new SqlConnection(connectionString))
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
            using (IDbConnection db = new SqlConnection(connectionString))
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
        public static List<Language> ListLanguages(StudyWave wave)
        {
            List<Language> languages = new List<Language>();
            string sql = "SELECT ID, Lang AS LanguageName, Abbrev, ISOAbbrev, NonLatin, PreferredFont, RTL " +
                "FROM Translations.FN_GetLanguagesWID(@wid) ORDER BY Lang;";

            var parameters = new { wid = wave.ID };
            using (IDbConnection db = new SqlConnection(connectionString))
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

            using (IDbConnection db = new SqlConnection(connectionString))
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
            string sql = "SELECT ID, QID, Survey, VarName, [Translation] AS TranslationText, LitQ, Bilingual, LastUpdate, " +
                "LanguageID, LanguageID AS ID, Lang AS LanguageName, Abbrev, ISOAbbrev, NonLatin, RTL, PreferredFont " +
                "FROM qryTranslation WHERE QID = @qid;";

            var parameters = new { qid = QID };

            using (IDbConnection db = new SqlConnection(connectionString))
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
            string sql = "SELECT ID, QID, Survey, VarName, [Translation] AS TranslationText, LitQ, Bilingual, LastUpdate, " +
                "LanguageID, LanguageID AS ID, Lang AS LanguageName, Abbrev, ISOAbbrev, NonLatin, RTL, PreferredFont " +
                "FROM qryTranslation WHERE QID = @qid AND Lang = @language;";

            var parameters = new { qid = QID, language };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                translation = db.Query<Translation, Language, Translation>(sql, (t, lang) => {
                    t.LanguageName = lang;
                    return t;
                }, parameters, splitOn: "LanguageID").FirstOrDefault();
            }

            return translation;
        }


        
    }
}
