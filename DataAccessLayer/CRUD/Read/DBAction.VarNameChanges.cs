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
        // VarName Changes
        // 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static VarNameChange GetVarNameChangeByID(int ID)
        {
            VarNameChange vc = null;
            string query = "SELECT * FROM FN_GetVarNameChangeID (@id)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
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
                            vc = new VarNameChange
                            {
                                //ID = (int)rdr["ID"],
                                OldName = (string)rdr["OldName"],
                                NewName = (string)rdr["NewName"],
                                ChangeDate = (DateTime)rdr["ChangeDate"],
                                ChangedBy = new Person((int)rdr["ChangedBy"]),
                                Authorization = (string)rdr["Authorization"],
                                Rationale = (string)rdr["Reasoning"],
                                HiddenChange = (bool)rdr["TempVar"],
                            };
                            if (!rdr.IsDBNull(rdr.GetOrdinal("ChangeDateApprox"))) vc.ApproxChangeDate = (DateTime)rdr["ChangeDateApprox"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Source"))) vc.Source = (string)rdr["Source"];
                            
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return vc;
        }

        /// <summary>
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static List<VarNameChangeRecord> GetVarNameChangeBySurvey(string surveyCode)
        {
            List<VarNameChangeRecord> vcs = new List<VarNameChangeRecord>();

            string query = "SELECT * FROM FN_GetVarNameChangesSurvey (@survey)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", surveyCode);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            VarNameChangeRecord vc = new VarNameChangeRecord();

                            vc.ID = (int)rdr["ID"];
                            vc.OldName = rdr.SafeGetString("OldName");
                            vc.NewName = rdr.SafeGetString("NewName");
                            vc.ChangeDate = (DateTime)rdr["ChangeDate"];
                            vc.ChangedBy = new Person(rdr.SafeGetString("ChangedByName"), (int)rdr["ChangedBy"]);
                            vc.HiddenChange = (bool)rdr["TempVar"];
                            vc.PreFWChange = (bool)rdr["TempVar"];

                            vc.Rationale = rdr.SafeGetString("Reasoning");
                            vc.Authorization = rdr.SafeGetString("Authorization");
                            vc.ApproxChangeDate = rdr.SafeGetDate("ChangeDateApprox");
                            vc.Source = rdr.SafeGetString("Source2");
                            
                            vcs.Add(vc);
                        }
                    }
                }
                catch 
                {
                    
                }

                foreach (VarNameChangeRecord record in vcs)
                {
                    query = "SELECT * FROM qryVarNameChangeSurveys WHERE ChangeID = @changeID";

                    sql.SelectCommand = new SqlCommand(query, conn);
                    sql.SelectCommand.Parameters.AddWithValue("@changeID", record.ID);
                    try
                    {
                        using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                record.SurveysAffected.Add(new VarNameChangeSurveyRecord()
                                    {
                                        NewRecord = false,
                                        ID = (int)rdr["ID"],
                                        ChangeID = (int)rdr["ChangeID"],
                                        SurvID = (int)rdr["SurveyID"],
                                        SurveyCode = (string)rdr["Survey"]
                                    });
                            }
                        }
                    }
                    catch
                    {

                    }
                }

                foreach (VarNameChangeRecord record in vcs)
                {
                    query = "SELECT N.*, P.Name FROM qryVarNameChangeNotifications AS N LEFT JOIN qryIssueInit AS P ON N.NotifyName = P.ID WHERE ChangeID = @changeID";

                    sql.SelectCommand = new SqlCommand(query, conn);
                    sql.SelectCommand.Parameters.AddWithValue("@changeID", record.ID);
                    try
                    {
                        using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                record.Notifications.Add(
                                    new VarNameChangeNotificationRecord()
                                    {
                                        NewRecord = false,
                                        ID = (int)rdr["ID"],
                                        ChangeID = (int)rdr["ChangeID"],
                                        PersonID = (int)rdr["NotifyName"],
                                        Name = rdr.SafeGetString("Name"),
                                        NotifyType = rdr.SafeGetString("NotifyType")
                                    });
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }

            return vcs;
        }

        /// <summary>
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static List<VarNameChangeRecord> GetVarNameChanges(StudyWave wave, bool excludeTempChanges)
        {
            List<VarNameChangeRecord> vcs = new List<VarNameChangeRecord>();

            string query = "SELECT C.ID, [NewName], OldName, ChangeDate, ChangeDateApprox, S.Survey, W.StudyWave, P.Name AS ChangedByName, ChangedBy, Reasoning, [Authorization] , TempVar, [Source2] " +
                            "FROM((tblVarNameChanges AS C LEFT JOIN qryVarNameChangeSurveys AS S ON C.ID = S.ChangeID) LEFT JOIN tblStudyAttributes AS SA ON S.SurveyID = SA.ID) LEFT JOIN qryStudyWaves AS W ON SA.WaveID = W.WaveID " +
                            "LEFT JOIN qryIssueInit AS P ON C.ChangedBy = P.ID " +
                            "WHERE W.StudyWave = @wave";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@wave", wave.WaveCode);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (excludeTempChanges && (bool)rdr["TempVar"])
                                continue;

                            VarNameChangeRecord vc = new VarNameChangeRecord();

                            vc.ID = (int)rdr["ID"];
                            vc.OldName = rdr.SafeGetString("OldName");
                            vc.NewName = rdr.SafeGetString("NewName");
                            vc.ChangeDate = (DateTime)rdr["ChangeDate"];
                            vc.ChangedBy = new Person(rdr.SafeGetString("ChangedByName"), (int)rdr["ChangedBy"]);
                            vc.HiddenChange = (bool)rdr["TempVar"];


                            vc.Rationale = rdr.SafeGetString("Reasoning");
                            vc.Authorization = rdr.SafeGetString("Authorization");
                            vc.ApproxChangeDate = rdr.SafeGetDate("ChangeDateApprox");
                            vc.Source = rdr.SafeGetString("Source2");

                            vcs.Add(vc);
                        }
                    }
                }
                catch
                {

                }

                foreach (VarNameChangeRecord record in vcs)
                {
                    query = "SELECT * FROM qryVarNameChangeSurveys WHERE ChangeID = @changeID";


                    sql.SelectCommand = new SqlCommand(query, conn);
                    sql.SelectCommand.Parameters.AddWithValue("@changeID", record.ID);
                    try
                    {
                        using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                record.SurveysAffected.Add(new VarNameChangeSurveyRecord()
                                {
                                    NewRecord = false,
                                    ID = (int)rdr["ID"],
                                    ChangeID = (int)rdr["ChangeID"],
                                    SurvID = (int)rdr["SurveyID"],
                                    SurveyCode = (string)rdr["Survey"]
                                });
                            }
                        }
                    }
                    catch
                    {

                    }
                }

                foreach (VarNameChangeRecord record in vcs)
                {
                    query = "SELECT N.*, P.Name FROM qryVarNameChangeNotifications AS N LEFT JOIN qryIssueInit AS P ON N.NotifyName = P.ID WHERE ChangeID = @changeID";


                    sql.SelectCommand = new SqlCommand(query, conn);
                    sql.SelectCommand.Parameters.AddWithValue("@changeID", record.ID);
                    try
                    {
                        using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                record.Notifications.Add(
                                    new VarNameChangeNotificationRecord()
                                    {
                                        NewRecord = false,
                                        ID = (int)rdr["ID"],
                                        ChangeID = (int)rdr["ChangeID"],
                                        PersonID = (int)rdr["NotifyName"],
                                        Name = rdr.SafeGetString("Name"),
                                        NotifyType = rdr.SafeGetString("NotifyType")
                                    });
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }

            return vcs;
        }


        /// <summary>
        /// TODO include changed surveys
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static List<VarNameChange> GetVarChanges(string survey = null, string varname = null, bool refVarName = false, 
            DateTime? commentDateLower = null, DateTime? commentDateUpper = null, int commentAuthor = 0, string commentSource = null, string commentText = null)
        {
            List<VarNameChange> vcs = new List<VarNameChange>();
            VarNameChange vc = null;

            string query;
            if (refVarName)
                query = "SELECT * FROM dbo.FN_ChangeCommentSearchRefVar(" +
                        "@survey,@varname,@LDate,@UDate,@author,@commentText,@commentSource)";
            else
                query = "SELECT * FROM dbo.FN_ChangeCommentSearchVar(" +
                        "@survey, @varname, @LDate, @UDate, @author, @commentText, @commentSource)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);

                if (string.IsNullOrEmpty(survey))
                    sql.SelectCommand.Parameters.AddWithValue("@survey", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@survey", survey);

                if (string.IsNullOrEmpty(varname))
                    sql.SelectCommand.Parameters.AddWithValue("@varname", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@varname", varname);

                if (commentDateLower == null)
                    sql.SelectCommand.Parameters.AddWithValue("@LDate", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@LDate", commentDateLower);

                if (commentDateLower == null)
                    sql.SelectCommand.Parameters.AddWithValue("@UDate", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@UDate", commentDateLower);

                if (commentAuthor == 0)
                    sql.SelectCommand.Parameters.AddWithValue("@author", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@author", commentAuthor);

                if (string.IsNullOrEmpty(commentText))
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", commentText);

                if (string.IsNullOrEmpty(commentSource))
                    sql.SelectCommand.Parameters.AddWithValue("@commentSource", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@commentSource", commentSource);

                

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            
                            vc = new VarNameChange
                            {
                                //ID = (int)rdr["ID"],
                                OldName = (string)rdr["OldName"],
                                NewName = (string)rdr["NewName"],
                                ChangeDate = (DateTime)rdr["ChangeDate"],
                                ChangedBy = new Person((string)rdr["Name"], (int)rdr["ChangedBy"]),
                                HiddenChange = (bool)rdr["TempVar"],
                            };

                            if (!rdr.IsDBNull(rdr.GetOrdinal("Reasoning"))) vc.Rationale = (string)rdr["Reasoning"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Authorization"))) vc.Authorization = (string)rdr["Authorization"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("ChangeDateApprox"))) vc.ApproxChangeDate = (DateTime)rdr["ChangeDateApprox"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Source"))) vc.Source = (string)rdr["Source"];
                            vc.SurveysAffected.Add(new Survey((string)rdr["Survey"]));
                            vcs.Add(vc);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return vcs;
        }

        
    }
}
