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
        /// <summary>
        /// Updates the survey record matching the provided survey object.
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static int UpdateSurvey(Survey survey)
        {
            string sql = "UPDATE tblStudyAttributes " +
                                "SET Survey=@surveycode, SurveyTitle=@title, Cohort=@cohort, Mode=@mode, " +
                                    "SurveyFileName=@filename, NCT=@nct, ReRun=@rerun, HideSurvey=@hide, Locked=@locked, ITCSurvey=@itc, " +
                                    "ISISCreationDate=@date WHERE ID = @SID;";

            var parameters = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("SID", survey.SID),
                new KeyValuePair<string, object>("surveycode", survey.SurveyCode),
                new KeyValuePair<string, object>("title", survey.Title),
                new KeyValuePair<string, object>("cohort", survey.Cohort.ID),
                new KeyValuePair<string, object>("mode", survey.Mode.ID),
                new KeyValuePair<string, object>("filename", survey.WebName),
                new KeyValuePair<string, object>("nct", survey.NCT),
                new KeyValuePair<string, object>("rerun", survey.ReRun),
                new KeyValuePair<string, object>("hide", survey.HideSurvey),
                new KeyValuePair<string, object>("locked", survey.Locked),
                new KeyValuePair<string, object>("itc", survey.ITCSurvey),
                new KeyValuePair<string, object>("date", survey.CreationDate)
            };

            return Update_Query(sql, parameters);
        }

        /// <summary>
        /// Updates the wording numbers for the provided question. 
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public static int UpdateQuestionWordings(SurveyQuestion question)
        {
            string sql = "proc_updateQuestionWordings";
            var parameters = new
            {
                QID = question.ID,
                prep = question.PrePW.WordID,
                prei = question.PreIW.WordID,
                prea = question.PreAW.WordID,
                litq = question.LitQW.WordID,
                psti = question.PstIW.WordID,
                pstp = question.PstPW.WordID,
                respname = question.RespOptionsS.RespSetName,
                nrname = question.NRCodesS.RespSetName
            };
            int rowsAffected = 0;

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
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

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            return 0;
        }

        /// <summary>
        /// Updates the labels for a VariableName.
        /// </summary>
        /// <param name="varname"></param>
        /// <returns></returns>
        public static int UpdateLabels(VariableName varname)
        {
            string sql = "proc_updateLabels";

            var parameters = new
            {
                varname = varname.VarName,
                varlabel = varname.VarLabel,
                domain = varname.Domain.ID,
                topic = varname.Topic.ID,
                content = varname.Content.ID,
                product = varname.Product.ID
            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the appropriate wording table.
        /// </summary>
        /// <param name="wording"></param>
        /// <returns></returns>
        public static int UpdateWording(Wording wording)
        {
            string query = string.Empty;
            switch (wording.Type)
            {
                case WordingType.PreP:
                    query = "UPDATE tblPreP SET Wording =@wording WHERE ID =@ID";
                    break;
                case WordingType.PreI:
                    query = "UPDATE tblPreI SET Wording =@wording WHERE ID =@ID";
                    break;
                case WordingType.PreA:
                    query = "UPDATE tblPreA SET Wording =@wording WHERE ID =@ID";
                    break;
                case WordingType.LitQ:
                    query = "UPDATE tblLitQ SET Wording =@wording WHERE ID =@ID";
                    break;
                case WordingType.PstI:
                    query = "UPDATE tblPstI SET Wording =@wording WHERE ID =@ID";
                    break;
                case WordingType.PstP:
                    query = "UPDATE tblPstP SET Wording =@wording WHERE ID =@ID";
                    break;
            }

            var parameters = new { ID = wording.WordID, wording = wording.WordingText };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the appropriate response set table.
        /// </summary>
        /// <param name="respSet"></param>
        /// <returns></returns>
        public static int UpdateResponseSet(ResponseSet respSet)
        {
            string field = respSet.FieldType;
            string query = string.Empty;
            switch (field)
            {
                case "RespOptions":
                    query = "UPDATE tblRespOptionsTableCombined SET RespOptions =@wording WHERE RespName =@setname";
                    break;
                case "NRCodes":
                    query = "UPDATE tblNonRespOptions SET NRCodes =@wording WHERE NRName =@setname";
                    break;
            }

            var parameters = new { setname = respSet.RespSetName, wording = respSet.RespList };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the region table.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public static int UpdateRegion(Region region)
        {
            string query = "proc_updateRegion";

            var parameters = new
            {
                id = region.ID,
                regionName = region.RegionName,
                tempPrefix = region.TempVarPrefix
            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        /// <summary>
        /// Updates Study info for specified study object.
        /// </summary>
        /// <param name="study"></param>
        /// <returns></returns>
        public static int UpdateStudy(Study study)
        {
            string query = "proc_updateStudy";
            var parameters = new
            {
                StudyID = study.ID,
                studyName = study.StudyName,
                countryName = study.CountryName,
                ageGroup = study.AgeGroup,
                countryCode = study.CountryCode,
                ISO_Code = study.ISO_Code,
                region = study.RegionID,
                cohort = study.Cohort,
                languages = study.Languages
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        /// <summary>
        /// Updates Study info for specified study object.
        /// </summary>
        /// <param name="wave"></param>
        /// <returns></returns>
        public static int UpdateWave(StudyWave wave)
        {
            string query = "proc_updateWave";
            var parameters = new
            {
                id = wave.ID,
                studyID = wave.StudyID,
                waveNum = wave.Wave,
                countries = wave.Countries,
                englishRouting = wave.EnglishRouting
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
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
            string query = "proc_updateQnum";
            var parameters = new
            {
                newqnum = sq.Qnum,
                qid = sq.ID
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
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
            string query = "UPDATE tblSurveyNumbers SET AltQnum=@altqnum, AltQnum2 = @altqnum2, AltQnum3 =@altqnum3 WHERE ID = @id";
            var parameters = new
            {
                altqnum = sq.AltQnum,
                altqnum2 = sq.AltQnum2,
                altqnum3 = sq.AltQnum3,
                id = sq.ID
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Saves User Preferences for specified user.
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static int UpdateUser(UserPrefs u)
        {
            string query = "proc_updateUser";
            var parameters = new
            {
                userid = u.userid,
                accessLevel = u.accessLevel,
                reportPath = u.ReportPath,
                reportPrompt = u.reportPrompt,
                wordingNumbers = u.wordingNumbers
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }
        
        /// <summary>
        /// Updates a record in the survey checks table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateSurveyCheckRecord(SurveyCheckRec record)
        {
            string query = "proc_updateSurveyCheckRecord";
            var parameters = new
            {
                ID = record.ID,
                checkDate = record.CheckDate,
                checkInit = record.Name.ID,
                comments = record.Comments,
                survID = record.SurveyCode.SID,
                checkType = record.CheckType.ID
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;                    
        }

        /// <summary>
        /// Updates a record in the survey check reference survey table
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateSurveyCheckRefRecord(SurveyCheckRefSurvey record)
        {
            string query = "proc_updateSurveyCheckRef";
            var parameters = new
            {
                ID = record.ID,
                checkID = record.CheckID,
                survID = record.SID,
                survDate = record.SurveyDate
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        /// <summary>
        /// Updates a label in the appropriate label table.
        /// </summary>
        /// <param name="labelType"></param>
        /// <param name="labelText"></param>
        /// <param name="labelID"></param>
        /// <returns></returns>
        public static int UpdateLabel(string labelType, string labelText, int labelID)
        {
            string query = "proc_updateLabel";
            var parameters = new
            {
                type = labelType,
                label = labelText,
                id = labelID
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
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
            string query = "proc_updateTranslation";
            var parameters = new
            {
                id = translation.ID,
                lang = translation.LanguageName.LanguageName,
                langID = translation.LanguageName.ID,
                text = translation.TranslationText,
                bilingual = translation.Bilingual
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
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
            string query = "proc_updateLanguage";
            var parameters = new
            {
                id = language.ID,
                language = language.LanguageName,
                abbrev = language.Abbrev,
                isoabbrev = language.ISOAbbrev,
                nonLatin = language.NonLatin,
                font = language.PreferredFont
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        /// <summary>
        /// Updates the specified praccing record.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdatePraccingIssue(PraccingIssue record)
        {
            string query = "proc_updatePraccIssue";
            var parameters = new
            {
                id = record.ID,
                survID = record.Survey.SID,
                issueNo = record.IssueNo,
                varnames = record.VarNames,
                date = record.IssueDate,
                from = record.IssueFrom.ID,
                to = record.IssueTo.ID,
                description = record.Description,
                category = record.Category.ID,
                resolved = record.Resolved,
                resolvedby = record.ResolvedBy.ID,
                resolvedon = record.ResolvedDate,
                language = record.Language,
                pin = record.PinNo
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        /// <summary>
        /// Updates the specified praccing response record.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdatePraccingResponse(PraccingResponse record)
        {
            string query = "proc_updatePraccResponse";
            var parameters = new
            {
                id = record.ID,
                issueID = record.IssueID,
                date = record.ResponseDate,
                from = record.ResponseFrom.ID,
                to = record.ResponseTo.ID,
                description = record.Response,
                pin = record.PinNo
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the survey processing dates table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateSurveyProcessingDate(SurveyProcessingDate record)
        {
            string query = "UPDATE tblSurveyProcessingDates Set StageDate = @stagedate, EntryDate = @entrydate, StageInit = @stageinit, StageContact = @contact " +
                "WHERE ID = @ID";
            var parameters = new
            {
                ID = record.ID,
                stagedate = record.StageDate,
                entrydate = record.EntryDate,
                stageinit = record.EnteredBy.ID,
                contact = record.Contact.ID
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the survey processing notes table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateSurveyProcessingNote(SurveyProcessingNote record)
        {
            string query = "UPDATE tblSurveyProcessingNotes SET EnteredBy = @enteredby, CommentDate = @commentdate, Note = @note " +
                "WHERE ID = @ID";
            var parameters = new
            {
                ID = record.ID,
                enteredby = record.Author.ID,
                commentdate = record.NoteDate,
                note = record.Note
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the survey processing stage table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateSurveyProcessingStage(SurveyProcessingRecord record)
        {
            string query = "UPDATE tblSurveyProcessing SET Stage = @stage, NA = @na, Done = @done WHERE ID = @ID";
            var parameters = new
            {
                ID = record.ID,
                stage = record.Stage.ID,
                na = record.NotApplicable,
                done = record.Done
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the notes table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateNote(Note record)
        {
            string query = "UPDATE tblNotes SET Notes = @notes WHERE ID = @ID";
            var parameters = new
            {
                ID = record.ID,
                notes = record.NoteText
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the question comment table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateQuestionComment(QuestionComment record)
        {
            string query = "proc_updateQuestionComment";
            var parameters = new
            {
                survey = record.Survey,
                varname = record.VarName,
                commentText = record.Notes.NoteText,
                notedate = record.NoteDate.Value,
                noteinit = record.Author.ID,
                authority = record.Authority.ID,
                notetype = record.NoteType.ID,
                source = record.Source
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the survey comment table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateSurveyComment(SurveyComment record)
        {
            string query = "proc_updateSurveyComment";
            var parameters = new
            {
                survey = record.Survey,
                commentText = record.Notes.NoteText,
                notedate = record.NoteDate.Value,
                noteinit = record.Author.ID,
                authority = record.Authority.ID,
                notetype = record.NoteType.ID,
                source = record.Source
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the wave comment table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateWaveComment(WaveComment record)
        {
            string query = "proc_updateWaveComment";
            var parameters = new
            {
                wave = record.StudyWave,
                commentText = record.Notes.NoteText,
                notedate = record.NoteDate.Value,
                noteinit = record.Author.ID,
                authority = record.Authority.ID,
                notetype = record.NoteType.ID,
                source = record.Source
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the refvar comment table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateRefVarComment(RefVarComment record)
        {
            string query = "proc_updateRefVarComment";
            var parameters = new
            {
                varname = record.RefVarName,
                commentText = record.Notes.NoteText,
                notedate = record.NoteDate.Value,
                noteinit = record.Author.ID,
                authority = record.Authority.ID,
                notetype = record.NoteType.ID,
                source = record.Source
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the deleted question comments table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateDeletedQuestionComment(DeletedComment record)
        {
            string query = "proc_updateDeletedComment";
            var parameters = new
            {
                survey = record.Survey,
                varname = record.VarName,
                commentText = record.Notes.NoteText,
                notedate = record.NoteDate.Value,
                noteinit = record.Author.ID,
                authority = record.Authority.ID,
                notetype = record.NoteType.ID,
                source = record.Source
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        /// <summary>
        /// Updates a user's stored comment.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="record"></param>
        /// <param name="recordNumber"></param>
        /// <returns></returns>
        public static int UpdateStoredComment(int userID, Comment record, int recordNumber)
        {
            string query = "UPDATE tblSavedComments SET NoteDate = @notedate, NoteInit = @noteinit, NoteTypeID = @notetypeid, Comment = @comment, " +
                "Source = @source, AuthorityID = @authority WHERE PersonnelID = @userID AND ID = @id";
            var parameters = new
            {
                notedate = record.NoteDate.Value,
                noteinit = record.Author.ID,
                notetypeid = record.NoteType.ID,
                authority = record.Authority.ID,
                comment = record.Notes.NoteText,
                source = record.Source,
                userID = userID,
                id = recordNumber
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates a user's saved sources.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="source"></param>
        /// <param name="recordNumber"></param>
        /// <returns></returns>
        public static int UpdateSavedSource(int userID, string source, int recordNumber)
        {
            string query = "UPDATE tblSavedSources SET SourceText=@source WHERE PersonnelID = @userID AND SourceNumber = @id";
            var parameters = new
            {
                source = source,
                userID = userID,
                id = recordNumber
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates VarName field for a survey in the survey questions table.
        /// </summary>
        /// <param name="oldname"></param>
        /// <param name="newname"></param>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static int RenameVariableName(string oldname, string newname, string survey)
        {
            string query = "proc_renameVariable";
            var parameters = new
            {
                oldname = oldname,
                newname = newname,
                survey = survey
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        /// <summary>
        /// Updates or inserts a new PreP record.
        /// </summary>
        /// <param name="wording"></param>
        /// <param name="oldval"></param>
        /// <param name="newval"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static int UpdatePreP(Wording wording, string oldval, string newval, bool overwrite)
        {
            string query = "proc_updatePreP";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@prepid", wording.WordID);
            parameters.Add("@oldval", oldval);
            parameters.Add("@newval", newval);
            parameters.Add("@overwrite", overwrite);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
                wording.WordID = parameters.Get<int>("@newID");
            }

            return 0;
        }

        /// <summary>
        /// Updates or inserts a new PreI record.
        /// </summary>
        /// <param name="wording"></param>
        /// <param name="oldval"></param>
        /// <param name="newval"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static int UpdatePreI(Wording wording, string oldval, string newval, bool overwrite)
        {
            string query = "proc_updatePreI";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@preiid", wording.WordID);
            parameters.Add("@oldval", oldval);
            parameters.Add("@newval", newval);
            parameters.Add("@overwrite", overwrite);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
                wording.WordID = parameters.Get<int>("@newID");
            }

            return 0;
        }

        /// <summary>
        /// Updates or inserts a new PreA record.
        /// </summary>
        /// <param name="wording"></param>
        /// <param name="oldval"></param>
        /// <param name="newval"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static int UpdatePreA(Wording wording, string oldval, string newval, bool overwrite)
        {
            string query = "proc_updatePreA";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@preaid", wording.WordID);
            parameters.Add("@oldval", oldval);
            parameters.Add("@newval", newval);
            parameters.Add("@overwrite", overwrite);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
                wording.WordID = parameters.Get<int>("@newID");
            }

            return 0;
        }

        /// <summary>
        /// Updates or inserts a new LitQ record.
        /// </summary>
        /// <param name="wording"></param>
        /// <param name="oldval"></param>
        /// <param name="newval"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static int UpdateLitQ(Wording wording, string oldval, string newval, bool overwrite)
        {
            string query = "proc_updateLitQ";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@litqid", wording.WordID);
            parameters.Add("@oldval", oldval);
            parameters.Add("@newval", newval);
            parameters.Add("@overwrite", overwrite);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
                wording.WordID = parameters.Get<int>("@newID");
            }

            return 0;
        }

        /// <summary>
        /// Updates or inserts a new PstI record.
        /// </summary>
        /// <param name="wording"></param>
        /// <param name="oldval"></param>
        /// <param name="newval"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static int UpdatePstI(Wording wording, string oldval, string newval, bool overwrite)
        {
            string query = "proc_updatePstI";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@pstiid", wording.WordID);
            parameters.Add("@oldval", oldval);
            parameters.Add("@newval", newval);
            parameters.Add("@overwrite", overwrite);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
                wording.WordID = parameters.Get<int>("@newID");
            }

            return 0;
        }

        /// <summary>
        /// Updates or inserts a new PstP record.
        /// </summary>
        /// <param name="wording"></param>
        /// <param name="oldval"></param>
        /// <param name="newval"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static int UpdatePstP(Wording wording, string oldval, string newval, bool overwrite)
        {
            string query = "proc_updatePstP";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@pstpid", wording.WordID);
            parameters.Add("@oldval", oldval);
            parameters.Add("@newval", newval);
            parameters.Add("@overwrite", overwrite);
            parameters.Add("@newID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
                wording.WordID = parameters.Get<int>("@newID");
            }

            return 0;
        }


        /// <summary>
        /// Updates all questions in the survey using the old PreP# to the new PreP#
        /// </summary>
        /// <param name="wording"></param>
        /// <param name="oldval"></param>
        /// <param name="newval"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static int UpdateSurveyPreP(string survey, int oldID, int newID)
        {
            string query = "UPDATE tblSurveyNumbers Set PreP#=@newid WHERE Survey = @survey AND PreP#=@oldid;";

            var parameters = new
            {
                newid = newID,
                survey = survey,
                oldid = oldID,
            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates all questions in the survey using the old PreI# to the new PreI#
        /// </summary>
        /// <param name="wording"></param>
        /// <param name="oldval"></param>
        /// <param name="newval"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static int UpdateSurveyPreI(string survey, int oldID, int newID)
        {
            string query = "UPDATE tblSurveyNumbers Set PreI#=@newid WHERE Survey = @survey AND PreI#=@oldid;";

            var parameters = new
            {
                newid = newID,
                survey = survey,
                oldid = oldID,
            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates all questions in the survey using the old PreA# to the new PreA#
        /// </summary>
        /// <param name="wording"></param>
        /// <param name="oldval"></param>
        /// <param name="newval"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static int UpdateSurveyPreA(string survey, int oldID, int newID)
        {
            string query = "UPDATE tblSurveyNumbers Set PreA#=@newid WHERE Survey = @survey AND PreA#=@oldid;";

            var parameters = new
            {
                newid = newID,
                survey = survey,
                oldid = oldID,
            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates all questions in the survey using the old LitQ# to the new LitQ#
        /// </summary>
        /// <param name="wording"></param>
        /// <param name="oldval"></param>
        /// <param name="newval"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static int UpdateSurveyLitQ(string survey, int oldID, int newID)
        {
            string query = "UPDATE tblSurveyNumbers Set LitQ#=@newid WHERE Survey = @survey AND LitQ#=@oldid;";

            var parameters = new
            {
                newid = newID,
                survey = survey,
                oldid = oldID,
            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates all questions in the survey using the old PstI# to the new PstI#
        /// </summary>
        /// <param name="wording"></param>
        /// <param name="oldval"></param>
        /// <param name="newval"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static int UpdateSurveyPstI(string survey, int oldID, int newID)
        {
            string query = "UPDATE tblSurveyNumbers Set PstI#=@newid WHERE Survey = @survey AND PstI#=@oldid;";

            var parameters = new
            {
                newid = newID,
                survey = survey,
                oldid = oldID,
            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates all questions in the survey using the old PstP# to the new PstP#
        /// </summary>
        /// <param name="wording"></param>
        /// <param name="oldval"></param>
        /// <param name="newval"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static int UpdateSurveyPstP(string survey, int oldID, int newID)
        {
            string query = "UPDATE tblSurveyNumbers Set PstP#=@newid WHERE Survey = @survey AND PstP#=@oldid;";

            var parameters = new
            {
                newid = newID,
                survey = survey,
                oldid = oldID,
            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;          
        }

        /// <summary>
        /// Updates a record in the varname changes table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateVarNameChange(VarNameChange record)
        {
            string query = "UPDATE tblVarNameChanges Set[refVarNameNew] = @refVarNameNew, [NewName] = @NewName, @refVarNameOld = @refVarNameOld, OldName = @OldName, " +
                    "[ChangeDate]=@changeDate,[ChangedBy]=@changedby, [Authorization]=@authorization,[Reasoning]=@reasoning,[Source2]=@source,[TempVar]=@tempvar " +
                    "WHERE ID = @changeID;";

            var parameters = new {
                changeID = record.ID,
                refVarNameNew = record.NewRefName,
                NewName = record.NewName,
                refVarNameOld = record.OldRefName,
                OldName = record.OldName,
                changeDate = record.ChangeDate,
                changedby = record.ChangedBy.ID,
                authorization = record.Authorization,
                reasoning = record.Rationale,
                source = record.Source,
                tempvar = record.HiddenChange
            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the personnel table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdatePersonnel(Person record)
        {
            string query = "proc_updatePersonnel";

            var parameters = new
            {
                ID = record.ID,
                firstname = record.FirstName,
                lastname = record.LastName,
                username = record.Username,
                email = record.Email,
                workphone = record.WorkPhone,
                homephone = record.HomePhone,
                officeno = record.OfficeNo,
                institution = record.Institution,
                active = record.Active,
                entry = record.Entry,
                praccentry = record.PraccEntry,
                varnamechangenotify = record.VarNameChangeNotify,
                praccid = record.PraccID
            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the personnel comment table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdatePersonnelComment(PersonnelComment record)
        {
            string query = "UPDATE tblPersonnelComments SET CommentType = @commentType, Comment = @comment WHERE ID = @ID";

            var parameters = new
            {
                ID = record.ID,
                commentType = record.CommentType,
                comment = record.Comment
            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the cohort table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateCohort(SurveyCohort record)
        {
            string query = "UPDATE tblCohort SET Cohort = @cohort, Code = @code, WebName =@webname WHERE ID = @ID";

            var parameters = new
            {
                ID = record.ID,
                cohort = record.Cohort,
                code = record.Code,
                webname = record.WebName
            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the user states table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateUserState(UserState record)
        {
            string query = "UPDATE tblUserStates SET UserState = @userstate WHERE ID = @ID";

            var parameters = new
            {
                ID = record.ID,
                userstate = record.UserStateName,
            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the alternate spelling table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateSimilarWords(SimilarWords record)
        {
            string query = "UPDATE tblAlternateSpelling SET Word = @words WHERE ID = @ID";

            var parameters = new
            {
                ID = record.ID,
                words = record.WordList,
            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the canonical varnames table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateCanonVar(CanonicalRefVarName record)
        {
            string query = "UPDATE tblCanonVars SET VarName = @refvarname, AnySuffix=@anysuffix, Notes=@notes, Active= @active WHERE ID = @ID;";

            var parameters = new
            {
                ID = record.ID,
                refvarname = record.RefVarName,
                anysuffix = record.AnySuffix,
                notes = record.Notes,
                active = record.Active
            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the prefix table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdatePrefix(VariablePrefix record)
        {
            string sql = "UPDATE tblDomainList SET prefix = @prefix, PrefixName=@prefixName, ProductType=@productType, RelatedPrefixes= @relatedPrefixes,DomainName=@domainName, " +
                "Comments=@comments, InactiveDomain=@inactive WHERE ID = @ID";

            var parameters = new
            {
                ID = record.ID,
                prefix = record.Prefix,
                prefixName = record.PrefixName,
                productType = record.ProductType,
                relatedPrefixes = record.RelatedPrefixes,
                domainName = record.Description,
                comments = record.Comments,
                inactive = record.Inactive
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.Text);
            }
            return 0;

        }

        /// <summary>
        /// Updates a record in the prefix range table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdatePrefixRange(VariableRange record)
        {
            string sql = "proc_updatePrefixRange";

            var parameters = new
            {
                ID = record.ID,
                low = record.Lower,
                high = record.Upper,
                description = record.Description,
              
            };
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            }
            return 0;

        }

        /// <summary>
        /// Updates a record in the survey drafts table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateSurveyDraft(SurveyDraft record)
        {
            string query = "UPDATE qrySurveyDraftInfo SET SurvID=@survID, DraftTitle=@title, DraftDate=@date, DraftComments=@comments, Investigator=@investigator WHERE ID = @ID;";

            var parameters = new
            {
                ID = record.ID,
                title = record.DraftTitle,
                date = record.DraftDate,
                comments = record.DraftComments,
                investigator = record.Investigator,
                survID = record.SurvID
            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Updates a record in the survey drafts extra fields table.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static int UpdateSurveyDraftExtraField(SurveyDraftExtraField record)
        {
            string query = "UPDATE qrySurveyDraftExtraFields SET DraftID=@draftID, ExtraFieldNum=@fieldNum, ExtraFieldLabel=@fieldLabel WHERE ID = @ID;";

            var parameters = new
            {
                ID = record.ID,
                draftID = record.DraftID,
                fieldNum = record.FieldNumber,
                fieldLabel = record.Label

            };

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                rowsAffected = db.Execute(query, parameters, commandType: CommandType.Text);
            }

            return 0;
        }

        /// <summary>
        /// Saves a filter for the provided user.
        /// </summary>
        /// <param name="record"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static int UpdateFormFilter(FormState record, int userid)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@formCode", record.FormName);
            parameters.Add("@filter", record.Filter);
            parameters.Add("@formNum", record.FormNum);
            parameters.Add("@personnelID", userid);
            parameters.Add("@position", record.RecordPosition);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("proc_updateFormFilter", parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        /// <summary>
        /// Saves a filter for the provided user.
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="filter"></param>
        /// <param name="position"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int UpdateFormSurvey(FormState record, int userid)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@formCode", record.FormName);
            parameters.Add("@survID", record.FilterID);
            parameters.Add("@formNum", record.FormNum);
            parameters.Add("@personnelID", userid);
            parameters.Add("@position", record.RecordPosition);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("proc_updateFormSurvey", parameters, commandType: CommandType.StoredProcedure);
            }

            return 0;
        }

        public static int Update (string procedureName, List<KeyValuePair<string,object>> parameters)
        {
            int result;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                int rowsAffected = db.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
                if (rowsAffected > 0)
                    result = 0;
                else
                    result = 1;
            }
            return result;
        }

        public static int Update_SP(string procedureName, List<KeyValuePair<string, object>> parameters)
        {
            int result;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                int rowsAffected = db.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
                if (rowsAffected > 0)
                    result = 0;
                else
                    result = 1;
            }
            return result;
        }

        public static int Update_Query(string queryString, List<KeyValuePair<string, object>> parameters)
        {
            int result;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    int rowsAffected = db.Execute(queryString, parameters);
                    if (rowsAffected > 0)
                        result = 0;
                    else
                        result = 1;
                }
                catch
                {
                    result = 1;
                }
            }
            return result;
        }
    }


}
