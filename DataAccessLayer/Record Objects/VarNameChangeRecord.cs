using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITCLib;

namespace ITCLib
{
    public class VarNameChangeRecord : VarNameChange, IRecord
    {
        public int ID { get; set; }

        public new List<VarNameChangeSurveyRecord> SurveysAffected { get; set; }
        public new List<VarNameChangeNotificationRecord> Notifications { get; set; }

        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public VarNameChangeRecord () : base()
        {
            ChangeDate = DateTime.Today;
            SurveysAffected = new List<VarNameChangeSurveyRecord>();
            Notifications = new List<VarNameChangeNotificationRecord>();
        }

        public string GetSurveysAffected()
        {
            if (SurveysAffected.Count == 0)
                return string.Empty;

            string list = "";
            foreach (VarNameChangeSurveyRecord s in SurveysAffected)
                list += s.SurveyCode + ", ";

            list = Utilities.TrimString(list, ", ");
            return list;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertVarNameChange(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {

                if (DBAction.UpdateVarNameChange(this) == 1)
                    return 1;

                Dirty = false;
            }
            return 0;
        }
    }

    public class VarNameChangeSurveyRecord : IRecord
    {
        public int ID { get; set; }
        public int ChangeID { get; set; }
        public int SurvID { get; set; }

        public string SurveyCode { get; set; }

        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public VarNameChangeSurveyRecord()
        {
            NewRecord = true;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertVarNameChangeSurvey(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {

                if (DBAction.UpdateVarNameChangeSurvey(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    public class VarNameChangeNotificationRecord : IRecord
    {
        public int ID { get; set; }
        public int ChangeID { get; set; }
        public int PersonID { get; set; }
        public string NotifyType { get; set; }

        public string Name { get; set; }

        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }

        public VarNameChangeNotificationRecord()
        {
            NewRecord = true;
            Name = string.Empty;
            NotifyType = string.Empty;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertVarNameChangeNotification(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdateVarNameChangeNotification(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    
}
