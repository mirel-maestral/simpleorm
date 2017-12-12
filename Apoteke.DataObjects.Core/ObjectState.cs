using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apoteke.DataObjects.Core
{
    public enum ObjectState
    {
        None = 0,

        New = 1,

        Existing = 2,

        Dirty = 3
    }
}
