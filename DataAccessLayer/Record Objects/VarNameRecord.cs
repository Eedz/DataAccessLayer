using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class VariablePrefixRecord : VariablePrefix, IRecord
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }


        public new List<ParallelPrefixRecord> ParallelPrefixes { get; set; }
        public new List<VariableRangeRecord> Ranges { get; set; }

        public VariablePrefixRecord()
        {
            ParallelPrefixes = new List<ParallelPrefixRecord>();
            Ranges = new List<VariableRangeRecord>();
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertPrefix(this) == 1)
                    return 1;

                foreach (ParallelPrefixRecord r in ParallelPrefixes)
                {
                    r.PrefixID = this.ID;
                    r.SaveRecord();
                }

                foreach (VariableRangeRecord r in Ranges)
                {
                    r.PrefixID = this.ID;
                    r.SaveRecord();
                }

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdatePrefix(this) == 1)
                    return 1;

                foreach (ParallelPrefixRecord p in ParallelPrefixes)
                {
                    if (NewRecord)
                        DBAction.InsertParallelPrefix(p);
                }

                foreach (VariableRangeRecord r in Ranges)
                {
                    r.SaveRecord();
                }

                Dirty = false;
            }
            return 0;
        }
    }

    public class VariableRangeRecord : VariableRange, IRecord
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public int PrefixID { get; set; }

        public VariableRangeRecord() : base()
        {
            NewRecord = true;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertPrefixRange(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdatePrefixRange(this) == 1)
                    return 1;

                Dirty = false;
            }
            return 0;
        }
    }

    public class ParallelPrefixRecord : IRecord
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public int ID { get; set; }
        public int PrefixID { get; set; }
        public int RelatedID { get; set; }
        public string Prefix { get; set; }

        public ParallelPrefixRecord() : base()
        {
            NewRecord = true;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertParallelPrefix(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdateParallelPrefix(this) == 1)
                    return 1;

                Dirty = false;
            }
            return 0;
        }
    }

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
