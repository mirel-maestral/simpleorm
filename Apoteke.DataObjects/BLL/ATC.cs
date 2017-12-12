using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects.Core;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_ATC_Ins", "sp_ATC_Upd", "sp_ATC_DelByID", 
        "sp_ATC_SelAll", "sp_ATC_SelByID")]
    public class ATC: ObjectBase<ATC>
    {
        [ObjectProperty("pNaziv", "Naziv")]
        public string Naziv
        {
            get;
            set;
        }

        public ATC()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public ATC(int id)
            : base(id)
        { }
    }
}
