using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ITCLib;

namespace ITCLib
{
    // TODO: sort comments
    partial class DBAction
    {
        //
        // Get Methods
        //

        //
        // Comments
        //
        /// <summary>
        /// Returns the complete list of Notes.
        /// </summary>
        /// <param name="SurvID"></param>
        /// <returns></returns>
        public static List<Note> GetNotes()
        {
            List<Note> ns = new List<Note>();
            Note n;
            string query = "SELECT * FROM Comments.FN_GetNotes";

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
                            n = new Note
                            {
                                ID = (int)rdr["ID"],
                                NoteText = (string) rdr["Notes"]
                            };

                            ns.Add(n);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }
            return ns;
        }

        //
        // Deleted Question Comments
        //
        /// <summary>
        /// Returns all question comments with the specified note.
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static List<QuestionComment> GetDeletedComments(string survey, string varname)
        {
            List<QuestionComment> cs = new List<QuestionComment>();
            QuestionComment c;
            string query = "SELECT * FROM Comments.FN_GetDeletedComments (@survey, @varname)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", survey);
                sql.SelectCommand.Parameters.AddWithValue("@varname", varname);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            c = new QuestionComment
                            {
                                Notes = new Note((int)rdr["ID"], (string)rdr["Notes"]),
                                Survey = (string)rdr["Survey"],
                                VarName = (string)rdr["VarName"],
                                CID = (int)rdr["CID"],
                                NoteDate = (DateTime)rdr["NoteDate"],
                                NoteInit = (int)rdr["NoteInit"],
                                Name = (string)rdr["Name"],
                                SourceName = (string)rdr["SourceName"],
                                NoteType = (string)rdr["NoteType"],
                                Source = (string)rdr["Source"],
                            };

