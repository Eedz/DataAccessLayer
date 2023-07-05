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
        //
        // Search related queries
        //

        //
        // Question Search
        //
        public static List<SurveyQuestion> GetSurveyQuestions(List<SearchCriterium> criteria, bool withTranslation, string withTranslationText=null, bool excludeHiddenSurveys = true)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();

            string query;
            string where = "";
            query = "SELECT * FROM qrySurveyQuestions ";

            if (criteria.Count > 0)
            { 
                int t = 0;
                foreach (SearchCriterium sc in criteria)
                {
                    //where += sc.ToString(t);
                    where += sc.GetParameterizedCondition(t);
                    where += " AND ";
                    t = t + sc.Criteria.Count();
                }                
            }
            else 
            {

            }

            if (excludeHiddenSurveys)
                where += " NOT Survey IN (SELECT Survey FROM qrySurveyInfo WHERE HideSurvey = 1)";
            else
                where = Utilities.TrimString(where, " AND ");


            if (!string.IsNullOrEmpty(where))
                query += "WHERE " + where;

            query += " ORDER BY ID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                int t = 0;
                foreach (SearchCriterium sc in criteria)
                {
                    foreach (string f in sc.Fields)
                    foreach (string s in sc.Criteria)
                    {
                        sql.SelectCommand.Parameters.AddWithValue("@tag" + t, s);
                        t++;
                    }
                }

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

                            if (withTranslation)
                            { 
                                if (string.IsNullOrEmpty(withTranslationText))
                                    q.Translations = DBAction.GetQuestionTranslations(q.ID).ToList();
                                else
                                    q.Translations = DBAction.GetQuestionTranslations(q.ID).Where(x => x.TranslationText.ToLower().Contains(withTranslationText.ToLower())).ToList();
                            }

                            // if there was translation criteria, yet no translations exist, do not add this question to the result
                            if (!string.IsNullOrEmpty(withTranslationText) && q.Translations.Count == 0)
                                continue;

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

        public static List<SurveyQuestion> GetSurveyQuestions(List<SearchCriterium> criteria)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();

            string query;
            string where = "";
            query = "SELECT * FROM qrySurveyQuestions ";

            if (criteria.Count > 0)
            {
                int t = 0;
                foreach (SearchCriterium sc in criteria)
                {
                    //where += sc.ToString(t);
                    where += sc.GetParameterizedCondition(t);
                    where += " AND ";
                    t = t + sc.Criteria.Count();
                }

                where = Utilities.TrimString(where, " AND ");

                if (!string.IsNullOrEmpty(where))
                    query += "WHERE " + where;
            }

            query += " ORDER BY ID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                int t = 0;
                foreach (SearchCriterium sc in criteria)
                {
                    foreach (string s in sc.Criteria)
                    {
                        sql.SelectCommand.Parameters.AddWithValue("@tag" + t, s);
                        t++;
                    }
                }

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

                            q.AltQnum =rdr.SafeGetString("AltQnum");
                            q.AltQnum2 = rdr.SafeGetString("AltQnum2");
                            q.AltQnum3 = rdr.SafeGetString("AltQnum3");

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
    }
}
