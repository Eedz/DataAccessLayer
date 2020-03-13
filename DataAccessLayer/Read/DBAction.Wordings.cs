using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;

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
                                WordID = (int)rdr["WordID"],
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
        /// Returns all wordings of a specific type.
        /// </summary>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        public static List<Wording> GetWordings(string fieldname)
        {
            List<Wording> wordings = new List<Wording>();
            Wording w;
            string query = "SELECT * FROM Wordings.FN_GetWordings(@field) ORDER BY WordID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@field", fieldname);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            w = new Wording
                            {
                                WordID = (int)rdr["WordID"],
                                FieldName = fieldname,
                                WordingText = (string) rdr["WordingText"]

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
        /// Returns all response sets of a specific type.
        /// </summary>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        public static List<ResponseSet> GetResponseSets(string fieldname)
        {
            List<ResponseSet> setList = new List<ResponseSet>();
            ResponseSet rs;
            string query = "SELECT * FROM Wordings.FN_GetResponseSets(@field) ORDER BY RespName";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@field", fieldname);
                
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            rs = new ResponseSet
                            {
                                RespSetName = (string)rdr["RespName"],
                                FieldName = fieldname,
                                RespList = (string)rdr["ResponseList"]

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
                catch
                {
                    int i = 0;
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
    }
}
