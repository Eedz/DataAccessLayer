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
}
