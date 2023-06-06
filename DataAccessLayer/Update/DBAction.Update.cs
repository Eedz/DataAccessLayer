﻿using System;
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
        /// <summary>
        /// Updates the VarLabel of the given VarName.
        /// </summary>
        /// <param name="varname"></param>
        /// <param name="varlabel"></param>
        /// <returns></returns>
        public static int UpdateVarLabel(string varname, string varlabel)
        {
            int rowsAffected = 0;
            string sql = "UPDATE tblVariableInformation SET VarLabel = @varlabel WHERE VarName = @varname";
            var parameters = new { varname, varlabel };
            using (SqlConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }
            return rowsAffected;
        }

        /// <summary>
        /// Updates the survey record matching the provided survey object.
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static int UpdateSurvey (Survey survey)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblStudyAttributes SET Survey=@surveycode, SurveyTitle=@title, Cohort=@cohort, Mode=@mode, " +
                        "SurveyFileName=@filename, NCT=@nct, ReRun=@rerun, HideSurvey=@hide, Locked=@locked, ITCSurvey=@itc, ISISCreationDate=@date WHERE ID = @SID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@SID", survey.SID);
                sql.UpdateCommand.Parameters.AddWithValue("@surveycode", survey.SurveyCode);
                sql.UpdateCommand.Parameters.AddWithValue("@title", survey.Title);
                sql.UpdateCommand.Parameters.AddWithValue("@cohort", survey.Cohort.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@mode", survey.Mode.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@filename", survey.WebName);
                sql.UpdateCommand.Parameters.AddWithValue("@nct", survey.NCT);
                sql.UpdateCommand.Parameters.AddWithValue("@rerun", survey.ReRun);
                sql.UpdateCommand.Parameters.AddWithValue("@hide", survey.HideSurvey);
                sql.UpdateCommand.Parameters.AddWithValue("@locked", survey.Locked);
                sql.UpdateCommand.Parameters.AddWithValue("@itc", survey.ITCSurvey);

                if (survey.CreationDate == null)
                    sql.UpdateCommand.Parameters.AddWithValue("@date", DBNull.Value);
                else
                    sql.UpdateCommand.Parameters.AddWithValue("@date", survey.CreationDate);
                


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
        /// Updates the wording numbers for the provided question. 
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

        /// <summary>
        /// Updates/inserts the plain filter for the provided question. 
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public static int UpdatePlainFilter(SurveyQuestion question)
        {
            string sql = "proc_upsertPlainFilter";

            var parameters = new { QID = question.ID, filter = question.FilterDescription };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            return 0;
        }

        public static int UpdateLabels(VariableName varname)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateLabels", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@varname", varname.VarName);
                sql.UpdateCommand.Parameters.AddWithValue("@varlabel", varname.VarLabel);
                sql.UpdateCommand.Parameters.AddWithValue("@domain", varname.Domain.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@topic", varname.Topic.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@content", varname.Content.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@product", varname.Product.ID);
                

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
            string field = wording.FieldName;
            string query = "";
            switch (field)
            {
                case "PreP":
                    query = "UPDATE tblPreP SET Wording =@wording WHERE ID =@ID";
                    break;
                case "PreI":
                    query = "UPDATE tblPreI SET Wording =@wording WHERE ID =@ID";
                    break;
                case "PreA":
                    query = "UPDATE tblPreA SET Wording =@wording WHERE ID =@ID";
                    break;
                case "LitQ":
                    query = "UPDATE tblLitQ SET Wording =@wording WHERE ID =@ID";
                    break;
                case "PstI":
                    query = "UPDATE tblPstI SET Wording =@wording WHERE ID =@ID";
                    break;
                case "PstP":
                    query = "UPDATE tblPstP SET Wording =@wording WHERE ID =@ID";
                    break;
            }

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand(query, conn);
                
                sql.UpdateCommand.Parameters.AddWithValue("@ID", wording.WordID);
                sql.UpdateCommand.Parameters.AddWithValue("@wording", wording.WordingText);                

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

        public static int UpdateResponseSet (ResponseSet respSet)
        {
            string field = respSet.FieldName;
            string query = "";
            switch (field)
            {
                case "RespOptions":
                    query = "UPDATE tblRespOptionsTableCombined SET RespOptions =@wording WHERE RespName =@setname";
                    break;
                case "NRCodes":
                    query = "UPDATE tblNonRespOptions SET NRCodes =@wording WHERE NRName =@setname";
                    break;
            }

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand(query, conn);

                sql.UpdateCommand.Parameters.AddWithValue("@setname", respSet.RespSetName);
                sql.UpdateCommand.Parameters.AddWithValue("@wording", respSet.RespList);

                try
                {
                    sql.UpdateCommand.ExecuteNonQuery();
                }
                catch 
                {
                    return 1;
                }
            }
            return 0;
        }

        public static int UpdateRegion (RegionRecord region)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateRegion", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@id", region.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@regionName", region.RegionName);
                sql.UpdateCommand.Parameters.AddWithValue("@tempPrefix", region.TempVarPrefix);

                try
                {
                    sql.UpdateCommand.ExecuteNonQuery();
                }
                catch 
                {
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Updates Study info for specified study object.
        /// </summary>
        /// <param name="sq"></param>
        /// <returns></returns>
        public static int UpdateStudy(StudyRecord study)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateStudy", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@StudyID", study.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@studyName", study.StudyName);
                sql.UpdateCommand.Parameters.AddWithValue("@countryName", study.CountryName);
                sql.UpdateCommand.Parameters.AddWithValue("@ageGroup", study.AgeGroup);
                sql.UpdateCommand.Parameters.AddWithValue("@countryCode", study.CountryCode);
                sql.UpdateCommand.Parameters.AddWithValue("@ISO_Code", study.ISO_Code);
                sql.UpdateCommand.Parameters.AddWithValue("@region", study.RegionID);
                sql.UpdateCommand.Parameters.AddWithValue("@cohort", study.Cohort);
                sql.UpdateCommand.Parameters.AddWithValue("@languages", study.Languages);

                try
                {
                    sql.UpdateCommand.ExecuteNonQuery();
                }
                catch 
                {
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Updates Study info for specified study object.
        /// </summary>
        /// <param name="sq"></param>
        /// <returns></returns>
        public static int UpdateWave(StudyWaveRecord wave)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateWave", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@id", wave.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@studyID", wave.StudyID);
                sql.UpdateCommand.Parameters.AddWithValue("@waveNum", wave.Wave);
                sql.UpdateCommand.Parameters.AddWithValue("@countries", wave.Countries);
                sql.UpdateCommand.Parameters.AddWithValue("@englishRouting", wave.EnglishRouting);
                
                

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
        /// Saves Qnum field for a specified question.
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
        /// Saves AltQnum fields for a specified question.
        /// </summary>
        /// <param name="sq"></param>
        /// <returns></returns>
        public static int UpdateAltQnum(SurveyQuestion sq)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblSurveyNumbers SET AltQnum=@altqnum, AltQnum2 = @altqnum2, AltQnum3 =@altqnum3 WHERE ID = @id", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@altqnum", !string.IsNullOrEmpty(sq.AltQnum) ? (object)sq.AltQnum : DBNull.Value);
                sql.UpdateCommand.Parameters.AddWithValue("@altqnum2", !string.IsNullOrEmpty(sq.AltQnum2) ? (object)sq.AltQnum2 : DBNull.Value);
                sql.UpdateCommand.Parameters.AddWithValue("@altqnum3", !string.IsNullOrEmpty(sq.AltQnum3) ? (object)sq.AltQnum3 : DBNull.Value);
                sql.UpdateCommand.Parameters.AddWithValue("@id", sq.ID);

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

        public static void UpdateSurveyDraftQuestion(DraftQuestionRecord d)
        {
            string query = "UPDATE qrySurveyDrafts SET QuestionText = @questionText, Comment=@comment WHERE ID = @id";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand(query, conn);
                sql.UpdateCommand.Parameters.AddWithValue("@id", d.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@questionText", d.QuestionText);
                sql.UpdateCommand.Parameters.AddWithValue("@comment", d.Comments);

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

        public static int UpdateBugReport(BugReport record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateBugReport", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                //sql.UpdateCommand.Parameters.AddWithValue("@checkDate", record.CheckDate);
                //sql.UpdateCommand.Parameters.AddWithValue("@checkInit", record.Name.ID);
                //sql.UpdateCommand.Parameters.AddWithValue("@sendDate", record.SentOn);
                //sql.UpdateCommand.Parameters.AddWithValue("@sendTo", record.SentTo.ID);
                //sql.UpdateCommand.Parameters.AddWithValue("@reviewed", record.Reviewed);
                //sql.UpdateCommand.Parameters.AddWithValue("@reviewedBy", record.ReviewedBy.ID);
                //sql.UpdateCommand.Parameters.AddWithValue("@reviewDetails", record.ReviewDetails);
                //sql.UpdateCommand.Parameters.AddWithValue("@editsMadeDate", record.Edited);
                //sql.UpdateCommand.Parameters.AddWithValue("@comments", record.Comments);
                //sql.UpdateCommand.Parameters.AddWithValue("@survID", record.SurveyCode.SID);
                //sql.UpdateCommand.Parameters.AddWithValue("@checkType", record.CheckType.ID);

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

        public static int UpdateLabel(string labelType, string labelText, int labelID)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateLabel", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@type", labelType);
                sql.UpdateCommand.Parameters.AddWithValue("@label", labelText);
                sql.UpdateCommand.Parameters.AddWithValue("@id", labelID);

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
        /// Updates a single translation record.
        /// </summary>
        /// <param name="translation"></param>
        /// <returns></returns>
        public static int UpdateTranslation(Translation translation)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateTranslation", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@id", translation.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@lang", translation.Language);
                sql.UpdateCommand.Parameters.AddWithValue("@text", translation.TranslationText);
                sql.UpdateCommand.Parameters.AddWithValue("@bilingual", translation.Bilingual);
             

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
        /// Updates Language info for specified language object.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static int UpdateLanguage(Language language)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateLanguage", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@id", language.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@language", language.LanguageName);
                sql.UpdateCommand.Parameters.AddWithValue("@abbrev", language.Abbrev);
                sql.UpdateCommand.Parameters.AddWithValue("@isoabbrev", language.ISOAbbrev);
                sql.UpdateCommand.Parameters.AddWithValue("@nonLatin", language.NonLatin);
                sql.UpdateCommand.Parameters.AddWithValue("@font", language.PreferredFont);

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
        /// Updates Language info for specified language object.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static int UpdateSurveyLanguage(SurveyLanguage language)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblSurveyLanguages SET SurvID=@survID, LanguageID = @langID WHERE ID =@id", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@id", language.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@survID", language.SurvID);
                sql.UpdateCommand.Parameters.AddWithValue("@langID", language.SurvLanguage.ID);
                

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
        /// Updates user state record for particular survey.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static int UpdateSurveyUserState(SurveyUserState userstate)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblSurveyUserStates SET SurvID=@survID, UserStateID = @userStateID WHERE ID =@id", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@id", userstate.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@survID", userstate.SurvID);
                sql.UpdateCommand.Parameters.AddWithValue("@userStateID", userstate.State.ID);


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
        /// Updates user state record for particular survey.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static int UpdateSurveyScreenedProduct(SurveyScreenedProduct product)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblSurveyProducts SET SurvID=@survID, ProductID = @productID WHERE ID =@id", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@id", product.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@survID", product.SurvID);
                sql.UpdateCommand.Parameters.AddWithValue("@productID", product.Product.ID);


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
        /// Updates the specified praccing record.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static int UpdatePraccingIssue(PraccingIssue record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updatePraccIssue", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@id", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@survID", record.Survey.SID);
                sql.UpdateCommand.Parameters.AddWithValue("@issueNo", record.IssueNo);
                sql.UpdateCommand.Parameters.AddWithValue("@varnames", record.VarNames);
                sql.UpdateCommand.Parameters.AddWithValue("@date", record.IssueDate);
                sql.UpdateCommand.Parameters.AddWithValue("@from", record.IssueFrom.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@to", record.IssueTo.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@description", record.Description);
                sql.UpdateCommand.Parameters.AddWithValue("@category", record.Category.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@resolved", record.Resolved);
                sql.UpdateCommand.Parameters.AddWithValue("@resolvedby", record.ResolvedBy.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@resolvedon", record.ResolvedDate);
                sql.UpdateCommand.Parameters.AddWithValue("@language", record.Language);


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
        /// Updates the specified praccing record.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static int UpdatePraccingResponse(PraccingResponse record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updatePraccResponse", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@id", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@issueID", record.IssueID);
                sql.UpdateCommand.Parameters.AddWithValue("@date", record.ResponseDate);
                sql.UpdateCommand.Parameters.AddWithValue("@from", record.ResponseFrom.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@to", record.ResponseTo.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@description", record.Response);

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

        public static int UpdateSurveyProcessingDate(SurveyProcessingDate record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblSurveyProcessingDates Set StageDate = @stagedate, EntryDate = @entrydate, StageInit = @stageinit, StageContact=@contact " +
                        "WHERE ID=@ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                
                if (record.StageDate ==null)
                    sql.UpdateCommand.Parameters.AddWithValue("@stagedate", DBNull.Value);
                else
                    sql.UpdateCommand.Parameters.AddWithValue("@stagedate", record.StageDate);

                if (record.EntryDate == null)
                    sql.UpdateCommand.Parameters.AddWithValue("@entrydate", DBNull.Value);
                else
                    sql.UpdateCommand.Parameters.AddWithValue("@entrydate", record.EntryDate);

                sql.UpdateCommand.Parameters.AddWithValue("@stageinit", record.EnteredBy.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@contact", record.Contact.ID);


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

        public static int UpdateSurveyProcessingNote(SurveyProcessingNote record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblSurveyProcessingNotes SET EnteredBy=@enteredby, CommentDate = @commentdate, Note=@note " +
                        "WHERE ID=@ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@enteredby", record.Author.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@commentdate", record.NoteDate);
                sql.UpdateCommand.Parameters.AddWithValue("@note", record.Note);


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

        public static int UpdateSurveyProcessingStage(SurveyProcessingRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblSurveyProcessing SET Stage=@stage, NA = @na, Done=@done WHERE ID=@ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@stage", record.Stage.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@na", record.NotApplicable);
                sql.UpdateCommand.Parameters.AddWithValue("@done", record.Done);


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


        public static int UpdateNote(Note record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblNotes SET Notes=@notes WHERE ID=@ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@notes", record.NoteText);

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

        public static int UpdateQuestionComment(QuestionComment record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateQuestionComment", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@survey", record.Survey);
                sql.UpdateCommand.Parameters.AddWithValue("@varname", record.VarName);
                sql.UpdateCommand.Parameters.AddWithValue("@commentText", record.Notes.NoteText);
                sql.UpdateCommand.Parameters.AddWithValue("@notedate", record.NoteDate.Value);
                sql.UpdateCommand.Parameters.AddWithValue("@noteinit", record.Author.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@sourcename", record.SourceName);
                sql.UpdateCommand.Parameters.AddWithValue("@notetype", record.NoteType.TypeName);
                sql.UpdateCommand.Parameters.AddWithValue("@source", record.Source);

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

        public static int UpdateSurveyComment(SurveyComment record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateSurveyComment", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@survey", record.Survey);
                sql.UpdateCommand.Parameters.AddWithValue("@commentText", record.Notes.NoteText);
                sql.UpdateCommand.Parameters.AddWithValue("@notedate", record.NoteDate.Value);
                sql.UpdateCommand.Parameters.AddWithValue("@noteinit", record.Author.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@sourcename", record.SourceName);
                sql.UpdateCommand.Parameters.AddWithValue("@notetype", record.NoteType.TypeName);
                sql.UpdateCommand.Parameters.AddWithValue("@source", record.Source);

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

        public static int UpdateWaveComment(WaveComment record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateWaveComment", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@wave", record.StudyWave);
                sql.UpdateCommand.Parameters.AddWithValue("@commentText", record.Notes.NoteText);
                sql.UpdateCommand.Parameters.AddWithValue("@notedate", record.NoteDate.Value);
                sql.UpdateCommand.Parameters.AddWithValue("@noteinit", record.Author.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@sourcename", record.SourceName);
                sql.UpdateCommand.Parameters.AddWithValue("@notetype", record.NoteType.TypeName);
                sql.UpdateCommand.Parameters.AddWithValue("@source", record.Source);

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

        public static int UpdateRefVarComment(RefVarComment record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateWaveComment", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@varname", record.RefVarName);
                sql.UpdateCommand.Parameters.AddWithValue("@commentText", record.Notes.NoteText);
                sql.UpdateCommand.Parameters.AddWithValue("@notedate", record.NoteDate.Value);
                sql.UpdateCommand.Parameters.AddWithValue("@noteinit", record.Author.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@sourcename", record.SourceName);
                sql.UpdateCommand.Parameters.AddWithValue("@notetype", record.NoteType.TypeName);
                sql.UpdateCommand.Parameters.AddWithValue("@source", record.Source);

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

        public static int UpdateDeletedQuestionComment(DeletedComment record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateDeletedComment", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@survey", record.Survey);
                sql.UpdateCommand.Parameters.AddWithValue("@varname", record.VarName);
                sql.UpdateCommand.Parameters.AddWithValue("@commentText", record.Notes.NoteText);
                sql.UpdateCommand.Parameters.AddWithValue("@notedate", record.NoteDate.Value);
                sql.UpdateCommand.Parameters.AddWithValue("@noteinit", record.Author.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@sourcename", record.SourceName);
                sql.UpdateCommand.Parameters.AddWithValue("@notetype", record.NoteType.TypeName);
                sql.UpdateCommand.Parameters.AddWithValue("@source", record.Source);

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

        public static int UpdateStoredComment(int userID, Comment record, int recordNumber)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblSavedComments SET NoteDate=@notedate, NoteInit=@noteinit, NoteTypeID=@notetypeid, Comment=@comment, Source=@source, AuthorityID=@authority " +
                        "WHERE PersonnelID = @userID AND ID = @id", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@notedate", record.NoteDate.Value);
                sql.UpdateCommand.Parameters.AddWithValue("@noteinit", record.Author.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@notetypeid", record.NoteType.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@authority", record.Authority.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@comment", record.Notes.NoteText);
                sql.UpdateCommand.Parameters.AddWithValue("@source", record.Source);
                
                sql.UpdateCommand.Parameters.AddWithValue("@userID", userID);
                sql.UpdateCommand.Parameters.AddWithValue("@id", recordNumber);

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

        public static int UpdateSavedSource(int userID, string source, int recordNumber)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblSavedSources SET SourceText=@source WHERE PersonnelID = @userID AND SourceNumber = @id", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@source", source);

                sql.UpdateCommand.Parameters.AddWithValue("@userID", userID);
                sql.UpdateCommand.Parameters.AddWithValue("@id", recordNumber);

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

        public static int RenameVariableName(string oldname, string newname, string survey)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_renameVariable", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@oldname", oldname);
                sql.UpdateCommand.Parameters.AddWithValue("@newname", newname);
                sql.UpdateCommand.Parameters.AddWithValue("@survey", survey);

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

        public static int RenameVariableName(VarNameChange change)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_renameVariable", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (Survey s in change.SurveysAffected)
                {
                    sql.UpdateCommand.Parameters.AddWithValue("@oldname", change.OldName);
                    sql.UpdateCommand.Parameters.AddWithValue("@newname", change.NewName);
                    sql.UpdateCommand.Parameters.AddWithValue("@survey", s.SurveyCode);
                }

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

        public static int UpdatePreP(Wording wording, string oldval, string newval, bool overwrite)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updatePreP", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@prepid", wording.WordID);
                sql.UpdateCommand.Parameters.AddWithValue("@oldval", oldval);
                sql.UpdateCommand.Parameters.AddWithValue("@newval", newval);
                sql.UpdateCommand.Parameters.AddWithValue("@overwrite", overwrite);
                sql.UpdateCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;


                try
                {
                    sql.UpdateCommand.ExecuteNonQuery();
                    wording.WordID = Convert.ToInt32(sql.UpdateCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int UpdatePreI(Wording wording, string oldval, string newval, bool overwrite)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updatePreI", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@preiid", wording.WordID);
                sql.UpdateCommand.Parameters.AddWithValue("@oldval", oldval);
                sql.UpdateCommand.Parameters.AddWithValue("@newval", newval);
                sql.UpdateCommand.Parameters.AddWithValue("@overwrite", overwrite);
                sql.UpdateCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;


                try
                {
                    sql.UpdateCommand.ExecuteNonQuery();
                    wording.WordID = Convert.ToInt32(sql.UpdateCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int UpdatePreA(Wording wording, string oldval, string newval, bool overwrite)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updatePreA", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@preaid", wording.WordID);
                sql.UpdateCommand.Parameters.AddWithValue("@oldval", oldval);
                sql.UpdateCommand.Parameters.AddWithValue("@newval", newval);
                sql.UpdateCommand.Parameters.AddWithValue("@overwrite", overwrite);
                sql.UpdateCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;


                try
                {
                    sql.UpdateCommand.ExecuteNonQuery();
                    wording.WordID = Convert.ToInt32(sql.UpdateCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int UpdateLitQ(Wording wording, string oldval, string newval, bool overwrite)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updateLitQ", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@litqid", wording.WordID);
                sql.UpdateCommand.Parameters.AddWithValue("@oldval", oldval);
                sql.UpdateCommand.Parameters.AddWithValue("@newval", newval);
                sql.UpdateCommand.Parameters.AddWithValue("@overwrite", overwrite);
                sql.UpdateCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;


                try
                {
                    sql.UpdateCommand.ExecuteNonQuery();
                    wording.WordID = Convert.ToInt32(sql.UpdateCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int UpdatePstI(Wording wording, string oldval, string newval, bool overwrite)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updatePstI", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@pstiid", wording.WordID);
                sql.UpdateCommand.Parameters.AddWithValue("@oldval", oldval);
                sql.UpdateCommand.Parameters.AddWithValue("@newval", newval);
                sql.UpdateCommand.Parameters.AddWithValue("@overwrite", overwrite);
                sql.UpdateCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;


                try
                {
                    sql.UpdateCommand.ExecuteNonQuery();
                    wording.WordID = Convert.ToInt32(sql.UpdateCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int UpdatePstP(Wording wording, string oldval, string newval, bool overwrite)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updatePstP", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@pstpid", wording.WordID);
                sql.UpdateCommand.Parameters.AddWithValue("@oldval", oldval);
                sql.UpdateCommand.Parameters.AddWithValue("@newval", newval);
                sql.UpdateCommand.Parameters.AddWithValue("@overwrite", overwrite);
                sql.UpdateCommand.Parameters.Add("@newID", SqlDbType.Int).Direction = ParameterDirection.Output;


                try
                {
                    sql.UpdateCommand.ExecuteNonQuery();
                    wording.WordID = Convert.ToInt32(sql.UpdateCommand.Parameters["@newID"].Value);
                }
                catch (Exception)
                {
                    return 1;
                }

            }

            return 0;
        }

        public static int UpdateSurveyPreP(string survey, int oldID, int newID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblSurveyNumbers Set PreP#=@newid WHERE Survey = @survey AND PreP#=@oldid", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@newid", newID);
                sql.UpdateCommand.Parameters.AddWithValue("@oldid", oldID);
                sql.UpdateCommand.Parameters.AddWithValue("@survey", survey);

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

        public static int UpdateSurveyPreI(string survey, int oldID, int newID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblSurveyNumbers Set PreI#=@newid WHERE Survey = @survey AND PreI#=@oldid", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@newid", newID);
                sql.UpdateCommand.Parameters.AddWithValue("@oldid", oldID);
                sql.UpdateCommand.Parameters.AddWithValue("@survey", survey);

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

        public static int UpdateSurveyPreA(string survey, int oldID, int newID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblSurveyNumbers Set PreA#=@newid WHERE Survey = @survey AND PreA#=@oldid", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@newid", newID);
                sql.UpdateCommand.Parameters.AddWithValue("@oldid", oldID);
                sql.UpdateCommand.Parameters.AddWithValue("@survey", survey);

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

        public static int UpdateSurveyLitQ(string survey, int oldID, int newID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblSurveyNumbers Set LitQ#=@newid WHERE Survey = @survey AND LitQ#=@oldid", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@newid", newID);
                sql.UpdateCommand.Parameters.AddWithValue("@oldid", oldID);
                sql.UpdateCommand.Parameters.AddWithValue("@survey", survey);

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

        public static int UpdateSurveyPstI(string survey, int oldID, int newID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblSurveyNumbers Set PstI#=@newid WHERE Survey = @survey AND PstI#=@oldid", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@newid", newID);
                sql.UpdateCommand.Parameters.AddWithValue("@oldid", oldID);
                sql.UpdateCommand.Parameters.AddWithValue("@survey", survey);

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

        public static int UpdateSurveyPstP(string survey, int oldID, int newID)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblSurveyNumbers Set PstP#=@newid WHERE Survey = @survey AND PstP#=@oldid", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@newid", newID);
                sql.UpdateCommand.Parameters.AddWithValue("@oldid", oldID);
                sql.UpdateCommand.Parameters.AddWithValue("@survey", survey);

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

        public static int UpdateVarNameChange(VarNameChangeRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblVarNameChanges Set [refVarNameNew] = @refVarNameNew, [NewName]=@NewName, @refVarNameOld=@refVarNameOld, OldName=@OldName, " +
                    "[ChangeDate]=@changeDate,[ChangedBy]=@changedby, [Authorization]=@authorization,[Reasoning]=@reasoning,[Source2]=@source,[TempVar]=@tempvar " +
                    "WHERE ID = @changeID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@changeID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@refVarNameNew", record.NewRefName);
                sql.UpdateCommand.Parameters.AddWithValue("@NewName", record.NewName);
                sql.UpdateCommand.Parameters.AddWithValue("@refVarNameOld", record.OldRefName);
                sql.UpdateCommand.Parameters.AddWithValue("@OldName", record.OldName);
                sql.UpdateCommand.Parameters.AddWithValue("@changeDate", record.ChangeDate);
                sql.UpdateCommand.Parameters.AddWithValue("@changedby", record.ChangedBy.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@authorization", record.Authorization);
                sql.UpdateCommand.Parameters.AddWithValue("@reasoning", record.Rationale);
                sql.UpdateCommand.Parameters.AddWithValue("@source", record.Source);
                sql.UpdateCommand.Parameters.AddWithValue("@tempvar", record.HiddenChange);

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

        public static int UpdateVarNameChangeSurvey(int changeID, Survey s)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblVarNameChangeSurveys Set [SurveyID] = @survid WHERE ID = @changeID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@changeID", changeID);
                sql.UpdateCommand.Parameters.AddWithValue("@survid", s.SID);
                

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

        public static int UpdateVarNameChangeSurvey(VarNameChangeSurveyRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblVarNameChangeSurveys Set [SurveyID] = @survid WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@survid", record.SurvID);


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

        public static int UpdateVarNameChangeNotification(VarNameChangeNotificationRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblVarNameChangeNotifications Set [NotifyName] = @notifyname, NotifyType = @notifytype WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@notifyname", record.PersonID);
                sql.UpdateCommand.Parameters.AddWithValue("@notifytype", record.NotifyType);

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

        public static int UpdatePersonnel(Person record)
        {

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updatePersonnel", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };


                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);

                sql.UpdateCommand.Parameters.AddWithValue("@firstname", string.IsNullOrEmpty(record.FirstName) ? DBNull.Value : (object)record.FirstName);
                sql.UpdateCommand.Parameters.AddWithValue("@lastname", string.IsNullOrEmpty(record.LastName) ? DBNull.Value : (object)record.LastName);
                sql.UpdateCommand.Parameters.AddWithValue("@username", string.IsNullOrEmpty(record.Username) ? DBNull.Value : (object)record.Username);
                sql.UpdateCommand.Parameters.AddWithValue("@email", string.IsNullOrEmpty(record.Email) ? DBNull.Value : (object)record.Email);
                sql.UpdateCommand.Parameters.AddWithValue("@workphone", string.IsNullOrEmpty(record.WorkPhone) ? DBNull.Value : (object)record.WorkPhone);
                sql.UpdateCommand.Parameters.AddWithValue("@homephone", string.IsNullOrEmpty(record.HomePhone) ? DBNull.Value : (object)record.HomePhone);
                sql.UpdateCommand.Parameters.AddWithValue("@officeno", string.IsNullOrEmpty(record.OfficeNo) ? DBNull.Value : (object)record.OfficeNo);
                sql.UpdateCommand.Parameters.AddWithValue("@institution", string.IsNullOrEmpty(record.Institution) ? DBNull.Value : (object)record.Institution);
                sql.UpdateCommand.Parameters.AddWithValue("@active", record.Active);
                sql.UpdateCommand.Parameters.AddWithValue("@smg", record.SMG);
                sql.UpdateCommand.Parameters.AddWithValue("@analyst", record.Analyst);
                sql.UpdateCommand.Parameters.AddWithValue("@praccer", record.Praccer);
                sql.UpdateCommand.Parameters.AddWithValue("@praccid", string.IsNullOrEmpty(record.PraccID) ? DBNull.Value : (object)record.PraccID);
                sql.UpdateCommand.Parameters.AddWithValue("@programmer", record.Programmer);
                sql.UpdateCommand.Parameters.AddWithValue("@firm", record.Firm);
                sql.UpdateCommand.Parameters.AddWithValue("@countryteam", record.CountryTeam);
                sql.UpdateCommand.Parameters.AddWithValue("@entry", record.Entry);
                sql.UpdateCommand.Parameters.AddWithValue("@praccentry", record.PraccEntry);
                sql.UpdateCommand.Parameters.AddWithValue("@admin", record.Admin);
                sql.UpdateCommand.Parameters.AddWithValue("@ra", record.ResearchAssistant);
                sql.UpdateCommand.Parameters.AddWithValue("@dissemination", record.Dissemination);
                sql.UpdateCommand.Parameters.AddWithValue("@investigator", record.Investigator);
                sql.UpdateCommand.Parameters.AddWithValue("@projectmanager", record.ProjectManager);
                sql.UpdateCommand.Parameters.AddWithValue("@statistician", record.Statistician);
                sql.UpdateCommand.Parameters.AddWithValue("@varnamechangenotify", record.VarNameChangeNotify);

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

        public static int UpdatePersonnelStudy(PersonnelStudyRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblPersonnelCountry SET CountryID = @countryID WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@countryID", record.StudyID);
               
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

        public static int UpdatePersonnelComment(PersonnelCommentRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblPersonnelComments SET CommentType = @commentType, Comment = @comment WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@commentType", record.CommentType);
                sql.UpdateCommand.Parameters.AddWithValue("@comment", record.Comment);

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

        public static int UpdateCohort(SurveyCohortRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblCohort SET Cohort = @cohort, Code = @code, WebName =@webname WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@cohort", record.Cohort);
                sql.UpdateCommand.Parameters.AddWithValue("@code", record.Code);
                sql.UpdateCommand.Parameters.AddWithValue("@webname", record.WebName);

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

        public static int UpdateUserState(UserStateRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {

                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblUserStates SET UserState = @userstate WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@userstate", record.UserStateName);

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

        public static int UpdateSimilarWords(SimilarWordsRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblAlternateSpelling SET Word = @words WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@words", record.Words);

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

        public static int UpdateCanonVar(CanonicalVariableRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblCanonVars SET VarName = @refvarname, AnySuffix=@anysuffix, Notes=@notes, Active= @active WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@refvarname", record.RefVarName);
                sql.UpdateCommand.Parameters.AddWithValue("@anysuffix", record.AnySuffix);
                sql.UpdateCommand.Parameters.AddWithValue("@notes", record.Notes);
                sql.UpdateCommand.Parameters.AddWithValue("@active", record.Active);

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

        public static int UpdatePrefix(VariablePrefixRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE tblDomainList SET prefix = @prefix, PrefixName=@prefixName, ProductType=@productType, RelatedPrefixes= @relatedPrefixes," +
                    "DomainName=@domainName, Comments=@comments, InactiveDomain=@inactive WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@prefix", record.Prefix);
                sql.UpdateCommand.Parameters.AddWithValue("@prefixName", record.PrefixName);
                sql.UpdateCommand.Parameters.AddWithValue("@productType", record.ProductType);
                sql.UpdateCommand.Parameters.AddWithValue("@relatedPrefixes", record.RelatedPrefixes);
                sql.UpdateCommand.Parameters.AddWithValue("@domainName", record.Description);
                sql.UpdateCommand.Parameters.AddWithValue("@comments", record.Comments);
                sql.UpdateCommand.Parameters.AddWithValue("@inactive", record.Inactive);

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

        public static int UpdatePrefixRange(VariableRangeRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("proc_updatePrefixRange", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@low", string.IsNullOrEmpty(record.Lower) ? DBNull.Value : (object)record.Lower);
                sql.UpdateCommand.Parameters.AddWithValue("@high", string.IsNullOrEmpty(record.Upper) ? DBNull.Value : (object)record.Upper);
                sql.UpdateCommand.Parameters.AddWithValue("@description", string.IsNullOrEmpty(record.Description) ? DBNull.Value : (object)record.Description);

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

        public static int UpdateParallelPrefix(ParallelPrefixRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE qryDomainListRelated SET RelatedPrefixID=@relatedID WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@relatedID", record.RelatedID);
                

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

        public static int UpdateSurveyDraft(SurveyDraftRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE qrySurveyDraftInfo SET SurvID=@survID, DraftTitle=@title, DraftDate=@date, DraftComments=@comments, Investigator=@investigator WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@title", record.DraftTitle);
                sql.UpdateCommand.Parameters.AddWithValue("@date", record.DraftDate);
                sql.UpdateCommand.Parameters.AddWithValue("@comments", record.DraftComments);
                sql.UpdateCommand.Parameters.AddWithValue("@investigator", record.Investigator);
                sql.UpdateCommand.Parameters.AddWithValue("@survID", record.SurvID);

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

        public static int UpdateSurveyDraftExtraField(SurveyDraftExtraFieldRecord record)
        {
            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.UpdateCommand = new SqlCommand("UPDATE qrySurveyDraftExtraFields SET DraftID=@draftID, ExtraFieldNum=@fieldNum, ExtraFieldLabel=@fieldLabel WHERE ID = @ID", conn)
                {
                    CommandType = CommandType.Text
                };

                sql.UpdateCommand.Parameters.AddWithValue("@ID", record.ID);
                sql.UpdateCommand.Parameters.AddWithValue("@draftID", record.DraftID);
                sql.UpdateCommand.Parameters.AddWithValue("@fieldNum", record.FieldNumber);
                sql.UpdateCommand.Parameters.AddWithValue("@fieldLabel", record.Label);
                

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
