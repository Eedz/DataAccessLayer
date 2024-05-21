using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace ITCLib
{
    public class DraftQuestionRecord : IRecord <DraftQuestion>
    {
        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }

        public DraftQuestion Item { get; set; }

        public DraftQuestionRecord (DraftQuestion question)
        {
            Item = question;
        }

        public int SaveRecord()
        {
            return 0;
        }


    }

}
