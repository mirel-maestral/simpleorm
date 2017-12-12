using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Apoteke.DataObjects;
using Apoteke.DataObjects.Core;
using MySql.Data.MySqlClient;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_StavkaRacuna_Ins", "sp_StavkaRacuna_Upd", "sp_StavkaRacuna_DelByID",
        "sp_StavkaRacuna_SelAll", "sp_StavkaRacuna_SelByID")]
    public class RacunStavka : ApotekaBase<RacunStavka>
    {
        #region Properties
        [ObjectProperty("pRacunID", "RacunID")]
        public int RacunID
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

        //[ObjectProperty("pMjeraID", "MjeraID")]
        //public int MjeraID
        //{
        //    get;
        //    set;
        //}

        [ObjectProperty("pKolicina", "Kolicina")]
        public decimal Kolicina
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

        [ObjectProperty("pVrijednost", "Vrijednost")]
        public decimal Vrijednost
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

        [ObjectProperty("pIznosPopusta", "IznosPopusta")]
        public decimal IznosPopusta
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

        [ObjectProperty("pPDVStopa", "PDVStopa")]
        public decimal PDVStopa
        {
            get;
            set;
        }

        [ObjectProperty("pLjekarID", "LjekarID")]
        public int LjekarID
        {
            get;
            set;
        }

        [ObjectProperty("pVrstaID", "VrstaID")]
        public int VrstaID
        {
            get;
            set;
        }

        [ObjectProperty("pJMBG", "JMBG")]
        public string JMBG
        {
            get;
            set;
        }

        [ObjectProperty("pPausal", "Pausal")]
        public decimal Pausal
        {
            get;
            set;
        }

        [ObjectProperty("pBrojRecepta", "BrojRecepta")]
        public string BrojRecepta
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

        [ObjectProperty("pIznosTakse", "IznosTakse")]
        public decimal IznosTakse
        {
            get;
            set;
        }

        [ObjectProperty("pBrJedinica", "BrJedinica")]
        public int BrJedinica
        {
            get;
            set;
        }

        [ObjectProperty("pIznosKupac", "IznosKupac")]
        public decimal IznosKupac
        {
            get;
            set;
        }

        [ObjectProperty("pIznosKzzo", "IznosKzzo")]
        public decimal IznosKzzo
        {
            get;
            set;
        }

        [ObjectProperty("pStornirano", "Stornirano")]
        public bool Stornirano
        {
            get;
            set;
        }
        //eso 
        [ObjectProperty("pDatumPropisivanja", "DatumPropisivanja")]
        public DateTime DatumPropisivanja
        {
            get;
            set;
        }
        //end eso

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

        [ObjectProperty("pPonovljenRecept", "PonovljenRecept")]
        public int PonovljenRecept
        {
            get;
            set;
        }

        [ObjectProperty("pPropisanaKolicina", "PropisanaKolicina")]
        public decimal PropisanaKolicina
        {
            get;
            set;
        }

        public Racun Racun
        {
            get { return ApotekeDB.Instance.GetRacun(this.RacunID); }
        }
        
        
        public Roba Roba
        {
            get { return Roba.GetByID(this.RobaID, true); }
        }

        //public Mjera Mjera
        //{
        //    get { return Mjera.GetByID(this.MjeraID); }
        //}

        public Ljekar Ljekar
        {
            get { return ApotekeDB.Instance.GetLjekar(this.LjekarID); }
        }

        public VrstaRecepta Vrsta
        {
            get { return ApotekeDB.Instance.GetVrstaRecepta(this.VrstaID); }
        }
        #endregion

        #region Constructors
        public RacunStavka()
            : this(0)
        {
            //int pj = 0;
            //int.TryParse(Global.Instance.Konfiguracija["BrojApoteke"], out pj);
            //this.PoslovnaJedinica = pj;

            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public RacunStavka(int id)
            : base(id)
        { }
        #endregion

        #region Methods
        public RacunStavka Copy()
        {
            RacunStavka stavka = new RacunStavka();
            stavka.BrojRecepta = this.BrojRecepta;
            stavka.Cijena = this.Cijena;
            stavka.Izlaz = this.Izlaz;
            stavka.Iznos = this.Iznos;
            stavka.IznosPopusta = this.IznosPopusta;
            stavka.JMBG = this.JMBG;
            stavka.Kolicina = this.Kolicina;
            stavka.KreatorID = this.KreatorID;
            stavka.LjekarID = this.LjekarID;
            //stavka.MjeraID = this.MjeraID;
            stavka.ModifikatorID = this.ModifikatorID;
            stavka.Participacija = this.Participacija;
            stavka.Pausal = this.Pausal;
            stavka.PDVStopa = this.PDVStopa;
            stavka.RacunID = this.RacunID;
            stavka.ReferalnaCijena = this.ReferalnaCijena;
            stavka.Popust = this.Popust;
            stavka.DoplataBezPDV = this.DoplataBezPDV;
            stavka.DoplataSaPDV = this.DoplataSaPDV;
            stavka.RobaID = this.RobaID;
            stavka.State = ObjectState.New;
            stavka.Vrijednost = this.Vrijednost;
            stavka.VrijemeKreiranja = this.VrijemeKreiranja;
            stavka.VrijemeModificiranja = this.VrijemeModificiranja;
            stavka.VrstaID = this.VrstaID;
            stavka.Zaliha = this.Zaliha;
            stavka.DatumPropisivanja = this.DatumPropisivanja;
            stavka.PonovljenRecept = this.PonovljenRecept;
            stavka.PropisanaKolicina = this.PropisanaKolicina;
            return stavka;
        }
        
        public Recept ExtractRecept()
        {
            Recept existingRecept = new Recept();
            existingRecept.BrJedinica = this.BrJedinica;
            existingRecept.BrojRecepta = this.BrojRecepta;
            existingRecept.Participacija = this.Participacija;
            existingRecept.Pausal = this.Pausal;
            existingRecept.VrstaID = this.VrstaID;
            existingRecept.LjekarID = this.LjekarID;
            existingRecept.JMBG = this.JMBG;
            existingRecept.StavkaRacunaID = this.ID;
            existingRecept.DatumPropisivanja = this.DatumPropisivanja;
            existingRecept.PonovljenRecept = this.PonovljenRecept;
            existingRecept.PropisanaKolicina = this.PropisanaKolicina;
            

            return existingRecept;
        }


        public RacunStavka Storniraj(Racun racun)
        {
            MySqlTransaction trans = null;

            try
            {
                MySqlConnection con = ApotekeDB.Instance.OpenConnection();

                trans = con.BeginTransaction();

                if (!racun.Save(trans))
                {
                    trans.Rollback();
                    return null;
                }

                MySqlCommand commandStavke = new MySqlCommand("sp_StornoStavkaRacuna", con, trans);
                commandStavke.CommandType = System.Data.CommandType.StoredProcedure;
                commandStavke.Parameters.Add(new MySqlParameter("@pStavkaRacunaID", MySqlDbType.Int32)).Value = this.ID;
                commandStavke.Parameters.Add(new MySqlParameter("@pModifikatorID", MySqlDbType.Int32)).Value = Global.Instance.LoggedKorisnik.ID;
                commandStavke.Parameters.Add(new MySqlParameter("@pStornoRacunID", MySqlDbType.Int32)).Value = racun.ID;
                commandStavke.Parameters.Add(new MySqlParameter("@pStornoStavkaID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                
                commandStavke.ExecuteNonQuery();

                trans.Commit();

                Logging.Log.Create("Stavka Racuna je stornirana.", Logging.LogEntryLevel.Informational, null,
                    new Logging.LogEntryAttribute("StackaRacunaID", this.ID.ToString()),
                    new Logging.LogEntryAttribute("Operater", Global.Instance.LoggedKorisnik.KorisnickoIme));

                int stornoId = (int)commandStavke.Parameters["@pStornoStavkaID"].Value;
                return ApotekeDB.Instance.GetStavkuRacuna(stornoId);
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom storniranja stavke racuna.", Logging.LogEntryLevel.Informational, null,
                        new Logging.LogEntryAttribute("StavkaRacunaID", this.ID.ToString()),
                        new Logging.LogEntryAttribute("Operater", Global.Instance.LoggedKorisnik.KorisnickoIme));

                if (trans != null)
                    trans.Rollback();
            }
            finally
            {
                if (trans != null && trans.Connection != null)
                    trans.Connection.Close();
            }

            return null;
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
                MySqlCommand command = new MySqlCommand("sp_StavkaRacuna_Ins", trans.Connection, trans);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(new MySqlParameter("pRacunID", MySqlDbType.Int32)).Value = this.RacunID;
                command.Parameters.Add(new MySqlParameter("pRobaID", MySqlDbType.Int32)).Value = this.RobaID;
                //command.Parameters.Add(new MySqlParameter("pMjeraID", MySqlDbType.Int32)).Value = this.MjeraID;
                command.Parameters.Add(new MySqlParameter("pKolicina", MySqlDbType.Decimal)).Value = this.Kolicina;
                command.Parameters.Add(new MySqlParameter("pReferalnaCijena", MySqlDbType.Decimal)).Value = this.ReferalnaCijena;
                command.Parameters.Add(new MySqlParameter("pPopust", MySqlDbType.Decimal)).Value = this.Popust;
                command.Parameters.Add(new MySqlParameter("pDoplataBezPDV", MySqlDbType.Decimal)).Value = this.DoplataBezPDV;
                command.Parameters.Add(new MySqlParameter("pDoplataSaPDV", MySqlDbType.Decimal)).Value = this.DoplataSaPDV;
                command.Parameters.Add(new MySqlParameter("pIznosPopusta", MySqlDbType.Decimal)).Value = this.IznosPopusta;
                command.Parameters.Add(new MySqlParameter("pParticipacija", MySqlDbType.Int32)).Value = this.Participacija;
                command.Parameters.Add(new MySqlParameter("pPDVStopa", MySqlDbType.Decimal)).Value = this.PDVStopa;
                command.Parameters.Add(new MySqlParameter("pCijena", MySqlDbType.Decimal)).Value = this.Cijena;
                command.Parameters.Add(new MySqlParameter("pVrijednost", MySqlDbType.Decimal)).Value = this.Vrijednost;
                command.Parameters.Add(new MySqlParameter("pIznos", MySqlDbType.Decimal)).Value = this.Iznos;
                command.Parameters.Add(new MySqlParameter("pLjekarID", MySqlDbType.Int32)).Value = this.LjekarID;
                command.Parameters.Add(new MySqlParameter("pVrstaID", MySqlDbType.Int32)).Value = this.VrstaID;
                command.Parameters.Add(new MySqlParameter("pJMBG", MySqlDbType.String)).Value = this.JMBG;
                command.Parameters.Add(new MySqlParameter("pPausal", MySqlDbType.Decimal)).Value = this.Pausal;
                command.Parameters.Add(new MySqlParameter("pBrojRecepta", MySqlDbType.String)).Value = this.BrojRecepta;
                command.Parameters.Add(new MySqlParameter("pIzlaz", MySqlDbType.Decimal)).Value = this.Izlaz;
                command.Parameters.Add(new MySqlParameter("pZaliha", MySqlDbType.Decimal)).Value = this.Zaliha;
                command.Parameters.Add(new MySqlParameter("pKreatorID", MySqlDbType.Int32)).Value = Global.Instance.LoggedKorisnik.ID;
                command.Parameters.Add(new MySqlParameter("pPoslovnaJedinica", MySqlDbType.Int32)).Value = this.PoslovnaJedinica;
                command.Parameters.Add(new MySqlParameter("pIznosTakse", MySqlDbType.Decimal)).Value = this.IznosTakse;
                command.Parameters.Add(new MySqlParameter("pIznosKupac", MySqlDbType.Decimal)).Value = this.IznosKupac;
                command.Parameters.Add(new MySqlParameter("pIznosKzzo", MySqlDbType.Decimal)).Value = this.IznosKzzo;
                command.Parameters.Add(new MySqlParameter("pBrJedinica", MySqlDbType.Int32)).Value = this.BrJedinica;
                
                command.Parameters.Add(new MySqlParameter("pPonovljenRecept", MySqlDbType.Int32)).Value = this.PonovljenRecept;
                command.Parameters.Add(new MySqlParameter("pPropisanaKolicina", MySqlDbType.Decimal)).Value = this.PropisanaKolicina;
                
                //command.Parameters.Add(new MySqlParameter("pStornirano", MySqlDbType.Bit)).Value = this.Stornirano;
                //eso
                //MessageBox.Show(this.DatumPropisivanja);

                command.Parameters.Add(new MySqlParameter("pDatumPropisivanja", MySqlDbType.Date)).Value = this.DatumPropisivanja;
                //end eso

                command.ExecuteNonQuery();

                int id = (int)command.Parameters["pID"].Value;
                this.ID = id;
                this.State = ObjectState.Existing;
                return true;
            }
            catch (SaveFailedException saveEx)
            {
                Logging.Log.Create("Insert (Transaction) Stavke Racuna. Neko je vec promjenio podatke.",
                   Logging.LogEntryLevel.Critical, saveEx);
                //throw saveEx;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom Save (Transaction) Stavke Racuna.",
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
                MySqlCommand command = new MySqlCommand("sp_StavkaRacuna_Upd", trans.Connection, trans);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Value = this.ID;
                command.Parameters.Add(new MySqlParameter("pRacunID", MySqlDbType.Int32)).Value = this.RacunID;
                command.Parameters.Add(new MySqlParameter("pRobaID", MySqlDbType.Int32)).Value = this.RobaID;
                //command.Parameters.Add(new MySqlParameter("pMjeraID", MySqlDbType.Int32)).Value = this.MjeraID;
                command.Parameters.Add(new MySqlParameter("pKolicina", MySqlDbType.Decimal)).Value = this.Kolicina;
                command.Parameters.Add(new MySqlParameter("pReferalnaCijena", MySqlDbType.Decimal)).Value = this.ReferalnaCijena;
                command.Parameters.Add(new MySqlParameter("pParticipacija", MySqlDbType.Int32)).Value = this.Participacija;
                command.Parameters.Add(new MySqlParameter("pPDVStopa", MySqlDbType.Decimal)).Value = this.PDVStopa;
                command.Parameters.Add(new MySqlParameter("pCijena", MySqlDbType.Decimal)).Value = this.Cijena;
                command.Parameters.Add(new MySqlParameter("pVrijednost", MySqlDbType.Decimal)).Value = this.Vrijednost;
                command.Parameters.Add(new MySqlParameter("pIznos", MySqlDbType.Decimal)).Value = this.Iznos;
                command.Parameters.Add(new MySqlParameter("pLjekarID", MySqlDbType.Int32)).Value = this.LjekarID;
                command.Parameters.Add(new MySqlParameter("pVrstaID", MySqlDbType.Int32)).Value = this.VrstaID;
                command.Parameters.Add(new MySqlParameter("pJMBG", MySqlDbType.String)).Value = this.JMBG;
                command.Parameters.Add(new MySqlParameter("pPausal", MySqlDbType.Decimal)).Value = this.Pausal;
                command.Parameters.Add(new MySqlParameter("pBrojRecepta", MySqlDbType.String)).Value = this.BrojRecepta;
                command.Parameters.Add(new MySqlParameter("pIzlaz", MySqlDbType.Decimal)).Value = this.Izlaz;
                command.Parameters.Add(new MySqlParameter("pZaliha", MySqlDbType.Decimal)).Value = this.Zaliha;
                command.Parameters.Add(new MySqlParameter("pModifikatorID", MySqlDbType.Int32)).Value = this.ModifikatorID;
                command.Parameters.Add(new MySqlParameter("pIznosTakse", MySqlDbType.Decimal)).Value = this.IznosTakse;
                command.Parameters.Add(new MySqlParameter("pIznosKupac", MySqlDbType.Decimal)).Value = this.IznosKupac;
                command.Parameters.Add(new MySqlParameter("pIznosKzzo", MySqlDbType.Decimal)).Value = this.IznosKzzo;
                command.Parameters.Add(new MySqlParameter("pBrJedinica", MySqlDbType.Int32)).Value = this.BrJedinica;
                command.Parameters.Add(new MySqlParameter("pStornirano", MySqlDbType.Bit)).Value = this.Stornirano;

                command.Parameters.Add(new MySqlParameter("pPonovljenRecept", MySqlDbType.Int32)).Value = this.PonovljenRecept;
                command.Parameters.Add(new MySqlParameter("pPropisanaKolicina", MySqlDbType.Decimal)).Value = this.PropisanaKolicina;

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

                this.State = ObjectState.Existing;
                return true;
            }
            catch (SaveFailedException saveEx)
            {
                Logging.Log.Create("Update (Transaction) Stavke Racuna. Neko je vec promjenio podatke.",
                   Logging.LogEntryLevel.Critical, saveEx);
                //throw saveEx;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom Save (Transaction) Stavke Racuna.",
                    Logging.LogEntryLevel.Critical, ex);
                //throw ex;
            }

            return false;
        }
        public static RacunStavka Create(Racun noviRacun, Roba selectedRoba, ref Recept lastRecept, decimal kolicina)
        {
             MySqlTransaction trans = null;
            try
            {               
                MySqlConnection con = ApotekeDB.Instance.OpenConnection();
                trans = con.BeginTransaction();

                if (!noviRacun.Save(trans))
                {
                    trans.Rollback();
                    return null;
                }

               // KREIRANJE STAVKE
                RacunStavka novaStavka = new RacunStavka();
                novaStavka.State = Apoteke.DataObjects.Core.ObjectState.New;
                novaStavka.RacunID = noviRacun.ID;
                novaStavka.RobaID = selectedRoba.ID;
                //novaStavka.MjeraID = selectedRoba.MjeraID;
                novaStavka.Kolicina = kolicina;
                novaStavka.Participacija = selectedRoba.Participacija;
                novaStavka.ReferalnaCijena = selectedRoba.ReferalnaCijena;
                novaStavka.Popust = selectedRoba.Popust;
                novaStavka.DoplataBezPDV = selectedRoba.DoplataBezPDV;
                
                //jasenko
                novaStavka.DoplataSaPDV = selectedRoba.DoplataSaPDV;
                
                novaStavka.PDVStopa = selectedRoba.PDVStopa.Stopa;
                novaStavka.Cijena = selectedRoba.MPC;

                // KREIRANJE RECEPTA
                if (lastRecept == null)
                {
                    //Decimal iznosPopust;
                    //Decimal popust = Math.Round(Decimal.Divide(novaStavka.Popust, 100),2);

                    //iznosPopust = Math.Round(Math.Round(novaStavka.Cijena * novaStavka.Kolicina, 2) * popust,2);
                    //novaStavka.Iznos = Math.Round(novaStavka.Cijena * novaStavka.Kolicina, 2) - iznosPopust;

                    novaStavka.Iznos = Math.Round(novaStavka.Cijena * novaStavka.Kolicina, 2);
                    novaStavka.IznosPopusta = Math.Round(novaStavka.Iznos * Math.Round(Decimal.Divide(novaStavka.Popust, 100),2), 2);
                   
                    novaStavka.IznosKupac = novaStavka.Iznos;
                    novaStavka.IznosKzzo = Decimal.Zero;
                    novaStavka.Participacija = 0;
                    novaStavka.DoplataSaPDV = 0;
                    novaStavka.DoplataBezPDV = 0;

                     //eso
                    //if (Global.Instance.Konfiguracija["Kanton"] == "Zenica")
                    // {
                    //     novaStavka.DoplataSaPDV = selectedRoba.DoplataSaPDV;
                    // }

                     //////////////////////////////////

                    

                    if (selectedRoba.Taksa)
                    {
                        string strIznosTakseZaOtapanje = Global.Instance.Konfiguracija["TaksaZaOtapanje"];
                        if (!String.IsNullOrEmpty(strIznosTakseZaOtapanje))
                        {
                            decimal iznosTakseZaOtapanje = 0;
                            decimal.TryParse(strIznosTakseZaOtapanje,out iznosTakseZaOtapanje);
                            if (novaStavka.Kolicina < 0)
                                novaStavka.IznosTakse = iznosTakseZaOtapanje * - 1;
                            else
                                novaStavka.IznosTakse = iznosTakseZaOtapanje;
                        }
                    }
                }
                else
                {
                    novaStavka.BrJedinica = lastRecept.BrJedinica;
                    novaStavka.Participacija = lastRecept.Participacija;
                    novaStavka.VrstaID = lastRecept.VrstaID;
                    novaStavka.LjekarID = lastRecept.LjekarID;
                    novaStavka.Pausal = lastRecept.Pausal;
                    novaStavka.BrojRecepta = lastRecept.BrojRecepta;
                    novaStavka.JMBG = lastRecept.JMBG;
                    novaStavka.DatumPropisivanja = lastRecept.DatumPropisivanja;
                    if (selectedRoba.TipRobe.Sifra == "55")
                        novaStavka.BrJedinica = lastRecept.BrJedinica;

                    novaStavka.Iznos = Math.Round(novaStavka.Cijena * novaStavka.Kolicina, 2);

                    if (Global.Instance.Konfiguracija["Kanton"] == "Sarajevo")
                    {
                        novaStavka.DoplataSaPDV = Math.Round(Decimal.Multiply(Math.Round(Decimal.Multiply(novaStavka.DoplataBezPDV, (Decimal.Divide(novaStavka.PDVStopa, 100) + 1)), 2), novaStavka.Kolicina), 2);
                        novaStavka.PonovljenRecept = lastRecept.PonovljenRecept;
                        novaStavka.PropisanaKolicina = lastRecept.PropisanaKolicina;
                    }
                    else
                        novaStavka.DoplataSaPDV = Math.Round(Decimal.Multiply(novaStavka.DoplataSaPDV, novaStavka.Kolicina), 2);

                    decimal cijena = 0;

                    if (Global.Instance.Konfiguracija["Kanton"] == "Sarajevo")
                    {
                        cijena = System.Math.Round(novaStavka.ReferalnaCijena +
                           (novaStavka.ReferalnaCijena * novaStavka.PDVStopa / 100), 2);
                    }
                    else
                    {
                        decimal cijenaMarza;

                        cijenaMarza = novaStavka.ReferalnaCijena * Convert.ToDecimal(Global.Instance.Konfiguracija["MarzaZDK"]) / 100;

                        cijena = Math.Round((novaStavka.ReferalnaCijena + cijenaMarza) * ((100 + novaStavka.PDVStopa) / 100), 2); 
                    }
                    //decimal cijena = novaStavka.ReferalnaCijena;

                    novaStavka.IznosKupac = System.Math.Round(cijena * novaStavka.Kolicina *
                        (100 - novaStavka.Participacija) / 100, 2);

                    novaStavka.IznosKzzo = System.Math.Round(novaStavka.ReferalnaCijena * novaStavka.Kolicina *
                        (novaStavka.Participacija) / 100, 2);

                    if (novaStavka.VrstaID == 8 && Global.Instance.Konfiguracija["POSPrinterName"] != "Generic / Text Only")
                    {
                        novaStavka.IznosKzzo = Math.Round(novaStavka.Iznos / ((100 + novaStavka.PDVStopa) / 100), 2);
                        novaStavka.ReferalnaCijena = Math.Round(novaStavka.Cijena / ((100 + novaStavka.PDVStopa) / 100), 2);
                        //novaStavka.Iznos += lastRecept.Pausal;
                    }
                    else
                    {
                        novaStavka.IznosKzzo = Decimal.Zero;
                        //novaStavka.Iznos += lastRecept.Pausal;
                    }
                    
                }


                // PRORACUNAVANJE ZALIHE, VRIJEDNOSTI ( IZNOS BEZ PDV-a ) I IZLAZA ZA
                // STAVKU I NJENO SNIMANJE
                novaStavka.Vrijednost =Math.Round(novaStavka.IznosKupac /( 1 + novaStavka.PDVStopa / 100), 2);
                novaStavka.KreatorID = Global.Instance.LoggedKorisnik.ID;
                novaStavka.Zaliha = selectedRoba.Zaliha - novaStavka.Kolicina;
                novaStavka.Izlaz = selectedRoba.Izlaz + novaStavka.Kolicina;

                if (!novaStavka.Save(trans))
                {
                    trans.Rollback();
                    return null;
                }

                // UPDATE ZALIHE I IZLAZA NA ROBI
                selectedRoba.Zaliha -= kolicina;
                selectedRoba.Izlaz += kolicina;
                selectedRoba.ModifikatorID = Global.Instance.LoggedKorisnik.ID;
                
                if ( !selectedRoba.Save(trans))
                {
                    trans.Rollback();
                    return null;
                }

                // AZURIRANJE IZNOSA RACUNA I NJEGOVO SNIMANJE

                noviRacun.Vrijednost += novaStavka.Vrijednost;
                noviRacun.Iznos += novaStavka.IznosKupac;           //******************************************
                noviRacun.IznosPopusta += novaStavka.IznosPopusta;
                noviRacun.DoplataSaPDV += novaStavka.DoplataSaPDV;

                if (novaStavka.Pausal != 0)
                {
                    noviRacun.Vrijednost += Math.Round((novaStavka.Pausal / (1 + novaStavka.PDVStopa / 100)),2);
                    noviRacun.Iznos += novaStavka.Pausal;
                }

                if (novaStavka.IznosTakse != 0)
                {
                    decimal stopaTaksi = 0;
                    if (!String.IsNullOrEmpty(Global.Instance.Konfiguracija["StopaTakse"]))
                        decimal.TryParse(Global.Instance.Konfiguracija["StopaTakse"], out stopaTaksi);

                    noviRacun.Vrijednost += Math.Round(novaStavka.IznosTakse / (1 + stopaTaksi / 100),2);
                    noviRacun.Iznos += novaStavka.IznosTakse;
                }
                
                //zaokruzenje
                noviRacun.Zaokruzenje = ((noviRacun.Iznos - noviRacun.IznosPopusta+noviRacun.DoplataSaPDV) - Global.Instance.Zaokruzenje(noviRacun.Iznos - noviRacun.IznosPopusta +noviRacun.DoplataSaPDV)) * (-1); //*****
                noviRacun.NacinPlacanja = NacinPlacanja.Gotovina;
                noviRacun.ModifikatorID = Global.Instance.LoggedKorisnik.ID;
                
                if ( !noviRacun.Save(trans) )
                {
                    trans.Rollback();
                    return null;
                }

                trans.Commit();
                con.Close();
                return novaStavka;
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
                Logging.Log.Create("Greska prilikom pravljenja racuna.",
                        Logging.LogEntryLevel.Critical, ex,
                        new Logging.LogEntryAttribute("Operater", Global.Instance.LoggedKorisnik.KorisnickoIme));
                if (trans != null)
                    trans.Rollback();
            }

            return null;
        }

        public bool Delete(Racun racun)
        {
            MySqlTransaction trans = null;
            MySqlConnection con  = null;
            try
            {
                con = ApotekeDB.Instance.OpenConnection();
                trans = con.BeginTransaction();

                racun.Vrijednost -= this.Vrijednost;
                racun.Iznos -= this.IznosKupac;
                racun.IznosPopusta -= this.IznosPopusta;
                racun.DoplataSaPDV -= this.DoplataSaPDV;

                if (this.IznosTakse != 0)
                {
                    racun.Iznos -= this.IznosTakse;
                    decimal stopaTaksi = 0;
                    if (!String.IsNullOrEmpty(Global.Instance.Konfiguracija["StopaTakse"]))
                        decimal.TryParse(Global.Instance.Konfiguracija["StopaTakse"], out stopaTaksi);

                    racun.Vrijednost -= Math.Round(this.IznosTakse / (1 + stopaTaksi / 100),2);
                }
                
                if (this.Pausal != 0)
                {
                    racun.Iznos -= this.Pausal;
                    racun.Vrijednost -= Math.Round((this.Pausal / (1 + this.PDVStopa / 100)),2);
                }

                racun.Zaokruzenje = ((racun.Iznos + racun.DoplataSaPDV - racun.IznosPopusta) - Global.Instance.Zaokruzenje(racun.Iznos + racun.DoplataSaPDV - racun.IznosPopusta)) * (-1);
                if (!racun.Save(trans))
                {
                    trans.Rollback();
                    return false;
                }

                Roba roba = this.Roba;
                roba.Izlaz -= this.Kolicina;
                roba.Zaliha += this.Kolicina;
                roba.ModifikatorID = Global.Instance.LoggedKorisnik.ID;
                if (!roba.Save(trans))
                {
                    trans.Rollback();
                    return false;
                }
                
                MySqlCommand command = new MySqlCommand("sp_StavkaRacuna_DelByID", trans.Connection, trans);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Value = this.ID;
                command.ExecuteNonQuery();
                                
                trans.Commit();
                con.Close();

                Logging.Log.Create("Stavka je stornirana.",
                       Logging.LogEntryLevel.Informational, null,
                       new Logging.LogEntryAttribute("Operater", Global.Instance.LoggedKorisnik.KorisnickoIme),
                       new Logging.LogEntryAttribute("StavkaID", this.ID.ToString()));

                return true;

            }
            catch (SaveFailedException saveEx)
            {
                Logging.Log.Create("Neko je promjenio podatke stavke racuna.",
                       Logging.LogEntryLevel.Critical, saveEx,
                       new Logging.LogEntryAttribute("Operater", Global.Instance.LoggedKorisnik.KorisnickoIme),
                       new Logging.LogEntryAttribute("StavkaID", this.ID.ToString()));
                if (trans != null)
                    trans.Rollback();
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom brisanja stavke racuna.",
                        Logging.LogEntryLevel.Critical, ex,
                        new Logging.LogEntryAttribute("Operater", Global.Instance.LoggedKorisnik.KorisnickoIme),
                        new Logging.LogEntryAttribute("StavkaID", this.ID.ToString()));
                if (trans != null)
                    trans.Rollback();
            }
            finally
            {
                try
                {
                    if (con != null && con.State != System.Data.ConnectionState.Closed)
                        con.Close();
                }
                catch (System.Exception conCloseEx)
                {
                    Logging.Log.Create("Ne moze se zatvoriti konekcija na bazu.",
                        Logging.LogEntryLevel.Critical, conCloseEx);
                }
            }

            return false;
        }


        public static RacunStavka FindByRecept(string brojRecepta)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {

                    MySqlCommand commandStavke = new MySqlCommand("sp_FindRecept", con);
                    commandStavke.CommandType = System.Data.CommandType.StoredProcedure;
                    commandStavke.Parameters.Add(new MySqlParameter("@pBrojRecepta", MySqlDbType.String)).Value = brojRecepta;
                    MySqlDataReader dr = commandStavke.ExecuteReader();
                    if (dr != null && dr.HasRows)
                    {
                        dr.Read();
                        return Factory.CreateStavkaRacunaFromReader(dr);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom pozivanja Recept.Find metoda.",
                    Logging.LogEntryLevel.Critical, ex);
            }


            return null;
        }

        #endregion
    }
}

