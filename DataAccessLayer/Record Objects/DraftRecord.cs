using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ITCLib
{
    public class DraftRecord : IRecord<SurveyDraft>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public SurveyDraft Item { get; set; }

        public List<SurveyDraftExtraField> EditedExtraFields { get; set; }

        public DraftRecord()
        {
            Item = new SurveyDraft();
            EditedExtraFields = new List<SurveyDraftExtraField>();
        }

        public DraftRecord (SurveyDraft item)
        {
            Item = item;
            EditedExtraFields = new List<SurveyDraftExtraField>();
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertSurveyDraft(Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdateSurveyDraft(Item) == 1)
                    return 1;

                Dirty = false;
            }

            SaveExtraFields();

            return 0;
        }

        public int SaveExtraFields()
        {
            foreach (SurveyDraftExtraField field in EditedExtraFields)
            {
                DBAction.UpdateSurveyDraftExtraField(field);
            }
            EditedExtraFields.Clear();

            return 0;
        }
    }
}
