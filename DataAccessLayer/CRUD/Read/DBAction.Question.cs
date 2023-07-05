using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;
using Dapper;
namespace ITCLib
{
    partial class DBAction
    {
        //
        // SurveyQuestions
        //

        /// <summary>
        /// Returns a list of SurveyQuestion objects matching the provided criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static List<SurveyQuestion> GetSurveyQuestions(SurveyQuestionCriteria criteria)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();

            string query = "SELECT ID, Survey AS SurveyCode, Qnum, PreP# AS PrePNum, PreP, PreI# AS PreINum, PreI, PreA# AS PreANum, PreA, LitQ# AS LitQNum, LitQ, " +
                    "PstI# AS PstINum, PstI, PstP# AS PstPNum, PstP, RespName, RespOptions, NRName, NRCodes, CorrectedFlag, ScriptOnly, AltQnum, AltQnum2, AltQnum3, FilterDescription, " +
                    "VarName, VarLabel, " +
                    "DomainNum, DomainNum AS ID, Domain AS LabelText, " +
                    "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                    "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                    "ProductNum, ProductNum AS ID, Product AS LabelText FROM qrySurveyQuestions";
                    
            string where = criteria.GetCriteria();

            if (string.IsNullOrEmpty(where))
                return qs;

            query += " WHERE " + where + " ORDER BY Survey";
            
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                qs = db.Query<SurveyQuestion, VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, SurveyQuestion>(query, (question, variable, domain, topic, content, product) =>
                {
                    variable.Domain = domain;
                    variable.Topic = topic;
                    variable.Content = content;
                    variable.Product = product;
                    question.VarName = variable;
                    return question;
                }, splitOn: "VarName, DomainNum, TopicNum, ContentNum, ProductNum").ToList();
            }
            return qs;
        }

        /// <summary>
        /// Returns a SurveyQuestion with the provided ID. 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>SurveyQuestion if ID is valid, null otherwise.</returns>
        public static SurveyQuestion GetSurveyQuestion(int ID)
        {
            SurveyQuestion question = new SurveyQuestion();

            string query = "SELECT ID, Survey AS SurveyCode, Qnum, PreP# AS PrePNum, PreP, PreI# AS PreINum, PreI, PreA# AS PreANum, PreA, LitQ# AS LitQNum, LitQ, " +
                    "PstI# AS PstINum, PstI, PstP# AS PstPNum, PstP, RespName, RespOptions, NRName, NRCodes, CorrectedFlag, ScriptOnly, AltQnum, AltQnum2, AltQnum3, FilterDescription, " +
                    "VarName, VarLabel, " +
                    "DomainNum, DomainNum AS ID, Domain AS LabelText, " +
                    "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                    "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                    "ProductNum, ProductNum AS ID, Product AS LabelText FROM Questions.FN_GetSurveyQuestion(@id)";

            var parameters = new { id = ID };
            
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                question = db.Query<SurveyQuestion, VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, SurveyQuestion>(query, (q, variable, domain, topic, content, product) =>
                {
                    variable.Domain = domain;
                    variable.Topic = topic;
                    variable.Content = content;
                    variable.Product = product;
                    q.VarName = variable;
                    return q;
                }, parameters, splitOn: "VarName, DomainNum, TopicNum, ContentNum, ProductNum").FirstOrDefault();
            }
            return question;
        }

        /// <summary>
        /// Retrieves a set of records for a particular survey ID and returns a list of SurveyQuestion objects. 
        /// </summary>
        /// <param name="Survey">Survey object</param>
        /// <returns>List of SurveyQuestions</returns>
        public static List<SurveyQuestion> GetSurveyQuestions(Survey s)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();

            string sql = "SELECT ID, Survey AS SurveyCode, Qnum, PreP# AS PrePNum, PreP, PreI# AS PreINum, PreI, PreA# AS PreANum, PreA, LitQ# AS LitQNum, LitQ, " +
                    "PstI# AS PstINum, PstI, PstP# AS PstPNum, RespName, RespOptions, NRName, NRCodes, TableFormat, CorrectedFlag, ScriptOnly, AltQnum, AltQnum2, AltQnum3, FilterDescription, " +
                    "VarName, VarLabel, " +
                    "DomainNum, DomainNum AS ID, Domain AS LabelText, " +
                    "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                    "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                    "ProductNum, ProductNum AS ID, Product AS LabelText " +
                    "FROM Questions.FN_GetSurveyQuestions(@SID) ORDER BY Qnum;" +
                    "SELECT TF.ID, QID, TimeFrame FROM tblQuestionTimeFrames AS TF LEFT JOIN qrySurveyQuestions AS SQ ON TF.QID = SQ.ID WHERE SQ.SurvID = @SID;";

            var parameters = new { s.SID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var results = db.QueryMultiple(sql, parameters);
                qs = results.Read<SurveyQuestion, VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, SurveyQuestion>(
                    (question, varname, domain, topic, content, product) =>
                    {
                        varname.Domain = domain;
                        varname.Topic = topic;
                        varname.Content = content;
                        varname.Product = product;
                        question.VarName = varname;
                        return question;
                    }, splitOn: "VarName, DomainNum, TopicNum, ContentNum, ProductNum").ToList();

                var timeframes = results.Read<QuestionTimeFrame>().ToList();

                // add time frames to questions
                foreach (SurveyQuestion qr in qs)
                {
                    qr.TimeFrames = timeframes.Where(x => x.QID == qr.ID).ToList();
                }
            }
            return qs;
        }

        /// <summary>
        /// Retrieves a set of records for a particular survey ID and returns a list of SurveyQuestion objects. 
        /// </summary>
        /// <param name="Survey">Survey object</param>
        /// <returns>List of SurveyQuestions</returns>
        public static List<SurveyQuestion> GetSurveyQuestionRecords(Survey s)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();

            string sql = "SELECT ID, Survey AS SurveyCode, Qnum, PreP# AS PrePNum, PreP, PreI# AS PreINum, PreI, PreA# AS PreANum, PreA, LitQ# AS LitQNum, LitQ, " +
                    "PstI# AS PstINum, PstI, PstP# AS PstPNum, RespName, RespOptions, NRName, NRCodes, TableFormat, CorrectedFlag, ScriptOnly, AltQnum, AltQnum2, AltQnum3, " +
                    "VarName, VarLabel, " +
                    "DomainNum, DomainNum AS ID, Domain AS LabelText, "+
                    "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                    "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                    "ProductNum, ProductNum AS ID, Product AS LabelText " +
                    "FROM Questions.FN_GetSurveyQuestions(@SID) ORDER BY Qnum";

            var parameters = new { SID = s.SID };

            using (IDbConnection db = new SqlConnection(connectionString))
            {              
                var results = db.Query<SurveyQuestion, VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, SurveyQuestion>(sql,
                    (question, varname, domain, topic, content, product) =>
                {
                    varname.Domain = domain;
                    varname.Topic = topic;
                    varname.Content = content;
                    varname.Product = product;
                    question.VarName = varname;
                    return question;
                }, parameters, splitOn: "VarName, DomainNum, TopicNum, ContentNum, ProductNum").ToList();

                qs = new List<SurveyQuestion>(results);
            }
            return qs;
        }

        /// <summary>
        /// Retrieves a set of records for a particular survey ID and returns a list of SurveyQuestion objects. 
        /// </summary>
        /// <param name="Survey">Survey object</param>
        /// <returns>List of SurveyQuestions</returns>
        public static List<SurveyQuestion> GetCompleteSurvey(Survey s)
        {
            List<SurveyQuestion> questions = new List<SurveyQuestion>();

            string sql = "SELECT ID, Survey AS SurveyCode, Qnum, PreP# AS PrePNum, PreP, PreI# AS PreINum, PreI, PreA# AS PreANum, PreA, LitQ# AS LitQNum, LitQ, " +
                    "PstI# AS PstINum, PstI, PstP# AS PstPNum, PstP, RespName, RespOptions, NRName, NRCodes, TableFormat, CorrectedFlag, ScriptOnly, AltQnum, AltQnum2, AltQnum3, " +
                    "FilterDescription, " +
                    "VarName, VarLabel, " +
                    "DomainNum, DomainNum AS ID, Domain AS LabelText, " +
                    "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                    "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                    "ProductNum, ProductNum AS ID, Product AS LabelText " +
                    "FROM Questions.FN_GetSurveyQuestions(@SID) ORDER BY Qnum;" +
                    "SELECT T.ID, QID, Q.Survey, Q.VarName, Lang AS Language, Translation AS TranslationText, Bilingual, " +
                        "LanguageID, LanguageID AS ID, Lang AS LanguageName, Abbrev, ISOAbbrev, NonLatin, PreferredFont, RTL " +
                        "FROM qryTranslation AS T LEFT JOIN qrySurveyQuestions AS Q ON T.QID = Q.ID WHERE SurvID=@SID;" +
                    "SELECT ID, QID, SurvID, Survey, VarName, NoteDate, SourceName, Source, " +
                        "CID, CID AS ID, Notes AS NoteText, "+
                        "NoteInit, NoteInit AS ID, Name, " +
                        "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, ShortForm " +
                        "FROM qryCommentsQues WHERE SurvID = @SID;" +
                    "SELECT ID, QID, TimeFrame FROM qryQuestionTimeFrames WHERE SurvID = @SID;";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var parameters = new { s.SID };

                var results = db.QueryMultiple(sql, parameters);

                // questions
                questions = results.Read<SurveyQuestion, VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, SurveyQuestion>(
                    (question, varname, domain, topic, content, product) =>
                    {
                        varname.Domain = domain;
                        varname.Topic = topic;
                        varname.Content = content;
                        varname.Product = product;
                        question.VarName = varname;
                        return question;
                    }, splitOn: "VarName, DomainNum, TopicNum, ContentNum, ProductNum").ToList();

                // translations
                var translations = results.Read<Translation, Language, Translation>((translation, language) =>
                {
                    translation.LanguageName = language;
                    return translation;
                }, splitOn: "LanguageID").ToList();

                // comments
                var comments = results.Read<QuestionComment, Note, Person, CommentType, QuestionComment>(
                    (comment, note, author, type) =>
                    {
                        comment.Notes = note;
                        comment.Author = author;
                        comment.NoteType = type;
                        return comment;
                    }, splitOn: "CID, NoteInit, NoteTypeID").ToList();

                // time frames
                var timeframes = results.Read<QuestionTimeFrame>().ToList();

                // add translations, comments, and time frames to questions
                foreach(SurveyQuestion qr in questions)
                {
                    qr.Translations = translations.Where(x => x.QID == qr.ID).ToList();
                    qr.Comments = comments.Where(x => x.QID == qr.ID).ToList();
                    qr.TimeFrames = timeframes.Where(x => x.QID == qr.ID).ToList();
                }
            }

            return questions;
        }

        /// <summary>
        /// Retrieves a set of records for a particular VarName and returns a list of SurveyQuestion objects. 
        /// </summary>
        /// <param name="varname">A valid VarName.</param>
        /// <returns>List of SurveyQuestions</returns>
        public static List<SurveyQuestion> GetVarNameQuestions(string varname)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();

            string sql = "SELECT ID, Survey AS SurveyCode, Qnum, PreP# AS PrePNum, PreP, PreI# AS PreINum, PreI, PreA# AS PreANum, PreA, LitQ# AS LitQNum, LitQ, " +
                    "PstI# AS PstINum, PstI, PstP# AS PstPNum, RespName, RespOptions, NRName, NRCodes, TableFormat, CorrectedFlag, ScriptOnly, AltQnum, AltQnum2, AltQnum3, " +
                    "VarName, VarLabel, " +
                    "DomainNum, DomainNum AS ID, Domain AS LabelText, " +
                    "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                    "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                    "ProductNum, ProductNum AS ID, Product AS LabelText " +
                    "FROM Questions.FN_GetVarNameQuestions(@varname) ORDER BY Qnum";

            var parameters = new { varname };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                qs = db.Query<SurveyQuestion, VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, SurveyQuestion>(sql,
                    (question, varName, domain, topic, content, product) =>
                    {
                        varName.Domain = domain;
                        varName.Topic = topic;
                        varName.Content = content;
                        varName.Product = product;
                        question.VarName = varName;
                        return question;
                    }, parameters, splitOn: "VarName, DomainNum, TopicNum, ContentNum, ProductNum").ToList();
            }
            return qs;
        }

        /// <summary>
        /// Retrieves a set of records for a particular VarName and returns a list of QuestionUsage objects. 
        /// </summary>
        /// <param name="varname">A valid VarName.</param>
        /// <returns>List of QuestionUsage</returns>
        public static List<QuestionUsage> GetVarNameQuestions(VariableName varname)
        {
            List<QuestionUsage> qs = new List<QuestionUsage>();

            string query = "SELECT PreP, PreI, PreA, LitQ, PstI, PstP, RespOptions, NRCodes, " + 
                            "STUFF((SELECT  ',' + Survey " +
                                "FROM qrySurveyQuestions SQ2 " +
                                "WHERE VarName = sq1.VarName AND PreP = sq1.PreP AND PreI = sq1.PreI AND PreA = sq1.PreA AND LitQ=sq1.LitQ AND " +
                                "PstI = sq1.PstI AND PstP = sq1.PstP AND Respoptions = sq1.RespOptions AND NRCodes = sq1.NRCodes " +
                                "GROUP BY SQ2.Survey " +
                                "ORDER BY Survey " +
                                "FOR XML PATH(''), TYPE).value('text()[1]', 'nvarchar(max)') ,1, LEN(','), '') AS SurveyList, " +
                            "VarName, refVarName " +
                            "FROM qrySurveyQuestions Sq1 " +
                            "WHERE VarName = @varname " +
                            "GROUP BY sq1.refVarName, VarName, PreP, PreI, PreA, LitQ, PstI, PstP, RespOptions, NRCodes " +
                            "ORDER BY refVarName";

            var parameters = new { varname = varname.VarName };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                qs = db.Query<QuestionUsage, VariableName, QuestionUsage>(query, (question, varName) =>
                {
                    question.VarName = varName;
                    return question;
                }, parameters, splitOn: "VarName").ToList();
            }
            return qs;
        }

        /// <summary>
        /// Retrieves a set of records for a particular refVarName and returns a list of SurveyQuestion objects. 
        /// </summary>
        /// <param name="refvarname">A valid refVarName.</param>
        /// <returns>List of SurveyQuestions</returns>
        public static List<SurveyQuestion> GetRefVarNameQuestions(string refVarName)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();

            string sql = "SELECT ID, Survey AS SurveyCode, Qnum, PreP# AS PrePNum, PreP, PreI# AS PreINum, PreI, PreA# AS PreANum, PreA, LitQ# AS LitQNum, LitQ, " +
                   "PstI# AS PstINum, PstI, PstP# AS PstPNum, RespName, RespOptions, NRName, NRCodes, TableFormat, CorrectedFlag, ScriptOnly, AltQnum, AltQnum2, AltQnum3, " +
                   "VarName, VarLabel, " +
                   "DomainNum, DomainNum AS ID, Domain AS LabelText, " +
                   "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                   "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                   "ProductNum, ProductNum AS ID, Product AS LabelText " +
                   "FROM Questions.FN_GetRefVarNameQuestions(@refVarName) ORDER BY Qnum";

            var parameters = new { refVarName };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                qs = db.Query<SurveyQuestion, VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, SurveyQuestion>(sql,
                    (question, varName, domain, topic, content, product) =>
                    {
                        varName.Domain = domain;
                        varName.Topic = topic;
                        varName.Content = content;
                        varName.Product = product;
                        question.VarName = varName;
                        return question;
                    }, parameters, splitOn: "VarName, DomainNum, TopicNum, ContentNum, ProductNum").ToList();
            }
            return qs;
        }

        /// <summary>
        /// Retrieves a set of records for a particular VarName and returns a list of SurveyQuestion objects. 
        /// </summary>
        /// <param name="refvarname">A valid VarName.</param>
        /// <param name="surveyPattern">Survey code pattern.</param>
        /// <returns>List of SurveyQuestions</returns>
        public static List<SurveyQuestion> GetRefVarNameQuestionsGlob(string refvarname, string surveyPattern = "%")
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();

            string sql = "SELECT ID, Survey AS SurveyCode, Qnum, PreP# AS PrePNum, PreP, PreI# AS PreINum, PreI, PreA# AS PreANum, PreA, LitQ# AS LitQNum, LitQ, " +
                    "PstI# AS PstINum, PstI, PstP# AS PstPNum, RespName, RespOptions, NRName, NRCodes, TableFormat, CorrectedFlag, ScriptOnly, AltQnum, AltQnum2, AltQnum3, " +
                    "VarName, VarLabel, " +
                    "DomainNum, DomainNum AS ID, Domain AS LabelText, " +
                    "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                    "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                    "ProductNum, ProductNum AS ID, Product AS LabelText " +
                    "FROM Questions.FN_GetRefVarNameQuestionsGlob(@refvarname, @surveyPattern) ORDER BY Qnum";

            var parameters = new { refvarname, surveyPattern };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                qs = db.Query<SurveyQuestion, VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, SurveyQuestion>(sql,
                    (question, varName, domain, topic, content, product) =>
                    {
                        varName.Domain = domain;
                        varName.Topic = topic;
                        varName.Content = content;
                        varName.Product = product;
                        question.VarName = varName;
                        return question;
                    }, parameters, splitOn: "VarName, DomainNum, TopicNum, ContentNum, ProductNum").ToList();
            }
            return qs;
        }

        /// <summary>
        /// Returns the list of corrected questions for a specified survey.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<SurveyQuestion> GetCorrectedWordings(Survey s)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();

            string sql = "SELECT ID, Survey AS SurveyCode, Qnum, PreP# AS PrePNum, PreP, PreI# AS PreINum, PreI, PreA# AS PreANum, PreA, LitQ# AS LitQNum, LitQ, " +
                    "PstI# AS PstINum, PstI, PstP# AS PstPNum, RespName, RespOptions, NRName, NRCodes " +
                    "FROM Questions.FN_GetCorrectedQuestions(@survey)";

            var parameters = new { survey = s.SurveyCode };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
               qs = db.Query<SurveyQuestion>(sql, parameters).ToList();
            }
            return qs;
        }

        /// <summary>
        /// Returns the ID of the question matching the survey and varname parameters.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="varname"></param>
        /// <returns></returns>
        public static int GetQuestionID(string survey, string varname)
        {
            int qid = 0;
            string query = "SELECT ID FROM qrySurveyQuestions WHERE Survey =@survey AND Varname=@varname";

            var parameters = new { survey, varname };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var results = db.ExecuteScalar(query, parameters);
                if (results != null)
                    qid = (int)results;
            }
            return qid;
        }

        /// <summary>
        /// Returns the ID of the question matching the survey and refvarname parameters.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="refvarname"></param>
        /// <returns></returns>
        public static int GetQuestionIDRef(string survey, string refvarname)
        {
            int qid = 0;
            string query = "SELECT ID FROM qrySurveyQuestions WHERE Survey = @survey AND refVarname= @refvarname";

            var parameters = new { survey, refvarname };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var results = db.ExecuteScalar(query, parameters);
                if (results != null)
                    qid = (int)results;
            }
            return qid;
        }

        /// <summary>
        /// Gets the list of DeleteQuestion objects and comments for the specified survey.
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static List<DeletedQuestion> GetDeletedQuestions (string survey)
        {
            List<DeletedQuestion> list = new List<DeletedQuestion>();

            string query = "SELECT ID, Survey AS SurveyCode, VarName, VarLabel, ContentLabel, TopicLabel, DomainLabel, DeleteDate, DeletedBy FROM qryDeletedSurveyVars WHERE Survey = @survey;" +
                "SELECT ID, Survey, VarName, NoteDate, SourceName, Source, " +
                    "CID, CID AS ID, Notes AS NoteText, " +
                    "NoteInit, NoteInit AS ID, Name, "+
                    "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName " +
                    "FROM qryNotesByDeletedVar WHERE Survey = @survey;";

            var parameters = new { survey };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var results = db.QueryMultiple(query, parameters);

                list = results.Read<DeletedQuestion>().ToList();

                var comments = results.Read<DeletedComment, Note, Person, CommentType, DeletedComment>(
                    (comment, note, author, type) =>
                    {
                        comment.Notes = note;
                        comment.Author = author;
                        comment.NoteType = type;
                        return comment;
                    }, splitOn: "CID, NoteInit, NoteTypeID").ToList();

                foreach  (DeletedQuestion dq in list)
                {
                    dq.DeleteNotes = comments.Where(x => x.VarName.Equals(dq.VarName) && x.Survey.Equals(dq.SurveyCode)).ToList();
                }
            }
            return list;
        }

        /// <summary>
        /// Returns a list of heading questions for the specified survey. TODO make this part of the Survey object.
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static List<Heading> GetHeadingQuestions(Survey survey)
        {
            List<Heading> list = new List<Heading>();
            
            List<SurveyQuestion> questions = GetSurveyQuestions(survey).ToList();

            bool inSection = false;
            bool firstDone = false;
            Heading currentSection = null; 

            for (int i = 0; i < questions.Count; i++)
            {
                if (questions[i].VarName.VarName.StartsWith("Z") && !questions[i].VarName.VarName.EndsWith("s"))
                {
                    if (i > 0 && currentSection != null)
                    {
                        currentSection.EndQnum = questions[i - 1].Qnum;
                        currentSection.LastVarName = questions[i - 1].VarName.VarName;
                        firstDone = false;
                    }
                    Heading heading = new Heading();
                    heading.VarName.VarName = questions[i].VarName.VarName;
                    heading.PreP = questions[i].PreP;
                    heading.Qnum = questions[i].Qnum;
                    list.Add(heading);
                    inSection = true;
                    currentSection = heading;
                    
                }
                else
                {
                    if (inSection && !firstDone )
                    {
                        currentSection.StartQnum = questions[i].Qnum;
                        currentSection.FirstVarName = questions[i].VarName.VarName;
                        firstDone = true;
                    }
                }
            }
            currentSection.EndQnum = questions.Last().Qnum;
            currentSection.LastVarName = questions.Last().VarName.VarName;

            return list;
        }

        /// <summary>
        /// Returns true if the provided VarName exists in the database.
        /// </summary>
        /// <param name="varname"></param>
        /// <returns></returns>
        public static bool VarNameIsUsed(string varname)
        {
            bool exists = false;
            string query = "SELECT dbo.FN_VarNameUsed (@varname)";

            var parameters = new { varname };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                exists = (bool)db.ExecuteScalar(query, parameters);
            }
            return exists;
        }

        /// <summary>
        /// Returns the list of Parallel Questions for the provided survey code.
        /// </summary>
        /// <param name="surveyCode"></param>
        /// <returns></returns>
        public static List<ParallelQuestion> GetParallelQuestions (string surveyCode)
        {
            List<ParallelQuestion> questions = new List<ParallelQuestion>();

            string sql = "SELECT tblParallelQuestions.ID, MatchID, QID, Survey, tblVariableInformation.VarName, tblProduct.ID AS ProductID, Product " + 
                "FROM tblParallelQuestions INNER JOIN tblSurveyNumbers ON tblParallelQuestions.QID = tblSurveyNumbers.ID " +
                    "INNER JOIN tblVariableInformation ON tblSurveyNumbers.VarName = tblVariableInformation.VarName " +
                    "INNER JOIN tblProduct ON tblVariableInformation.ProductNum = tblProduct.ID " +
                "WHERE Survey = @survey " +
                "ORDER BY MatchID";

            var parameters = new { survey = surveyCode };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var results = db.Query(sql, parameters).Select(x => x as IDictionary<string, object>).ToList();
                
                foreach (IDictionary<string, object> row in results)
                {
                    ParallelQuestion pq = new ParallelQuestion();
                    pq.ID  = (int)row["ID"];
                    pq.MatchID = (int)row["MatchID"];

                    ProductLabel product = new ProductLabel((int)row["ProductID"], (string)row["Product"]);
                    SurveyQuestion sq = new SurveyQuestion((string)row["Survey"], (string)row["VarName"], product);
                    sq.ID = (int)row["QID"];
                    pq.Question = sq;

                    questions.Add(pq);
                }
            }
            return questions;
        }

        /// <summary>
        /// Returns the list of Time Frames for the specified survey.
        /// </summary>
        /// <param name="surveyCode"></param>
        /// <returns></returns>
        public static List<QuestionTimeFrame> GetTimeFrames (string surveyCode)
        {
            List<QuestionTimeFrame> timeframes;
            string sql = "SELECT Q.ID, QID, TimeFrame FROM tblQuestionTimeFrames AS Q LEFT JOIN tblSurveyNumbers AS N ON Q.QID = N.ID WHERE N.Survey =@survey;";

            var parameters = new { survey = surveyCode };

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                timeframes = db.Query<QuestionTimeFrame>(sql, parameters).ToList();
            }
            return timeframes;
        }
    }
}
