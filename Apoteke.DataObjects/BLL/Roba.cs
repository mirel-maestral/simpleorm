using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects;
using Apoteke.DataObjects.Core;
using MySql.Data.MySqlClient;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_Roba_Ins", "sp_Roba_Upd", "sp_Roba_DelByID","sp_Roba_SelAll","sp_Roba_SelByID")]
    public class Roba : ApotekaBase<Roba>
    {
        #region Properties
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

        [ObjectProperty("pProizvodjacID", "ProizvodjacID")]
        public int ProizvodjacID
        {
            get;
            set;
        }

      

        [ObjectProperty("pFakturnaCijena", "FakturnaCijena")]
        public decimal FakturnaCijena
        {
            get;
            set;
        }

        [ObjectProperty("pPDVStopaID", "PDVStopaID")]
        public int PDVStopaID
        {
            get;
            set;
        }

        [ObjectProperty("pMjeraID", "MjeraID")]
        public int MjeraID
        {
            get;
            set;
        }
        
        [ObjectProperty("pPocetnoStanje", "PocetnoStanje")]
        public decimal PocetnoStanje
        {
            get;
            set;
        }

        [ObjectProperty("pUlaz", "Ulaz")]
        public decimal Ulaz
        {
            get;
            set;
        }

        [ObjectProperty("pIzlaz", "Izlaz")]
        public decimal Izlaz
        {
            get;
            set;
        }

        [ObjectProperty("pZaliha", "Zaliha")]
        public decimal Zaliha
        {
            get;
            set;
        }

        [ObjectProperty("pStopaMarze", "StopaMarze")]
        public decimal StopaMarze
        {
            get;
            set;
        }

        [ObjectProperty("pATC", "ATC")]
        public string  ATC
        {
            get;
            set;
        }

        [ObjectProperty("pReferalnaCijena", "ReferalnaCijena")]
        public decimal ReferalnaCijena
        {
            get;
            set;
        }

        [ObjectProperty("pParticipacija", "Participacija")]
        public int Participacija
        {
            get;
            set;
        }


        [ObjectProperty("pOpis", "Opis")]
        public string Opis
        {
            get;
            set;
        }

        [ObjectProperty("pMPC", "MPC")]
        public decimal MPC
        {
            get;
            set;
        }

        [ObjectProperty("pOznaka", "Oznaka")]
        public char Oznaka
        {
            get;
            set;
        }

        [ObjectProperty("pDatumCijene", "DatumCijene")]
        public DateTime DatumCijene
        {
            get;
            set;
        }

        [ObjectProperty("pVrstaProskripcije", "VrstaProskripcije")]
        public VrstaProskripcije VrstaProskripcije
        {
            get;
            set;
        }

        [ObjectProperty("pTaksa", "Taksa")]
        public bool Taksa
        {
            get;
            set;
        }
       
        public Proizvodjac Proizvodjac
        {
            get { return Proizvodjac.GetByID(this.ProizvodjacID); }
        }

        [ObjectProperty("pTipRobeID", "TipRobeId")]
        public int TipRobeID
        {
            get;
            set;
        }

        [ObjectProperty("pPopust", "Popust")]
        public decimal Popust
        {
            get;
            set;
        }

        [ObjectProperty("pDoplataBezPDV", "DoplataBezPDV")]
        public decimal DoplataBezPDV
        {
            get;
            set;
        }

        [ObjectProperty("pDoplataSaPDV", "DoplataSaPDV")]
        public decimal DoplataSaPDV
        {
            get;
            set;
        }

        [ObjectProperty("pRezimIzdavanja", "RezimIzdavanja")]
        public string RezimIzdavanja
        {
            get;
            set;
        }

        public TipRobe TipRobe
        {
            get { return TipRobe.GetByID(this.TipRobeID); }
        }

        public PDVStopa PDVStopa
        {
            get { return PDVStopa.GetByID(this.PDVStopaID); }
        }

        public Mjera Mjera
        {
            get { return Mjera.GetByID(this.MjeraID); }
        }

        public List<Normativ> Normativi
        {
            get { return ApotekeDB.Instance.GetNormativi(this.ID); }
        }

        public decimal LastFakturnaCijena
        {
            get { return ApotekeDB.Instance.GetLastFakturnaCijena(this.ID); }
        }

        #endregion

        #region Constructors
        public Roba()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public Roba(int id)
            : base(id)
        { }
        #endregion

        #region Methods
        public static bool Exist(string sifra)
        {
            using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
            {
                MySqlCommand command = new MySqlCommand("sp_ValidateSifraRobe", con);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("@pSifra", MySqlDbType.VarChar)).Value = sifra;
                command.Parameters.Add(new MySqlParameter("@pResult", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                command.ExecuteNonQuery();

                if ((int)command.Parameters["@pResult"].Value == 1)
                    return true;
            }

            return false;
        }

        public static Roba FindRoba(string searchParameter)
        {
            if (String.IsNullOrEmpty(searchParameter))
                return null;

            if (searchParameter.Length == 5)
                return FindRobaBySifra(searchParameter);

            return FindRobaByBarKod(searchParameter);
        }

        public static Roba FindRobaBySifra(string searchParameter)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_FindRobaBySifra", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlCommandBuilder.DeriveParameters(command);
                    command.Parameters["@pSearch"].Value = searchParameter;
                    MySqlDataReader dr = command.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                    if (dr != null && dr.Read())
                        return Factory.CreateRobaFromReader(dr);

                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom trazenja robe.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static Roba FindRobaByBarKod(string searchParameter)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_FindRobaByBarKod", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlCommandBuilder.DeriveParameters(command);
                    command.Parameters["@pSearch"].Value = searchParameter;
                    MySqlDataReader dr = command.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                    if (dr != null && dr.Read())
                        return Factory.CreateRobaFromReader(dr);

                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom trazenja robe.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static void Kumuliranje()
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_Kumuliranje", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom kumiliranja.",
                    Logging.LogEntryLevel.Critical, ex);
            }
        }

        public static void AzuriranjeKZZO()
        {
            try
            {
                if (String.IsNullOrEmpty(Global.Instance.Konfiguracija["Kanton"]))
                    return;
                string procName = String.Empty;
                if (Global.Instance.Konfiguracija["Kanton"] == "Sarajevo")
                    procName = "sp_Lista_sk";
                else if (Global.Instance.Konfiguracija["Kanton"] == "Zenica")
                    procName = "sp_Lista_zdk";

                if (String.IsNullOrEmpty(procName))
                    return;

                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand(procName, con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom kumiliranja.",
                    Logging.LogEntryLevel.Critical, ex);
            }
        }

        public virtual bool Save(MySqlTransaction trans)
        {
            if (this.State == ObjectState.New)
                return Insert(trans);
            else if (this.State == ObjectState.Existing)
                return Update(trans);

            return false;
        }
        private bool Insert(MySqlTransaction trans)
        {
            if (trans == null)
                return false;

            try
            {
                MySqlCommand command = new MySqlCommand("sp_Roba_Ins", trans.Connection, trans);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(new MySqlParameter("pSifra", MySqlDbType.String)).Value = this.Sifra;
                command.Parameters.Add(new MySqlParameter("pNaziv", MySqlDbType.String)).Value = this.Naziv;
                command.Parameters.Add(new MySqlParameter("pProizvodjacID", MySqlDbType.Int32)).Value = this.ProizvodjacID;
                command.Parameters.Add(new MySqlParameter("pFakturnaCijena", MySqlDbType.Decimal)).Value = this.FakturnaCijena;
                command.Parameters.Add(new MySqlParameter("pPDVStopaID", MySqlDbType.Int32)).Value = this.PDVStopaID;
                //command.Parameters.Add(new MySqlParameter("pPocetnoStanje", MySqlDbType.Decimal)).Value = this.PocetnoStanje;
                //command.Parameters.Add(new MySqlParameter("pUlaz", MySqlDbType.Decimal)).Value = this.Ulaz;
                //command.Parameters.Add(new MySqlParameter("pIzlaz", MySqlDbType.Decimal)).Value = this.Izlaz;
                //command.Parameters.Add(new MySqlParameter("pZaliha", MySqlDbType.Decimal)).Value = this.Zaliha;
                command.Parameters.Add(new MySqlParameter("pATC", MySqlDbType.String)).Value = this.ATC;
                command.Parameters.Add(new MySqlParameter("pReferalnaCijena", MySqlDbType.Decimal)).Value = this.ReferalnaCijena;

                command.Parameters.Add(new MySqlParameter("pParticipacija", MySqlDbType.Int32)).Value = this.Participacija;
                command.Parameters.Add(new MySqlParameter("pOpis", MySqlDbType.String)).Value = this.Opis;
                command.Parameters.Add(new MySqlParameter("pMPC", MySqlDbType.Decimal)).Value = this.MPC;
                command.Parameters.Add(new MySqlParameter("pOznaka", MySqlDbType.String)).Value = this.Oznaka;
                command.Parameters.Add(new MySqlParameter("pMjeraID", MySqlDbType.Int32)).Value = this.MjeraID;
                command.Parameters.Add(new MySqlParameter("pKreatorID", MySqlDbType.Int32)).Value = Global.Instance.LoggedKorisnik.ID;
                command.Parameters.Add(new MySqlParameter("pStopaMarze", MySqlDbType.Decimal)).Value = this.StopaMarze;
                //command.Parameters.Add(new MySqlParameter("pDatumCijene", MySqlDbType.Date)).Value = this.DatumCijene;
                command.Parameters.Add(new MySqlParameter("pVrstaProskripcije", MySqlDbType.Int32)).Value = this.VrstaProskripcije;
                command.Parameters.Add(new MySqlParameter("pPoslovnaJedinica", MySqlDbType.Int32)).Value = this.PoslovnaJedinica;
                command.Parameters.Add(new MySqlParameter("pTaksa", MySqlDbType.Bit)).Value = this.Taksa;

                command.ExecuteNonQuery();

                int id = (int)command.Parameters["pID"].Value;
                this.ID = id;
                this.State = ObjectState.Existing;
                CacheSync.Sync<Roba>(this);
                return true;
            }
            catch (SaveFailedException saveEx)
            {
                Logging.Log.Create("Insert (Transaction) Roba. Neko je vec promjenio podatke.",
                  Logging.LogEntryLevel.Critical, saveEx);
                //throw saveEx;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom Save (Transaction) Robe.",
                    Logging.LogEntryLevel.Critical, ex);
                //throw ex;
            }

            return false;
        }

        private bool Update(MySqlTransaction trans)
        {
            if (trans == null)
                return false;

            try
            {
                MySqlCommand command = new MySqlCommand("sp_Roba_Upd", trans.Connection, trans);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Value = this.ID;
                command.Parameters.Add(new MySqlParameter("pSifra", MySqlDbType.String)).Value = this.Sifra;
                command.Parameters.Add(new MySqlParameter("pNaziv", MySqlDbType.String)).Value = this.Naziv;
                command.Parameters.Add(new MySqlParameter("pProizvodjacID", MySqlDbType.Int32)).Value = this.ProizvodjacID;
                command.Parameters.Add(new MySqlParameter("pFakturnaCijena", MySqlDbType.Decimal)).Value = this.FakturnaCijena;
                command.Parameters.Add(new MySqlParameter("pPDVStopaID", MySqlDbType.Int32)).Value = this.PDVStopaID;
                command.Parameters.Add(new MySqlParameter("pPocetnoStanje", MySqlDbType.Decimal)).Value = this.PocetnoStanje;
                command.Parameters.Add(new MySqlParameter("pUlaz", MySqlDbType.Decimal)).Value = this.Ulaz;
                command.Parameters.Add(new MySqlParameter("pIzlaz", MySqlDbType.Decimal)).Value = this.Izlaz;
                command.Parameters.Add(new MySqlParameter("pZaliha", MySqlDbType.Decimal)).Value = this.Zaliha;
                command.Parameters.Add(new MySqlParameter("pATC", MySqlDbType.String)).Value = this.ATC;
                command.Parameters.Add(new MySqlParameter("pReferalnaCijena", MySqlDbType.Decimal)).Value = this.ReferalnaCijena;
                command.Parameters.Add(new MySqlParameter("pParticipacija", MySqlDbType.Int32)).Value = this.Participacija;
                command.Parameters.Add(new MySqlParameter("pOpis", MySqlDbType.String)).Value = this.Opis;
                command.Parameters.Add(new MySqlParameter("pMPC", MySqlDbType.Decimal)).Value = this.MPC;
                command.Parameters.Add(new MySqlParameter("pOznaka", MySqlDbType.String)).Value = this.Oznaka;
                command.Parameters.Add(new MySqlParameter("pMjeraID", MySqlDbType.Int32)).Value = this.MjeraID;
                command.Parameters.Add(new MySqlParameter("pModifikatorID", MySqlDbType.Int32)).Value = this.ModifikatorID;
                command.Parameters.Add(new MySqlParameter("pStopaMarze", MySqlDbType.Decimal)).Value = this.StopaMarze;
                command.Parameters.Add(new MySqlParameter("pDatumCijene", MySqlDbType.Date)).Value = this.DatumCijene;
                command.Parameters.Add(new MySqlParameter("pVrstaProskripcije", MySqlDbType.Int32)).Value = this.VrstaProskripcije;
                command.Parameters.Add(new MySqlParameter("pTaksa", MySqlDbType.Bit)).Value = this.Taksa;
                command.Parameters.Add(new MySqlParameter("pTipRobeID", MySqlDbType.Int32)).Value = this.TipRobeID;

                if (this.VrijemeModificiranja.Year == 1 && this.VrijemeModificiranja.Month == 1 &&
                   this.VrijemeModificiranja.Day == 1)
                    command.Parameters.Add(new MySqlParameter("pVrijemeModifikacije", MySqlDbType.DateTime)).Value = null;
                else
                    command.Parameters.Add(new MySqlParameter("pVrijemeModifikacije", MySqlDbType.DateTime)).Value = this.VrijemeModificiranja;

                //command.Parameters.Add(new MySqlParameter("pVrijemeModifikacije", MySqlDbType.DateTime)).Value = DateTime.Now;
                command.Parameters["pVrijemeModifikacije"].Direction = System.Data.ParameterDirection.InputOutput;
                command.Parameters.Add(new MySqlParameter("pError", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;

                command.ExecuteNonQuery();

                int pError = (int)command.Parameters["pError"].Value;
                this.VrijemeModificiranja = (DateTime)command.Parameters["pVrijemeModifikacije"].Value;

                if (pError == -1)
                    throw new SaveFailedException("Neko je vec promjenio podatke.");
                //throw new ApplicationException("OBARANJE APPLIKACIJE");

                CacheSync.Sync<Roba>(this);
                return true;
            }
            catch (SaveFailedException saveEx)
            {
                //throw saveEx;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom Save (Transaction) Robe.",
                    Logging.LogEntryLevel.Critical, ex);
                //throw ex;
            }

            return false;
        }


        public static Roba GetByID(int id, bool forceFromDb)
        {
            if (!forceFromDb && !Global.Instance.DisableCache)
            {
                Roba robaFromCache = CacheSync.Get<Roba>(id) as Roba;
                if (robaFromCache != null)
                    return robaFromCache;
            }

            Roba robaFromDb = ApotekeDB.Instance.GetRoba(id);
            if (!Global.Instance.DisableCache)
                CacheSync.Sync<Roba>(robaFromDb);
            return robaFromDb;
        }

        public static Roba GetByID(int id)
        {
            return GetByID(id, false);
        }

        public static List<Roba>  GetAll()
        {
            return ApotekeDB.Instance.GetRoba(false);
        }

        public static List<Roba>  GetMagistralnaRoba()
        {
            return ApotekeDB.Instance.GetRobaZaMagistralu();
        }
        #endregion
    }
}
