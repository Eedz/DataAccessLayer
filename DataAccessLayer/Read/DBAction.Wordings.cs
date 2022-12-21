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
    public static partial class DBAction
    {
        /// <summary>
        /// Returns all wording records.
        /// </summary>
        /// <returns></returns>
        public static List<Wording> GetWordings()
        {
            List<Wording> wordings = new List<Wording>();
            Wording w;
            string query = "SELECT * FROM Wordings.FN_GetAllWordings() ORDER BY FieldName, ID";

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
                            w = new Wording
                            {
                                WordID = (int)rdr["ID"],
                                FieldName = (string)rdr["FieldName"],
                                WordingText = (string)rdr["Wording"]
                            };

                            wordings.Add(w);
                        }
                    }
                }
                catch
                {
                    
                }
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                wordings = db.Query<ResponseSet>(sql, parameters).ToList();
            }

            return wordings;
        }

        /// <summary>
        /// Returns all response sets of a specific type.
        /// </summary>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        public static List<ResponseSet> GetResponseSets(List<string> criteria)
        {
            List<ResponseSet> setList = new List<ResponseSet>();

            string query = "SELECT * FROM qryRespOptions ";

            if (criteria.Count > 0) query += "WHERE ";           
            for (int i =0; i <criteria.Count;i ++)
            {
                query += "RespOptions LIKE '%' + @criteria" + i + " + '%' OR ";
            }
            query = query.Substring(0, query.Length - 3);
            query += "ORDER BY RespName";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                for (int i = 0; i < criteria.Count; i++)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@criteria" + i, criteria[i]);
                }

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ResponseSet rs = new ResponseSet
                            {
                                RespSetName = rdr.SafeGetString("RespName"),
                                FieldName = "RespOptions",
                                RespList = rdr.SafeGetString("RespOptions")

                            };

                            setList.Add(rs);
                        }
                    }
                }
                catch
                {

                }
            }


            return setList;
        }

        /// <summary>
        /// Returns all response sets of a specific type.
        /// </summary>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        public static List<ResponseSet> GetResponseSets(List<string> criteria, bool exactMatch)
        {
            List<ResponseSet> setList = new List<ResponseSet>();

            string query = "SELECT * FROM qryRespOptions ";

            if (criteria.Count > 0) query += "WHERE ";
            for (int i = 0; i < criteria.Count; i++)
            {
                if (exactMatch)
                    query += "RespOptions = @criteria" + i + " OR ";
                else 
                    query += "RespOptions LIKE '%' + @criteria" + i + " + '%' OR ";
            }
            query = query.Substring(0, query.Length - 3);
            query += "ORDER BY RespName";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                for (int i = 0; i < criteria.Count; i++)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@criteria" + i, criteria[i]);
                }

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ResponseSet rs = new ResponseSet
                            {
                                RespSetName = rdr.SafeGetString("RespName"),
                                FieldName = "RespOptions",
                                RespList = rdr.SafeGetString("RespOptions")

                            };

                            setList.Add(rs);
                        }
                    }
                }
                catch
                {

                }
            }


            return setList;
        }

        /// <summary>
        /// Returns all response sets of a specific type.
        /// </summary>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        public static List<ResponseSet> GetNonResponseSets(List<string> criteria)
        {
            List<ResponseSet> setList = new List<ResponseSet>();

            string query = "SELECT * FROM qryNonRespOptions ";

            if (criteria.Count > 0) query += "WHERE ";

            for (int i = 0; i < criteria.Count; i++)
            {
                query += "NRCodes LIKE '%' + @criteria" + i + " + '%' OR ";
            }
            query = query.Substring(0, query.Length - 3);
            query += "ORDER BY NRName";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                for (int i = 0; i < criteria.Count; i++)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@criteria" + i, criteria[i]);
                }

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ResponseSet rs = new ResponseSet
                            {
                                RespSetName = rdr.SafeGetString("NRName"),
                                FieldName = "NRCodes",
                                RespList = rdr.SafeGetString("NRCodes")
                            };

                            setList.Add(rs);
                        }
                    }
                }
                catch
                {

                }
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

            string query = "SELECT * FROM qryNonRespOptions ";

            if (criteria.Count > 0) query += "WHERE ";

            for (int i = 0; i < criteria.Count; i++)
            {
                if (exactMatch)
                    query += "NRCodes = @criteria" + i + " OR ";
                else
                    query += "NRCodes LIKE '%' + @criteria" + i + " + '%' OR ";
            }
            query = query.Substring(0, query.Length - 3);
            query += "ORDER BY NRName";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                for (int i = 0; i < criteria.Count; i++)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@criteria" + i, criteria[i]);
                }

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ResponseSet rs = new ResponseSet
                            {
                                RespSetName = rdr.SafeGetString("NRName"),
                                FieldName = "NRCodes",
                                RespList = rdr.SafeGetString("NRCodes")
                            };

                            setList.Add(rs);
                        }
                    }
                }
                catch
                {

                }
            }
            return setList;
        }

        /// <summary>
        /// Returns the text of a particular wording.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="wordID"></param>
        /// <returns></returns>
        public static string GetWordingText(string field, int wordID)
        {
            string text = "";
            string query = "SELECT Wordings.FN_GetWordingText(@fieldname, @wordID)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@fieldname", field);
                sql.SelectCommand.Parameters.AddWithValue("@wordID", wordID);

                try
                {
                    text = (string)sql.SelectCommand.ExecuteScalar();

                }
                catch (Exception)
                {
                    
                }
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
            string query;
        
            query = "SELECT Wordings.FN_GetResponseText(@respname)";
            
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@respname", respname);

                try
                {
                    text = (string)sql.SelectCommand.ExecuteScalar();     
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return text;
        }

        /// <summary>
        /// Returns the text of a specified non-response set.
        /// </summary>
        /// <param name="respName"></param>
        /// <returns></returns>
        public static string GetNonResponseText(string nrname)
        {
            string text = "";
            string query;
          
            query = "SELECT Wordings.FN_GetNonResponseText(@nrname)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@nrname", nrname);

                try
                {
                    text = (string)sql.SelectCommand.ExecuteScalar();
                }
                catch
                {

                }
            }

            return text;
        }

        /// <summary>
        /// Returns a list of WordingUsage objects which represent the questions that use the provided field/wordID combination.
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="wordID"></param>
        /// <returns></returns>
        public static List<WordingUsage> GetWordingUsage(string fieldname, int wordID)
        {
            List<WordingUsage> qList = new List<WordingUsage>();
            WordingUsage sq;
            string query = "SELECT * FROM Wordings.FN_GetWordingUsage (@field, @wordID)";
           
            if (query == "")
                return null;

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@field", fieldname);
                sql.SelectCommand.Parameters.AddWithValue("@wordID", wordID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            sq = new WordingUsage
                            {
                                VarName = (string)rdr["VarName"],
                                VarLabel = (string)rdr["VarLabel"],
                                SurveyCode = (string)rdr["Survey"],
                                WordID = wordID,
                                Qnum = (string)rdr["Qnum"],
                                Locked = (bool)rdr["Locked"]
                                
                            };

                            qList.Add(sq);
                        }
                    }
                }
                catch
                {
                   
                }
            }

            return qList;
        }

        /// <summary>
        /// Returns a list of WordingUsage objects which represent the questions that use the provided field/wordID combination.
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="wordID"></param>
        /// <returns></returns>
        public static List<ResponseUsage> GetResponseUsage(string fieldname, string respName)
        {
            List<ResponseUsage> qList = new List<ResponseUsage>();
            ResponseUsage sq;
            string query = "SELECT * FROM Wordings.FN_GetResponseUsage (@field, @wordID)";

            if (query == "")
                return null;

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@field", fieldname);
                sql.SelectCommand.Parameters.AddWithValue("@wordID", respName);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            sq = new ResponseUsage
                            {
                                VarName = (string)rdr["VarName"],
                                VarLabel = (string)rdr["VarLabel"],
                                SurveyCode = (string)rdr["Survey"],
                                RespName = respName,
                                Qnum = (string)rdr["Qnum"],
                                Locked = (bool)rdr["Locked"]

                            };

                            qList.Add(sq);
                        }
                    }
                }
                catch
                {
                    
                }
            }

            return qList;
        }

        /// <summary>
        /// Returns all wording records.
        /// </summary>
        /// <returns></returns>
        public static List<Wording> GetSurveyPreP(string search, string survey)
        {
            List<Wording> wordings = new List<Wording>();
            Wording w;
            string query = "SELECT * FROM Wordings.FN_GetSurveyPrePMatching(@search, @survey) ORDER BY ID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@search", search);
                sql.SelectCommand.Parameters.AddWithValue("@survey", survey);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            w = new Wording
                            {
                                WordID = (int)rdr["ID"],
                                FieldName = "PreP",
                                WordingText = (string)rdr["Wording"]

                            };

                            wordings.Add(w);
                        }
                    }
                }
                catch
                {

                }
            }

            return wordings;
        }

        /// <summary>
        /// Returns all wording records.
        /// </summary>
        /// <returns></returns>
        public static List<Wording> GetSurveyPreI(string search, string survey)
        {
            List<Wording> wordings = new List<Wording>();
            Wording w;
            string query = "SELECT * FROM Wordings.FN_GetSurveyPreIMatching(@search, @survey) ORDER BY ID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@search", search);
                sql.SelectCommand.Parameters.AddWithValue("@survey", survey);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            w = new Wording
                            {
                                WordID = (int)rdr["ID"],
                                FieldName = "PreI",
                                WordingText = (string)rdr["Wording"]

                            };

                            wordings.Add(w);
                        }
                    }
                }
                catch
                {

                }
            }

            return wordings;
        }

        /// <summary>
        /// Returns all wording records.
        /// </summary>
        /// <returns></returns>
        public static List<Wording> GetSurveyPreA(string search, string survey)
        {
            List<Wording> wordings = new List<Wording>();
            Wording w;
            string query = "SELECT * FROM Wordings.FN_GetSurveyPreAMatching(@search, @survey) ORDER BY ID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@search", search);
                sql.SelectCommand.Parameters.AddWithValue("@survey", survey);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            w = new Wording
                            {
                                WordID = (int)rdr["ID"],
                                FieldName = "PreA",
                                WordingText = (string)rdr["Wording"]

                            };

                            wordings.Add(w);
                        }
                    }
                }
                catch
                {

                }
            }

            return wordings;
        }

        /// <summary>
        /// Returns all wording records.
        /// </summary>
        /// <returns></returns>
        public static List<Wording> GetSurveyLitQ(string search, string survey)
        {
            List<Wording> wordings = new List<Wording>();
            Wording w;
            string query = "SELECT * FROM Wordings.FN_GetSurveyLitQMatching(@search, @survey) ORDER BY ID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@search", search);
                sql.SelectCommand.Parameters.AddWithValue("@survey", survey);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            w = new Wording
                            {
                                WordID = (int)rdr["ID"],
                                FieldName = "LitQ",
                                WordingText = (string)rdr["Wording"]

                            };

                            wordings.Add(w);
                        }
                    }
                }
                catch
                {

                }
            }

            return wordings;
        }

        /// <summary>
        /// Returns all wording records.
        /// </summary>
        /// <returns></returns>
        public static List<Wording> GetSurveyPstI(string search, string survey)
        {
            List<Wording> wordings = new List<Wording>();
            Wording w;
            string query = "SELECT * FROM Wordings.FN_GetSurveyPstIMatching(@search, @survey) ORDER BY ID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@search", search);
                sql.SelectCommand.Parameters.AddWithValue("@survey", survey);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            w = new Wording
                            {
                                WordID = (int)rdr["ID"],
                                FieldName = "PstI",
                                WordingText = (string)rdr["Wording"]

                            };

                            wordings.Add(w);
                        }
                    }
                }
                catch
                {

                }
            }

            return wordings;
        }

        /// <summary>
        /// Returns all wording records.
        /// </summary>
        /// <returns></returns>
        public static List<Wording> GetSurveyPstP(string search, string survey)
        {
            List<Wording> wordings = new List<Wording>();
            Wording w;
            string query = "SELECT * FROM Wordings.FN_GetSurveyPstPMatching(@search, @survey) ORDER BY ID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@search", search);
                sql.SelectCommand.Parameters.AddWithValue("@survey", survey);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            w = new Wording
                            {
                                WordID = (int)rdr["ID"],
                                FieldName = "PstP",
                                WordingText = (string)rdr["Wording"]

                            };

                            wordings.Add(w);
                        }
                    }
                }
                catch
                {

                }
            }

            return wordings;
        }

        /// <summary>
        /// Returns the jagged array of words that should be considered the same.
        /// </summary>
        /// <returns></returns>
        public static string[][] GetSimilarWords()
        {
            string[][] similarWords = new string[0][];
            string[] words;
            string currentList;
            int i = 1;
            string query = "SELECT * FROM Wordings.FN_GetSimilarWords()";

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
                            Array.Resize(ref similarWords, i);
                            currentList = (string)rdr["word"];
                            words = new string[currentList.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries).Length];
                            words = currentList.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                            similarWords[i - 1] = words;
                            i++;
                        }
                    }
                }
                catch (Exception)
                {

                }

            }
            return similarWords;
        }

        /// <summary>
        /// Returns the jagged array of words that should be considered the same.
        /// </summary>
        /// <returns></returns>
        public static List<SimilarWordsRecord> GetSimilarWordings()
        {
            List<SimilarWordsRecord> records = new List<SimilarWordsRecord>();
           
            string query = "SELECT * FROM Wordings.FN_GetSimilarWords()";

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
                            SimilarWordsRecord record = new SimilarWordsRecord();
                            record.ID = (int)rdr["ID"];
                            record.Words = rdr.SafeGetString("word");
                            
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
    }
}
