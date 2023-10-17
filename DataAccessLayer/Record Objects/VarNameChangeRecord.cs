using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITCLib;

namespace ITCLib
{
    public class VarNameChangeRecord : IRecord<VarNameChange>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public VarNameChange Item { get; set; }

        public List<VarNameChangeSurvey> AddedSurveysAffected { get; set; }
        public List<VarNameChangeSurvey> DeletedSurveysAffected { get; set; }
        public List<VarNameChangeNotification> AddedNotifications { get; set; }
        public List<VarNameChangeNotification> DeletedNotifications { get; set; }

        public VarNameChangeRecord()
        {
            Item = new VarNameChange();

            AddedSurveysAffected = new List<VarNameChangeSurvey>();
            DeletedSurveysAffected = new List<VarNameChangeSurvey>();

            AddedNotifications = new List<VarNameChangeNotification>();
            DeletedNotifications = new List<VarNameChangeNotification>();
        }

        public VarNameChangeRecord(VarNameChange item)
        {
            Item = item;

            AddedSurveysAffected = new List<VarNameChangeSurvey>();
            DeletedSurveysAffected = new List<VarNameChangeSurvey>();

            AddedNotifications = new List<VarNameChangeNotification>();
            DeletedNotifications = new List<VarNameChangeNotification>();
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertVarNameChange(Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdateVarNameChange(Item) == 1)
                    return 1;

                Dirty = false;
            }

            SaveSurveysAffected();
            SaveNotifications();

            return 0;
        }

        public int SaveSurveysAffected()
        {
            foreach(VarNameChangeSurvey s in AddedSurveysAffected)
            {
                DBAction.InsertVarNameChangeSurvey(s);
            }
            AddedSurveysAffected.Clear();

            foreach(VarNameChangeSurvey s in DeletedSurveysAffected)
            {
                DBAction.DeleteRecord(s);
            }
            DeletedSurveysAffected.Clear();

            return 0;
        }

        public int SaveNotifications()
        {
            foreach (VarNameChangeNotification n in AddedNotifications)
            {
                DBAction.InsertVarNameChangeNotification(n);
            }
            AddedNotifications.Clear();

            foreach (VarNameChangeNotification n in DeletedNotifications)
            {
                DBAction.DeleteRecord(n);
            }
            DeletedNotifications.Clear();

            return 0;
        }
    }    
}
