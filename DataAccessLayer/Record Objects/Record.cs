
namespace ITCLib
{
    public interface IRecord
    {
        bool NewRecord { get; set; }
        bool Dirty { get; set; }
        int SaveRecord();

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
