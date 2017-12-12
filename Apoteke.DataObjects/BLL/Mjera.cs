using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects;
using Apoteke.DataObjects.Core;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_Mjera_Ins","sp_Mjera_Upd","sp_Mjera_DelByID","sp_Mjera_SelAll","sp_Mjera_SelByID")]
    public class Mjera: ApotekaBase<Mjera>
    {
        [ObjectProperty("pNaziv", "Naziv")]
        public string Naziv
        {
            get;
            set;
        }

        public Mjera()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public Mjera(int id)
            : base(id)
        {
        }
        public static Mjera GetByID(int id)
        {
            if (!Global.Instance.DisableCache)
            {
                Mjera mjeraFromCache = CacheSync.Get<Mjera>(id) as Mjera;
                if (mjeraFromCache != null)
                    return mjeraFromCache;
            }

            Mjera mjeraFromDb = ApotekeDB.Instance.GetMjera(id);
            if (!Global.Instance.DisableCache)
                CacheSync.Sync<Mjera>(mjeraFromDb);
            return mjeraFromDb;
        }
    }
}
