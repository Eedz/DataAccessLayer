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

        /// <summary>
        /// Returns the list of praccing issue categories.
        /// </summary>
        /// <returns></returns>
        public static List<PraccingCategory> GetPraccingCategories()
        {
            List<PraccingCategory> categories = new List<PraccingCategory>();
            string query = "SELECT ID, IssueType AS Category FROM qryIssuesCategory ORDER BY IssueType";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                categories = db.Query<PraccingCategory>(query).ToList();
            }

            return categories;
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
        /// Returns the next praccing issue number for the provided survey ID.
        /// </summary>
        /// <returns></returns>
        public static int GetNextPraccingIssueNo(int survID)
        {
            int nextIssueNo = -1;
            string query = "SELECT COALESCE(MAX(IssueNo), 0) + 1 AS Next FROM qryPraccingIssues WHERE SurvID = @survID";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                var parameters = new { survID = survID };

                nextIssueNo = db.ExecuteScalar<int>(query, parameters);
            }

            return nextIssueNo;
        }

        /// <summary>
        /// Returns the praccing issue numbers for the provided survey.
        /// </summary>
        /// <returns></returns>
        public static List<int> GetIssueNumbers(string surveyCode)
        {
            List<int> issueNums;
            string query = "SELECT IssueNo FROM qryPraccingIssues WHERE Survey = @survey ORDER BY IssueNo";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                var parameters = new { survey = surveyCode };

                issueNums = db.Query<int>(query, parameters).ToList();
            }

            return issueNums;
        }

        /// <summary>
        /// Returns the praccing issue for the specified ID.
        /// </summary>
        /// <returns></returns>
        public static PraccingIssue GetPraccingIssue(int ID)
        {
            PraccingIssue issue;

            string query = "SELECT ID, IssueNo, VarNames, IssueDescription AS Description, Date AS IssueDate, Resolved, ResDate AS ResolvedDate, Language, EnteredOn " +
                    "SurvID, SurvID AS SID, Survey AS SurveyCode, " +
                    "[By], [By] AS ID, IssueFrom AS Name, " +
                    "[To], [To] AS ID, IssueTo AS Name, " +
                    "CategoryID, CategoryID AS ID, IssueType As Category, " +
                    "ResInit, ResInit AS ID, ResolvedBy AS Name, " +
                    "EnteredBy, EnteredBy AS ID, EnteredName AS Name " +
                    "FROM qryPraccingIssues WHERE ID = @id ORDER BY IssueNo;" +
                "SELECT M.ID, PraccID, ImagePath AS Path FROM tblPraccingImages AS M INNER JOIN qryPraccingIssues AS I ON M.PraccID =I.ID WHERE I.ID = @id;" +
                "SELECT R.ID, R.IssueID, R.Date AS ResponseDate, R.Comment AS Response, " +
                    "R.From_By, R.From_By AS ID, R.ResponseFrom AS Name, " +
                    "R.[To], R.[To] AS ID, ResponseTo AS Name FROM qryPraccingResponses AS R INNER JOIN qryPraccingIssues AS I ON R.IssueID =I.ID WHERE I.ID = @id " +
                    "ORDER BY ResponseDate;" +
                "SELECT M.ID, PraccResponseID AS PraccID, ImagePath AS Path FROM tblPraccingResponseImages AS M " +
                    "INNER JOIN qryPraccingResponses AS R ON R.ID = M.PraccResponseID " +
                    "INNER JOIN qryPraccingIssues AS I ON R.IssueID =I.ID WHERE I.ID = @id;";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                var parameters = new { id = ID };

                var results = db.QueryMultiple(query, parameters);

                issue = results.Read<PraccingIssue, Survey, Person, Person, PraccingCategory, Person, Person, PraccingIssue>(
                    (record, survey, from, to, category, resolvedby, enteredby) =>
                    {
                        record.Survey = survey;
                        record.IssueFrom = from;
                        record.IssueTo = to;
                        record.Category = category;
                        record.ResolvedBy = resolvedby;
                        record.EnteredBy = enteredby;
                        return record;
                    },
                    splitOn: "SurvID, By, To, CategoryID, ResInit, EnteredBy").FirstOrDefault();

                if (issue == null)
                    return null;

                var images = results.Read<PraccingImage>();

                issue.Images.AddRange(images);

                var responses = results.Read<PraccingResponse, Person, Person, PraccingResponse>((response, from, to) =>
                {
                    response.ResponseFrom = from;
                    response.ResponseTo = to;
                    return response;
                }, splitOn: "From_By, To").ToList();

                var responseImages = results.Read<PraccingImage>();

                foreach (var image in responseImages)
                {
                    PraccingResponse response = responses.Where(x => x.ID == image.PraccID).FirstOrDefault();
                    response.Images.Add(image);
                }

                issue.Responses.AddRange(responses);
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

            string query = "SELECT ID, IssueNo, VarNames, IssueDescription AS Description, Date AS IssueDate, Resolved, ResDate AS ResolvedDate, Language, EnteredOn " +
                    "SurvID, SurvID AS SID, Survey AS SurveyCode, " +
                    "[By], [By] AS ID, IssueFrom AS Name, " +
                    "[To], [To] AS ID, IssueTo AS Name, " +
                    "CategoryID, CategoryID AS ID, IssueType As Category, " + 
                    "ResInit, ResInit AS ID, ResolvedBy AS Name, " +
                    "EnteredBy, EnteredBy AS ID, EnteredName AS Name " +
                    "FROM qryPraccingIssues WHERE SurvID = @survid ORDER BY IssueNo;" +
                "SELECT M.ID, PraccID, ImagePath AS Path FROM tblPraccingImages AS M INNER JOIN qryPraccingIssues AS I ON M.PraccID =I.ID WHERE I.SurvID = @survid;" +
                "SELECT R.ID, R.IssueID, R.Date AS ResponseDate, R.Comment AS Response, " + 
                    "R.From_By, R.From_By AS ID, R.ResponseFrom AS Name, " +
                    "R.[To], R.[To] AS ID, ResponseTo AS Name FROM qryPraccingResponses AS R INNER JOIN qryPraccingIssues AS I ON R.IssueID =I.ID WHERE SurvID = @survid " +
                    "ORDER BY ResponseDate;" +
                "SELECT M.ID, PraccResponseID AS PraccID, ImagePath AS Path FROM tblPraccingResponseImages AS M " +
                    "INNER JOIN qryPraccingResponses AS R ON R.ID = M.PraccResponseID " +
                    "INNER JOIN qryPraccingIssues AS I ON R.IssueID =I.ID WHERE I.SurvID = @survid;";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                var parameters = new { survid = SurvID };

                var results = db.QueryMultiple(query, parameters);

                issues = results.Read<PraccingIssue, Survey, Person, Person, PraccingCategory, Person, Person, PraccingIssue>(
                    (issue, survey, from, to, category, resolvedby, enteredby) =>
                    {
                        issue.Survey = survey;
                        issue.IssueFrom = from;
                        issue.IssueTo = to;
                        issue.Category = category;
                        issue.ResolvedBy = resolvedby;
                        issue.EnteredBy = enteredby;
                        return issue;
                    }, 
                    splitOn: "SurvID, By, To, CategoryID, ResInit, EnteredBy").ToList();

                var images = results.Read<PraccingImage>();

                foreach(var image in images)
                {
                    PraccingIssue issue = issues.Where(x => x.ID == image.PraccID).FirstOrDefault();
                    issue.Images.Add(image);
                }

                var responses = results.Read<PraccingResponse, Person, Person, PraccingResponse>((response, from, to) =>
                {
                    response.ResponseFrom = from;
                    response.ResponseTo = to;
                    return response;
                }, splitOn: "From_By, To").ToList();

                var responseImages = results.Read<PraccingImage>();

                foreach (var image in responseImages)
                {
                    PraccingResponse response = responses.Where(x => x.ID == image.PraccID).FirstOrDefault();
                    response.Images.Add(image);
                }

                foreach (PraccingResponse response in responses)
                {
                    PraccingIssue issue = issues.Where(x => x.ID == response.IssueID).FirstOrDefault();
                    issue.Responses.Add(response);
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
            List<PraccingImage> images;

            string query = "SELECT ID, PraccID, ImagePath AS Path FROM qryPraccingImages WHERE PraccID = @id";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                var parameters = new { id = praccingID };

                images = db.Query<PraccingImage>(query, parameters).ToList();
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

            string query = "SELECT ID, PraccResponseID AS PraccID, ImagePath AS Path FROM tblPraccingResponseImages WHERE PraccResponseID = @id";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ISISConnectionString"].ConnectionString))
            {
                var parameters = new { id = praccResponseID };

                images = db.Query<PraccingImage>(query, parameters).ToList();
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
