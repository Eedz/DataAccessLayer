using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ITCLib
{
    public class LockedSurveyRecord : IRecord
    {
        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }

        public int ID { get; set; }
        public int SurvID { get; set; }
        public string SurveyCode { get; set; }
        public int UnlockedBy { get; set; }
        public string Name { get; set; }
        public int UnlockedFor { get; set; }
        public DateTime UnlockedAt { get; set; }
        public double UnlockedForMin { get {
                TimeSpan ts = UnlockedAt.AddMinutes(UnlockedFor) - DateTime.Now;
                return ts.TotalMinutes;
            } }


        public int SaveRecord()
        {
            return 0;
        }
    }
}
