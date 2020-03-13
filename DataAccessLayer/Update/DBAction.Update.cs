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
        /// <summary>
        /// Updates the wording numbers for the provided question. USES TEST BACKEND
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public static int UpdateQuestionWordings(SurveyQuestion question)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateQuestionWordings", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@QID", question.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@prep", question.PrePNum);
                sql.UpdateCommand.Parameters.AddWithValue("@prei", question.PreINum);
                sql.UpdateCommand.Parameters.AddWithValue("@prea", question.PreANum);
                sql.UpdateCommand.Parameters.AddWithValue("@litq", question.LitQNum);
                sql.UpdateCommand.Parameters.AddWithValue("@psti", question.PstINum);
                sql.UpdateCommand.Parameters.AddWithValue("@pstp", question.PstPNum);
                sql.UpdateCommand.Parameters.AddWithValue("@respname", question.RespName);
                sql.UpdateCommand.Parameters.AddWithValue("@nrname", question.NRName);

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

        public static int UpdateWording(Wording wording)
        {
            return 1;
        }

        public static int UpdateResponseSet (ResponseSet respSet)
        {
            return 1;
        }

        public static int UpdateRegion (Region region)
        {
            return 0;
        }

        /// <summary>
        /// Updates Study info for specified study object.
        /// </summary>
        /// <param name="sq"></param>
        /// <returns></returns>
        public static int UpdateStudy(Study study)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateStudy", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@StudyID", study.StudyID);
                sql.UpdateCommand.Parameters.AddWithValue("@studyName", study.StudyName);
                sql.UpdateCommand.Parameters.AddWithValue("@countryName", study.CountryName);
                sql.UpdateCommand.Parameters.AddWithValue("@ageGroup", study.AgeGroup);
                sql.UpdateCommand.Parameters.AddWithValue("@countryCode", study.CountryCode);
                sql.UpdateCommand.Parameters.AddWithValue("@ISO_Code", study.ISO_Code);

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
        /// Saves Qnum field for a specified question. USES TEST BACKEND.
        /// </summary>
        /// <param name="sq"></param>
        /// <returns></returns>
        public static int UpdateQnum(SurveyQuestion sq)
        {
           
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateQnum", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@newqnum", sq.Qnum);
                sql.UpdateCommand.Parameters.AddWithValue("@qid", sq.ID);

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
        /// Saves User Preferences for specified user. USES TEST BACKEND. 
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static int UpdateUser(UserPrefs u)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateUser", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@userid", u.userid);
                sql.UpdateCommand.Parameters.AddWithValue("@accessLevel", u.accessLevel);
                sql.UpdateCommand.Parameters.AddWithValue("@reportPath", u.ReportPath);
                sql.UpdateCommand.Parameters.AddWithValue("@reportPrompt", u.reportPrompt);
                sql.UpdateCommand.Parameters.AddWithValue("@wordingNumbers", u.wordingNumbers);

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

        public static void UpdateSurveyDraftQuestion(DraftQuestion d)
        {
            string query = "UPDATE qrySurveyDrafts SET QuestionText = @questionText, Comment=@comment WHERE ID = @id";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand(query, conn);
                sql.UpdateCommand.Parameters.AddWithValue("@id", d.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@questionText", d.questionText);
                sql.UpdateCommand.Parameters.AddWithValue("@comment", d.comment);

                sql.UpdateCommand.ExecuteNonQuery();
            }
        }

        public static int UpdateSurveyCheckRecord(SurveyCheckRec record)
        {
            
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateSurveyCheckRecord", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@checkDate", record.CheckDate);
                sql.UpdateCommand.Parameters.AddWithValue("@checkInit", record.Name.ID);
                //sql.UpdateCommand.Parameters.AddWithValue("@sendDate", record.SentOn);
                //sql.UpdateCommand.Parameters.AddWithValue("@sendTo", record.SentTo.ID);
                //sql.UpdateCommand.Parameters.AddWithValue("@reviewed", record.Reviewed);
                //sql.UpdateCommand.Parameters.AddWithValue("@reviewedBy", record.ReviewedBy.ID);
                //sql.UpdateCommand.Parameters.AddWithValue("@reviewDetails", record.ReviewDetails);
                //sql.UpdateCommand.Parameters.AddWithValue("@editsMadeDate", record.Edited);
                sql.UpdateCommand.Parameters.AddWithValue("@comments", record.Comments);
                sql.UpdateCommand.Parameters.AddWithValue("@survID", record.SurveyCode.SID);
                sql.UpdateCommand.Parameters.AddWithValue("@checkType", record.CheckType.ID);

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

        public static int UpdateSurveyCheckRefRecord(SurveyCheckRefSurvey record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateSurveyCheckRef", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@checkID", record.CheckID);
                sql.UpdateCommand.Parameters.AddWithValue("@survID", record.SID);
                if (record.SurveyDate == null)
                    sql.UpdateCommand.Parameters.AddWithValue("@survDate", DBNull.Value);
                else
                    sql.UpdateCommand.Parameters.AddWithValue("@survDate", record.SurveyDate);

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
