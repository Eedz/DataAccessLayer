using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            DataTable imagesTable;

            BackupConnection bkp = new BackupConnection(backup);

            string select = "SELECT tblSurveyNumbers.[ID], [Qnum] AS SortBy, [Survey], tblSurveyNumbers.[VarName], refVarName, Qnum, AltQnum, CorrectedFlag, TableFormat, tblDomain.ID AS DomainNum, tblDomain.[Domain], " +
                "tblTopic.ID AS TopicNum, [Topic], tblContent.ID AS ContentNum, [Content], VarLabel, tblProduct.ID AS ProductNum, [Product], PreP, [PreP#], PreI, [PreI#], PreA, [PreA#], LitQ, [LitQ#], PstI, [PstI#], PstP, [PstP#], RespOptions, tblSurveyNumbers.RespName, NRCodes, tblSurveyNumbers.NRName ";
            string where = "Survey = '" + s.SurveyCode + "'";

            if (!bkp.Connected)
                // could not unzip backup/7zip not installed etc. 
                return qs;

            
            Console.Write("unzipped");

            try
            {
                rawTable = bkp.GetSurveyTable(select, where);
                imagesTable = bkp.GetQuestionImageData(where);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                q.PrePW = new Wording(Convert.ToInt32(r["PreP#"]), WordingType.PreP, r["PreP"].Equals(DBNull.Value) ? "" : (string)r["PreP"]);
                q.PreIW = new Wording(Convert.ToInt32(r["PreI#"]), WordingType.PreI, r["PreI"].Equals(DBNull.Value) ? "" : (string)r["PreI"]);
                q.PreAW = new Wording(Convert.ToInt32(r["PreA#"]), WordingType.PreA, r["PreA"].Equals(DBNull.Value) ? "" : (string)r["PreA"]);
                q.LitQW = new Wording(Convert.ToInt32(r["LitQ#"]), WordingType.LitQ, r["LitQ"].Equals(DBNull.Value) ? "" : (string)r["LitQ"]);
                q.PstIW = new Wording(Convert.ToInt32(r["PstI#"]), WordingType.PstI, r["PstI"].Equals(DBNull.Value) ? "" : (string)r["PstI"]);
                q.PstPW = new Wording(Convert.ToInt32(r["PstP#"]), WordingType.PstP, r["PstP"].Equals(DBNull.Value) ? "" : (string)r["PstP"]);
                q.RespOptionsS = new ResponseSet((string)r["RespName"], ResponseType.RespOptions, r["RespOptions"].Equals(DBNull.Value) ? "" : (string)r["RespOptions"]);
                q.NRCodesS = new ResponseSet((string)r["NRName"], ResponseType.NRCodes, r["NRCodes"].Equals(DBNull.Value) ? "" : (string)r["NRCodes"]);

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
            List<SurveyImage> images = new List<SurveyImage>();
            foreach(DataRow r in imagesTable.Rows)
            {
                SurveyImage img = new SurveyImage();
                img.ID = (int)r["ID"];
                img.QID = (int)((double)r["QID"]);
                img.Survey = (string)r["Survey"];
                img.VarName = (string)r["VarName"];
                img.ImageName = (string)r["ImageName"];

                var question = qs.FirstOrDefault(x => x.ID == img.QID);
                if (question == null) continue;

                images.Add(img);
                question.Images.Add(img);
            }

            GetSurveyImageInfo(images, s);

            return qs;
        }
    }
}
