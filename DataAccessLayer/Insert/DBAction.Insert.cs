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
    public static partial class DBAction
    {

        public static int InsertNote (Note record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createNote", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@noteText", record.NoteText);
                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        public static int InsertQuestion (string surveyCode, SurveyQuestion question)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createQuestion", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@survey", surveyCode);
                sql.InsertCommand.Parameters.AddWithValue("@varname", question.VarName.VarName);
                sql.InsertCommand.Parameters.AddWithValue("@qnum", question.Qnum);
                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    question.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }
            return 0;
        }

        public static int InsertVariable(VariableName varname)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createVariableLabeled", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@varname", varname.VarName);
                sql.InsertCommand.Parameters.AddWithValue("@varlabel", varname.VarLabel);
                sql.InsertCommand.Parameters.AddWithValue("@content", varname.Content.ID);
                sql.InsertCommand.Parameters.AddWithValue("@topic", varname.Topic.ID);
                sql.InsertCommand.Parameters.AddWithValue("@domain", varname.Domain.ID);
                sql.InsertCommand.Parameters.AddWithValue("@product", varname.Product.ID);

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }

            }
            return 0;
        }

        /// <summary>
        /// Saves Qnum field for a specified question.
        /// </summary>
        /// <param name="sq"></param>
        /// <returns></returns>
        public static int InsertLabel(string labelType, string newLabel)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createLabel", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@type", labelType);
                sql.InsertCommand.Parameters.AddWithValue("@label", newLabel);

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Creates a new domain label.
        /// </summary>
        /// <param name="sq"></param>
        /// <returns></returns>
        public static int InsertDomainLabel(DomainLabel newLabel)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createLabel", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@type", "Domain");
                sql.InsertCommand.Parameters.AddWithValue("@label", newLabel.LabelText);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    newLabel.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Creates a new topic label.
        /// </summary>
        /// <param name="sq"></param>
        /// <returns></returns>
        public static int InsertTopicLabel(TopicLabel newLabel)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createLabel", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@type", "Topic");
                sql.InsertCommand.Parameters.AddWithValue("@label", newLabel.LabelText);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    newLabel.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Creates a new topic label.
        /// </summary>
        /// <param name="sq"></param>
        /// <returns></returns>
        public static int InsertContentLabel(ContentLabel newLabel)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createLabel", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@type", "Content");
                sql.InsertCommand.Parameters.AddWithValue("@label", newLabel.LabelText);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    newLabel.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Creates a new product label.
        /// </summary>
        /// <param name="sq"></param>
        /// <returns></returns>
        public static int InsertProductLabel(ProductLabel newLabel)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createLabel", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@type", "Product");
                sql.InsertCommand.Parameters.AddWithValue("@label", newLabel.LabelText);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    newLabel.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Creates a new keyword.
        /// </summary>
        /// <param name="sq"></param>
        /// <returns></returns>
        public static int InsertKeyword(Keyword newKeyword)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createLabel", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@type", "Keyword");
                sql.InsertCommand.Parameters.AddWithValue("@label", newKeyword.LabelText);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    newKeyword.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        public static int InsertRegion(RegionRecord region)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createRegion", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@regionName", region.RegionName);
                sql.InsertCommand.Parameters.AddWithValue("@tempPrefix", region.TempVarPrefix);
               
                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    region.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);

                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Inserts a new study record.
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static int InsertCountry(StudyRecord newStudy)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createStudy", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@studyName", newStudy.StudyName);
                sql.InsertCommand.Parameters.AddWithValue("@countryName", newStudy.CountryName);
                sql.InsertCommand.Parameters.AddWithValue("@ageGroup", newStudy.AgeGroup);
                sql.InsertCommand.Parameters.AddWithValue("@countryCode", newStudy.CountryCode);
                sql.InsertCommand.Parameters.AddWithValue("@ISO_Code", newStudy.ISO_Code);
                sql.InsertCommand.Parameters.AddWithValue("@region", newStudy.RegionID);
                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    newStudy.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);

                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Inserts a new wave record.
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static int InsertStudyWave(StudyWaveRecord newWave)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createWave", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@studyID", newWave.StudyID);
                sql.InsertCommand.Parameters.AddWithValue("@waveNum", newWave.Wave);
                sql.InsertCommand.Parameters.AddWithValue("@countries", newWave.Countries);
                sql.InsertCommand.Parameters.AddWithValue("@englishRouting", newWave.EnglishRouting);
                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    newWave.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }



        public static int InsertWording(Wording wording)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createWording", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@wording", wording.WordingText);
                sql.InsertCommand.Parameters.AddWithValue("@fieldname", wording.FieldName);
                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;


                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    wording.WordID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        public static int InsertResponseSet(ResponseSet respSet)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createResponseSet", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@wording", respSet.RespList);
                sql.InsertCommand.Parameters.AddWithValue("@setname", respSet.RespSetName);
                sql.InsertCommand.Parameters.AddWithValue("@fieldname", respSet.FieldName);
               
                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sq"></param>
        /// <returns></returns>
        public static int BackupComments(int QID)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_backupComments", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@QID", QID);
                

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

        public static int InsertSurveyDraft(SurveyDraft draft)
        {
            int newID;
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createSurveyDraft", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@SurvID", draft.SurvID);
                sql.InsertCommand.Parameters.AddWithValue("@DraftTitle", draft.DraftTitle);
                sql.InsertCommand.Parameters.AddWithValue("@DraftDate", draft.DraftDate);
                sql.InsertCommand.Parameters.AddWithValue("@DraftComments", draft.DraftComments);
                sql.InsertCommand.Parameters.Add(new SqlParameter("@newID", SqlDbType.Int)).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    newID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
            return newID;
        }

        public static int InsertSurveyDraftExtraInfo(int draftID, int extraFieldNum, string extraFieldLabel)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createSurveyDraftExtraInfo", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@DraftID", draftID);
                sql.InsertCommand.Parameters.AddWithValue("@ExtraFieldNum", extraFieldNum);
                sql.InsertCommand.Parameters.AddWithValue("@ExtraFieldLabel", extraFieldLabel);

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        public static int InsertDraftQuestion(DraftQuestionRecord dq)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createSurveyDraftQuestion", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@DraftID", dq.DraftID);
                sql.InsertCommand.Parameters.AddWithValue("@SortBy", dq.SortBy);
                sql.InsertCommand.Parameters.AddWithValue("@Qnum", dq.Qnum);
                sql.InsertCommand.Parameters.AddWithValue("@AltQnum", dq.AltQnum);
                sql.InsertCommand.Parameters.AddWithValue("@VarName", dq.VarName);
                sql.InsertCommand.Parameters.AddWithValue("@QuestionText", dq.QuestionText);
                sql.InsertCommand.Parameters.AddWithValue("@Comment", dq.Comments);
                sql.InsertCommand.Parameters.AddWithValue("@Extra1", dq.Extra1);
                sql.InsertCommand.Parameters.AddWithValue("@Extra2", dq.Extra2);
                sql.InsertCommand.Parameters.AddWithValue("@Extra3", dq.Extra3);
                sql.InsertCommand.Parameters.AddWithValue("@Extra4", dq.Extra4);
                sql.InsertCommand.Parameters.AddWithValue("@Extra5", dq.Extra5);
                sql.InsertCommand.Parameters.AddWithValue("@Inserted", dq.Inserted);
                sql.InsertCommand.Parameters.AddWithValue("@Deleted", dq.Deleted);

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }
        
        // TODO add LitQ
        public static int InsertTranslation(Translation tq)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createTranslationQID", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@qid", tq.QID);
                sql.InsertCommand.Parameters.AddWithValue("@text", tq.TranslationText);
                sql.InsertCommand.Parameters.AddWithValue("@lang", tq.Language);
                sql.InsertCommand.Parameters.AddWithValue("@bilingual", tq.Bilingual);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    tq.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newId"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        // TODO optimization: insert the ref Surveys in this method, so the whole record + related records are inserted in one action
        public static int InsertSurveyCheckRecord(SurveyCheckRec record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
               
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createSurveyCheckRecord", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@checkDate", record.CheckDate);
                sql.InsertCommand.Parameters.AddWithValue("@checkInit", record.Name.ID);
                //sql.InsertCommand.Parameters.AddWithValue("@sendDate", record.SentOn);
                //sql.InsertCommand.Parameters.AddWithValue("@sendTo", record.SentTo.ID);
                //sql.InsertCommand.Parameters.AddWithValue("@reviewed", record.Reviewed);
                //sql.InsertCommand.Parameters.AddWithValue("@reviewedBy", record.ReviewedBy.ID);
                //sql.InsertCommand.Parameters.AddWithValue("@reviewDetails", record.ReviewDetails);
                //sql.InsertCommand.Parameters.AddWithValue("@editsMadeDate", record.Edited);
                sql.InsertCommand.Parameters.AddWithValue("@comments", record.Comments);
                sql.InsertCommand.Parameters.AddWithValue("@survID", record.SurveyCode.SID);
                sql.InsertCommand.Parameters.AddWithValue("@checkType", record.CheckType.ID);
                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;
                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@NewId"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }
                
            }
            return 0;
        }

        public static int InsertSurveyCheckRef(SurveyCheckRefSurvey record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createSurveyCheckRef", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@checkID", record.CheckID);
                sql.InsertCommand.Parameters.AddWithValue("@survID", record.SID);
                if (record.SurveyDate == null)
                    sql.InsertCommand.Parameters.AddWithValue("@survDate", DBNull.Value);
                else 
                    sql.InsertCommand.Parameters.AddWithValue("@survDate", record.SurveyDate);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return 1;
                }

            }
            return 0;
        }

        public static int InsertBugReport(BugReport record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createBugReport", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //sql.InsertCommand.Parameters.AddWithValue("@checkDate", record.CheckDate);
                //sql.InsertCommand.Parameters.AddWithValue("@checkInit", record.Name.ID);
                //sql.InsertCommand.Parameters.AddWithValue("@sendDate", record.SentOn);
                //sql.InsertCommand.Parameters.AddWithValue("@sendTo", record.SentTo.ID);
                //sql.InsertCommand.Parameters.AddWithValue("@reviewed", record.Reviewed);
                //sql.InsertCommand.Parameters.AddWithValue("@reviewedBy", record.ReviewedBy.ID);
                //sql.InsertCommand.Parameters.AddWithValue("@reviewDetails", record.ReviewDetails);
                //sql.InsertCommand.Parameters.AddWithValue("@editsMadeDate", record.Edited);
                //sql.InsertCommand.Parameters.AddWithValue("@comments", record.Comments);
                //sql.InsertCommand.Parameters.AddWithValue("@survID", record.SurveyCode.SID);
                //sql.InsertCommand.Parameters.AddWithValue("@checkType", record.CheckType.ID);
                //sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;
                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@NewId"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }
            return 0;

           
        }

        public static int InsertBugResponse(BugResponse record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createBugResponse", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //sql.InsertCommand.Parameters.AddWithValue("@checkDate", record.CheckDate);
                //sql.InsertCommand.Parameters.AddWithValue("@checkInit", record.Name.ID);
                //sql.InsertCommand.Parameters.AddWithValue("@sendDate", record.SentOn);
                //sql.InsertCommand.Parameters.AddWithValue("@sendTo", record.SentTo.ID);
                //sql.InsertCommand.Parameters.AddWithValue("@reviewed", record.Reviewed);
                //sql.InsertCommand.Parameters.AddWithValue("@reviewedBy", record.ReviewedBy.ID);
                //sql.InsertCommand.Parameters.AddWithValue("@reviewDetails", record.ReviewDetails);
                //sql.InsertCommand.Parameters.AddWithValue("@editsMadeDate", record.Edited);
                //sql.InsertCommand.Parameters.AddWithValue("@comments", record.Comments);
                //sql.InsertCommand.Parameters.AddWithValue("@survID", record.SurveyCode.SID);
                //sql.InsertCommand.Parameters.AddWithValue("@checkType", record.CheckType.ID);
                //sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;
                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@NewId"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }
            return 0;


        }

        public static int InsertLanguage(Language record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createLanguage", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                
                sql.InsertCommand.Parameters.AddWithValue("@language", record.LanguageName);
                sql.InsertCommand.Parameters.AddWithValue("@abbrev", record.Abbrev);
                sql.InsertCommand.Parameters.AddWithValue("@isoabbrev", record.ISOAbbrev);
                sql.InsertCommand.Parameters.AddWithValue("@nonLatin", record.NonLatin);
                sql.InsertCommand.Parameters.AddWithValue("@font", record.PreferredFont);
                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }
            return 0;


        }

        public static int InsertSurveyLanguage(SurveyLanguage record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createSurveyLanguage", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };


                sql.InsertCommand.Parameters.AddWithValue("@survID", record.SurvID);
                sql.InsertCommand.Parameters.AddWithValue("@langID", record.SurvLanguage.ID);
             
                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }
            return 0;


        }

        public static int InsertSurveyUserState(SurveyUserState record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("INSERT INTO tblSurveyUserStates (SurvID, UserStateID) VALUES (@survID, @stateID)", conn)
                {
                    CommandType = CommandType.Text
                };


                sql.InsertCommand.Parameters.AddWithValue("@survID", record.SurvID);
                sql.InsertCommand.Parameters.AddWithValue("@stateID", record.State.ID);

                //sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    // record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }
            return 0;


        }

        public static int InsertSurveyScreenedProduct(SurveyScreenedProduct record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("INSERT INTO tblSurveyProducts (SurvID, ProductID) VALUES (@survID, @productID)", conn)
                {
                    CommandType = CommandType.Text
                };


                sql.InsertCommand.Parameters.AddWithValue("@survID", record.SurvID);
                sql.InsertCommand.Parameters.AddWithValue("@productID", record.Product.ID);

                //sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    // record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }
            return 0;


        }

        public static int InsertPraccingIssue(PraccingIssue record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createPraccIssue", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@survID", record.Survey.SID);
                sql.InsertCommand.Parameters.AddWithValue("@issueNo", record.IssueNo);
                sql.InsertCommand.Parameters.AddWithValue("@varnames", record.VarNames);
                sql.InsertCommand.Parameters.AddWithValue("@date", record.IssueDate);
                sql.InsertCommand.Parameters.AddWithValue("@from", record.IssueFrom.ID);
                sql.InsertCommand.Parameters.AddWithValue("@to", record.IssueTo.ID);
                sql.InsertCommand.Parameters.AddWithValue("@description", record.Description);
                sql.InsertCommand.Parameters.AddWithValue("@category", record.Category.ID);
                sql.InsertCommand.Parameters.AddWithValue("@resolved", record.Resolved);
                sql.InsertCommand.Parameters.AddWithValue("@resolvedby", record.ResolvedBy.ID);
                sql.InsertCommand.Parameters.AddWithValue("@resolvedon", record.ResolvedDate);
                sql.InsertCommand.Parameters.AddWithValue("@language", record.Language);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;
                sql.InsertCommand.Parameters.Add("@newNo", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                    record.IssueNo = Convert.ToInt32(sql.InsertCommand.Parameters["@newNo"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }
            return 0;
        }

        public static int InsertPraccingResponse(PraccingResponse record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createPraccResponse", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@praccID", record.IssueID);
                sql.InsertCommand.Parameters.AddWithValue("@date", record.ResponseDate);
                sql.InsertCommand.Parameters.AddWithValue("@from", record.ResponseFrom.ID);
                sql.InsertCommand.Parameters.AddWithValue("@to", record.ResponseTo.ID);
                sql.InsertCommand.Parameters.AddWithValue("@response", record.Response);
                

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }
            
            return 0;


        }

        public static int InsertPraccingImage(int praccingID, List<PraccingImage> images)
        {
            
            foreach (PraccingImage img in images)
            {
                string imagePath = @"\\psychfile\psych$\psych-lab-gfong\SMG\Praccing Images\" + img.Path;
                
                using (SqlDataAdapter sql = new SqlDataAdapter())
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
                {
                    conn.Open();

                    sql.InsertCommand = new SqlCommand("INSERT INTO tblPraccingImages (PraccID, ImagePath) VALUES (@praccID, @imagePath)", conn)
                    {
                        CommandType = CommandType.Text
                    };

                    sql.InsertCommand.Parameters.AddWithValue("@praccID", praccingID);
                    sql.InsertCommand.Parameters.AddWithValue("@imagePath", imagePath);


                    //sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                    try
                    {
                        sql.InsertCommand.ExecuteNonQuery();
                        //record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                    }
                    catch (Exception)
                    {
                        return 1;
                    }

                }
            }
            return 0;
        }

        public static int InsertPraccingImage(PraccingImage image)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createPraccingImage", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@praccID", image.PraccID);
                sql.InsertCommand.Parameters.AddWithValue("@path", image.Path);
                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    image.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }
            
            }
            
            return 0;
        }

        public static int InsertPraccingResponseImage(int responseID, List<PraccingImage> images)
        {

            foreach (PraccingImage img in images)
            {
                string imagePath = @"\\psychfile\psych$\psych-lab-gfong\SMG\Praccing Images\" + img.Path;

                using (SqlDataAdapter sql = new SqlDataAdapter())
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
                {
                    conn.Open();

                    sql.InsertCommand = new SqlCommand("INSERT INTO tblPraccingResponseImages (PraccResponseID, ImagePath) VALUES (@responseID, @imagePath)", conn)
                    {
                        CommandType = CommandType.Text
                    };

                    sql.InsertCommand.Parameters.AddWithValue("@responseID", responseID);
                    sql.InsertCommand.Parameters.AddWithValue("@imagePath", imagePath);


                    //sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                    try
                    {
                        sql.InsertCommand.ExecuteNonQuery();
                        //record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                    }
                    catch (Exception)
                    {
                        return 1;
                    }

                }
            }
            return 0;
        }

        public static int InsertPraccingResponseImage(PraccingImage image)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createPraccingResponseImage", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@praccResponseID", image.PraccID);
                sql.InsertCommand.Parameters.AddWithValue("@path", image.Path);
                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    image.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }
            
            return 0;
        }

        public static int InsertSurveyProcessingDate(SurveyProcessingDate record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createSurveyProcessingDate", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@stageID", record.StageID);
                if (record.StageDate == null)
                    sql.InsertCommand.Parameters.AddWithValue("@stagedate", DBNull.Value);
                else
                    sql.InsertCommand.Parameters.AddWithValue("@stagedate", record.StageDate);

                if (record.EntryDate == null)
                    sql.InsertCommand.Parameters.AddWithValue("@entrydate", DBNull.Value);
                else
                    sql.InsertCommand.Parameters.AddWithValue("@entrydate", record.EntryDate);

                sql.InsertCommand.Parameters.AddWithValue("@stageinit", record.EnteredBy.ID);
                sql.InsertCommand.Parameters.AddWithValue("@contact", record.Contact.ID);


                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;


        }

        public static int InsertSurveyProcessingNote(SurveyProcessingNote record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createSurveyProcessingNote", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@stageID", record.DateID);
                sql.InsertCommand.Parameters.AddWithValue("@enteredby", record.Author.ID);

                if (record.NoteDate == null)
                    sql.InsertCommand.Parameters.AddWithValue("@commentdate", DBNull.Value);
                else
                    sql.InsertCommand.Parameters.AddWithValue("@commentdate", record.NoteDate);

                if (record.Note == null)
                    sql.InsertCommand.Parameters.AddWithValue("@note", DBNull.Value);
                else
                    sql.InsertCommand.Parameters.AddWithValue("@note", record.Note);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;


        }

        public static int InsertSurvey (SurveyRecord record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createSurvey", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@survey", record.SurveyCode);
                sql.InsertCommand.Parameters.AddWithValue("@title", record.Title);
                sql.InsertCommand.Parameters.AddWithValue("@cohort", record.Cohort.ID);
                sql.InsertCommand.Parameters.AddWithValue("@mode", record.Mode.ID);
                sql.InsertCommand.Parameters.AddWithValue("@filename", record.WebName);
                sql.InsertCommand.Parameters.AddWithValue("@date", record.CreationDate);
                sql.InsertCommand.Parameters.AddWithValue("@hidesurvey", record.HideSurvey);
                sql.InsertCommand.Parameters.AddWithValue("@waveid", record.WaveID);
                sql.InsertCommand.Parameters.AddWithValue("@itcsurvey", record.ITCSurvey);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.SID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int InsertQuestionComment(QuestionComment record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();
                 
                sql.InsertCommand = new SqlCommand("proc_createQuestionComment", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@survey", record.Survey);
                sql.InsertCommand.Parameters.AddWithValue("@varname", record.VarName);
                sql.InsertCommand.Parameters.AddWithValue("@commentText", record.Notes.NoteText);
                sql.InsertCommand.Parameters.AddWithValue("@notedate", record.NoteDate);
                sql.InsertCommand.Parameters.AddWithValue("@noteinit", record.Author.ID);
                sql.InsertCommand.Parameters.AddWithValue("@sourcename", record.SourceName);
                sql.InsertCommand.Parameters.AddWithValue("@notetype", record.NoteType.ID);
                sql.InsertCommand.Parameters.AddWithValue("@source", record.Source);
               

                //sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    //record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int InsertSurveyComment(SurveyComment record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createSurveyComment", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@survey", record.Survey);
                
                sql.InsertCommand.Parameters.AddWithValue("@commentText", record.Notes.NoteText);
                sql.InsertCommand.Parameters.AddWithValue("@notedate", record.NoteDate);
                sql.InsertCommand.Parameters.AddWithValue("@noteinit", record.Author.ID);
                sql.InsertCommand.Parameters.AddWithValue("@sourcename", record.SourceName);
                sql.InsertCommand.Parameters.AddWithValue("@notetype", record.NoteType.ID);
                sql.InsertCommand.Parameters.AddWithValue("@source", record.Source);


                //sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    //record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int InsertWaveComment(WaveComment record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createWaveComment", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@wave", record.StudyWave);
                sql.InsertCommand.Parameters.AddWithValue("@commentText", record.Notes.NoteText);
                sql.InsertCommand.Parameters.AddWithValue("@notedate", record.NoteDate);
                sql.InsertCommand.Parameters.AddWithValue("@noteinit", record.Author.ID);
                sql.InsertCommand.Parameters.AddWithValue("@sourcename", record.SourceName);
                sql.InsertCommand.Parameters.AddWithValue("@notetype", record.NoteType.ID);
                sql.InsertCommand.Parameters.AddWithValue("@source", record.Source);


                //sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    //record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int InsertDeletedComment(DeletedComment record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createDeletedComment", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@survey", record.Survey);
                sql.InsertCommand.Parameters.AddWithValue("@varname", record.VarName);
                sql.InsertCommand.Parameters.AddWithValue("@commentText", record.Notes.NoteText);
                sql.InsertCommand.Parameters.AddWithValue("@notedate", record.NoteDate);
                sql.InsertCommand.Parameters.AddWithValue("@noteinit", record.Author.ID);
                sql.InsertCommand.Parameters.AddWithValue("@sourcename", record.SourceName);
                sql.InsertCommand.Parameters.AddWithValue("@notetype", record.NoteType.ID);
                sql.InsertCommand.Parameters.AddWithValue("@source", record.Source);


                //sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    //record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int InsertRefVarComment(RefVarComment record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createRefVarComment", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

     
                sql.InsertCommand.Parameters.AddWithValue("@varname", record.RefVarName);
                sql.InsertCommand.Parameters.AddWithValue("@commentText", record.Notes.NoteText);
                sql.InsertCommand.Parameters.AddWithValue("@notedate", record.NoteDate);
                sql.InsertCommand.Parameters.AddWithValue("@noteinit", record.Author.ID);
                sql.InsertCommand.Parameters.AddWithValue("@sourcename", record.SourceName);
                sql.InsertCommand.Parameters.AddWithValue("@notetype", record.NoteType.ID);
                sql.InsertCommand.Parameters.AddWithValue("@source", record.Source);


                //sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    //record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int InsertVarNameChange(VarNameChangeRecord record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createVarNameChange", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sql.InsertCommand.Parameters.AddWithValue("@refVarNameNew", string.IsNullOrEmpty(record.NewRefName) ? DBNull.Value : (object)record.NewRefName);
                sql.InsertCommand.Parameters.AddWithValue("@NewName", string.IsNullOrEmpty(record.NewName) ? DBNull.Value : (object)record.NewName);
                sql.InsertCommand.Parameters.AddWithValue("@refVarNameOld", string.IsNullOrEmpty(record.OldRefName) ? DBNull.Value : (object)record.OldRefName);
                sql.InsertCommand.Parameters.AddWithValue("@OldName", string.IsNullOrEmpty(record.OldName) ? DBNull.Value : (object)record.OldName);
                sql.InsertCommand.Parameters.AddWithValue("@ChangeDate", record.ChangeDate);
                sql.InsertCommand.Parameters.AddWithValue("@ChangedBy", record.ChangedBy.ID);
                sql.InsertCommand.Parameters.AddWithValue("@Authorization", string.IsNullOrEmpty(record.Authorization) ? DBNull.Value : (object)record.Authorization);
                sql.InsertCommand.Parameters.AddWithValue("@Reasoning", string.IsNullOrEmpty(record.Rationale) ? DBNull.Value : (object)record.Rationale);
                sql.InsertCommand.Parameters.AddWithValue("@source", string.IsNullOrEmpty(record.Source) ? DBNull.Value : (object)record.Source); 
                sql.InsertCommand.Parameters.AddWithValue("@tempvar", record.HiddenChange);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

                sql.InsertCommand = new SqlCommand("proc_createVarNameChangeSurvey", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (VarNameChangeSurveyRecord sr in record.SurveysAffected)
                {
                    sql.InsertCommand.Parameters.AddWithValue("@changeID", record.ID);
                    sql.InsertCommand.Parameters.AddWithValue("@survID", sr.SurvID);
                    sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                    try
                    {
                        sql.InsertCommand.ExecuteNonQuery();
                        sr.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                    }
                    catch (Exception)
                    {
                        return 1;
                    }
                    sql.InsertCommand.Parameters.Clear();
                }

                sql.InsertCommand = new SqlCommand("proc_createVarNameChangeNotification", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (VarNameChangeNotificationRecord nr in record.Notifications)
                {
                    sql.InsertCommand.Parameters.AddWithValue("@changeID", record.ID);
                    sql.InsertCommand.Parameters.AddWithValue("@notifyname", nr.PersonID);
                    sql.InsertCommand.Parameters.AddWithValue("@notifytype", string.IsNullOrEmpty(nr.NotifyType) ? DBNull.Value : (object)nr.NotifyType);
                    sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                    try
                    {
                        sql.InsertCommand.ExecuteNonQuery();
                        nr.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                    }
                    catch (Exception)
                    {
                        return 1;
                    }
                    sql.InsertCommand.Parameters.Clear();
                }
            }
            return 0;
        }

        public static int InsertVarNameChangeSurvey(VarNameChangeSurveyRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createVarNameChangeSurvey", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@changeID", record.ChangeID);
                sql.InsertCommand.Parameters.AddWithValue("@survID", record.SurvID);
                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }

        public static int InsertVarNameChangeNotification(VarNameChangeNotificationRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createVarNameChangeNotification", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@changeID", record.ChangeID);
                sql.InsertCommand.Parameters.AddWithValue("@notifyname", record.PersonID);
                sql.InsertCommand.Parameters.AddWithValue("@notifytype", string.IsNullOrEmpty(record.NotifyType) ? DBNull.Value : (object)record.NotifyType);
                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }
                

            }
            return 0;
        }

        public static int InsertPersonnel(Person record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createPersonnel", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@firstname", string.IsNullOrEmpty(record.FirstName) ? DBNull.Value : (object)record.FirstName);
                sql.InsertCommand.Parameters.AddWithValue("@lastname", string.IsNullOrEmpty(record.LastName) ? DBNull.Value : (object)record.LastName);
                sql.InsertCommand.Parameters.AddWithValue("@username", string.IsNullOrEmpty(record.Username) ? DBNull.Value : (object)record.Username);
                sql.InsertCommand.Parameters.AddWithValue("@email", string.IsNullOrEmpty(record.Email) ? DBNull.Value : (object)record.Email);
                sql.InsertCommand.Parameters.AddWithValue("@workphone", string.IsNullOrEmpty(record.WorkPhone) ? DBNull.Value : (object)record.WorkPhone);
                sql.InsertCommand.Parameters.AddWithValue("@homephone", string.IsNullOrEmpty(record.HomePhone) ? DBNull.Value : (object)record.HomePhone);
                sql.InsertCommand.Parameters.AddWithValue("@officeno", string.IsNullOrEmpty(record.OfficeNo) ? DBNull.Value : (object)record.OfficeNo);
                sql.InsertCommand.Parameters.AddWithValue("@institution", string.IsNullOrEmpty(record.Institution) ? DBNull.Value : (object)record.Institution);
                sql.InsertCommand.Parameters.AddWithValue("@active", record.Active);
                sql.InsertCommand.Parameters.AddWithValue("@smg", record.SMG);
                sql.InsertCommand.Parameters.AddWithValue("@analyst", record.Analyst);
                sql.InsertCommand.Parameters.AddWithValue("@praccer", record.Praccer);
                sql.InsertCommand.Parameters.AddWithValue("@praccid", string.IsNullOrEmpty(record.PraccID) ? DBNull.Value : (object)record.PraccID);
                sql.InsertCommand.Parameters.AddWithValue("@programmer", record.Programmer);
                sql.InsertCommand.Parameters.AddWithValue("@firm", record.Firm);
                sql.InsertCommand.Parameters.AddWithValue("@countryteam", record.CountryTeam);
                sql.InsertCommand.Parameters.AddWithValue("@entry", record.Entry);
                sql.InsertCommand.Parameters.AddWithValue("@praccentry", record.PraccEntry);
                sql.InsertCommand.Parameters.AddWithValue("@admin", record.Admin);
                sql.InsertCommand.Parameters.AddWithValue("@ra", record.ResearchAssistant);
                sql.InsertCommand.Parameters.AddWithValue("@dissemination", record.Dissemination);
                sql.InsertCommand.Parameters.AddWithValue("@investigator", record.Investigator);
                sql.InsertCommand.Parameters.AddWithValue("@projectmanager", record.ProjectManager);
                sql.InsertCommand.Parameters.AddWithValue("@statistician", record.Statistician);
                sql.InsertCommand.Parameters.AddWithValue("@varnamechangenotify", record.VarNameChangeNotify);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int InsertPersonnelStudy(PersonnelStudyRecord record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createPersonnelCountry", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };


                sql.InsertCommand.Parameters.AddWithValue("@personnelID", record.PersonnelID);
                sql.InsertCommand.Parameters.AddWithValue("@countryID", record.StudyID);
                
                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int InsertPersonnelComment(PersonnelCommentRecord record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createPersonnelComment", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };


                sql.InsertCommand.Parameters.AddWithValue("@personnelID", record.PersonnelID);
                sql.InsertCommand.Parameters.AddWithValue("@commentType", record.CommentType);
                sql.InsertCommand.Parameters.AddWithValue("@comment", record.Comment);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int InsertCohort(SurveyCohortRecord record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createCohort", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };


                sql.InsertCommand.Parameters.AddWithValue("@cohort", record.Cohort);
                sql.InsertCommand.Parameters.AddWithValue("@code", record.Code);
                sql.InsertCommand.Parameters.AddWithValue("@webname", record.WebName);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int InsertUserState(UserStateRecord record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createUserState", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@userstate", record.UserStateName);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int InsertSimilarWords(SimilarWordsRecord record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createSimilarWords", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@words", record.Words);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int InsertCanonVar(CanonicalVariableRecord record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createCanonVar", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@refVarName", record.RefVarName);
                sql.InsertCommand.Parameters.AddWithValue("@anysuffix", record.AnySuffix);
                sql.InsertCommand.Parameters.AddWithValue("@notes", record.Notes);
                sql.InsertCommand.Parameters.AddWithValue("@active", record.Active);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int InsertPrefix(VariablePrefixRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createPrefix", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                
                sql.InsertCommand.Parameters.AddWithValue("@prefix", string.IsNullOrEmpty(record.Prefix) ? DBNull.Value : (object)record.Prefix);
                sql.InsertCommand.Parameters.AddWithValue("@prefixName", string.IsNullOrEmpty(record.PrefixName) ? DBNull.Value : (object)record.PrefixName);
                sql.InsertCommand.Parameters.AddWithValue("@productType", string.IsNullOrEmpty(record.ProductType) ? DBNull.Value : (object)record.ProductType);
                sql.InsertCommand.Parameters.AddWithValue("@relatedPrefixes", string.IsNullOrEmpty(record.RelatedPrefixes) ? DBNull.Value : (object)record.RelatedPrefixes);
                sql.InsertCommand.Parameters.AddWithValue("@domainName", string.IsNullOrEmpty(record.Description) ? DBNull.Value : (object)record.Description);
                sql.InsertCommand.Parameters.AddWithValue("@comments", string.IsNullOrEmpty(record.Comments) ? DBNull.Value : (object)record.Comments);
                sql.InsertCommand.Parameters.AddWithValue("@inactive", record.Inactive);

                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }
            }

            return 0;
        }

        public static int InsertPrefixRange(VariableRangeRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createPrefixRange", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@prefixID", record.PrefixID);
                sql.InsertCommand.Parameters.AddWithValue("@low", string.IsNullOrEmpty(record.Lower) ? DBNull.Value : (object)record.Lower);
                sql.InsertCommand.Parameters.AddWithValue("@high", string.IsNullOrEmpty(record.Upper) ? DBNull.Value : (object)record.Upper);
                sql.InsertCommand.Parameters.AddWithValue("@description", string.IsNullOrEmpty(record.Description) ? DBNull.Value : (object)record.Description);
                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }
            }

            return 0;
        }

        public static int InsertParallelPrefix(ParallelPrefixRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.InsertCommand = new SqlCommand("proc_createRelatedPrefix", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.InsertCommand.Parameters.AddWithValue("@prefixID", record.PrefixID);
                sql.InsertCommand.Parameters.AddWithValue("@relatedPrefixID", record.RelatedID);
                sql.InsertCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    sql.InsertCommand.ExecuteNonQuery();
                    record.ID = Convert.ToInt32(sql.InsertCommand.Parameters["@newID"].Value);
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