                            cs.Add(c);
                        }
                    }
                }
                catch (Exception)
                {
                    int i = 0;
                }
            }

            return cs;
        }


        //
        // Question Comments
        //

        /// <summary>
        /// Returns all question comments with the specified note.
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static List<QuestionComment> GetQuesCommentsByCID (int CID)
        {
            List<QuestionComment> cs = new List<QuestionComment>();
            QuestionComment c;
            string query = "SELECT * FROM Comments.FN_GetQuesCommentsByCID (@cid)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@cid", CID);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            c = new QuestionComment
                            {
                                Notes = new Note((int)rdr["ID"], (string)rdr["Notes"]),
                                QID = (int)rdr["QID"],
                                SurvID = (int)rdr["SurvID"],
                                Survey = (string)rdr["Survey"],
                                VarName = (string)rdr["VarName"],
                                CID = (int)rdr["CID"],
                                NoteDate = (DateTime)rdr["NoteDate"],
                                NoteInit = (int)rdr["NoteInit"],
                                Name = (string)rdr["Name"],
                                SourceName = (string)rdr["SourceName"],
                                NoteType = (string)rdr["NoteType"],
                                ShortNoteType = (string)rdr["ShortForm"],
                                Source = (string)rdr["Source"],
                            };

                            cs.Add(c);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }

            return cs;
        }

        /// <summary>
        /// Returns question comments for the specified survey.
        /// </summary>
        /// <param name="SurvID"></param>
        /// <returns></returns>
        public static List<QuestionComment> GetQuesCommentsBySurvey(int SurvID)
        {
            List<QuestionComment> cs = new List<QuestionComment>();
            QuestionComment c;
            string query = "SELECT * FROM Comments.FN_GetQuesCommentsBySurvID(@sid)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@sid", SurvID);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            c = new QuestionComment
                            {
                                Notes = new Note((int)rdr["ID"], (string)rdr["Notes"]),
                                QID = (int)rdr["QID"],
                                SurvID = (int)rdr["SurvID"],
                                Survey = (string)rdr["Survey"],
                                VarName = (string)rdr["VarName"],
                                CID = (int)rdr["CID"],
                                NoteDate = (DateTime)rdr["NoteDate"],
                                NoteInit = (int)rdr["NoteInit"],
                                Name = (string)rdr["Name"],
                                SourceName = (string)rdr["SourceName"],
                                NoteType = (string)rdr["NoteType"],
                                ShortNoteType = (string)rdr["ShortForm"],
                                Source = (string)rdr["Source"],
                            };

                            cs.Add(c);
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return cs;
        }

        /// <summary>
        /// Returns question comments for the specified survey.
        /// </summary>
        /// <param name="survey">Survey object.</param>
        /// <returns></returns>
        public static List<QuestionComment> GetQuesCommentsBySurvey(Survey survey)
        {
            return GetQuesCommentsBySurvey(survey.SID);
        }

        /// <summary>
        /// Returns comments for the specified question.
        /// </summary>
        /// <param name="QID"></param>
        /// <returns></returns>
        public static List<QuestionComment> GetQuesCommentsByQID(int QID)
        {
            List<QuestionComment> comments = new List<QuestionComment>();
            QuestionComment c;
            string query = "SELECT * FROM Comments.FN_GetQuesCommentsByQID(@qid)";

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
                            c = new QuestionComment
                            {
                                Notes = new Note((int)rdr["ID"], (string)rdr["Notes"]),
                                QID = (int)rdr["QID"],
                                Survey = (string)rdr["Survey"],
                                VarName = (string)rdr["VarName"],
                                CID = (int)rdr["CID"],
                                NoteDate = (DateTime)rdr["NoteDate"],
                                NoteInit = (int)rdr["NoteInit"],
                                Name = (string)rdr["Name"],
                                NoteType = (string)rdr["NoteType"],
                                ShortNoteType = (string)rdr["ShortForm"],
                                SurvID = (int)rdr["SurvID"]
                            };
                            if (!rdr.IsDBNull(rdr.GetOrdinal("SourceName"))) c.SourceName = (string)rdr["SourceName"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Source"))) c.Source = (string)rdr["Source"];

                            comments.Add(c);
                        }
                    }
                }
                catch (Exception)
                {

                    return comments;
                }

            }
            return comments;
        }

        /// <summary>
        /// Returns comments for the specified question.
        /// </summary>
        /// <param name="QID"></param>
        /// <returns></returns>
        public static List<QuestionComment> GetQuesCommentsByQID(SurveyQuestion question)
        {
            return GetQuesCommentsByQID(question.ID);
        }

        // TODO replace with server function
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SurvID"></param>
        /// <param name="commentTypes"></param>
        /// <param name="commentDate"></param>
        /// <param name="commentAuthors"></param>
        /// <param name="commentSources"></param>
        /// <returns></returns>
        public static List<QuestionComment> GetQuesCommentsBySurvey(int SurvID, List<string> commentTypes = null, DateTime? commentDate = null, List<int> commentAuthors = null, List<string> commentSources = null)
        {
            List<QuestionComment> cs = new List<QuestionComment>();
            QuestionComment c;
            string query = "SELECT * FROM qryCommentsQues WHERE SurvID = @sid";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@sid", SurvID);

                if (commentTypes != null && commentTypes.Count != 0)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@commentTypes", String.Join(",", commentTypes));
                    query += " AND NoteType IN (@commentTypes)";
                }

                if (commentDate != null)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@commentDate", commentDate);
                    query += " AND NoteDate >= (@commentDate)";
                }

                if (commentAuthors != null && commentAuthors.Count != 0)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@commentAuthors", String.Join(",", commentAuthors));
                    query += " AND NoteInit IN (@commentAuthors)";
                }

                if (commentSources != null && commentSources.Count != 0)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@commentSources", String.Join(",", commentSources));
                    query += " AND Source IN (@commentSources)";
                }

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            c = new QuestionComment
                            {
                                Notes = new Note((int)rdr["ID"], (string)rdr["Notes"]),
                                QID = (int)rdr["QID"],
                                Survey = (string)rdr["Survey"],
                                VarName = (string)rdr["VarName"],
                                CID = (int)rdr["CID"],
                                NoteDate = (DateTime)rdr["NoteDate"],
                                NoteInit = (int)rdr["NoteInit"],
                                Name = (string)rdr["Name"],
                                SourceName = (string)rdr["SourceName"],
                                NoteType = (string)rdr["NoteType"],
                                ShortNoteType = (string)rdr["ShortForm"],
                                Source = (string)rdr["Source"],
                                SurvID = (int)rdr["SurvID"]
                            };
                            cs.Add(c);
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return cs;
        }


        //
        // Survey Comments
        //

        /// <summary>
        /// Returns all survey comments with the specified note.
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static List<SurveyComment> GetSurvCommentsByCID(int CID)
        {
            List<SurveyComment> cs = new List<SurveyComment>();
            SurveyComment c;
            string query = "SELECT * FROM Comments.FN_GetSurvCommentsByID(@cid)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@cid", CID);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            c = new SurveyComment
                            {
                                Notes = new Note((int)rdr["ID"], (string)rdr["Notes"]),
                                SurvID = (int)rdr["SurvID"],
                                CID = (int)rdr["CID"],
                                Survey = (string)rdr["Survey"],
                                NoteDate = (DateTime)rdr["NoteDate"],
                                NoteInit = (int)rdr["NoteInit"],
                                Name = (string)rdr["Name"],
                                SourceName = (string)rdr["SourceName"],
                                NoteType = (string)rdr["NoteType"],
                                Source = (string)rdr["Source"],
                                
                            };

                            cs.Add(c);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }

            return cs;
        }


        /// <summary>
        /// Returns survey comments for the specified survey.
        /// </summary>
        /// <param name="SurvID"></param>
        /// <returns></returns>
        public static List<SurveyComment> GetSurvCommentsBySurvey(int SurvID)
        {
            List<SurveyComment> cs = new List<SurveyComment>();
            SurveyComment c;
            string query = "SELECT * FROM Comments.FN_GetSurvCommentsBySurvID (@sid)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@sid", SurvID);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            c = new SurveyComment
                            {
                                Notes = new Note((int)rdr["ID"], (string)rdr["Notes"]),
                                SurvID = (int)rdr["SurvID"],
                                Survey = (string)rdr["Survey"],
                                CID = (int)rdr["CID"],
                            };
                            if (!rdr.IsDBNull(rdr.GetOrdinal("NoteDate"))) c.NoteDate = (DateTime)rdr["NoteDate"];
                            c.NoteInit = (int)rdr["NoteInit"];
                            c.Name = (string)rdr["Name"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("SourceName"))) c.SourceName = (string)rdr["SourceName"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("NoteType"))) c.NoteType = (string)rdr["NoteType"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Source"))) c.Source = (string)rdr["Source"];



                            cs.Add(c);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return cs;
        }

        /// <summary>
        /// Returns survey comments for the specified survey.
        /// </summary>
        /// <param name="SurvID"></param>
        /// <returns></returns>
        public static List<SurveyComment> GetSurvCommentsBySurvey(Survey survey)
        {
            return GetSurvCommentsBySurvey(survey.SID);
        }

        //
        // Comment Info
        //

        /// <summary>
        /// Returns all of the comment types that exist for a particular survey.
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static List<string> GetQuesCommentTypes(int survID)
        {
            List<string> types = new List<string>();
            string query = "SELECT * FROM Comments.FN_GetQuesCommentTypesBySurvID (@survID)";

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
                            types.Add((string)rdr["NoteType"]);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            return types;
        }

        /// <summary>
        /// Returns all of the comment types that exist for a particular survey.
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static List<string> GetQuesCommentTypes(string survey)
        {
            List<string> types = new List<string>();
            string query = "SELECT * FROM Comments.FN_GetQuesCommentTypesBySurvey (@survey)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", survey);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            types.Add((string)rdr["NoteType"]);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            return types;
        }


        /// <summary>
        /// Returns all question comment authors that exist for a particular survey.
        /// </summary>
        /// <param name="SurvID"></param>
        /// <returns></returns>
        public static List<Person> GetCommentAuthors(int SurvID)
        {
            List<Person> ps = new List<Person>();
            Person p;
            string query = "SELECT * FROM Comments.FN_GetQuesCommentAuthorsBySurvID (@survID)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survID", SurvID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            p = new Person((string)rdr["Name"], (int)rdr["NoteInit"]);

                            ps.Add(p);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                    return null;
                }
            }
            return ps;
        }

        /// <summary>
        /// Returns all question comment authors that exist for a particular survey.
        /// </summary>
        /// <param name="SurveyCode"></param>
        /// <returns></returns>
        public static List<Person> GetCommentAuthors(string SurveyCode)
        {
            List<Person> ps = new List<Person>();
            Person p;
            string query = "SELECT * FROM Comments.FN_GetQuesCommentAuthorsBySurvID (@survey)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", SurveyCode);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            p = new Person((string)rdr["Name"], (int)rdr["NoteInit"]);

                            ps.Add(p);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                    //return null;
                }
            }
            return ps;
        }

        /// <summary>
        /// Returns all question comment source names that exist for a particular survey.
        /// </summary>
        /// <param name="SurveyCode"></param>
        /// <returns></returns>
        public static List<string> GetCommentSourceNames(string SurveyCode)
        {
            List<string> sourceList = new List<string>();
            string query = "SELECT * FROM Comments.FN_GetQuesCommentSourceNamesBySurvey (@survey)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", SurveyCode);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            sourceList.Add((string)rdr["SourceName"]);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            return sourceList;
        }

        
        //
        // Fill methods
        //
        

        /// <summary>
        /// // TODO replace with server function
        /// </summary>
        /// <param name="s"></param>
        /// <param name="commentTypes"></param>
        /// <param name="commentDate"></param>
        /// <param name="commentAuthors"></param>
        /// <param name="commentSources"></param>
        public static void FillCommentsBySurvey(Survey s, List<string> commentTypes = null, DateTime? commentDate = null, List<int> commentAuthors = null, List<string> commentSources = null, string commentText = null)
        {
            QuestionComment c;
            string query = "SELECT * FROM qryCommentsQues WHERE SurvID = @sid";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.Parameters.AddWithValue("@sid", s.SID);

                if (commentTypes != null && commentTypes.Count != 0)
                {
                    query += " AND (";
                    for (int i = 0; i < commentTypes.Count; i++)
                    {
                        sql.SelectCommand.Parameters.AddWithValue("@commentTypes" + i, commentTypes[i]);
                        query += " NoteType = @commentTypes" + i + " OR ";
                    }
                    query = Utilities.TrimString(query, " OR ");
                    query += ")";
                }

                if (commentDate != null)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@commentDate", commentDate.Value);
                    query += " AND NoteDate >= @commentDate";
                }

                if (commentAuthors != null && commentAuthors.Count != 0)
                {
                    query += " AND (";
                    for (int i = 0; i < commentAuthors.Count; i++)
                    {
                        sql.SelectCommand.Parameters.AddWithValue("@commentAuthors" + i, commentAuthors[i]);
                        query += " NoteInit = @commentAuthors" + i + " OR ";
                    }
                    query = Utilities.TrimString(query, " OR ");
                    query += ")";
                }

                if (commentSources != null && commentSources.Count != 0)
                {
                    query += " AND (";
                    for (int i = 0; i < commentSources.Count; i++)
                    {
                        sql.SelectCommand.Parameters.AddWithValue("@commentSources" + i, commentSources[i]);
                        query += " SourceName = @commentSources" + i + " OR ";
                    }
                    query = Utilities.TrimString(query, " OR ");
                    query += ")";
                }

                if (commentText != null)
                {
                    query += " AND ";
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", commentText);
                    query += " Notes LIKE '%' + @commentText + '%'";
                }

                query += " ORDER BY NoteDate ASC";

                sql.SelectCommand.CommandText = query;
                sql.SelectCommand.Connection = conn;

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            c = new QuestionComment
                            {
                                Notes = new Note((int)rdr["ID"], (string)rdr["Notes"]),
                                QID = (int)rdr["QID"],
                                Survey = (string)rdr["Survey"],
                                VarName = (string)rdr["VarName"],
                                CID = (int)rdr["CID"],
                                NoteDate = (DateTime)rdr["NoteDate"],
                                NoteInit = (int)rdr["NoteInit"],
                                Name = (string)rdr["Name"],
                                NoteType = (string)rdr["NoteType"],
                                ShortNoteType = (string)rdr["ShortForm"],
                                SurvID = (int)rdr["SurvID"]
                            };
                            if (!rdr.IsDBNull(rdr.GetOrdinal("SourceName"))) c.SourceName = (string)rdr["SourceName"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Source"))) c.Source = (string)rdr["Source"];
                        
                            s.QuestionByID((int)rdr["QID"]).Comments.Add(c);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// // TODO replace with server function
        /// </summary>
        /// <param name="s"></param>
        /// <param name="commentTypes"></param>
        /// <param name="commentDate"></param>
        /// <param name="commentAuthors"></param>
        /// <param name="commentSources"></param>
        public static void FillCommentsBySurvey(ReportSurvey s)
        {
            QuestionComment c;
            string query = "SELECT * FROM qryCommentsQues WHERE SurvID = @sid";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.Parameters.AddWithValue("@sid", s.SID);

                if (s.CommentFields != null && s.CommentFields.Count != 0)
                {
                    query += " AND (";
                    for (int i = 0; i < s.CommentFields.Count; i++)
                    {
                        sql.SelectCommand.Parameters.AddWithValue("@commentTypes" + i, s.CommentFields[i]);
                        query += " NoteType = @commentTypes" + i + " OR ";
                    }
                    query = Utilities.TrimString(query, " OR ");
                    query += ")";
                }

                if (s.CommentDate != null)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@commentDate", s.CommentDate.Value);
                    query += " AND NoteDate >= @commentDate";
                }

                if (s.CommentAuthors != null && s.CommentAuthors.Count != 0)
                {
                    query += " AND (";
                    for (int i = 0; i < s.CommentAuthors.Count; i++)
                    {
                        sql.SelectCommand.Parameters.AddWithValue("@commentAuthors" + i, s.CommentAuthors[i]);
                        query += " NoteInit = @commentAuthors" + i + " OR ";
                    }
                    query = Utilities.TrimString(query, " OR ");
                    query += ")";
                }

                if (s.CommentSources != null && s.CommentSources.Count != 0)
                {
                    query += " AND (";
                    for (int i = 0; i < s.CommentSources.Count; i++)
                    {
                        sql.SelectCommand.Parameters.AddWithValue("@commentSources" + i, s.CommentSources[i]);
                        query += " SourceName = @commentSources" + i + " OR ";
                    }
                    query = Utilities.TrimString(query, " OR ");
                    query += ")";
                }

                if (s.CommentText != null)
                {
                    query += " AND ";
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", s.CommentText);
                    query += " Notes LIKE '%' + @commentText + '%'";
                }

                query += " ORDER BY NoteDate ASC";

                sql.SelectCommand.CommandText = query;
                sql.SelectCommand.Connection = conn;

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            c = new QuestionComment
                            {
                                Notes = new Note((int)rdr["ID"], (string)rdr["Notes"]),
                                QID = (int)rdr["QID"],
                                Survey = (string)rdr["Survey"],
                                VarName = (string)rdr["VarName"],
                                CID = (int)rdr["CID"],
                                NoteDate = (DateTime)rdr["NoteDate"],
                                NoteInit = (int)rdr["NoteInit"],
                                Name = (string)rdr["Name"],
                                NoteType = (string)rdr["NoteType"],
                                ShortNoteType = (string)rdr["ShortForm"],
                                SurvID = (int)rdr["SurvID"]
                            };
                            if (!rdr.IsDBNull(rdr.GetOrdinal("SourceName"))) c.SourceName = (string)rdr["SourceName"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Source"))) c.Source = (string)rdr["Source"];

                            s.QuestionByID((int)rdr["QID"]).Comments.Add(c);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                    return;
                }
            }
        }

    }
}
