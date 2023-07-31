using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    /// <summary>
    /// Container to keep track of changes to a survey.
    /// </summary>
    public class SurveyRecord : IRecord<Survey>
    {  
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public List<SurveyLanguage> AddLanguages { get; set; }
        public List<SurveyLanguage> DeleteLanguages { get; set; }

        public List<SurveyScreenedProduct> AddProducts { get; set; }
        public List<SurveyScreenedProduct> DeleteProducts { get; set; }

        public List<SurveyUserState> AddStates { get; set; }
        public List<SurveyUserState> DeleteStates { get; set; }

        public Survey Item { get; set; }

        public SurveyRecord() 
        {
            AddLanguages = new List<SurveyLanguage>();
            DeleteLanguages = new List<SurveyLanguage>();

            AddProducts = new List<SurveyScreenedProduct>();
            DeleteProducts = new List<SurveyScreenedProduct>(); 

            AddStates = new List<SurveyUserState>();
            DeleteStates = new List<SurveyUserState>();

            Item = new Survey();
        }

        public SurveyRecord(Survey survey) : this()
        {
            Item = survey;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertSurvey(Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {

                if (DBAction.UpdateSurvey(Item) == 1)
                    return 1;

                Dirty = false;
            }

            SaveLanguages();
            SaveStates();
            SaveProducts();

            return 0;
        }

        private void SaveLanguages()
        {
            foreach(SurveyLanguage l in AddLanguages)
            {
                DBAction.InsertSurveyLanguage(l);
            }
            AddLanguages.Clear();

            foreach(SurveyLanguage l in DeleteLanguages)
            {
                DBAction.DeleteRecord(l);
            }
            DeleteLanguages.Clear();
        }

        private void SaveStates()
        {
            foreach (SurveyUserState l in AddStates)
            {
                DBAction.InsertSurveyUserState(l);
            }
            AddStates.Clear();

            foreach (SurveyUserState l in DeleteStates)
            {
                DBAction.DeleteRecord(l);
            }
            DeleteStates.Clear();
        }

        private void SaveProducts()
        {
            foreach (SurveyScreenedProduct l in AddProducts)
            {
                DBAction.InsertSurveyScreenedProduct(l);
            }
            AddProducts.Clear();

            foreach (SurveyScreenedProduct l in DeleteProducts)
            {
                DBAction.DeleteRecord(l);
            }
            DeleteProducts.Clear();
        }

        /// <summary>
        /// Adds a question to the survey's question list.
        /// </summary>
        /// <param name="newQ"></param>
        public void AddQuestion(QuestionRecord newQ)
        {
            if (!Item.Questions.Contains(newQ.Item, new SurveyQuestionComparer()))
            {
                Item.Questions.Add(newQ.Item);
                Item.UpdateEssentialQuestions();
            }
        }

        /// <summary>
        /// Adds each question in the list to the survey's question list.
        /// </summary>
        /// <param name="newQ"></param>
        public void AddQuestions(List<QuestionRecord> questions)
        {
            foreach (QuestionRecord record in questions)
                if (!Item.Questions.Contains(record.Item, new SurveyQuestionComparer()))
                {
                    Item.Questions.Add(record.Item);
                }

            Item.UpdateEssentialQuestions();
        }

        /// <summary>
        /// Adds a question to the survey's question list at the specified location.
        /// </summary>
        /// <param name="newQ"></param>
        public void AddQuestion(QuestionRecord newQ, int afterIndex, bool withRenumber)
        {
            Item.AddQuestion(newQ.Item, afterIndex, withRenumber);
            Item.UpdateEssentialQuestions();
        }

        /// <summary>
        /// Adds each question in the list to the survey's question list.
        /// </summary>
        /// <param name="newQ"></param>
        public void AddQuestions(List<QuestionRecord> questions, int afterIndex, bool withRenumber)
        {
            // add an offset to the afterIndex so that the questions aren't all added at the same index, resulting in reversed order
            int offset = 0;

            foreach (QuestionRecord sq in questions)
            {
                Item.AddQuestion(sq.Item, afterIndex + offset, withRenumber);
                offset++;
            }
            Item.UpdateEssentialQuestions();
        }
    }
}
