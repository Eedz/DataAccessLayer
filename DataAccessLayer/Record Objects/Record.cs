﻿
namespace ITCLib
{
    public interface IRecord
    {
        bool NewRecord { get; set; }
        bool Dirty { get; set; }
        int SaveRecord();

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
