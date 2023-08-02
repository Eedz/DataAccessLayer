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
        //
        // Survey Checks
        //
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SurveyCheckRec> GetSurveyCheckRecords()
        {
            List<SurveyCheckRec> records = new List<SurveyCheckRec>();
            
            string query = "SELECT SC.*, P.[Name], SA.Survey,CT.CheckType " +
                "FROM ((tblSurveyChecks AS SC LEFT JOIN tblStudyAttributes AS SA ON SC.SurvID = SA.ID) " +
                "LEFT JOIN qryIssueInit AS P ON SC.CheckInit = P.ID) " +
                "LEFT JOIN dbo.tblSurveyCheckTypes AS CT ON SC.CheckTypeID = CT.ID " +
                "ORDER BY CheckDate;";

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
                            SurveyCheckRec r = new SurveyCheckRec() {
                                ID = (int)rdr["ID"],
                                CheckType = new SurveyCheckType((int)rdr["CheckTypeID"], rdr.SafeGetString("CheckType")),
                                Name = new Person(rdr.SafeGetString("Name"), (int)rdr["CheckInit"]),
                                Comments = rdr.SafeGetString("Comments"),
                                SurveyCode = new Survey(rdr.SafeGetString("Survey")),
                            };

                            r.SurveyCode.SID = (int)rdr["SurvID"];

                            if (!rdr.IsDBNull(rdr.GetOrdinal("CheckDate")))
                                r.CheckDate = (DateTime)rdr["CheckDate"];

                    

                            r.ReferenceSurveys = new BindingList<SurveyCheckRefSurvey>(GetSurveyCheckRefSurveys(r.ID));

                            records.Add(r);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return records;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkID"></param>
        /// <returns></returns>
        public static List<SurveyCheckRefSurvey> GetSurveyCheckRefSurveys(int checkID)
        {
            List<SurveyCheckRefSurvey> records = new List<SurveyCheckRefSurvey>();

            string query = "SELECT * FROM qrySurveyCheckRefSurvs WHERE CheckID = @checkID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@checkID", checkID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            SurveyCheckRefSurvey r = new SurveyCheckRefSurvey() {
                                ID = (int)rdr["ID"],
                                CheckID = (int)rdr["CheckID"],
                                SID = (int)rdr["SurvID"]
                            };

                            if (!rdr.IsDBNull(rdr.GetOrdinal("SurveyDate")))
                                r.SurveyDate = (DateTime)rdr["SurveyDate"];

                            records.Add(r);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }

            return records;
        }

        /// <summary>
        /// Returns the list of Survey Check types.
        /// </summary>
        /// <returns></returns>
        public static List<SurveyCheckType> GetSurveyCheckTypes()
        {
            List<SurveyCheckType> records = new List<SurveyCheckType>();

            string query = "SELECT ID, CheckType FROM qrySurveyCheckTypes ORDER BY ID";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                records = db.Query<SurveyCheckType>(query).ToList();
            }

            return records;
        }

    }
}
