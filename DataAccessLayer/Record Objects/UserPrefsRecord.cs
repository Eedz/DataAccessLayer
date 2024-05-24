using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCLib
{
    public class UserPrefsRecord : IRecord<UserPrefs>
    {
        public bool NewRecord { get; set; }
        public bool Dirty { get; set; }
        public UserPrefs Item { get; set; }

        public UserPrefsRecord(UserPrefs item)
        {
            Item = item;
        }

        public int SaveRecord()
        {
            if (Dirty)
            {
                if (DBAction.UpdateUser(Item) == 1)
                    return 1;

                Dirty = false;
            }

            return 0;
        }
    }
}
