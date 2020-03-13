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
        // TODO create dynamic sql stored procedure for this
        public static List<SurveyQuestion> GetSurveyQuestions(string crit, bool withTranslation)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();

            //string[] conditions = crit.Split(new string[] { " AND " }, StringSplitOptions.RemoveEmptyEntries);

            
            crit = crit.Replace("*", "%");
            string query;
            if (withTranslation)
                query = "SELECT * FROM qrySurveyQuestionsTranslations WHERE " + crit + " ORDER BY ID";
            else
                query = "SELECT * FROM qrySurveyQuestions WHERE " + crit + " ORDER BY ID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@crit", crit);
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
                                NumCol = (int)rdr["NumCol"],
                                NumDec = (int)rdr["NumDec"],
                                VarType = (string)rdr["VarType"],
                                ScriptOnly = (bool)rdr["ScriptOnly"]
                            };

                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum"))) q.AltQnum = (string)rdr["AltQnum"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum2"))) q.AltQnum2 = (string)rdr["AltQnum2"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("AltQnum3"))) q.AltQnum3 = (string)rdr["AltQnum3"];

                            if (withTranslation)
                            {
                                q.Translations.Add(new Translation()
                                {
                                    Survey = q.SurveyCode,
                                    VarName = q.VarName.FullVarName,
                                    Language = (string)rdr["Lang"],
                                    TranslationText = (string)rdr["Translation"]
                                });
                            }

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

    }
}
