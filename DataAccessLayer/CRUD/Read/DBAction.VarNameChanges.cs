using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace ITCLib
{
    partial class DBAction
    {
        //
        // VarName Changes
        // 
        
        /// <summary>
        /// Returns a list of VarName Changes for the specified survey.
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static List<VarNameChangeRecord> GetVarNameChanges(Survey survey)
        {
            List<VarNameChangeRecord> changes = new List<VarNameChangeRecord>();

            string sql = "SELECT ID, OldName, NewName, ChangeDate, TempVar AS HiddenChange, TempVar AS PreFWChange, " +
                    "Reasoning AS Rationale, [Authorization], ChangeDateApprox AS ApproxChangeDate, Source2 AS Source, " +
                    "ChangedBy, ChangedBy AS ID, ChangedByName AS Name " +
                    "FROM FN_GetVarNameChangesSurvey (@survey);" +
                "SELECT ID, ChangeID, SurveyID AS SurvID, Survey AS SurveyCode FROM qryVarNameChangeSurveys " +
                    "WHERE ChangeID IN (SELECT ChangeID FROM qryVarNameChangeSurveys WHERE Survey = @survey);" +
                "SELECT N.ID, N.ChangeID, N.NotifyName AS PersonID, P.Name, NotifyType " + 
                    "FROM qryVarNameChangeNotifications AS N " +
                    "LEFT JOIN qryIssueInit AS P ON N.NotifyName = P.ID " +
                    "LEFT JOIN qryVarNameChangeSurveys AS S ON N.ChangeID = S.ChangeID WHERE Survey = @survey";
            
            var parameters = new { survey = survey.SurveyCode };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var results = db.QueryMultiple(sql, parameters);

                changes = results.Read<VarNameChangeRecord, Person, VarNameChangeRecord>
                    (
                        (change, name) =>
                        {
                            change.ChangedBy = name;
                            return change;
                        }, 
                        splitOn: "ChangedBy"
                    ).ToList();

                var surveys = results.Read<VarNameChangeSurveyRecord>();
                var notifications = results.Read<VarNameChangeNotificationRecord>();

                foreach (VarNameChangeRecord change in changes)
                {
                    change.SurveysAffected = surveys.Where(x => x.ChangeID == change.ID).ToList();
                    change.Notifications = notifications.Where(x => x.ChangeID == change.ID).ToList();
                }
            }

            return changes;
        }

        /// <summary>
        /// </summary>
        /// <param name="wave"></param>
        /// <param name="excludeTempChanges"></param>
        /// <returns></returns>
        public static List<VarNameChangeRecord> GetVarNameChanges(StudyWave wave)
        {
            List<VarNameChangeRecord> changes = new List<VarNameChangeRecord>();

            string sql = "SELECT C.ID, [NewName], OldName, ChangeDate, ChangeDateApprox AS ApproxChangeDate, Reasoning AS Rationale, " +
                            "[Authorization], TempVar AS HiddenChange, TempVar AS PreFWChange, [Source2] AS Source, " +
                            "ChangedBy, ChangedBy AS ID, P.Name " +
                            "FROM tblVarNameChanges AS C " +
                            "LEFT JOIN qryVarNameChangeSurveys AS S ON C.ID = S.ChangeID " +
                            "LEFT JOIN qrySurveyInfo AS SA ON S.SurveyID = SA.ID " +
                            "LEFT JOIN qryIssueInit AS P ON C.ChangedBy = P.ID " +
                            "WHERE SA.WaveID = @wave;" +
                        "SELECT ID, ChangeID, SurveyID AS SurvID, Survey AS SurveyCode FROM qryVarNameChangeSurveys " +
                            "WHERE ChangeID IN " +
                            "(SELECT ChangeID FROM qryVarNameChangeSurveys AS S LEFT JOIN qrySurveyInfo AS SA ON S.SurveyID = SA.ID " +
                            "WHERE SA.WaveID = @wave);" +
                        "SELECT N.ID, N.ChangeID, N.NotifyName AS PersonID, P.Name, NotifyType " +
                            "FROM qryVarNameChangeNotifications AS N " +
                            "LEFT JOIN qryIssueInit AS P ON N.NotifyName = P.ID " +
                            "LEFT JOIN qryVarNameChangeSurveys AS S ON N.ChangeID = S.ChangeID " +
                            "LEFT JOIN qrySurveyInfo AS SA ON S.SurveyID = SA.ID " +
                            "WHERE SA.WaveID = @wave;"; 

            var parameters = new { wave = wave.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var results = db.QueryMultiple(sql, parameters);

                changes = results.Read<VarNameChangeRecord, Person, VarNameChangeRecord>
                    (
                        (change, name) =>
                        {
                            change.ChangedBy = name;
                            return change;
                        },
                        splitOn: "ChangedBy"
                    ).ToList();

                var surveys = results.Read<VarNameChangeSurveyRecord>();
                var notifications = results.Read<VarNameChangeNotificationRecord>();

                foreach (VarNameChangeRecord change in changes)
                {
                    change.SurveysAffected = surveys.Where(x => x.ChangeID == change.ID).ToList();
                    change.Notifications = notifications.Where(x => x.ChangeID == change.ID).ToList();
                }
            }

            return changes;
        }

        /// <summary>
        /// Returns a list of VarName Changes that match the provided criteria. TODO add surveys affected
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="varname"></param>
        /// <param name="refVarName"></param>
        /// <param name="LDate"></param>
        /// <param name="UDate"></param>
        /// <param name="author"></param>
        /// <param name="commentSource"></param>
        /// <param name="commentText"></param>
        /// <returns></returns>
        public static List<VarNameChange> GetVarChanges(string survey = null, string varname = null, bool refVarName = false, 
            DateTime? LDate = null, DateTime? UDate = null, int? author = null, string commentSource = null, string commentText = null)
        {
            List<VarNameChange> changes = new List<VarNameChange>();

            string query;
            if (refVarName)
                query = "SELECT ID, OldName, NewName, ChangeDate, TempVar AS HiddenChange, TempVar AS PreFWChange, " +
                        "Reasoning AS Rationale, [Authorization], ChangeDateApprox AS ApproxChangeDate, Source2 AS Source, " +
                        "ChangedBy, ChangedBy AS ID, Name " + 
                        "FROM dbo.FN_ChangeCommentSearchRefVar(@survey, @varname, @LDate, @UDate, @author, @commentText, @commentSource);";
            else
                query = "SELECT ID, OldName, NewName, ChangeDate, TempVar AS HiddenChange, TempVar AS PreFWChange, " +
                        "Reasoning AS Rationale, [Authorization], ChangeDateApprox AS ApproxChangeDate, Source2 AS Source, " +
                        "ChangedBy, ChangedBy AS ID, Name " +
                        "FROM dbo.FN_ChangeCommentSearchVar(@survey, @varname, @LDate, @UDate, @author, @commentText, @commentSource);";

            if (author == 0) author = null;

            var parameters = new { survey, varname, LDate, UDate, author, commentText, commentSource };

            
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                changes = db.Query<VarNameChange, Person, VarNameChange>
                    (query,
                    (change, name) =>
                    {
                        change.ChangedBy = name;
                        return change;
                    },
                    parameters,
                    splitOn: "ChangedBy"
                    ).ToList();
            }
            return changes;
        }
    }
}
