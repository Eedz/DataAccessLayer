using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using ITCLib;

namespace ITCLib
{
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
        /// <returns></returns>
        public static List<NoteRecord> GetNotes()
        {
            List<NoteRecord> notes = new List<NoteRecord>();

            string sql = "SELECT ID, Notes AS NoteText FROM Comments.FN_GetNotes()";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                notes = db.Query<NoteRecord>(sql).ToList();
            }

            return notes;
        }

        /// <summary>
        /// Returns the complete list of Note Types.
        /// </summary>
        /// <returns></returns>
        public static List<CommentType> GetCommentTypes()
        {
            List<CommentType> types = new List<CommentType>();

            string sql = "SELECT ID, CommentType AS TypeName, ShortForm FROM qryCommentType";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                types = db.Query<CommentType>(sql).ToList();
            }

            return types;
        }

        /// <summary>
        /// Returns the list of Note Types used by a survey.
        /// </summary>
        /// <param name="SurvID"></param>
        /// <returns></returns>
        public static List<CommentType> GetCommentTypes(Survey survey)
        {
            List<CommentType> ns = new List<CommentType>();
            CommentType n;
            string query = "SELECT NoteTypeID, CommentType, ShortForm FROM qryCommentsQues WHERE Survey=@survey GROUP BY NoteTypeID, CommentType, ShortForm ORDER BY CommentType";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", survey.SurveyCode);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            n = new CommentType
                            {
                                ID = (int)rdr["NoteTypeID"],
                                TypeName = rdr.SafeGetString("CommentType"),
                                ShortForm = rdr.SafeGetString("ShortForm")
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
        public static List<DeletedComment> GetDeletedComments(string survey, string varname)
        {
            List<DeletedComment> cs = new List<DeletedComment>();
            DeletedComment c;
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
                            c = new DeletedComment
                            {
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], (string)rdr["Notes"]),
                                Survey = rdr.SafeGetString("Survey"),
                                VarName = rdr.SafeGetString("VarName"),
                                NoteDate = (DateTime)rdr["NoteDate"],
                                SourceName = rdr.SafeGetString("SourceName"),
                                Source = rdr.SafeGetString("Source"),
                            };

                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = (int)rdr["NoteTypeID"];
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");

                            cs.Add(c);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return cs;
        }


        /// <summary>
        /// Returns true if the provided comment exists for the survey.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static bool DeletedCommentExists(DeletedQuestion question, int CID)
        {
            bool exists = false;

            string query;

            query = "SELECT dbo.FN_DeletedCommentExists(@survey, @varname, @cid)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.Connection = conn;
                sql.SelectCommand.CommandType = CommandType.Text;
                sql.SelectCommand.CommandText = query;


                sql.SelectCommand.Parameters.AddWithValue("@survey", question.SurveyCode);
                sql.SelectCommand.Parameters.AddWithValue("@varname", question.VarName);
                sql.SelectCommand.Parameters.AddWithValue("@cid", CID);

                try
                {
                    exists = (bool)sql.SelectCommand.ExecuteScalar();
                }
                catch (Exception)
                {

                }
            }

            return exists;
        }

        //
        // Question Comments
        //

        /// <summary>
        /// Returns true if the provided comment exists for the survey.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static bool QuestionCommentExists(SurveyQuestion question, int CID)
        {
            bool exists = false;

            string query;

            query = "SELECT dbo.FN_QuestionCommentExists(@survey, @varname, @cid)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.Connection = conn;
                sql.SelectCommand.CommandType = CommandType.Text;
                sql.SelectCommand.CommandText = query;


                sql.SelectCommand.Parameters.AddWithValue("@survey", question.SurveyCode);
                sql.SelectCommand.Parameters.AddWithValue("@varname", question.VarName.VarName);
                sql.SelectCommand.Parameters.AddWithValue("@cid", CID);

                try
                {
                    exists = (bool)sql.SelectCommand.ExecuteScalar();
                }
                catch (Exception)
                {

                }
            }

            return exists;
        }

        /// <summary>
        /// Returns all question comments with the specified note.
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static List<QuestionCommentRecord> GetQuesCommentsByCID (int CID)
        {
            List<QuestionCommentRecord> cs = new List<QuestionCommentRecord>();
            QuestionCommentRecord c;
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
                            c = new QuestionCommentRecord
                            {
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], (string)rdr["Notes"]),
                                QID = (int)rdr["QID"],
                                SurvID = (int)rdr["SurvID"],
                                Survey = (string)rdr["Survey"],
                                VarName = (string)rdr["VarName"],
                                NoteDate = (DateTime)rdr["NoteDate"],
                                SourceName = rdr.SafeGetString("SourceName"),
                                Source = rdr.SafeGetString("Source"),
                            };
                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = rdr.SafeGetInt("NoteTypeID");
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");
                            c.NoteType.ShortForm = rdr.SafeGetString("ShortForm");

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
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], (string)rdr["Notes"]),
                                QID = (int)rdr["QID"],
                                SurvID = (int)rdr["SurvID"],
                                Survey = (string)rdr["Survey"],
                                VarName = (string)rdr["VarName"],
                                NoteDate = (DateTime)rdr["NoteDate"],
                                SourceName = (string)rdr["SourceName"],
                                Source = (string)rdr["Source"],
                            };
                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = (int)rdr["NoteTypeID"];
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");
                            c.NoteType.ShortForm = rdr.SafeGetString("ShortForm");

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
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], (string)rdr["Notes"]),
                                QID = (int)rdr["QID"],
                                SurvID = (int)rdr["SurvID"],
                                Survey = (string)rdr["Survey"],
                                VarName = (string)rdr["VarName"],
                                NoteDate = (DateTime)rdr["NoteDate"],
                                SourceName = (string)rdr["SourceName"],
                                Source = (string)rdr["Source"],
                            };
                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = rdr.SafeGetInt("NoteTypeID");
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");
                            c.NoteType.ShortForm = rdr.SafeGetString("ShortForm");

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
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], (string)rdr["Notes"]),
                                QID = (int)rdr["QID"],
                                SurvID = (int)rdr["SurvID"],
                                Survey = (string)rdr["Survey"],
                                VarName = (string)rdr["VarName"],
                                NoteDate = (DateTime)rdr["NoteDate"],
                                SourceName = (string)rdr["SourceName"],
                                Source = (string)rdr["Source"],
                            };
                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = (int)rdr["NoteTypeID"];
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");
                            c.NoteType.ShortForm = rdr.SafeGetString("ShortForm");

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
        /// Returns a list of Question Comments matching the provided criteria.
        /// </summary>
        /// <param name="SurvID"></param>
        /// <param name="commentTypes"></param>
        /// <param name="commentDate"></param>
        /// <param name="commentAuthors"></param>
        /// <param name="commentSources"></param>
        /// <returns></returns>
        public static List<QuestionComment> GetQuesComments(string survey = null, string varname = null, bool refVarName = false, string commentType = null, 
                DateTime? commentDateLower = null, DateTime? commentDateUpper = null, int commentAuthor = 0, string commentSource = null, string commentText = null)
        {
            List<QuestionComment> cs = new List<QuestionComment>();
            QuestionComment c;

            string query;
            if (refVarName)
                query = "SELECT * FROM dbo.FN_QuestionCommentSearchRefVar(" +
                        "@survey,@varname,@LDate,@UDate,@author,@commentText,@commentSource,@commentType)";
            else 
                query = "SELECT * FROM dbo.FN_QuestionCommentSearchVar(" +
                        "@survey, @varname, @LDate, @UDate, @author, @commentText, @commentSource, @commentType)";

          

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.Connection = conn;
                sql.SelectCommand.CommandType = CommandType.Text;
                sql.SelectCommand.CommandText = query;

                if (string.IsNullOrEmpty(survey))
                    sql.SelectCommand.Parameters.AddWithValue("@survey", DBNull.Value);
                else 
                    sql.SelectCommand.Parameters.AddWithValue("@survey", survey);

                if (string.IsNullOrEmpty(varname))
                    sql.SelectCommand.Parameters.AddWithValue("@varname", DBNull.Value);
                else 
                    sql.SelectCommand.Parameters.AddWithValue("@varname", varname + "%");

                if (commentDateLower==null)
                    sql.SelectCommand.Parameters.AddWithValue("@LDate", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@LDate", commentDateLower);

                if (commentDateLower == null)
                    sql.SelectCommand.Parameters.AddWithValue("@UDate", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@UDate", commentDateLower);

                if (commentAuthor==0)
                    sql.SelectCommand.Parameters.AddWithValue("@author", DBNull.Value);
                else 
                    sql.SelectCommand.Parameters.AddWithValue("@author", commentAuthor);

                if (string.IsNullOrEmpty(commentText))
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", DBNull.Value);
                else 
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", commentText);

                if (string.IsNullOrEmpty(commentSource))
                    sql.SelectCommand.Parameters.AddWithValue("@commentSource", DBNull.Value);
                else 
                    sql.SelectCommand.Parameters.AddWithValue("@commentSource", commentSource);

                if (string.IsNullOrEmpty(commentType))
                    sql.SelectCommand.Parameters.AddWithValue("@commentType", DBNull.Value);
                else 
                    sql.SelectCommand.Parameters.AddWithValue("@commentType", commentType);
                
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            c = new QuestionComment
                            {
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], rdr.SafeGetString("Notes")),
                                QID = (int)rdr["QID"],
                                SurvID = (int)rdr["SurvID"],
                                Survey = rdr.SafeGetString("Survey"),
                                VarName = rdr.SafeGetString("VarName"),
                                NoteDate = (DateTime)rdr["NoteDate"],
                                SourceName = rdr.SafeGetString("SourceName"),
                                Source = rdr.SafeGetString("Source"),
                            };
                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = (int)rdr["NoteTypeID"];
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");

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

        //
        // Survey Comments
        //

        /// <summary>
        /// Returns true if the provided comment exists for the survey.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static bool SurveyCommentExists(Survey s, int CID)
        {
            bool exists = false;

            string query;

            query = "SELECT dbo.FN_SurveyCommentExists(@survey,@cid)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.Connection = conn;
                sql.SelectCommand.CommandType = CommandType.Text;
                sql.SelectCommand.CommandText = query;

                
                sql.SelectCommand.Parameters.AddWithValue("@survey", s.SurveyCode);
                sql.SelectCommand.Parameters.AddWithValue("@cid", CID);

                try
                {
                    exists = (bool)sql.SelectCommand.ExecuteScalar();
                }
                catch (Exception)
                {
                    
                }
            }

            return exists;
        }

        /// <summary>
        /// Returns a list of Question Comments matching the provided criteria.
        /// </summary>
        /// <param name="SurvID"></param>
        /// <param name="commentTypes"></param>
        /// <param name="commentDate"></param>
        /// <param name="commentAuthors"></param>
        /// <param name="commentSources"></param>
        /// <returns></returns>
        public static List<SurveyComment> GetSurveyComments(string survey = null, string commentType = null,
                DateTime? commentDateLower = null, DateTime? commentDateUpper = null, int commentAuthor = 0, string commentSource = null, string commentText = null)
        {
            List<SurveyComment> cs = new List<SurveyComment>();
            SurveyComment c;

            string query;
           
            query = "SELECT * FROM dbo.FN_SurveyCommentSearch(" +
                    "@survey,@LDate,@UDate,@author,@commentText,@commentSource,@commentType)";
          
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.Connection = conn;
                sql.SelectCommand.CommandType = CommandType.Text;
                sql.SelectCommand.CommandText = query;

                if (string.IsNullOrEmpty(survey))
                    sql.SelectCommand.Parameters.AddWithValue("@survey", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@survey", survey);

                if (commentDateLower == null)
                    sql.SelectCommand.Parameters.AddWithValue("@LDate", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@LDate", commentDateLower);

                if (commentDateLower == null)
                    sql.SelectCommand.Parameters.AddWithValue("@UDate", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@UDate", commentDateLower);

                if (commentAuthor == 0)
                    sql.SelectCommand.Parameters.AddWithValue("@author", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@author", commentAuthor);

                if (string.IsNullOrEmpty(commentText))
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", commentText);

                if (string.IsNullOrEmpty(commentSource))
                    sql.SelectCommand.Parameters.AddWithValue("@commentSource", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@commentSource", commentSource);

                if (string.IsNullOrEmpty(commentType))
                    sql.SelectCommand.Parameters.AddWithValue("@commentType", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@commentType", commentType);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            c = new SurveyComment
                            {
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], rdr.SafeGetString("Notes")),
                                SurvID = (int)rdr["SID"],
                                Survey = rdr.SafeGetString("Survey"),                                
                                NoteDate = rdr.SafeGetDate("NoteDate").Value,
                                SourceName = (string)rdr["SourceName"],                                
                                Source = rdr.SafeGetString("Source"),
                            };

                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = (int)rdr["NoteTypeID"];
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");

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
        /// Returns all survey comments with the specified note.
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static List<SurveyCommentRecord> GetSurvCommentsByCID(int CID)
        {
            List<SurveyCommentRecord> cs = new List<SurveyCommentRecord>();
            SurveyCommentRecord c;
            string query = "SELECT * FROM Comments.FN_GetSurvCommentsByCID(@cid)";

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
                            c = new SurveyCommentRecord
                            {
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], rdr.SafeGetString("Notes")),
                                SurvID = (int)rdr["SurvID"],
                                Survey = rdr.SafeGetString("Survey"),
                                NoteDate = rdr.SafeGetDate("NoteDate").Value,
                                SourceName = (string)rdr["SourceName"],
                                Source = rdr.SafeGetString("Source"),
                            };

                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = (int)rdr["NoteTypeID"];
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");

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
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], rdr.SafeGetString("Notes")),
                                SurvID = (int)rdr["SurvID"],
                                Survey = rdr.SafeGetString("Survey"),
                                NoteDate = rdr.SafeGetDate("NoteDate").Value,
                                SourceName = (string)rdr["SourceName"],
                                Source = rdr.SafeGetString("Source"),
                            };

                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = (int)rdr["NoteTypeID"];
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");

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
        public static List<SurveyComment> GetSurveyComments(int SurvID, List<string> commentTypes = null, DateTime? commentDateLower = null,
                DateTime? commentDateUpper = null, List<int> commentAuthors = null, List<string> commentSources = null, string commentText = null)
        {
            List<SurveyComment> cs = new List<SurveyComment>();
            SurveyComment c;
            string query = "SELECT * FROM qryCommentsSurv";
            string where = "";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.Connection = conn;
                sql.SelectCommand.CommandType = CommandType.Text;

                // survey
                if (SurvID > 0)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@sid", SurvID);
                    where += " AND SurvID = @sid";
                }
                
                // type
                if (commentTypes != null && commentTypes.Count != 0)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@commentTypes", String.Join(",", commentTypes));
                    where += " AND NoteType IN (@commentTypes)";
                }
                // lower date bound
                if (commentDateLower != null)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@commentDate", commentDateLower);
                    where += " AND NoteDate >= (@commentDate)";
                }
                // upper date bound
                if (commentDateUpper != null)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@commentDate", commentDateUpper);
                    where += " AND NoteDate <= (@commentDate)";
                }
                // author
                if (commentAuthors != null && commentAuthors.Count != 0)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@commentAuthors", String.Join(",", commentAuthors));
                    where += " AND NoteInit IN (@commentAuthors)";
                }
                // source
                if (commentSources != null && commentSources.Count != 0)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@commentSources", String.Join(",", commentSources));
                    where += " AND Source IN (@commentSources)";
                }
                // comment text
                if (commentText != null)
                {
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", commentText);
                    where += " AND Notes LIKE '%' + @commentSources + '%'";
                }

                if (!string.IsNullOrEmpty(where))
                {
                    where = where.Substring(5);
                    query += " WHERE " + where;
                }

                sql.SelectCommand.CommandText = query;

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            c = new SurveyComment
                            {
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], rdr.SafeGetString("Notes")),
                                SurvID = (int)rdr["SID"],
                                Survey = rdr.SafeGetString("Survey"),
                                NoteDate = rdr.SafeGetDate("NoteDate").Value,
                                SourceName = (string)rdr["SourceName"],
                                Source = rdr.SafeGetString("Source"),
                            };

                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = (int)rdr["NoteTypeID"];
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");

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


        //
        // Wave Comments
        //


        /// <summary>
        /// Returns true if the provided comment exists for the wave.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static bool WaveCommentExists(StudyWave w, int CID)
        {
            bool exists = false;

            string query;

            query = "SELECT dbo.FN_WaveCommentExists(@wave,@cid)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.Connection = conn;
                sql.SelectCommand.CommandType = CommandType.Text;
                sql.SelectCommand.CommandText = query;


                sql.SelectCommand.Parameters.AddWithValue("@wave", w.WaveCode);
                sql.SelectCommand.Parameters.AddWithValue("@cid", CID);

                try
                {
                    exists = (bool)sql.SelectCommand.ExecuteScalar();
                }
                catch (Exception)
                {

                }
            }

            return exists;
        }

        /// <summary>
        /// Returns all Wave comments with the specified note.
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static List<WaveCommentRecord> GetWaveCommentsByCID(int CID)
        {
            List<WaveCommentRecord> cs = new List<WaveCommentRecord>();
            WaveCommentRecord c;
            string query = "SELECT * FROM Comments.FN_GetWaveCommentsByCID (@cid)";

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
                            c = new WaveCommentRecord
                            {
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], (string)rdr["Notes"]),
                                WaveID = (int)rdr["WID"],
                                StudyWave = rdr.SafeGetString("StudyWave"),
                                NoteDate = (DateTime)rdr["NoteDate"],
                                SourceName = (string)rdr["SourceName"],
                                Source = (string)rdr["Source"],
                            };

                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = (int)rdr["NoteTypeID"];
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");

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
        /// 
        /// </summary>
        /// <param name="SurvID"></param>
        /// <param name="commentTypes"></param>
        /// <param name="commentDate"></param>
        /// <param name="commentAuthors"></param>
        /// <param name="commentSources"></param>
        /// <returns></returns>
        public static List<WaveComment> GetWaveComments(string wave, string commentType = null, DateTime? commentDateLower = null,
                DateTime? commentDateUpper = null, int commentAuthor = 0, string commentSource = null, string commentText = null)
        {
            List<WaveComment> cs = new List<WaveComment>();
            WaveComment c;
            string query = "SELECT * FROM FN_WaveCommentSearch(" +
                    "@wave,@LDate,@UDate,@author,@commentText,@commentSource,@commentType)";
            

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.Connection = conn;
                sql.SelectCommand.CommandType = CommandType.Text;
                sql.SelectCommand.CommandText = query;

                if (string.IsNullOrEmpty(wave))
                    sql.SelectCommand.Parameters.AddWithValue("@wave", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@wave", wave);

                if (commentDateLower == null)
                    sql.SelectCommand.Parameters.AddWithValue("@LDate", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@LDate", commentDateLower);

                if (commentDateLower == null)
                    sql.SelectCommand.Parameters.AddWithValue("@UDate", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@UDate", commentDateLower);

                if (commentAuthor == 0)
                    sql.SelectCommand.Parameters.AddWithValue("@author", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@author", commentAuthor);

                if (string.IsNullOrEmpty(commentText))
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", commentText);

                if (string.IsNullOrEmpty(commentSource))
                    sql.SelectCommand.Parameters.AddWithValue("@commentSource", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@commentSource", commentSource);

                if (string.IsNullOrEmpty(commentType))
                    sql.SelectCommand.Parameters.AddWithValue("@commentType", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@commentType", commentType);
           

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            c = new WaveComment
                            {
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], (string)rdr["Notes"]),
                                WaveID = (int)rdr["WID"],
                                StudyWave = rdr.SafeGetString("StudyWave"),
                                NoteDate = (DateTime)rdr["NoteDate"],
                                SourceName = (string)rdr["SourceName"],
                                Source = (string)rdr["Source"],
                            };

                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = (int)rdr["NoteTypeID"];
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");

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
        /// Returns all Wave comments with the specified note.
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static List<DeletedCommentRecord> GetDeletedCommentsByCID(int CID)
        {
            List<DeletedCommentRecord> cs = new List<DeletedCommentRecord>();
            DeletedCommentRecord c;
            string query = "SELECT * FROM Comments.FN_GetDeletedCommentsByCID (@cid)";

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
                            c = new DeletedCommentRecord
                            {
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], (string)rdr["Notes"]),
                                Survey = rdr.SafeGetString("Survey"),
                                VarName = rdr.SafeGetString("VarName"),
                                NoteDate = (DateTime)rdr["NoteDate"],
                                SourceName = rdr.SafeGetString("SourceName"),
                                Source = rdr.SafeGetString("Source"),
                            };

                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = (int)rdr["NoteTypeID"];
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");

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
        /// Returns all Wave comments with the specified note.
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static List<RefVarCommentRecord> GetRefVarCommentsByCID(int CID)
        {
            List<RefVarCommentRecord> cs = new List<RefVarCommentRecord>();
            RefVarCommentRecord c;
            string query = "SELECT * FROM Comments.FN_GetRefVarCommentsByCID (@cid)";

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
                            c = new RefVarCommentRecord
                            {
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], (string)rdr["Notes"]),
                                RefVarName = rdr.SafeGetString("RefVarName"),
                                NoteDate = (DateTime)rdr["NoteDate"],
                                SourceName = (string)rdr["SourceName"],
                                Source = (string)rdr["Source"],
                            };

                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = (int)rdr["NoteTypeID"];
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");

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
        /// Returns a list of Deleted Comments matching the provided criteria.
        /// </summary>
        /// <param name="SurvID"></param>
        /// <param name="commentTypes"></param>
        /// <param name="commentDate"></param>
        /// <param name="commentAuthors"></param>
        /// <param name="commentSources"></param>
        /// <returns></returns>
        public static List<DeletedComment> GetDeletedComments(string survey = null, string varname = null, bool refVarName = false, string commentType = null,
                DateTime? commentDateLower = null, DateTime? commentDateUpper = null, int commentAuthor = 0, string commentSource = null, string commentText = null)
        {
            List<DeletedComment> cs = new List<DeletedComment>();
            DeletedComment c;

            string query;
            if (refVarName)
                query = "SELECT * FROM dbo.FN_DeletedCommentSearchRefVar(" +
                        "@survey,@varname,@LDate,@UDate,@author,@commentText,@commentSource,@commentType)";
            else
                query = "SELECT * FROM dbo.FN_DeletedCommentSearchVar(" +
                        "@survey, @varname, @LDate, @UDate, @author, @commentText, @commentSource, @commentType)";



            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.Connection = conn;
                sql.SelectCommand.CommandType = CommandType.Text;
                sql.SelectCommand.CommandText = query;

                if (string.IsNullOrEmpty(survey))
                    sql.SelectCommand.Parameters.AddWithValue("@survey", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@survey", survey);

                if (string.IsNullOrEmpty(varname))
                    sql.SelectCommand.Parameters.AddWithValue("@varname", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@varname", varname);

                if (commentDateLower == null)
                    sql.SelectCommand.Parameters.AddWithValue("@LDate", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@LDate", commentDateLower);

                if (commentDateLower == null)
                    sql.SelectCommand.Parameters.AddWithValue("@UDate", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@UDate", commentDateLower);

                if (commentAuthor == 0)
                    sql.SelectCommand.Parameters.AddWithValue("@author", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@author", commentAuthor);

                if (string.IsNullOrEmpty(commentText))
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", commentText);

                if (string.IsNullOrEmpty(commentSource))
                    sql.SelectCommand.Parameters.AddWithValue("@commentSource", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@commentSource", commentSource);

                if (string.IsNullOrEmpty(commentType))
                    sql.SelectCommand.Parameters.AddWithValue("@commentType", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@commentType", commentType);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            c = new DeletedComment
                            {
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], (string)rdr["Notes"]),
                                Survey = rdr.SafeGetString("Survey"),
                                VarName = rdr.SafeGetString("VarName"),
                                NoteDate = (DateTime)rdr["NoteDate"],
                                SourceName = rdr.SafeGetString("SourceName"),
                                Source = rdr.SafeGetString("Source"),
                            };

                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = (int)rdr["NoteTypeID"];
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");

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
        /// Returns true if the provided comment exists for the refvar.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static bool RefVarCommentExists(string refVarName, int CID)
        {
            bool exists = false;

            string query;

            query = "SELECT dbo.FN_WaveCommentExists(@refVarName,@cid)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.Connection = conn;
                sql.SelectCommand.CommandType = CommandType.Text;
                sql.SelectCommand.CommandText = query;


                sql.SelectCommand.Parameters.AddWithValue("@refVarName", refVarName);
                sql.SelectCommand.Parameters.AddWithValue("@cid", CID);

                try
                {
                    exists = (bool)sql.SelectCommand.ExecuteScalar();
                }
                catch (Exception)
                {

                }
            }

            return exists;
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
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], (string)rdr["Notes"]),
                                QID = (int)rdr["QID"],
                                SurvID = (int)rdr["SurvID"],
                                Survey = (string)rdr["Survey"],
                                VarName = (string)rdr["VarName"],
                                NoteDate = (DateTime)rdr["NoteDate"],
                                SourceName = (string)rdr["SourceName"],
                                Source = (string)rdr["Source"],
                            };
                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = (int)rdr["NoteTypeID"];
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");

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
                        query += " CommentType = @commentTypes" + i + " OR ";
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
                        sql.SelectCommand.Parameters.AddWithValue("@commentAuthors" + i, s.CommentAuthors[i].ID);
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
                                ID = (int)rdr["ID"],
                                Notes = new Note((int)rdr["CID"], (string)rdr["Notes"]),
                                QID = (int)rdr["QID"],
                                SurvID = (int)rdr["SurvID"],
                                Survey = (string)rdr["Survey"],
                                VarName = (string)rdr["VarName"],
                                NoteDate = (DateTime)rdr["NoteDate"],
                                SourceName = rdr.SafeGetString("SourceName"),
                                Source = rdr.SafeGetString("Source"),
                            };
                            c.Author.ID = (int)rdr["NoteInit"];
                            c.Author.Name = rdr.SafeGetString("Name");
                            c.NoteType.ID = (int)rdr["NoteTypeID"];
                            c.NoteType.TypeName = rdr.SafeGetString("CommentType");
                            c.NoteType.ShortForm = rdr.SafeGetString("ShortForm");
                            

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
