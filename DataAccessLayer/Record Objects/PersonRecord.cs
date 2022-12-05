using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class PersonRecord : Person, IRecord
    {
        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }
        public new List<PersonnelStudyRecord> AssociatedStudies { get; set; }
        public new List<PersonnelCommentRecord> PersonnelComments { get; set; }

        public PersonRecord () 
        {
            AssociatedStudies = new List<PersonnelStudyRecord>();
            PersonnelComments = new List<PersonnelCommentRecord>();
            NewRecord = true;   
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertPersonnel(this) == 1)
                    return 1;

                foreach (PersonnelStudyRecord r in AssociatedStudies)
                {
                    r.PersonnelID = this.ID;
                    r.SaveRecord();
                }

                foreach (PersonnelCommentRecord r in PersonnelComments)
                {
                    r.PersonnelID = this.ID;
                    r.SaveRecord();
                }

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {

                if (DBAction.UpdatePersonnel(this) == 1)
                    return 1;

                foreach (PersonnelStudyRecord r in AssociatedStudies)
                {
                    if (r.NewRecord)
                        r.PersonnelID = this.ID;

                    r.SaveRecord();
                }

                foreach (PersonnelCommentRecord r in PersonnelComments)
                {
                    if (r.NewRecord)
                        r.PersonnelID = this.ID;

                    r.SaveRecord();
                }

                Dirty = false;
            }

            return 0;
        }
    }

    public class PersonnelStudyRecord : IRecord
    {
        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }

        public int ID { get; set; }
        public int PersonnelID { get; set; }

        public int StudyID { get; set; }
        public string StudyName { get; set; }

        public PersonnelStudyRecord()
        {
            NewRecord = true;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertPersonnelStudy(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {

                if (DBAction.UpdatePersonnelStudy(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }

    public class PersonnelCommentRecord : PersonnelComment, IRecord
    {
        public int PersonnelID { get; set; }

        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }

        public PersonnelCommentRecord() : base()
        {
            NewRecord = true;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertPersonnelComment(this) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {

                if (DBAction.UpdatePersonnelComment(this) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }
    

    public class UserRecord : UserPrefs, IRecord
    {
        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }
        public bool DirtyFilters { get; set; }
        public new List<FormStateRecord> FormStates { get;set; }

        public UserRecord()
        {
            FormStates = new List<FormStateRecord>();
        }

        public int SaveRecord()
        {
            if (Dirty)
            {
                if (DBAction.UpdateUser(this) == 1)
                    return 1;

                if (DirtyFilters)
                {
                    UpdateFormStates();
                }

                Dirty = false;
            }

            return 0;   
        }

        public int UpdateFormStates()
        {
            int count = 0;
            foreach (FormStateRecord fs in FormStates)
            {
                if (fs.SaveRecord() == 1)
                {
                    count++;
                }
            }
            return 0;
        }

        public new FormState GetFormState(string formname, int formnum)
        {
            return FormStates.Where(x => x.FormName.Equals(formname) && x.FormNum == formnum).FirstOrDefault();
        }

        public new int GetFilterID(string formname, int formnum)
        {
            var state = FormStates.Where(x => x.FormName.Equals(formname) && x.FormNum == formnum).FirstOrDefault();
            if (state == null)
                return 0;
            else
                return state.FilterID;
        }

        public new string GetFilter(string formname, int formnum)
        {
            var state = FormStates.Where(x => x.FormName.Equals(formname) && x.FormNum == formnum).FirstOrDefault();
            if (state == null)
                return string.Empty;
            else
                return state.Filter;
        }

    }

    public class FormStateRecord : FormState, IRecord
    {

        public int PersonnelID { get; set; }
        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }

        public int SaveRecord()
        {
            if (Dirty)
            {
                if (FilterID > 0)
                {
                    if (DBAction.UpdateFormSurvey(this, PersonnelID) == 1)
                        return 1;
                }
                if (!string.IsNullOrWhiteSpace(Filter))
                {
                    DBAction.UpdateFormFilter(this, PersonnelID);
                }
                 

                Dirty = false;
            }

            return 0;
        }
    }
}
