using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class ResponseSetRecord : IRecord<ResponseSet>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public ResponseSet Item { get; set; }

        public ResponseSetRecord (ResponseSet item)
        {
            Item = item;
        }

        public int SaveRecord()
        {
            if (NewRecord) 
            {
                DBAction.InsertResponseSet(Item);
                Dirty = false;
                NewRecord = false;

            }
            else if (Dirty)
            {
                DBAction.UpdateResponseSet(Item);
                Dirty = false;
            }

            return 0;
        }
    }
}
