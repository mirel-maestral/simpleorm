using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects;
using Apoteke.DataObjects.Core;
using MySql.Data.MySqlClient;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_Proizvodjac_Ins", "sp_Proizvodjac_Upd", "sp_Proizvodjac_DelByID", "sp_Proizvodjac_SelAll", "sp_Proizvodjac_SelByID")]
    public class Proizvodjac: ApotekaBase<Proizvodjac>
    {

        [ObjectProperty("pNaziv", "Naziv")]
        public string Naziv
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

        [ObjectProperty("pSifra", "Sifra")]
        public string Sifra
        {
            get;
            set;
        }

        [ObjectProperty("pTelefon", "Telefon")]
        public string Telefon
        {
            get;
            set;
        }

        [ObjectProperty("pEmail", "Email")]
        public string Email
        {
            get;
            set;
        }

        [ObjectProperty("pGrad", "Grad")]
        public string Grad
        {
            get;
            set;
        }

        [ObjectProperty("pPTT", "PTT")]
        public string PTT
        {
            get;
            set;
        }

        public Proizvodjac()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public Proizvodjac(int id)
            : base(id)
        {
        }

        public static bool Exist(string sifra)
        {
            using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
            {
                MySqlCommand command = new MySqlCommand("sp_ValidateSifraProizvodjaca", con);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("@pSifra", MySqlDbType.VarChar)).Value = sifra;
                command.Parameters.Add(new MySqlParameter("@pResult", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                command.ExecuteNonQuery();

                if ((int)command.Parameters["@pResult"].Value == 1)
                    return true;
            }

            return false;
        }

        public static Proizvodjac GetByID(int id)
        {
            if (id == 0)
                return null;
            
            if (!Global.Instance.DisableCache)
            {
                Proizvodjac proizvodjacFromCache = CacheSync.Get<Proizvodjac>(id) as Proizvodjac;
                if (proizvodjacFromCache != null)
                    return proizvodjacFromCache;
            }

            Proizvodjac proizvodjacFromDb = ApotekeDB.Instance.GetProizvodjac(id);

            if (!Global.Instance.DisableCache && proizvodjacFromDb != null)
                CacheSync.Sync<Proizvodjac>(proizvodjacFromDb);
            return proizvodjacFromDb;
        }



       

        public static List<Proizvodjac> GetAll()
        {
            return ApotekeDB.Instance.GetProizvodjac();
        }







    }
}
