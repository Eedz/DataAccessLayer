//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data;
//using System.Data.SqlClient;
//using ITCLib;
//using Dapper;

//namespace DataAccessLayer.Repositories
//{
//    public class SurveyQuestionRepository
//    {
//        private readonly IDbConnection _connection;

//        public SurveyQuestionRepository(string connectionString)
//        {
//            _connection = new SqlConnection(connectionString);
//        }

//        public SurveyQuestion GetById(int id)
//        {
//            try
//            {
//                string sql = "SELECT * FROM SurveyQuestions WHERE Id = @Id";
//                return _connection.QueryFirstOrDefault<SurveyQuestion>(sql, new { Id = id });
//            }
//            catch (Exception ex)
//            {
//                // Log or handle the exception
//                throw new Exception($"Error occurred while retrieving survey question with ID {id}.", ex);
//            }
//        }

//        public IEnumerable<SurveyQuestion> GetBySurvId(int id)
//        {
//            try
//            {
//                string sql = "SELECT * FROM SurveyQuestions WHERE Id = @Id";
//                return _connection.Query<SurveyQuestion>(sql, new { Id = id });
//            }
//            catch (Exception ex)
//            {
//                // Log or handle the exception
//                throw new Exception($"Error occurred while retrieving survey question with ID {id}.", ex);
//            }
//        }

//        public void Create(SurveyQuestion question)
//        {
//            try { 
//            string sql = "INSERT INTO SurveyQuestions (QuestionText, QuestionType) VALUES (@QuestionText, @QuestionType)";
//            _connection.Execute(sql, question);
//            }
//            catch (Exception ex)
//            {
//                // Log or handle the exception
//                throw new Exception($"Error occurred while creating survey question.", ex);
//            }
//}

//        public void Update(SurveyQuestion question)
//        {
//            try { 
//            string sql = "UPDATE SurveyQuestions SET QuestionText = @QuestionText, QuestionType = @QuestionType WHERE Id = @Id";
//            _connection.Execute(sql, question);
//            }
//            catch (Exception ex)
//            {
//                // Log or handle the exception
//                throw new Exception($"Error occurred while updating survey question with ID {question.ID}.", ex);
//            }
//}

//        public void Delete(int id)
//        {
//            try { 
//            string sql = "DELETE FROM SurveyQuestions WHERE Id = @Id";
//            _connection.Execute(sql, new { Id = id });
//            }
//            catch (Exception ex)
//            {
//                // Log or handle the exception
//                throw new Exception($"Error occurred while deleting survey question with ID {id}.", ex);
//            }
//        }
//    }
//}
