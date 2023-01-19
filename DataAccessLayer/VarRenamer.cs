using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    // TODO delete unused varnames after rename
    public class VarRenamer
    {
        RefVariableName OldName;
        RefVariableName NewName;
        List<Survey> SurveyList;
        List<string> ToDelete; // list of VarNames to delete

        public List<string> FailedRenames;

        public List<VarNameChangeRecord> Changes { get; set; }

        public VarRenamer()
        {
            OldName = new RefVariableName();
            NewName = new RefVariableName();
            SurveyList = new List<Survey>();
        }

        public VarRenamer(RefVariableName old, RefVariableName newname, List<Survey> surveys)
        {
            OldName = old;
            NewName = newname;
            SurveyList = surveys;
        }

        public void PerformRefRename()
        {
            FailedRenames = new List<string>();
            // create a list of change objects
            Changes = new List<VarNameChangeRecord>();
            foreach (Survey s in SurveyList)
            {
                VarNameChangeSurveyRecord sr = new VarNameChangeSurveyRecord();
                sr.NewRecord = true;
                sr.SurvID = s.SID;
                sr.SurveyCode = s.SurveyCode;

                string oldname = Utilities.ChangeCC(OldName.RefVarName, s.CountryCode);
                string newname = Utilities.ChangeCC(NewName.RefVarName, s.CountryCode);

                var existingChange = Changes.FirstOrDefault(x => x.NewName.Equals(newname));
                if (existingChange != null)
                {
                    existingChange.SurveysAffected.Add(sr);
                    continue;
                }

                VarNameChangeRecord change = new VarNameChangeRecord();
                change.NewRecord = true;
                change.OldName = oldname;
                change.NewName = newname;
                change.ChangeDate = DateTime.Now;
                

                change.SurveysAffected.Add(sr);

                Changes.Add(change);
            }

            FailedRenames.Clear();
            // rename vars in surveys
            foreach (Survey s in SurveyList)
            {
                if (RenameVariable(OldName.RefVarName, NewName.RefVarName, s.SurveyCode) == 1)
                {
                    FailedRenames.Add(s.SurveyCode);
                }
            }
            // remove failed surveys from list
            foreach (VarNameChangeRecord record in Changes)
            {
                List<VarNameChangeSurveyRecord> matches = record.SurveysAffected.Where(x => FailedRenames.Contains(x.SurveyCode)).ToList();
                foreach (var match in matches)
                    record.SurveysAffected.Remove(match);
            }

            // rename in wordings
            var successes = SurveyList.Where(x => !FailedRenames.Contains(x.SurveyCode));

            foreach (Survey s in successes)
            {
                UpdatePreP(OldName, NewName, s.SurveyCode);
                UpdatePreI(OldName, NewName, s.SurveyCode);
                UpdatePreA(OldName, NewName, s.SurveyCode);
                UpdateLitQ(OldName, NewName, s.SurveyCode);
                UpdatePstI(OldName, NewName, s.SurveyCode);
                UpdatePstP(OldName, NewName, s.SurveyCode);

                if (!DBAction.VarNameIsUsed(Utilities.ChangeCC(OldName.RefVarName, s.CountryCode)))
                    DBAction.DeleteVariable(Utilities.ChangeCC(OldName.RefVarName, s.CountryCode));
            }

            
        }

        

        private void UpdatePreP(RefVariableName oldname, RefVariableName newname, string survey)
        {
            // get a list of PrePs in this survey that contain the old name
            List<Wording> preps = DBAction.GetSurveyPreP(oldname.RefVarName, survey);

            foreach (Wording w in preps)
            {
                int oldID = w.WordID; // save the old ID
                // upsert the wording
                DBAction.UpdatePreP(w, oldname.RefVarName, newname.RefVarName, false); // potentially get a new ID

                if (oldID != w.WordID)
                {
                    DBAction.UpdateSurveyPreP(survey, oldID, w.WordID);
                }
            }
        }

        private void UpdatePreI(RefVariableName oldname, RefVariableName newname, string survey)
        {
            // get a list of PreIs in this survey that contain the old name
            List<Wording> preis = DBAction.GetSurveyPreI(oldname.RefVarName, survey);

            foreach (Wording w in preis)
            {
                int oldID = w.WordID; // save the old ID
                                      // upsert the wording
                DBAction.UpdatePreI(w, oldname.RefVarName, newname.RefVarName, false); // potentially get a new ID
                if (oldID != w.WordID)
                {
                    DBAction.UpdateSurveyPreI(survey, oldID, w.WordID);
                }
            }
        }

        private void UpdatePreA(RefVariableName oldname, RefVariableName newname, string survey)
        {
            // get a list of PreAs in this survey that contain the old name
            List<Wording> preas = DBAction.GetSurveyPreA(oldname.RefVarName, survey);

            foreach (Wording w in preas)
            {
                int oldID = w.WordID; // save the old ID
                                      // upsert the wording
                DBAction.UpdatePreA(w, oldname.RefVarName, newname.RefVarName, false); // potentially get a new ID
                if (oldID != w.WordID)
                {
                    DBAction.UpdateSurveyPreA(survey, oldID, w.WordID);
                }
            }
        }

        private void UpdateLitQ(RefVariableName oldname, RefVariableName newname, string survey)
        {
            // get a list of LitQs in this survey that contain the old name
            List<Wording> litqs = DBAction.GetSurveyLitQ(oldname.RefVarName, survey);

            foreach (Wording w in litqs)
            {
                int oldID = w.WordID; // save the old ID
                                      // upsert the wording
                DBAction.UpdateLitQ(w, oldname.RefVarName, newname.RefVarName, false); // potentially get a new ID
                if (oldID != w.WordID)
                {
                    DBAction.UpdateSurveyLitQ(survey, oldID, w.WordID);
                }
            }
        }

        private void UpdatePstI(RefVariableName oldname, RefVariableName newname, string survey)
        {
            // get a list of PstIs in this survey that contain the old name
            List<Wording> pstis = DBAction.GetSurveyPstI(oldname.RefVarName, survey);

            foreach (Wording w in pstis)
            {
                int oldID = w.WordID; // save the old ID
                                      // upsert the wording
                DBAction.UpdatePstI(w, oldname.RefVarName, newname.RefVarName, false); // potentially get a new ID
                if (oldID != w.WordID)
                {
                    DBAction.UpdateSurveyPstI(survey, oldID, w.WordID);
                }
            }
        }

        private void UpdatePstP(RefVariableName oldname, RefVariableName newname, string survey)
        {
            // get a list of PstPs in this survey that contain the old name
            List<Wording> pstps = DBAction.GetSurveyPstP(oldname.RefVarName, survey);

            foreach (Wording w in pstps)
            {
                int oldID = w.WordID; // save the old ID
                                      // upsert the wording
                DBAction.UpdatePstP(w, oldname.RefVarName, newname.RefVarName, false); // potentially get a new ID
                if (oldID != w.WordID)
                {
                    DBAction.UpdateSurveyPstP(survey, oldID, w.WordID);
                }
            }
        }

        private int RenameVariable(string oldname, string newname, string survey)
        {
            if (DBAction.RenameVariableName(oldname, newname, survey) == 1)
            {
                return 1;
            }
            return 0;
        }

        private void RenameVariable(List<VarNameChange> changes)
        {
            foreach (VarNameChange change in changes)
            {
                foreach (Survey s in change.SurveysAffected)
                {
                    if (DBAction.RenameVariableName(change.OldName, change.NewName, s.SurveyCode) == 1)
                    {
                        FailedRenames.Add(s.SurveyCode);
                    }
                }
            }
        }
    }
}
