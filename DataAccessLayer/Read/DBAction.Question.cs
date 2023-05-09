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
        public static List<SurveyQuestion> GetAllSurveyQuestions()
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();

            string query = "SELECT * FROM qrySurveyQuestions ORDER BY refVarName, Survey";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            SurveyQuestion q = new SurveyQuestion
                            {
                                ID = (int)rdr["ID"],
                                SurveyCode = rdr.SafeGetString("Survey"),
                                Qnum = rdr.SafeGetString("Qnum"),
                                //PreP = new Wording ((int)rdr["PreP#"], (string)rdr["PreP"]),
                                PrePNum = (int)rdr["PreP#"],
                                PreP = (string)rdr["PreP"],
                                PreINum = (int)rdr["PreI#"],
                                PreI = (string)rdr["PreI"],
                                PreANum = (int)rdr["PreA#"],
                                PreA = (string)rdr["PreA"],
                                LitQNum = (int)rdr["LitQ#"],
                                LitQ = (string)rdr["LitQ"],
                                PstINum = (int)rdr["PstI#"],
                                PstI = (string)rdr["PstI"],
                                PstPNum = (int)rdr["PstP#"],
                                PstP = (string)rdr["PstP"],
                                RespName = (string)rdr["RespName"],
                                RespOptions = (string)rdr["RespOptions"],
                                NRName = (string)rdr["NRName"],
                                NRCodes = (string)rdr["NRCodes"],
                                VarName = new VariableName((string)rdr["VarName"])
                                {
                                    VarLabel = rdr.SafeGetString("VarLabel"),
                                    Domain = new DomainLabel((int)rdr["DomainNum"], (string)rdr["Domain"]),
                                    Topic = new TopicLabel((int)rdr["TopicNum"], (string)rdr["Topic"]),
                                    Content = new ContentLabel((int)rdr["ContentNum"], (string)rdr["Content"]),
                                    Product = new ProductLabel((int)rdr["ProductNum"], (string)rdr["Product"])
                                },
                                TableFormat = (bool)rdr["TableFormat"],
                                CorrectedFlag = (bool)rdr["CorrectedFlag"],
                                
                                ScriptOnly = (bool)rdr["ScriptOnly"]
                            };

                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum"))) q.AltQnum = (string)rdr["AltQnum"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum2"))) q.AltQnum2 = (string)rdr["AltQnum2"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum3"))) q.AltQnum3 = (string)rdr["AltQnum3"];
                            qs.Add(q);
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return qs;
        }

        public static List<SurveyQuestion> GetAllSurveyVarNames()
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();

            string query = "SELECT ID, Survey, VarName, refVarName, VarLabel, ContentNum, Content, TopicNum, Topic, DomainNum, Domain, ProductNum, Product " + 
                "FROM qrySurveyQuestions ORDER BY refVarName, Survey";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            SurveyQuestion q = new SurveyQuestion
                            {
                                ID = (int)rdr["ID"],
                                SurveyCode = rdr.SafeGetString("Survey"),
                                
                                VarName = new VariableName((string)rdr["VarName"])
                                {
                                    VarLabel = rdr.SafeGetString("VarLabel"),
                                    Domain = new DomainLabel((int)rdr["DomainNum"], (string)rdr["Domain"]),
                                    Topic = new TopicLabel((int)rdr["TopicNum"], (string)rdr["Topic"]),
                                    Content = new ContentLabel((int)rdr["ContentNum"], (string)rdr["Content"]),
                                    Product = new ProductLabel((int)rdr["ProductNum"], (string)rdr["Product"])
                                },                                
                            };

                            qs.Add(q);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }

            return qs;
        }

        public static List<SurveyQuestion> GetSurveyQuestions(SurveyQuestionCriteria criteria)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();

            string query = "SELECT * FROM qrySurveyQuestions";
            query += " WHERE " + criteria.GetCriteria() + " ORDER BY Survey";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            SurveyQuestion q = new SurveyQuestion
                            {
                                ID = (int)rdr["ID"],
                                SurveyCode = rdr.SafeGetString("Survey"),
                                Qnum = rdr.SafeGetString("Qnum"),
                                //PreP = new Wording ((int)rdr["PreP#"], (string)rdr["PreP"]),
                                PrePNum = (int)rdr["PreP#"],
                                PreP = (string)rdr["PreP"],
                                PreINum = (int)rdr["PreI#"],
                                PreI = (string)rdr["PreI"],
                                PreANum = (int)rdr["PreA#"],
                                PreA = (string)rdr["PreA"],
                                LitQNum = (int)rdr["LitQ#"],
                                LitQ = (string)rdr["LitQ"],
                                PstINum = (int)rdr["PstI#"],
                                PstI = (string)rdr["PstI"],
                                PstPNum = (int)rdr["PstP#"],
                                PstP = (string)rdr["PstP"],
                                RespName = (string)rdr["RespName"],
                                RespOptions = (string)rdr["RespOptions"],
                                NRName = (string)rdr["NRName"],
                                NRCodes = (string)rdr["NRCodes"],
                                VarName = new VariableName((string)rdr["VarName"])
                                {
                                    VarLabel = rdr.SafeGetString("VarLabel"),
                                    Domain = new DomainLabel((int)rdr["DomainNum"], (string)rdr["Domain"]),
                                    Topic = new TopicLabel((int)rdr["TopicNum"], (string)rdr["Topic"]),
                                    Content = new ContentLabel((int)rdr["ContentNum"], (string)rdr["Content"]),
                                    Product = new ProductLabel((int)rdr["ProductNum"], (string)rdr["Product"])
                                },
                                TableFormat = (bool)rdr["TableFormat"],
                                CorrectedFlag = (bool)rdr["CorrectedFlag"],
                                
                                ScriptOnly = (bool)rdr["ScriptOnly"]
                            };

                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum"))) q.AltQnum = (string)rdr["AltQnum"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum2"))) q.AltQnum2 = (string)rdr["AltQnum2"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum3"))) q.AltQnum3 = (string)rdr["AltQnum3"];
                            qs.Add(q);
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
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
            SurveyQuestion q = null;
            string query = "SELECT * FROM Questions.FN_GetSurveyQuestion(@id)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@id", ID);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            q = new SurveyQuestion
                            {
                                ID = (int)rdr["ID"],
                                SurveyCode = (string)rdr["Survey"],
                                //VarName = (string)rdr["VarName"],
                                Qnum = (string)rdr["Qnum"],
                                //PreP = new Wording ((int)rdr["PreP#"], (string)rdr["PreP"]),
                                PrePNum = (int)rdr["PreP#"],
                                PreP = (string)rdr["PreP"],
                                PreINum = (int)rdr["PreI#"],
                                PreI = (string)rdr["PreI"],
                                PreANum = (int)rdr["PreA#"],
                                PreA = (string)rdr["PreA"],
                                LitQNum = (int)rdr["LitQ#"],
                                LitQ = (string)rdr["LitQ"],
                                PstINum = (int)rdr["PstI#"],
                                PstI = (string)rdr["PstI"],
                                PstPNum = (int)rdr["PstP#"],
                                PstP = (string)rdr["PstP"],
                                RespName = (string)rdr["RespName"],
                                RespOptions = (string)rdr["RespOptions"],
                                NRName = (string)rdr["NRName"],
                                NRCodes = (string)rdr["NRCodes"],
                                VarName = new VariableName((string)rdr["VarName"])
                                {
                                    VarLabel = (string)rdr["VarLabel"],
                                    Domain = new DomainLabel((int)rdr["DomainNum"], (string)rdr["Domain"]),
                                    Topic = new TopicLabel((int)rdr["TopicNum"], (string)rdr["Topic"]),
                                    Content = new ContentLabel((int)rdr["ContentNum"], (string)rdr["Content"]),
                                    Product = new ProductLabel((int)rdr["ProductNum"], (string)rdr["Product"])
                                },
                                TableFormat = (bool)rdr["TableFormat"],
                                CorrectedFlag = (bool)rdr["CorrectedFlag"],
                                
                                ScriptOnly = (bool)rdr["ScriptOnly"]
                            };

                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum"))) q.AltQnum = (string)rdr["AltQnum"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum2"))) q.AltQnum2 = (string)rdr["AltQnum2"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum3"))) q.AltQnum3 = (string)rdr["AltQnum3"];

                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return q;
        }

        /// <summary>
        /// Retrieves a set of records for a particular survey ID and returns a list of SurveyQuestion objects. 
        /// </summary>
        /// <param name="Survey">Survey object</param>
        /// <returns>List of SurveyQuestions</returns>
        public static BindingList<SurveyQuestion> GetSurveyQuestions(Survey s)
        {
            BindingList<SurveyQuestion> qs = new BindingList<SurveyQuestion>();
            
            SurveyQuestion q;
            string query = "SELECT * FROM Questions.FN_GetSurveyQuestions(@SID) ORDER BY Qnum";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@SID", s.SID);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            q = new SurveyQuestion
                            {
                                ID = (int)rdr["ID"],
                                SurveyCode = (string)rdr["Survey"],
                                //VarName = (string)rdr["VarName"],
                                Qnum = (string)rdr["Qnum"],
                                //PreP = new Wording((int)rdr["PreP#"], (string)rdr["PreP"]),
                                PrePNum = (int)rdr["PreP#"],
                                PreP = (string)rdr["PreP"],
                                PreINum = (int)rdr["PreI#"],
                                PreI = (string)rdr["PreI"],
                                PreANum = (int)rdr["PreA#"],
                                PreA = (string)rdr["PreA"],
                                LitQNum = (int)rdr["LitQ#"],
                                LitQ = (string)rdr["LitQ"],
                                PstINum = (int)rdr["PstI#"],
                                PstI = (string)rdr["PstI"],
                                PstPNum = (int)rdr["PstP#"],
                                PstP = (string)rdr["PstP"],
                                RespName = (string)rdr["RespName"],
                                RespOptions = (string)rdr["RespOptions"],
                                NRName = (string)rdr["NRName"],
                                NRCodes = (string)rdr["NRCodes"],
                                VarName = new VariableName((string)rdr["VarName"])
                                {
                                    VarLabel = rdr.SafeGetString("VarLabel"),
                                    Domain = new DomainLabel((int)rdr["DomainNum"], (string)rdr["Domain"]),
                                    Topic = new TopicLabel((int)rdr["TopicNum"], (string)rdr["Topic"]),
                                    Content = new ContentLabel((int)rdr["ContentNum"], (string)rdr["Content"]),
                                    Product = new ProductLabel((int)rdr["ProductNum"], (string)rdr["Product"])
                                },
                                TableFormat = (bool)rdr["TableFormat"],
                                CorrectedFlag = (bool)rdr["CorrectedFlag"],
                                
                                ScriptOnly = (bool)rdr["ScriptOnly"]
                            };

                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum"))) q.AltQnum = (string)rdr["AltQnum"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum2"))) q.AltQnum2 = (string)rdr["AltQnum2"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum3"))) q.AltQnum3 = (string)rdr["AltQnum3"];

                            qs.Add(q);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }

            return qs;
        }

        /// <summary>
        /// Retrieves a set of records for a particular survey ID and returns a list of SurveyQuestion objects. 
        /// </summary>
        /// <param name="Survey">Survey object</param>
        /// <returns>List of SurveyQuestions</returns>
        public static BindingList<QuestionRecord> GetSurveyQuestionRecords(Survey s)
        {
            BindingList<QuestionRecord> qs = new BindingList<QuestionRecord>();


            string sql = "SELECT ID, Survey AS SurveyCode, Qnum, PreP# AS PrePNum, PreP, PreI# AS PreINum, PreI, PreA# AS PreANum, PreA, LitQ# AS LitQNum, LitQ, " +
                    "PstI# AS PstINum, PstI, PstP# AS PstPNum, RespName, RespOptions, NRName, NRCodes, TableFormat, CorrectedFlag, ScriptOnly, AltQnum, AltQnum2, AltQnum3, " +
                    "VarName, VarLabel, " +
                    "DomainNum, DomainNum AS ID, Domain AS LabelText, "+
                    "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                    "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                    "ProductNum, ProductNum AS ID, Product AS LabelText " +
                    "FROM Questions.FN_GetSurveyQuestions(@SID) ORDER BY Qnum";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
    
                var parameters = new { SID = s.SID };

                var results = db.Query<QuestionRecord, VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, QuestionRecord>(sql,
                    (question, varname, domain, topic, content, product) =>
                {
                    varname.Domain = domain;
                    varname.Topic = topic;
                    varname.Content = content;
                    varname.Product = product;
                    question.VarName = varname;
                    return question;
                }, parameters, splitOn: "VarName, DomainNum, TopicNum, ContentNum, ProductNum").ToList();

                qs = new BindingList<QuestionRecord>(results);

            }
            
            return qs;
        }

        /// <summary>
        /// Retrieves a set of records for a particular survey ID and returns a list of SurveyQuestion objects. 
        /// </summary>
        /// <param name="Survey">Survey object</param>
        /// <returns>List of SurveyQuestions</returns>
        public static BindingList<QuestionRecord> GetCompleteSurvey(Survey s)
        {
            BindingList<QuestionRecord> qs = new BindingList<QuestionRecord>();


            string sql = "SELECT ID, Survey AS SurveyCode, Qnum, PreP# AS PrePNum, PreP, PreI# AS PreINum, PreI, PreA# AS PreANum, PreA, LitQ# AS LitQNum, LitQ, " +
                    "PstI# AS PstINum, PstI, PstP# AS PstPNum, PstP, RespName, RespOptions, NRName, NRCodes, TableFormat, CorrectedFlag, ScriptOnly, AltQnum, AltQnum2, AltQnum3, " +
                    "VarName, VarLabel, " +
                    "DomainNum, DomainNum AS ID, Domain AS LabelText, " +
                    "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                    "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                    "ProductNum, ProductNum AS ID, Product AS LabelText " +
                    "FROM Questions.FN_GetSurveyQuestions(@SID) ORDER BY Qnum;" +
                    "SELECT T.ID, QID, Q.Survey, Q.VarName, Lang AS Language, Translation AS TranslationText, Bilingual "+
                        "LanguageID, LanguageID AS ID, Lang AS LanguageName, Abbrev, ISOAbbrev, NonLatin, PreferredFont, RTL " +
                        "FROM qryTranslation AS T LEFT JOIN qrySurveyQuestions AS Q ON T.QID = Q.ID WHERE SurvID=@SID;" +
                    "SELECT ID, QID, SurvID, Survey, VarName, NoteDate, SourceName, Source " +
                        "CID, CID AS ID, Notes AS NoteText, "+
                        "NoteInit, NoteInit AS ID, Name, " +
                        "NoteTypeID, NoteTypeID AS ID, CommentType AS TypeName, ShortForm " +
                        "FROM qryCommentsQues WHERE SurvID = @SID";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                var parameters = new { SID = s.SID };

                var results = db.QueryMultiple(sql, parameters);

                // questions
                var questions = results.Read<QuestionRecord, VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, QuestionRecord>(
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
                var translations = results.Read<TranslationRecord, Language, TranslationRecord>((translation, language) =>
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

                // add translations and comments to questions
                foreach(QuestionRecord qr in questions)
                {
                    qr.Translations = translations.Where(x => x.QID == qr.ID).ToList();
                    qr.Comments = comments.Where(x => x.QID == qr.ID).ToList();
                }

                qs = new BindingList<QuestionRecord>(questions);
            }

            return qs;
        }

        

        /// <summary>
        /// Retrieves a set of records for a particular VarName and returns a list of SurveyQuestion objects. 
        /// </summary>
        /// <param name="varname">A valid VarName.</param>
        /// <returns>List of SurveyQuestions</returns>
        public static List<SurveyQuestion> GetVarNameQuestions(string varname)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();
            SurveyQuestion q;
            string query = "SELECT * FROM Questions.FN_GetVarNameQuestions(@varname) ORDER BY Qnum";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@varname", varname);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            q = new SurveyQuestion
                            {
                                ID = (int)rdr["ID"],
                                SurveyCode = (string)rdr["Survey"],
                                //VarName = (string)rdr["VarName"],
                                Qnum = (string)rdr["Qnum"],
                                //PreP = new Wording((int)rdr["PreP#"], (string)rdr["PreP"]),
                                PrePNum = (int)rdr["PreP#"],
                                PreP = (string)rdr["PreP"],
                                PreINum = (int)rdr["PreI#"],
                                PreI = (string)rdr["PreI"],
                                PreANum = (int)rdr["PreA#"],
                                PreA = (string)rdr["PreA"],
                                LitQNum = (int)rdr["LitQ#"],
                                LitQ = (string)rdr["LitQ"],
                                PstINum = (int)rdr["PstI#"],
                                PstI = (string)rdr["PstI"],
                                PstPNum = (int)rdr["PstP#"],
                                PstP = (string)rdr["PstP"],
                                RespName = (string)rdr["RespName"],
                                RespOptions = (string)rdr["RespOptions"],
                                NRName = (string)rdr["NRName"],
                                NRCodes = (string)rdr["NRCodes"],
                                VarName = new VariableName((string)rdr["VarName"])
                                {
                                    VarLabel = (string)rdr["VarLabel"],
                                    Domain = new DomainLabel((int)rdr["DomainNum"], (string)rdr["Domain"]),
                                    Topic = new TopicLabel((int)rdr["TopicNum"], (string)rdr["Topic"]),
                                    Content = new ContentLabel((int)rdr["ContentNum"], (string)rdr["Content"]),
                                    Product = new ProductLabel((int)rdr["ProductNum"], (string)rdr["Product"])
                                },
                                TableFormat = (bool)rdr["TableFormat"],
                                CorrectedFlag = (bool)rdr["CorrectedFlag"],
                                
                                ScriptOnly = (bool)rdr["ScriptOnly"]

                            };

                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum"))) q.AltQnum = (string)rdr["AltQnum"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum2"))) q.AltQnum2 = (string)rdr["AltQnum2"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum3"))) q.AltQnum3 = (string)rdr["AltQnum3"];

                            qs.Add(q);
                        }
                    }
                }
                catch (Exception)
                {
                   
                }
            }

            return qs;
        }

        /// <summary>
        /// Retrieves a set of records for a particular VarName and returns a list of SurveyQuestion objects. 
        /// </summary>
        /// <param name="varname">A valid VarName.</param>
        /// <returns>List of SurveyQuestions</returns>
        public static List<QuestionUsage> GetVarNameQuestions(VariableName varname)
        {
            List<QuestionUsage> qs = new List<QuestionUsage>();
            QuestionUsage q;
            string query = "SELECT refVarName, VarName, PreP, PreI, PreA, LitQ, PstI, PstP, RespOptions, NRCodes, STUFF((SELECT  ',' + Survey " +
                            "FROM qrySurveyQuestions SQ2 " +
                            "WHERE VarName = sq1.VarName AND PreP = sq1.PreP AND PreI = sq1.PreI AND PreA = sq1.PreA AND LitQ=sq1.LitQ AND " +
                                "PstI = sq1.PstI AND PstP = sq1.PstP AND Respoptions = sq1.RespOptions AND NRCodes = sq1.NRCodes " +
                            "GROUP BY SQ2.Survey " +
                            "ORDER BY Survey " +
                            "FOR XML PATH(''), TYPE).value('text()[1]', 'nvarchar(max)') ,1, LEN(','), '') AS SurveyList " +
                            "FROM qrySurveyQuestions Sq1 " +
                            "WHERE VarName = @varname " +
                            "GROUP BY sq1.refVarName, VarName, PreP, PreI, PreA, LitQ, PstI, PstP, RespOptions, NRCodes " +
                            "ORDER BY refVarName";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@varname", varname.VarName);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            q = new QuestionUsage
                            {
                                SurveyList = rdr.SafeGetString("SurveyList"),
                                PreP = (string)rdr["PreP"],
                                PreI = (string)rdr["PreI"],
                                PreA = (string)rdr["PreA"],
                                LitQ = (string)rdr["LitQ"],
                                PstI = (string)rdr["PstI"],
                                PstP = (string)rdr["PstP"],
                                RespOptions = (string)rdr["RespOptions"],
                                NRCodes = (string)rdr["NRCodes"],
                                VarName = new VariableName((string)rdr["VarName"])
                                
                            };
                            q.VarName.RefVarName = rdr.SafeGetString("refVarName");
                           

                            qs.Add(q);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return qs;
        }

        /// <summary>
        /// Retrieves a set of records for a particular refVarName and returns a list of SurveyQuestion objects. 
        /// </summary>
        /// <param name="refvarname">A valid refVarName.</param>
        /// <returns>List of SurveyQuestions</returns>
        public static List<SurveyQuestion> GetRefVarNameQuestions(string refvarname)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();
            SurveyQuestion q;
            string query = "SELECT * FROM Questions.FN_GetRefVarNameQuestions(@refVarName) ORDER BY Qnum";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@refVarName", refvarname);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            q = new SurveyQuestion
                            {
                                ID = (int)rdr["ID"],
                                SurveyCode = (string)rdr["Survey"],
                                //VarName = (string)rdr["VarName"],
                                Qnum = (string)rdr["Qnum"],
                                //PreP = new Wording((int)rdr["PreP#"], (string)rdr["PreP"]),
                                PrePNum = (int)rdr["PreP#"],
                                PreP = (string)rdr["PreP"],
                                PreINum = (int)rdr["PreI#"],
                                PreI = (string)rdr["PreI"],
                                PreANum = (int)rdr["PreA#"],
                                PreA = (string)rdr["PreA"],
                                LitQNum = (int)rdr["LitQ#"],
                                LitQ = (string)rdr["LitQ"],
                                PstINum = (int)rdr["PstI#"],
                                PstI = (string)rdr["PstI"],
                                PstPNum = (int)rdr["PstP#"],
                                PstP = (string)rdr["PstP"],
                                RespName = (string)rdr["RespName"],
                                RespOptions = (string)rdr["RespOptions"],
                                NRName = (string)rdr["NRName"],
                                NRCodes = (string)rdr["NRCodes"],
                                VarName = new VariableName((string)rdr["VarName"])
                                {
                                    VarLabel = (string)rdr["VarLabel"],
                                    Domain = new DomainLabel((int)rdr["DomainNum"], (string)rdr["Domain"]),
                                    Topic = new TopicLabel((int)rdr["TopicNum"], (string)rdr["Topic"]),
                                    Content = new ContentLabel((int)rdr["ContentNum"], (string)rdr["Content"]),
                                    Product = new ProductLabel((int)rdr["ProductNum"], (string)rdr["Product"])
                                },
                                TableFormat = (bool)rdr["TableFormat"],
                                CorrectedFlag = (bool)rdr["CorrectedFlag"],
                                
                                ScriptOnly = (bool)rdr["ScriptOnly"]
                            };

                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum"))) q.AltQnum = (string)rdr["AltQnum"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum2"))) q.AltQnum2 = (string)rdr["AltQnum2"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum3"))) q.AltQnum3 = (string)rdr["AltQnum3"];

                            qs.Add(q);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            return qs;
        }

        /// <summary>
        /// Retrieves a set of records for a particular VarName and returns a list of SurveyQuestion objects. 
        /// </summary>
        /// <param name="refvarname">A valid VarName.</param>
        /// <param name="surveyGlob">Survey code pattern.</param>
        /// <returns>List of SurveyQuestions</returns>
        public static List<QuestionRecord> GetRefVarNameQuestionsGlob(string refvarname, string surveyGlob = "%")
        {
            List<QuestionRecord> qs = new List<QuestionRecord>();
            QuestionRecord q;
            string query = "SELECT * FROM Questions.FN_GetRefVarNameQuestionsGlob(@refvarname, @surveyPattern) ORDER BY Qnum";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@refVarName", refvarname);
                sql.SelectCommand.Parameters.AddWithValue("@surveyPattern", surveyGlob.Replace("*", "%"));
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            q = new QuestionRecord
                            {
                                ID = (int)rdr["ID"],
                                SurveyCode = (string)rdr["Survey"],
                                Qnum = (string)rdr["Qnum"],
                                //PreP = new Wording((int)rdr["PreP#"], (string)rdr["PreP"]),
                                PrePNum = (int)rdr["PreP#"],
                                PreP = (string)rdr["PreP"],
                                PreINum = (int)rdr["PreI#"],
                                PreI = (string)rdr["PreI"],
                                PreANum = (int)rdr["PreA#"],
                                PreA = (string)rdr["PreA"],
                                LitQNum = (int)rdr["LitQ#"],
                                LitQ = (string)rdr["LitQ"],
                                PstINum = (int)rdr["PstI#"],
                                PstI = (string)rdr["PstI"],
                                PstPNum = (int)rdr["PstP#"],
                                PstP = (string)rdr["PstP"],
                                RespName = (string)rdr["RespName"],
                                RespOptions = (string)rdr["RespOptions"],
                                NRName = (string)rdr["NRName"],
                                NRCodes = (string)rdr["NRCodes"],
                                VarName = new VariableName((string)rdr["VarName"])
                                {
                                    VarLabel = (string)rdr["VarLabel"],
                                    Domain = new DomainLabel((int)rdr["DomainNum"], (string)rdr["Domain"]),
                                    Topic = new TopicLabel((int)rdr["TopicNum"], (string)rdr["Topic"]),
                                    Content = new ContentLabel((int)rdr["ContentNum"], (string)rdr["Content"]),
                                    Product = new ProductLabel((int)rdr["ProductNum"], (string)rdr["Product"])
                                },
                                TableFormat = (bool)rdr["TableFormat"],
                                CorrectedFlag = (bool)rdr["CorrectedFlag"],

                                ScriptOnly = (bool)rdr["ScriptOnly"]
                            };

                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum"))) q.AltQnum = (string)rdr["AltQnum"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum2"))) q.AltQnum2 = (string)rdr["AltQnum2"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum3"))) q.AltQnum3 = (string)rdr["AltQnum3"];

                            qs.Add(q);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return qs;
        }

        /// <summary>
        /// Returns a list of questions from a backup database.
        /// </summary>
        /// <remarks>
        /// This could be achieved by changing the FROM clause in GetSurveyTable but often there are columns that don't exist in the backups, due to 
        /// their age and all the changes that have happened to the database over the years. 
        /// </remarks>
        public static List<SurveyQuestion> GetBackupQuestions(Survey s, DateTime backup)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();
            DataTable rawTable;

            BackupConnection bkp = new BackupConnection(backup);

            string select = "SELECT tblSurveyNumbers.[ID], [Qnum] AS SortBy, [Survey], tblSurveyNumbers.[VarName], refVarName, Qnum, AltQnum, CorrectedFlag, TableFormat, tblDomain.ID AS DomainNum, tblDomain.[Domain], " +
                "tblTopic.ID AS TopicNum, [Topic], tblContent.ID AS ContentNum, [Content], VarLabel, tblProduct.ID AS ProductNum, [Product], PreP, [PreP#], PreI, [PreI#], PreA, [PreA#], LitQ, [LitQ#], PstI, [PstI#], PstP, [PstP#], RespOptions, tblSurveyNumbers.RespName, NRCodes, tblSurveyNumbers.NRName " ;
            string where = "Survey = '" + s.SurveyCode + "'";

            if (bkp.Connected)
            {
                Console.Write("unzipped");
                rawTable = bkp.GetSurveyTable(select, where);
            }
            else
            {
                // could not unzip backup/7zip not installed etc. 
                return qs;
            }

            foreach (DataRow r in rawTable.Rows)
            {
                SurveyQuestion q = new SurveyQuestion();

                q.ID = (int)r["ID"];
                q.SurveyCode = (string)r["Survey"];
                q.VarName.VarName = (string)r["VarName"];
                
                q.Qnum = (string)r["Qnum"];
                if (!DBNull.Value.Equals(r["AltQnum"])) q.AltQnum = (string)r["AltQnum"];
                //q.PreP = new Wording(Convert.ToInt32(r["PreP#"]), (string)r["PreP"]);
                q.PrePNum = Convert.ToInt32(r["PreP#"]);
                q.PreP = r["PreP"].Equals(DBNull.Value) ? "" : (string)r["PreP"];
                q.PreINum = Convert.ToInt32(r["PreI#"]);
                q.PreI = r["PreI"].Equals(DBNull.Value) ? "" : (string)r["PreI"];
                q.PreANum = Convert.ToInt32(r["PreA#"]);
                q.PreA = r["PreA"].Equals(DBNull.Value) ? "" : (string)r["PreA"];
                q.LitQNum = Convert.ToInt32(r["LitQ#"]);
                q.LitQ = r["LitQ"].Equals(DBNull.Value) ? "" : (string)r["LitQ"];
                q.PstINum = Convert.ToInt32(r["PstI#"]);
                if (DBNull.Value.Equals(r["PstI"])) q.PstI = ""; else q.PstI = (string)r["PstI"];
                q.PstPNum = Convert.ToInt32(r["PstP#"]);
                q.PstP = r["PstP"].Equals(DBNull.Value) ? "" : (string)r["PstP"];
                q.RespName = (string)r["RespName"];
                q.RespOptions = r["RespOptions"].Equals(DBNull.Value) ? "" : (string)r["RespOptions"];
                q.NRName = (string)r["NRName"];
                q.NRCodes = r["NRCodes"].Equals(DBNull.Value) ? "" : (string)r["NRCodes"];

                q.VarName = new VariableName((string)r["VarName"])
                {
                    VarLabel = (string)r["VarLabel"],
                    Domain = new DomainLabel((int)r["DomainNum"], (string)r["Domain"]),
                    Topic = new TopicLabel((int)r["TopicNum"], (string)r["Topic"]),
                    Content = new ContentLabel((int)r["ContentNum"], (string)r["Content"]),
                    Product = new ProductLabel((int)r["ProductNum"], (string)r["Product"])
                };
                
                q.TableFormat = (bool)r["TableFormat"];
                q.CorrectedFlag = (bool)r["CorrectedFlag"];

                qs.Add(q);
            }

            return qs;
        }

        /// <summary>
        /// Returns the list of corrected questions for a specified survey.
        /// </summary>
        /// <param name="surveyCode"></param>
        /// <returns></returns>
        public static List<SurveyQuestion> GetCorrectedWordings(Survey s)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();
            string query = "SELECT * FROM Questions.FN_GetCorrectedQuestions(@survey)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", s.SurveyCode);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            SurveyQuestion q = new SurveyQuestion
                            {
                                ID = (int)rdr["ID"],
                                SurveyCode = (string)rdr["Survey"],
                                Qnum = (string)rdr["Qnum"],
                                PrePNum = (int)rdr["PreP#"],
                                PreP = (string)rdr["PreP"],
                                PreINum = (int)rdr["PreI#"],
                                PreI = (string)rdr["PreI"],
                                PreANum = (int)rdr["PreA#"],
                                PreA = (string)rdr["PreA"],
                                LitQNum = (int)rdr["LitQ#"],
                                LitQ = (string)rdr["LitQ"],
                                PstINum = (int)rdr["PstI#"],
                                PstI = (string)rdr["PstI"],
                                PstPNum = (int)rdr["PstP#"],
                                PstP = (string)rdr["PstP"],
                                RespName = (string)rdr["RespName"],
                                RespOptions = (string)rdr["RespOptions"],
                                NRName = (string)rdr["NRName"],
                                NRCodes = (string)rdr["NRCodes"],
                            };

                            qs.Add(q);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return qs;
        }

        /// <summary>
        /// Returns the list of corrected questions for a specified survey.
        /// </summary>
        /// <param name="surveyCode"></param>
        /// <returns></returns>
        public static int GetQuestionID(string survey, string varname)
        {
            int qid = 0;
            string query = "SELECT ID FROM qrySurveyQuestions WHERE Survey =@survey AND Varname=@varname";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", survey);
                sql.SelectCommand.Parameters.AddWithValue("@varname", varname);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {

                            qid = (int)rdr["ID"];
                            
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }

            return qid;
        }

        /// <summary>
        /// Returns the list of corrected questions for a specified survey.
        /// </summary>
        /// <param name="surveyCode"></param>
        /// <returns></returns>
        public static int GetQuestionIDRef(string survey, string refvarname)
        {
            int qid = 0;
            string query = "SELECT ID FROM qrySurveyQuestions WHERE Survey =@survey AND refVarname=@refvarname";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", survey);
                sql.SelectCommand.Parameters.AddWithValue("@refvarname", refvarname);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {

                            qid = (int)rdr["ID"];

                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return qid;
        }

        public static List<DeletedQuestion> GetDeletedQuestions (string survey)
        {
            List<DeletedQuestion> list = new List<DeletedQuestion>();
            string query = "SELECT * FROM qryDeletedSurveyVars WHERE Survey =@survey";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", survey);
                
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            DeletedQuestion d = new DeletedQuestion();
                            d.ID = (int)rdr["ID"];
                            d.SurveyCode = rdr.SafeGetString("Survey");
                            d.VarName = rdr.SafeGetString("VarName");
                            d.VarLabel = rdr.SafeGetString("VarLabel");
                            d.ContentLabel = rdr.SafeGetString("ContentLabel");
                            d.TopicLabel = rdr.SafeGetString("TopicLabel");
                            d.DomainLabel = rdr.SafeGetString("DomainLabel");
                            d.DeleteDate = rdr.SafeGetDate("DeleteDate");
                            d.DeletedBy = rdr.SafeGetString("DeletedBy");

                            d.DeleteNotes = GetDeletedComments(survey, d.VarName);

                            list.Add(d);

                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return list;
        }

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
            bool result = false; ;
            string query = "SELECT dbo.FN_VarNameUsed (@varname)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@varname", varname);

                try
                {
                    result = (bool)sql.SelectCommand.ExecuteScalar();
                }
                catch (Exception)
                {
                    result = false;
                }
            }

            return result;
        }

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

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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

        

        //
        // Fill Methods
        // 

        /// <summary>
        /// Populates the provided Survey's question list.
        /// </summary>
        /// <param name="s"></param>
        public static void FillQuestions(Survey s, bool clearBeforeFill = false)
        {

            if (clearBeforeFill) s.Questions.Clear();

            SurveyQuestion q;
            string query = "SELECT * FROM Questions.FN_GetSurveyQuestions(@SID) ORDER BY Qnum";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@SID", s.SID);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            q = new SurveyQuestion
                            {
                                ID = (int)rdr["ID"],
                                SurveyCode = (string)rdr["Survey"],
                               // VarName = (string)rdr["VarName"],
                                Qnum = (string)rdr["Qnum"],
                                //PreP = new Wording((int)rdr["PreP#"], (string)rdr["PreP"]),
                                PrePNum = (int)rdr["PreP#"],
                                //PreP = (string)rdr["PreP"],
                                PreINum = (int)rdr["PreI#"],
                               // PreI = (string)rdr["PreI"],
                                PreANum = (int)rdr["PreA#"],
                                //PreA = (string)rdr["PreA"],
                                LitQNum = (int)rdr["LitQ#"],
                               // LitQ = (string)rdr["LitQ"],
                                PstINum = (int)rdr["PstI#"],
                               // PstI = (string)rdr["PstI"],
                                PstPNum = (int)rdr["PstP#"],
                               // PstP = (string)rdr["PstP"],
                                RespName = (string)rdr["RespName"],
                                RespOptions = (string)rdr["RespOptions"],
                                NRName = (string)rdr["NRName"],
                                NRCodes = (string)rdr["NRCodes"],

                                VarName = new VariableName((string)rdr["VarName"])
                                {
                                    VarLabel = (string)rdr["VarLabel"],
                                    Domain = new DomainLabel((int)rdr["DomainNum"], (string)rdr["Domain"]),
                                    Topic = new TopicLabel((int)rdr["TopicNum"], (string)rdr["Topic"]),
                                    Content = new ContentLabel((int)rdr["ContentNum"], (string)rdr["Content"]),
                                    Product = new ProductLabel((int)rdr["ProductNum"], (string)rdr["Product"])
                                },
                                TableFormat = (bool)rdr["TableFormat"],
                                CorrectedFlag = (bool)rdr["CorrectedFlag"],
                                
                                ScriptOnly = (bool)rdr["ScriptOnly"]
                            };

                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum"))) q.AltQnum = (string)rdr["AltQnum"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum2"))) q.AltQnum2 = (string)rdr["AltQnum2"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum3"))) q.AltQnum3 = (string)rdr["AltQnum3"];

                            if (!rdr.IsDBNull(rdr.GetOrdinal("PreP"))) q.PreP = (string)rdr["PreP"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("PreI"))) q.PreI = (string)rdr["PreI"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("PreA"))) q.PreA = (string)rdr["PreA"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("LitQ"))) q.LitQ = (string)rdr["LitQ"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("PstI"))) q.PstI = (string)rdr["PstI"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("PstP"))) q.PstP = (string)rdr["PstP"];

                            s.AddQuestion(q);
                        }
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }

            FillQuestionTimeFrames(s);
        }

        public static void FillQuestionTimeFrames(Survey s)
        {
            string sql = "SELECT Q.ID, QID, TimeFrame FROM tblQuestionTimeFrames AS Q LEFT JOIN tblSurveyNumbers AS N ON Q.QID = N.ID WHERE N.Survey =@survey;";
            var parameters = new { survey = s.SID };
            using (SqlConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                var timeframes = db.Query<QuestionTimeFrame>(sql, parameters).ToList();

                foreach (QuestionTimeFrame tf in timeframes)
                {
                    var q = s.Questions.FirstOrDefault(x => x.ID == tf.QID);
                    if (q != null)
                        q.TimeFrames.Add(tf);
                }
            }

        }

        /// <summary>
        /// Populates the provided Survey's corrected questions list.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="withComments"></param>
        /// <param name="withTranslation"></param>
        public static void FillCorrectedQuestions(Survey s)
        {
            SurveyQuestion q;
            string query = "SELECT * FROM Questions.FN_GetCorrectedQuestions(@survey) ORDER BY Qnum";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", s.SurveyCode);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            q = new SurveyQuestion
                            {
                                ID = (int)rdr["ID"],
                                SurveyCode = (string)rdr["Survey"],
                                //VarName = (string)rdr["VarName"],
                                Qnum = (string)rdr["Qnum"],
                                //AltQnum = (string)rdr["AltQnum"],
                                //PreP = new Wording((int)rdr["PreP#"], (string)rdr["PreP"]),
                                PrePNum = (int)rdr["PreP#"],
                                PreP = (string)rdr["PreP"],
                                PreINum = (int)rdr["PreI#"],
                                PreI = (string)rdr["PreI"],
                                PreANum = (int)rdr["PreA#"],
                                PreA = (string)rdr["PreA"],
                                LitQNum = (int)rdr["LitQ#"],
                                LitQ = (string)rdr["LitQ"],
                                PstINum = (int)rdr["PstI#"],
                                PstI = (string)rdr["PstI"],
                                PstPNum = (int)rdr["PstP#"],
                                PstP = (string)rdr["PstP"],
                                RespName = (string)rdr["RespName"],
                                RespOptions = (string)rdr["RespOptions"],
                                NRName = (string)rdr["NRName"],
                                NRCodes = (string)rdr["NRCodes"]
                                
                            };

                            s.CorrectedQuestions.Add(q);
                        }
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

    }
}
