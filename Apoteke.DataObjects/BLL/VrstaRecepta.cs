using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects;
using Apoteke.DataObjects.Core;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_VrstaRecepta_Ins", "sp_VrstaRecepta_Upd", "sp_VrstaRecepta_DelByID","sp_VrstaRecepta_SelAll","sp_VrstaRecepta_SelByID")]
    public class VrstaRecepta : ApotekaBase<VrstaRecepta>
    {
        [ObjectProperty("pNaziv", "Naziv")]
        public string Naziv
        {
            get;
            set;
        }

        public VrstaRecepta()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public VrstaRecepta(int id)
            : base(id)
        { }
    }
}
