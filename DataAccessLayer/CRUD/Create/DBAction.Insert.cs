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
            int rowsAffected = 0;
            newID = 0;
            using (IDbConnection db = new SqlConnection(connectionString))
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
            return 0;
        }

        public static int InsertNote(Note record)
        {
            string sql = "proc_createNote";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@noteText", record.NoteText);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected;
            using (IDbConnection db = new SqlConnection(connectionString))
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
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
                question.ID = parameters.Get<int>("@newID");
            }

            if (rowsAffected == 0)
                return 1;
            else
                return 0;
        }

        public static int InsertQuestion(SurveyQuestion question)
        {
            string sql = "proc_createQuestion";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@survey", question.SurveyCode);
            parameters.Add("@varname", question.VarName.VarName);
            parameters.Add("@qnum", question.Qnum);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
                question.ID = parameters.Get<int?>("@newID") ?? 0;
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
        public static int InsertRegion(Region region)
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
        public static int InsertCountry(Study newStudy)
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
        public static int InsertStudyWave(StudyWave newWave)
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
            parameters.Add("@fieldname", wording.FieldType);
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
            parameters.Add("@fieldname", respSet.FieldType);

            int recordsAffected = SP_Insert("proc_createResponseSet", parameters, out int newID);          

            return recordsAffected;
        }

        /// <summary>
        /// Inserts a new survey draft record.
        /// </summary>
        /// <param name="draft"></param>
        /// <returns></returns>
        public static int InsertSurveyDraft(SurveyDraft draft)
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

            int recordsAffected = SP_Insert("proc_createSurveyDraftExtraInfo", parameters, out int result);

            return recordsAffected;
        }

        /// <summary>
        /// Inserts a new survey draft question. TODO set ID 
        /// </summary>
        /// <param name="dq"></param>
        /// <returns></returns>
        public static int InsertDraftQuestion(DraftQuestion dq)
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

            int recordsAffected = SP_Insert("proc_createSurveyDraftQuestion", parameters, out int result);
            dq.ID = result;

            return recordsAffected;
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
        public static int InsertSurveyCheckRecord(SurveyCheck record)
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
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@survID", record.SurvID);
            parameters.Add("@stateID", record.State.ID);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createSurveyUserState", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
       }

        public static int InsertSurveyScreenedProduct(SurveyScreenedProduct record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@survID", record.SurvID);
            parameters.Add("@stateID", record.Product.ID);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createSurveyProduct", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertPraccingIssue(PraccingIssue record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@survID", record.Survey.SID);
            parameters.Add("@varnames", record.VarNames);
            parameters.Add("@date", record.IssueDate);
            parameters.Add("@from", record.IssueFrom.ID);
            parameters.Add("@to", record.IssueTo.ID);
            parameters.Add("@description", record.Description);
            parameters.Add("@category", record.Category.ID);
            parameters.Add("@resolved", record.Resolved);
            parameters.Add("@resolvedby", record.ResolvedBy.ID);
            parameters.Add("@resolvedon", record.ResolvedDate);
            parameters.Add("@language", record.Language);
            parameters.Add("@pin", record.PinNo);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@newNo", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = 0;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                recordsAffected = db.Execute("proc_createPraccIssue", parameters, commandType: CommandType.StoredProcedure);
                record.ID = parameters.Get<int>("@newID");
                record.IssueNo = parameters.Get<int>("@newNo");
            }

            return recordsAffected;
        }

        public static int InsertPraccingResponse(PraccingResponse record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@praccID", record.IssueID);
            parameters.Add("@date", record.ResponseDate);
            parameters.Add("@from", record.ResponseFrom.ID);
            parameters.Add("@to", record.ResponseTo.ID);
            parameters.Add("@response", record.Response);
            parameters.Add("@pin", record.PinNo);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createPraccResponse", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertPraccingImage(int praccingID, List<PraccingImage> images)
        {
            foreach (PraccingImage img in images)
            {
                img.PraccID = praccingID;
                InsertPraccingImage(img);
            }
            return 0;
        }

        public static int InsertPraccingImage(PraccingImage record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@praccID", record.PraccID);
            parameters.Add("@path", record.Path);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createPraccingImage", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertPraccingResponseImage(int responseID, List<PraccingImage> images)
        {
            foreach (PraccingImage img in images)
            {
                img.PraccID = responseID;
                InsertPraccingResponseImage(img);
            }
            return 0;
        }

        public static int InsertPraccingResponseImage(PraccingImage record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@praccResponseID", record.PraccID);
            parameters.Add("@path", record.Path);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createPraccingResponseImage", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertSurveyProcessingDate(SurveyProcessingDate record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@stageID", record.StageID);
            parameters.Add("@stagedate", record.StageDate);
            parameters.Add("@entrydate", record.EntryDate);
            parameters.Add("@stageinit", record.EnteredBy.ID);
            parameters.Add("@contact", record.Contact.ID);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createSurveyProcessingDate", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertSurveyProcessingNote(SurveyProcessingNote record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@stageID", record.DateID);
            parameters.Add("@enteredby", record.Author.ID);
            parameters.Add("@commentdate", record.NoteDate);
            parameters.Add("@note", record.Note);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createSurveyProcessingNote", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertSurvey(Survey record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@survey", record.SurveyCode);
            parameters.Add("@title", record.Title);
            parameters.Add("@cohort", record.Cohort.ID);
            parameters.Add("@mode", record.Mode.ID);
            parameters.Add("@filename", record.WebName);
            parameters.Add("@date", record.CreationDate);
            parameters.Add("@hidesurvey", record.HideSurvey);
            parameters.Add("@waveid", record.WaveID);
            parameters.Add("@itcsurvey", record.ITCSurvey);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createSurvey", parameters, out int newID);
            record.SID = newID;

            return recordsAffected;
        }

        public static int InsertQuestionComment(QuestionComment record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@survey", record.Survey);
            parameters.Add("@varname", record.VarName);
            parameters.Add("@commentText", record.Notes.NoteText);
            parameters.Add("@notedate", record.NoteDate);
            parameters.Add("@noteinit", record.Author.ID);
            parameters.Add("@authority", record.Authority.ID);
            parameters.Add("@notetype", record.NoteType.ID);
            parameters.Add("@source", record.Source);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createQuestionComment", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertSurveyComment(SurveyComment record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@survey", record.Survey);
            parameters.Add("@commentText", record.Notes.NoteText);
            parameters.Add("@notedate", record.NoteDate);
            parameters.Add("@noteinit", record.Author.ID);
            parameters.Add("@authority", record.Authority.ID); 
            parameters.Add("@notetype", record.NoteType.ID);
            parameters.Add("@source", record.Source);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createSurveyComment", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertWaveComment(WaveComment record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@wave", record.StudyWave);
            parameters.Add("@commentText", record.Notes.NoteText);
            parameters.Add("@notedate", record.NoteDate);
            parameters.Add("@noteinit", record.Author.ID);
            parameters.Add("@authority", record.Authority.ID);
            parameters.Add("@notetype", record.NoteType.ID);
            parameters.Add("@source", record.Source);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createWaveComment", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertDeletedComment(DeletedComment record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@survey", record.Survey);
            parameters.Add("@varname", record.VarName);
            parameters.Add("@commentText", record.Notes.NoteText);
            parameters.Add("@notedate", record.NoteDate);
            parameters.Add("@noteinit", record.Author.ID);
            parameters.Add("@authority", record.Authority.ID);
            parameters.Add("@notetype", record.NoteType.ID);
            parameters.Add("@source", record.Source);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createDeletedComment", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertRefVarComment(RefVarComment record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@varname", record.RefVarName);
            parameters.Add("@commentText", record.Notes.NoteText);
            parameters.Add("@notedate", record.NoteDate);
            parameters.Add("@noteinit", record.Author.ID);
            parameters.Add("@authority", record.Authority.ID);
            parameters.Add("@notetype", record.NoteType.ID);
            parameters.Add("@source", record.Source);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createRefVarComment", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertVarNameChange(VarNameChange record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@refVarNameNew", record.NewRefName);
            parameters.Add("@NewName", record.NewName);
            parameters.Add("@refVarNameOld", record.OldRefName);
            parameters.Add("@OldName", record.OldName);
            parameters.Add("@ChangeDate", record.ChangeDate);
            parameters.Add("@ChangedBy", record.ChangedBy.ID);
            parameters.Add("@Authorization", record.Authorization);
            parameters.Add("@Reasoning", record.Rationale);
            parameters.Add("@source", record.Source);
            parameters.Add("@tempvar", record.HiddenChange);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createVarNameChange", parameters, out int newID);
            record.ID = newID;

            foreach (VarNameChangeSurvey sr in record.SurveysAffected)
            {
                sr.ChangeID = record.ID;
                InsertVarNameChangeSurvey(sr);
            }

            foreach (VarNameChangeNotification nr in record.Notifications)
            {
                nr.ChangeID = record.ID;
                InsertVarNameChangeNotification(nr);
            }

            return recordsAffected;
        }

        public static int InsertVarNameChangeSurvey(VarNameChangeSurvey survey)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@changeID", survey.ChangeID);
            parameters.Add("@survID", survey.SurveyCode.SID);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createVarNameChangeSurvey", parameters, out int newID);
            survey.ID = newID;

            return recordsAffected;
        }

        public static int InsertVarNameChangeNotification(VarNameChangeNotification record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@changeID", record.ChangeID);
            parameters.Add("@notifyname", record.Name.ID);
            parameters.Add("@notifytype", record.NotifyType);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createVarNameChangeNotification", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertPersonnel(Person record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@firstname", record.FirstName);
            parameters.Add("@lastname", record.LastName);
            parameters.Add("@username", record.Username);
            parameters.Add("@email", record.Email);
            parameters.Add("@workphone", record.WorkPhone);
            parameters.Add("@homephone", record.HomePhone);
            parameters.Add("@officeno", record.OfficeNo);
            parameters.Add("@institution", record.Institution);
            parameters.Add("@active", record.Active);
            parameters.Add("@praccid", record.PraccID);
            parameters.Add("@entry", record.Entry);
            parameters.Add("@praccentry", record.PraccEntry);
            parameters.Add("@varnamechangenotify", record.VarNameChangeNotify);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createPersonnel", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertPersonnelRole(PersonnelRole record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@personnelID", record.PersonnelID);
            parameters.Add("@roleID", record.RoleName.ID);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createPersonnelRole", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertPersonnelStudy(PersonnelStudy record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@personnelID", record.PersonnelID);
            parameters.Add("@countryID", record.StudyName.ID);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createPersonnelCountry", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertPersonnelComment(PersonnelComment record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@personnelID", record.PersonnelID);
            parameters.Add("@commentType", record.CommentType);
            parameters.Add("@comment", record.Comment);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createPersonnelComment", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertCohort(SurveyCohort record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@cohort", record.Cohort);
            parameters.Add("@code", record.Code);
            parameters.Add("@webname", record.WebName);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createCohort", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertUserState(UserState record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@userstate", record.UserStateName);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createUserState", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertSimilarWords(SimilarWords record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@words", record.WordList);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createSimilarWords", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertCanonVar(CanonicalRefVarName record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@refVarName", record.RefVarName);
            parameters.Add("@anysuffix", record.AnySuffix);
            parameters.Add("@notes", record.Notes);
            parameters.Add("@active", record.Active);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int recordsAffected = SP_Insert("proc_createCanonVar", parameters, out int newID);
            record.ID = newID;

            return recordsAffected;
        }

        public static int InsertPrefix(VariablePrefix record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@prefix", record.Prefix);
            parameters.Add("@prefixName", record.PrefixName);
            parameters.Add("@productType", record.ProductType);
            parameters.Add("@relatedPrefixes", record.Prefix);
            parameters.Add("@domainName", record.Prefix);
            parameters.Add("@comments", record.Prefix);
            parameters.Add("@inactive", record.Prefix);
            parameters.Add("@newID", record.Prefix, DbType.Int32, ParameterDirection.Output);

            int rowsAffected = SP_Insert("proc_createPrefix", parameters, out int newID);
            record.ID = newID;

            return rowsAffected;
        }

        public static int InsertPrefixRange(VariableRange record)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@prefixID", record.PrefixID);
            parameters.Add("@low", record.Lower);
            parameters.Add("@high", record.Upper);
            parameters.Add("@description", record.Description);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected = SP_Insert("proc_createPrefixRange", parameters, out int newID);
            record.ID = newID;

            return rowsAffected;
        }

        public static int InsertParallelPrefix(ParallelPrefix record)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@prefixID", record.PrefixID);
            parameters.Add("@relatedPrefixID", record.RelatedPrefixID);

            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected = SP_Insert("proc_createRelatedPrefix", parameters, out int newID);
            record.ID = newID;

            return rowsAffected;
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
            using (IDbConnection db = new SqlConnection(connectionString))
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
            using (IDbConnection db = new SqlConnection(connectionString))
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

        public static int InsertGrantLabel (string survey, string varname, string grantcode)
        {
            string sql = "proc_createQuestionGrantLabel";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@survey", survey);
            parameters.Add("@varname", varname);
            parameters.Add("@grantcode", grantcode);
            //parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected = SP_Insert(sql, parameters, out int newID);
            
            return rowsAffected;
        }

        public static int InsertLastUsedComment(UserPrefs user, Comment comment)
        {
            string sql = "proc_upsertLastUsedComment";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@userID", user.userid);
            parameters.Add("@date", comment.NoteDate);
            parameters.Add("@type", comment.NoteType.ID);
            parameters.Add("@author", comment.Author.ID);
            parameters.Add("@source", comment.Source);
            parameters.Add("@authority", comment.Authority.ID);

            int rowsAffected = SP_Insert(sql, parameters, out int newID);

            return rowsAffected;
        }

        public static int InsertQuestionImage(SurveyImage img)
        {
            string sql = "proc_createQuestionImage";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@qid", img.QID);
            parameters.Add("@imagename", img.ImageName);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected = SP_Insert(sql, parameters, out int newID);

            return rowsAffected;
        }
    }
}
