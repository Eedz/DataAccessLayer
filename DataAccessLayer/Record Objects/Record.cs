
namespace ITCLib
{
    public interface IRecord
    {
        bool NewRecord { get; set; }
        bool Dirty { get; set; }
        int SaveRecord();

    }

    public class SurveyCohortRecord : IRecord<SurveyCohort>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public SurveyCohort Item { get; set; }

        public SurveyCohortRecord (SurveyCohort item)
        {
            Item = item;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertCohort(this.Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdateCohort(this.Item) == 1)
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
