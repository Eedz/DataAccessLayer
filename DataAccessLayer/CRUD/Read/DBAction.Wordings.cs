using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace ITCLib
{
    public static partial class DBAction
    {
        /// <summary>
        /// Returns all wording records.
        /// </summary>
        /// <returns></returns>
        public static List<Wording> GetWordings()
        {
            List<Wording> wordings = new List<Wording>();
            string query = "SELECT ID AS WordID, FieldName, Wording AS WordingText FROM Wordings.FN_GetAllWordings() ORDER BY FieldName, ID";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                wordings = db.Query<Wording>(query).ToList();
            }

            return wordings;
        }

        /// <summary>
        /// Returns the complete list of wordings of a certain type.
        /// </summary>
        /// <param name="fieldname">Name of wording type.</param>
        /// <returns></returns>
        public static List<Wording> GetWordings(string fieldname)
        {
            List<Wording> wordings = new List<Wording>();

            var parameters = new { field = fieldname };
            string sql = "SELECT WordID, WordingText, @field AS FieldName FROM Wordings.FN_GetWordings(@field) ORDER BY WordID";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                wordings = db.Query<Wording>(sql, parameters).ToList();
            }

            return wordings;
        }

        /// <summary>
        /// Returns the complete list of response sets of a certain type.
        /// </summary>
        /// <param name="fieldname">Name of response type.</param>
        /// <returns></returns>
        public static List<ResponseSet> GetResponseSets(string fieldname)
        {
            List<ResponseSet> wordings = new List<ResponseSet>();

            var parameters = new { field = fieldname };
            string sql = "SELECT RespName AS RespSetName, @field AS FieldName, ResponseList AS RespList FROM Wordings.FN_GetResponseSets(@field) ORDER BY RespName";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                wordings = db.Query<ResponseSet>(sql, parameters).ToList();
            }

            return wordings;
        }

        /// <summary>
        /// Returns all response sets matching criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="exactMatch"></param>
        /// <returns></returns>
        public static List<ResponseSet> GetResponseSets(List<string> criteria, bool exactMatch)
        {
            List<ResponseSet> setList = new List<ResponseSet>();

            string query = "SELECT RespName AS RespSetName, 'RespOptions' AS FieldName, RespOptions AS RespList FROM qryRespOptions ";

            DynamicParameters parameters = new DynamicParameters();

            if (criteria.Count > 0) query += "WHERE ";

            for (int i = 0; i < criteria.Count; i++)
            {
                if (string.IsNullOrEmpty(criteria[i]))
                    continue;

                if (exactMatch)
                    query += "RespOptions = @criteria" + i + " OR ";
                else
                    query += "RespOptions LIKE '%' + @criteria" + i + " + '%' OR ";

                parameters.Add("@criteria" + i, criteria[i]);
            }
            query = query.Substring(0, query.Length - 3);
            query += " ORDER BY RespName";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                setList = db.Query<ResponseSet>(query, parameters).ToList();       
            }

            return setList;
        }

        /// <summary>
        /// Returns all response sets of a specific type.
        /// </summary>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        public static List<ResponseSet> GetNonResponseSets(List<string> criteria, bool exactMatch)
        {
            List<ResponseSet> setList = new List<ResponseSet>();

            string query = "SELECT NRName AS RespSetName, 'NRCodes' AS FieldName, NRCodes AS RespList FROM qryNonRespOptions ";

            DynamicParameters parameters = new DynamicParameters();

            if (criteria.Count > 0) query += "WHERE ";

            for (int i = 0; i < criteria.Count; i++)
            {
                if (string.IsNullOrEmpty(criteria[i]))
                    continue;

                if (exactMatch)
                    query += "NRCodes = @criteria" + i + " OR ";
                else
                    query += "NRCodes LIKE '%' + @criteria" + i + " + '%' OR ";

                parameters.Add("@criteria" + i, criteria[i]);
            }
            query = query.Substring(0, query.Length - 3);
            query += " ORDER BY NRName";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                setList = db.Query<ResponseSet>(query, parameters).ToList();
            }

            return setList;
        }

        /// <summary>
        /// Returns the text of a particular wording.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="wordID"></param>
        /// <returns></returns>
        public static string GetWordingText(string fieldname, int wordID)
        {
            string text = "";
            string query = "SELECT Wordings.FN_GetWordingText(@fieldname, @wordID)";

            var parameters = new { fieldname, wordID };
            
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                text = db.ExecuteScalar<string>(query, parameters);
            }

            return text;
        }

        /// <summary>
        /// Returns the text of a specified response set.
        /// </summary>
        /// <param name="respname"></param>
        /// <returns></returns>
        public static string GetResponseText(string respname)
        {
            string text = "";
            string query = "SELECT Wordings.FN_GetResponseText(@respname)";

            var parameters = new { respname };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                text = db.ExecuteScalar<string>(query, parameters);
            }

            return text;
        }

        /// <summary>
        /// Returns the text of a specified non-response set.
        /// </summary>
        /// <param name="nrname"></param>
        /// <returns></returns>
        public static string GetNonResponseText(string nrname)
        {
            string text = "";
            string query = "SELECT Wordings.FN_GetNonResponseText(@nrname)";

            var parameters = new { nrname };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                text = db.ExecuteScalar<string>(query, parameters);
            }

            return text;
        }

        /// <summary>
        /// Returns a list of WordingUsage objects which represent the questions that use the provided field/wordID combination.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="wordID"></param>
        /// <returns></returns>
        public static List<WordingUsage> GetWordingUsage(string field, int wordID)
        {
            List<WordingUsage> qList = new List<WordingUsage>();
            string query = "SELECT VarName, VarLabel, Survey AS SurveyCode, WordID, Qnum, Locked FROM Wordings.FN_GetWordingUsage (@field, @wordID)";

            var parameters = new { field, wordID };
           
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                qList = db.Query<WordingUsage>(query, parameters).ToList();
            }

            return qList;
        }

        /// <summary>
        /// Returns a list of ResponseUsage objects which represent the questions that use the provided field/respName combination.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="respName"></param>
        /// <returns></returns>
        public static List<ResponseUsage> GetResponseUsage(string field, string respName)
        {
            List<ResponseUsage> qList = new List<ResponseUsage>();
            string query = "SELECT VarName, VarLabel, Survey AS SurveyCode, RespName, Qnum, Locked FROM Wordings.FN_GetResponseUsage (@field, @respName)";

            var parameters = new { field, respName };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                qList = db.Query<ResponseUsage> (query, parameters).ToList();
            }

            return qList;
        }

        /// <summary>
        /// Returns all wording records used in a survey that contain the search parameter.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static List<Wording> GetSurveyPreP(string search, string survey)
        {
            List<Wording> wordings = new List<Wording>();
            string query = "SELECT ID AS WordID, 'PreP' AS FieldName, Wording AS WordingText FROM Wordings.FN_GetSurveyPrePMatching(@search, @survey) ORDER BY ID";

            var parameters = new { search, survey };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                wordings = db.Query<Wording>(query, parameters).ToList();
            }

            return wordings;
        }

        /// <summary>
        /// Returns all wording records used in a survey that contain the search parameter.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static List<Wording> GetSurveyPreI(string search, string survey)
        {
            List<Wording> wordings = new List<Wording>();
            string query = "SELECT ID AS WordID, 'PreI' AS FieldName, Wording AS WordingText FROM Wordings.FN_GetSurveyPreIMatching(@search, @survey) ORDER BY ID";

            var parameters = new { search, survey };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                wordings = db.Query<Wording>(query, parameters).ToList();
            }

            return wordings;
        }

        /// <summary>
        /// Returns all wording records used in a survey that contain the search parameter.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static List<Wording> GetSurveyPreA(string search, string survey)
        {
            List<Wording> wordings = new List<Wording>();
            string query = "SELECT ID AS WordID, 'PreA' AS FieldName, Wording AS WordingText FROM Wordings.FN_GetSurveyPreAMatching(@search, @survey) ORDER BY ID";

            var parameters = new { search, survey };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                wordings = db.Query<Wording>(query, parameters).ToList();
            }

            return wordings;
        }

        /// <summary>
        /// Returns all wording records used in a survey that contain the search parameter.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static List<Wording> GetSurveyLitQ(string search, string survey)
        {
            List<Wording> wordings = new List<Wording>();
            string query = "SELECT ID AS WordID, 'LitQ' AS FieldName, Wording AS WordingText FROM Wordings.FN_GetSurveyLitQMatching(@search, @survey) ORDER BY ID";

            var parameters = new { search, survey };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                wordings = db.Query<Wording>(query, parameters).ToList();
            }

            return wordings;
        }

        /// <summary>
        /// Returns all wording records used in a survey that contain the search parameter.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static List<Wording> GetSurveyPstI(string search, string survey)
        {
            List<Wording> wordings = new List<Wording>();
            string query = "SELECT ID AS WordID, 'PstI' AS FieldName, Wording AS WordingText FROM Wordings.FN_GetSurveyPstIMatching(@search, @survey) ORDER BY ID";

            var parameters = new { search, survey };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                wordings = db.Query<Wording>(query, parameters).ToList();
            }

            return wordings;
        }

        /// <summary>
        /// Returns all wording records used in a survey that contain the search parameter.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static List<Wording> GetSurveyPstP(string search, string survey)
        {
            List<Wording> wordings = new List<Wording>();
            string query = "SELECT ID AS WordID, 'PstP' AS FieldName, Wording AS WordingText FROM Wordings.FN_GetSurveyPstPMatching(@search, @survey) ORDER BY ID";

            var parameters = new { search, survey };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                wordings = db.Query<Wording>(query, parameters).ToList();
            }

            return wordings;
        }

        /// <summary>
        /// Returns the jagged array of words that should be considered the same.
        /// </summary>
        /// <returns></returns>
        public static string[][] GetSimilarWords()
        {
            List<string> list = new List<string>();
            string[][] similarWords = new string[0][];
            string[] words;
            string query = "SELECT word FROM Wordings.FN_GetSimilarWords()";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                list = db.Query<string>(query).ToList();
            }

            for (int i =1;i <list.Count-1;i++)
            {
                Array.Resize(ref similarWords, i);
                words = new string[list[i-1].Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries).Length];
                words = list[i-1].Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                similarWords[i - 1] = words;
            }

            return similarWords;
        }

        /// <summary>
        /// Returns the list of SimilarWords records.
        /// </summary>
        /// <returns></returns>
        public static List<SimilarWordsRecord> GetSimilarWordings()
        {
            List<SimilarWordsRecord> records = new List<SimilarWordsRecord>();        
            string query = "SELECT ID, word as Words FROM Wordings.FN_GetSimilarWords()";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                records = conn.Query<SimilarWordsRecord>(query).ToList();
            }
            return records;
        }
    }
}
