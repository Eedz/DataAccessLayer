using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using ITCLib;
using Dapper;

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
        public static List<Note> GetNotes()
        {
            List<Note> notes = new List<Note>();

            string sql = "SELECT ID, Notes AS NoteText FROM tblNotes ORDER BY ID;";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                notes = db.Query<Note>(sql).ToList();
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

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                types = db.Query<CommentType>(sql).ToList();
            }

            return types;
        }

        /// <summary>
        /// Returns the list of Note Types used by a survey.
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static List<CommentType> GetCommentTypes(Survey survey)
        {
            List<CommentType> commentTypes = new List<CommentType>();
            
            string sql = "SELECT NoteTypeID AS ID, CommentType AS TypeName, ShortForm FROM qryCommentsQues WHERE Survey=@survey GROUP BY NoteTypeID, CommentType, ShortForm ORDER BY CommentType;";           
            var parameters = new { survey = survey.SurveyCode };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                commentTypes = db.Query<CommentType>(sql, parameters).ToList();    
            }
            return commentTypes;
        }

        //
        // Deleted Question Comments
        //

        /// <summary>
        /// Returns all deleted question comments with the specified survey and varname.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="varname"></param>
        /// <returns></returns>
        public static List<DeletedComment> GetDeletedComments(string survey, string varname)
        {
            List<DeletedComment> comments = new List<DeletedComment>();
            
            string sql = "SELECT ID, Survey, VarName, NoteDate, Source, " +
                "CID, CID AS ID, Notes AS NoteText, " +
                "NoteInit, NoteInit AS ID, Name, " + 
                "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, " +
                "AuthorityID, AuthorityID AS ID, Authority AS Name " +
                "FROM Comments.FN_GetDeletedComments (@survey, @varname);";

            var parameters = new { survey, varname };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                comments = db.Query<DeletedComment, Note, Person, CommentType, Person, DeletedComment>(sql, (comment, note, author, type, authority) =>
                {
                    comment.Notes = note;
                    comment.Author = author;
                    comment.NoteType = type;
                    comment.Authority = authority;
                    return comment;
                }, parameters, splitOn: "CID, NoteInit, NoteTypeID, AuthorityID").ToList();
            }

            return comments;
        }

        /// <summary>
        /// Returns true if the provided comment exists for the deleted question.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static bool DeletedCommentExists(DeletedQuestion question, int CID)
        {
            bool exists = false;

            string query = "SELECT dbo.FN_DeletedCommentExists(@survey, @varname, @cid)";

            var parameters = new { survey = question.SurveyCode, varname = question.VarName, cid = CID };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                exists = (bool)db.ExecuteScalar(query, parameters);
            }

            return exists;
        }

        //
        // Question Comments
        //

        /// <summary>
        /// Returns true if the provided comment exists for the question.
        /// </summary>
        /// <param name="question"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static bool QuestionCommentExists(SurveyQuestion question, int CID)
        {
            bool exists = false;

            string query;

            query = "SELECT dbo.FN_QuestionCommentExists(@survey, @varname, @cid)";
            var parameters = new { survey = question.SurveyCode, varname = question.VarName.VarName, cid = CID };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                exists = (bool)db.ExecuteScalar(query, parameters);
            }

            return exists;
        }

        /// <summary>
        /// Returns all question comments with the specified note.
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static List<QuestionComment> GetQuesCommentsByCID (int CID)
        {
            List<QuestionComment> cs = new List<QuestionComment>();

            string query = "SELECT ID, QID, SurvID, Survey, VarName, NoteDate, Source, " + 
                "CID, CID AS ID, Notes AS NoteText, " + 
                "NoteInit, NoteInit AS ID, Name, " +
                "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, ShortForm, " +
                "AuthorityID, AuthorityID AS ID, Authority AS Name " +
                "FROM Comments.FN_GetQuesCommentsByCID (@cid);";

            var parameters = new { cid = CID };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                cs = db.Query<QuestionComment, Note, Person, CommentType, Person, QuestionComment>(query, (record, note, person, type, authority) =>
                {
                    record.Notes = note;
                    record.Author = person;
                    record.NoteType = type;
                    record.Authority = authority;
                    return record;
                }, parameters, splitOn: "CID, NoteInit, NoteTypeID, AuthorityID").ToList();
            }

            return cs;
        }

        /// <summary>
        /// Returns question comments for the specified survey.
        /// </summary>
        /// <param name="SurvID"></param>
        /// <returns></returns>
        public static List<QuestionComment> GetQuesComments(Survey survey)
        {
            List<QuestionComment> cs = new List<QuestionComment>();
            
            string query = "SELECT ID, QID, SurvID, Survey, VarName, NoteDate, Source, " +
                "CID, CID AS ID, Notes AS NoteText, " +
                "NoteInit, NoteInit AS ID, Name, " +
                "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, ShortForm, " +
                "AuthorityID, AuthorityID AS ID, Authority AS Name " +
                "FROM Comments.FN_GetQuesCommentsBySurvID(@sid) ORDER BY NoteDate;";

            var parameters = new { sid = survey.SID };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                cs = db.Query<QuestionComment, Note, Person, CommentType, Person, QuestionComment>(query, (record, note, person, type, authority) =>
                {
                    record.Notes = note;
                    record.Author = person;
                    record.NoteType = type;
                    return record;
                }, parameters, splitOn: "CID, NoteInit, NoteTypeID, AuthorityID").ToList();
            }

            return cs;
        }

        /// <summary>
        /// Returns comments for the specified question.
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public static List<QuestionComment> GetQuesComments(SurveyQuestion question)
        {
            List<QuestionComment> cs = new List<QuestionComment>();

            string query = "SELECT ID, QID, SurvID, Survey, VarName, NoteDate, Source, " +
                "CID, CID AS ID, Notes AS NoteText, " +
                "NoteInit, NoteInit AS ID, Name, " +
                "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, ShortForm, " +
                "AuthorityID, AuthorityID AS ID, Authority AS Name " +
                "FROM Comments.FN_GetQuesCommentsByQID(@qid);";

            var parameters = new { qid = question.ID };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                cs = db.Query<QuestionComment, Note, Person, CommentType, Person, QuestionComment>(query, (record, note, person, type, authority) =>
                {
                    record.Notes = note;
                    record.Author = person;
                    record.NoteType = type;
                    record.Authority = authority;
                    return record;
                }, parameters, splitOn: "CID, NoteInit, NoteTypeID, AuthorityID").ToList();
            }

            return cs;
        }

        /// <summary>
        /// Returns a list of Question Comments matching the provided criteria.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="varname"></param>
        /// <param name="refVarName"></param>
        /// <param name="commentType"></param>
        /// <param name="LDate"></param>
        /// <param name="UDate"></param>
        /// <param name="author"></param>
        /// <param name="commentSource"></param>
        /// <param name="commentText"></param>
        /// <returns></returns>
        public static List<QuestionComment> GetQuesComments(string survey = null, string varname = null, bool refVarName = false, string commentType = null, 
                DateTime? LDate = null, DateTime? UDate = null, int? author = null, string commentSource = null, string commentText = null)
        {
            List<QuestionComment> comments = new List<QuestionComment>();

            string query;
            if (refVarName)
                query = "SELECT ID, SurvID, Survey, VarName, NoteDate, Source, " +
                            "CID, CID AS ID, Notes AS NoteText, " +
                            "NoteInit, NoteInit AS ID, Name, " +
                            "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, " +
                            "AuthorityID, AuthorityID AS ID, Authority AS Name " +
                        "FROM " +
                            "dbo.FN_QuestionCommentSearchRefVar(@survey,@varname,@LDate,@UDate,@author,@commentText,@commentSource,@commentType)";
            else 
                query = "SELECT ID, SurvID, Survey, VarName, NoteDate, Source, " +
                            "CID, CID AS ID, Notes AS NoteText, " + 
                            "NoteInit, NoteInit AS ID, Name, " + 
                            "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, " +
                            "AuthorityID, AuthorityID AS ID, Authority AS Name " +
                        "FROM " +
                            "dbo.FN_QuestionCommentSearchVar(@survey, @varname, @LDate, @UDate, @author, @commentText, @commentSource, @commentType)";

            var parameters = new { survey, varname, LDate, UDate, author, commentText, commentSource, commentType };
          
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                comments = db.Query<QuestionComment, Note, Person, CommentType, Person, QuestionComment>(query, (comment, note, person, type, authority)=>
                {
                    comment.Notes = note;
                    comment.Author = person;
                    comment.NoteType = type;
                    comment.Authority = authority;
                    return comment;
                },parameters, splitOn: "CID, NoteInit, NoteTypeID, AuthorityID").ToList();
            }

            return comments;
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
        public static bool SurveyCommentExists(Survey survey, int CID)
        {
            bool exists = false;

            string query;

            query = "SELECT dbo.FN_SurveyCommentExists(@survey,@cid)";
            var parameters = new { survey = survey.SurveyCode, cid = CID };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                exists = (bool)db.ExecuteScalar(query, parameters);
            }

            return exists;
        }

        /// <summary>
        /// Returns a list of Survey Comments matching the provided criteria.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="commentType"></param>
        /// <param name="LDate"></param>
        /// <param name="UDate"></param>
        /// <param name="author"></param>
        /// <param name="commentSource"></param>
        /// <param name="commentText"></param>
        /// <returns></returns>
        public static List<SurveyComment> GetSurveyComments(string survey = null, string commentType = null,
                DateTime? LDate = null, DateTime? UDate = null, int? author = null, string commentSource = null, string commentText = null)
        {
            List<SurveyComment> comments = new List<SurveyComment>();

            string query;
           
            query = "SELECT ID, SID AS SurvID, Survey, NoteDate, Source, " +
                        "CID, CID AS ID, Notes AS NoteText, " + 
                        "NoteInit, NoteInit AS ID, Name, " +
                        "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, " +
                        "AuthorityID, AuthorityID AS ID, Authority AS Name " +
                    "FROM " + 
                        "dbo.FN_SurveyCommentSearch(@survey,@LDate,@UDate,@author,@commentText,@commentSource,@commentType)";

            var parameters = new { survey, LDate, UDate, author, commentText, commentSource, commentType };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                comments = db.Query<SurveyComment, Note, Person, CommentType, Person, SurveyComment>(query, (comment, note, person, type, authority) =>
                {
                    comment.Notes = note;
                    comment.Author = person;
                    comment.NoteType = type;
                    comment.Authority = authority;
                    return comment;
                }, parameters, splitOn: "CID, NoteInit, NoteTypeID, AuthorityID").ToList();
            }

            return comments;
        }

        /// <summary>
        /// Returns all survey comments with the specified note.
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static List<SurveyComment> GetSurvCommentsByCID(int CID)
        {
            List<SurveyComment> comments = new List<SurveyComment>();

            string query = "SELECT ID, SurvID, Survey, NoteDate, Source, " +
                "CID, CID AS ID, Notes AS NoteText, " +
                "NoteInit, NoteInit AS ID, Name, " +
                "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, ShortForm, " +
                "AuthorityID, AuthorityID AS ID, Authority AS Name " +
                "FROM Comments.FN_GetSurvCommentsByCID (@cid);";

            var parameters = new { cid = CID };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                comments = db.Query<SurveyComment, Note, Person, CommentType, Person, SurveyComment>(query, (record, note, person, type, authority) =>
                {
                    record.Notes = note;
                    record.Author = person;
                    record.NoteType = type;
                    record.Authority = authority;
                    return record;
                }, parameters, splitOn: "CID, NoteInit, NoteTypeID, AuthorityID").ToList();
            }

            return comments;
        }


        /// <summary>
        /// Returns survey comments for the specified survey.
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static List<SurveyComment> GetSurvComments(Survey survey)
        {
            List<SurveyComment> cs = new List<SurveyComment>();

            string query = "SELECT ID, SurvID, Survey, NoteDate, Source, " +
                "CID, CID AS ID, Notes AS NoteText, " +
                "NoteInit, NoteInit AS ID, Name, " +
                "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, ShortForm, " +
                "AuthorityID, AuthorityID AS ID, Authority AS Name " +
                "FROM Comments.FN_GetSurvCommentsBySurvID (@sid);";

            var parameters = new { sid = survey.SID };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                cs = db.Query<SurveyComment, Note, Person, CommentType, Person, SurveyComment>(query, (record, note, person, type, authority) =>
                {
                    record.Notes = note;
                    record.Author = person;
                    record.NoteType = type;
                    record.Authority = authority;
                    return record;
                }, parameters, splitOn: "CID, NoteInit, NoteTypeID, AuthorityID").ToList();
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

            string query = "SELECT dbo.FN_WaveCommentExists(@wave,@cid)";
            var parameters = new { wave = w.WaveCode, cid = CID };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                exists = (bool)db.ExecuteScalar(query, parameters);
            }

            return exists;
        }

        /// <summary>
        /// Returns all Wave comments with the specified note.
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static List<WaveComment> GetWaveCommentsByCID(int CID)
        {
            List<WaveComment> comments = new List<WaveComment>();

            string query = "SELECT ID, WID AS WaveID, StudyWave, NoteDate, Source, " +
                "CID, CID AS ID, Notes AS NoteText, " +
                "NoteInit, NoteInit AS ID, Name, " +
                "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, ShortForm, " +
                "AuthorityID, AuthorityID AS ID, Authority AS Name " +
                "FROM Comments.FN_GetWaveCommentsByCID (@cid);";

            var parameters = new { cid = CID };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                comments = db.Query<WaveComment, Note, Person, CommentType, Person, WaveComment>(query, (record, note, person, type, authority) =>
                {
                    record.Notes = note;
                    record.Author = person;
                    record.NoteType = type;
                    record.Authority = authority;
                    return record;
                }, parameters, splitOn: "CID, NoteInit, NoteTypeID, AuthorityID").ToList();
            }

            return comments;
        }

        /// <summary>
        /// Returns a list of Wave Comments matching the provided criteria.
        /// <param name="wave"></param>
        /// <param name="commentType"></param>
        /// <param name="LDate"></param>
        /// <param name="UDate"></param>
        /// <param name="author"></param>
        /// <param name="commentSource"></param>
        /// <param name="commentText"></param>
        /// <returns></returns>
        public static List<WaveComment> GetWaveComments(string wave, string commentType = null, DateTime? LDate = null,
                DateTime? UDate = null, int? author = null, string commentSource = null, string commentText = null)
        {
            List<WaveComment> comments = new List<WaveComment>();

            string query = "SELECT ID, WID AS WaveID, StudyWave, NoteDate, Source, " +
                                "CID, CID AS ID, Notes AS NoteText, " +
                                "NoteInit, NoteInit AS ID, Name, " +
                                "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, " +
                                "AuthorityID, AuthorityID AS ID, Authority AS Name " +
                            "FROM " +
                                "dbo.FN_WaveCommentSearch(@wave,@LDate,@UDate,@author,@commentText,@commentSource,@commentType)";

            var parameters = new { wave, LDate, UDate, author, commentText, commentSource, commentType };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                comments = db.Query<WaveComment, Note, Person, CommentType, Person, WaveComment>(query, (comment, note, person, type, authority) =>
                {
                    comment.Notes = note;
                    comment.Author = person;
                    comment.NoteType = type;
                    comment.Authority = authority;
                    return comment;
                }, parameters, splitOn: "CID, NoteInit, NoteTypeID, AuthorityID").ToList();
            }

            return comments;
        }


        //
        // Deleted Comments
        //

        /// <summary>
        /// Returns a list of Deleted Comments matching the provided criteria.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="varname"></param>
        /// <param name="refVarName"></param>
        /// <param name="commentType"></param>
        /// <param name="LDate"></param>
        /// <param name="UDate"></param>
        /// <param name="author"></param>
        /// <param name="commentSource"></param>
        /// <param name="commentText"></param>
        /// <returns></returns>
        public static List<DeletedComment> GetDeletedComments(string survey = null, string varname = null, bool refVarName = false, string commentType = null,
                DateTime? LDate = null, DateTime? UDate = null, int? author = null, string commentSource = null, string commentText = null)
        {
            List<DeletedComment> comments = new List<DeletedComment>();

            string query;
            if (refVarName)
                query = "SELECT ID, Survey, VarName, NoteDate, Source, " +
                                "CID, CID AS ID, Notes AS NoteText, " +
                                "NoteInit, NoteInit AS ID, Name, " +
                                "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, ShortForm, " +
                                "AuthorityID, AuthorityID AS ID, Authority AS Name " +
                            "FROM " + 
                                "dbo.FN_DeletedCommentSearchRefVar(@survey,@varname,@LDate,@UDate,@author,@commentText,@commentSource,@commentType)";
            else
                query = "SELECT ID, Survey, VarName, NoteDate, Source, " +
                                "CID, CID AS ID, Notes AS NoteText, " +
                                "NoteInit, NoteInit AS ID, Name, " +
                                "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, ShortForm, " +
                                "AuthorityID, AuthorityID AS ID, Authority AS Name " +
                            "FROM " + 
                                "dbo.FN_DeletedCommentSearchVar(@survey, @varname, @LDate, @UDate, @author, @commentText, @commentSource, @commentType)";

            var parameters = new { survey, varname, LDate, UDate, author, commentText, commentSource, commentType };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                comments = db.Query<DeletedComment, Note, Person, CommentType, Person, DeletedComment>(query, (comment, note, person, type, authority) =>
                {
                    comment.Notes = note;
                    comment.Author = person;
                    comment.NoteType = type;
                    comment.Authority = authority;
                    return comment;
                }, parameters, splitOn: "CID, NoteInit, NoteTypeID, AuthorityID").ToList();
            }

            return comments;
        }

        /// <summary>
        /// Returns all Daleted comments with the specified note.
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static List<DeletedComment> GetDeletedCommentsByCID(int CID)
        {
            List<DeletedComment> comments = new List<DeletedComment>();

            string query = "SELECT ID, Survey, VarName, NoteDate, Source, " +
                "CID, CID AS ID, Notes AS NoteText, " +
                "NoteInit, NoteInit AS ID, Name, " +
                "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, ShortForm, " +
                "AuthorityID, AuthorityID AS ID, Authority AS Name " +
                "FROM Comments.FN_GetDeletedCommentsByCID (@cid);";

            var parameters = new { cid = CID };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                comments = db.Query<DeletedComment, Note, Person, CommentType, Person, DeletedComment>(query, (record, note, person, type, authority) =>
                {
                    record.Notes = note;
                    record.Author = person;
                    record.NoteType = type;
                    record.Authority = authority;
                    return record;
                }, parameters, splitOn: "CID, NoteInit, NoteTypeID, AuthorityID").ToList();
            }

            return comments;
        }

        //
        // RefVarName comments
        //

        /// <summary>
        /// Returns all RefVarName comments with the specified note.
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static List<RefVarComment> GetRefVarCommentsByCID(int CID)
        {
            List<RefVarComment> comments = new List<RefVarComment>();

            string query = "SELECT ID, RefVarName, NoteDate, Source, " +
                "CID, CID AS ID, Notes AS NoteText, " +
                "NoteInit, NoteInit AS ID, Name, " +
                "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, ShortForm, " +
                "AuthorityID, AuthorityID AS ID, Authority AS Name " +
                "FROM Comments.FN_GetRefVarCommentsByCID (@cid);";

            var parameters = new { cid = CID };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                comments = db.Query<RefVarComment, Note, Person, CommentType, Person, RefVarComment>(query, (record, note, person, type, authority) =>
                {
                    record.Notes = note;
                    record.Author = person;
                    record.NoteType = type;
                    record.Authority = authority;
                    return record;
                }, parameters, splitOn: "CID, NoteInit, NoteTypeID, AuthorityID").ToList();
            }

            return comments;
        }

        

        /// <summary>
        /// Returns true if the provided comment exists for the refvar.
        /// </summary>
        /// <param name="refVarName"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        public static bool RefVarCommentExists(string refVarName, int CID)
        {
            bool exists = false;

            string query = "SELECT dbo.FN_RefVarCommentExists(@refVarName,@cid)";
            var parameters = new { refVarName, cid = CID };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                exists = (bool)db.ExecuteScalar(query, parameters);
            }

            return exists;
        }

        //
        // Fill methods
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        public static void FillCommentsBySurvey(Survey survey)
        {
            var comments = GetQuesComments(survey);

            foreach(QuestionComment comment in comments)
            {
                var q = survey.QuestionByID(comment.QID);
                if (q!=null)
                    q.Comments.Add(comment);
            }
        }

    }
}
