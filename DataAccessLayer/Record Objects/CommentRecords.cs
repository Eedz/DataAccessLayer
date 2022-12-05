using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class NoteRecord : Note, IRecord
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertNote(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;

            }
            else if (Dirty)
            {
                if (DBAction.UpdateNote(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    public class QuestionCommentRecord : QuestionComment, IRecord
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public int SaveRecord()
        {

            if (NewRecord)
            {

                if (DBAction.InsertQuestionComment(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;

            }
            else if (Dirty)
            {

                if (DBAction.UpdateQuestionComment(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    public class SurveyCommentRecord : SurveyComment, IRecord
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public int SaveRecord()
        {

            if (NewRecord)
            {

                if (DBAction.InsertSurveyComment(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;

            }
            else if (Dirty)
            {

                if (DBAction.UpdateSurveyComment(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    public class WaveCommentRecord : WaveComment, IRecord
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public int SaveRecord()
        {

            if (NewRecord)
            {

                if (DBAction.InsertWaveComment(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;

            }
            else if (Dirty)
            {

                if (DBAction.UpdateWaveComment(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    public class DeletedCommentRecord : DeletedComment, IRecord
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public int SaveRecord()
        {

            if (NewRecord)
            {

                if (DBAction.InsertDeletedComment(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;

            }
            else if (Dirty)
            {

                if (DBAction.UpdateDeletedQuestionComment(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    public class RefVarCommentRecord : RefVarComment, IRecord
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public int SaveRecord()
        {

            if (NewRecord)
            {

                if (DBAction.InsertRefVarComment(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;

            }
            else if (Dirty)
            {

                if (DBAction.UpdateRefVarComment(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }
}
