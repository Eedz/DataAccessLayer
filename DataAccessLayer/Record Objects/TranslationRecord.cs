﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class TranslationRecord : IRecord<Translation>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public Translation Item { get; set; }

        public TranslationRecord(Translation t) 
        {
            Item = t;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertTranslation(Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {

                if (DBAction.UpdateTranslation(Item) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }
}
