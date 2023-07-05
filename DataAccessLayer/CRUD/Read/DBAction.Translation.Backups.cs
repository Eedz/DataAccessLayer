using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
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
        public static void FillBackupTranslation(Survey s, DateTime backup, List<string> langs)
        {
            DataTable rawTable;
            BackupConnection bkp = new BackupConnection(backup);
            string select = "SELECT * ";
            string where = "Survey = '" + s.SurveyCode + "' AND Lang IN ('" + string.Join("','", langs) + "')";

            if (bkp.Connected)
            {
                Console.Write("unzipped");
                rawTable = bkp.GetTranslationData(select, where);
            }
            else
            {
                // could not unzip backup/7zip not installed etc. 
                return;
            }

            foreach (DataRow r in rawTable.Rows)
            {
                Translation t = new Translation();
                t.ID = (int)r["ID"];
                t.QID = (int)Math.Floor((double)r["QID"]);
                if (!DBNull.Value.Equals(r["Translation"])) t.TranslationText = (string)r["Translation"];
                if (!DBNull.Value.Equals(r["Lang"]))
                {
                    t.LanguageName = new Language() { LanguageName = (string)r["Lang"] };
                }
                if (!DBNull.Value.Equals(r["LitQ"])) t.LitQ = (string)r["LitQ"];

                SurveyQuestion q = s.QuestionByID(t.QID);

                t.Survey = q.SurveyCode;
                t.VarName = q.VarName.VarName;

                q.Translations.Add(t);
            }
        }
    }
}
