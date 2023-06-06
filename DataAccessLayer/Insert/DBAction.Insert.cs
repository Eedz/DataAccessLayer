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
        public static int SP_Insert(string procedureName, DynamicParameters parameters, out int newID)
        {
            int rowsAffected=0;
            newID = 0;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                try
                {
                    rowsAffected = db.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);

                    if (parameters.ParameterNames.Contains("newID"))
                    {
                        var output = parameters.Get<dynamic>("@newID");
                        if (output != null)
                            newID = parameters.Get<int>("@newID");
                    }
                }
                catch (SqlException)
                {
                    return 1;
                }
                catch 
                {

                }
            }

            return rowsAffected;

        }

        public static int InsertNote(Note record)
        {
            string sql = "proc_createNote";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@noteText", record.NoteText);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
                record.ID = parameters.Get<int>("@newID");
            }

            if (rowsAffected == 0)
                return 1;
            else
                return 0;
        }

        public static int InsertQuestion(string surveyCode, SurveyQuestion question)
        {
            string sql = "proc_createQuestion";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@survey", surveyCode);
            parameters.Add("@varname", question.VarName.VarName);
            parameters.Add("@qnum", question.Qnum);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
                question.ID = parameters.Get<int>("@newID");
            }

            if (rowsAffected == 0)
                return 1;
            else
                return 0;
        }

        public static int InsertVariable(VariableName varname)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@varname", varname.VarName);
            parameters.Add("@varlabel", varname.VarLabel);
            parameters.Add("@content", varname.Content.ID);
            parameters.Add("@topic", varname.Topic.ID);
            parameters.Add("@domain", varname.Domain.ID);
            parameters.Add("@product", varname.Product.ID);
            
            int recordsAffected = SP_Insert("proc_createVariableLabeled", parameters, out int id);

            return recordsAffected;
        }

        /// <summary>
        /// Creates a new Domain label.
        /// </summary>
        /// <param name="newLabel"></param>
        /// <returns></returns>
        public static int InsertDomainLabel(DomainLabel newLabel)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@type", "Domain");
            parameters.Add("@label", newLabel.LabelText);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createLabel", parameters, out int newID);

            newLabel.ID = newID;

            return recordsAffected;
        }

        /// <summary>
        /// Creates a new Topic label.
        /// </summary>
        /// <param name="newLabel"></param>
        /// <returns></returns>
        public static int InsertTopicLabel(TopicLabel newLabel)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@type", "Topic");
            parameters.Add("@label", newLabel.LabelText);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createLabel", parameters, out int newID);

            newLabel.ID = newID;

            return recordsAffected;
        }

        /// <summary>
        /// Creates a new Content label.
        /// </summary>
        /// <param name="newLabel"></param>
        /// <returns></returns>
        public static int InsertContentLabel(ContentLabel newLabel)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@type", "Content");
            parameters.Add("@label", newLabel.LabelText);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createLabel", parameters, out int newID);

            newLabel.ID = newID;

            return recordsAffected;
        }

        /// <summary>
        /// Creates a new Product label.
        /// </summary>
        /// <param name="newLabel"></param>
        /// <returns></returns>
        public static int InsertProductLabel(ProductLabel newLabel)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@type", "Product");
            parameters.Add("@label", newLabel.LabelText);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createLabel", parameters, out int newID);

            newLabel.ID = newID;

            return recordsAffected;
        }

        /// <summary>
        /// Creates a new Keyword.
        /// </summary>
        /// <param name="newKeyword"></param>
        /// <returns></returns>
        public static int InsertKeyword(Keyword newKeyword)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@type", "Keyword");
            parameters.Add("@label", newKeyword.LabelText);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createLabel", parameters, out int newID);

            newKeyword.ID = newID;

            return recordsAffected;
        }

       
        /// <summary>
        /// Creates a new Region and Reserved Prefix for that region.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public static int InsertRegion(RegionRecord region)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@regionName", region.RegionName);
            parameters.Add("@tempPrefix", region.TempVarPrefix);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createRegion", parameters, out int newID);

            region.ID = newID;

            return recordsAffected;
        }

        /// <summary>
        /// Inserts a new study record.
        /// </summary>
        /// <param name="newStudy"></param>
        /// <returns></returns>
        public static int InsertCountry(StudyRecord newStudy)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@studyName", newStudy.StudyName);
            parameters.Add("@countryName", newStudy.CountryName);
            parameters.Add("@ageGroup", newStudy.AgeGroup);
            parameters.Add("@countryCode", newStudy.CountryCode);
            parameters.Add("@ISO_Code", newStudy.ISO_Code);
            parameters.Add("@region", newStudy.RegionID);
            parameters.Add("@cohort", newStudy.Cohort);
            parameters.Add("@languages", newStudy.Languages);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createStudy", parameters, out int newID);

            newStudy.ID = newID;

            return recordsAffected;
        }

        /// <summary>
        /// Inserts a new wave record.
        /// </summary>
        /// <param name="newWave"></param>
        /// <returns></returns>
        public static int InsertStudyWave(StudyWaveRecord newWave)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@studyID", newWave.StudyID);
            parameters.Add("@waveNum", newWave.Wave);
            parameters.Add("@countries", newWave.Countries);
            parameters.Add("@englishRouting", newWave.EnglishRouting);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createWave", parameters, out int newID);

            newWave.ID = newID;

            return recordsAffected;
        }

        /// <summary>
        /// Inserts a new wording record.
        /// </summary>
        /// <param name="wording"></param>
        /// <returns></returns>
        public static int InsertWording(Wording wording)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@wording", wording.WordingText);
            parameters.Add("@fieldname", wording.FieldName);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createWording", parameters, out int newID);

            wording.WordID = newID;

            return recordsAffected;
        }

        /// <summary>
        /// Inserts a response set record.
        /// </summary>
        /// <param name="respSet"></param>
        /// <returns></returns>
        public static int InsertResponseSet(ResponseSet respSet)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@wording", respSet.RespList);
            parameters.Add("@setname", respSet.RespSetName);
            parameters.Add("@fieldname", respSet.FieldName);

            int recordsAffected = SP_Insert("proc_createResponseSet", parameters, out int newID);          

            return recordsAffected;
        }

        /// <summary>
        /// Inserts a new survey draft record.
        /// </summary>
        /// <param name="draft"></param>
        /// <returns></returns>
        public static int InsertSurveyDraft(SurveyDraftRecord draft)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SurvID", draft.SurvID);
            parameters.Add("@DraftTitle", draft.DraftTitle);
            parameters.Add("@DraftDate", draft.DraftDate);
            parameters.Add("@DraftComments", draft.DraftComments);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createSurveyDraft", parameters, out int newID);
            draft.ID = newID;

            return recordsAffected;
        }

        /// <summary>
        /// Inserts extra field label information for a specified draft.
        /// </summary>
        /// <param name="draftID"></param>
        /// <param name="extraFieldNum"></param>
        /// <param name="extraFieldLabel"></param>
        /// <returns></returns>
        public static int InsertSurveyDraftExtraInfo(int draftID, int extraFieldNum, string extraFieldLabel)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DraftID", draftID);
            parameters.Add("@ExtraFieldNum", extraFieldNum);
            parameters.Add("@ExtraFieldLabel", extraFieldLabel);

            SP_Insert("proc_createSurveyDraftExtraInfo", parameters, out int result);
            return result;
        }

        /// <summary>
        /// Inserts a new survey draft question. TODO set ID 
        /// </summary>
        /// <param name="dq"></param>
        /// <returns></returns>
        public static int InsertDraftQuestion(DraftQuestionRecord dq)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DraftID", dq.DraftID);
            parameters.Add("@SortBy", dq.SortBy);
            parameters.Add("@Qnum", dq.Qnum);
            parameters.Add("@AltQnum", dq.AltQnum);
            parameters.Add("@VarName", dq.VarName);
            parameters.Add("@QuestionText", dq.QuestionText);
            parameters.Add("@Comment", dq.Comments);
            parameters.Add("@Extra1", dq.Extra1);
            parameters.Add("@Extra2", dq.Extra2);
            parameters.Add("@Extra3", dq.Extra3);
            parameters.Add("@Extra4", dq.Extra4);
            parameters.Add("@Extra5", dq.Extra5);
            parameters.Add("@Inserted", dq.Inserted);
            parameters.Add("@Deleted", dq.Deleted);

            SP_Insert("proc_createSurveyDraftQuestion", parameters, out int result);
            return result;
        }

        /// <summary>
        /// Inserts a new translation record.
        /// </summary>
        /// <param name="tq"></param>
        /// <returns></returns>
        public static int InsertTranslation(Translation tq)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@qid", tq.QID);
            parameters.Add("@text", tq.TranslationText);
            parameters.Add("@lang", tq.LanguageName.ID);
            parameters.Add("@languageName", tq.Language);
            parameters.Add("@bilingual", tq.Bilingual);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createTranslation", parameters, out int newID);
            tq.ID = newID;

            return recordsAffected;
        }

        /// <summary>
        /// Inserts a new survey check record.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int InsertSurveyCheckRecord(SurveyCheckRec record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@checkDate", record.CheckDate);
            parameters.Add("@checkInit", record.Name.ID);
            parameters.Add("@comments", record.Comments);
            parameters.Add("@survID", record.SurveyCode.SID);
            parameters.Add("@checkType", record.CheckType.ID);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            //sql.InsertCommand.Parameters.AddWithValue("@sendDate", record.SentOn);
            //sql.InsertCommand.Parameters.AddWithValue("@sendTo", record.SentTo.ID);
            //sql.InsertCommand.Parameters.AddWithValue("@reviewed", record.Reviewed);
            //sql.InsertCommand.Parameters.AddWithValue("@reviewedBy", record.ReviewedBy.ID);
            //sql.InsertCommand.Parameters.AddWithValue("@reviewDetails", record.ReviewDetails);
            //sql.InsertCommand.Parameters.AddWithValue("@editsMadeDate", record.Edited);

            int recordsAffected = SP_Insert("proc_createSurveyCheckRecord", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        /// <summary>
        /// Inserts a new survey check reference survey record.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int InsertSurveyCheckRef(SurveyCheckRefSurvey record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@checkID", record.CheckID);
            parameters.Add("@survID", record.SID);
            parameters.Add("@survDate", record.SurveyDate);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            //sql.InsertCommand.Parameters.AddWithValue("@sendDate", record.SentOn);
            //sql.InsertCommand.Parameters.AddWithValue("@sendTo", record.SentTo.ID);
            //sql.InsertCommand.Parameters.AddWithValue("@reviewed", record.Reviewed);
            //sql.InsertCommand.Parameters.AddWithValue("@reviewedBy", record.ReviewedBy.ID);
            //sql.InsertCommand.Parameters.AddWithValue("@reviewDetails", record.ReviewDetails);
            //sql.InsertCommand.Parameters.AddWithValue("@editsMadeDate", record.Edited);

            int recordsAffected = SP_Insert("proc_createSurveyCheckRef", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        /// <summary>
        /// Inserts a new language.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int InsertLanguage(Language record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@language", record.LanguageName);
            parameters.Add("@abbrev", record.Abbrev);
            parameters.Add("@isoabbrev", record.ISOAbbrev);
            parameters.Add("@nonLatin", record.NonLatin);
            parameters.Add("@font", record.PreferredFont);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createLanguage", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        /// <summary>
        /// Inserts a new language.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int InsertSurveyLanguage(SurveyLanguage record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@survID", record.SurvID);
            parameters.Add("@langID", record.SurvLanguage.ID);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createSurveyLanguage", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
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

        public static int InsertParallelQuestion(ParallelQuestion record)
        {
            string sql = "proc_createParallelQuestion";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@matchID", record.MatchID);
            parameters.Add("@qid", record.Question.ID);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected = SP_Insert(sql, parameters, out int newID);
            record.ID = newID;

            return rowsAffected;
        }

        /// <summary>
        /// Inserts a set of parallel questions but inserting the first one, then applying the matchID to the subsequent members.
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public static int InsertParallelQuestion(List<ParallelQuestion> records)
        {
            string sql = "proc_createParallelQuestionMatch";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@qid", records[0].Question.ID);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@matchID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected = 0;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
                records[0].ID = parameters.Get<int>("@newID");
                records[0].MatchID = parameters.Get<int>("@matchID");
            }
                    
            sql = "proc_createParallelQuestion";

            foreach (ParallelQuestion record in records)
            {
                if (record.ID > 0) continue;
                parameters = new DynamicParameters();
                parameters.Add("@matchID", records[0].MatchID);
                parameters.Add("@qid", record.Question.ID);
                parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                rowsAffected += SP_Insert(sql, parameters, out int newID);
                record.ID = newID;
            }
            return rowsAffected;
        }

        public static int InsertPlainLanguageFilter(int qid, string filter)
        {
            string sql = "INSERT INTO tblFilterDescriptions (QID, FilterDescription) VALUES (@QID, @filter)";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@QID", qid);
            parameters.Add("@filter", filter);

            int rowsAffected = 0;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                rowsAffected = db.Execute(sql, parameters);
            }

            return rowsAffected;
        }

        public static int InsertQuestionTimeFrame(QuestionTimeFrame record)
        {
            string sql = "proc_createQuestionTimeFrame";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@QID", record.QID);
            parameters.Add("@timeframe", record.TimeFrame);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected = SP_Insert(sql, parameters, out int newID);
            record.ID = newID;

            return rowsAffected;
        }
    }
}
