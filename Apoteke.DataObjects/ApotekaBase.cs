using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects.Core;
using Apoteke.DataObjects.BLL;

namespace Apoteke.DataObjects
{
    public class ApotekaBase<T>: ObjectBase<T>
    {
        public Korisnik Modifikator
        {
            get { return Korisnik.GetByID(base.ModifikatorID); }
        }

        public Korisnik Kreator
        {
            get { return Korisnik.GetByID(base.KreatorID); }
        }

        public ApotekaBase(int id):base(id)
        {
        }

    }
}
