using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
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

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                categories = db.Query<PraccingCategory>(query).ToList();
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

            using (IDbConnection db = new SqlConnection(connectionString))
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

            using (IDbConnection db = new SqlConnection(connectionString))
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

            string query = "SELECT ID, IssueNo, VarNames, IssueDescription AS Description, Date AS IssueDate, Resolved, ResDate AS ResolvedDate, Language, EnteredOn, PinNo, " +
                    "SurvID, SurvID AS SID, Survey AS SurveyCode, " +
                    "[By], [By] AS ID, IssueFrom AS Name, " +
                    "[To], [To] AS ID, IssueTo AS Name, " +
                    "CategoryID, CategoryID AS ID, IssueType As Category, " +
                    "ResInit, ResInit AS ID, ResolvedBy AS Name, " +
                    "EnteredBy, EnteredBy AS ID, EnteredName AS Name " +
                    "FROM qryPraccingIssues WHERE ID = @id ORDER BY IssueNo;" +
                "SELECT M.ID, PraccID, ImagePath AS Path FROM tblPraccingImages AS M INNER JOIN qryPraccingIssues AS I ON M.PraccID =I.ID WHERE I.ID = @id;" +
                "SELECT R.ID, R.IssueID, R.Date AS ResponseDate, R.Comment AS Response, PinNo, " +
                    "R.From_By, R.From_By AS ID, R.ResponseFrom AS Name, " +
                    "R.[To], R.[To] AS ID, ResponseTo AS Name FROM qryPraccingResponses AS R INNER JOIN qryPraccingIssues AS I ON R.IssueID =I.ID WHERE I.ID = @id " +
                    "ORDER BY ResponseDate;" +
                "SELECT M.ID, PraccResponseID AS PraccID, ImagePath AS Path FROM tblPraccingResponseImages AS M " +
                    "INNER JOIN qryPraccingResponses AS R ON R.ID = M.PraccResponseID " +
                    "INNER JOIN qryPraccingIssues AS I ON R.IssueID =I.ID WHERE I.ID = @id;";

            using (IDbConnection db = new SqlConnection(connectionString))
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

            string query = "SELECT ID, IssueNo, VarNames, IssueDescription AS Description, Date AS IssueDate, Resolved, ResDate AS ResolvedDate, Language, EnteredOn, PinNo, " +
                    "SurvID, SurvID AS SID, Survey AS SurveyCode, " +
                    "[By], [By] AS ID, IssueFrom AS Name, " +
                    "[To], [To] AS ID, IssueTo AS Name, " +
                    "CategoryID, CategoryID AS ID, IssueType As Category, " + 
                    "ResInit, ResInit AS ID, ResolvedBy AS Name, " +
                    "EnteredBy, EnteredBy AS ID, EnteredName AS Name " +
                    "FROM qryPraccingIssues WHERE SurvID = @survid ORDER BY IssueNo;" +
                "SELECT M.ID, PraccID, ImagePath AS Path FROM tblPraccingImages AS M INNER JOIN qryPraccingIssues AS I ON M.PraccID =I.ID WHERE I.SurvID = @survid;" +
                "SELECT R.ID, R.IssueID, R.Date AS ResponseDate, R.Comment AS Response, R.PinNo, " + 
                    "R.From_By, R.From_By AS ID, R.ResponseFrom AS Name, " +
                    "R.[To], R.[To] AS ID, ResponseTo AS Name FROM qryPraccingResponses AS R INNER JOIN qryPraccingIssues AS I ON R.IssueID =I.ID WHERE SurvID = @survid " +
                    "ORDER BY ResponseDate;" +
                "SELECT M.ID, PraccResponseID AS PraccID, ImagePath AS Path FROM tblPraccingResponseImages AS M " +
                    "INNER JOIN qryPraccingResponses AS R ON R.ID = M.PraccResponseID " +
                    "INNER JOIN qryPraccingIssues AS I ON R.IssueID =I.ID WHERE I.SurvID = @survid;";

            using (IDbConnection db = new SqlConnection(connectionString))
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
                    image.SetSize(image.Path);
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
                    image.SetSize(image.Path);

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

            using (IDbConnection db = new SqlConnection(connectionString))
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

            using (IDbConnection db = new SqlConnection(connectionString))
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

            string sql = "SELECT ID, IssueNo, VarNames, IssueDescription AS Description, Date AS IssueDate, Resolved, ResDate AS ResolvedDate, LastUpdate, Language, Fixed,Notify, EnteredOn, PinNo,  " +
                    "SurvID, SurvID AS SID, Survey AS SurveyCode, " +
                    "[By], [By] AS ID, IssueFrom AS Name, " +
                    "[To], [To] AS ID, [IssueTo] AS Name, " +
                    "CategoryID, CategoryID AS ID, IssueType AS Category, " +
                    "ResInit, ResInit AS ID, ResolvedBy AS Name, " +
                    "EnteredBy, EnteredBy AS ID, EnteredName AS Name " +
                    "FROM FN_PraccingCommentSearch(@survey,@varname,@LDate,@UDate,@author,@commentText,@commentType);";
                

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@survey", survey);
            parameters.Add("@varname", varname);
            parameters.Add("@LDate", commentDateLower);
            parameters.Add("@UDate", commentDateUpper);
            parameters.AddNullableParameter("@author", commentAuthor);
            parameters.Add("@commentText", commentText);
            parameters.Add("@commentType", commentType);

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                
                issues = db.Query<PraccingIssue, Survey, Person, Person, PraccingCategory, Person, Person, PraccingIssue>(sql, (issue, surv, by, to, category, resolved, entered) =>
                {
                    issue.Survey = surv;
                    issue.IssueFrom = by;
                    issue.IssueTo = to;
                    issue.Category = category;
                    issue.ResolvedBy = resolved;
                    issue.EnteredBy = entered;
                    return issue;
                }, parameters, splitOn: "SurvID, By, To, CategoryID, ResInit, EnteredBy").ToList();
            }

            return issues;
        }

    }
}
