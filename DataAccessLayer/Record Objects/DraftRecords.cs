using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace ITCLib
{
    public class SurveyDraftRecord : SurveyDraft, IRecord 
    {
        public int ID { get; set; }

        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public bool DirtyExtraFields { get; set; }

        public new List<SurveyDraftExtraFieldRecord> ExtraFields { get; set; }

        public SurveyDraftRecord()
        {
            Questions = new List<DraftQuestion>();
            ExtraFields = new List<SurveyDraftExtraFieldRecord>();
        }

        // TODO I think all the properties would have to be hidden by this derived type for this to work
        //public new event PropertyChangedEventHandler PropertyChanged;

        //// This method is called by the Set accessor of each property.
        //// The CallerMemberName attribute that is applied to the optional propertyName
        //// parameter causes the property name of the caller to be substituted as an argument.
        //private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //        Dirty = true;
        //    }
        //}

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertSurveyDraft(this) == 1)
                    return 1;

                SaveExtraFields();

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdateSurveyDraft(this) == 1)
                    return 1;

                
                SaveExtraFields();

                Dirty = false;
            }

            return 0;
        }

        // TODO return an error if something doesnt save?
        public int SaveExtraFields()
        {       
            foreach (SurveyDraftExtraFieldRecord e in ExtraFields)
                e.SaveRecord();
            
            return 0;
        }
    }

    public class SurveyDraftExtraFieldRecord : SurveyDraftExtraField, IRecord
    {
        public int ID { get; set; }
        public int DraftID { get; set; }

        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        
        public SurveyDraftExtraFieldRecord()
        {
            NewRecord = true;
        }

        public SurveyDraftExtraFieldRecord(bool newrecord)
        {
            NewRecord = newrecord;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertSurveyDraftExtraInfo(DraftID, FieldNumber,Label) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {

                if (DBAction.UpdateSurveyDraftExtraField(this) == 1)
                    return 1;

                Dirty = false;
            }
            
            return 0;
        }
    }

    public class DraftQuestionRecord : DraftQuestion, IRecord
    {
        public int ID { get; set; }
        public int DraftID { get; set; }

        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }

        public int SaveRecord()
        {
            return 1;
        }


    }

}
