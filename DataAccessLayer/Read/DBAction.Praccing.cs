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
        /// <summary>
        /// Returns ID of the praccing record specified by the survey and issue number.
        /// </summary>
        /// <returns></returns>
        public static int GetPraccingID(string surveyCode, int issueNo)
        {
            int praccID = 0;
            string query = "SELECT ID FROM qryPraccingIssues WHERE Survey=@survey AND IssueNo=@issueNo";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survey", surveyCode);
                sql.SelectCommand.Parameters.AddWithValue("@issueNo", issueNo);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            praccID = (int)rdr["ID"];
                        }

                    }
                }
                catch (Exception)
                {

                }
            }


            return praccID;
        }

        /// <summary>
        /// Returns true if the praccing issue with the provided ID is resolved.
        /// </summary>
        /// <returns></returns>
        public static bool IsPraccingIssueResolved(int id)
        {
            bool resolved = false;
            string query = "SELECT Resolved FROM qryPraccingIssues WHERE ID=@id";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@id", id);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            resolved = (bool)rdr["Resolved"];
                        }

                    }
                }
                catch (Exception)
                {

                }
            }


            return resolved;
        }


        /// <summary>
        /// Returns responses to the praccing record specified by the survey and issue number.
        /// </summary>
        /// <returns></returns>
        public static List<PraccingResponse> GetPraccResponses(string surveyCode, int issueNo)
        {
            List<PraccingResponse> responses = new List<PraccingResponse>();
            string query = "SELECT R.* FROM qryPraccingResponses AS R INNER JOIN qryPraccingIssues AS I ON R.IssueID = I.ID " +
                "WHERE I.IssueNo = @issueNo AND I.Survey = @survey";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@issueNo", issueNo);
                sql.SelectCommand.Parameters.AddWithValue("@survey", surveyCode);


                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            PraccingResponse r = new PraccingResponse();
                            r.ID = (int)rdr["ID"];
                            r.IssueID = (int)rdr["IssueID"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Date")))
                                r.ResponseDate = (DateTime)rdr["Date"];
                            else
                                r.ResponseDate = null;

                            r.Response = rdr.SafeGetString("Response");
                            r.ResponseFrom = new Person(rdr.SafeGetString("ResponseFrom"), (int)rdr["From_By"]);
                            r.ResponseTo = new Person(rdr.SafeGetString("ResponseTo"), (int)rdr["To"]);

                            responses.Add(r);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }


            return responses;
        }

        /// <summary>
        /// Returns responses to the praccing record specified by the ID.
        /// </summary>
        /// <returns></returns>
        public static List<PraccingResponse> GetPraccResponses(int id)
        {
            List<PraccingResponse> responses = new List<PraccingResponse>();
            string query = "SELECT R.* FROM qryPraccingResponses AS R INNER JOIN qryPraccingIssues AS I ON R.IssueID = I.ID " +
                "WHERE I.ID = @id ORDER BY Date";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@id", id);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            PraccingResponse r = new PraccingResponse();
                            r.ID = (int)rdr["ID"];
                            r.IssueID = (int)rdr["IssueID"];
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Date")))
                                r.ResponseDate = (DateTime)rdr["Date"];
                            else
                                r.ResponseDate = null;

                            r.Response = rdr.SafeGetString("Comment").Replace("&nbsp;", " ");
                            r.ResponseFrom = new Person(rdr.SafeGetString("ResponseFrom"), (int)rdr["From_By"]);
                            r.ResponseTo = new Person(rdr.SafeGetString("ResponseTo"), (int)rdr["To"]);
                            r.Images = GetPraccResponseImages(r.ID);
                            responses.Add(r);
                        }

                    }
                }
                catch (Exception)
                {
                    Console.Write("Error getting praccing responses.");
                }
            }


            return responses;
        }

        /// <summary>
        /// Returns ID of the praccing response record specified by the survey, issue number and response text.
        /// </summary>
        /// <returns></returns>
        public static int GetPraccResponseID(string surveyCode, int issueNo, string response)
        {
            int praccID = 0;
            string query = "SELECT R.ID FROM qryPraccingResponses AS R INNER JOIN qryPraccingIssues AS I ON R.IssueID = I.ID " +
                "WHERE I.IssueNo = @issueNo AND I.Survey = @survey AND " +
                "R.Comment = @response";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@issueNo", issueNo);
                sql.SelectCommand.Parameters.AddWithValue("@survey", surveyCode);
                sql.SelectCommand.Parameters.AddWithValue("@response", response);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            praccID = (int)rdr["ID"];
                        }

                    }
                }
                catch (Exception)
                {
                    return 0;
                }
            }


            return praccID;
        }

        /// <summary>
        /// Returns the list of praccing issue categories.
        /// </summary>
        /// <returns></returns>
        public static List<PraccingCategory> GetPraccingCategories()
        {
            List<PraccingCategory> categories = new List<PraccingCategory>();
            PraccingCategory cat;
            string query = "SELECT * FROM qryIssuesCategory ORDER BY IssueType";

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
                            cat = new PraccingCategory
                            {
                                ID = (int)rdr["ID"],
                                Category = rdr.SafeGetString("IssueType")

                            };

                            categories.Add(cat);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return categories;
        }

        /// <summary>
        /// Returns the next praccing issue number for the provided survey ID.
        /// </summary>
        /// <returns></returns>
        public static int GetNextPraccingIssueNo(int survID)
        {
            int nextIssueNo = -1;

            string query = "SELECT COALESCE(MAX(IssueNo), 0) + 1 AS Next FROM qryPraccingIssues WHERE SurvID = @survID";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survID", survID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            nextIssueNo = (int)rdr["next"];
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return nextIssueNo;
        }

        /// <summary>
        /// Returns the praccing issue numbers for the provided survey.
        /// </summary>
        /// <returns></returns>
        public static List<int> GetIssueNumbers(string surveyCode)
        {
            List<int> issueNums = new List<int>();

            string query = "SELECT IssueNo FROM qryPraccingIssues WHERE Survey = @survey ORDER BY IssueNo";

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
                            issueNums.Add((int)rdr["IssueNo"]);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return issueNums;
        }

        /// <summary>
        /// Returns the praccing issue for the specified ID.
        /// </summary>
        /// <returns></returns>
        public static PraccingIssue GetPraccingIssue(int ID)
        {
            PraccingIssue issue = new PraccingIssue();

            string query = "SELECT * FROM qryPraccingIssues WHERE ID = @id";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@id", ID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            issue.ID = (int)rdr["ID"];
                            issue.Survey = new Survey(rdr.SafeGetString("Survey"));
                            issue.Survey.SID = (int)rdr["SurvID"];

                            issue.VarNames = rdr.SafeGetString("VarNames");
                            issue.Description = rdr.SafeGetString("IssueDescription");
                            if (!rdr.IsDBNull(rdr.GetOrdinal("IssueDate")))
                                issue.IssueDate = (DateTime)rdr["IssueDate"];

                            issue.IssueFrom = new Person(rdr.SafeGetString("IssueFrom"), (int)rdr["By"]);
                            issue.IssueTo = new Person(rdr.SafeGetString("IssueTo"), (int)rdr["To"]);
                            if (!rdr.IsDBNull(rdr.GetOrdinal("CategoryID")))
                                issue.Category = new PraccingCategory((int)rdr["CategoryID"], rdr.SafeGetString("IssueType"));
                            else
                                issue.Category = new PraccingCategory();

                            issue.Resolved = (bool)rdr["Resolved"];

                            if (!rdr.IsDBNull(rdr.GetOrdinal("ResDate")))
                                issue.ResolvedDate = (DateTime)rdr["ResDate"];

                            if (!rdr.IsDBNull(rdr.GetOrdinal("ResInit")))
                                issue.ResolvedBy = new Person(rdr.SafeGetString("ResolvedBy"), (int)rdr["ResInit"]);

                            issue.Language = rdr.SafeGetString("Language");
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return issue;
        }

        /// <summary>
        /// Returns the praccing issues for the specified Survey ID.
        /// </summary>
        /// <returns></returns>
        public static List<PraccingIssue> GetPraccingIssues(int SurvID)
        {
            List<PraccingIssue> issues = new List<PraccingIssue>();

            string query = "SELECT * FROM qryPraccingIssues WHERE SurvID = @survid ORDER BY IssueNo";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@survid", SurvID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            PraccingIssue issue = new PraccingIssue();
                            issue.ID = (int)rdr["ID"];
                            issue.IssueNo = (int)rdr["IssueNo"];
                            issue.Survey = new Survey(rdr.SafeGetString("Survey"));
                            issue.Survey.SID = (int)rdr["SurvID"];

                            issue.VarNames = rdr.SafeGetString("VarNames");
                            issue.Description = rdr.SafeGetString("IssueDescription");
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Date")))
                                issue.IssueDate = (DateTime)rdr["Date"];

                            issue.IssueFrom = new Person(rdr.SafeGetString("IssueFrom"), (int)rdr["By"]);
                            issue.IssueTo = new Person(rdr.SafeGetString("IssueTo"), (int)rdr["To"]);
                            if (!rdr.IsDBNull(rdr.GetOrdinal("CategoryID")))
                                issue.Category = new PraccingCategory((int)rdr["CategoryID"], rdr.SafeGetString("IssueType"));
                            else
                                issue.Category = new PraccingCategory();

                            issue.Resolved = (bool)rdr["Resolved"];

                            if (!rdr.IsDBNull(rdr.GetOrdinal("ResDate")))
                                issue.ResolvedDate = (DateTime)rdr["ResDate"];

                            if (!rdr.IsDBNull(rdr.GetOrdinal("ResInit")))
                                issue.ResolvedBy = new Person(rdr.SafeGetString("ResolvedBy"), (int)rdr["ResInit"]);

                            issue.Language = rdr.SafeGetString("Language");

                            issue.EnteredBy = new Person(rdr.SafeGetString("EnteredName"), (int)rdr["EnteredBy"]);
                            issue.EnteredOn = rdr.SafeGetDate("EnteredOn");

                            issue.Responses = GetPraccResponses(issue.ID);
                            issue.Images = GetPraccingImages(issue.ID);

                            issues.Add(issue);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return issues;
        }

        /// <summary>
        /// Returns the praccing issues for the specified Survey ID.
        /// </summary>
        /// <returns></returns>
        public static List<PraccingImage> GetPraccingImages(int praccingID)
        {
            List<PraccingImage> images = new List<PraccingImage>();

            string query = "SELECT * FROM qryPraccingImages WHERE PraccID = @id";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@id", praccingID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            PraccingImage image = new PraccingImage((int)rdr["ID"], (string)rdr["ImagePath"]);
                            image.PraccID = (int)rdr["PraccID"];

                            images.Add(image);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return images;
        }

        /// <summary>
        /// Returns the response images for the specified praccing response ID.
        /// </summary>
        /// <returns></returns>
        public static List<PraccingImage> GetPraccResponseImages(int praccResponseID)
        {
            List<PraccingImage> images = new List<PraccingImage>();

            string query = "SELECT * FROM tblPraccingResponseImages WHERE PraccResponseID = @id";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand(query, conn);
                sql.SelectCommand.Parameters.AddWithValue("@id", praccResponseID);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            PraccingImage image = new PraccingImage((int)rdr["ID"], (string)rdr["ImagePath"]);
                            image.PraccID = (int)rdr["PraccResponseID"];

                            images.Add(image);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return images;
        }

        public static List<PraccingIssue> GetPraccingIssues(string survey = null, string varname = null, string commentType = null,
                DateTime? commentDateLower = null, DateTime? commentDateUpper = null, int commentAuthor = 0, string commentText = null)
        {
            List<PraccingIssue> issues = new List<PraccingIssue>();

            string query;

            query = "SELECT * FROM FN_PraccingCommentSearch(" +
                    "@survey,@varname,@LDate,@UDate,@author,@commentText,@commentType)";

            using (SqlDataAdapter sql = new SqlDataAdapter())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                conn.Open();

                sql.SelectCommand = new SqlCommand();
                sql.SelectCommand.Connection = conn;
                sql.SelectCommand.CommandType = CommandType.Text;
                sql.SelectCommand.CommandText = query;

                if (string.IsNullOrEmpty(survey))
                    sql.SelectCommand.Parameters.AddWithValue("@survey", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@survey", survey);

                if (string.IsNullOrEmpty(varname))
                    sql.SelectCommand.Parameters.AddWithValue("@varname", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@varname", varname);

                if (commentDateLower == null)
                    sql.SelectCommand.Parameters.AddWithValue("@LDate", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@LDate", commentDateLower);

                if (commentDateLower == null)
                    sql.SelectCommand.Parameters.AddWithValue("@UDate", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@UDate", commentDateLower);

                if (commentAuthor == 0)
                    sql.SelectCommand.Parameters.AddWithValue("@author", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@author", commentAuthor);

                if (string.IsNullOrEmpty(commentText))
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@commentText", commentText);

                if (string.IsNullOrEmpty(commentType))
                    sql.SelectCommand.Parameters.AddWithValue("@commentType", DBNull.Value);
                else
                    sql.SelectCommand.Parameters.AddWithValue("@commentType", commentType);

                try
                {
                    using (SqlDataReader rdr = sql.SelectCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            PraccingIssue issue = new PraccingIssue();
                            issue.ID = (int)rdr["ID"];
                            issue.IssueNo = (int)rdr["IssueNo"];
                            issue.Survey = new Survey(rdr.SafeGetString("Survey"));
                            issue.Survey.SID = (int)rdr["SurvID"];

                            issue.VarNames = rdr.SafeGetString("VarNames");
                            issue.Description = rdr.SafeGetString("IssueDescription");
                            if (!rdr.IsDBNull(rdr.GetOrdinal("Date")))
                                issue.IssueDate = (DateTime)rdr["Date"];

                            issue.IssueFrom = new Person(rdr.SafeGetString("IssueFrom"), (int)rdr["By"]);
                            issue.IssueTo = new Person(rdr.SafeGetString("IssueTo"), (int)rdr["To"]);
                            if (!rdr.IsDBNull(rdr.GetOrdinal("CategoryID")))
                                issue.Category = new PraccingCategory((int)rdr["CategoryID"], rdr.SafeGetString("IssueType"));
                            else
                                issue.Category = new PraccingCategory();

                            issue.Resolved = (bool)rdr["Resolved"];

                            if (!rdr.IsDBNull(rdr.GetOrdinal("ResDate")))
                                issue.ResolvedDate = (DateTime)rdr["ResDate"];

                            if (!rdr.IsDBNull(rdr.GetOrdinal("ResInit")))
                                issue.ResolvedBy = new Person(rdr.SafeGetString("ResolvedBy"), (int)rdr["ResInit"]);

                            issue.Language = rdr.SafeGetString("Language");

                            issue.Responses = GetPraccResponses(issue.ID);
                            issue.Images = GetPraccingImages(issue.ID);

                            issues.Add(issue);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return issues;
        }

    }
}
