using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace ITCLib
{
   partial class DBAction
    {
        //
        // VarNames
        // 

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
                            RefVariableName rv = new RefVariableName((string)rdr["refVarName"]);
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
        public static List<RefVariableName> GetAllRefVars()
        {
            List<RefVariableName> refVarNames = new List<RefVariableName>();

            string query = "SELECT * FROM VarNames.FN_GetAllRefVars() ORDER BY refVarName GROUP BY refVarName";

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
                            RefVariableName rv = new RefVariableName((string)rdr["refVarName"]);
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
        public static List<RefVariableName> GetAllRefVarNames()
        {
            List<RefVariableName> refVarNames = new List<RefVariableName>();

            string query = "SELECT refVarName FROM qryVariableInfo GROUP BY refVarName ORDER BY refVarName";

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
                            RefVariableName rv = new RefVariableName(rdr.SafeGetString("refVarName"));
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
        /// Returns a list containing every Survey VarName.
        /// </summary>
        /// <returns></returns>
        public static List<VariableName> GetAllVarNames()
        {
            List<VariableName> VarNames = new List<VariableName>();

            string query = "SELECT * FROM qryVariableInfo ORDER BY refVarName";

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
                            VariableName v = new VariableName()
                            {
                                VarName = (string)rdr["VarName"],
                                RefVarName = (string)rdr.SafeGetString("refVarname"),
                                VarLabel = rdr.SafeGetString("VarLabel"),
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
        /// Returns a list containing every Survey VarName.
        /// </summary>
        /// <returns></returns>
        public static List<VariableName> GetAllVarNamesD()
        {
            List<VariableName> VarNames = new List<VariableName>();

            string sql = "SELECT VarName, refVarName, VarLabel, " +
                "DomainNum, DomainNum AS ID, Domain AS LabelText, " + 
                "TopicNum, TopicNum AS ID, Topic AS LabelText, " + 
                "ContentNum, ContentNum AS ID, Content AS LabelText, " + 
                "ProductNum, ProductNum AS ID, Product AS LabelText " +
                "FROM qryVariableInfo ORDER BY refVarName";

            using (IDbConnection db = new SqlConnection(connectionString))
            {

                VarNames = db.Query<VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, VariableName>(sql,
                    (varname, domain, topic, content, product) =>
                    {
                        varname.Domain = domain;
                        varname.Topic = topic;
                        varname.Content = content;
                        varname.Product = product;
                        return varname;
                    },
                    splitOn: "DomainNum, TopicNum, ContentNum, ProductNum"
                    ).ToList();

               
            }
            
            return VarNames;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<CanonicalVariableRecord> GetAllCanonVars()
        {
            List<CanonicalVariableRecord> canonVars = new List<CanonicalVariableRecord>();

            string query = "SELECT * FROM qryEssentialQuestions ORDER BY VarName";

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
                            CanonicalVariableRecord v = new CanonicalVariableRecord()
                            {
                                ID = (int)rdr["ID"],
                                RefVarName = rdr.SafeGetString("VarName"),
                                AnySuffix = (bool)rdr["AnySuffix"],
                                Notes = rdr.SafeGetString("Notes"),
                                Active = (bool)rdr["Active"]
                            };
                            canonVars.Add(v);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }
            return canonVars;
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
            using (SqlConnection conn = new SqlConnection(connectionString))
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
            using (SqlConnection conn = new SqlConnection(connectionString))
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
        /// <returns></returns>
        public static List<string> GetVariablePrefixList()
        {
            List<string> prefixes = new List<string>();
            string query = "SELECT SUBSTRING(VarName,1,2) AS Prefix FROM qryVariableInfo GROUP BY SUBSTRING(VarName,1,2) ORDER BY SUBSTRING(VarName, 1,2)";

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
        /// Returns the list of all variable prefixes in use by the listed surveys. TODO (eliminate non-standard vars? or make it an option)
        /// </summary>
        /// <returns></returns>
        public static List<string> GetVariablePrefixes(List<string> surveys)
        {
            List<string> prefixes = new List<string>();
            string query = "SELECT SUBSTRING(VarName,1,2) AS Prefix FROM qrySurveyQuestions WHERE Survey IN ({0}) GROUP BY SUBSTRING(VarName,1,2) ORDER BY SUBSTRING(VarName, 1,2)";

            string[] paramNames = surveys.Select((s, i) => "@tag" + i.ToString()).ToArray();
            string inClause = string.Join(", ", paramNames);
            query = string.Format(query, inClause);

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                for (int i = 0; i < paramNames.Length; i++)
                {
                    sql.SelectCommand.Parameters.AddWithValue(paramNames[i], surveys[i]);
                }

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
        /// Returns a list containing all VarNames with a particular refVarName.
        /// </summary>
        /// <param name="refVarName"></param>
        /// <returns></returns>
        public static List<VariableName> GetVarNamesByRef(string refVarName)
        {
            List<VariableName> VarNames = new List<VariableName>();
            

            string query = "SELECT * FROM VarNames.FN_GetVarNamesByRef(@refVarName)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
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
            
            string query = "SELECT refVarName FROM qryVariableInfo WHERE SUBSTRING(refVarName,1,2) = @prefix GROUP BY refVarName ORDER BY refVarName ";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
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
                            RefVariableName rv = new RefVariableName((string)rdr["refVarName"]);
                            
                            refVarNames.Add(rv);
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            return refVarNames;
        }

        /// <summary>
        /// Returns a list of VariableNameSurveys objects with the provided refVarName.
        /// </summary>
        /// <param name="refVarName"></param>
        /// <returns></returns>
        async public static Task<List<VariableNameSurveys>> GetVarNamesPrefixAsync(string prefix)
        {
            List<VariableNameSurveys> refVarNames = new List<VariableNameSurveys>();
            if (string.IsNullOrEmpty(prefix))
                return refVarNames;

            string query = "SELECT refVarName, VarName, VarLabel, Domain, DomainNum, Content, ContentNum, Topic, TopicNum, ProductNum, Product, STUFF((SELECT  ',' + Survey " +
                            "FROM qrySurveyQuestions SQ2 " +
                            "WHERE VarName = sq1.VarName " +
                            "GROUP BY SQ2.Survey " +
                            "ORDER BY Survey " +
                            "FOR XML PATH(''), TYPE).value('text()[1]', 'nvarchar(max)') ,1, LEN(','), '') AS SurveyList " +
                            "FROM qrySurveyQuestions Sq1 " +
                            "WHERE SUBSTRING(refVarName,1,2) = @prefix " +
                            "GROUP BY sq1.refVarName, VarName, Sq1.VarLabel, Sq1.Domain, DomainNum, Sq1.Content, ContentNum, Sq1.Topic, TopicNum, Sq1.Product, ProductNum " +
                            "ORDER BY refVarName";           

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@prefix", prefix);
                try
                {
                    var rdr = await sql.SelectCommand.ExecuteReaderAsync();
                    using (rdr)// = sql.SelectCommand.ExecuteReaderAsync().Result)
                    {
                        while (rdr.Read())
                        {
                            VariableNameSurveys rv = new VariableNameSurveys();
                            rv.VarName = rdr.SafeGetString("VarName");
                            rv.RefVarName = rdr.SafeGetString("refVarName");
                            rv.SurveyList = rdr.SafeGetString("SurveyList");

                            rv.VarLabel = rdr.SafeGetString("VarLabel");
                            rv.Domain.ID = (int)rdr["DomainNum"];
                            rv.Domain.LabelText = rdr.SafeGetString("Domain");
                            rv.Topic.ID = (int)rdr["TopicNum"];
                            rv.Topic.LabelText = rdr.SafeGetString("Topic");
                            rv.Content.ID = (int)rdr["ContentNum"];
                            rv.Content.LabelText = rdr.SafeGetString("Content");
                            rv.Product.ID = (int)rdr["ProductNum"];
                            rv.Product.LabelText = rdr.SafeGetString("Product");

                            refVarNames.Add(rv);
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            return refVarNames;
        }

        /// <summary>
        /// Returns a list of VariableNameSurveys objects with the provided refVarName.
        /// </summary>
        /// <param name="refVarName"></param>
        /// <returns></returns>
        public static List<VariableNameSurveys> GetVarNamesPrefix(string prefix)
        {
            List<VariableNameSurveys> refVarNames = new List<VariableNameSurveys>();
            if (string.IsNullOrEmpty(prefix))
                return refVarNames;

            string query = "SELECT refVarName, VarName, VarLabel, Domain, DomainNum, Content, ContentNum, Topic, TopicNum, ProductNum, Product, STUFF((SELECT  ',' + Survey " +
                            "FROM qrySurveyQuestions SQ2 " +
                            "WHERE VarName = sq1.VarName " +
                            "GROUP BY SQ2.Survey " +
                            "ORDER BY Survey " +
                            "FOR XML PATH(''), TYPE).value('text()[1]', 'nvarchar(max)') ,1, LEN(','), '') AS SurveyList " +
                            "FROM qrySurveyQuestions Sq1 " +
                            "WHERE SUBSTRING(refVarName,1,2) = @prefix " +
                            "GROUP BY sq1.refVarName, VarName, Sq1.VarLabel, Sq1.Domain, DomainNum, Sq1.Content, ContentNum, Sq1.Topic, TopicNum, Sq1.Product, ProductNum " +
                            "ORDER BY refVarName";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
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
                            VariableNameSurveys rv = new VariableNameSurveys();
                            rv.VarName = rdr.SafeGetString("VarName");
                            rv.RefVarName = rdr.SafeGetString("refVarName");
                            rv.SurveyList = rdr.SafeGetString("SurveyList");

                            rv.VarLabel = rdr.SafeGetString("VarLabel");
                            rv.Domain.ID = (int)rdr["DomainNum"];
                            rv.Domain.LabelText = rdr.SafeGetString("Domain");
                            rv.Topic.ID = (int)rdr["TopicNum"];
                            rv.Topic.LabelText = rdr.SafeGetString("Topic");
                            rv.Content.ID = (int)rdr["ContentNum"];
                            rv.Content.LabelText = rdr.SafeGetString("Content");
                            rv.Product.ID = (int)rdr["ProductNum"];
                            rv.Product.LabelText = rdr.SafeGetString("Product");

                            refVarNames.Add(rv);
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            return refVarNames;
        }

        /// <summary>
        /// Returns a list of RefVariableName objects with the provided refVarName.
        /// </summary>
        /// <param name="refVarName"></param>
        /// <returns></returns>
        public static List<VariableNameSurveys> GetVarNameUsage()
        {
            List<VariableNameSurveys> refVarNames = new List<VariableNameSurveys>();

            string query = "SELECT refVarName, VarName, VarLabel, Domain, DomainNum, Content, ContentNum, Topic, TopicNum, ProductNum, Product, STUFF((SELECT  ',' + Survey " +
                            "FROM qrySurveyQuestions SQ2 " +
                            "WHERE VarName = sq1.VarName " +
                            "GROUP BY SQ2.Survey " +
                            "ORDER BY Survey " +
                            "FOR XML PATH(''), TYPE).value('text()[1]', 'nvarchar(max)') ,1, LEN(','), '') AS SurveyList " +
                            "FROM qrySurveyQuestions Sq1 " +
                             "GROUP BY sq1.refVarName, VarName, Sq1.VarLabel, Sq1.Domain, DomainNum, Sq1.Content, ContentNum, Sq1.Topic, TopicNum, Sq1.Product, ProductNum " +
                            "ORDER BY refVarName";

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
                            VariableNameSurveys rv = new VariableNameSurveys();
                            rv.VarName = rdr.SafeGetString("VarName");
                            rv.RefVarName = rdr.SafeGetString("refVarName");
                            rv.SurveyList = rdr.SafeGetString("SurveyList");

                            rv.VarLabel = rdr.SafeGetString("VarLabel");
                            rv.Domain.ID = (int)rdr["DomainNum"];
                            rv.Domain.LabelText = rdr.SafeGetString("Domain");
                            rv.Topic.ID = (int)rdr["TopicNum"];
                            rv.Topic.LabelText = rdr.SafeGetString("Topic");
                            rv.Content.ID = (int)rdr["ContentNum"];
                            rv.Content.LabelText = rdr.SafeGetString("Content");
                            rv.Product.ID = (int)rdr["ProductNum"];
                            rv.Product.LabelText = rdr.SafeGetString("Product");

                            refVarNames.Add(rv);
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
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
            string query = "SELECT * FROM VarNames.FN_GetRefVarNames(@refVarName) ORDER BY refVarName GROUP BY refVarName";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
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
            using (SqlConnection conn = new SqlConnection(connectionString))
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
            using (SqlConnection conn = new SqlConnection(connectionString))
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
            using (SqlConnection conn = new SqlConnection(connectionString))
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
                            Heading heading = new Heading((string)rdr["Qnum"], Utilities.FixElements((string)rdr["PreP"]));
                            heading.VarName.VarName = rdr.SafeGetString("VarName");
                            headings.Add(heading);
                            
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            

            return headings;
        }

        

        public static List<VariableName> GetSectionVarNames(Heading heading, Survey survey)
        {
            List<VariableName> vars = new List<VariableName>();
            List<SurveyQuestion> questions = GetSurveyQuestions(survey).ToList();

            int f = questions.FindIndex(x => x.VarName.VarName.Equals(heading.VarName));

            if (f == -1)
                return vars;

            for (int i = f +1; i < questions.Count; i ++)
            {
                if (questions[i].VarName.VarName.StartsWith("Z") && !questions[i].VarName.VarName.EndsWith("s"))
                {
                    break;
                }
                else
                {
                    vars.Add(new VariableName(questions[i].VarName.VarName));
                }
            }


            return vars;
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
            using (SqlConnection conn = new SqlConnection(connectionString))
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
                catch (Exception e)
                {
                    Console.Write(e.Message);
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
            using (SqlConnection conn = new SqlConnection(connectionString))
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
            using (SqlConnection conn = new SqlConnection(connectionString))
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
                    if (string.IsNullOrEmpty(currentName))
                        currentName = varname;
                }
                catch (SqlException ex)
                {
#if DEBUG
                    Console.WriteLine(ex.ToString());

#endif
                    currentName = varname;
                }
            }

            return currentName;
        }


        public static List<VarNameKeyword> GetVarNameKeywords()
        {
            List<VarNameKeyword> list = new List<VarNameKeyword>();
         
            string query = "SELECT tblVarNameKeywords.*, tblKeyword.Keyword AS KeywordLabel FROM tblVarNameKeywords INNER JOIN tblKeyword ON tblVarNameKeywords.Keyword = tblKeyword.ID ORDER BY refVarName";

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
                            VarNameKeyword keyword = new VarNameKeyword();
                            keyword.ID = (int)rdr["ID"];
                            keyword.RefVarName = rdr.SafeGetString("refVarName");
                            keyword.Key = new Keyword((int)rdr["Keyword"], rdr.SafeGetString("KeywordLabel"));

                            list.Add(keyword);
                        }
                    }
                }
                catch (SqlException ex)
                {

                    Console.WriteLine(ex.ToString());

                }
            }

            return list;
        }


        public static List<SurveyQuestion> GetVarNameUses(string fullVarName)
        {
            List<SurveyQuestion> list = new List<SurveyQuestion>();

            string query = "SELECT Survey, VarName, refVarName, Qnum FROM qrySurveyQuestions WHERE VarName = @varname ORDER BY Survey";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@varname", fullVarName);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            SurveyQuestion q = new SurveyQuestion();
                            q.SurveyCode = rdr.SafeGetString("Survey");
                            q.VarName.VarName = rdr.SafeGetString("VarName");
                            q.VarName.RefVarName = rdr.SafeGetString("refVarName");
                            q.Qnum = rdr.SafeGetString("Qnum");



                            list.Add(q);
                        }
                    }
                }
                catch (SqlException ex)
                {

                    Console.WriteLine(ex.ToString());

                }
            }

            return list;
        }

        /// <summary>
        /// Returns the list of all variables in use by a list of surveys.
        /// </summary>
        /// <param name="surveyFilter"></param>
        /// <returns></returns>
        public static DataTable GetSurveysVariableList(List<string> surveys, List<string> vars, List<string> additionalColumns, List<string> exclusions)
        {
            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn("refVarName", Type.GetType("System.String")));
            foreach (string s in additionalColumns)
                result.Columns.Add(new DataColumn(s, Type.GetType("System.String")));
            foreach (string s in surveys)
                result.Columns.Add(new DataColumn(s, Type.GetType("System.String")));


            string columns = "[" + string.Join("],[", surveys) + "]";
            string filter = "'" + string.Join("','", surveys) + "'";
            string varFilter = "'" + string.Join("','", vars) + "'";
            string addColumns = ", " + string.Join(", ", additionalColumns);
            addColumns = addColumns.TrimEnd(new char[] { ',', ' ' });
            string exclusionList = "(" + string.Join(") AND (", exclusions) + ")";

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM ");
            sb.Append("(SELECT Survey, refVarName, Qnum" + addColumns + " ");
            
            sb.Append("FROM qrySurveyQuestions ");
            if (surveys.Count > 0 || exclusions.Count > 0 || vars.Count > 0)
            {
                exclusionList = " AND " + exclusionList;
                sb.Append("WHERE ");
            }

            if (surveys.Count > 0)
                sb.Append("Survey IN (" + filter + ")");
            if (exclusions.Count>0)
                sb.Append(exclusionList);
            if (vars.Count > 0)
                sb.Append(" AND refVarName IN (" + varFilter + ")");

            sb.Append(") as j ");
            sb.Append("PIVOT " +
                                "(Max(Qnum) FOR Survey IN (" + columns + ")) AS p ORDER BY refVarName;");

            string query = sb.ToString();

            //"SELECT * FROM " +
             //                   "(SELECT Survey, refVarName, Qnum" + addColumns +
              //                      " FROM qrySurveyQuestions WHERE Survey IN (" + filter + ")) AS j " +
               //             "PIVOT " +
                //                "(Count(Qnum) FOR Survey IN (" + columns + ")) AS p ORDER BY refVarName;"; 

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
                            DataRow r = result.Rows.Add();
                            for (int i = 0; i < result.Columns.Count;i++)
                            {
                                r[i] = rdr[i];
                            }
                            
                            
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the list of all variables in use by a list of surveys.
        /// </summary>
        /// <param name="surveyFilter"></param>
        /// <returns></returns>
        public static DataTable GetSurveysVariableListAltQnum(List<string> surveys, List<string> vars, List<string> additionalColumns, List<string> exclusions)
        {
            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn("refVarName", Type.GetType("System.String")));
            foreach (string s in additionalColumns)
                result.Columns.Add(new DataColumn(s, Type.GetType("System.String")));
            foreach (string s in surveys)
                result.Columns.Add(new DataColumn(s, Type.GetType("System.String")));


            string columns = "[" + string.Join("],[", surveys) + "]";
            string filter = "'" + string.Join("','", surveys) + "'";
            string varFilter = "'" + string.Join("','", vars) + "'";
            string addColumns = ", " + string.Join(", ", additionalColumns);
            addColumns = addColumns.TrimEnd(new char[] { ',', ' ' });
            string exclusionList = "(" + string.Join(") AND (", exclusions) + ")";

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM ");
            sb.Append("(SELECT Survey, refVarName, ALtQnum2" + addColumns + " ");

            sb.Append("FROM qrySurveyQuestions ");
            if (surveys.Count > 0 || exclusions.Count > 0 || vars.Count > 0)
            {
                exclusionList = " AND " + exclusionList;
                sb.Append("WHERE ");
            }

            if (surveys.Count > 0)
                sb.Append("Survey IN (" + filter + ")");
            if (exclusions.Count > 0)
                sb.Append(exclusionList);
            if (vars.Count > 0)
                sb.Append(" AND refVarName IN (" + varFilter + ")");

            sb.Append(") as j ");
            sb.Append("PIVOT " +
                                "(Max(AltQnum2) FOR Survey IN (" + columns + ")) AS p ORDER BY refVarName;");

            string query = sb.ToString();

            //"SELECT * FROM " +
            //                   "(SELECT Survey, refVarName, Qnum" + addColumns +
            //                      " FROM qrySurveyQuestions WHERE Survey IN (" + filter + ")) AS j " +
            //             "PIVOT " +
            //                "(Count(Qnum) FOR Survey IN (" + columns + ")) AS p ORDER BY refVarName;"; 

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
                            DataRow r = result.Rows.Add();
                            for (int i = 0; i < result.Columns.Count; i++)
                            {
                                r[i] = rdr[i];
                            }


                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the list of all variables in use by a list of waves.
        /// </summary>
        /// <param name="surveyFilter"></param>
        /// <returns></returns>
        public static DataTable GetWavesVariableList(List<string> waves, List<string> vars, List<string> additionalColumns, List<string> exclusions)
        {
            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn("refVarName", Type.GetType("System.String")));
            foreach (string s in additionalColumns)
                result.Columns.Add(new DataColumn(s, Type.GetType("System.String")));
            foreach (string s in waves)
                result.Columns.Add(new DataColumn(s, Type.GetType("System.String")));


            string columns = "[" + string.Join("],[", waves) + "]";
            string filter = "'" + string.Join("','", waves) + "'";
            string varFilter = "'" + string.Join("','", vars) + "'";
            string addColumns = ", " + string.Join(", ", additionalColumns);
            addColumns = addColumns.TrimEnd(new char[] { ',', ' ' });
            string exclusionList = "(" + string.Join(") AND (", exclusions) + ")";

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM ");
            sb.Append("(SELECT CONCAT(ISO_Code, Wave) AS StudyWave, refVarName, Qnum" + addColumns + " ");
            sb.Append("FROM qrySurveyQuestions ");
            if (waves.Count > 0 || exclusions.Count > 0 || vars.Count >0)
            {
                exclusionList = " AND " + exclusionList;
                sb.Append("WHERE ");
            }

            if (waves.Count > 0)
                sb.Append("CONCAT(ISO_Code, Wave) IN (" + filter + ")");
            if (exclusions.Count > 0)
                sb.Append(exclusionList);
            if (vars.Count > 0)
                sb.Append(" AND refVarName IN (" + varFilter + ")");

            sb.Append(") as j ");
            sb.Append("PIVOT " +
                                "(Count(Qnum) FOR StudyWave IN (" + columns + ")) AS p ORDER BY refVarName;");

            string query = sb.ToString();

            //"SELECT * FROM " +
            //                   "(SELECT Survey, refVarName, Qnum" + addColumns +
            //                      " FROM qrySurveyQuestions WHERE Survey IN (" + filter + ")) AS j " +
            //             "PIVOT " +
            //                "(Count(Qnum) FOR Survey IN (" + columns + ")) AS p ORDER BY refVarName;"; 

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
                            DataRow r = result.Rows.Add();
                            for (int i = 0; i < result.Columns.Count; i++)
                            {
                                r[i] = rdr[i];
                            }


                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the list of all variables in use by a list of surveys.
        /// </summary>
        /// <param name="surveyFilter"></param>
        /// <returns></returns>
        public static DataTable GetProductsVariableList(List<string> surveys, List<string> topics, List<string> contents, List<string> products, List<string> exclusions)
        {
            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn("Topic", Type.GetType("System.String")));
            result.Columns.Add(new DataColumn("Content", Type.GetType("System.String")));
           // result.Columns.Add(new DataColumn("#", Type.GetType("System.String")));

            foreach (string s in products)
                result.Columns.Add(new DataColumn(s, Type.GetType("System.String")));


            string columns = "[" + string.Join("],[", products) + "]";
            string filter = "'" + string.Join("','", surveys) + "'";
            string topicFilter = "'" + string.Join("','", topics) + "'";
            string contentFilter = "'" + string.Join("','", contents) + "'";
            string prodFilter = "'" + string.Join("','", products) + "'";

            string exclusionList = "(" + string.Join(") AND (", exclusions) + ")";

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM ");
            sb.Append("(SELECT Product, Topic, Content, refVarName ");

            sb.Append("FROM qrySurveyQuestions ");
            if (surveys.Count > 0 || topics.Count>0 || contents.Count>0 || exclusions.Count > 0)
            {
                exclusionList = " AND " + exclusionList;
                sb.Append("WHERE ");
            }

            if (surveys.Count > 0)
                sb.Append("Survey IN (" + filter + ")");
            if (exclusions.Count > 0)
                sb.Append(exclusionList);
            if (topics.Count > 0)
                sb.Append(" AND Topic IN (" + topicFilter + ")");
            if (contents.Count > 0)
                sb.Append(" AND Content IN (" + contentFilter + ")");
            if (products.Count > 0)
                sb.Append(" AND Product IN (" + prodFilter + ")");

            sb.Append(") as j ");
            sb.Append("PIVOT " +
                                "(Max(refVarName) FOR Product IN (" + columns + ")) AS p ORDER BY Topic;");

            string query = sb.ToString();

            //"SELECT * FROM " +
            //                   "(SELECT Survey, refVarName, Qnum" + addColumns +
            //                      " FROM qrySurveyQuestions WHERE Survey IN (" + filter + ")) AS j " +
            //             "PIVOT " +
            //                "(Count(Qnum) FOR Survey IN (" + columns + ")) AS p ORDER BY refVarName;"; 

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
                            DataRow r = result.Rows.Add();
                            for (int i = 0; i < result.Columns.Count; i++)
                            {
                                r[i] = rdr[i];
                            }


                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a list of containing all refVarNames for a list of surveys.
        /// </summary>
        /// <param name="surveys"></param>
        /// <returns></returns>
        public static List<string> GetSurveyVarNames(List<string> surveys)
        {
            List<string> refVarNames = new List<string>();

            string query = "SELECT refVarName FROM qrySurveyQuestions WHERE Survey IN ('" + string.Join("','", surveys) + "') GROUP BY refVarName";

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
                            refVarNames.Add((string)rdr["refVarName"]);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }
            return refVarNames;
        }

        public static List<VariableName> GetOrphanVarNames()
        {
            List<VariableName> orphans = new List<VariableName>();

            string query = "SELECT * FROM qryOrphanVariables ORDER BY VarName";

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
                            VariableName orphan = new VariableName();
                            orphan.VarName = rdr.SafeGetString("VarName");
                            orphan.VarLabel = rdr.SafeGetString("VarLabel");
                            orphan.Content = new ContentLabel((int)rdr["ContentNum"], "");
                            orphan.Topic = new TopicLabel((int)rdr["TopicNum"], "");
                            orphan.Domain = new DomainLabel((int)rdr["Domain"], "");
                            orphan.Product = new ProductLabel((int)rdr["ProductNum"], "");
                            orphans.Add(orphan);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }
            return orphans;
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
                names = GetPreviousNames(s.SurveyCode, q.VarName.VarName, excludeTempNames);
                
                foreach (string v in names)
                {
                    if (v != q.VarName.VarName)
                        q.PreviousNameList.Add(new VariableName(v));
                }
            }
        }
    }
}
