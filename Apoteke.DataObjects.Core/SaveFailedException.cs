using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apoteke.DataObjects.Core
{
    public class SaveFailedException: System.Data.DataException
    {
        public SaveFailedException(string message, System.Exception exception)
            : base(message,exception)
        { }

        public SaveFailedException(string message)
            : this(message, null)
        { }
    }
}
