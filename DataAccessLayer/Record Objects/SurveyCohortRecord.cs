using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class SurveyCohortRecord : IRecord<SurveyCohort>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public SurveyCohort Item { get; set; }

        public SurveyCohortRecord(SurveyCohort item)
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
}
