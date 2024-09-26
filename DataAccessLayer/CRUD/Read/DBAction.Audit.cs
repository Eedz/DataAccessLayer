using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace ITCLib
{
    partial class DBAction
    {

        /// <summary>
        /// Returns the most recent 'top' number of audit entries. 
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public static List<AuditEntry> GetMostRecentChanges(int top)
        {
            List<AuditEntry> entries = new List<AuditEntry>();
     
            string query = "SELECT TOP (@top) AuditID, TableName, PrimaryKeyField, PrimaryKeyValue, FieldName, UpdateDate, UserName, OldValue, " +
                "NewValue, Notes, Type " +
                "FROM tblAudit ORDER BY UpdateDate DESC";

            var parameters = new { top };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                entries = db.Query<AuditEntry, string, AuditEntry>(query, (entry, type) =>
                {
                    switch (type)
                    {
                        case "I":
                            entry.Type = AuditEntryType.Insert;
                            break;
                        case "U":
                            entry.Type = AuditEntryType.Update;
                            break;
                        case "D":
                            entry.Type = AuditEntryType.Delete;
                            break;
                    }
                    return entry;
                },
                parameters, 
                splitOn: "Type").ToList();
            }

            return entries;
        }

        /// <summary>
        /// Returns a list of audit entries from tblSurveyNumbers for the specified ID 
        /// </summary>
        /// <param name="QID"></param>
        /// <returns></returns>
        public static List<AuditEntry> GetQuestionHistory(int qid)
        {
            List<AuditEntry> entries = new List<AuditEntry>();
            string query = "SELECT AuditID, TableName, PrimaryKeyField, PrimaryKeyValue, FieldName, UpdateDate, UserName, OldValue, " +
                "NewValue, Notes, Type " +
                "FROM tblAudit WHERE TableName ='tblSurveyNumbers' AND PrimaryKeyValue=@qid ORDER BY UpdateDate ASC";

            var parameters = new { qid };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                entries = db.Query<AuditEntry, string, AuditEntry>(query, (entry, type) =>
                {
                    switch (type)
                    {
                        case "I":
                            entry.Type = AuditEntryType.Insert;
                            break;
                        case "U":
                            entry.Type = AuditEntryType.Update;
                            break;
                        case "D":
                            entry.Type = AuditEntryType.Delete;
                            break;
                    }
                    return entry;
                },
                parameters,
                splitOn: "Type").ToList();
            }

            return entries;
        }

        /// <summary>
        /// Returns a list of audit entries from the specified table with a primary key value of 'ID'
        /// </summary>
        /// <param name="wordingType"></param>
        /// <param name="qid"></param>
        /// <returns></returns>
        public static List<AuditEntry> GetWordingHistory(string wordingType, int qid)
        {
            List<AuditEntry> entries = new List<AuditEntry>();
            string query = "SELECT AuditID, TableName, PrimaryKeyField, PrimaryKeyValue, FieldName, UpdateDate, UserName, OldValue, " +
                "NewValue, Notes, Type " +
                "FROM tblAudit WHERE TableName ='tbl" + wordingType + "' AND PrimaryKeyValue=@qid ORDER BY UpdateDate ASC";

            var parameters = new { qid };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                entries = db.Query<AuditEntry, string, AuditEntry>(query, (entry, type) =>
                {
                    switch (type)
                    {
                        case "I":
                            entry.Type = AuditEntryType.Insert;
                            break;
                        case "U":
                            entry.Type = AuditEntryType.Update;
                            break;
                        case "D":
                            entry.Type = AuditEntryType.Delete;
                            break;
                    }
                    return entry;
                },
                parameters,
                splitOn: "Type").ToList();
            }

            return entries;
        }

        /// <summary>
        /// Returns a list of audit entries from tblSurveyNumbers for the specified ID 
        /// </summary>
        /// <param name="QID"></param>
        /// <returns></returns>
        public static List<AuditEntry> GetWordingHistory(string responseType, string respName)
        {
            List<AuditEntry> entries = new List<AuditEntry>();
            string query;
            if (responseType.Equals("RespOptions"))
                query = "SELECT AuditID, TableName, PrimaryKeyField, PrimaryKeyValue, FieldName, UpdateDate, UserName, OldValue, " +
                    "NewValue, Notes, Type " +
                    "FROM tblAudit WHERE TableName ='tblRespOptionsTableCombined' AND PrimaryKeyValue=@respName ORDER BY UpdateDate ASC";
            else if (responseType.Equals("NRCodes"))
                query = "SELECT AuditID, TableName, PrimaryKeyField, PrimaryKeyValue, FieldName, UpdateDate, UserName, OldValue, " +
                    "NewValue, Notes, Type " +
                    "FROM tblAudit WHERE TableName ='tblNonResponseOptions' AND PrimaryKeyValue=@respName ORDER BY UpdateDate ASC";
            else
                return entries;

            var parameters = new { respName };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                entries = db.Query<AuditEntry, string, AuditEntry>(query, (entry, type) =>
                {
                    switch (type)
                    {
                        case "I":
                            entry.Type = AuditEntryType.Insert;
                            break;
                        case "U":
                            entry.Type = AuditEntryType.Update;
                            break;
                        case "D":
                            entry.Type = AuditEntryType.Delete;
                            break;
                    }
                    return entry;
                },
                parameters,
                splitOn: "Type").ToList();
            }

            return entries;
        }

        /// <summary>
        /// Returns true if the specified wording has some records in the audit table
        /// </summary>
        /// <param name="wordingType"></param>
        /// <param name="qid"></param>
        /// <returns></returns>
        public static bool WordingHasHistory (string wordingType, int qid)
        {
            bool hasHistory = false;

            string query = "SELECT TOP 1 AuditID, TableName, PrimaryKeyField, PrimaryKeyValue, FieldName, UpdateDate, UserName, OldValue, " +
                "NewValue, Notes, Type " +
                "FROM tblAudit WHERE TableName ='tbl" + wordingType + "' AND PrimaryKeyValue=@qid ORDER BY UpdateDate ASC";

            var parameters = new { qid };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var entries = db.Query<AuditEntry, string, AuditEntry>(query, (entry, type) =>
                {
                    switch (type)
                    {
                        case "I":
                            entry.Type = AuditEntryType.Insert;
                            break;
                        case "U":
                            entry.Type = AuditEntryType.Update;
                            break;
                        case "D":
                            entry.Type = AuditEntryType.Delete;
                            break;
                    }
                    return entry;
                },
                parameters,
                splitOn: "Type").ToList();

                hasHistory = entries.Count > 0;
            }

            return hasHistory;
        }

        /// <summary>
        /// Returns true if the specified response set has some records in the audit table
        /// </summary>
        /// <param name="responseType"></param>
        /// <param name="respName"></param>
        /// <returns></returns>
        public static bool ResponseHasHistory(string responseType, string respName)
        {
            bool hasHistory = false;
            string query;
            if (responseType.Equals("RespOptions"))
                query = "SELECT TOP 1 AuditID, TableName, PrimaryKeyField, PrimaryKeyValue, FieldName, UpdateDate, UserName, OldValue, " +
                    "NewValue, Notes, Type " +
                    "FROM tblAudit WHERE TableName ='tblRespOptionsTableCombined' AND PrimaryKeyValue=@respName ORDER BY UpdateDate ASC";
            else if (responseType.Equals("NRCodes"))
                query = "SELECT TOP 1 AuditID, TableName, PrimaryKeyField, PrimaryKeyValue, FieldName, UpdateDate, UserName, OldValue, " +
                    "NewValue, Notes, Type " + 
                    "FROM tblAudit WHERE TableName ='tblNonResponseOptions' AND PrimaryKeyValue=@respName ORDER BY UpdateDate ASC";
            else
                return false;

            var parameters = new { respName };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var entries = db.Query<AuditEntry, string, AuditEntry>(query, (entry, type) =>
                {
                    switch (type)
                    {
                        case "I":
                            entry.Type = AuditEntryType.Insert;
                            break;
                        case "U":
                            entry.Type = AuditEntryType.Update;
                            break;
                        case "D":
                            entry.Type = AuditEntryType.Delete;
                            break;
                    }
                    return entry;
                },
                parameters,
                splitOn: "Type").ToList();

                hasHistory = entries.Count > 0;
            }

            return hasHistory;
        }

        /// <summary>
        /// Returns a list of Surveys fround in audit entries for tblSurveyNumbers
        /// </summary>
        /// <returns>List of Survey codes as strings</returns>
        public static List<string> GetAuditSurveys()
        {
            List<string> entries = new List<string>();
            
            string query = "SELECT Survey FROM Auditing.qryAuditQuestions WHERE NOT Survey IS NULL GROUP BY Survey ORDER BY Survey";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                entries = db.Query<string>(query).ToList();
            }

            return entries;
        }

        /// <summary>
        /// Returns a list of VarNames fround in audit entries for tblSurveyNumbers for the specified survey
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static List<VariableName> GetAuditVarNames(string survey)
        {
            List<VariableName> entries = new List<VariableName>();

            string query = "SELECT VarName FROM Auditing.qryAuditQuestions WHERE Survey=@survey AND NOT VarName IS NULL GROUP BY VarName ORDER BY VarName";

            var parameters = new { survey };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                entries = db.Query<VariableName>(query, parameters).ToList();
            }

            return entries;
        }

        /// <summary>
        /// Returns the question ID from a record of a delete action on a survey question.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="varname"></param>
        /// <returns></returns>
        public static string GetDeletedQID(string survey, string varname)
        {
            string pk = "0";
            string query = "SELECT PrimaryKeyValue " +
                            "FROM(SELECT PrimaryKeyValue, OldValue, NewValue, FieldName FROM tblAudit WHERE TableName = 'tblSurveyNumbers' AND [Type] = 'D') AS Entries " +
                            "PIVOT( " +
                                "MAX(OldValue) " +
                                "FOR FieldName IN(Survey, VarName) " +
                                ") AS pivOld WHERE Survey=@survey AND VarName=@varname";

            var parameters = new { survey, varname };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                pk = db.ExecuteScalar<string>(query, parameters);
            }

            return pk;
        }

        /// <summary>
        /// Returns a list of VarNames that were deleted from the specified survey
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static List<VariableName> GetDeletedVarNames(string survey)
        {
            List<VariableName> entries = new List<VariableName>();

            string query = "SELECT OldValue AS VarName FROM tblAudit " +
                    "WHERE FieldName='VarName' AND NOT OldValue IS NULL AND " + 
                    "PrimaryKeyValue IN (SELECT PrimaryKeyValue FROM tblAudit WHERE TableName='tblSurveyNumbers' AND FieldName='Survey' AND OldValue=@survey AND Type ='D' GROUP BY PrimaryKeyValue) " +
                    "GROUP BY OldValue ORDER BY OldValue";

            var parameters = new { survey };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                entries = db.Query<VariableName>(query, parameters).ToList();
            }

            return entries;
        }

        /// <summary>
        /// Returns a question's wording at time of deletion.
        /// </summary>
        /// <param name="wordingType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetDeletedWording(string wordingType, string id)
        {
            string wording = string.Empty;

            string type;

            if (wordingType.Equals("RespOptions"))
            {
                type = "tblRespOptionsTableCombined";

            }else if (wordingType.Equals("NRCodes"))
            {
                type = "tblNonRespOptions";
            }
            else
            {
                type = "tbl" + wordingType;
            }

            string query = "SELECT OldValue FROM tblAudit " +
                    "WHERE FieldName ='Wording' AND NOT OldValue IS NULL AND PrimaryKeyValue IN (SELECT PrimaryKeyValue FROM tblAudit WHERE TableName=@table AND FieldName='ID' AND OldValue=@id AND Type ='D') " +
                    "GROUP BY OldValue ORDER BY OldValue";

            var parameters = new { ID = id , table = type };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                wording = db.ExecuteScalar<string>(query, parameters);
            }

            return wording;
        }
    }
}
