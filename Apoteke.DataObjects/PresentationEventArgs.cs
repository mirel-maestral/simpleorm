using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apoteke.DataObjects
{
    public class PresentationEventArgs: EventArgs
    {
        public int RecordCount
        {
            get;
            set;
        }

        public int RecordsAffected
        {
            get;
            set;
        }

        public string Info
        {
            get;
            set;
        }

        public PresentationEventArgs(int recordCount, int recordsAffected, string info)
        {
            RecordCount = recordCount;
            RecordsAffected = recordsAffected;
            Info = info;
        }
    }
}
