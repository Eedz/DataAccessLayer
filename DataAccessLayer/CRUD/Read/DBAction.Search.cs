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
        // Search related queries
        //

        //
        // Question Search
        //
        public static List<SurveyQuestion> GetSurveyQuestions(List<SearchCriterium> criteria, bool withTranslation, string withTranslationText = null, bool excludeHiddenSurveys = true)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();

            string query = "SELECT ID, Survey AS SurveyCode, Qnum, PreP# AS PrePNum, PreP, PreI# AS PreINum, PreI, PreA# AS PreANum, PreA, " +
                    "LitQ# AS LitQNum, LitQ, PstI#, PstI, RespName, RespOptions, NRName, NRCodes, CorrectedFlag, ScriptOnly, AltQnum, AltQnum2, AltQnum3, " +
                    "VarName, VarLabel, " +
                    "DomainNum, DomainNum AS ID, Domain AS LabelText, " +
                    "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                    "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                    "ProductNum, ProductNum AS ID, Product AS LabelText " +
                    "FROM qrySurveyQuestions ";
            string where = string.Empty;

            if (criteria.Count > 0)
            {
                int t = 0;
                foreach (SearchCriterium sc in criteria)
                {
                    where += sc.GetParameterizedCondition(t);
                    where += " AND ";
                    t = t + sc.Criteria.Count();
                }
            }

            if (excludeHiddenSurveys)
                where += " NOT Survey IN (SELECT Survey FROM qrySurveyInfo WHERE HideSurvey = 1)";
            else
                where = Utilities.TrimString(where, " AND ");


            if (!string.IsNullOrEmpty(where))
                query += "WHERE " + where;

            DynamicParameters parameters = new DynamicParameters();

            int c = 0;
            foreach (SearchCriterium sc in criteria)
            {
                foreach (string f in sc.Fields)
                    foreach (string s in sc.Criteria)
                    {
                        parameters.Add("@tag" + c, s);
                        c++;
                    }
            }

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                qs = db.Query<SurveyQuestion, VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, SurveyQuestion>(query, (question, variable, domain, topic, content, product) =>
                {
                    variable.Domain = domain;
                    variable.Topic = topic;
                    variable.Content = content;
                    variable.Product = product;
                    question.VarName = variable;
                    return question;
                }, parameters,splitOn: "VarName, DomainNum, TopicNum, ContentNum, ProductNum").ToList();
            
            }

            if (withTranslation)
            {
                // get translations of questions matching the same criteria
                var translations = GetTranslations(criteria);

                if (!string.IsNullOrEmpty(withTranslationText))
                    translations = translations.Where(x => !string.IsNullOrEmpty(x.TranslationText) && x.TranslationText.ToLower().Contains(withTranslationText.ToLower())).ToList();

                foreach (SurveyQuestion q in qs)
                {
                    q.Translations = translations.Where(x => x.QID == q.ID).ToList();
                }
                // only include questions with translations
                qs = qs.Where(x=>x.Translations.Count>0).ToList();
            }

            return qs;
        }

        public static List<Translation> GetTranslations(List<SearchCriterium> criteria)
        {
            List<Translation> qs = new List<Translation>();

            string query = "SELECT TID AS ID, ID AS QID, Survey, VarName, [Translation] AS TranslationText, LanguageID, LanguageID AS ID, Lang AS LanguageName " +
                    "FROM qrySurveyQuestionsTranslations ";
            string where = string.Empty;

            if (criteria.Count > 0)
            {
                int t = 0;
                foreach (SearchCriterium sc in criteria)
                {
                    where += sc.GetParameterizedCondition(t);
                    where += " AND ";
                    t = t + sc.Criteria.Count();
                }
            }

            where = Utilities.TrimString(where, " AND ");

            if (!string.IsNullOrEmpty(where))
                query += "WHERE " + where;

            DynamicParameters parameters = new DynamicParameters();

            int c = 0;
            foreach (SearchCriterium sc in criteria)
            {
                foreach (string f in sc.Fields)
                    foreach (string s in sc.Criteria)
                    {
                        parameters.Add("@tag" + c, s);
                        c++;
                    }
            }

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                qs = db.Query<Translation, Language, Translation>(query, (translation, language) =>
                {
                    translation.LanguageName = language;
                    return translation;
                }, parameters, splitOn: "LanguageID").ToList();

            }
            return qs;
        }

        public static List<SurveyQuestion> GetSurveyQuestions(List<SearchCriterium> criteria)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();

            string query = "SELECT ID, Survey AS SurveyCode, Qnum, PreP# AS PrePNum, PreP, PreI# AS PreINum, PreI, PreA# AS PreANum, PreA, " +
                    "LitQ# AS LitQNum, LitQ, PstI#, PstI, RespName, RespOptions, NRName, NRCodes, CorrectedFlag, ScriptOnly, AltQnum, AltQnum2, AltQnum3, " +
                    "VarName, VarLabel, " +
                    "DomainNum, DomainNum AS ID, Domain AS LabelText, " +
                    "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                    "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                    "ProductNum, ProductNum AS ID, Product AS LabelText " +
                    "FROM qrySurveyQuestions ";
            string where = string.Empty;

            if (criteria.Count > 0)
            {
                int t = 0;
                foreach (SearchCriterium sc in criteria)
                {
                    where += sc.GetParameterizedCondition(t);
                    where += " AND ";
                    t = t + sc.Criteria.Count();
                }
            }

            where = Utilities.TrimString(where, " AND ");

            if (!string.IsNullOrEmpty(where))
                query += "WHERE " + where;

            DynamicParameters parameters = new DynamicParameters();

            int c = 0;
            foreach (SearchCriterium sc in criteria)
            {
                foreach (string f in sc.Fields)
                    foreach (string s in sc.Criteria)
                    {
                        parameters.Add("@tag" + c, s);
                        c++;
                    }
            }

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                qs = db.Query<SurveyQuestion, VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, SurveyQuestion>(query, (question, variable, domain, topic, content, product) =>
                {
                    variable.Domain = domain;
                    variable.Topic = topic;
                    variable.Content = content;
                    variable.Product = product;
                    question.VarName = variable;
                    return question;
                }, parameters, splitOn: "VarName, DomainNum, TopicNum, ContentNum, ProductNum").ToList();

            }

            return qs;
        }
    }
}
