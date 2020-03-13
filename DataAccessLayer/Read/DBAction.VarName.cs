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
        // VarNames
        // 

        /// <summary>
        /// Returns a list containing every unique refVarName.
        /// </summary>
        /// <returns></returns>
        public static List<RefVariableName> GetAllRefVars()
        {
            List<RefVariableName> refVarNames = new List<RefVariableName>();

            string query = "SELECT * FROM VarNames.FN_GetAllRefVars()";

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
                            RefVariableName rv = new RefVariableName()
                            {
                                refVarName = (string)rdr["refVarName"],
                                VarLabel = (string)rdr["VarLabel"],
                                Domain = new DomainLabel((int)rdr["DomainNum"], (string)rdr["Domain"]),
                                Topic = new TopicLabel((int)rdr["TopicNum"], (string)rdr["Topic"]),
                                Content = new ContentLabel((int)rdr["ContentNum"], (string)rdr["Content"]),
                                Product = new ProductLabel((int)rdr["ProductNum"], (string)rdr["Product"])
                            };
                            refVarNames.Add(rv);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }
            return refVarNames;
        }

        /// <summary>
        /// Returns a list containing every unique refVarName.
        /// </summary>
        /// <returns></returns>
        public static List<VariableName> GetAllVarNames()
        {
            List<VariableName> VarNames = new List<VariableName>();

            string query = "SELECT * FROM qryVariableInfo ORDER BY VarName";

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
                            VariableName v = new VariableName()
                            {
                                FullVarName = (string)rdr["VarName"],
                                VarLabel = (string)rdr["VarLabel"],
                                Domain = new DomainLabel((int)rdr["DomainNum"], (string)rdr["Domain"]),
                                Topic = new TopicLabel((int)rdr["TopicNum"], (string)rdr["Topic"]),
                                Content = new ContentLabel((int)rdr["ContentNum"], (string)rdr["Content"]),
                                Product = new ProductLabel((int)rdr["ProductNum"], (string)rdr["Product"])
                            };
                            VarNames.Add(v);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }
            return VarNames;
        }

        /// <summary>
        /// Returns a VariabelName object with the provided VarName.
        /// </summary>
        /// <param name="varname">A valid VarName.</param>
        /// <returns> Null is returned if the VarName is not found in the database.</returns>
        public static VariableName GetVariable(string varname)
        {
            VariableName v;
            string query = "SELECT * FROM VarNames.FN_GetVarName(@varname)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@varname", varname);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        rdr.Read();
                        v = new VariableName((string)rdr["VarName"])
                        {
                            RefVarName = (string)rdr["refVarName"],
                            VarLabel = (string)rdr["VarLabel"],
                            Domain = new DomainLabel((int)rdr["DomainNum"], ((string)rdr["Domain"])),
                            Topic = new TopicLabel((int)rdr["TopicNum"], ((string)rdr["Topic"])),
                            Content = new ContentLabel((int)rdr["ContentNum"], ((string)rdr["Content"])),
                            Product = new ProductLabel((int)rdr["ProductNum"], ((string)rdr["Product"])),
                        };
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return v;
        }

        /// <summary>
        /// Returns the list of all variable prefixes in use by a specific survey. TODO (eliminate non-standard vars? or make it an option)
        /// </summary>
        /// <param name="surveyFilter"></param>
        /// <returns></returns>
        public static List<string> GetVariablePrefixes(string surveyFilter)
        {
            List<string> prefixes = new List<string>();
            string query = "SELECT Prefix FROM VarNames.FN_GetVarNamePrefixes(@survey)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", surveyFilter);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            prefixes.Add((string)rdr["Prefix"]);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return prefixes;
        }

        /// <summary>
        /// Returns the list of all variable prefixes in use. TODO (eliminate non-standard vars? or make it an option)
        /// </summary>
        /// <param name="surveyFilter"></param>
        /// <returns></returns>
        public static List<string> GetVariablePrefixes()
        {
            List<string> prefixes = new List<string>();
            string query = "SELECT SUBSTRING(VarName,1,2) AS Prefix FROM qryVariableInfo GROUP BY SUBSTRING(VarName,1,2) ORDER BY SUBSTRING(VarName, 1,2)";

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
                            prefixes.Add((string)rdr["Prefix"]);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return prefixes;
        }

        /// <summary>
        /// Returns a list of containing all refVarNames for a particular survey.
        /// </summary>
        /// <param name="surveyCode"></param>
        /// <returns></returns>
        public static List<RefVariableName> GetAllRefVars(string surveyCode)
        {
            List<RefVariableName> refVarNames = new List<RefVariableName>();

            string query = "SELECT  * FROM VarNames.FN_GetSurveyRefVars(@survey)";

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
                            RefVariableName rv = new RefVariableName()
                            {
                                refVarName = (string)rdr["refVarName"],
                                VarLabel = (string)rdr["VarLabel"],
                                Domain = new DomainLabel((int)rdr["DomainNum"], (string)rdr["Domain"]),
                                Topic = new TopicLabel((int)rdr["TopicNum"], (string)rdr["Topic"]),
                                Content = new ContentLabel((int)rdr["ContentNum"], (string)rdr["Content"]),
                                Product = new ProductLabel((int)rdr["ProductNum"], (string)rdr["Product"])
                            };
                            refVarNames.Add(rv);
                        }

                    }
                }
                catch (Exception)
                {
                    
                }
            }
            return refVarNames;
        }

        /// <summary>
        /// Returns a list containing all VarNames with a particular refVarName.
        /// </summary>
        /// <param name="refVarName"></param>
        /// <returns></returns>
        public static List<VariableName> GetVarNamesByRef(string refVarName)
        {
            List<VariableName> VarNames = new List<VariableName>();
            

            string query = "SELECT * FROM VarNames.FN_GetVarNamesByRef(@refVarName)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@refVarName", refVarName);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            VariableName v = new VariableName((string)rdr["VarName"])
                            {
                                VarLabel = (string)rdr["VarLabel"],
                                Domain = new DomainLabel((int)rdr["DomainNum"], (string)rdr["Domain"]),
                                Topic = new TopicLabel((int)rdr["TopicNum"], (string)rdr["Topic"]),
                                Content = new ContentLabel((int)rdr["ContentNum"], (string)rdr["Content"]),
                                Product = new ProductLabel((int)rdr["ProductNum"], (string)rdr["Product"])
                            };
                            VarNames.Add(v);
                        }

                    }
                }
                catch (Exception)
                {
                    
                }
            }
            return VarNames;
        }

        /// <summary>
        /// Returns a list of RefVariableName objects with the provided refVarName.
        /// </summary>
        /// <param name="refVarName"></param>
        /// <returns></returns>
        public static List<RefVariableName> GetRefVarNamesPrefix(string prefix)
        {
            List<RefVariableName> refVarNames = new List<RefVariableName>();
            
            string query = "SELECT refVarName, VarLabel, DomainNum, Domain, TopicNum, Topic, ContentNum, Content, ProductNum, Product FROM qryVariableInfo WHERE SUBSTRING(refVarName,1,2) = @prefix ORDER BY refVarName";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@prefix", prefix);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {


                            RefVariableName rv = new RefVariableName()
                            {
                                refVarName = (string)rdr["refVarName"],
                                VarLabel = (string)rdr["VarLabel"],
                                Domain = new DomainLabel((int)rdr["DomainNum"], (string)rdr["Domain"]),
                                Topic = new TopicLabel((int)rdr["TopicNum"], (string)rdr["Topic"]),
                                Content = new ContentLabel((int)rdr["ContentNum"], (string)rdr["Content"]),
                                Product = new ProductLabel((int)rdr["ProductNum"], (string)rdr["Product"])
                            };
                            refVarNames.Add(rv);
                        }

                    }
                }
                catch (Exception)
                {
                    int i = 0;
                }
            }
            return refVarNames;
        }

        /// <summary>
        /// Returns a list of RefVariableName objects with the provided refVarName.
        /// </summary>
        /// <param name="refVarName"></param>
        /// <returns></returns>
        public static List<RefVariableName> GetRefVarNames(string refVarName)
        {
            List<RefVariableName> refVarNames = new List<RefVariableName>();
            RefVariableName rv;
            string query = "SELECT * FROM VarNames.FN_GetRefVarNames(@refVarName) ORDER BY refVarName";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@refVarName", refVarName);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            rv = new RefVariableName((string)rdr["refVarName"]);

                            rv.VarLabel = (string)rdr["VarLabel"];
                            rv.Domain = new DomainLabel((int)rdr["DomainNum"], (string)rdr["Domain"]);
                            rv.Topic = new TopicLabel((int)rdr["TopicNum"], (string)rdr["Topic"]);
                            rv.Content = new ContentLabel((int)rdr["ContentNum"], (string)rdr["Content"]);
                            rv.Product = new ProductLabel((int)rdr["ProductNum"], (string)rdr["Product"]);

                            refVarNames.Add(rv);
                        }

                    }
                }
                catch (Exception)
                {
                    
                }
            }
            return refVarNames;
        }

        /// <summary>
        /// Returns true if the provided VarName exists in the database.
        /// </summary>
        /// <param name="varname"></param>
        /// <returns></returns>
        public static bool VarNameExists(string varname)
        {
            bool result = false; ;
            string query = "SELECT VarNames.FN_VarNameExists(@varname)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@varname", varname);

                try
                {
                    result = (bool)sql.SelectCommand.ExecuteScalar();
                }
                catch (Exception)
                {
                    result = false;
                }
            }

            return result;
        }

        public static bool RefVarNameExists(string refvarname)
        {
            bool result = false; ;
            string query = "SELECT VarNames.FN_RefVarNameExists(@refvarname)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@refvarname", refvarname);

                try
                {
                    result = (bool)sql.SelectCommand.ExecuteScalar();
                }
                catch (Exception)
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the list of all heading variables for a specific survey.
        /// </summary>
        /// <param name="surveyFilter"></param>
        /// <returns></returns>
        public static List<Heading> GetHeadings(string surveyFilter)
        {
            List<Heading> headings = new List<Heading>();
            string query = "SELECT * FROM Questions.FN_GetHeadings(@survey) ORDER BY Qnum";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", surveyFilter);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            headings.Add(new Heading((string)rdr["Qnum"], (string)rdr["PreP"]));
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return headings;
        }

        /// <summary>
        /// Returns the list of all variables in use by a specific survey.
        /// </summary>
        /// <param name="surveyFilter"></param>
        /// <returns></returns>
        public static List<VariableName> GetVariableList(string surveyFilter)
        {
            List<VariableName> varnames = new List<VariableName>();
            string query = "SELECT * FROM VarNames.FN_GetSurveyVarNames(@survey)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", surveyFilter);
                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            VariableName v = new VariableName((string)rdr["VarName"])
                            {
                                VarLabel = (string)rdr["VarLabel"],
                                Domain = new DomainLabel((int)rdr["DomainNum"], (string)rdr["Domain"]),
                                Topic = new TopicLabel((int)rdr["TopicNum"], (string)rdr["Topic"]),
                                Content = new ContentLabel((int)rdr["ContentNum"], (string)rdr["Content"]),
                                Product = new ProductLabel((int)rdr["ProductNum"], (string)rdr["Product"]),
                            };
                            varnames.Add(v);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return varnames;
        }

        /// <summary>
        /// Gets the list of previous VarNames for a provided survey and varname.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="varname"></param>
        /// <param name="excludeTempNames"></param>
        /// <returns></returns>
        public static List<string> GetPreviousNames(string survey, string varname, bool excludeTempNames)
        {
            List<string> varlist = new List<string>();
            string list;
            string query = "SELECT dbo.FN_VarNamePreviousNames(@varname, @survey, @excludeTemp)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@varname", SqlDbType.VarChar);
                cmd.Parameters["@varname"].Value = varname;
                cmd.Parameters.Add("@survey", SqlDbType.VarChar);
                cmd.Parameters["@survey"].Value = survey;
                cmd.Parameters.Add("@excludeTemp", SqlDbType.Bit);
                cmd.Parameters["@excludeTemp"].Value = excludeTempNames;

                try
                {
                    list = (string)cmd.ExecuteScalar();
                    varlist.AddRange(list.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries));
                }
                catch (SqlException ex)
                {
#if DEBUG
                    Console.WriteLine(ex.ToString());
#endif
                }
            }

            return varlist;
        }

        /// <summary>
        /// Gets the list of previous VarNames for a provided survey and varname.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="varname"></param>
        /// <param name="excludeTempNames"></param>
        /// <returns></returns>
        public static string GetCurrentName(string survey, string varname, DateTime dateSince)
        {
            
            string currentName = "";
            string query = "SELECT dbo.FN_GetCurrentName(@varname, @survey, @date)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@varname", SqlDbType.VarChar);
                cmd.Parameters["@varname"].Value = varname;
                cmd.Parameters.Add("@survey", SqlDbType.VarChar);
                cmd.Parameters["@survey"].Value = survey;
                cmd.Parameters.AddWithValue("@date", dateSince);
                

                try
                {
                    currentName = (string)cmd.ExecuteScalar();
                    
                }
                catch (SqlException ex)
                {
#if DEBUG
                    Console.WriteLine(ex.ToString());
#endif
                }
            }

            return currentName;
        }

        //
        // Fill methods
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="excludeTempNames"></param>
        public static void FillPreviousNames(Survey s, bool excludeTempNames)
        {
            List<string> names;
            foreach (SurveyQuestion q in s.Questions)
            {
                names = GetPreviousNames(s.SurveyCode, q.VarName.FullVarName, excludeTempNames);
                
                foreach (string v in names)
                {
                    if (v != q.VarName.RefVarName)
                        q.PreviousNameList.Add(new VariableName(v));
                }
            }
        }
    }
}
