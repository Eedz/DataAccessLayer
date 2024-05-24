using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class FormStateRecord : IRecord<FormState>
    {
        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }
        public FormState Item { get; set; }

        public FormStateRecord()
        {
            this.Item = new FormState();
        }

        public FormStateRecord (FormState item)
        {
            this.Item = item;
        }

        public int SaveRecord()
        {
            if (Dirty)
            {
                if (Item.FilterID > 0)
                {
                    if (DBAction.UpdateFormSurvey(Item, Item.PersonnelID) == 1)
                        return 1;
                }
                if (!string.IsNullOrWhiteSpace(Item.Filter))
                {
                    DBAction.UpdateFormFilter(Item, Item.PersonnelID);
                }

                Dirty = false;
            }

            return 0;
        }
    }
}
