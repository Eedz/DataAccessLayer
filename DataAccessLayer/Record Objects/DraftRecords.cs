using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace ITCLib
{
    public class DraftQuestionRecord : DraftQuestion, IRecord
    {
        public int ID { get; set; }
        public int DraftID { get; set; }

        public bool Dirty { get; set; }
        public bool NewRecord { get; set; }

        public int SaveRecord()
        {
            return 1;
        }


    }

}
