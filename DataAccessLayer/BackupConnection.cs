using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;

namespace ITCLib
{
    // TEST
    public class BackupConnection
    {
        public readonly DateTime FirstDateForSurveyNumbersID = new DateTime(2016, 6, 14); // any backups before this date will not have an ID field in tblSurveyNumbers
        public readonly DateTime FirstBackup = new DateTime(2007, 3, 7);
        DateTime dtBackupDate;
        string backupFilePath;
        string unzippedPath;    // location of unzipped file (TODO make this the application's folder)
        bool connected;
        string usualFrom = "((((((((((((tblSurveyNumbers LEFT JOIN tblVariableInformation AS VI ON tblSurveyNumbers.VarName = VI.VarName) " +
            "LEFT JOIN tblNonRespOptions ON tblSurveyNumbers.NRName = tblNonRespOptions.NRName) " +
            "LEFT JOIN tblRespOptionsTableCombined ON tblSurveyNumbers.RespName = tblRespOptionsTableCombined.RespName) " +
            "LEFT JOIN tblDomain ON VI.[Domain] = tblDomain.ID) " +
            "LEFT JOIN tblTopic ON VI.TopicNum = tblTopic.ID) " +
            "LEFT JOIN tblContent ON VI.ContentNum = tblContent.ID) " +
            "LEFT JOIN tblProduct ON VI.ProductNum = tblProduct.ID) " +
            "LEFT JOIN (SELECT [W#], Wording AS PreP FROM Wording_AllFields WHERE FieldName ='PreP') AS tblPreP ON tblSurveyNumbers.[PreP#] = tblPreP.[W#]) " +
            "LEFT JOIN (SELECT [W#], Wording AS PreI FROM Wording_AllFields WHERE FieldName ='PreI') AS tblPreI ON tblSurveyNumbers.[PreI#] = tblPreI.[W#]) " +
            "LEFT JOIN (SELECT [W#], Wording AS PreA FROM Wording_AllFields WHERE FieldName ='PreA') AS tblPreA ON tblSurveyNumbers.[PreA#] = tblPreA.[W#]) " +
            "LEFT JOIN (SELECT [W#], Wording AS LitQ FROM Wording_AllFields WHERE FieldName ='LitQ') AS tblLitQ ON tblSurveyNumbers.[LitQ#] = tblLitQ.[W#]) " +
            "LEFT JOIN (SELECT [W#], Wording AS PstI FROM Wording_AllFields WHERE FieldName ='PstI') AS tblPstI ON tblSurveyNumbers.[PstI#] = tblPstI.[W#]) " +
            "LEFT JOIN (SELECT [W#], Wording AS PstP FROM Wording_AllFields WHERE FieldName ='PstP') AS tblPstP ON tblSurveyNumbers.[PstP#] = tblPstP.[W#]";

        const string backupRepo = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\Backend\\DailyBackups\\VarInfoBack\\";

        public bool Connected { get => connected; set => connected = value; }

        public BackupConnection(DateTime date)
        {
            if (date == null)
                return;

            dtBackupDate = date;
            
            backupFilePath = date.ToString("yyyy-MM-dd") + ".7z";
            connected = false;
            switch (Connect())
            {
                case 0:
                    // no issues
                    connected = true;
                    break;
                case 1:
                    // backup file not found
                    connected = false;
                    break;
                case 2:
                    // 7zip not installed
                    connected = false;
                    break;

            }

            unzippedPath = "D:\\users\\Backend of C_VarInfo.accdb";
            // include the path to the file in the usual FROM clause
            usualFrom = usualFrom.Replace("Wording_AllFields", "Wording_AllFields IN '" + unzippedPath + "'") + " IN '" + unzippedPath + "'";


        }

        private int Connect()
        {
            if (!IsValidBackup())
                return 1;

            if (!Directory.Exists("C:\\Program Files\\7-Zip"))
                return 2;

            // unzip file here (see 7zip c# library)
            ProcessStartInfo p = new ProcessStartInfo();
            p.WorkingDirectory = Directory.GetCurrentDirectory();
            p.FileName = "7za.exe";

            //p.Arguments = "7za x " + backupRepo + backupFilePath + " -y -oD:\\users\\";
            p.Arguments = "x " + backupRepo + backupFilePath + " -y -oD:\\users\\";
            p.WindowStyle = ProcessWindowStyle.Hidden;

            Process x = Process.Start(p);
            x.WaitForExit();

            return x.ExitCode;
        }


