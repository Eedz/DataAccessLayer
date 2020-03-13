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
    public static partial class DBAction
    {
        //
        // Domain Label
        //

        /// <summary>
        /// Returns the list of Domain Labels in the database.
        /// </summary>
        /// <returns></returns>
        public static List<DomainLabel> ListDomainLabels()
        {
            List<DomainLabel> domains = new List<DomainLabel>();
            DomainLabel d;
            string query = "SELECT * FROM Labels.FN_ListDomainLabels() ORDER BY Domain";

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
                            d = new DomainLabel ((int)rdr["ID"], (string)rdr["Domain"]);

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
        /// Returns the number of uses for a specific Domain Label.
        /// </summary>
        /// <param name="DomainID"></param>
        /// <returns></returns>
        public static int CountDomainLabelsUses(int DomainID)
        {

            int count;
            string query = "SELECT Labels.FN_CountDomainUses(@domainID)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@domainID", DomainID);

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

        //
        // Topic Label
        //

        /// <summary>
        /// Returns the list of Topic Labels in the database.
        /// </summary>
        /// <returns></returns>
        public static List<TopicLabel> GetTopicLabels()
        {
            List<TopicLabel> topics = new List<TopicLabel>();
            TopicLabel t;
            string query = "SELECT * FROM Labels.FN_ListTopicLabels() ORDER BY Topic";

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
                            t = new TopicLabel ( (int)rdr["ID"], (string)rdr["Topic"]);

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
        /// Returns the number of uses for a specific Topic Label.
        /// </summary>
        /// <param name="DomainID"></param>
        /// <returns></returns>
        public static int CountTopicLabelsUses(int TopicID)
        {

            int count;
            string query = "SELECT Labels.FN_CountTopicUses(@topicID)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@topicID", TopicID);

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


        //
        // Content Label
        //

        /// <summary>
        /// Returns the list of Content Labels in the database.
        /// </summary>
        /// <returns></returns>
        public static List<ContentLabel> GetContentLabels()
        {
            List<ContentLabel> contents = new List<ContentLabel>();
            ContentLabel c;
            string query = "SELECT * FROM Labels.FN_ListContentLabels() ORDER BY Content";

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
        /// Returns the number of uses for a specific Content Label.
        /// </summary>
        /// <param name="DomainID"></param>
        /// <returns></returns>
        public static int CountContentLabelsUses(int ContentID)
        {

            int count;
            string query = "SELECT Labels.FN_CountContentUses(@contentID)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@contentID", ContentID);

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

        //
        // Product Label

        //
        /// <summary>
        /// Returns the list of Product Labels in the database.
        /// </summary>
        /// <returns></returns>
        public static List<ProductLabel> GetProductLabels()
        {
            List<ProductLabel> products = new List<ProductLabel>();
            ProductLabel t;
            string query = "SELECT * FROM Labels.FN_ListProductLabels() ORDER BY Product";

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
                            t = new ProductLabel ((int)rdr["ID"],(string)rdr["Product"]);

                            products.Add(t);
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
        /// Returns the number of uses for a specific Product Label.
        /// </summary>
        /// <param name="DomainID"></param>
        /// <returns></returns>
        public static int CountProductLabelsUses(int ProductID)
        {

            int count;
            string query = "SELECT Labels.FN_CountProductUses(@productID)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@productID", ProductID);

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
