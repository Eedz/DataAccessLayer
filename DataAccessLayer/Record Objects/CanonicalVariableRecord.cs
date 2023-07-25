using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class CanonicalVariableRecord : CanonicalRefVarName, IRecord
    {
        public new int ID { get; set; }
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public CanonicalVariableRecord() : base()
        {

        }

        public int SaveRecord()
        {

            if (NewRecord)
            {

                if (DBAction.InsertCanonVar(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;

            }
            else if (Dirty)
            {

                if (DBAction.UpdateCanonVar(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }
}
