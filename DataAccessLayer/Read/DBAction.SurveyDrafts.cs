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
        // Survey Drafts
        //
        public static List<SurveyDraftRecord> ListSurveyDrafts()
        {
            List<SurveyDraftRecord> sd = new List<SurveyDraftRecord>();
            string query = "SELECT * FROM qrySurveyDraftInfo ORDER BY ID";

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
                            SurveyDraftRecord d = new SurveyDraftRecord();
                            d.ID = (int)rdr["ID"];
                            d.DraftTitle = rdr.SafeGetString("DraftTitle");
                            d.DraftDate = rdr.SafeGetDate("DraftDate");
                            d.DraftComments = rdr.SafeGetString("DraftComments");
                            d.SurvID = (int)rdr["SurvID"];
                            d.Investigator = rdr.SafeGetInt("Investigator");

                            sd.Add(d);
                        }
                    }
                }
                catch 
                {
                    
                }

                // extra fields
                query = "SELECT * FROM qrySurveyDraftExtraFields ORDER BY ID";
                sql.SelectCommand = new SqlCommand(query, conn);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            SurveyDraftExtraFieldRecord e = new SurveyDraftExtraFieldRecord(false);
                            e.ID = (int)rdr["ID"];
                            e.DraftID = (int)rdr["DraftID"];
                            e.FieldNumber = (int)rdr["ExtraFieldNum"];
                            e.Label = rdr.SafeGetString("ExtraFieldLabel");

                            var draft = sd.FirstOrDefault(x => x.ID == e.DraftID);
                            if (draft != null)
                                draft.ExtraFields.Add(e);
                            
                        }
                    }
                }
                catch
                {

                }
            }
            return sd;
        }
    

        public static SurveyDraftRecord GetSurveyDraft(int DraftID)
        {
            SurveyDraftRecord d = new SurveyDraftRecord();
            
            DraftQuestion dq;
            string query = "SELECT * FROM qrySurveyDrafts WHERE DraftID = @draftID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@draftID", DraftID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            dq = new DraftQuestion()
                            {
                                
                                Qnum = (string)rdr["Qnum"],
                                VarName = (string)rdr["VarName"],
                                QuestionText= (string)rdr["QuestionText"],
                                Comments= (string)rdr["Comment"],
                                Extra1 = (string)rdr["Extra1"],
                                Extra2 = (string)rdr["Extra2"],
                                Extra3 = (string)rdr["Extra3"],
                                Extra4 = (string)rdr["Extra4"],
                                Extra5 = (string)rdr["Extra5"],
                                Deleted = (bool)rdr["Deleted"],
                                Inserted = (bool)rdr["Inserted"]

                            };

                            d.Questions.Add(dq);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }
            return d;
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
        public static List<DraftQuestion> GetDraftComments(string survey = null, string varname = null, 
                DateTime? commentDateLower = null, DateTime? commentDateUpper = null, string commentText = null)
        {
            List<DraftQuestion> cs = new List<DraftQuestion>();
            DraftQuestion c;

            string query = "SELECT * FROM dbo.FN_DraftCommentSearch(" +
                        "@survey, @varname, @LDate, @UDate, @commentText)";



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

                if (string.IsNullOrEmpty(commentText))
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", commentText);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            c = new DraftQuestion
                            {
                                VarName = rdr.SafeGetString("VarName"),
                                Qnum = rdr.SafeGetString("Qnum"),
                                QuestionText = rdr.SafeGetString("QuestionText"),
                                Comments = rdr.SafeGetString("Comments"),
                                Extra1 = rdr.SafeGetString("Extra1"),
                                Extra2 = rdr.SafeGetString("Extra2"),
                                Extra3 = rdr.SafeGetString("Extra3"),
                                Extra4 = rdr.SafeGetString("Extra4"),
                                Extra5 = rdr.SafeGetString("Extra5"),
                                


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
        /// Returns a list of Question Comments matching the provided criteria.
        /// </summary>
        
        /// <returns></returns>
        public static List<DraftQuestionRecord> GetDraftQuestions(int? survey = null, string varname = null,
                int? draftID = null, int? investigator = null, string questionText = null, string commentText = null)
        {
            List<DraftQuestionRecord> cs = new List<DraftQuestionRecord>();

            string query = "SELECT D.*, I.SurvID FROM tblSurveyDrafts AS D LEFT JOIN tblSurveyDraftInfo AS I ON D.DraftID = I.ID ";
            string where = "";
          
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.Connection = conn;
                sql.SelectCommand.CommandType = CommandType.Text;

                if (survey != null)
                {
                    where += " I.SurvID = @survey AND ";
                    sql.SelectCommand.Parameters.AddWithValue("@survey", survey);
                }

                if (!string.IsNullOrEmpty(varname)) {
                    where += " VarName LIKE COALESCE(@varname, VarName) AND ";
                    sql.SelectCommand.Parameters.AddWithValue("@varname", varname);
                }

                if (draftID != null) {
                    where += " DraftID = @draftID AND ";
                    sql.SelectCommand.Parameters.AddWithValue("@draftID", draftID);
                }

                if (!string.IsNullOrEmpty(questionText)) {
                    where += " QuestionText LIKE @questionText AND ";
                    sql.SelectCommand.Parameters.AddWithValue("@questionText", questionText);
                }

                if (!string.IsNullOrEmpty(commentText)) {
                    where += " Comment LIKE @commentText AND ";
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", commentText);
                }

                where = Utilities.TrimString(where, " AND ");

                if (!string.IsNullOrWhiteSpace(where))
                    query +=  " WHERE " + where;

                query += " ORDER BY SortBy";

                sql.SelectCommand.CommandText = query;

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            DraftQuestionRecord c = new DraftQuestionRecord();

                            c.ID = (int)rdr["ID"];
                            c.DraftID = (int)rdr["DraftID"];
                            c.VarName = rdr.SafeGetString("VarName");
                            c.Qnum = rdr.SafeGetString("Qnum");
                            c.QuestionText = rdr.SafeGetString("QuestionText");
                            c.Comments = rdr.SafeGetString("Comment");
                            c.Extra1 = rdr.SafeGetString("Extra1");
                            c.Extra2 = rdr.SafeGetString("Extra2");
                            c.Extra3 = rdr.SafeGetString("Extra3");
                            c.Extra4 = rdr.SafeGetString("Extra4");
                            c.Extra5 = rdr.SafeGetString("Extra5");
                            c.Inserted = (bool)rdr["Inserted"];
                            c.Deleted = (bool)rdr["Deleted"];
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
        /// Returns a list of Question Comments matching the survey and draft IDs.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="draftID"></param>
        /// <returns></returns>
        public static List<DraftQuestion> GetDraftQuestions(Survey survey, int draftID)
        {
            List<DraftQuestion> cs = new List<DraftQuestion>();

            string query = "SELECT D.*, I.SurvID FROM tblSurveyDrafts AS D LEFT JOIN tblSurveyDraftInfo AS I ON D.DraftID = I.ID WHERE I.SurvID = @survey AND DraftID =@draftID ORDER BY SortBy";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.CommandText = query;
                sql.SelectCommand.Connection = conn;
                sql.SelectCommand.CommandType = CommandType.Text;

                sql.SelectCommand.Parameters.AddWithValue("@survey", survey.SID);
                sql.SelectCommand.Parameters.AddWithValue("@draftID", draftID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            DraftQuestion c = new DraftQuestion();

                            c.VarName = rdr.SafeGetString("VarName");
                            c.Qnum = rdr.SafeGetString("Qnum");
                            c.QuestionText = rdr.SafeGetString("QuestionText");
                            c.Comments = rdr.SafeGetString("Comment");
                            c.Extra1 = rdr.SafeGetString("Extra1");
                            c.Extra2 = rdr.SafeGetString("Extra2");
                            c.Extra3 = rdr.SafeGetString("Extra3");
                            c.Extra4 = rdr.SafeGetString("Extra4");
                            c.Extra5 = rdr.SafeGetString("Extra5");
                            c.Inserted = (bool)rdr["Inserted"];
                            c.Deleted = (bool)rdr["Deleted"];
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
        /// Returns a list of Question Comments matching the survey and draft ID.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="draftID"></param>
        /// <returns></returns>
        public static List<DraftQuestion> GetDraftQuestions(Survey survey, Person investigator)
        {
            List<DraftQuestion> cs = new List<DraftQuestion>();

            string query = "SELECT D.*, I.SurvID FROM tblSurveyDrafts AS D LEFT JOIN tblSurveyDraftInfo AS I ON D.DraftID = I.ID WHERE I.SurvID = @survey AND Investigator =@investigator ORDER BY SortBy";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.CommandText = query;
                sql.SelectCommand.Connection = conn;
                sql.SelectCommand.CommandType = CommandType.Text;

                sql.SelectCommand.Parameters.AddWithValue("@survey", survey.SID);
                sql.SelectCommand.Parameters.AddWithValue("@investigator", investigator.ID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            DraftQuestion c = new DraftQuestion();

                           
                            c.VarName = rdr.SafeGetString("VarName");
                            c.Qnum = rdr.SafeGetString("Qnum");
                            c.QuestionText = rdr.SafeGetString("QuestionText");
                            c.Comments = rdr.SafeGetString("Comment");
                            c.Extra1 = rdr.SafeGetString("Extra1");
                            c.Extra2 = rdr.SafeGetString("Extra2");
                            c.Extra3 = rdr.SafeGetString("Extra3");
                            c.Extra4 = rdr.SafeGetString("Extra4");
                            c.Extra5 = rdr.SafeGetString("Extra5");
                            c.Inserted = (bool)rdr["Inserted"];
                            c.Deleted = (bool)rdr["Deleted"];
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
        /// Returns a list of Question Comments matching the survey and date range.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="draftID"></param>
        /// <returns></returns>
        public static List<DraftQuestion> GetDraftQuestions(Survey survey, DateTime lowerBound, DateTime upperBound)
        {
            List<DraftQuestion> cs = new List<DraftQuestion>();

            string query = "SELECT D.*, I.SurvID FROM tblSurveyDrafts AS D LEFT JOIN tblSurveyDraftInfo AS I ON D.DraftID = I.ID WHERE I.SurvID = @survey AND DraftDate >= @lower AND DraftDate <= @upper ORDER BY SortBy";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.CommandText = query;
                sql.SelectCommand.Connection = conn;
                sql.SelectCommand.CommandType = CommandType.Text;

                sql.SelectCommand.Parameters.AddWithValue("@survey", survey.SID);
                sql.SelectCommand.Parameters.AddWithValue("@lower", lowerBound);
                sql.SelectCommand.Parameters.AddWithValue("@upper", upperBound);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            DraftQuestion c = new DraftQuestion();

                           
                            c.VarName = rdr.SafeGetString("VarName");
                            c.Qnum = rdr.SafeGetString("Qnum");
                            c.QuestionText = rdr.SafeGetString("QuestionText");
                            c.Comments = rdr.SafeGetString("Comment");
                            c.Extra1 = rdr.SafeGetString("Extra1");
                            c.Extra2 = rdr.SafeGetString("Extra2");
                            c.Extra3 = rdr.SafeGetString("Extra3");
                            c.Extra4 = rdr.SafeGetString("Extra4");
                            c.Extra5 = rdr.SafeGetString("Extra5");
                            c.Inserted = (bool)rdr["Inserted"];
                            c.Deleted = (bool)rdr["Deleted"];
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
    }
}
