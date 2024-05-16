using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class UserStateRecord : IRecord<UserState>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public UserState Item { get; set; }

        public UserStateRecord(UserState item)
        {
            Item = item;
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertUserState(this.Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdateUserState(this.Item) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }
}
