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
                "tblTopic.ID AS TopicNum, [Topic], tblContent.ID AS ContentNum, [Content], VarLabel, tblProduct.ID AS ProductNum, [Product], PreP, [PreP#], PreI, [PreI#], PreA, [PreA#], LitQ, [LitQ#], PstI, [PstI#], PstP, [PstP#], RespOptions, tblSurveyNumbers.RespName, NRCodes, tblSurveyNumbers.NRName ";
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
    }
}
