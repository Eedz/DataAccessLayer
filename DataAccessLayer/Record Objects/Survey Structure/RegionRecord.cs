﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ITCLib
{
    // see if this class can listen to Item's onpropertychanged
    public class RegionRecord : IRecord<Region>
    {
        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }
        public Region Item { get; set; }

        public RegionRecord()
        {
            Item = new Region();
        }

        public RegionRecord(Region region)
        {
            Item = region;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertRegion(Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdateRegion(Item) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }
}
