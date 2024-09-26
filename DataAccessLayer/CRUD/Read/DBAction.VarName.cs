using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;

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
        public static List<RefVariableName> GetAllRefVarNames()
        {
            List<RefVariableName> refVarNames = new List<RefVariableName>();
            string sql = "SELECT refVarName FROM qryVariableInfo GROUP BY refVarName ORDER BY refVarName";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                refVarNames = db.Query<RefVariableName>(sql).ToList(); 
            }
            return refVarNames;
        }

        /// <summary>
        /// Returns a list containing every Survey VarName.
        /// </summary>
        /// <returns></returns>
        public static List<VariableName> GetAllVarNames()
        {
            List<VariableName> varnames = new List<VariableName>();

            string sql = "SELECT VarName, refVarName, VarLabel, " +
                "DomainNum, DomainNum AS ID, Domain AS LabelText, " +
                "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                "ProductNum, ProductNum AS ID, Product AS LabelText " +
                "FROM qryVariableInfo ORDER BY refVarName";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                varnames = db.Query<VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, VariableName>(
                    sql,
                    (varname, domain, topic, content, product)=>
                    {
                        varname.Domain = domain;
                        varname.Topic = topic;
                        varname.Content = content;
                        varname.Product = product;
                        return varname;
                    },
                    splitOn: "DomainNum, TopicNum, ContentNum, ProductNum").ToList();
            }
            return varnames;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<CanonicalRefVarName> GetAllCanonVars()
        {
            List<CanonicalRefVarName> canonVars = new List<CanonicalRefVarName>();

            string sql = "SELECT ID, VarName as RefVarName, AnySuffix, Notes, Active FROM qryEssentialQuestions ORDER BY VarName";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                canonVars = db.Query<CanonicalRefVarName>(sql).ToList();
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
            string sql = "SELECT VarName, refVarName, VarLabel, "+
                "DomainNum, DomainNum AS ID, Domain AS LabelText, " +
                "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                "ProductNum, ProductNum AS ID, Product AS LabelText " +
                "FROM VarNames.FN_GetVarName(@varname)";
            var parameters = new { varname };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var results = db.Query<VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, VariableName>(
                    sql, 
                    (varName, domain, topic, content, product) =>
                    {
                        varName.Domain = domain;
                        varName.Topic = topic;
                        varName.Content = content;
                        varName.Product = product;
                        return varName;
                    }, 
                    parameters,
                    splitOn: "DomainNum, TopicNum, ContentNum, ProductNum").ToList();

                if (results.Count > 0)
                    v = results[0];
                else v = new VariableName();
            }
            return v;
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
            List<VariableName> varnames = new List<VariableName>();
            
            string sql = "SELECT VarName, refVarName, VarLabel, " +
                "DomainNum, DomainNum AS ID, Domain AS LabelText, " +
                "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                "ProductNum, ProductNum AS ID, Product AS LabelText " +
                "FROM VarNames.FN_GetVarNamesByRef(@refVarName)";

            var parameters = new { refVarName };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                varnames = db.Query<VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, VariableName>(
                    sql,
                    (varname, domain, topic, content, product) =>
                    {
                        varname.Domain = domain;
                        varname.Topic = topic;
                        varname.Content = content;
                        varname.Product = product;
                        return varname;
                    },
                    parameters,
                    splitOn: "DomainNum, TopicNum, ContentNum, ProductNum").ToList();
            }
            return varnames;
        }

        /// <summary>
        /// Returns a list of VariableNameSurveys objects with the provided refVarName.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        async public static Task<List<VariableNameSurveys>> GetVarNamesPrefixAsync(string prefix)
        {
            List<VariableNameSurveys> refVarNames = new List<VariableNameSurveys>();

            string query = "SELECT refVarName, VarName, VarLabel, " +
                            "STUFF((SELECT  ',' + Survey FROM qrySurveyQuestions SQ2 WHERE VarName = sq1.VarName GROUP BY SQ2.Survey ORDER BY Survey " +
                                "FOR XML PATH(''), TYPE).value('text()[1]', 'nvarchar(max)') ,1, LEN(','), '') AS SurveyList, " +
                            "DomainNum, DomainNum AS ID, Domain AS LabelText, " +
                            "TopicNum, TopicNum as ID, Topic AS LabelText, " +
                            "ContentNum, ContentNum AS ID, Content AS LabelText, " + 
                            "ProductNum, ProductNum AS ID, Product AS LabelText " +
                            "FROM qrySurveyQuestions Sq1 " +
                            "WHERE SUBSTRING(refVarName,1,2) = @prefix " +
                            "GROUP BY sq1.refVarName, VarName, Sq1.VarLabel, Sq1.Domain, DomainNum, Sq1.Content, ContentNum, Sq1.Topic, TopicNum, Sq1.Product, ProductNum " +
                            "ORDER BY refVarName";

            var parameters = new { prefix };

            using (IDbConnection db = new SqlConnection(connectionString))
            {

                var results = await db.QueryAsync<VariableNameSurveys, DomainLabel, TopicLabel, ContentLabel, ProductLabel, VariableNameSurveys>(
                    query,
                    (list, domain, topic, content, product) =>
                    {
                        list.Domain = domain;
                        list.Topic = topic;
                        list.Content = content;
                        list.Product = product;
                        return list;
                    },
                    parameters,
                    splitOn: "DomainNum, TopicNum, ContentNum, ProductNum");

                refVarNames = results.ToList();
            }

            return refVarNames;
        }

        /// <summary>
        /// Returns the list of VariableNames in the database with their usages.
        /// </summary>
        /// <returns></returns>
        async public static Task<List<VariableNameSurveys>> GetVarNameUsageAsync()
        {
            List<VariableNameSurveys> refVarNames = new List<VariableNameSurveys>();

            string query = "SELECT refVarName, VarName, VarLabel, " +
                                "STUFF((SELECT  ',' + Survey FROM qrySurveyQuestions SQ2 WHERE VarName = sq1.VarName GROUP BY SQ2.Survey ORDER BY Survey " +
                                "FOR XML PATH(''), TYPE).value('text()[1]', 'nvarchar(max)') ,1, LEN(','), '') AS SurveyList, " +
                            "DomainNum, DomainNum AS ID, Domain AS LabelText, " + 
                            "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                            "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                            "ProductNum, ProductNum AS ID, Product AS LabelText " +
                            "FROM qrySurveyQuestions Sq1 " +
                             "GROUP BY sq1.refVarName, VarName, Sq1.VarLabel, Sq1.Domain, DomainNum, Sq1.Content, ContentNum, Sq1.Topic, TopicNum, Sq1.Product, ProductNum " +
                            "ORDER BY refVarName";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var results = await db.QueryAsync<VariableNameSurveys, DomainLabel, TopicLabel, ContentLabel, ProductLabel, VariableNameSurveys>(
                    query, 
                    (varname, domain, topic, content, product) => 
                    {
                        varname.Domain = domain;
                        varname.Topic = topic;
                        varname.Content = content;
                        varname.Product = product;
                        return varname; 
                    }, 
                    splitOn: "DomainNum, TopicNum, ContentNum, ProductNum"
                );
                refVarNames = results.ToList();
            }

            return refVarNames;
        }

        /// <summary>
        /// Returns the list of VariableNames in the database with their usages.
        /// </summary>
        /// <param name="startsWith"></param>
        /// <returns></returns>
        async public static Task<List<VariableNameSurveys>> GetVarNameUsageAsync(string startsWith)
        {
            List<VariableNameSurveys> refVarNames = new List<VariableNameSurveys>();

            string query = "SELECT refVarName, VarName, VarLabel, " +
                                "STUFF((SELECT  ',' + Survey FROM qrySurveyQuestions SQ2 WHERE VarName = sq1.VarName GROUP BY SQ2.Survey ORDER BY Survey " +
                                "FOR XML PATH(''), TYPE).value('text()[1]', 'nvarchar(max)') ,1, LEN(','), '') AS SurveyList, " +
                            "DomainNum, DomainNum AS ID, Domain AS LabelText, " +
                            "TopicNum, TopicNum AS ID, Topic AS LabelText, " +
                            "ContentNum, ContentNum AS ID, Content AS LabelText, " +
                            "ProductNum, ProductNum AS ID, Product AS LabelText " +
                            "FROM qrySurveyQuestions Sq1 " +
                            "WHERE sq1.refVarName LIKE @startsWith + '%' " +
                             "GROUP BY sq1.refVarName, VarName, Sq1.VarLabel, Sq1.Domain, DomainNum, Sq1.Content, ContentNum, Sq1.Topic, TopicNum, Sq1.Product, ProductNum " +
                            "ORDER BY refVarName";

            var parameters = new { startsWith };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var results = await db.QueryAsync<VariableNameSurveys, DomainLabel, TopicLabel, ContentLabel, ProductLabel, VariableNameSurveys>(
                    query,
                    (varname, domain, topic, content, product) =>
                    {
                        varname.Domain = domain;
                        varname.Topic = topic;
                        varname.Content = content;
                        varname.Product = product;
                        return varname;
                    }, parameters,
                    splitOn: "DomainNum, TopicNum, ContentNum, ProductNum"
                );
                refVarNames = results.ToList();
            }

            return refVarNames;
        }

        /// <summary>
        /// Gets the list of previous VarNames for a provided survey and varname.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="varname"></param>
        /// <param name="excludeTempNames"></param>
        /// <returns></returns>
        public static List<string> GetPreviousNames(string survey, string varname, bool excludeTemp)
        {
            List<string> varlist = new List<string>();
            string query = "SELECT dbo.FN_VarNamePreviousNames(@varname, @survey, @excludeTemp)";

            var parameters = new { varname, survey, excludeTemp };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    string list = db.ExecuteScalar<string>(query, parameters);
                    varlist.AddRange(list.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries));
                }
                catch
                {

                }
            }

            return varlist;
        }

        /// <summary>
        /// Gets the current name of a variable and survey.
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="varname"></param>
        /// <param name="dateSince"></param>
        /// <returns></returns>
        public static string GetCurrentName(string survey, string varname, DateTime dateSince)
        {
            string currentName = string.Empty;
            string query = "SELECT dbo.FN_GetCurrentName(@varname, @survey, @date)";
            var parameters = new { varname, survey, date = dateSince };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    currentName = db.ExecuteScalar<string>(query, parameters);
                }
                catch
                {

                }
            }
            return currentName;
        }

        /// <summary>
        /// Returns a DataTable containing VarNames and which surveys they appear in.
        /// </summary>
        /// <param name="surveys"></param>
        /// <param name="vars"></param>
        /// <param name="additionalColumns"></param>
        /// <param name="exclusions"></param>
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
        /// Returns a DataTable containing VarNames and which surveys they appear in.
        /// </summary>
        /// <param name="surveys"></param>
        /// <param name="vars"></param>
        /// <param name="additionalColumns"></param>
        /// <param name="exclusions"></param>
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
        /// Returns a DataTable containing VarNames and which waves they appear in.
        /// </summary>
        /// <param name="waves"></param>
        /// <param name="vars"></param>
        /// <param name="additionalColumns"></param>
        /// <param name="exclusions"></param>
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
        /// Returns a DataTable containing VarNames and which surveys they appear in.
        /// </summary>
        /// <param name="surveys"></param>
        /// <param name="topics"></param>
        /// <param name="contents"></param>
        /// <param name="products"></param>
        /// <param name="exclusions"></param>
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

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                refVarNames = db.Query<string>(query).ToList();
            }
            return refVarNames;
        }

        public static List<VariableName> GetOrphanVarNames()
        {
            List<VariableName> orphans = new List<VariableName>();

            string sql = "SELECT VarName, refVarName, VarLabel, " +
                "DomainNum, DomainNum AS ID, Domain AS LabelText, " +
                "TopicNum, TopicNum AS ID, Topic AS LabelText, " + 
                "ContentNum, ContentNum AS ID, Content AS LabelText, " + 
                "ProductNum, ProductNum AS ID, Product AS LabelText " +
                "FROM qryOrphanVariables ORDER BY VarName;";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                orphans = db.Query<VariableName, DomainLabel, TopicLabel, ContentLabel, ProductLabel, VariableName>(
                    sql,
                    (varname, domain, topic, content, product) =>
                    {
                        varname.Domain = domain;
                        varname.Topic = topic;
                        varname.Content = content;
                        varname.Product = product;
                        return varname;
                    },
                    splitOn: "DomainNum, TopicNum, ContentNum, ProductNum").ToList();
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