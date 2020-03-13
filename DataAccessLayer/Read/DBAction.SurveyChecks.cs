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
        // Survey Checks
        //
        /// <summary>
        /// Returns the user profile for the specified username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<Survey> GetSurveyCheckSurveys()
        {
            List<Survey> records = new List<Survey>();

            string query = "SELECT SurvID, Survey FROM FN_GetSurveyChecks() GROUP BY Survey, SurvID ORDER BY Survey ";

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
                            Survey r = new Survey();

                            r.SID = (int)rdr["SurvID"];
                            r.SurveyCode = rdr.SafeGetString("Survey");

                            records.Add(r);
                        }
                    }
                }
                catch (Exception)
                {
                    int i = 0;
                }
            }

            return records;
        }


        /// <summary>
        /// Returns the user profile for the specified username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<SurveyCheckRec> GetSurveyCheckRecords()
        {
            List<SurveyCheckRec> records = new List<SurveyCheckRec>();
            
            string query = "SELECT * FROM FN_GetSurveyChecks() ORDER BY CheckDate";

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
                            SurveyCheckRec r = new SurveyCheckRec();

                            r.ID = (int)rdr["ID"];
                            r.CheckType = new SurveyCheckType((int)rdr["CheckTypeID"], rdr.SafeGetString("CheckType"));
                           
                            r.Name = new Person(rdr.SafeGetString("Name"),(int)rdr["CheckInit"]);
                            
           
                            
                            r.Comments = rdr.SafeGetString("Comments");
                            r.SurveyCode = new Survey(rdr.SafeGetString("Survey"));
                            r.SurveyCode.SID = (int)rdr["SurvID"];

                            if (!rdr.IsDBNull(rdr.GetOrdinal("CheckDate")))
                                r.CheckDate = (DateTime)rdr["CheckDate"];

                    

                            r.ReferenceSurveys = new BindingList<SurveyCheckRefSurvey>(GetSurveyCheckRefSurveys(r.ID));

                            records.Add(r);
                        }
                    }
                }
                catch (Exception)
                {
                    int i = 0;
                }
            }

            return records;
        }

        /// <summary>
        /// Returns the user profile for the specified username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<SurveyCheckRefSurvey> GetSurveyCheckRefSurveys(int checkID)
        {
            List<SurveyCheckRefSurvey> records = new List<SurveyCheckRefSurvey>();

            string query = "SELECT * FROM qrySurveyCheckRefSurvs WHERE CheckID = @checkID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
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
                            SurveyCheckRefSurvey r = new SurveyCheckRefSurvey();

                            r.ID = (int)rdr["ID"];
                            r.CheckID = (int)rdr["CheckID"];
                            r.SID = (int)rdr["SurvID"];

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
        /// Returns the user profile for the specified username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<SurveyCheckType> GetSurveyCheckTypes()
        {
            List<SurveyCheckType> records = new List<SurveyCheckType>();

            string query = "SELECT * FROM qrySurveyCheckTypes ORDER BY ID";

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
                            SurveyCheckType r = new SurveyCheckType();

                            r.ID = (int)rdr["ID"];
                            r.CheckName = rdr.SafeGetString("CheckType");

                            records.Add(r);
                        }
                    }
                }
                catch (Exception)
                {
                    int i = 0;
                }
            }

            return records;
        }

    }
}
