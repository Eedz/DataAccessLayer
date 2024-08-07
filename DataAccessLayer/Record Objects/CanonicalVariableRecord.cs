﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class CanonicalVariableRecord : IRecord<CanonicalRefVarName>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public CanonicalRefVarName Item { get; set; }

        public CanonicalVariableRecord (CanonicalRefVarName item)
        {
            Item = item;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertCanonVar(this.Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdateCanonVar(this.Item) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }
}
