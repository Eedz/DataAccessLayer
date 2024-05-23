using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class PersonRecord : IRecord<Person>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public Person Item { get; set; }

        public List<PersonnelStudy> AddedStudies { get; set; }
        public List<PersonnelStudy> DeletedStudies { get; set; }

        public List<PersonnelComment> AddedComment { get; set; }
        public List<PersonnelComment> EditedComment { get; set; }
        public List<PersonnelComment> DeletedComment { get; set; }

        public List<PersonnelRole> AddedRoles { get; set; }
        public List<PersonnelRole> DeletedRoles { get; set; }

        public PersonRecord()
        {
            AddedStudies = new List<PersonnelStudy>();
            DeletedStudies = new List<PersonnelStudy>();

            AddedComment = new List<PersonnelComment>();
            EditedComment = new List<PersonnelComment>();
            DeletedComment = new List<PersonnelComment>();

            AddedRoles = new List<PersonnelRole>();
            DeletedRoles = new List<PersonnelRole>();

            Item = new Person();
        }

        public PersonRecord (Person item)
        {
            AddedStudies = new List<PersonnelStudy>();
            DeletedStudies = new List<PersonnelStudy>();

            AddedComment = new List<PersonnelComment>();
            EditedComment = new List<PersonnelComment>();
            DeletedComment = new List<PersonnelComment>();

            AddedRoles = new List<PersonnelRole>();
            DeletedRoles = new List<PersonnelRole>();

            Item = item;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertPersonnel(Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdatePersonnel(Item) == 1)
                    return 1;

                Dirty = false;
            }

            SaveStudies();
            SaveComments();
            SaveRoles();

            return 0;
        }

        private void SaveStudies()
        {
            foreach (PersonnelStudy study in AddedStudies)
            {
                DBAction.InsertPersonnelStudy(study);
            }
            AddedStudies.Clear();

            foreach (PersonnelStudy study in DeletedStudies)
            {
                DBAction.DeleteRecord(study);
            }
            DeletedStudies.Clear();
        }

        private void SaveComments()
        {
            foreach (PersonnelComment comment in AddedComment)
            {
                DBAction.InsertPersonnelComment(comment);
            }
            AddedComment.Clear();

            foreach (PersonnelComment comment in EditedComment)
            {
                DBAction.UpdatePersonnelComment(comment);
            }
            EditedComment.Clear();

            foreach (PersonnelComment comment in DeletedComment)
            {
                DBAction.DeleteRecord(comment);
            }
            DeletedComment.Clear();
        }

        private void SaveRoles()
        {
            foreach (PersonnelRole comment in AddedRoles)
            {
                DBAction.InsertPersonnelRole(comment);
            }
            AddedComment.Clear();

            foreach (PersonnelRole comment in DeletedRoles)
            {
                DBAction.DeleteRecord(comment);
            }
            DeletedComment.Clear();
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
