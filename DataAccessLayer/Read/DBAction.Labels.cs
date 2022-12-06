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

            string sql = "SELECT ID, Domain AS LabelText, Uses FROM Labels.FN_ListCountDomainLabels() ORDER BY Domain";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                domains = db.Query<DomainLabel>(sql).ToList();
            }

            return domains;
        }

        //
        // Topic Label
        //

        /// <summary>
        /// Returns the list of Topic Labels in the database.
        /// </summary>
        /// <returns></returns>
        public static List<TopicLabel> ListTopicLabels()
        {
            List<TopicLabel> topics = new List<TopicLabel>();
            
            string sql = "SELECT ID, Topic AS LabelText, Uses FROM Labels.FN_ListCountTopicLabels() ORDER BY Topic";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                topics = db.Query<TopicLabel>(sql).ToList();
            }

            return topics;
        }


        //
        // Content Label
        //

        

        /// <summary>
        /// Returns the list of Content Labels in the database.
        /// </summary>
        /// <returns></returns>
        public static List<ContentLabel> ListContentLabels()
        {
            List<ContentLabel> contents = new List<ContentLabel>();
            string sql = "SELECT ID, Content AS LabelText, Uses FROM Labels.FN_ListCountContentLabels() ORDER BY Content";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                contents = db.Query<ContentLabel>(sql).ToList();
            }

            return contents;
        }

        //
        // Product Label
        //

        /// <summary>
        /// Returns the list of Product Labels in the database.
        /// </summary>
        /// <returns></returns>
        public static List<ProductLabel> ListProductLabels()
        {
            List<ProductLabel> products = new List<ProductLabel>();

            string sql = "SELECT ID, Product AS LabelText, Uses FROM Labels.FN_ListCountProductLabels() ORDER BY Product";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                products = db.Query<ProductLabel>(sql).ToList();
            }

            return products;
        }

        /// <summary>
        /// Returns the list of Product Labels used by a particular survey.
        /// </summary>
        /// <param name="surveyFilter"></param>
        /// <returns></returns>
        public static List<ProductLabel> GetProductLabels(string surveyFilter)
        {
            List<ProductLabel> products = new List<ProductLabel>();
           
            string sql = "SELECT ID, Product AS LabelText FROM Labels.FN_ListProductLabelsBySurvey(@survey) ORDER BY Product";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                products = db.Query<ProductLabel>(sql).ToList();
            }

            return products;
        }

        /// <summary>
        /// Returns the number of uses for a specific Keyword.
        /// </summary>
        /// <param name="KeywordID"></param>
        /// <returns></returns>
        public static int CountKeywordUses(int KeywordID)
        {
            int count;
            var parameters = new { keywordID = KeywordID };
            string sql = "SELECT Labels.FN_CountKeywordUses(@keywordID)";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                count = db.ExecuteScalar<int>(sql, parameters);
            }

            return count;
        }

        /// <summary>
        /// Returns the list of Keywords in the database.
        /// </summary>
        /// <returns></returns>
        public static List<Keyword> GetKeywords()
        {
            List<Keyword> keywords = new List<Keyword>();
            
            string sql = "SELECT ID, Keyword AS LabelText FROM Labels.FN_ListKeywords() ORDER BY Keyword";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                keywords = db.Query<Keyword>(sql).ToList();
            }

            return keywords;
        }
    }
}
