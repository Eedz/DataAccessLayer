using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace ITCLib
{
    public interface IRecord
    {
        bool NewRecord { get; set; }
        bool Dirty { get; set; }
        int SaveRecord();

    }

    public class RegionRecord : Region, IRecord
    {
        public int ID { get; set; }
        public new BindingList<StudyRecord> Studies { get; set; }
        public bool StudyAdded { get; set; }
        public bool Dirty {get;set;}
        public bool NewRecord { get; set; }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertRegion(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {

                if (DBAction.UpdateRegion(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    public class StudyRecord : Study, IRecord
    {
        public int ID { get; set; }
        public new BindingList<StudyWaveRecord> Waves { get; set; }
        public bool WaveAdded { get; set; }
        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }

        public int RegionID { get; set; }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertCountry(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {

                if (DBAction.UpdateStudy(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    public class StudyWaveRecord : StudyWave, IRecord
    {
        public int ID { get; set; }
        public new BindingList<SurveyRecord> Surveys { get; set; }
        public bool SurveyAdded { get; set; }
        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }
        
        public int StudyID { get; set; }

        public bool HasSurvey(string surveycode)
        {
            return Surveys.Any(x => x.SurveyCode.Equals(surveycode));
            
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertStudyWave(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {

                if (DBAction.UpdateWave(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    /// <summary>
    /// Container to keep track of changes to a survey.
    /// </summary>
    public class SurveyRecord : Survey, IRecord
    {
        //public int SID { get; set; }

        public new BindingList<QuestionRecord> Questions { get; set; }
        
        public bool QuestionsAdded { get; set; } // true if questions are new and unsaved
        public bool QuestionsDeleted { get; set; } // true if questions are pending delete

        public bool NeedsRenumber { get; set; } // true if this survey needs to  be renumbered
        public bool TranslationSync { get; set; } // true if the english changed

        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public int WaveID { get; set; }

        public SurveyRecord() :base()
        {
            
            
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertSurvey(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {

                if (DBAction.UpdateSurvey(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }

        /// <summary>
        /// Adds a question to the survey's question list.
        /// </summary>
        /// <param name="newQ"></param>
        public void AddQuestion(QuestionRecord newQ)
        {
            if (!Questions.Contains(newQ, new SurveyQuestionComparer()))
            {
                Questions.Add(newQ);
                UpdateEssentialQuestions();
            }
        }

        /// <summary>
        /// Adds each question in the list to the survey's question list.
        /// </summary>
        /// <param name="newQ"></param>
        public void AddQuestions(List<QuestionRecord> questions)
        {
            foreach (QuestionRecord sq in questions)
                if (!Questions.Contains(sq, new SurveyQuestionComparer()))
                    Questions.Add(sq);

            UpdateEssentialQuestions();
        }

        /// <summary>
        /// Adds a question to the survey's question list at the specified location.
        /// </summary>
        /// <param name="newQ"></param>
        public void AddQuestion(QuestionRecord newQ, int afterIndex, bool withRenumber)
        {
            Questions.Insert(afterIndex, newQ);
            if (withRenumber) Renumber(0);
            UpdateEssentialQuestions();
        }

        /// <summary>
        /// Adds each question in the list to the survey's question list.
        /// </summary>
        /// <param name="newQ"></param>
        public void AddQuestions(BindingList<QuestionRecord> questions, int afterIndex, bool withRenumber)
        {
            // add an offset to the afterIndex so that the questions aren't all added at the same index, resulting in reversed order
            int offset = 0; 
            
            foreach (QuestionRecord sq in questions)
            {
                Questions.Insert(afterIndex+ offset, sq);
                offset++;
            }

            if (withRenumber) Renumber(0);
            UpdateEssentialQuestions();
        }

        /// <summary>
        /// Gets a specific question by it's varname.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>SurveyQuestion object matching the supplied varname. Returns null if one is not found.</returns>
        public SurveyQuestion QuestionByVar(string varname)
        {
            foreach (QuestionRecord sq in Questions)
            {
                if (sq.VarName.VarName.Equals(varname))
                    return sq;
            }
            return null;
        }

        /// <summary>
        /// </summary>
        protected override void Renumber(int start)
        {
            int qLet = 0;
            int hcount = 0;
            int i;
            int counter = 0;
            QuestionType qType;

            int currQnum;
            string newQnum;

            currQnum = start;

            foreach (SurveyQuestion sq in Questions)
            {
                qType = Utilities.GetQuestionType(sq);

                // increment either the letter or the number, count headings
                switch (qType)
                {
                    case QuestionType.Series:
                        qLet++;
                        hcount = 0;
                        break;
                    case QuestionType.Standalone:
                        currQnum++;
                        qLet = 1;
                        hcount = 0;
                        break;
                    case QuestionType.Heading:
                        hcount++;
                        break;
                    case QuestionType.Subheading:
                        hcount++;
                        break;
                }

                newQnum = currQnum.ToString("000");

                if (qType != QuestionType.Standalone)
                {
                    newQnum += new string('z', (qLet - 1) / 26);
                    newQnum += Char.ConvertFromUtf32(96 + qLet - 26 * ((qLet - 1) / 26));

                }

                if (hcount > 0)
                    newQnum += "!" + hcount.ToString("000");

                sq.Qnum = newQnum;

                // add 'a' to series starters
                if (qType == QuestionType.Standalone)
                {
                    i = counter;

                    do
                    {
                        if (i < Questions.Count - 1)
                            i++;
                        else
                            break;

                    } while (Utilities.GetQuestionType(Questions[i]) == QuestionType.Heading || Utilities.GetQuestionType(Questions[i]) == QuestionType.InterviewerNote);

                    if (Utilities.GetQuestionType(Questions[i]) == QuestionType.Series)
                        sq.Qnum += "a";
                }
                counter++;
            }
        }

    }

    

    /// <summary>
    /// Container to keep track of changes to a question.
    /// </summary>
    /// // TODO implement change event and set dirty = true when things change
    public class QuestionRecord : SurveyQuestion, IRecord
    {
       
        public bool DirtyLabels { get; set; }
        public bool DirtyQnum { get; set; }
        public bool DirtyAltQnum { get; set; }
        public bool DirtyPlainFilter { get; set; }

        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public new List<TranslationRecord> Translations { get; set; }

        //public string SurveyCode { get; set; }
        public int SurvID { get; set; }

        public QuestionRecord() : base()
        {
            Translations = new List<TranslationRecord>();
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        // TODO use this method to set the dirty flag
        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        protected override void NotifyPropertyChanged<T>(T oldValue, T newValue, [CallerMemberName] String propertyName = "")
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedExtendedEventArgs<T>(propertyName, oldValue, newValue));
            }

        }

        public bool IsEdited()
        {
            return Dirty || DirtyQnum || DirtyLabels || DirtyAltQnum || DirtyPlainFilter;
        }

        public int SaveRecord()
        {
            
            if (NewRecord)
            {
                int result = DBAction.InsertVariable(VarName);

                if (DBAction.InsertQuestion(SurveyCode, this) == 1)
                    return 1;

                if (DBAction.UpdateQuestionWordings(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdateQuestionWordings(this) == 1)
                    return 1;

                Dirty = false;
            }

            if (DirtyPlainFilter)
            {
                if (DBAction.UpdatePlainFilter(this) == 1)
                    return 1;

                DirtyPlainFilter = false;
            }

            if (DirtyQnum)
                if (DBAction.UpdateQnum(this) == 1)
                    return 1;
                else
                    DirtyQnum = false;

            if (DirtyAltQnum)
                if (DBAction.UpdateAltQnum(this) == 1)
                    return 1;
                else
                    DirtyAltQnum = false;

            if (DirtyLabels)
            {
                if (DBAction.UpdateLabels(VarName) == 1)
                    return 1;

                DirtyLabels = false;
            }
            
            return 0;
        }

    }

    

    public class SurveyCohortRecord : SurveyCohort, IRecord
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public SurveyCohortRecord () : base()
        {

        }

        public SurveyCohortRecord(int id, string cohort)
        {
            ID = id;
            Cohort = cohort;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertCohort(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;


            }
            else if (Dirty)
            {

                if (DBAction.UpdateCohort(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    public class UserStateRecord : UserState, IRecord
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public UserStateRecord() : base()
        {

        }

        public UserStateRecord(int id, string state)
        {
            ID = id;
            UserStateName = state;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertUserState(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;


            }
            else if (Dirty)
            {

                if (DBAction.UpdateUserState(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    public class SimilarWordsRecord : IRecord
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public int ID { get; set; }
        public string Words { get; set; }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertSimilarWords(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;


            }
            else if (Dirty)
            {

                if (DBAction.UpdateSimilarWords(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    

    
}
