using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects;
using Apoteke.DataObjects.Core;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_Proskripcija_Ins", "sp_Proskripcija_Upd", "sp_Proskripcija_DelByID",
        "sp_Proskripcija_SelAll", "sp_Proskripcija_SelByID")]
    public class Proskripcija : ApotekaBase<Proskripcija>
    {
        [ObjectProperty("pNaziv", "Naziv")]
        public string Naziv
        {
            get;
            set;
        }

        [ObjectProperty("pSifra", "Sifra")]
        public string Sifra
        {
            get;
            set;
        }

        [ObjectProperty("pVrsta", "Vrsta")]
        public MagistralnaVrsta Vrsta
        {
            get;
            set;
        }

        public List<Normativ> Normativi
        {
            get { return ApotekeDB.Instance.GetNormativi(this.ID); }
        }
        public Proskripcija()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public Proskripcija(int id)
            : base(id)
        { }
    }
}
