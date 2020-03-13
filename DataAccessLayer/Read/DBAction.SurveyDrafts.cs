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
        // Survey Drafts
        //
        public static List<SurveyDraft> ListSurveyDrafts()
        {
            List<SurveyDraft> sd = new List<SurveyDraft>();
            SurveyDraft d;
            string query = "SELECT * FROM qrySurveyDraftInfo";

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
                            d = new SurveyDraft((int)rdr["ID"], (string)rdr["DraftTitle"]);

                            sd.Add(d);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }
            return sd;
        }
    

        public static SurveyDraft GetSurveyDraft(int DraftID)
        {
            SurveyDraft d = new SurveyDraft(DraftID);
            DraftQuestion dq;
            string query = "SELECT * FROM qrySurveyDrafts WHERE DraftID = @draftID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@draftID", DraftID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            dq = new DraftQuestion()
                            {
                                ID = (int)rdr["ID"],
                                qnum = (string)rdr["Qnum"],
                                varname = (string)rdr["VarName"],
                                questionText= (string)rdr["QuestionText"],
                                comment= (string)rdr["Comment"],
                                extra1 = (string)rdr["Extra1"],
                                extra2 = (string)rdr["Extra2"],
                                extra3 = (string)rdr["Extra3"],
                                extra4 = (string)rdr["Extra4"],
                                extra5 = (string)rdr["Extra5"],
                                deleted = (bool)rdr["Deleted"],
                                inserted = (bool)rdr["Inserted"]

                            };

                            d.Questions.Add(dq);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }
            return d;
        }

        
    }
}
