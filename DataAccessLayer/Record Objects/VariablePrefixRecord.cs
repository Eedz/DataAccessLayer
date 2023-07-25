using ITCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VariablePrefixRecord: IRecord<VariablePrefix>
{
    public bool NewRecord { get; set; }
    public bool Dirty { get; set ; }
    public VariablePrefix Item { get; set; }

    public List<ParallelPrefix> AddedParallels { get; set; }
    public List<ParallelPrefix> DeletedParallels { get; set; }

    public List<VariableRange> AddedRanges { get; set; }
    public List<VariableRange> EditedRanges { get; set; }
    public List<VariableRange> DeletedRanges { get; set; }


    public VariablePrefixRecord (VariablePrefix item)
    {
        AddedParallels = new List<ParallelPrefix>();
        EditedRanges = new List<VariableRange>();
        DeletedParallels = new List<ParallelPrefix>();

        AddedRanges = new List<VariableRange>();
        DeletedRanges = new List<VariableRange>();


        Item = item;
    }

    public int SaveRecord()
    {
        if (NewRecord)
        {
            if (DBAction.InsertPrefix(Item) == 1)
                return 1;

            NewRecord = false;
            Dirty = false;
        }
        else if (Dirty)
        {
            if (DBAction.UpdatePrefix(Item) == 1)
                return 1;

            Dirty = false;
        }

        SaveParallels();
        SaveRanges();

        return 0;
    }

    private void SaveParallels()
    {
        foreach (ParallelPrefix r in AddedParallels)
        {
            DBAction.InsertParallelPrefix(r);
        }
        AddedParallels.Clear();
        foreach (ParallelPrefix r in DeletedParallels)
        {
            DBAction.DeleteRecord(r);
        }
        DeletedParallels.Clear();
    }

    private void SaveRanges()
    {
        foreach(VariableRange r in AddedRanges)
        {
            DBAction.InsertPrefixRange(r);
        }
        AddedRanges.Clear();
        foreach(VariableRange r in DeletedRanges)
        {
            DBAction.DeleteRecord(r);
        }
        DeletedRanges.Clear();

        foreach (VariableRange r in EditedRanges)
        {
            DBAction.UpdatePrefixRange(r);
        }
        EditedRanges.Clear();
    }
}


