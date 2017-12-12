using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects;
using Apoteke.DataObjects.Core;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_Racun_Ins", "sp_Racun_Upd", "sp_Racun_DelByID","sp_Racun_SelAll","sp_Racun_SelByID")]
    public class Racun : ApotekaBase<Racun>
    {
        #region Internal class
        private class TBR_Rekapitulacija
        {
            public decimal IznosTaksi
            {
                get;
                set;
            }

            public decimal UkupnoBezPDV
            {
                get;
                set;
            }

            public decimal UkupnoSaPDV
            {
                get;
                set;
            }

            public decimal IznosPDV
            {
                get;
                set;
            }

            public decimal Stopa
            {
                get;
                set;
            }

            public string TBR
            {
                get;
                set;
            }

            public TBR_Rekapitulacija(string tbr, decimal stopa)
            {
                Stopa = stopa;
                TBR = tbr;
            }
        }
        #endregion

        #region Properties
        [ObjectProperty("pDatum","Datum")]
        public DateTime Datum
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

        [ObjectProperty("pIznos","Iznos")]
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

        [ObjectProperty("pDoplataSaPDV", "DoplataSaPDV")]
        public decimal DoplataSaPDV
        {
            get;
            set;
        }

        [ObjectProperty("pZaokruzenje", "Zaokruzenje")]
        public decimal Zaokruzenje
        {
            get;
            set;
        }

        [ObjectProperty("pNacinPlacanja", "NacinPlacanja")]
        public NacinPlacanja NacinPlacanja
        {
            get;
            set;
        }

        [ObjectProperty("pSmjenaID", "SmjenaID")]
        public int SmjenaID
        {
            get;
            set;
        }

        public Smjena Smjena
        {
            get { return Smjena.GetByID(this.SmjenaID); }
        }

        [ObjectProperty("pIsprintan", "Isprintan")]
        public bool Isprintan
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

        public decimal PlacenoGotovinom
        {
            get;
            set;
        }

        [ObjectProperty("pKomitentID", "KomitentID")]
        public int KomitentID
        {
            get;
            set;
        }

        //jasenko tring
        [ObjectProperty("pBrojFisk", "BrojFisk")]
        public int BrojFisk
        {
            get;
            set;
        }

        [ObjectProperty("pBrojKase", "BrojKase")]
        public int BrojKase
        {
            get;
            set;
        }

        [ObjectProperty("pFiskalniIznos", "FiskalniIznos")]
        public decimal FiskalniIznos
        {
            get;
            set;
        }

        [ObjectProperty("pFiskalniDatum", "FiskalniDatum")]
        public DateTime FiskalniDatum
        {
            get;
            set;
        }
        //jasenko end tring

        public Komitent Komitent
        {
            get { return Komitent.GetByID(this.KomitentID); }
        }

        public List<RacunStavka> Stavke
        {
            get { return ApotekeDB.Instance.GetStavkeRacunaByRacunID(this.ID); }
        }

        #endregion

        #region Constructors
        public Racun()
            : this(0)
        {
            //int pj = 0;
            //int.TryParse(Global.Instance.Konfiguracija["BrojApoteke"], out pj);
            //this.PoslovnaJedinica = pj;

            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public Racun(int id)
            : base(id)
        { }
        #endregion

        #region Methods

        public bool Print()
        {
            try
            {
               if (!Global.Instance.IsPOSPrinterInstalled)
                    return false;

               Print printanje = new Print();
               
               if (printanje.PrinterSpreman(Global.Instance.POSPrinterName))
               {
                   string output = GetPrintString("racun");
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

        public bool PrintNaknadnoPOS()
        {
            try
            {
                //if (!Global.Instance.IsPOSPrinterInstalled)
                //    return false;

                Print printanje = new Print();

                if (printanje.PrinterSpreman("Generic / Text Only"))
                {
                    string output = GetPrintString("rekapitulacija");
                    if (!String.IsNullOrEmpty(output))
                    {
                        PrintThroughDriver.SendStringToPrinter("Generic / Text Only", output);
                        return true;
                    }
                }

            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Error while printing racun.",
                    Logging.LogEntryLevel.Critical, ex);
                return false;
            }

            return false;
        }

        private string GetPrintString(string rekapitulacijaRacuna)
        {
           TextOutput output = new TextOutput(40);
           
            // OTVARANJE LADICE
            output.NewLine();
            output.Add(new TextItem(0, (char)27 + "" + 'p' + "" + (char)0 + "" + (char)100 + "" + (char)250, TextAlignment.Left));
            output.CommitLine();

            #region Header

            string nazivUstanove = Global.Instance.Konfiguracija["NazivUstanove"];
            string adresaUstanove = Global.Instance.Konfiguracija["AdresaUstanove"];
            string pdvBroj = Global.Instance.Konfiguracija["PDVBroj"];
            string nazivApoteke = Global.Instance.Konfiguracija["NazivApoteke"];
            string adresaApoteke = Global.Instance.Konfiguracija["AdresaApoteke"];

            output.Add(new TextItem(0, nazivUstanove, TextAlignment.Left));
            output.Add(new TextItem(40, "PDV:"+pdvBroj, TextAlignment.Right));
            output.CommitLine();
           
            output.NewLine();
            output.Add(new TextItem(0, adresaUstanove, TextAlignment.Left));
            output.CommitLine();

            output.NewLine();
            output.Add(new TextItem(0, nazivApoteke, TextAlignment.Left));
            output.CommitLine();

            output.NewLine();
            output.Add(new TextItem(0, adresaApoteke, TextAlignment.Left));
            output.CommitLine();

            output.NewLine();
            output.Add(new TextItem(0, Racun.GetByID(this.ID).VrijemeKreiranja.ToString(), TextAlignment.Left));
            output.CommitLine();

            #endregion

            output.AddBlankLine();
            output.AddBlankLine();
            
            #region Racun Header
            if (rekapitulacijaRacuna == "rekapitulacija")
            {
                output.NewLine();
                output.Add(new TextItem(3, "Rekapitulacija naplačenog iznosa za: ", TextAlignment.Left));
                output.CommitLine();
                output.NewLine();
                output.Add(new TextItem(9, "Fiskalni račun br:  " + this.BrojFisk.ToString(), TextAlignment.Left));
                output.CommitLine();
            }
            output.NewLine();
            output.Add(new TextItem(13, "Račun br:  " + this.ID.ToString(), TextAlignment.Left));
            output.CommitLine();

            output.AddBlankLine();
            
           
            #endregion

            output.AddSplitLine();

            #region Column Names
            output.NewLine();
            output.Add(new TextItem(0, "Šifra - Naziv", TextAlignment.Left));
            output.CommitLine();


            output.NewLine();
            TextItem itemRCP = new TextItem(27, "RCP%", TextAlignment.Left);
            TextItem itemKolicina = new TextItem(7, "Količina", TextAlignment.Left);
            TextItem itemCijena = new TextItem(18, "Cijena", TextAlignment.Left);
            TextItem itemIznos = new TextItem(35, "Iznos", TextAlignment.Left);
            TextItem itemPdvIznos = new TextItem(0, "T PDV%", TextAlignment.Left);
            output.Add(itemRCP, itemCijena, itemIznos, itemKolicina, itemPdvIznos);
            output.CommitLine();

            #endregion

            output.AddSplitLine();

            #region Stavke

            Dictionary<string, TBR_Rekapitulacija> rekapitulacija = new Dictionary<string, TBR_Rekapitulacija>();

            decimal ukupnaVrijednostTakse = 0;
            decimal ukupanIznosTakse = 0;
            //decimal ukupnaVrijednost = 0;
            //decimal ukupanIznos = 0;
            Decimal iznosPop = 0;

            foreach (RacunStavka stavka in this.Stavke)
            {
                if (stavka == null)
                    continue;

                output.NewLine();
                output.Add(new TextItem(0, stavka.Roba.Sifra, TextAlignment.Left),
                    new TextItem(6, stavka.Roba.Naziv, TextAlignment.Left));
                output.CommitLine();

                string tbr = stavka.Roba.PDVStopa.TBR;

                iznosPop = stavka.IznosKupac - stavka.IznosPopusta;

                output.NewLine();
                TextItem itemIznosValue = new TextItem(40, String.Format("{0:0.00}", stavka.IznosKupac), TextAlignment.Right);
                TextItem itemCijenaValue = null;
                if (stavka.Participacija == 0)
                    itemCijenaValue = new TextItem(24, String.Format("{0:0.00}", stavka.Cijena), TextAlignment.Right);
                else
                {
                    decimal refCijena = Math.Round(stavka.ReferalnaCijena + stavka.ReferalnaCijena * stavka.PDVStopa / 100, 2);
                    itemCijenaValue = new TextItem(24, String.Format("{0:0.00}", refCijena), TextAlignment.Right);

                }
                TextItem itemKolicinaValue = new TextItem(15, String.Format("{0:0.00}", stavka.Kolicina), TextAlignment.Right);
                TextItem itemRCPValue = new TextItem(27, stavka.Participacija + "%", TextAlignment.Left);
                TextItem itemPDVValue = new TextItem(0, tbr + " " +
                    String.Format("{0:0}", stavka.Roba.PDVStopa.Stopa) + "%", TextAlignment.Left);
                output.Add(itemIznosValue, itemCijenaValue, itemKolicinaValue, itemPDVValue, itemRCPValue);
                output.CommitLine();

                // Ispis popusta na racunu ukoliko postoji ********************************************************
                Int16 popustInt;
                String popustInt1;

                if (stavka.Popust > 0)
                {
                    popustInt = Convert.ToInt16(stavka.Popust);
                    popustInt1 = String.Format("{0:0}", stavka.Popust);
                    output.NewLine();
                    output.Add(new TextItem(0, "Iznos popusta", TextAlignment.Left));
                    output.CommitLine();
                    output.NewLine();
                    TextItem itemIznosPopustaValue = new TextItem(40, String.Format("{0:0.00}", -stavka.IznosPopusta), TextAlignment.Right); // Iznos popusta
                    //TextItem itemTaksaCijenaValue = new TextItem(24, String.Format("{0:0.00}", stavka.IznosTakse), TextAlignment.Right);
                   // TextItem itemTaksaKolicinaValue = new TextItem(15, String.Format("{0:0.00}", Decimal.One), TextAlignment.Right); // Treba
                    TextItem itemPopustValue = new TextItem(26, popustInt + "%", TextAlignment.Left); // Popust
                    TextItem itemTaksaPDVValue = new TextItem(0, tbr + " " +
                        String.Format("{0:0}", stavka.Roba.PDVStopa.Stopa) + "%", TextAlignment.Left); // Treba
                    output.Add(itemIznosPopustaValue, itemTaksaPDVValue, itemPopustValue);
                    output.CommitLine();

                    //ukupnaVrijednostTakse += stavka.IznosTakse - (Math.Round(stavka.IznosTakse * stavka.Roba.PDVStopa.Stopa / (100 + stavka.Roba.PDVStopa.Stopa), 2));
                    //ukupanIznosTakse += stavka.IznosTakse;
                }

                Int16 doplataInt;
                String doplataInt1;



                if (stavka.DoplataSaPDV != 0)
                {
                    doplataInt = Convert.ToInt16(stavka.DoplataBezPDV);
                    doplataInt1 = String.Format("{0:0}", stavka.DoplataBezPDV);
                    output.NewLine();
                    output.Add(new TextItem(0, "Doplata do proizvođačke cijene", TextAlignment.Left));
                    output.CommitLine();
                    output.NewLine();
                    TextItem itemIznosDoplateValue = new TextItem(40, String.Format("{0:0.00}", stavka.DoplataSaPDV), TextAlignment.Right); // Iznos popusta
                    //TextItem itemTaksaCijenaValue = new TextItem(24, String.Format("{0:0.00}", stavka.IznosTakse), TextAlignment.Right);
                    // TextItem itemTaksaKolicinaValue = new TextItem(15, String.Format("{0:0.00}", Decimal.One), TextAlignment.Right); // Treba
                    //TextItem itemDoplataValue = new TextItem(26, doplataInt + "%", TextAlignment.Left); // Popust
                    TextItem itemKolicina1Value = new TextItem(15, String.Format("{0:0.00}", stavka.Kolicina), TextAlignment.Right);

                    TextItem DoplataCijenaValue = new TextItem(24, String.Format("{0:0.00}", Math.Round(Decimal.Divide( stavka.DoplataSaPDV, stavka.Kolicina), 2)), TextAlignment.Right);
                    //TextItem itemTaksaPDVValue = new TextItem(0, tbr + " " +
                    //    String.Format("{0:0}", stavka.Roba.PDVStopa.Stopa) + "%", TextAlignment.Left); // Treba
                    output.Add(itemIznosDoplateValue, itemKolicina1Value, DoplataCijenaValue);
                    output.CommitLine();

                    //ukupnaVrijednostTakse += stavka.IznosTakse - (Math.Round(stavka.IznosTakse * stavka.Roba.PDVStopa.Stopa / (100 + stavka.Roba.PDVStopa.Stopa), 2));
                    //ukupanIznosTakse += stavka.IznosTakse;
                }

                //*************************************************************************************************

                if (stavka.IznosTakse != 0)
                {
                    output.NewLine();
                    output.Add(new TextItem(0, "Taksa za otapanje sirupa", TextAlignment.Left));
                    output.CommitLine();
                    output.NewLine();
                    TextItem itemTaksaIznosValue = new TextItem(40, String.Format("{0:0.00}", stavka.IznosTakse), TextAlignment.Right);
                    TextItem itemTaksaCijenaValue = new TextItem(24, String.Format("{0:0.00}", stavka.IznosTakse), TextAlignment.Right);
                    TextItem itemTaksaKolicinaValue = new TextItem(15, String.Format("{0:0.00}", Decimal.One ), TextAlignment.Right);
                    TextItem itemTaksaRCPValue = new TextItem(27, stavka.Participacija + "%", TextAlignment.Left);
                    TextItem itemTaksaPDVValue = new TextItem(0, tbr + " " +
                        String.Format("{0:0}", stavka.Roba.PDVStopa.Stopa) + "%", TextAlignment.Left);
                    output.Add(itemTaksaIznosValue, itemTaksaCijenaValue, itemTaksaKolicinaValue, itemTaksaPDVValue, itemTaksaRCPValue);
                    output.CommitLine();

                    ukupnaVrijednostTakse += stavka.IznosTakse - (Math.Round(stavka.IznosTakse * stavka.Roba.PDVStopa.Stopa / (100 + stavka.Roba.PDVStopa.Stopa), 2));
                    ukupanIznosTakse += stavka.IznosTakse;
                }

                //ukupnaVrijednost += stavka.Vrijednost;
                //ukupanIznos += stavka.Iznos;

                if (stavka.Pausal != 0)
                {
                    output.NewLine();
                    output.Add(new TextItem(0, "Paušal sa recepta", TextAlignment.Left), new TextItem(40,String.Format("{0:0.00}", stavka.Pausal), TextAlignment.Right));
                    output.CommitLine();
                    output.NewLine();
                }
               
                // RACUNANJE REKAPITULACIJE

                if (stavka.Roba != null && stavka.Roba.PDVStopa != null &&
                    !rekapitulacija.Keys.Contains(tbr))
                    rekapitulacija.Add(tbr,
                       new TBR_Rekapitulacija(tbr, stavka.Roba.PDVStopa.Stopa));


                if (stavka.Roba != null && stavka.Roba.PDVStopa != null &&
                    rekapitulacija.Keys.Contains(tbr))
                {
                    rekapitulacija[tbr].UkupnoBezPDV += iznosPop + stavka.DoplataSaPDV; //- (stavka.Iznos - Global.Instance.Zaokruzenje(stavka.Iznos));
                    //rekapitulacija[tbr].UkupnoBezPDV += stavka.Vrijednost;
                    // ako ovako onda se izgubi svaki put 0.01 ili 0.02 
                    // zato sto se svaki iznos pojedino zaokruzuje
                    //rekapitulacija[tbr].UkupnoSaPDV += Global.Instance.Zaokruzenje(stavka.Iznos);
                    rekapitulacija[tbr].UkupnoSaPDV += iznosPop + stavka.IznosTakse + stavka.Pausal + stavka.DoplataSaPDV;
                    //rekapitulacija[tbr].UkupnoSaPDV += stavka.IznosKupac + stavka.IznosTakse + stavka.Pausal;
                    rekapitulacija[tbr].IznosPDV += iznosPop + stavka.DoplataSaPDV - stavka.Vrijednost;
                    //rekapitulacija[tbr].IznosPDV += stavka.IznosKupac - stavka.Vrijednost;
                    //rekapitulacija[tbr].IznosTaksi += stavka.IznosTakse;
                }
               
            }

            #endregion
            if (this.Zaokruzenje != Decimal.Zero)
            {
                output.NewLine();
                output.Add(new TextItem(0, "zaokruženje računa :", TextAlignment.Left),
                    new TextItem(output.LineLenght, String.Format("{0:0.00}", this.Zaokruzenje), TextAlignment.Right));
                output.CommitLine();
            }
            output.AddSplitLine();
 
            #region Footer

            output.NewLine();
            Racun racNew = Racun.GetByID(this.ID);
            output.Add(new TextItem(0, "UKUPNO ZA PLAĆANJE:", TextAlignment.Left),
                new TextItem(output.LineLenght, "KM= " + String.Format("{0:0.00}", racNew.Iznos + racNew.Zaokruzenje - racNew.IznosPopusta + racNew.DoplataSaPDV), TextAlignment.Right));
            output.CommitLine();

            output.AddSplitLine();
           
            #region Rekapitulacija

            #region Zaglavlje
            output.NewLine();
            output.Add(new TextItem(3, "ukupno", TextAlignment.Left));
            output.Add(new TextItem(22, "iznos", TextAlignment.Left));
            output.Add(new TextItem(32, "ukupno", TextAlignment.Left));
            output.CommitLine();
            output.NewLine();
            output.Add(new TextItem(2, "baz pdv", TextAlignment.Left));
            output.Add(new TextItem(10, "ts", TextAlignment.Left));
            output.Add(new TextItem(16, "%", TextAlignment.Left));
            output.Add(new TextItem(24, "pdv", TextAlignment.Left));
            output.Add(new TextItem(32, "sa_pdv", TextAlignment.Left));
            output.CommitLine();
            #endregion

            foreach (string tbr in rekapitulacija.Keys)
            {
                //decimal pdv_temp =
                //    Global.Instance.Zaokruzenje(rekapitulacija[tbr].UkupnoSaPDV) * 17 / 117;

                decimal pdv_temp =
                    Global.Instance.Zaokruzenje(rekapitulacija[tbr].UkupnoSaPDV) * rekapitulacija[tbr].Stopa / (100 + rekapitulacija[tbr].Stopa);

                output.NewLine();
                output.Add(new TextItem(9, String.Format("{0:0.00}", 
                    //rekapitulacija[tbr].UkupnoBezPDV
                    Global.Instance.Zaokruzenje(rekapitulacija[tbr].UkupnoSaPDV) - pdv_temp
                    ), TextAlignment.Right));
                output.Add(new TextItem(10, rekapitulacija[tbr].TBR, TextAlignment.Left));
                output.Add(new TextItem(17,  String.Format("{0:0.}",
                    rekapitulacija[tbr].Stopa)+"%", TextAlignment.Right));
                output.Add(new TextItem(27, String.Format("{0:0.00}",
                   //rekapitulacija[tbr].IznosPDV
                   pdv_temp
                   ) , TextAlignment.Right));
                output.Add(new TextItem(38, String.Format("{0:0.00}",
                   //dodao ovdje zaokruzenje
                    Global.Instance.Zaokruzenje(rekapitulacija[tbr].UkupnoSaPDV) ) , TextAlignment.Right));
                output.CommitLine();
            }

            #endregion

            output.AddSplitLine();
            output.NewLine();
            output.Add(new TextItem(12, "* Hvala na posjeti !*", TextAlignment.Left));
            output.CommitLine();

            output.NewLine();
            output.Add(new TextItem(13, DateTime.Now.ToString(), TextAlignment.Left));
            output.CommitLine();
                        
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

            //CUT za Star SP600
            //if (Global.Instance.BrojPoslovnice == 92)
            //{
            //    output.NewLine();
            //    output.Add(new TextItem(0, "1B 64 74", TextAlignment.Left));
            //    output.CommitLine();
            //}
           
            #endregion
          
            return output.GetOutput();
        }

        public int Storniraj()
        {
            MySqlTransaction trans = null;

            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {

                    trans = con.BeginTransaction();

                    MySqlCommand command = new MySqlCommand("sp_Racun_Storno", con,trans);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pRacunID", MySqlDbType.Int32)).Value = this.ID;
                    command.Parameters.Add(new MySqlParameter("@pModifikatorID", MySqlDbType.Int32)).Value = Global.Instance.LoggedKorisnik.ID;
                    command.Parameters.Add(new MySqlParameter("@pSmjenaID", MySqlDbType.Int32)).Value = this.SmjenaID;

                    command.Parameters.Add(new MySqlParameter("@pStornoRacunID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    int stornoRacunID = (int)command.Parameters["@pStornoRacunID"].Value;

                    foreach (RacunStavka stavka in this.Stavke)
                    {
                        MySqlCommand commandStavke = new MySqlCommand("sp_StavkeRacuna_Storno", con, trans);
                        commandStavke.CommandType = System.Data.CommandType.StoredProcedure;
                        commandStavke.Parameters.Add(new MySqlParameter("@pStavkaRacunaID", MySqlDbType.Int32)).Value = stavka.ID;
                        commandStavke.Parameters.Add(new MySqlParameter("@pModifikatorID", MySqlDbType.Int32)).Value = Global.Instance.LoggedKorisnik.ID;
                        commandStavke.Parameters.Add(new MySqlParameter("@pStornoRacunID", MySqlDbType.Int32)).Value = stornoRacunID;
                        commandStavke.ExecuteNonQuery();
                    }

                    trans.Commit();

                    Logging.Log.Create("Racun je Storniran.", Logging.LogEntryLevel.Informational, null,
                        new Logging.LogEntryAttribute("RacunID", this.ID.ToString()),
                        new Logging.LogEntryAttribute("Operater", Global.Instance.LoggedKorisnik.KorisnickoIme));

                    return stornoRacunID;
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom storniranja racuna.", Logging.LogEntryLevel.Informational, null,
                        new Logging.LogEntryAttribute("RacunID", this.ID.ToString()),
                        new Logging.LogEntryAttribute("Operater", Global.Instance.LoggedKorisnik.KorisnickoIme));
                try
                {
                    if (trans != null)
                        trans.Rollback();
                }
                catch (System.Exception transEx)
                {
                    Logging.Log.Create("Greska prilikom rollbacka transakcije unutar storniranja racuna.",Logging.LogEntryLevel.Critical,
                        transEx, new Logging.LogEntryAttribute("RacunID", this.ID.ToString()),
                        new Logging.LogEntryAttribute("Operater", Global.Instance.LoggedKorisnik.KorisnickoIme));
                }
            }

            return -1;
        }

        public Racun Copy()
        {
            Racun newRacun = new Racun();
            newRacun.Datum = this.Datum;
            newRacun.Isprintan = this.Isprintan;
            newRacun.Iznos = this.Iznos;
            newRacun.IznosPopusta = this.IznosPopusta;
            newRacun.DoplataSaPDV = this.DoplataSaPDV;
            newRacun.KreatorID = this.KreatorID;
            newRacun.ModifikatorID = this.ModifikatorID;
            newRacun.NacinPlacanja = this.NacinPlacanja;
            newRacun.PlacenoGotovinom = this.PlacenoGotovinom;
            newRacun.SmjenaID = this.SmjenaID;
            newRacun.State = this.State;
            newRacun.Vrijednost = this.Vrijednost;
            newRacun.VrijemeKreiranja = this.VrijemeKreiranja;
            newRacun.VrijemeModificiranja = this.VrijemeModificiranja;
            newRacun.Zaokruzenje = this.Zaokruzenje;
            // jasenko tring
            newRacun.BrojFisk = this.BrojFisk;
            newRacun.BrojKase = this.BrojKase;
            newRacun.FiskalniDatum = this.FiskalniDatum;
            newRacun.FiskalniIznos = this.FiskalniIznos;
            
            return newRacun;
        }

       
        
        private bool Insert(MySqlTransaction trans)
        {
            if (trans == null)
                return false;

            try
            {
                MySqlCommand command = new MySqlCommand("sp_Racun_Ins", trans.Connection, trans);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                
                command.Parameters.Add(new MySqlParameter("pDatum", MySqlDbType.Date)).Value = this.Datum;
                command.Parameters.Add(new MySqlParameter("pVrijednost", MySqlDbType.Decimal)).Value = this.Vrijednost;
                command.Parameters.Add(new MySqlParameter("pIznos", MySqlDbType.Decimal)).Value = this.Iznos;
                command.Parameters.Add(new MySqlParameter("pIznosPopusta", MySqlDbType.Decimal)).Value = this.IznosPopusta;
                command.Parameters.Add(new MySqlParameter("pDoplataSaPDV", MySqlDbType.Decimal)).Value = this.DoplataSaPDV;
                command.Parameters.Add(new MySqlParameter("pKreatorID", MySqlDbType.Int32)).Value = Global.Instance.LoggedKorisnik.ID;
                command.Parameters.Add(new MySqlParameter("pNacinPlacanja", MySqlDbType.Int32)).Value = (int)this.NacinPlacanja;
                command.Parameters.Add(new MySqlParameter("pSmjenaID", MySqlDbType.Int32)).Value = this.SmjenaID;
                command.Parameters.Add(new MySqlParameter("pIsprintan", MySqlDbType.Bit)).Value = this.Isprintan;
                command.Parameters.Add(new MySqlParameter("pZaokruzenje", MySqlDbType.Decimal)).Value = this.Zaokruzenje;
                command.Parameters.Add(new MySqlParameter("pPoslovnaJedinica", MySqlDbType.Int32)).Value = this.PoslovnaJedinica;
                // jasenko tring
                command.Parameters.Add(new MySqlParameter("pBrojFisk", MySqlDbType.Int32)).Value = this.BrojFisk;
                command.Parameters.Add(new MySqlParameter("pBrojKase", MySqlDbType.Int32)).Value = this.BrojKase;
                command.Parameters.Add(new MySqlParameter("pFiskalniIznos", MySqlDbType.Decimal)).Value = this.FiskalniIznos;
                command.Parameters.Add(new MySqlParameter("pFiskalniDatum", MySqlDbType.DateTime)).Value = this.FiskalniDatum;                

                command.ExecuteNonQuery();

                int id = (int)command.Parameters["pID"].Value;
                this.ID = id;
                this.State = ObjectState.Existing;
                return true;
            }
            catch (SaveFailedException saveEx)
            {
                Logging.Log.Create("Insert (Transaction) Racuna. Neko je vec promjenio podatke.",
                    Logging.LogEntryLevel.Critical, saveEx);
                //throw saveEx;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom Insert (Transaction) Racuna.",
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
                MySqlCommand command = new MySqlCommand("sp_Racun_Upd", trans.Connection, trans);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Value = this.ID;
                command.Parameters.Add(new MySqlParameter("pDatum", MySqlDbType.Date)).Value = this.Datum;
                command.Parameters.Add(new MySqlParameter("pVrijednost", MySqlDbType.Decimal)).Value = this.Vrijednost;
                command.Parameters.Add(new MySqlParameter("pIznos", MySqlDbType.Decimal)).Value = this.Iznos;
                command.Parameters.Add(new MySqlParameter("pIznosPopusta", MySqlDbType.Decimal)).Value = this.IznosPopusta;
                command.Parameters.Add(new MySqlParameter("pDoplataSaPDV", MySqlDbType.Decimal)).Value = this.DoplataSaPDV;
                command.Parameters.Add(new MySqlParameter("pModifikatorID", MySqlDbType.Int32)).Value = this.ModifikatorID;
                command.Parameters.Add(new MySqlParameter("pNacinPlacanja", MySqlDbType.Int32)).Value = (int)this.NacinPlacanja;
                command.Parameters.Add(new MySqlParameter("pSmjenaID", MySqlDbType.Int32)).Value = this.SmjenaID;
                command.Parameters.Add(new MySqlParameter("pIsprintan", MySqlDbType.Bit)).Value = this.Isprintan;
                command.Parameters.Add(new MySqlParameter("pZaokruzenje", MySqlDbType.Decimal)).Value = this.Zaokruzenje;
                command.Parameters.Add(new MySqlParameter("pKomitentID", MySqlDbType.Int32)).Value = this.KomitentID;
                command.Parameters.Add(new MySqlParameter("pStorniran", MySqlDbType.Bit)).Value = this.Storniran;
                // jasenko tring
                command.Parameters.Add(new MySqlParameter("pBrojFisk", MySqlDbType.Int32)).Value = this.BrojFisk;
                command.Parameters.Add(new MySqlParameter("pBrojKase", MySqlDbType.Int32)).Value = this.BrojKase;
                command.Parameters.Add(new MySqlParameter("pFiskalniIznos", MySqlDbType.Decimal)).Value = this.FiskalniIznos;
                command.Parameters.Add(new MySqlParameter("pFiskalniDatum", MySqlDbType.DateTime)).Value = this.FiskalniDatum;                

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
                Logging.Log.Create("Update (Transaction) Racuna. Neko je vec promjenio podatke.",
                    Logging.LogEntryLevel.Critical, saveEx);
                //throw saveEx;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom Update (Transaction) Racuna.",
                    Logging.LogEntryLevel.Critical, ex);
                //throw ex;
            }

            return false;
        }

        public virtual bool Save(MySqlTransaction trans)
        {
            if (this.State == ObjectState.New )
                return Insert(trans);
            else if (this.State == ObjectState.Existing)
                return Update(trans);

            return false;
        }

        public static bool ImaRecepata(int racunID)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_Racun_ImaRecepata", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlCommandBuilder.DeriveParameters(command);

                    command.Parameters["@pRacunID"].Value = racunID;

                    command.ExecuteNonQuery();

                    if ((int)command.Parameters["@pIma"].Value == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom trazenja recepata na racunu.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return false;

        }

        public bool ImaRecepata()
        {
            return Racun.ImaRecepata(this.ID);
        }

        public static List<Racun> GetByDate(DateTime datumod, DateTime datumDo)
        {
            return ApotekeDB.Instance.GetRacuni(DateTime.Now, DateTime.Now);
        }

        public static List<Racun> GetByIDRange(int idFrom)
        {
            return ApotekeDB.Instance.GetRacuni(idFrom);
        }

        public static Racun GetByID(int id)
        {
            return ApotekeDB.Instance.GetRacun(id);
        }

        #endregion
    }
}
