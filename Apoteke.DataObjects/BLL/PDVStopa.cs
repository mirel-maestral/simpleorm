using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects;
using Apoteke.DataObjects.Core;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_PDVStopa_Ins", "sp_PDVStopa_Upd", "sp_PDVStopa_DelByID", "sp_PDVStopa_SelAll", "sp_PDVStopa_SelByID")]
    public class PDVStopa : ApotekaBase<PDVStopa>
    {
        /// <summary>
        /// Tarifni Broj.
        /// </summary>
        [ObjectProperty("pTBR", "TBR")]
        public string TBR
        {
            get;
            set;
        }
        
        /// <summary>
        /// PDV Stopa.
        /// </summary>
        [ObjectProperty("pStopa", "Stopa")]
        public decimal Stopa
        {
            get;
            set;
        }

        /// <summary>
        /// Naziv pdv stope.
        /// </summary>
        [ObjectProperty("pNaziv", "Naziv")]
        public string Naziv
        {
            get;
            set;
        }

        /// <summary>
        /// Opis pdv Stope.
        /// </summary>
        [ObjectProperty("pOpis", "Opis")]
        public string Opis
        {
            get;
            set;
        }

        [ObjectProperty("pUplatniRacun", "UplatniRacun")]
        public string UplatniRacun
        {
            get;
            set;
        }

        public PDVStopa()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public PDVStopa(int id)
            : base(id)
        { }

        public static PDVStopa GetByID(int id)
        {
            if (id == 0)
                return null;

            if (!Global.Instance.DisableCache)
            {
                PDVStopa pdvStopaFromCache = CacheSync.Get<PDVStopa>(id) as PDVStopa;
                if (pdvStopaFromCache != null)
                    return pdvStopaFromCache;
            }

            PDVStopa pdvStopaFromDb = ApotekeDB.Instance.GetPDVStopa(id);
            if (!Global.Instance.DisableCache)
                CacheSync.Sync<PDVStopa>(pdvStopaFromDb);
            return pdvStopaFromDb;
        }
    }
}
