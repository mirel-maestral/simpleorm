using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects;
using Apoteke.DataObjects.Core;
using MySql.Data.MySqlClient;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_Komitent_Ins", "sp_Komitent_Upd", "sp_Komitent_DelByID","sp_Komitent_SelAll","sp_Komitent_SelByID")]
    public class Komitent : ApotekaBase<Komitent>
    {
        [ObjectProperty("pNaziv", "Naziv")]
        public string Naziv
        {
            get;
            set;
        }

        [ObjectProperty("pMjesto", "Mjesto")]
        public string Mjesto
        {
            get;
            set;
        }

        [ObjectProperty("pAdresa", "Adresa")]
        public string Adresa
        {
            get;
            set;
        }

        [ObjectProperty("pRacun", "Racun")]
        public string Racun
        {
            get;
            set;
        }

        [ObjectProperty("pPDV", "PDV")]
        public string PDV
        {
            get;
            set;
        }

        [ObjectProperty("pIDB", "IDB")]
        public string IDB
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

        public Komitent()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public Komitent(int id)
            : base(id)
        { }

        public static bool Exist(string sifra)
        {
            using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
            {
                MySqlCommand command = new MySqlCommand("sp_ValidateSifraKomitenta", con);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("@pSifra", MySqlDbType.VarChar)).Value = sifra;
                command.Parameters.Add(new MySqlParameter("@pResult", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                command.ExecuteNonQuery();

                if ((int)command.Parameters["@pResult"].Value == 1)
                    return true;
            }

            return false;
        }

        public static Komitent GetByID(int id)
        {
            if (!Global.Instance.DisableCache)
            {
                Komitent komitentFromCache = CacheSync.Get<Komitent>(id) as Komitent;
                if (komitentFromCache != null)
                    return komitentFromCache;
            }

            Komitent komitentFromDb = ApotekeDB.Instance.GetKomitent(id);
            if (!Global.Instance.DisableCache)
                CacheSync.Sync<Komitent>(komitentFromDb);
            return komitentFromDb;
        }
    }
}
