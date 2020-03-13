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
                            vc = new VarNameChange
                            {
                                ID = (int)rdr["ID"],
                                OldName = new VariableName((string)rdr["OldName"]),
                                NewName = new VariableName((string)rdr["NewName"]),
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
        /// TODO include changed surveys
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static List<VarNameChange> GetVarNameChangeBySurvey(string surveyCode, bool excludeTempChanges)
        {
            List<VarNameChange> vcs = new List<VarNameChange>();
            VarNameChange vc = null;

            string query = "SELECT * FROM FN_GetVarNameChangesSurvey (@survey)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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
                            if (excludeTempChanges && (bool)rdr["TempVar"])
                                continue;

                            vc = new VarNameChange
                            {
                                ID = (int)rdr["ID"],
                                OldName = new VariableName((string)rdr["OldName"]),
                                NewName = new VariableName((string)rdr["NewName"]),
                                ChangeDate = (DateTime)rdr["ChangeDate"],
                                ChangedBy = new Person((string)rdr["ChangedByName"], (int)rdr["ChangedBy"]),
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

        public static List<VarNameChangeNotification> GetVarNameChangeNotifications(int ChangeID)
        {

            return null;
        }
    }
}
