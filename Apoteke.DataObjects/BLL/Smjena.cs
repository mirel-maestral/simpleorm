using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects.Core;
using MySql.Data.MySqlClient;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_Smjena_Ins", "sp_Smjena_Upd", "sp_Smjena_DelByID",
       "sp_Smjena_SelAll", "sp_Smjena_SelByID")]
    public class Smjena : ApotekaBase<Smjena>
    {
       
        public Smjena()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public Smjena(int id)
            : base(id)
        { }

        [ObjectProperty("pVrijemeOtvaranja", "VrijemeOtvaranja")]
        public DateTime VrijemeOtvaranja
        {
            get;
            set;
        }

        [ObjectProperty("pVrijemeZatvaranja", "VrijemeZatvaranja")]
        public DateTime VrijemeZatvaranja
        {
            get;
            set;
        }

        [ObjectProperty("pBroj", "Broj")]
        public int Broj
        {
            get;
            set;
        }

        [ObjectProperty("pOdgovornoLiceID", "OdgovornoLiceID")]
        public int OdgovornoLiceID
        {
            get;
            set;
        }

        public Korisnik OdgovornoLice
        {
            get { return Korisnik.GetByID(this.OdgovornoLiceID); }
        }

        [ObjectProperty("pUkupno", "Ukupno")]
        public decimal Ukupno
        {
            get;
            set;
        }

        [ObjectProperty("pZatvorena", "Zatvorena")]
        public bool Zatvorena
        {
            get;
            set;
        }

        [ObjectProperty("pPoslovnaJedinica", "PoslovnaJedinica")]
        public int PoslovnaJedinica
        {
            get;
            set;
        }

        public List<Racun> Racuni
        {
            get { return ApotekeDB.Instance.GetRacuniBySmjena(this.ID, false); }
        }

        public Dictionary<NacinPlacanja, decimal> GetIznosForAllNacinPlacanja()
        {
            try
            {
                Dictionary<NacinPlacanja, decimal> temp = new Dictionary<NacinPlacanja, decimal>();

                string[] npArray = Enum.GetNames(typeof(NacinPlacanja));
                foreach (string np in npArray)
                {
                    NacinPlacanja currentNacinPlacanja = (NacinPlacanja)Enum.Parse(typeof(NacinPlacanja), np);
                    decimal iznos = 0;
                    foreach (Racun racun in this.Racuni)
                    {
                        if (racun.NacinPlacanja == currentNacinPlacanja)
                            iznos += racun.Iznos;
                    }

                    temp.Add(currentNacinPlacanja, iznos);
                }

                return temp;
            }
            catch (System.Exception ex)
            {

            }
            return null;
        }

        public decimal GetIznosForNacinPlacanja(NacinPlacanja nacinPlacanja)
        {
            Dictionary<NacinPlacanja, decimal> temp = GetIznosForAllNacinPlacanja();
            if (temp == null)
                return Decimal.Zero;

            return temp[nacinPlacanja];
        }
               
        public void Zatvori()
        {            
            Dictionary<NacinPlacanja, decimal> naps = GetIznosForAllNacinPlacanja();
            if (naps == null || naps.Keys == null || naps.Keys.Count == 0)
                return;

            MySqlTransaction trans = null;
            decimal ukupno = decimal.Zero;
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_SmjenaVrijednosti_Ins", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pSmjenaID", MySqlDbType.Int32)).Value = this.ID;
                    command.Parameters.Add(new MySqlParameter("@pNacinPlacanja", MySqlDbType.Int32));
                    command.Parameters.Add(new MySqlParameter("@pIznos", MySqlDbType.Decimal));

                    trans = con.BeginTransaction();
                   
                    foreach (NacinPlacanja nap in naps.Keys)
                    {
                        command.Parameters["@pNacinPlacanja"].Value = (int)nap;
                        command.Parameters["@pIznos"].Value = naps[nap];
                        command.ExecuteNonQuery();
                        ukupno += naps[nap];
                    }
                    
                    this.Zatvorena = true;
                    this.Ukupno = ukupno;
                    this.VrijemeZatvaranja = DateTime.Now;
                    this.Save();
                    trans.Commit();
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska u metodi Zatvori.Transakcija ce biti Rollback",
                    Logging.LogEntryLevel.Critical, ex);
                try
                {
                    if (trans != null)
                        trans.Rollback();
                }
                catch (System.Exception exTrans)
                {
                    Logging.Log.Create("Rollback transakcije nije moguce uraditi.", Logging.LogEntryLevel.Critical, exTrans);
                }
            }
        }

        public static Smjena GetOtvorenaSmjena()
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_Smjena_GetLastOpened", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pSmjenaID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                    command.ExecuteNonQuery();

                    int smjenaId = (int)command.Parameters["@pSmjenaID"].Value;
                    if (smjenaId > 0)
                        return Smjena.GetByID(smjenaId);
                    else
                        return null;
                }

            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska u metodi GetOtvorenaSmjena().",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        static Smjena GetLastSmjena()
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_Smjena_GetLast", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pSmjenaID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                    command.ExecuteNonQuery();

                    int smjenaId = (int)command.Parameters["@pSmjenaID"].Value;
                    if (smjenaId > 0)
                        return Smjena.GetByID(smjenaId);
                    else
                        return null;
                }

            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska u metodi GetLastSmjena().",
                    Logging.LogEntryLevel.Critical, ex);
            }
            return null;
        }

        public static int GetNextBrojSmjene()
        {
            int brSmjene = 1;

            Smjena smjena = GetLastSmjena();
            if (smjena == null)
                return 0;

            if (smjena.VrijemeOtvaranja.ToShortDateString() == DateTime.Now.ToShortDateString())
            {
                if (smjena.Broj + 1 <= 3)
                    brSmjene = smjena.Broj + 1;
            }

            return brSmjene;
        }

        public static bool Exist(int brSmjene, DateTime datum)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_Smjena_Exist", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pBroj", MySqlDbType.Int32)).Value = brSmjene;
                    command.Parameters.Add(new MySqlParameter("@pDatum", MySqlDbType.Date)).Value = datum;
                    command.Parameters.Add(new MySqlParameter("@pResult", MySqlDbType.Byte)).Direction = System.Data.ParameterDirection.Output;
                    command.ExecuteNonQuery();

                    return System.Convert.ToBoolean(command.Parameters["@pResult"].Value);
                }

            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska u metodi Exist().",
                    Logging.LogEntryLevel.Critical, ex);
            }
            return false;
        }

        public Dictionary<string, decimal> GetRekapitulacijuSmjene()
        {
            Dictionary<string, decimal> temp = new Dictionary<string, decimal>();
            foreach (Racun racun in this.Racuni)
            {
                if (racun == null)
                    continue;

                if (!temp.Keys.Contains("Iznos"))
                    temp.Add("Iznos", racun.Iznos);
                else
                    temp["Iznos"] += racun.Iznos;

                if (!temp.Keys.Contains("PDV"))
                    temp.Add("PDV", racun.Iznos - racun.Vrijednost);
                else
                    temp["PDV"] += racun.Iznos - racun.Vrijednost;

                if (!temp.Keys.Contains("IznosBezPDVa"))
                    temp.Add("IznosBezPDVa", racun.Vrijednost);
                else
                    temp["IznosBezPDVa"] += racun.Vrijednost;

                if (!temp.Keys.Contains("Zaokruzenje"))
                    temp.Add("Zaokruzenje", racun.Zaokruzenje);
                else
                    temp["Zaokruzenje"] += racun.Zaokruzenje;

                foreach (RacunStavka stavkaRacuna in racun.Stavke)
                {
                    if (stavkaRacuna == null)
                        continue;

                    if (!temp.Keys.Contains("IznosTaksi"))
                        temp.Add("IznosTaksi", stavkaRacuna.IznosTakse);
                    else
                        temp["IznosTaksi"] += stavkaRacuna.IznosTakse;
                }
                
            }

            return temp;
        }

        public string GetPrintStringRekapitulacije()
        {
            Dictionary<string, decimal> Rekapitulacija = GetRekapitulacijuSmjene();

            TextOutput output = new TextOutput(40);
            
            #region Header

            string nazivUstanove = Global.Instance.Konfiguracija["NazivUstanove"];
            string adresaUstanove = Global.Instance.Konfiguracija["AdresaUstanove"];
            string pdvBroj = Global.Instance.Konfiguracija["PDVBroj"];
            string nazivApoteke = Global.Instance.Konfiguracija["NazivApoteke"];
            string adresaApoteke = Global.Instance.Konfiguracija["AdresaApoteke"];

            output.Add(new TextItem(0, nazivUstanove, TextAlignment.Left));
            output.CommitLine();

            output.NewLine();
            output.Add(new TextItem(0, adresaUstanove, TextAlignment.Left));
            output.CommitLine();

            output.NewLine();
            output.Add(new TextItem(0, pdvBroj, TextAlignment.Left));
            output.CommitLine();

            output.NewLine();
            output.Add(new TextItem(0, nazivApoteke, TextAlignment.Left));
            output.CommitLine();

            output.NewLine();
            output.Add(new TextItem(0, adresaApoteke, TextAlignment.Left));
            output.CommitLine();

            #endregion

            output.AddSplitLine();

            #region Smjena Header
            output.NewLine();
            output.Add(new TextItem(0, "Datum smjene:" + this.VrijemeZatvaranja.ToString("dd.MM.yyyy"), TextAlignment.Left));
            output.CommitLine();

            output.NewLine();
            output.Add(new TextItem(0, "Broj smjene:" + this.Broj.ToString(), TextAlignment.Left));
            output.CommitLine();

            output.NewLine();
            output.Add(new TextItem(0, "Odgovorno lice: " + this.OdgovornoLice.Ime+ " " + 
                this.OdgovornoLice.Prezime,TextAlignment.Left));
            output.CommitLine();


            #endregion

            output.AddSplitLine();

            #region Smjena
            
            output.NewLine();
            output.Add(new TextItem(0, "Ukupni PDV: " + Rekapitulacija["PDV"].ToString(), TextAlignment.Left));
            output.CommitLine();
            output.NewLine();
            output.Add(new TextItem(0, "Ukupni iznos bez PDV-a: " + (Rekapitulacija["Iznos"] - 
                Rekapitulacija["PDV"]).ToString(), TextAlignment.Left));
            output.CommitLine();
            output.NewLine();
            output.Add(new TextItem(0, "Ukupni iznos: " + Rekapitulacija["Iznos"].ToString(), TextAlignment.Left));
            output.CommitLine();
            output.NewLine();
            output.Add(new TextItem(0, "Ukupno zaokruzenje: " + Rekapitulacija["Zaokruzenje"].ToString(), TextAlignment.Left));
            output.CommitLine();
            output.NewLine();
            output.Add(new TextItem(0, "Ukupno iznos taksi: " + Rekapitulacija["IznosTaksi"].ToString(), TextAlignment.Left));
            output.CommitLine();
            
            
            #endregion

            #region Footer
            output.AddBlankLine();
            output.AddBlankLine();
            output.AddBlankLine();
            output.AddBlankLine();
            output.AddBlankLine();
            output.AddBlankLine();
            output.AddBlankLine();
            output.AddBlankLine();

            // CUT COMMAND
            output.NewLine();
            output.Add(new TextItem(0, (char)29 + "" + (char)86 + "" + (char)1, TextAlignment.Left));
            output.CommitLine();
            #endregion

            return output.GetOutput();
        }

        public bool Print()
        {
            try
            {
                if (!Global.Instance.IsPOSPrinterInstalled)
                    return false;

                Print printanje = new Print();
                if (printanje.PrinterSpreman(Global.Instance.POSPrinterName))
                {
                    string output = GetPrintStringRekapitulacije();
                    if (!String.IsNullOrEmpty(output))
                    {
                        PrintThroughDriver.SendStringToPrinter(Global.Instance.POSPrinterName, output);
                        return true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Error while printing racun.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return false;
        }

        public static Smjena GetByID(int id)
        {
            if (id == 0)
                return null;

            if (!Global.Instance.DisableCache)
            {
                Smjena smjenaFromCache = CacheSync.Get<Smjena>(id) as Smjena;
                if (smjenaFromCache != null)
                    return smjenaFromCache;
            }

            Smjena smjenaFromDb = ApotekeDB.Instance.GetSmjena(id);
            if (!Global.Instance.DisableCache)
                CacheSync.Sync<Smjena>(smjenaFromDb);
            return smjenaFromDb;
        }

        public static List<Smjena> GetAll()
        {
            return ApotekeDB.Instance.GetSmjene();
        }

        public static Smjena GetSmjena(DateTime datum, int brojSmjene)
        {
            List<Smjena> smjene = GetAll();
            if (smjene == null)
                return null;

            Smjena smjena = smjene.SingleOrDefault(smj => smj.Broj == brojSmjene && smj.VrijemeOtvaranja.Date == datum.Date);
            if (smjena == null)
                return null;


            return smjena;
        }

    }
}
