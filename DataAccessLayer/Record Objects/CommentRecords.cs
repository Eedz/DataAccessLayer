using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class NoteRecord : IRecord<Note>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public Note Item { get; set; }

        public NoteRecord()
        {
            Item = new Note();
        }

        public NoteRecord(Note note)
        {
            Item = note;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertNote(this.Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;

            }
            else if (Dirty)
            {
                if (DBAction.UpdateNote(this.Item) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    public class QuestionCommentRecord : IRecord<QuestionComment>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public QuestionComment Item { get; set; }

        public QuestionCommentRecord()
        {
            Item = new QuestionComment();
        }

        public QuestionCommentRecord(QuestionComment comment)
        {
            Item = comment;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertQuestionComment(Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;

            }
            else if (Dirty)
            {
                if (DBAction.UpdateQuestionComment(Item) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    public class SurveyCommentRecord : IRecord<SurveyComment>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public SurveyComment Item { get; set; }

        public SurveyCommentRecord()
        {
            Item = new SurveyComment();
        }

        public SurveyCommentRecord(SurveyComment comment)
        {
            Item = comment;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertSurveyComment(Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;

            }
            else if (Dirty)
            {
                if (DBAction.UpdateSurveyComment(Item) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    public class WaveCommentRecord : IRecord<WaveComment>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public WaveComment Item { get; set; }

        public WaveCommentRecord()
        {
            Item = new WaveComment();   
        }

        public WaveCommentRecord(WaveComment comment)
        {
            Item = comment;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertWaveComment(Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;

            }
            else if (Dirty)
            {
                if (DBAction.UpdateWaveComment(Item) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    public class DeletedCommentRecord : IRecord<DeletedComment>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public DeletedComment Item { get; set; }

        public DeletedCommentRecord()
        {
            Item = new DeletedComment();
        }

        public DeletedCommentRecord(DeletedComment comment)
        {
            Item = comment;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertDeletedComment(Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;

            }
            else if (Dirty)
            {
                if (DBAction.UpdateDeletedQuestionComment(Item) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    public class RefVarCommentRecord : IRecord<RefVarComment>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public RefVarComment Item { get; set; }

        public RefVarCommentRecord()
        {
            Item = new RefVarComment();
        }

        public RefVarCommentRecord(RefVarComment comment)
        {
            Item = comment;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertRefVarComment(Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;

            }
            else if (Dirty)
            {
                if (DBAction.UpdateRefVarComment(Item) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }
}
