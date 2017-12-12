using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging
{
    public class LogEntryAttribute
    {
        public string Name
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public LogEntryAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
