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
    //TODO create null-safe column read
    
    /// <summary>
    /// Static class for interacting with the Database. TODO create stored procedures on server for each of these
    /// </summary>
    public static partial class DBAction
    {

        /// <summary>
        /// Returns the list of regions.
        /// </summary>
        /// <returns></returns>
        public static List<Person> GetPeople()
        {
            List<Person> people = new List<Person>();
            Person p;
            string query = "SELECT * FROM qryIssueInit";

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
                            p = new Person((int)rdr["ID"])
                            {
                                Name = rdr.SafeGetString("Name")
                                

                            };

                            people.Add(p);
                        }

                    }
                }
                catch (Exception)
                {
                    int i = 0;
                }
            }

            return people;
        }


        /// <summary>
        /// Unlocks a survey for 5 minutes.
        /// </summary>
        /// <returns></returns>
        public static int UnlockSurvey(Survey s, int interval)
        {
            int result = 0;
            string query = "SELECT proc_unlockSurvey(@survey, @interval)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", s.SurveyCode);
                sql.SelectCommand.Parameters.AddWithValue("@interval", interval);

                try
                {
                    result = (int)sql.SelectCommand.ExecuteScalar();
                }
                catch (Exception)
                {
                    result = 1;
                }
            }

            return result;
        }

        /// <summary>
        /// Locks a survey.
        /// </summary>
        /// <returns></returns>
        public static int LockSurvey(Survey s)
        {
            int result = 0;
            string query = "SELECT proc_unlockSurvey(@survey)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", s.SurveyCode);

                try
                {
                    result = (int)sql.SelectCommand.ExecuteScalar();
                }
                catch (Exception)
                {
                    result = 1;
                }
            }

            return result;
        }



        /// <summary>
        /// Returns the list of regions.
        /// </summary>
        /// <returns></returns>
        public static List<Region> GetRegionInfo()
        {
            List<Region> regions = new List<Region>();
            Region r;
            string query = "SELECT * FROM FN_GetAllRegions()";

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
                            r = new Region
                            {
                                RegionID = (int)rdr["ID"],
                                RegionName = (string)rdr["Region"]
                                
                            };

                            regions.Add(r);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return regions;
        }

        /// <summary>
        /// Returns the list of studies.
        /// </summary>
        /// <returns></returns>
        public static List<Study> GetStudyInfo()
        {
            List<Study> studies = new List<Study>();
            Study s;
            string query = "SELECT * FROM FN_GetStudyInfo()";

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
                            s =new Study
                            {
                                StudyID = (int)rdr["ID"],
                                CountryName = (string)rdr["Country"],
                                StudyName = (string)rdr["Study"],
                                CountryCode = Int32.Parse((string)rdr["CountryCode"]),
                                ISO_Code = (string)rdr["ISO_Code"],
                                AgeGroup = (string)rdr["AgeGroup"]
                            };

                            studies.Add(s);
                        }

                    }
                }
                catch (Exception)
                {
                    
                }
            }

            return studies;
        }

        /// <summary>
        /// Returns the studies for a particular region.
        /// </summary>
        /// <param name="regionID"></param>
        /// <returns></returns>
        public static List<Study> GetStudies(int regionID)
        {
            List<Study> studies = new List<Study>();
            Study s;
            string query = "SELECT * FROM FN_GetStudies(@regionID)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@regionID", regionID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            s = new Study
                            {
                                StudyID = (int)rdr["ID"],
                                CountryName = (string)rdr["Country"],
                                StudyName = (string)rdr["Study"],
                                CountryCode = Int32.Parse((string)rdr["CountryCode"]),
                                ISO_Code = (string)rdr["ISO_Code"],
                                AgeGroup = (string)rdr["AgeGroup"]
                            };

                            studies.Add(s);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return studies;
        }

        /// <summary>
        /// Returns a list of survey waves.
        /// </summary>
        /// <returns></returns>
        public static List<StudyWave> GetWaveInfo()
        {
            List<StudyWave> waves = new List<StudyWave>();
            StudyWave w;
            string query = "SELECT * FROM FN_GetAllWaves() ORDER BY ISO_Code, Wave";

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
                            w = new StudyWave()
                            {
                                WaveID = (int)rdr["WaveID"],
                                ISO_Code = (string)rdr["ISO_Code"],
                                Wave = (double)rdr["Wave"]
                            };

                            waves.Add(w);
                        }

                    }
                }
                catch (Exception)
                {
                    int i = 0;
                }
            }

            return waves;
        }

        /// <summary>
        /// Returns the list of waves for a study.
        /// </summary>
        /// <param name="studyID"></param>
        /// <param name="getSurveys"></param>
        /// <returns></returns>
        public static List<StudyWave> GetWaves(int studyID)
        {
            List<StudyWave> waves = new List<StudyWave>();
            StudyWave w;
            string query = "SELECT * FROM FN_GetWavesByStudy(@studyID)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@studyID", studyID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            w = new StudyWave
                            {
                                WaveID = (int)rdr["WaveID"],
                                ISO_Code = (string)rdr["ISO_Code"],
                                Wave = (double)rdr["Wave"]
                            };

                            waves.Add(w);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return waves;
        }

        /// <summary>
        /// Returns the list of waves for a study.
        /// </summary>
        /// <param name="studyID"></param>
        /// <param name="getSurveys"></param>
        /// <returns></returns>
        public static List<VariablePrefix> GetVarPrefixes()
        {
            List<VariablePrefix> prefixes = new List<VariablePrefix>();
            VariablePrefix p;
            string query = "SELECT * FROM qryDomainList ORDER BY Prefix";

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
                            p = new VariablePrefix
                            {
                                ID = (int)rdr["ID"],
                                Prefix = (string)rdr["Prefix"],
                                PrefixName = (string)rdr["PrefixName"],
                                ProductType = (string)rdr["ProductType"],
                                RelatedPrefixes = (string) rdr["RelatedPrefixes"],
                                Description = (string)rdr["DomainName"],
                                Comments = (string) rdr["Comments"],
                                Inactive = (bool)rdr["Inactive"]
                            };

                            prefixes.Add(p);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }
            // TODO get all parallel and VarName ranges

            return prefixes;
        }
    }
}
