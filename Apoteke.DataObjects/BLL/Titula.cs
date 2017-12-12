using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects.Core;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_Titula_Ins", "sp_Titula_Upd", "sp_Titula_DelByID",
        "sp_Titula_SelAll", "sp_Titula_SelByID")]
    public class Titula : ApotekaBase<Titula>
    {
        [ObjectProperty("pNaziv", "Naziv")]
        public string Naziv
        {
            get;
            set;
        }

        public Titula()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }
        public Titula(int id)
            : base(id)
        { }
    }
}
