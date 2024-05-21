using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class WordingRecord : IRecord<Wording>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public Wording Item { get; set; }

        public WordingRecord (Wording item)
        {
            Item = item;
        }

        public int SaveRecord()
        {
            if (NewRecord) // new wording created by this form
            {
                // insert into table
                DBAction.InsertWording(Item);
                Dirty = false;
                NewRecord = false;
                
            }
            else if (Dirty) // existing wording edited
            {
                DBAction.UpdateWording(Item);
                Dirty = false;
            }
            return 0;
        }
    }
}
