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
        // Survey Drafts
        //
        public static List<SurveyDraft> ListSurveyDrafts()
        {
            List<SurveyDraft> drafts = new List<SurveyDraft>();
            string query = "SELECT ID, DraftTitle, DraftDate, DraftComments, SurvID, Investigator FROM qrySurveyDraftInfo ORDER BY ID;" +
                "SELECT ID, DraftID, ExtraFieldNum AS FieldNumber, ExtraFieldLabel AS Label FROM qrySurveyDraftExtraFields;";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var results = db.QueryMultiple(query);
                drafts = results.Read<SurveyDraft>().ToList();

                var extraFields = results.Read<SurveyDraftExtraField>().ToList();
                
                foreach (SurveyDraft draft in drafts)
                {
                    draft.ExtraFields.AddRange(extraFields.Where(x => x.DraftID == draft.ID));
                }
            }
            return drafts;
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
            using (SqlConnection conn = new SqlConnection(connectionString))
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
            using (SqlConnection conn = new SqlConnection(connectionString))
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
                    where += " QuestionText LIKE '%' + @questionText + '%' AND ";
                    sql.SelectCommand.Parameters.AddWithValue("@questionText", questionText);
                }

                if (!string.IsNullOrEmpty(commentText)) {
                    where += " Comment LIKE '%' + @commentText + '%' AND ";
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", commentText);
                }

                where = where.Trim(" AND ".ToCharArray());

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
            using (SqlConnection conn = new SqlConnection(connectionString))
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
            using (SqlConnection conn = new SqlConnection(connectionString))
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
            using (SqlConnection conn = new SqlConnection(connectionString))
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
