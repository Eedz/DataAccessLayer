using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class TranslationRecord : Translation, IRecord 
    {
        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertTranslation(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {

                if (DBAction.UpdateTranslation(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }

        
    }
}
