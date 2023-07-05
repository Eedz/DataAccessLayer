using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class StudyWaveRecord : IRecord<StudyWave>
    {
        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }

        public StudyWave Item { get; set; }

        public StudyWaveRecord()
        {
            Item = new StudyWave();
        }

        public StudyWaveRecord(StudyWave wave)
        {
            Item = wave;
        }

        public bool HasSurvey(string surveycode)
        {
            return Item.Surveys.Any(x => x.SurveyCode.Equals(surveycode));
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertStudyWave(Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {

                if (DBAction.UpdateWave(Item) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }
}
