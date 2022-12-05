using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ITCLib;

namespace DataAccessLayer._OLD_ORM
{
    public static class DBActionOLD
    {
        /// <summary>
        /// Returns the list of Domain Labels in the database.
        /// </summary>
        /// <returns></returns>
        public static List<DomainLabel> ListDomainLabels_OLD()
        {
            List<DomainLabel> domains = new List<DomainLabel>();

            string query = "SELECT * FROM Labels.FN_ListCountDomainLabels() ORDER BY Domain";

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
                            DomainLabel d;
                            d = new DomainLabel((int)rdr["ID"], rdr.SafeGetString("Domain"));
                            d.Uses = (int)rdr["Uses"];

                            domains.Add(d);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return domains;
        }

        /// <summary>
        /// Returns the list of Topic Labels in the database.
        /// </summary>
        /// <returns></returns>
        public static List<TopicLabel> ListTopicLabels_OLD()
        {
            List<TopicLabel> topics = new List<TopicLabel>();
            TopicLabel t;
            string query = "SELECT * FROM Labels.FN_ListCountTopicLabels() ORDER BY Topic";

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
                            t = new TopicLabel((int)rdr["ID"], (string)rdr["Topic"]);
                            t.Uses = (int)rdr["Uses"];

                            topics.Add(t);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return topics;
        }

        /// <summary>
        /// Returns the list of Content Labels in the database.
        /// </summary>
        /// <returns></returns>
        public static List<ContentLabel> ListContentLabels_OLD()
        {
            List<ContentLabel> contents = new List<ContentLabel>();
            ContentLabel c;
            string query = "SELECT * FROM Labels.FN_ListCountContentLabels() ORDER BY Content";

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
                            c = new ContentLabel((int)rdr["ID"], (string)rdr["Content"]);
                            c.Uses = (int)rdr["Uses"];

                            contents.Add(c);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return contents;
        }

        /// <summary>
        /// Returns the list of Product Labels in the database.
        /// </summary>
        /// <returns></returns>
        public static List<ProductLabel> ListProductLabels_OLD()
        {
            List<ProductLabel> products = new List<ProductLabel>();
            ProductLabel p;
            string query = "SELECT * FROM Labels.FN_ListCountProductLabels() ORDER BY Product";

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
                            p = new ProductLabel((int)rdr["ID"], (string)rdr["Product"]);
                            p.Uses = (int)rdr["Uses"];

                            products.Add(p);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return products;
        }

        /// <summary>
        /// Returns the list of Product Labels in the database.
        /// </summary>
        /// <returns></returns>
        public static List<ProductLabel> GetProductLabels_OLD(string surveyFilter)
        {
            List<ProductLabel> products = new List<ProductLabel>();
            ProductLabel t;
            string query = "SELECT * FROM Labels.FN_ListProductLabelsBySurvey(@survey) ORDER BY Product";

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
                            t = new ProductLabel((int)rdr["ID"], (string)rdr["Product"]);

                            products.Add(t);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return products;
        }

        /// <summary>
        /// Returns the list of Topic Labels in the database.
        /// </summary>
        /// <returns></returns>
        public static List<Keyword> GetKeywords_OLD()
        {
            List<Keyword> topics = new List<Keyword>();
            Keyword t;
            string query = "SELECT * FROM Labels.FN_ListKeywords() ORDER BY Keyword";

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
                            t = new Keyword((int)rdr["ID"], (string)rdr["Keyword"]);

                            topics.Add(t);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return topics;
        }

        /// <summary>
        /// Returns the number of uses for a specific Product Label.
        /// </summary>
        /// <param name="DomainID"></param>
        /// <returns></returns>
        public static int CountKeywordUses_OLD(int KeywordID)
        {

            int count;
            string query = "SELECT Labels.FN_CountKeywordUses(@keywordID)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@keywordID", KeywordID);

                try
                {
                    count = (int)sql.SelectCommand.ExecuteScalar();

                }
                catch (Exception)
                {
                    count = 0;
                }
            }

            return count;
        }
    }
}
