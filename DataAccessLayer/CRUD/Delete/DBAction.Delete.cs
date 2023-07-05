using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Dapper;
    
namespace ITCLib
{
    public static partial class DBAction
    {

        /// <summary>
        /// General form for delete methods
        /// </summary>
        /// <returns></returns>
        //static public int DeleteRecord<T>(T record)
        //{
        //    int rowsAffected = 0;

        //    string sql = "DELETE FROM [TableName] WHERE ID = @ID;";
        //    var parameters = new { ID = record.ID };
        //    using (IDbConnection db = new SqlConnection(connectionString))
        //    {
        //        rowsAffected = db.Execute(sql, parameters);
        //    }
        //    return rowsAffected;
        //}

        public static int DeleteRecord(StudyRecord r)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteStudy";
            var parameters = new { studyID = r.Item.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            return 0;
        }

        public static int DeleteRecord(StudyWaveRecord r)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteWave";
            var parameters = new { ID = r.Item.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        public static int DeleteRecord(SurveyRecord r)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteSurvey";
            var parameters = new { survID = r.Item.SID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            return 0;
        }

        public static int DeleteRecord(SurveyLanguage r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblSurveyLanguages WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }

            return 0;
        }

        public static int DeleteRecord(SurveyUserState r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblSurveyUserStates WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }

            return 0;
        }

        public static int DeleteRecord(SurveyScreenedProduct r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblScreenedProducts WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }

            return 0;
        }

        public static int DeleteQuestion (string varname, string survey)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteQuestion";
            var parameters = new { survey, varname };


            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            return 0;

        }

        public static int DeleteVariable(string varname)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteVariable";
            var parameters = new { varname };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            return 0;
        }

        public static int DeleteRecord(VariableName r)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteVariable";
            var parameters = new { varname = r.VarName };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            return 0;
        }

        public static int DeleteRecord(NoteRecord r)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteNote";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        public static int DeleteRecord(QuestionCommentRecord r)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteComment";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        public static int DeleteDeletedComment(int commentID)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteDeletedVarComment";
            var parameters = new { ID = commentID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        public static int DeleteRecord(SurveyCheckRec r)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteSurveyCheck";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        public static int DeleteRecord(SurveyCheckRefSurvey r)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteSurveyCheckRefSurv";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        public static int DeleteWording(string field, int wordID)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteWording";
            var parameters = new { fieldname = field, ID = wordID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        public static int DeleteRecord(ResponseSet responseset)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteResponseSet";
            var parameters = new { fieldname = responseset.FieldName, ID = responseset.RespSetName };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        public static int DeleteRecord(PraccingIssue r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblIssuesTracking WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }

            return 0;
        }

        public static int DeleteRecord(PraccingImage r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblPraccingImages WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }

            return 0;
        }

        public static int DeleteRecord(PraccingResponse r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblIssuesResponses WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }

            return 0;
        }

        public static int DeletePraccResponseImage(PraccingImage r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblPraccingResponseImages WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }

            return 0;
        }

        public static int DeleteRecord(SurveyProcessingDate r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblSurveyProcessingDates WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }

            return 0;
        }

        public static int DeleteRecord(SurveyProcessingNote r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblSurveyProcessingNotes WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }

            return 0;
        }

        public static int DeleteRecord(PersonnelStudyRecord r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblPersonnelCountry WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }

        public static int DeleteRecord (PersonnelCommentRecord r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblPersonnelComments WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }

        public static int DeleteRecord(SurveyCohortRecord r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblCohort WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }

        public static int DeleteRecord(UserStateRecord r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblUserStates WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }

        public static int DeleteRecord(SimilarWordsRecord r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblAlternateSpelling WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }

        public static int DeleteRecord(CanonicalVariableRecord r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblCanonVars WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }

        public static int DeleteRecord(VariablePrefixRecord r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblDomainList WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }

        public static int DeleteRecord(VariableRangeRecord r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblVarNum WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }

        public static int DeleteRecord(ParallelPrefixRecord r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblRelatedPrefixList WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }

        public static int DeleteRenumberedSurvey(Survey r)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteRenumberedSurvey";
            var parameters = new { survID = r.SID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            return 0;
        }

        public static int DeleteRecord(SurveyDraftRecord r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblSurveyDraftInfo WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }

        public static int DeleteRecord(SurveyDraftExtraFieldRecord r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblSurveyDraftExtraFields WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }

        public static int DeleteRecord(VarNameChangeNotificationRecord r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblVarNameChangeNotifications WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }

        public static int DeleteRecord(VarNameChangeSurveyRecord r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblVarNameChangeSurveys WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }

        public static int DeleteRecord(VarNameChangeRecord r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblVarNameChanges WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }

        public static int DeleteRecord(ParallelQuestion r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblParallelQuestions WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }

        public static int DeleteRecord(QuestionTimeFrame r)
        {
            int rowsAffected = 0;

            string sql = "DELETE FROM tblQuestionTimeFrames WHERE ID = @ID";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }
    }
}
