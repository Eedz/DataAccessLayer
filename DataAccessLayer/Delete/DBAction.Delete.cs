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
        //    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
        //    {
        //        rowsAffected = db.Execute(sql, parameters);
        //    }
        //    return rowsAffected;
        //}

        public static int DeleteRecord(StudyRecord r)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteStudy";
            var parameters = new { studyID = r.ID };

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            return 0;
        }

        public static int DeleteRecord(StudyWaveRecord r)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteWave";
            var parameters = new { ID = r.ID };

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        public static int DeleteRecord(SurveyRecord r)
        {
            int rowsAffected = 0;

            string sql = "proc_deleteSurvey";
            var parameters = new { survID = r.SID };

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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


            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }

            return 0;
        }

        // continue dappering below

        public static int DeletePersonnelStudy(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblPersonnelCountry WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;

        }

        public static int DeletePersonnelComment(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblPersonnelComments WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;

        }

        public static int DeleteCohort(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblCohort WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;

        }

        public static int DeleteUserState(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblUserStates WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;

        }

        public static int DeleteSimilarWords(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblAlternateSpelling WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;

        }

        public static int DeleteCanonVar(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblCanonVars WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;

        }

        public static int DeletePrefix(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblDomainList WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        public static int DeletePrefixRange(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblVarNum WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        public static int DeleteParallelPrefix(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblRelatedPrefixList WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        public static int DeleteRenumberedSurvey(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteRenumberedSurvey", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.DeleteCommand.Parameters.AddWithValue("@survID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        public static int DeleteSurveyDraft(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblSurveyDraftInfo WHERE ID =@ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        public static int DeleteDraftExtraField(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblSurveyDraftExtraFields WHERE ID =@ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        public static int DeleteVarNameChangeNotification(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblVarNameChangeNotifications WHERE ID =@ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        public static int DeleteVarNameChangeSurvey(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblVarNameChangeSurveys WHERE ID =@ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        public static int DeleteVarNameChange(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblVarNameChanges WHERE ID =@ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", recordID);

                try
                {
                    sql.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        public static int DeleteParallelQuestion(int recordID)
        {
            string sql = "DELETE FROM tblParallelQuestions WHERE ID = @ID;";
            var parameters = new { ID = recordID };

            using (SqlConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                int rowsAffected = db.Execute(sql, parameters);
            }
            return 0;
        }
    }
}
