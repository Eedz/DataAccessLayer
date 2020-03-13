using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;

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

            string query = "SELECT * FROM qrySurveyQuestions ORDER BY ID";

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
                                SurveyCode = (string)rdr["Survey"],
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
                                NumCol = (int)rdr["NumCol"],
                                NumDec = (int)rdr["NumDec"],
                                VarType = (string)rdr["VarType"],
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
                                NumCol = (int)rdr["NumCol"],
                                NumDec = (int)rdr["NumDec"],
                                VarType = (string)rdr["VarType"],
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
                                NumCol = (int)rdr["NumCol"],
                                NumDec = (int)rdr["NumDec"],
                                VarType = (string)rdr["VarType"],
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
        public static List<SurveyQuestion> GetRefVarNameQuestionsGlob(string refvarname, string surveyGlob)
        {
            List<SurveyQuestion> qs = new List<SurveyQuestion>();
            SurveyQuestion q;
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
                                NumCol = (int)rdr["NumCol"],
                                NumDec = (int)rdr["NumDec"],
                                VarType = (string)rdr["VarType"],
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
            SurveyQuestion q;
            DataTable rawTable;
            //string filePath = backup.ToString("yyyy-MM-dd") + ".7z";
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
                return null;
            }

            foreach (DataRow r in rawTable.Rows)
            {
                q = new SurveyQuestion();

                q.ID = (int)r["ID"];
                q.SurveyCode = (string)r["Survey"];
                q.VarName.FullVarName = (string)r["VarName"];
                
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
                               // VarName = (string)rdr["VarName"],
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
            int qid=0;
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
                    int i = 0;
                }
            }

            return qid;
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
                                NumCol = (int)rdr["NumCol"],
                                NumDec = (int)rdr["NumDec"],
                                VarType = (string)rdr["VarType"],
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
