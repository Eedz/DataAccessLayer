using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class SurveyCheckRecord : IRecord<SurveyChecker>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public SurveyChecker Item { get; set; }

        public SurveyCheckRecord (SurveyChecker item)
        {
            this.Item = item; 
        }
    
        public int SaveRecord()
        {
            throw new NotImplementedException();
        }
    }
}
