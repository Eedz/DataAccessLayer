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

        /// <summary>
        /// Returns the last X number of audit entries. 
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public static List<AuditEntry> GetMostRecentChanges(int top)
        {
            List<AuditEntry> entries = new List<AuditEntry>();
            AuditEntry entry;
            string query = "SELECT TOP (" + top + ") * FROM tblAudit ORDER BY UpdateDate DESC";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
         

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            entry = new AuditEntry
                            {
                                AuditID = (int)rdr["AuditID"],
                                TableName = (string)rdr["TableName"],
                                PrimaryKeyField = (string)rdr["PrimaryKeyField"],
                                PrimaryKeyValue = (string)rdr["PrimaryKeyValue"],
                                FieldName = (string)rdr["FieldName"],
                                UpdateDate = (DateTime)rdr["UpdateDate"],
                                UserName = (string)rdr["UserName"]

                            };

                            if (!rdr.IsDBNull(rdr.GetOrdinal("OldValue"))) entry.OldValue = (string)rdr["OldValue"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("NewValue"))) entry.NewValue = (string)rdr["NewValue"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Notes"))) entry.Notes = (string)rdr["Notes"];
                            switch ((string)rdr["Type"])
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

                            entries.Add(entry);
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return entries;
        }

        /// <summary>
        /// Returns a list of audit entries from tblSurveyNumbers for the specified ID 
        /// </summary>
        /// <param name="QID"></param>
        /// <returns></returns>
        public static List<AuditEntry> GetQuestionHistory(int QID)
        {
            List<AuditEntry> entries = new List<AuditEntry>();
            AuditEntry entry;
            string query = "SELECT * FROM tblAudit WHERE TableName ='tblSurveyNumbers' AND PrimaryKeyValue=@qid ORDER BY UpdateDate ASC";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@qid", QID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            entry = new AuditEntry
                            {
                                AuditID = (int)rdr["AuditID"],
                                TableName = (string)rdr["TableName"],
                                PrimaryKeyField = (string)rdr["PrimaryKeyField"],
                                PrimaryKeyValue = (string)rdr["PrimaryKeyValue"],
                                FieldName = (string)rdr["FieldName"],
                                UpdateDate = (DateTime)rdr["UpdateDate"],
                                UserName = (string)rdr["UserName"]

                            };

                            if (!rdr.IsDBNull(rdr.GetOrdinal("OldValue"))) entry.OldValue = (string)rdr["OldValue"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("NewValue"))) entry.NewValue = (string)rdr["NewValue"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Notes"))) entry.Notes = (string)rdr["Notes"];
                            switch ((string)rdr["Type"])
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

                            entries.Add(entry);
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return entries;
        }

        /// <summary>
        /// Returns a list of audit entries from tblSurveyNumbers for the specified ID 
        /// </summary>
        /// <param name="QID"></param>
        /// <returns></returns>
        public static List<AuditEntry> GetWordingHistory(string wordingType, int ID)
        {
            List<AuditEntry> entries = new List<AuditEntry>();
            AuditEntry entry;
            string query = "SELECT * FROM tblAudit WHERE TableName ='tbl" + wordingType + "' AND PrimaryKeyValue=@qid ORDER BY UpdateDate ASC";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@qid", ID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            entry = new AuditEntry
                            {
                                AuditID = (int)rdr["AuditID"],
                                TableName = (string)rdr["TableName"],
                                PrimaryKeyField = (string)rdr["PrimaryKeyField"],
                                PrimaryKeyValue = (string)rdr["PrimaryKeyValue"],
                                FieldName = (string)rdr["FieldName"],
                                UpdateDate = (DateTime)rdr["UpdateDate"],
                                UserName = (string)rdr["UserName"]

                            };

                            if (!rdr.IsDBNull(rdr.GetOrdinal("OldValue"))) entry.OldValue = (string)rdr["OldValue"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("NewValue"))) entry.NewValue = (string)rdr["NewValue"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Notes"))) entry.Notes = (string)rdr["Notes"];
                            switch ((string)rdr["Type"])
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

                            entries.Add(entry);
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
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
            AuditEntry entry;
            string query;
            if (responseType.Equals("RespOptions"))
                query = "SELECT * FROM tblAudit WHERE TableName ='tblRespOptionsTableCombined' AND PrimaryKeyValue=@respName ORDER BY UpdateDate ASC";
            else if (responseType.Equals("NRCodes"))
                query = "SELECT * FROM tblAudit WHERE TableName ='tblNonResponseOptions' AND PrimaryKeyValue=@respName ORDER BY UpdateDate ASC";
            else
                return entries;

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@respName", respName);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            entry = new AuditEntry
                            {
                                AuditID = (int)rdr["AuditID"],
                                TableName = (string)rdr["TableName"],
                                PrimaryKeyField = (string)rdr["PrimaryKeyField"],
                                PrimaryKeyValue = (string)rdr["PrimaryKeyValue"],
                                FieldName = (string)rdr["FieldName"],
                                UpdateDate = (DateTime)rdr["UpdateDate"],
                                UserName = (string)rdr["UserName"]

                            };

                            if (!rdr.IsDBNull(rdr.GetOrdinal("OldValue"))) entry.OldValue = (string)rdr["OldValue"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("NewValue"))) entry.NewValue = (string)rdr["NewValue"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Notes"))) entry.Notes = (string)rdr["Notes"];
                            switch ((string)rdr["Type"])
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

                            entries.Add(entry);
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return entries;
        }

        /// <summary>
        /// Returns true if the specified wording has some records in the audit table
        /// </summary>
        /// <param name="QID"></param>
        /// <returns></returns>
        public static bool WordingHasHistory (string wordingType, int ID)
        {

            bool hasHistory = false;
            string query = "SELECT TOP 1 * FROM tblAudit WHERE TableName ='tbl" + wordingType + "' AND PrimaryKeyValue=@qid ORDER BY UpdateDate ASC";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@qid", ID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        hasHistory = rdr.Read();
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return hasHistory;
        }

        /// <summary>
        /// Returns true if the specified response set has some records in the audit table
        /// </summary>
        /// <param name="QID"></param>
        /// <returns></returns>
        public static bool ResponseHasHistory(string responseType, string responseName)
        {

            bool hasHistory = false;
            string query;
            if (responseType.Equals("RespOptions"))
                query = "SELECT TOP 1 * FROM tblAudit WHERE TableName ='tblRespOptionsTableCombined' AND PrimaryKeyValue=@respName ORDER BY UpdateDate ASC";
            else if (responseType.Equals("NRCodes"))
                query = "SELECT TOP 1 * FROM tblAudit WHERE TableName ='tblNonResponseOptions' AND PrimaryKeyValue=@respName ORDER BY UpdateDate ASC";
            else
                return false;


            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@respName", responseName);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        hasHistory = rdr.Read();
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
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

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            entries.Add((string)rdr["Survey"]);
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
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

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
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
                            VariableName v = new VariableName((string)rdr["VarName"]);
                            entries.Add(v);


                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return entries;
        }

        /// <summary>
        /// Returns a list of ID numbers for the provided deleted Survey/VarName
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="varname"></param>
        /// <returns></returns>
        public static string GetDeletedQID(string survey, string varname)
        {

            string pk = "0";
            string query = "SELECT PrimaryKeyValue, Survey, VarName " +
                            "FROM(SELECT PrimaryKeyValue, OldValue, NewValue, FieldName FROM tblAudit WHERE TableName = 'tblSurveyNumbers' AND [Type] = 'D') AS Entries " + 
                            "PIVOT( " + 
                                "MAX(OldValue) " + 
                                "FOR FieldName IN(Survey, VarName) " + 
                                ") AS pivOld WHERE Survey=@survey AND VarName=@varname";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
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
                            pk = (string)rdr["PrimaryKeyValue"];
                        }
                    }
                }
                catch (Exception)
                {

                }
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

            string query = "SELECT OldValue FROM tblAudit " +
                    "WHERE FieldName='VarName' AND NOT OldValue IS NULL AND PrimaryKeyValue IN (SELECT PrimaryKeyValue FROM tblAudit WHERE TableName='tblSurveyNumbers' AND FieldName='Survey' AND OldValue=@survey AND Type ='D') " +
                    "GROUP BY OldValue ORDER BY OldValue";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
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
                            VariableName v = new VariableName((string)rdr["OldValue"]);
                            entries.Add(v);
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return entries;
        }

        /// <summary>
        /// Returns a list of VarNames that were deleted from the specified survey
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static string GetDeletedWording(string wordingType, int id)
        {

            string entry = "";

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

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@ID", id);
                sql.SelectCommand.Parameters.AddWithValue("@table", type);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            entry = (string)rdr["OldValue"];
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return entry;
        }

        /// <summary>
        /// Returns a list of VarNames that were deleted from the specified survey
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public static string GetDeletedResponseSet(string wordingType, string id)
        {

            string entry = "";

            string type = "";
            string query = "";
            if (wordingType.Equals("RespOptions"))
            {
                type = "tblRespOptionsTableCombined";
                query = "SELECT OldValue FROM tblAudit " +
                    "WHERE FieldName ='ResponseOptions' AND NOT OldValue IS NULL AND PrimaryKeyValue IN (SELECT PrimaryKeyValue FROM tblAudit WHERE TableName=@table AND FieldName='RespName' AND OldValue=@id AND Type ='D') " +
                    "GROUP BY OldValue ORDER BY OldValue";
            }
            else if (wordingType.Equals("NRCodes"))
            {
                type = "tblNonRespOptions";
                query = "SELECT OldValue FROM tblAudit " +
                    "WHERE FieldName ='NRCodes' AND NOT OldValue IS NULL AND PrimaryKeyValue IN (SELECT PrimaryKeyValue FROM tblAudit WHERE TableName=@table AND FieldName='NRName' AND OldValue=@id AND Type ='D') " +
                    "GROUP BY OldValue ORDER BY OldValue";
            }
           

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@ID", id);
                sql.SelectCommand.Parameters.AddWithValue("@table", type);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            entry = (string)rdr["OldValue"];
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return entry;
        }
    }
}
