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
        //
        // Users
        //

        /// <summary>
        /// Returns the user profile for the specified username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static UserRecord GetUser(string username)
        {
            UserRecord prefs;
            string sql = "SELECT PersonnelID as userid, username, AccessLevel, ReportFolder AS ReportPath, ReportPrompt, WordingNumbers, CommentDetails FROM Users.FN_GetUserPrefs (@username);" +
                            "SELECT PersonnelID, FormCode AS FormName, FormNumber AS FormNum, RecNum AS RecordPosition, Filter, SurvID AS FilterID FROM qryFormManager WHERE username = @username ORDER BY FormCode, FormNumber;";

            string sql2 = "SELECT ID, NoteDate, SourceInit AS SourceName, Source," +
                            "0 AS NoteID, Comment AS NoteText," +
                            "NoteInit AS AuthorID, NoteInit AS ID, Name," +
                            "AuthorityID, AuthorityID AS ID, Name," +
                            "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName FROM qrySavedComments WHERE PersonnelID = @userid ORDER BY qrySavedComments.ID ASC;" +
                            "SELECT ID, NoteDate, SourceName, Source," +
                            "0 AS NoteID, '' AS NoteText," +
                            "NoteInit AS AuthorID, NoteInit AS ID, Name," +
                            "NoteTypeID, NoteTypeID AS ID, NoteType AS TypeName FROM qryLastUsedComment WHERE PersonnelID = @userid;" +
                            "SELECT SourceText FROM qrySavedSources WHERE PersonnelID = @userid ORDER BY SourceNumber ASC;";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                var grid = db.QueryMultiple(sql, new { username = username});
                prefs = grid.Read<UserRecord>().First();
                List<FormStateRecord> states = grid.Read<FormStateRecord>().ToList();
                prefs.FormStates = states;

                //var grid2 = db.QueryMultiple(sql2, new { userid = prefs.userid });
                
                //var savedComments =  grid.Read<Comment, Note, Person, Person, CommentType, Comment>((comment, note, author, authority, type) =>
                //{
                //    comment.Notes = note;
                //    comment.Author = author;
                //    comment.Authority = authority;
                //    comment.NoteType = type;
                //    return comment;
                //}, splitOn: "NoteID, AuthorID, AuthorityID, NotTypeID").ToList();
                //var lastComment = grid.Read<Comment, Note, Person,  CommentType, Comment>((comment, note, author, type) =>
                //{
                //    comment.Notes = note;
                //    comment.Author = author;
                //    comment.NoteType = type;
                //    return comment;
                //}, splitOn: "NoteID, AuthorID, NotTypeID").FirstOrDefault();
                //var sources = grid.Read<string>().ToList();
                //prefs.SavedSources = sources;
                //prefs.SavedComments = savedComments;
                //prefs.LastUsedComment = lastComment;

            }
            return prefs;
        }

               

        /// <summary>
        /// Saves a filter for the provided user.
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="filter"></param>
        /// <param name="position"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int UpdateFormFilter(string formName, int formNum, string filter, int position, UserPrefs user)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();
                sql.UpdateCommand = new SqlCommand("proc_updateFormFilter", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@formCode", formName);
                sql.UpdateCommand.Parameters.AddWithValue("@filter", filter);
                sql.UpdateCommand.Parameters.AddWithValue("@formNum", formNum);
                sql.UpdateCommand.Parameters.AddWithValue("@personnelID", user.userid);
                sql.UpdateCommand.Parameters.AddWithValue("@position", position);
                

                try
                {
                    sql.UpdateCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;

        }

        /// <summary>
        /// Saves a filter for the provided user.
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="filter"></param>
        /// <param name="position"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int UpdateFormFilter(FormStateRecord record, int userid)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();
                sql.UpdateCommand = new SqlCommand("proc_updateFormFilter", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@formCode", record.FormName);
                sql.UpdateCommand.Parameters.AddWithValue("@filter", record.Filter);
                sql.UpdateCommand.Parameters.AddWithValue("@formNum", record.FormNum);
                sql.UpdateCommand.Parameters.AddWithValue("@personnelID", userid);
                sql.UpdateCommand.Parameters.AddWithValue("@position", record.RecordPosition);


                try
                {
                    sql.UpdateCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;

        }

        /// <summary>
        /// Saves a filter for the provided user.
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="filter"></param>
        /// <param name="position"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int UpdateFormSurvey(string formName, int formNum, int survID, int position, UserPrefs user)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();
                sql.UpdateCommand = new SqlCommand("proc_updateFormSurvey", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@formCode", formName);
                sql.UpdateCommand.Parameters.AddWithValue("@survID", survID);
                sql.UpdateCommand.Parameters.AddWithValue("@formNum", formNum);
                sql.UpdateCommand.Parameters.AddWithValue("@personnelID", user.userid);
                sql.UpdateCommand.Parameters.AddWithValue("@position", position);


                try
                {
                    sql.UpdateCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;

        }

        /// <summary>
        /// Saves a filter for the provided user.
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="filter"></param>
        /// <param name="position"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int UpdateFormSurvey(FormStateRecord record, int userid)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();
                sql.UpdateCommand = new SqlCommand("proc_updateFormSurvey", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@formCode", record.FormName);
                sql.UpdateCommand.Parameters.AddWithValue("@survID", record.FilterID);
                sql.UpdateCommand.Parameters.AddWithValue("@formNum", record.FormNum);
                sql.UpdateCommand.Parameters.AddWithValue("@personnelID", userid);
                sql.UpdateCommand.Parameters.AddWithValue("@position", record.RecordPosition);


                try
                {
                    sql.UpdateCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;

        }
    }
}