        public DataTable GetSurveyTable(string select, string where)
        {
            DataTable backupTable;
            DateTime fileDate;
            fileDate = DateTime.Parse(backupFilePath.Replace(".7z", ""));
            if (fileDate <= DateTime.Parse(FirstDateForSurveyNumbersID.ToString()))
            {
                // might not have ID number
                backupTable = GetOldSurveyData(select, where);
            }
            else
            {
                backupTable = GetSurveyData(select, where);
            }
            return backupTable;
        }

        /// <summary>
        /// Returns a DataTable resulting from the provided select statement
        /// </summary>
        /// <returns></returns>
        private DataTable GetSurveyData(string select, string where)
        {
            DataTable d = new DataTable();

            OleDbConnection conn = new OleDbConnection(@"Provider=" + GetProvider() + ";Data Source='" + unzippedPath + "'");
            OleDbDataAdapter sql = new OleDbDataAdapter();
            string query = select + " FROM  " + usualFrom;
            if (!where.Equals("")) query += " WHERE " + where;

            query += " ORDER BY Qnum";

            using (conn)
            {
                sql.SelectCommand = new OleDbCommand(query, conn);
                sql.Fill(d);

            }
            return d;

        }

        private DataTable GetOldSurveyData(string select, string where)
        {
            DataTable d = new DataTable();
            OleDbConnection conn = new OleDbConnection(@"Provider=" + GetProvider() + ";Data Source='" + unzippedPath + "'");
            OleDbDataAdapter sql = new OleDbDataAdapter();
            if (select.Contains("tblSurveyNumbers.ID,"))
                select = select.Replace("tblSurveyNumbers.ID,", "");

            string query = select + " FROM  " + usualFrom;
            if (!where.Equals("")) query += " WHERE " + where;

            using (conn)
            {
                sql.SelectCommand = new OleDbCommand(query, conn);
                sql.Fill(d);
            }

            return d;
        }

        /// <summary>
        /// Returns a DataTable resulting from the provided select statement
        /// </summary>
        /// <returns></returns>
        public DataTable GetTranslationData(string select, string where)
        {
            DataTable d = new DataTable();
            OleDbConnection conn = new OleDbConnection(@"Provider=" + GetProvider() + ";Data Source='" + unzippedPath + "'");
            OleDbDataAdapter sql = new OleDbDataAdapter();
            string query = select + " FROM  tblTranslation";
            if (!where.Equals("")) query += " WHERE " + where;

            using (conn)
            {
                sql.SelectCommand = new OleDbCommand(query, conn);
                sql.Fill(d);

            }
            return d;

        }

        public bool IsValidBackup()
        {

            bool exists = File.Exists(backupRepo + backupFilePath);

            if (!exists)
                return false;

            long size = new FileInfo(backupRepo + backupFilePath).Length;

            if (size <= 1024)
                return false;

            return true;
               
        }

        /// <summary>
        /// Returns a DateTime representing the closest date that contains a backup. The current date is also considered a valid backup, where the "backup" is the current data.
        /// </summary>
        /// <returns></returns>
        public DateTime GetNearestBackup()
        {
            DateTime nearestAhead;
            DateTime nearestBehind;
   
            if (dtBackupDate < DateTime.Today)
            {
                nearestAhead = GetNearestFutureDate();
                nearestBehind = GetNearestPastDate();

                double daysToFuture = (nearestAhead - dtBackupDate).TotalDays;
                double daysToPast = (nearestBehind - dtBackupDate).TotalDays;

                if (Math.Abs(daysToFuture) < Math.Abs(daysToPast))
                    return nearestAhead;
                else
                    return nearestBehind;
            }
            else
            {
                return DateTime.Today;
            }
        }

        /// <summary>
        /// Returns a DateTime representing the closest date that contains a backup. The current date is also considered a valid backup, where the "backup" is the current data.
        /// </summary>
        /// <returns></returns>
        public DateTime GetNextBackup()
        {
            return GetNearestPastDate();
        }

        /// <summary>
        /// Continually adds 1 day to the backup date until a backup file is found with that date or the current date is reached.
        /// </summary>
        /// <returns></returns>
        private DateTime GetNearestFutureDate()
        {

            if (dtBackupDate > DateTime.Today)
                return DateTime.Today;

            DateTime current;

            current = dtBackupDate;

            while (!File.Exists(backupRepo + current.ToString("yyyy-MM-dd") + ".7z") && (current != DateTime.Today))
            {
                current = current.AddDays(1);
            }

            return current;
        }

