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
        //
        // Survey Checks
        //
       
        /// <summary>
        /// Returns a list of survey check records.
        /// </summary>
        /// <returns></returns>
        public static List<SurveyCheck> GetSurveyCheckRecords()
        {
            List<SurveyCheck> records = new List<SurveyCheck>();

            string sql = "SELECT SC.ID, CheckDate, Comments, CheckInit, CheckInit AS ID, Name, SA.ID AS SurvID, SA.ID, SA.Survey, CheckTypeID, CheckTypeID AS ID, CheckType AS CheckName " +
                "FROM((tblSurveyChecks AS SC LEFT JOIN tblStudyAttributes AS SA ON SC.SurvID = SA.ID) " +
                "LEFT JOIN qryIssueInit AS P ON SC.CheckInit = P.ID) " +
                "LEFT JOIN dbo.tblSurveyCheckTypes AS CT ON SC.CheckTypeID = CT.ID " +
                "ORDER BY CheckDate;"+
                "SELECT ID, SurvID AS SID, CheckID, SurveyDate FROM qrySurveyCheckRefSurvs;";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var results = db.QueryMultiple(sql);

                records = results.Read<SurveyCheck, Person, Survey, SurveyCheckType, SurveyCheck>((check, author, survey, checktype)=>
                {
                    check.Name = author;
                    check.SurveyCode = survey;
                    check.CheckType = checktype;
                    return check;
                }, splitOn: "CheckInit,SurvID,CheckTypeID").ToList();

                var surveys = results.Read<SurveyCheckRefSurvey>();
                foreach(SurveyCheckRefSurvey s in surveys)
                {
                    records.First(x=>x.ID==s.CheckID).ReferenceSurveys.Add(s);
                }
            }

            return records;
        }

        /// <summary>
        /// Returns the list of Survey Check types.
        /// </summary>
        /// <returns></returns>
        public static List<SurveyCheckType> GetSurveyCheckTypes()
        {
            List<SurveyCheckType> records = new List<SurveyCheckType>();

            string query = "SELECT ID, CheckType FROM qrySurveyCheckTypes ORDER BY ID";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                records = db.Query<SurveyCheckType>(query).ToList();
            }

            return records;
        }

    }
}
