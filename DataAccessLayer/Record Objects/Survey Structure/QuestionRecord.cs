using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    /// <summary>
    /// Container to keep track of changes to a question.
    /// </summary>
    public class QuestionRecord : IRecord<SurveyQuestion>
    {
        public bool DirtyLabels { get; set; }
        public bool DirtyQnum { get; set; }
        public bool DirtyAltQnum { get; set; }
        public bool DirtyPlainFilter { get; set; }

        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public SurveyQuestion Item { get; set; }

        public List<QuestionTimeFrame> AddTimeFrames;
        public List<QuestionTimeFrame> DeleteTimeFrames;

        public List<SurveyImage> AddedImages;
        public List<SurveyImage> DeletedImages;

        public QuestionRecord() 
        {
            Item = new SurveyQuestion();

            AddTimeFrames = new List<QuestionTimeFrame>();
            DeleteTimeFrames = new List<QuestionTimeFrame>();

            AddedImages = new List<SurveyImage>();
            DeletedImages = new List<SurveyImage>();
        }

        public QuestionRecord(SurveyQuestion question)
        {
            Item = question;

            AddTimeFrames = new List<QuestionTimeFrame>();
            DeleteTimeFrames = new List<QuestionTimeFrame>();

            AddedImages = new List<SurveyImage>();
            DeletedImages = new List<SurveyImage>();
        }

        public bool IsEdited()
        {
            return Dirty || DirtyQnum || DirtyLabels || DirtyAltQnum || DirtyPlainFilter || AddTimeFrames.Count > 0 || DeleteTimeFrames.Count > 0 || AddedImages.Count>0 || DeletedImages.Count>0;
        }

        public int SaveRecord()
        {

            if (NewRecord)
            {
                int result = DBAction.InsertVariable(Item.VarName);

                if (DBAction.InsertQuestion(Item.SurveyCode, Item) == 1)
                    return 1;

                if (DBAction.UpdateQuestionWordings(Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdateQuestionWordings(Item) == 1)
                    return 1;

                Dirty = false;
            }

            if (DirtyPlainFilter)
            {
                if (DBAction.UpdatePlainFilter(Item) == 1)
                    return 1;

                DirtyPlainFilter = false;
            }

            if (DirtyQnum)
                if (DBAction.UpdateQnum(Item) == 1)
                    return 1;
                else
                    DirtyQnum = false;

            if (DirtyAltQnum)
                if (DBAction.UpdateAltQnum(Item) == 1)
                    return 1;
                else
                    DirtyAltQnum = false;

            if (DirtyLabels)
            {
                if (DBAction.UpdateLabels(Item.VarName) == 1)
                    return 1;

                DirtyLabels = false;
            }

            foreach (QuestionTimeFrame qtf in AddTimeFrames)
            {
                DBAction.InsertQuestionTimeFrame(qtf);
            }
            AddTimeFrames.Clear();

            foreach (QuestionTimeFrame qtf in DeleteTimeFrames)
            {
                DBAction.DeleteRecord(qtf);
            }
            DeleteTimeFrames.Clear();

            foreach(SurveyImage img in AddedImages)
            {
                DBAction.InsertQuestionImage(img);
            }
            AddedImages.Clear();

            foreach (SurveyImage img in DeletedImages)
            {
                DBAction.DeleteRecord(img);
            }
            DeletedImages.Clear();

            return 0;
        }

    }
}