        /// <summary>
        /// Continually subtracts 1 day to the backup date until a backup file is found with that date or the first backup is reached
        /// </summary>
        /// <returns></returns>
        private DateTime GetNearestPastDate()
        {

            if (dtBackupDate > DateTime.Today)
                return DateTime.Today;

            DateTime current;

            current = dtBackupDate;

            while (!File.Exists(backupRepo + current.ToString("yyyy-MM-dd") + ".7z") && (current != FirstBackup))
            {
                current = current.Subtract(new TimeSpan(1, 0, 0, 0));
            }

            return current;
        }

       

        /// <summary>
        /// Deletes the unzipped file.
        /// </summary>
        private void Disconnect()
        {
            if (File.Exists("D:\\users\\" + backupFilePath))
                File.Delete("D:\\users\\" + backupFilePath);
        }

        private string GetProvider()
        {
            bool is64BitOS = Environment.Is64BitOperatingSystem;
            string provider = is64BitOS ? "Microsoft.ACE.OLEDB.12.0" : "Microsoft.Jet.OLEDB.4.0";
            return provider;
        }
    }


    //' creates a table using the provided SQL statement and returns it's name
    //Function getTable(ByVal strSQL As String) As String
    //On Error GoTo err_handler
    //    Dim strFROM As String, strFROMIN As String
    //    strFROM = Mid(strSQL, InStr(1, strSQL, "FROM"))


    //    strFROM = Mid(strFROM, 1, InStrRev(strFROM, "WHERE") - 1)


    //    strFROMIN = "INTO TMP_tblBackup " & strFROM & " IN '" & BackupFilePath & "' "

    //    strSQL = Replace(strSQL, strFROM, strFROMIN)

    //    runSQL strSQL
    //    getTable = "TMP_tblBackup"


    //exit_procedure:
    //    Exit Function
    //err_handler:
    //    getTable = "error"
    //End Function



    //' Post: Returns a table containing all the survey data matching the strWHERE parameter
    //' Arguments:
    //' strWHERE - string. Filter for the survey data
    //Function getStandardSurveyTable(ByVal strWHERE As String) As String
    //    Dim strSQL As String


    //    strSQL = "SELECT Qnum AS SortBy, Qnum, tblSurveyNumbers.VarName, PreP, PreI, PreA, LitQ, PstI, PstP, RespOptions, NRCodes " & _
    //        "INTO TMP_tblBackup FROM " & Replace(usualFrom, "Wording_AllFields", "Wording_AllFields IN '" & BackupFilePath & "'") & " IN '" & BackupFilePath & "' WHERE " & strWHERE


    //    DoCmd.SetWarnings False
    //    DoCmd.runSQL strSQL
    //    DoCmd.SetWarnings True
    //    getStandardSurveyTable = "TMP_tblBackup"
    //End Function

    //' creates a table using the provided SQL statement and returns it's name
    //Function getTranslationTable(ByVal strSelectList As String, ByVal strWHERE As String) As String
    //On Error GoTo err_handler
    //    Dim strSELECT() As String
    //    Dim i As Integer
    //    strSELECT = Split(strSelectList, ",")


    //    DoCmd.SetWarnings False
    //    DoCmd.runSQL "SELECT " & strSelectList & " INTO TMP_tblBackup FROM tblTranslation AS T INNER JOIN tblSurveyNumbers AS S " & _
    //        "ON T.Survey = S.Survey AND T.VarName = S.VarName IN '" & BackupFilePath & "' WHERE " & strWHERE
    //    DoCmd.SetWarnings True
    //    getTranslationTable = "TMP_tblBackup"
    //exit_procedure:
    //    Exit Function
    //err_handler:
    //    Select Case err.number
    //        Case 3078
    //            MsgBox "Error getting backup data. One or more fields may not exist in the database at the chosen date."
    //        Case Else
    //            MsgBox "Error getting backup data." & vbCrLf & ErrorMessage(err.number, err.Description)
    //    End Select
    //    getTranslationTable = "error"
    //    Resume exit_procedure
    //End Function

    //Function FieldExists(ByVal strFieldName As String, ByVal strTableName As String) As Boolean
    //On Error GoTo err_handler
    //    Dim result As Boolean
    //    result = True
    //    runSQL "SELECT DISTINCT " & strFieldName & " INTO TMP FROM " & strTableName & " IN '" & BackupFilePath & "'"


    //exit_procedure:
    //    deleteTable "TMP"
    //    FieldExists = result
    //    Exit Function
    //err_handler:
    //    result = False
    //    Resume exit_procedure
    //End Function



    






}
