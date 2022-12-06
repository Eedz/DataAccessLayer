using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public enum ModifiedType { Undefined, Add, Change, Delete }

    public class ModifiedQuestion : QuestionRecord
    {
        // wordings
        public int OldPreP { get; set; }
        public int NewPreP { get; set; }

        public int OldPreI { get; set; }
        public int NewPreI { get; set; }

        public int OldPreA { get; set; }
        public int NewPreA { get; set; }

        public int OldLitQ { get; set; }
        public int NewLitQ { get; set; }

        public int OldPstI { get; set; }
        public int NewPstI { get; set; }

        public int OldPstP { get; set; }
        public int NewPstP { get; set; }

        public string OldRespName { get; set; }
        public string NewRespName { get; set; }
        
        public string OldNRName { get; set; }
        public string NewNRName { get; set; }

        public string OldVarLabel { get; set; }
        public string OldContent { get; set; }
        public string OldTopic { get; set; }
        public string OldProduct { get; set; }

        public ModifiedType ChangeType { get; set; }

    }
}
