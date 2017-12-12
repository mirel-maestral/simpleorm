using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects;
using Apoteke.DataObjects.Core;
using MySql.Data.MySqlClient;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_Korisnik_Ins", "sp_Korisnik_Upd", "sp_Korisnik_DelByID","sp_Korisnik_SelAll","sp_Korisnik_SelByID")]
    public class Korisnik: ApotekaBase<Korisnik>
    {
        [ObjectProperty("pIme", "Ime")]
        public string Ime
        {
            get;
            set;
        }

        [ObjectProperty("pPrezime", "Prezime")]
        public string Prezime
        {
            get;
            set;
        }

        [ObjectProperty("pKorisnickoIme", "KorisnickoIme")]
        public string KorisnickoIme
        {
            get;
            set;
        }

        [ObjectProperty("pLozinka", "Lozinka")]
        public string Lozinka
        {
            get;
            set;
        }

        [ObjectProperty("pEMail", "EMail")]
        public string EMail
        {
            get;
            set;
        }

        [ObjectProperty("pTitulaID", "TitulaID")]
        public int TitulaID
        {
            get;
            set;
        }

        [ObjectProperty("pAdmin", "Admin")]
        public bool IsAdmin
        {
            get;
            set;
        }

        public Titula Titula
        {
            get { return ApotekeDB.Instance.GetTitula(TitulaID); }
        }

        public Korisnik()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public Korisnik(int id)
            : base(id)
        { }

        public static int Validate(string korisnickoIme, string lozinka, ref int Prijavljen)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    if (con == null)
                        return 0;
                    MySqlCommand command = new MySqlCommand("sp_ValidateKorisnik", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pKorisnickoIme", MySqlDbType.VarChar)).Value = korisnickoIme;
                    command.Parameters.Add(new MySqlParameter("@pLozinka", MySqlDbType.VarChar)).Value = lozinka;
                    command.Parameters.Add(new MySqlParameter("@pID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(new MySqlParameter("@pPrijavljen", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                    command.ExecuteNonQuery();

                    Prijavljen = (int)command.Parameters["@pPrijavljen"].Value;
                    return (int)command.Parameters["@pId"].Value;
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom logovanja usera.", Logging.LogEntryLevel.Critical,
                    ex,new Logging.LogEntryAttribute("User", korisnickoIme));
                   
            }

            return 0;
        }

        public static void LogOut(int id)
        {
            using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
            {
                if (con == null)
                    return;

                MySqlCommand command = new MySqlCommand("sp_LogOutKorisnik", con);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("@pID", MySqlDbType.Int32)).Value = id;
                command.ExecuteNonQuery();
            }           
        }

        public static Korisnik GetByID(int id)
        {
            if (id == 0)
                return null;
            
            if (!Global.Instance.DisableCache)
            {
                Korisnik korisnikFromCache = CacheSync.Get<Korisnik>(id) as Korisnik;
                if (korisnikFromCache != null)
                    return korisnikFromCache;
            }

            Korisnik korisnikFromDb = ApotekeDB.Instance.GetKorisnik(id);
            if (!Global.Instance.DisableCache)
                CacheSync.Sync<Korisnik>(korisnikFromDb);
            return korisnikFromDb;
        }

        public static List<Korisnik> GetAll()
        {
            return ApotekeDB.Instance.GetKorisnici();
        }

        public static Korisnik GetAdmin()
        {
            foreach (Korisnik kor in GetAll())
                if (kor.IsAdmin)
                    return kor;

            return null;
        }
    }
}
