using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class SimilarWordsRecord : IRecord<SimilarWords>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public SimilarWords Item { get; set; }

        public SimilarWordsRecord (SimilarWords item)
        {
            Item = item;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertSimilarWords(this.Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdateSimilarWords(this.Item) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }
}
