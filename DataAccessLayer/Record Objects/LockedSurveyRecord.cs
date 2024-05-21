using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ITCLib
{
    public class LockedSurveyRecord : IRecord <LockedSurvey>
    {
        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }
        public LockedSurvey Item { get; set; }

        public LockedSurveyRecord (LockedSurvey item)
        {
            Item = item;
        }

        public int SaveRecord()
        {
            return 0;
        }
    }
}
