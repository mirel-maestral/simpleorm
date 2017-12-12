using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects;
using Apoteke.DataObjects.Core;
using MySql.Data.MySqlClient;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_TipRobe_Ins", "sp_TipRobe_Upd", "sp_TipRobe_DelByID", "sp_TipRobe_SelAll", "sp_TipRobe_SelByID")]
    public class TipRobe : ApotekaBase<TipRobe>
    {
        [ObjectProperty("pSifra", "Sifra")]
        public string Sifra
        {
            get;
            set;
        }

        [ObjectProperty("pNaziv", "Naziv")]
        public string Naziv
        {
            get;
            set;
        }

        public TipRobe()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public TipRobe(int id)
            : base(id)
        { }

        public static bool Exist(string sifra)
        {
            using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
            {
                MySqlCommand command = new MySqlCommand("sp_ValidateSifraTipaRobe", con);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("@pSifra", MySqlDbType.VarChar)).Value = sifra;
                command.Parameters.Add(new MySqlParameter("@pResult", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                command.ExecuteNonQuery();

                if ((int)command.Parameters["@pResult"].Value == 1)
                    return true;
            }

            return false;
        }

        public static TipRobe GetByID(int id)
        {
            if (!Global.Instance.DisableCache)
            {
                TipRobe tipRobeFromCache = CacheSync.Get<TipRobe>(id) as TipRobe;
                if (tipRobeFromCache != null)
                    return tipRobeFromCache;
            }

            TipRobe tipRobeFromDb = ApotekeDB.Instance.GetTipRobe(id);

            if (!Global.Instance.DisableCache && tipRobeFromDb != null)
                CacheSync.Sync<TipRobe>(tipRobeFromDb);
            return tipRobeFromDb;
        }

        public static List<TipRobe> GetAll()
        {
            return ApotekeDB.Instance.GetTipRobe();
        }
    }
}
