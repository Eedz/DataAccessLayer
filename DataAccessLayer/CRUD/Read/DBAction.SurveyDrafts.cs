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
        /// Returns a list of Draft Question matching the provided criteria.
        /// </summary>
        /// <returns></returns>
        public static List<DraftQuestion> GetDraftQuestions(int? survey = null, string varname = null,
                int? draftID = null, int? investigator = null, string questionText = null, string commentText = null)
        {
            List<DraftQuestion> cs = new List<DraftQuestion>();
            string query = "SELECT D.ID, DraftID, VarName, Qnum, QuestionText, Comment, Extra1, Extra2, Extra3, Extra4, Extra5, Inserted, Deleted, I.SurvID " + 
                "FROM tblSurveyDrafts AS D LEFT JOIN tblSurveyDraftInfo AS I ON D.DraftID = I.ID ";
            string where = string.Empty;

            var parameters = new DynamicParameters();
           
            if (survey != null)
            {
                where += " I.SurvID = @survey AND ";
                parameters.Add("@survey", survey);
            }

            if (!string.IsNullOrEmpty(varname))
            {
                where += " VarName LIKE COALESCE(@varname, VarName) AND ";
                parameters.Add("@varname", survey);
            }

            if (draftID != null)
            {
                where += " DraftID = @draftID AND ";
                parameters.Add("@draftID", draftID);
            }

            if (!string.IsNullOrEmpty(questionText))
            {
                where += " QuestionText LIKE '%' + @questionText + '%' AND ";
                parameters.Add("@questionText", questionText);
            }

            if (!string.IsNullOrEmpty(commentText))
            {
                where += " Comment LIKE '%' + @commentText + '%' AND ";
                parameters.Add("@commentText", commentText);
            }

            where = where.TrimAndRemoveAll(" AND ");

            if (!string.IsNullOrWhiteSpace(where))
                query += " WHERE " + where;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                cs = db.Query<DraftQuestion> (query, parameters).ToList();
            }
            return cs;
        }

        /// <summary>
        /// Returns a list of Draft Question matching the provided criteria.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="draftID"></param>
        /// <returns></returns>
        public static List<DraftQuestion> GetDraftQuestions(Survey survey, int draftID)
        {
            List<DraftQuestion> cs = new List<DraftQuestion>();
            string query = "SELECT D.ID, DraftID, VarName, Qnum, QuestionText, Comment, Extra1, Extra2, Extra3, Extra4, Extra5, Inserted, Deleted, I.SurvID " +
                "FROM tblSurveyDrafts AS D LEFT JOIN tblSurveyDraftInfo AS I ON D.DraftID = I.ID WHERE I.SurvID = @survey AND DraftID = @draftID ORDER BY SortBy;";

            var parameters = new
            {
                survey = survey.SID,
                draftID = draftID,
            };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                cs = db.Query<DraftQuestion>(query, parameters).ToList();
            }
            return cs;
        }

        /// <summary>
        /// Returns a list of Draft Question matching the provided criteria.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="investigator"></param>
        /// <returns></returns>
        public static List<DraftQuestion> GetDraftQuestions(Survey survey, Person investigator)
        {
            List<DraftQuestion> cs = new List<DraftQuestion>();

            string query = "SELECT D.ID, DraftID, VarName, Qnum, QuestionText, Comment, Extra1, Extra2, Extra3, Extra4, Extra5, Inserted, Deleted, I.SurvID " +
                "FROM tblSurveyDrafts AS D LEFT JOIN tblSurveyDraftInfo AS I ON D.DraftID = I.ID WHERE I.SurvID = @survey AND Investigator =@investigator ORDER BY SortBy;";

            var parameters = new
            {
                survey = survey.SID,
                investigator = investigator.ID
            };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                cs = db.Query<DraftQuestion>(query, parameters).ToList();
            }
            return cs;
        }

        /// <summary>
        /// Returns a list of Draft Question matching the provided criteria.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="lowerbound"></param>
        /// <param name="upperbound"></param>
        /// <returns></returns>
        public static List<DraftQuestion> GetDraftQuestions(Survey survey, DateTime lowerBound, DateTime upperBound)
        {
            List<DraftQuestion> cs = new List<DraftQuestion>();

            string query = "SELECT D.ID, DraftID, VarName, Qnum, QuestionText, Comment, Extra1, Extra2, Extra3, Extra4, Extra5, Inserted, Deleted, I.SurvID " + 
                "FROM tblSurveyDrafts AS D LEFT JOIN tblSurveyDraftInfo AS I ON D.DraftID = I.ID WHERE I.SurvID = @survey AND DraftDate >= @lower AND DraftDate <= @upper ORDER BY SortBy";

            var parameters = new
            {
                survey = survey.SID,
                lower = lowerBound,
                upper = upperBound
            };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                cs = db.Query<DraftQuestion>(query, parameters).ToList();
            }
            return cs;
        }
    }
}
