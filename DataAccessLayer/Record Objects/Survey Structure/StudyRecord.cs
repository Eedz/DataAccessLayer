using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class StudyRecord : IRecord<Study>
    {       
        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }

        public int RegionID { get; set; }

        public Study Item { get; set; }

        public StudyRecord()
        {
            Item = new Study();
        }

        public StudyRecord(Study item)
        {
            Item = item;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertCountry(Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {

                if (DBAction.UpdateStudy(Item) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }
}
