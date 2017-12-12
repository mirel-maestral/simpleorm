using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects.Core;
using MySql.Data.MySqlClient;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_MagNalog_Ins", "sp_MagNalog_Upd", "sp_MagNalog_DelByID",
       "sp_MagNalog_SelAll", "sp_MagNalog_SelByID")]
    public class MagistralaNalog : ApotekaBase<MagistralaNalog>
    {
        [ObjectProperty("pDatum", "Datum")]
        public DateTime Datum
        {
            get;
            set;
        }

        [ObjectProperty("pKolicina", "Kolicina")]
        public decimal Kolicina
        {
            get;
            set;
        }

        [ObjectProperty("pRobaID", "RobaID")]
        public int RobaID
        {
            get;
            set;
        }

        [ObjectProperty("pVrijednostSastojaka", "VrijednostSastojaka")]
        public decimal VrijednostSastojaka
        {
            get;
            set;
        }

        [ObjectProperty("pIznosTaksi", "IznosTaksi")]
        public decimal IznosTaksi
        {
            get;
            set;
        }

        [ObjectProperty("pCijena", "Cijena")]
        public decimal Cijena
        {
            get;
            set;
        }

        [ObjectProperty("pIznos", "Iznos")]
        public decimal Iznos
        {
            get;
            set;
        }

        [ObjectProperty("pOdstupanje", "Odstupanje")]
        public decimal Odstupanje
        {
            get;
            set;
        }

        [ObjectProperty("pStorniran", "Storniran")]
        public bool Storniran
        {
            get;
            set;
        }

        public Roba Roba
        {
            get { return Roba.GetByID(this.RobaID); }
        }

        public MagistralaNalog()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public MagistralaNalog(int id)
            : base(id)
        { }

        public static MagistralaNalog GetByID(int id)
        {
            if (id == 0)
                return null;

            if (!Global.Instance.DisableCache)
            {
                MagistralaNalog magNalogFromCache = CacheSync.Get<MagistralaNalog>(id) as MagistralaNalog;
                if (magNalogFromCache != null)
                    return magNalogFromCache;
            }

            MagistralaNalog magNalogFromDb = ApotekeDB.Instance.GetMagistralaNalog(id);

            if (!Global.Instance.DisableCache && magNalogFromDb != null)
                CacheSync.Sync<MagistralaNalog>(magNalogFromDb);
            return magNalogFromDb;
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
                MySqlCommand command = new MySqlCommand("sp_MagNalog_Ins", trans.Connection, trans);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;

                command.Parameters.Add(new MySqlParameter("pRobaID", MySqlDbType.Int32)).Value = this.RobaID;
                command.Parameters.Add(new MySqlParameter("pDatum", MySqlDbType.Date)).Value = this.Datum;
                command.Parameters.Add(new MySqlParameter("pVrijednostSastojaka", MySqlDbType.Decimal)).Value = this.VrijednostSastojaka;
                command.Parameters.Add(new MySqlParameter("pKolicina", MySqlDbType.Decimal)).Value = this.Kolicina;
                command.Parameters.Add(new MySqlParameter("pIznosTaksi", MySqlDbType.Decimal)).Value = this.IznosTaksi;
                command.Parameters.Add(new MySqlParameter("pOdstupanje", MySqlDbType.Decimal)).Value = this.Odstupanje;
                command.Parameters.Add(new MySqlParameter("pCijena", MySqlDbType.Decimal)).Value = this.Cijena;
                command.Parameters.Add(new MySqlParameter("pIznos", MySqlDbType.Decimal)).Value = this.Iznos;
                command.Parameters.Add(new MySqlParameter("pKreatorID", MySqlDbType.Int32)).Value = Global.Instance.LoggedKorisnik.ID;
                command.Parameters.Add(new MySqlParameter("pPoslovnaJedinica", MySqlDbType.Int32)).Value = this.PoslovnaJedinica;

                command.ExecuteNonQuery();

                int id = (int)command.Parameters["pID"].Value;
                this.ID = id;
                this.State = ObjectState.Existing;
                return true;
            }
            catch (SaveFailedException saveEx)
            {
                Logging.Log.Create("Insert (Transaction) MagNaloga. Neko je vec promjenio podatke.",
                    Logging.LogEntryLevel.Critical, saveEx);
                //throw saveEx;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom Insert (Transaction) MagNaloga.",
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
                MySqlCommand command = new MySqlCommand("sp_MagNalog_Upd", trans.Connection, trans);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Value = this.ID;
                command.Parameters.Add(new MySqlParameter("pRobaID", MySqlDbType.Int32)).Value = this.RobaID;
                command.Parameters.Add(new MySqlParameter("pDatum", MySqlDbType.Date)).Value = this.Datum;
                command.Parameters.Add(new MySqlParameter("pVrijednostSastojaka", MySqlDbType.Decimal)).Value = this.VrijednostSastojaka;
                command.Parameters.Add(new MySqlParameter("pKolicina", MySqlDbType.Decimal)).Value = this.Kolicina;
                command.Parameters.Add(new MySqlParameter("pIznosTaksi", MySqlDbType.Decimal)).Value = this.IznosTaksi;
                command.Parameters.Add(new MySqlParameter("pOdstupanje", MySqlDbType.Decimal)).Value = this.Odstupanje;
                command.Parameters.Add(new MySqlParameter("pCijena", MySqlDbType.Decimal)).Value = this.Cijena;
                command.Parameters.Add(new MySqlParameter("pIznos", MySqlDbType.Decimal)).Value = this.Iznos;
                command.Parameters.Add(new MySqlParameter("pModifikatorID", MySqlDbType.Int32)).Value = Global.Instance.LoggedKorisnik.ID;
                command.Parameters.Add(new MySqlParameter("pPoslovnaJedinica", MySqlDbType.Int32)).Value = this.PoslovnaJedinica;
                command.Parameters.Add(new MySqlParameter("pStorniran", MySqlDbType.Bit)).Value = this.Storniran;


                if (this.VrijemeModificiranja.Year == 1 && this.VrijemeModificiranja.Month == 1 &&
                    this.VrijemeModificiranja.Day == 1)
                    command.Parameters.Add(new MySqlParameter("pVrijemeModifikacije", MySqlDbType.DateTime)).Value = null;
                else
                    command.Parameters.Add(new MySqlParameter("pVrijemeModifikacije", MySqlDbType.DateTime)).Value = this.VrijemeModificiranja;

                command.Parameters["pVrijemeModifikacije"].Direction = System.Data.ParameterDirection.InputOutput;
                command.Parameters.Add(new MySqlParameter("pError", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;

                command.ExecuteNonQuery();

                int pError = (int)command.Parameters["pError"].Value;
                this.VrijemeModificiranja = (DateTime)command.Parameters["pVrijemeModifikacije"].Value;

                if (pError == -1)
                    throw new SaveFailedException("Neko je vec promjenio podatke.");

                this.State = ObjectState.Existing;
                return true;
            }
            catch (SaveFailedException saveEx)
            {
                Logging.Log.Create("Update (Transaction) MagNaloga. Neko je vec promjenio podatke.",
                    Logging.LogEntryLevel.Critical, saveEx);
                //throw saveEx;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom Update (Transaction) MagNaloga.",
                    Logging.LogEntryLevel.Critical, ex);
                //throw ex;
            }

            return false;
        }


        public static MagistralaNalog Create(MagistralaNalog nalog, List<MagistralaNalogStavka> magStavke)
        {
            MySqlTransaction trans = null;
            
            try
            {               
                MySqlConnection con = ApotekeDB.Instance.OpenConnection();
                trans = con.BeginTransaction();

                if (!nalog.Save(trans))
                {
                    trans.Rollback();
                    return null;
                }

                Roba magRoba = nalog.Roba;
                magRoba.Ulaz += nalog.Kolicina;
                magRoba.Zaliha += nalog.Kolicina;
                magRoba.MPC = nalog.Cijena;
                if (!magRoba.Save(trans))
                {
                    trans.Rollback();
                    return null;
                }

                foreach (MagistralaNalogStavka stavka in magStavke)
                {
                    if (stavka == null)
                        continue;
                    stavka.MagistralaNalogID = nalog.ID;

                    if (!stavka.Save(trans))
                    {
                        trans.Rollback();
                        return null;
                    }

                    Roba materijalRoba = stavka.MaterijalRoba;
                    materijalRoba.Ulaz -= stavka.Kolicina;
                    materijalRoba.Zaliha -= stavka.Kolicina;
                    if (!materijalRoba.Save(trans))
                    {
                        trans.Rollback();
                        return null;
                    }
                }

                trans.Commit();

                return nalog;
            }
            catch (SaveFailedException saveEx)
            {
                Logging.Log.Create("Neko je promjenio podatke.",
                       Logging.LogEntryLevel.Critical, saveEx,
                       new Logging.LogEntryAttribute("Operater", Global.Instance.LoggedKorisnik.KorisnickoIme));
                if (trans != null)
                    trans.Rollback();
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom pravljenja magistralnog naloga.",
                        Logging.LogEntryLevel.Critical, ex,
                        new Logging.LogEntryAttribute("Operater", Global.Instance.LoggedKorisnik.KorisnickoIme));
                if (trans != null)
                    trans.Rollback();
            }

            return null;
        }

        public int Storniraj()
        {
            MySqlTransaction trans = null;

            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {

                    trans = con.BeginTransaction();

                    MySqlCommand command = new MySqlCommand("sp_MagNalog_Storno", con, trans);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pMagNalogID", MySqlDbType.Int32)).Value = this.ID;
                    command.Parameters.Add(new MySqlParameter("@pKreatorID", MySqlDbType.Int32)).Value = Global.Instance.LoggedKorisnik.ID;

                    command.Parameters.Add(new MySqlParameter("@pStornoMagNalogID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                    command.ExecuteNonQuery();

                    int stornoMagNalogID = (int)command.Parameters["@pStornoMagNalogID"].Value;


                    MySqlCommand commandStavke = new MySqlCommand("sp_MagNalogStavke_Storno", con, trans);
                    commandStavke.CommandType = System.Data.CommandType.StoredProcedure;
                    commandStavke.Parameters.Add(new MySqlParameter("@pMagNalogID", MySqlDbType.Int32)).Value =this.ID;
                    commandStavke.Parameters.Add(new MySqlParameter("@pKreatorID", MySqlDbType.Int32)).Value = Global.Instance.LoggedKorisnik.ID;
                    commandStavke.Parameters.Add(new MySqlParameter("@pMagNalogStornoID", MySqlDbType.Int32)).Value = stornoMagNalogID;
                    commandStavke.ExecuteNonQuery();

                    Roba nalogRoba = this.Roba;
                    nalogRoba.Zaliha -= this.Kolicina;
                    nalogRoba.Ulaz -= this.Kolicina;

                    if (!nalogRoba.Save(trans))
                    {
                        trans.Rollback();
                        return -1;
                    }

                    trans.Commit();

                    Logging.Log.Create("MAgistralni Nalog je storniran.", Logging.LogEntryLevel.Informational, null,
                        new Logging.LogEntryAttribute("MagNalogID", this.ID.ToString()),
                        new Logging.LogEntryAttribute("Operater", Global.Instance.LoggedKorisnik.KorisnickoIme));

                    return stornoMagNalogID;
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom storniranja magistralnog naloga.", Logging.LogEntryLevel.Informational, null,
                        new Logging.LogEntryAttribute("MagNalogID", this.ID.ToString()),
                        new Logging.LogEntryAttribute("Operater", Global.Instance.LoggedKorisnik.KorisnickoIme));
                try
                {
                    if (trans != null)
                        trans.Rollback();
                }
                catch (System.Exception transEx)
                {
                    Logging.Log.Create("Greska prilikom rollbacka transakcije unutar storniranja magistralnog naloga.", Logging.LogEntryLevel.Critical,
                        transEx, new Logging.LogEntryAttribute("MagNalogID", this.ID.ToString()),
                        new Logging.LogEntryAttribute("Operater", Global.Instance.LoggedKorisnik.KorisnickoIme));
                }
            }

            return -1;
        }

       

    }
}
