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
            // user and saved forms
            string sql = "SELECT PersonnelID as userid, username, AccessLevel, ReportFolder AS ReportPath, ReportPrompt, WordingNumbers, " +
                    "CommentDetails FROM qryUserPrefs WHERE username = @username;" +
                "SELECT PersonnelID, FormCode AS FormName, FormNumber AS FormNum, RecNum AS RecordPosition, Filter, SurvID AS FilterID " +
                    "FROM qryFormManager WHERE username = @username ORDER BY FormCode, FormNumber;";
            // saved comments, sources and last used comment
            string sql2 = "SELECT ID, NoteDate, Source," +
                            "0 AS NoteID, Comment AS NoteText," +
                            "NoteInit AS AuthorID, NoteInit AS ID, Name," +
                            "AuthorityID, AuthorityID AS ID, Name," +
                            "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName FROM qrySavedComments WHERE PersonnelID = @userid "+
                            "ORDER BY qrySavedComments.ID ASC;" +
                        "SELECT ID, NoteDate, SourceName, Source," +
                        "0 AS NoteID, '' AS NoteText," +
                        "NoteInit AS AuthorID, NoteInit AS ID, Name," +
                        "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName FROM qryLastUsedComment WHERE PersonnelID = @userid;" +
                        "SELECT SourceText FROM qrySavedSources WHERE PersonnelID = @userid ORDER BY SourceNumber ASC;";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var grid = db.QueryMultiple(sql, new { username});
                prefs = grid.Read<UserRecord>().First();
                List<FormStateRecord> states = grid.Read<FormStateRecord>().ToList();
                prefs.FormStates = states;

                var grid2 = db.QueryMultiple(sql2, new { prefs.userid });

                var savedComments = grid2.Read<Comment, Note, Person, Person, CommentType, Comment>((comment, note, author, authority, type) =>
                {
                    comment.Notes = note;
                    comment.Author = author;
                    comment.Authority = authority;
                    comment.NoteType = type;
                    return comment;
                }, splitOn: "NoteID, AuthorID, AuthorityID, NoteTypeID").ToList();
                var lastComment = grid2.Read<Comment, Note, Person, CommentType, Comment>((comment, note, author, type) =>
                {
                    comment.Notes = note;
                    comment.Author = author;
                    comment.NoteType = type;
                    return comment;
                }, splitOn: "NoteID, AuthorID, NoteTypeID").FirstOrDefault();
                var sources = grid2.Read<string>().ToList();
                prefs.SavedSources = sources;
                prefs.SavedComments = savedComments;
                prefs.LastUsedComment = lastComment ?? new Comment();

            }
            return prefs;
        }
    }
}
