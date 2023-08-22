using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class VarNameRecord: IRecord<VariableName>
    {
        public bool NewRecord { get ; set ; }
        public bool Dirty { get; set ; }
        public VariableName Item { get; set; }

        public VarNameRecord(VariableName item)
        {
            Item = item;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertVariable(Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdateLabels(Item) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }
}
