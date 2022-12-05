using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ITCLib
{
    public static partial class DBAction
    {
        public static int DeleteSurvey (SurveyRecord s)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteSurvey", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.DeleteCommand.Parameters.AddWithValue("@survID", s.SID);
                

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

        public static int DeleteWave(StudyWaveRecord w)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteWave", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", w.ID);


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

        public static int DeleteSurveyLanguage(SurveyLanguage survLang)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblSurveyLanguages WHERE ID = @id", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@id", survLang.ID);


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

        public static int DeleteSurveyUserState(SurveyUserState userState)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblSurveyUserStates WHERE ID = @id", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@id", userState.ID);


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

        public static int DeleteSurveyScreenedProduct(SurveyScreenedProduct product)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblScreendProducts WHERE ID = @id", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.DeleteCommand.Parameters.AddWithValue("@id", product.ID);


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

        public static int DeleteQuestion (string varname, string survey)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteQuestion", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.DeleteCommand.Parameters.AddWithValue("@survey", survey);
                sql.DeleteCommand.Parameters.AddWithValue("@varname", varname);
                
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

        public static int DeleteVariable(string varname)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteVariable", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                
                sql.DeleteCommand.Parameters.AddWithValue("@varname", varname);
                sql.DeleteCommand.Parameters.AddWithValue("@hard", true);
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


        public static int DeleteRegion (int regionID)
        {
            return 1;
        }

        public static int DeleteStudy(int studyID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteStudy", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.DeleteCommand.Parameters.AddWithValue("@studyID", studyID);

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


        public static int DeleteNote(int noteID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteNote", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", noteID);

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

        public static int DeleteQuestionComment(int commentID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteComment", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", commentID);

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

        public static int DeleteDeletedComment(int commentID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteDeletedVarComment", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.DeleteCommand.Parameters.AddWithValue("@ID", commentID);

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

        public static int DeleteSurveyCheckRecord(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteSurveyCheck", conn)
                {
                    CommandType = CommandType.StoredProcedure
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

        public static int DeleteSurveyCheckRef(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteSurveyCheckRefSurv", conn)
                {
                    CommandType = CommandType.StoredProcedure
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

        public static int DeleteWording(string field, int wordID)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteWording", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.DeleteCommand.Parameters.AddWithValue("@fieldname", field);
                sql.DeleteCommand.Parameters.AddWithValue("@ID", wordID);

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

        public static int DeleteResponseSet(ResponseSet responseset)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("proc_deleteResponseSet ", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.DeleteCommand.Parameters.AddWithValue("@fieldname", responseset.FieldName);
                sql.DeleteCommand.Parameters.AddWithValue("@ID", responseset.RespSetName);

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

        public static int DeletePraccingIssue(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblIssuesTracking WHERE ID = @ID", conn)
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

        public static int DeletePraccingImage(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblPraccingImages WHERE ID = @ID", conn)
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

        public static int DeletePraccingResponse(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblIssuesResponses WHERE ID = @ID", conn)
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

        public static int DeletePraccingResponseImage(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblPraccingResponseImages WHERE ID = @ID", conn)
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

        public static int DeleteSurveyProcessingDate(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblSurveyProcessingDates WHERE ID = @ID", conn)
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


        public static int DeleteSurveyProcessingNote(int recordID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.DeleteCommand = new SqlCommand("DELETE FROM tblSurveyProcessingNotes WHERE ID = @ID", conn)
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
    }
}
