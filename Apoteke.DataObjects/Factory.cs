using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects.Core;
using Apoteke.DataObjects.BLL;

namespace Apoteke.DataObjects
{
    public class Factory: ObjectFactory
    {
        public Factory()
        { }

        public static MagistralaNalog CreateNalogFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            MagistralaNalog nalog = null;
            try
            {
                nalog = new MagistralaNalog(dr.GetInt32("ID"));
                nalog.Datum = dr.IsDBNull(dr.GetOrdinal("Datum")) ? DateTime.MinValue : dr.GetDateTime("Datum");
                nalog.RobaID = dr.IsDBNull(dr.GetOrdinal("RobaID")) ? 0 : dr.GetInt32("RobaID");
                nalog.Kolicina = dr.IsDBNull(dr.GetOrdinal("Kolicina")) ? 0 : dr.GetDecimal("Kolicina");
                nalog.VrijednostSastojaka = dr.IsDBNull(dr.GetOrdinal("VrijednostSastojaka")) ? Decimal.Zero : dr.GetDecimal("VrijednostSastojaka");
                nalog.IznosTaksi = dr.IsDBNull(dr.GetOrdinal("IznosTaksi")) ? Decimal.Zero : dr.GetDecimal("IznosTaksi");
                nalog.Odstupanje = dr.IsDBNull(dr.GetOrdinal("Odstupanje")) ? Decimal.Zero : dr.GetDecimal("Odstupanje");
                nalog.Cijena = dr.IsDBNull(dr.GetOrdinal("Cijena")) ? Decimal.Zero : dr.GetDecimal("Cijena");
                nalog.Iznos = dr.IsDBNull(dr.GetOrdinal("Iznos")) ? Decimal.Zero : dr.GetDecimal("Iznos");
                nalog.PoslovnaJedinica = dr.IsDBNull(dr.GetOrdinal("PoslovnaJedinica")) ? 0 : dr.GetInt32("PoslovnaJedinica");
                nalog.Storniran = dr.IsDBNull(dr.GetOrdinal("Storniran")) ? false : dr.GetBoolean("Storniran");
                SetBaseProps<MagistralaNalog>(nalog, dr);
                return nalog;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta Nalog iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static VrstaRecepta CreateVrstaReceptaFromReader(
            MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            VrstaRecepta vrstaRecepta = null;
            try
            {
                vrstaRecepta = new VrstaRecepta(dr.GetInt32("ID"));
                vrstaRecepta.Naziv = dr.IsDBNull(dr.GetOrdinal("Naziv")) ? String.Empty : dr.GetString("Naziv");
                SetBaseProps<VrstaRecepta>(vrstaRecepta, dr);
                return vrstaRecepta;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta VrstaRecepta iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }
        public static Ljekar CreateLjekarFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        { 
            Ljekar ljekar = null;
            try
            {
                ljekar = new Ljekar(dr.GetInt32("ID"));
                ljekar.Ime = dr.IsDBNull(dr.GetOrdinal("Ime")) ? String.Empty : dr.GetString("Ime");
                ljekar.Sifra = dr.IsDBNull(dr.GetOrdinal("Sifra")) ? String.Empty : dr.GetString("Sifra");
                ljekar.Aktivan = dr.IsDBNull(dr.GetOrdinal("Aktivan")) ? false : dr.GetBoolean("Aktivan");
                SetBaseProps<Ljekar>(ljekar, dr);
                return ljekar;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta Ljekar iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static Mjera CreateMjeraFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Mjera mjera = null;
            try
            {
                mjera = new Mjera(dr.GetInt32("ID"));
                mjera.Naziv = dr["Naziv"].GetType() == typeof(System.DBNull) ? String.Empty : dr.GetString("Naziv");
                SetBaseProps<Mjera>(mjera, dr);
                return mjera;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta Mjera iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static Dokument CreateDokumentFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Dokument dokument = null;
            try
            {
                dokument = new Dokument(dr.GetInt32("ID"));
                dokument.BrojDokumenta = dr["BrojDokumenta"].GetType() == typeof(System.DBNull) ? 0 : dr.GetInt32("BrojDokumenta");
                dokument.BrojRacuna = dr["BrojRacuna"].GetType() == typeof(System.DBNull) ? String.Empty : dr.GetString("BrojRacuna");
                dokument.DatumDokumenta = dr["DatumDokumenta"].GetType() == typeof(System.DBNull) ? DateTime.MinValue : dr.GetDateTime("DatumDokumenta");
                dokument.DatumRacuna = dr["DatumRacuna"].GetType() == typeof(System.DBNull) ? DateTime.MinValue : dr.GetDateTime("DatumRacuna");
                dokument.FakturnaVrijednost = dr["FakturnaVrijednost"].GetType() == typeof(System.DBNull) ? Decimal.Parse("0") : dr.GetDecimal("FakturnaVrijednost");
                dokument.Rabat = dr["Rabat"].GetType() == typeof(System.DBNull) ? Decimal.Parse("0") : dr.GetDecimal("Rabat");
                dokument.UlazniPDV = dr["UlazniPDV"].GetType() == typeof(System.DBNull) ? Decimal.Parse("0") : dr.GetDecimal("UlazniPDV");
                dokument.VrstaDokumenta = dr["VrstaDokumenta"].GetType() == typeof(System.DBNull) ? 0 : (VrstaDokumentaEnum)dr.GetInt32("VrstaDokumenta");
                dokument.KomitentID = dr["KomitentID"].GetType() == typeof(System.DBNull) ? 0 : dr.GetInt32("KomitentID");
                dokument.NabavnaVrijednost = dr["NabavnaVrijednost"].GetType() == typeof(System.DBNull) ? Decimal.Parse("0") : dr.GetDecimal("NabavnaVrijednost");
                dokument.Marza = dr["Marza"].GetType() == typeof(System.DBNull) ? Decimal.Parse("0") : dr.GetDecimal("Marza");
                dokument.UkalkulisaniPDV = dr["UkalkulisaniPDV"].GetType() == typeof(System.DBNull) ? Decimal.Parse("0") : dr.GetDecimal("UkalkulisaniPDV");
                dokument.ProdajnaVrijednost = dr["ProdajnaVrijednost"].GetType() == typeof(System.DBNull) ? Decimal.Parse("0") : dr.GetDecimal("ProdajnaVrijednost");
                dokument.Zatvoreno = dr.IsDBNull(dr.GetOrdinal("Zatvoreno")) ? false : dr.GetBoolean("Zatvoreno");

                dokument.BrojTK = dr["BrojTK"].GetType() == typeof(System.DBNull) ? 0 : dr.GetInt32("BrojTK");
                dokument.Exportovano = dr["Exportovano"].GetType() == typeof(System.DBNull) ? 0 : dr.GetInt32("Exportovano");
                SetBaseProps<Dokument>(dokument, dr);
                return dokument;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta Dokument iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static Roba CreateRobaFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Roba roba = null;
            try
            {
                roba = new Roba(dr.GetInt32("ID"));
                roba.Sifra = dr["Sifra"].GetType() == typeof(System.DBNull) ? String.Empty : dr.GetString("Sifra");
                roba.Naziv = dr["Naziv"].GetType() == typeof(System.DBNull) ? String.Empty : dr.GetString("Naziv");
                roba.ProizvodjacID = dr["ProizvodjacID"].GetType() == typeof(System.DBNull) ? 0 : dr.GetInt32("ProizvodjacID");
                roba.FakturnaCijena = dr["FakturnaCijena"].GetType() == typeof(System.DBNull) ? 0 : dr.GetDecimal("FakturnaCijena");
                roba.PDVStopaID = dr["PDVStopaID"].GetType() == typeof(System.DBNull) ? 0 : dr.GetInt32("PDVStopaID");
                roba.PocetnoStanje = dr["PocetnoStanje"].GetType() == typeof(System.DBNull) ? 0 : dr.GetDecimal("PocetnoStanje");
                roba.Ulaz = dr["Ulaz"].GetType() == typeof(System.DBNull) ? 0 : dr.GetDecimal("Ulaz");
                roba.Izlaz = dr["Izlaz"].GetType() == typeof(System.DBNull) ? 0 : dr.GetDecimal("Izlaz");
                roba.Zaliha = dr["Zaliha"].GetType() == typeof(System.DBNull) ? 0 : dr.GetDecimal("Zaliha");
                roba.ReferalnaCijena = dr["ReferalnaCijena"].GetType() == typeof(System.DBNull) ? 0 : dr.GetDecimal("ReferalnaCijena");
                roba.ATC = dr["ATC"].GetType() == typeof(System.DBNull) ? String.Empty : dr.GetString("ATC");
                roba.Participacija = dr["Participacija"].GetType() == typeof(System.DBNull) ? 0 : dr.GetInt32("Participacija");
                roba.Opis = dr["Opis"].GetType() == typeof(System.DBNull) ? String.Empty : dr.GetString("Opis");
                roba.MPC = dr["MPC"].GetType() == typeof(System.DBNull) ? 0 : dr.GetDecimal("MPC");
                roba.Popust = dr["Popust"].GetType() == typeof(System.DBNull) ? 0 : dr.GetDecimal("Popust");
                roba.DoplataBezPDV = dr["DoplataBezPDV"].GetType() == typeof(System.DBNull) ? 0 : dr.GetDecimal("DoplataBezPDV");
                roba.DoplataSaPDV = dr["DoplataSaPDV"].GetType() == typeof(System.DBNull) ? 0 : dr.GetDecimal("DoplataSaPDV");
                roba.RezimIzdavanja = dr["RezimIzdavanja"].GetType() == typeof(System.DBNull) ? String.Empty : dr.GetString("RezimIzdavanja");
                roba.MjeraID = dr["MjeraID"].GetType() == typeof(System.DBNull) ? 0 : dr.GetInt32("MjeraID");
                roba.StopaMarze = dr["StopaMarze"].GetType() == typeof(System.DBNull) ? 0 : dr.GetDecimal("StopaMarze");
                roba.VrstaProskripcije = dr["VrstaProskripcije"].GetType() == typeof(System.DBNull) ? 0 : (VrstaProskripcije)dr.GetInt32("VrstaProskripcije");
                roba.Taksa = dr["Taksa"].GetType() == typeof(System.DBNull) ? false : dr.GetBoolean("Taksa");
                if (!dr.IsDBNull(dr.GetOrdinal("DatumCijene")))
                    roba.DatumCijene = dr.GetDateTime("DatumCijene");

                if (dr["Oznaka"].GetType() == typeof(System.DBNull)
                    || String.IsNullOrEmpty(dr["Oznaka"].ToString()))
                    roba.Oznaka = ' ';
                else
                    roba.Oznaka = dr.GetChar("Oznaka");

                roba.TipRobeID = dr.IsDBNull(dr.GetOrdinal("TipRobeID")) ? 0 : dr.GetInt32("TipRobeID"); 

                SetBaseProps<Roba>(roba, dr);
                return roba;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta Roba iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static BarKod CreateBarKodFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            BarKod barKod = null;
            try
            {
                barKod = new BarKod(dr.GetInt32("ID"));
                barKod.Kod = dr.IsDBNull(dr.GetOrdinal("BarKod")) ? String.Empty : dr.GetString("BarKod");
                barKod.RobaID = dr.IsDBNull(dr.GetOrdinal("RobaID")) ? 0 : dr.GetInt32("RobaID");
                SetBaseProps<BarKod>(barKod, dr);
                return barKod;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta BarKod iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }
        private static void SetBaseProps<T>(ApotekaBase<T> obj ,MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            obj.KreatorID = dr["KreatorID"].GetType() == typeof(System.DBNull) ? 0 : dr.GetInt32("KreatorID");
            obj.ModifikatorID = dr["ModifikatorID"].GetType() == typeof(System.DBNull) ? 0 : dr.GetInt32("ModifikatorID");
            if (!dr.IsDBNull(dr.GetOrdinal("VrijemeModifikacije")))
                obj.VrijemeModificiranja = dr.GetDateTime("VrijemeModifikacije");
            if (!dr.IsDBNull(dr.GetOrdinal("VrijemeKreiranja")))
                obj.VrijemeKreiranja = dr.GetDateTime("VrijemeKreiranja");
            obj.State = ObjectState.Existing;
        }
        public static DokumentStavka CreateStavkaDokumentaFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            DokumentStavka stavka = null;
            try
            {
                stavka = new DokumentStavka(dr.GetInt32("ID"));
                stavka.RobaID = dr.IsDBNull(dr.GetOrdinal("RobaID")) ? 0 : dr.GetInt32("RobaID");
                stavka.DokumentID = dr.IsDBNull(dr.GetOrdinal("DokumentID")) ? 0 : dr.GetInt32("DokumentID");
                stavka.PDVStopaID = dr.IsDBNull(dr.GetOrdinal("PDVStopaID")) ? 0 : dr.GetInt32("PDVStopaID");
                stavka.Kolicina = dr.IsDBNull(dr.GetOrdinal("Kolicina")) ? 0 : dr.GetDecimal("Kolicina");
                stavka.FakturnaCijena = dr.IsDBNull(dr.GetOrdinal("FakturnaCijena")) ? 0 : dr.GetDecimal("FakturnaCijena");
                stavka.FakturnaVrijednost = dr.IsDBNull(dr.GetOrdinal("FakturnaVrijednost")) ? 0 : dr.GetDecimal("FakturnaVrijednost");
                stavka.Rabat = dr.IsDBNull(dr.GetOrdinal("Rabat")) ? 0 : dr.GetDecimal("Rabat");
                stavka.Marza = dr.IsDBNull(dr.GetOrdinal("Marza")) ? 0 : dr.GetDecimal("Marza");
                stavka.TBR = dr["TBR"].GetType() == typeof(System.DBNull) ? String.Empty : dr.GetString("TBR");
                stavka.PDVStopa = dr.IsDBNull(dr.GetOrdinal("PDVStopa")) ? 0 : dr.GetDecimal("PDVStopa");
                stavka.UkalkulisaniPDV = dr.IsDBNull(dr.GetOrdinal("UkalkulisaniPDV")) ? 0 : dr.GetDecimal("UkalkulisaniPDV");
                stavka.MPC = dr.IsDBNull(dr.GetOrdinal("MPC")) ? 0 : dr.GetDecimal("MPC");
                stavka.MPV = dr.IsDBNull(dr.GetOrdinal("MPV")) ? 0 : dr.GetDecimal("MPV");
                stavka.Ulaz = dr.IsDBNull(dr.GetOrdinal("Ulaz")) ? 0 : dr.GetDecimal("Ulaz");
                stavka.Zaliha = dr.IsDBNull(dr.GetOrdinal("Zaliha")) ? 0 : dr.GetDecimal("Zaliha");
                SetBaseProperties<DokumentStavka>(stavka, dr);
                return stavka;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta DokumentStavka iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static Komitent CreateKomitentFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Komitent komitent = null;
            try
            {
                komitent = new Komitent(dr.GetInt32("ID"));
                komitent.Sifra = dr.IsDBNull(dr.GetOrdinal("Sifra")) ? String.Empty : dr.GetString("Sifra");
                komitent.Naziv = dr.IsDBNull(dr.GetOrdinal("Naziv")) ? String.Empty : dr.GetString("Naziv");
                komitent.Mjesto = dr.IsDBNull(dr.GetOrdinal("Mjesto")) ? String.Empty : dr.GetString("Mjesto");
                komitent.Adresa = dr.IsDBNull(dr.GetOrdinal("Adresa")) ? String.Empty : dr.GetString("Adresa");
                komitent.IDB = dr.IsDBNull(dr.GetOrdinal("IDB")) ? String.Empty : dr.GetString("IDB");
                komitent.PDV = dr.IsDBNull(dr.GetOrdinal("PDV")) ? String.Empty : dr.GetString("PDV");
                komitent.Racun = dr.IsDBNull(dr.GetOrdinal("Racun")) ? String.Empty : dr.GetString("Racun");
                SetBaseProps<Komitent>(komitent, dr);
                return komitent;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta Komitent iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static Racun CreateRacunFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Racun racun = null;
            try
            {
                racun = new Racun(dr.GetInt32("ID"));
                racun.Datum = dr.IsDBNull(dr.GetOrdinal("Datum")) ? DateTime.MinValue : dr.GetDateTime("Datum");
                racun.Vrijednost = dr.IsDBNull(dr.GetOrdinal("Vrijednost")) ? 0 : dr.GetDecimal("Vrijednost");
                racun.Iznos = dr.IsDBNull(dr.GetOrdinal("Iznos")) ? 0 : dr.GetDecimal("Iznos");
                racun.IznosPopusta = dr.IsDBNull(dr.GetOrdinal("IznosPopusta")) ? 0 : dr.GetDecimal("IznosPopusta");
                racun.DoplataSaPDV = dr.IsDBNull(dr.GetOrdinal("DoplataSaPDV")) ? 0 : dr.GetDecimal("DoplataSaPDV");
                racun.NacinPlacanja = dr["NacinPlacanja"].GetType() == typeof(System.DBNull) ? 0 : (NacinPlacanja)dr.GetInt32("NacinPlacanja");
                racun.SmjenaID = dr["SmjenaID"].GetType() == typeof(System.DBNull) ? 0 : dr.GetInt32("SmjenaID");
                racun.Isprintan = dr["Isprintan"].GetType() == typeof(System.DBNull) ? false : dr.GetBoolean("Isprintan");
                racun.Storniran = dr["Storniran"].GetType() == typeof(System.DBNull) ? false : dr.GetBoolean("Storniran");
                racun.KomitentID = dr["KomitentID"].GetType() == typeof(System.DBNull) ? 0 : dr.GetInt32("KomitentID");
                racun.Zaokruzenje = dr.IsDBNull(dr.GetOrdinal("Zaokruzenje")) ? 0 : dr.GetDecimal("Zaokruzenje");
                //jasenko tring
                racun.BrojFisk = dr["BrojFisk"].GetType() == typeof(System.DBNull) ? 0 : dr.GetInt32("BrojFisk");
                racun.BrojKase = dr["BrojKase"].GetType() == typeof(System.DBNull) ? 0 : dr.GetInt32("BrojKase");
                racun.FiskalniIznos = dr.IsDBNull(dr.GetOrdinal("FiskalniIznos")) ? 0 : dr.GetDecimal("FiskalniIznos");
                racun.FiskalniDatum = dr.IsDBNull(dr.GetOrdinal("FiskalniDatum")) ? DateTime.MinValue : dr.GetDateTime("FiskalniDatum");

                SetBaseProps<Racun>(racun, dr);
                return racun;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta Racun iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static RacunStavka CreateStavkaRacunaFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            RacunStavka stavkaRacuna = null;
            try
            {
                stavkaRacuna = new RacunStavka(dr.GetInt32("ID"));
                stavkaRacuna.RacunID = dr.IsDBNull(dr.GetOrdinal("RacunID")) ? 0 : dr.GetInt32("RacunID");
                stavkaRacuna.RobaID = dr.IsDBNull(dr.GetOrdinal("RobaID")) ? 0 : dr.GetInt32("RobaID");
                //stavkaRacuna.MjeraID = 0;
                stavkaRacuna.Kolicina = dr.IsDBNull(dr.GetOrdinal("Kolicina")) ? 0 : dr.GetDecimal("Kolicina");
                stavkaRacuna.PDVStopa = dr.IsDBNull(dr.GetOrdinal("PDVStopa")) ? 0 : dr.GetDecimal("PDVStopa");
                stavkaRacuna.Participacija = dr.IsDBNull(dr.GetOrdinal("Participacija")) ? 0 : dr.GetInt32("Participacija");
                stavkaRacuna.ReferalnaCijena = dr.IsDBNull(dr.GetOrdinal("ReferalnaCijena")) ? 0 : dr.GetDecimal("ReferalnaCijena");
                stavkaRacuna.Popust = dr.IsDBNull(dr.GetOrdinal("Popust")) ? 0 : dr.GetDecimal("Popust");
                stavkaRacuna.IznosPopusta = dr.IsDBNull(dr.GetOrdinal("IznosPopusta")) ? 0 : dr.GetDecimal("IznosPopusta");
                stavkaRacuna.DoplataBezPDV = dr.IsDBNull(dr.GetOrdinal("DoplataBezPDV")) ? 0 : dr.GetDecimal("DoplataBezPDV");
                stavkaRacuna.DoplataSaPDV = dr.IsDBNull(dr.GetOrdinal("DoplataSaPDV")) ? 0 : dr.GetDecimal("DoplataSaPDV");
                stavkaRacuna.Cijena = dr.IsDBNull(dr.GetOrdinal("Cijena")) ? 0 : dr.GetDecimal("Cijena");
                stavkaRacuna.Vrijednost = dr.IsDBNull(dr.GetOrdinal("Vrijednost")) ? 0 : dr.GetDecimal("Vrijednost");
                stavkaRacuna.Iznos = dr.IsDBNull(dr.GetOrdinal("Iznos")) ? 0 : dr.GetDecimal("Iznos");
                stavkaRacuna.BrojRecepta = dr.IsDBNull(dr.GetOrdinal("BrojRecepta")) ? String.Empty : dr.GetString("BrojRecepta");
                stavkaRacuna.LjekarID = dr.IsDBNull(dr.GetOrdinal("LjekarID")) ? 0 : dr.GetInt32("LjekarID");
                stavkaRacuna.VrstaID = dr.IsDBNull(dr.GetOrdinal("VrstaID")) ? 0 : dr.GetInt32("VrstaID");
                stavkaRacuna.Pausal = dr.IsDBNull(dr.GetOrdinal("Pausal")) ? 0 : dr.GetDecimal("Pausal");
                stavkaRacuna.JMBG = dr.IsDBNull(dr.GetOrdinal("JMBG")) ? String.Empty : dr.GetString("JMBG");
                stavkaRacuna.Izlaz = dr.IsDBNull(dr.GetOrdinal("Izlaz")) ? 0 : dr.GetDecimal("Izlaz");
                stavkaRacuna.Zaliha = dr.IsDBNull(dr.GetOrdinal("Zaliha")) ? 0 : dr.GetDecimal("Zaliha");
                stavkaRacuna.IznosTakse = dr.IsDBNull(dr.GetOrdinal("IznosTakse")) ? 0 : dr.GetDecimal("IznosTakse");
                stavkaRacuna.BrJedinica = dr.IsDBNull(dr.GetOrdinal("BrJedinica")) ? 0 : dr.GetInt32("BrJedinica");
                stavkaRacuna.IznosKupac = dr.IsDBNull(dr.GetOrdinal("IznosKupac")) ? 0 : dr.GetDecimal("IznosKupac");
                stavkaRacuna.IznosKzzo = dr.IsDBNull(dr.GetOrdinal("IznosKzzo")) ? 0 : dr.GetDecimal("IznosKzzo");
                stavkaRacuna.Stornirano = dr.IsDBNull(dr.GetOrdinal("Stornirano")) ? false : dr.GetBoolean("Stornirano");
                stavkaRacuna.DatumPropisivanja = dr.IsDBNull(dr.GetOrdinal("DatumPropisivanja")) ? DateTime.MinValue : dr.GetDateTime("DatumPropisivanja");
                
                stavkaRacuna.PonovljenRecept = dr.IsDBNull(dr.GetOrdinal("PonovljenRecept")) ? 0 : dr.GetInt32("PonovljenRecept");
                stavkaRacuna.PropisanaKolicina = dr.IsDBNull(dr.GetOrdinal("PropisanaKolicina")) ? 0 : dr.GetDecimal("PropisanaKolicina");

                SetBaseProps<RacunStavka>(stavkaRacuna, dr);
                return stavkaRacuna;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta RacunStavka iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static Smjena CreateSmjenaFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Smjena smjena = null;
            try
            {
                smjena = new Smjena(dr.GetInt32("ID"));
                smjena.VrijemeOtvaranja = dr.IsDBNull(dr.GetOrdinal("VrijemeOtvaranja")) ? DateTime.MinValue : dr.GetDateTime("VrijemeOtvaranja");
                smjena.VrijemeZatvaranja = dr.IsDBNull(dr.GetOrdinal("VrijemeZatvaranja")) ? DateTime.MinValue : dr.GetDateTime("VrijemeZatvaranja");
                smjena.Broj = dr.IsDBNull(dr.GetOrdinal("Broj")) ? 0 : dr.GetInt32("Broj");
                smjena.OdgovornoLiceID = dr.IsDBNull(dr.GetOrdinal("OdgovornoLiceID")) ? 0 : dr.GetInt32("OdgovornoLiceID");
                smjena.Ukupno = dr.IsDBNull(dr.GetOrdinal("Ukupno")) ? 0 : dr.GetDecimal("Ukupno");
                smjena.Zatvorena = dr.IsDBNull(dr.GetOrdinal("Zatvorena")) ? false : dr.GetBoolean("Zatvorena");
                SetBaseProps<Smjena>(smjena, dr);
                return smjena;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta Smjena iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static PDVStopa CreatePDVStopaFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            PDVStopa stopa = null;
            try
            {
                stopa = new PDVStopa(dr.GetInt32("ID"));
                stopa.Stopa = dr.IsDBNull(dr.GetOrdinal("Stopa")) ? 0 : dr.GetDecimal("Stopa");
                stopa.Naziv = dr.IsDBNull(dr.GetOrdinal("Naziv")) ? String.Empty : dr.GetString("Naziv");
                stopa.TBR = dr.IsDBNull(dr.GetOrdinal("TBR")) ? String.Empty : dr.GetString("TBR");
                stopa.Opis = dr.IsDBNull(dr.GetOrdinal("Opis")) ? String.Empty : dr.GetString("Opis");
                stopa.UplatniRacun = dr.IsDBNull(dr.GetOrdinal("UplatniRacun")) ? String.Empty : dr.GetString("UplatniRacun");
                SetBaseProps<PDVStopa>(stopa, dr);
                return stopa;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta PDVStopa iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static Korisnik CreateKorisnikFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Korisnik korisnik = null;
            try
            {
                korisnik = new Korisnik(dr.GetInt32("ID"));
                korisnik.Ime = dr.IsDBNull(dr.GetOrdinal("Ime")) ? String.Empty : dr.GetString("Ime");
                korisnik.Prezime = dr.IsDBNull(dr.GetOrdinal("Prezime")) ? String.Empty : dr.GetString("Prezime");
                korisnik.KorisnickoIme = dr.IsDBNull(dr.GetOrdinal("KorisnickoIme")) ? String.Empty : dr.GetString("KorisnickoIme");
                korisnik.Lozinka = dr.IsDBNull(dr.GetOrdinal("Lozinka")) ? String.Empty : dr.GetString("Lozinka");
                korisnik.EMail = dr.IsDBNull(dr.GetOrdinal("EMail")) ? String.Empty : dr.GetString("EMail");
                korisnik.TitulaID = dr.IsDBNull(dr.GetOrdinal("TitulaID")) ? 0 : dr.GetInt32("TitulaID");
                korisnik.IsAdmin = dr.IsDBNull(dr.GetOrdinal("Admin")) ? false : dr.GetBoolean("Admin");
                SetBaseProps<Korisnik>(korisnik, dr);
                return korisnik;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta Korisnik iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static Proizvodjac CreateProizvodjacFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Proizvodjac pro = null;
            try
            {
                pro = new Proizvodjac(dr.GetInt32("ID"));
                pro.Naziv = dr.IsDBNull(dr.GetOrdinal("Naziv")) ? String.Empty : dr.GetString("Naziv");
                pro.Sifra = dr.IsDBNull(dr.GetOrdinal("Sifra")) ? String.Empty : dr.GetString("Sifra");
                pro.Telefon = dr.IsDBNull(dr.GetOrdinal("Telefon")) ? String.Empty : dr.GetString("Telefon");
                pro.Email = dr.IsDBNull(dr.GetOrdinal("Email")) ? String.Empty : dr.GetString("Email");
                pro.Grad = dr.IsDBNull(dr.GetOrdinal("Grad")) ? String.Empty : dr.GetString("Grad");
                pro.PTT = dr.IsDBNull(dr.GetOrdinal("PTT")) ? String.Empty : dr.GetString("PTT");
                pro.PoslovnaJedinica = dr.IsDBNull(dr.GetOrdinal("PoslovnaJedinica")) ? 0 : dr.GetInt32("PoslovnaJedinica");
                SetBaseProps<Proizvodjac>(pro, dr);
                return pro;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta Proizvodjac iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }


        public static Proskripcija CreateProskripcijaFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Proskripcija pros = null;
            try
            {
                pros = new Proskripcija(dr.GetInt32("ID"));
                pros.Naziv = dr.IsDBNull(dr.GetOrdinal("Naziv")) ? String.Empty : dr.GetString("Naziv");
                pros.Sifra = dr.IsDBNull(dr.GetOrdinal("Sifra")) ? String.Empty : dr.GetString("Sifra");
                pros.Vrsta = dr["Vrsta"].GetType() == typeof(System.DBNull) ? 0 : (MagistralnaVrsta)dr.GetInt32("Vrsta");
                SetBaseProps<Proskripcija>(pros, dr);
                return pros;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta Proskripcija iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static Normativ CreateNormativFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Normativ normativ = null;
            try
            {
                normativ = new Normativ(dr.GetInt32("ID"));
                normativ.RobaID = dr.IsDBNull(dr.GetOrdinal("RobaID")) ? 0 : dr.GetInt32("RobaID");
                normativ.MaterijalRobaID = dr.IsDBNull(dr.GetOrdinal("MaterijalRobaID")) ? 0 : dr.GetInt32("MaterijalRobaID");
                normativ.NormativVrijednost = dr.IsDBNull(dr.GetOrdinal("Normativ")) ? 0 : dr.GetDecimal("Normativ");
                normativ.PoslovnaJedinica = dr.IsDBNull(dr.GetOrdinal("PoslovnaJedinica")) ? 0 : dr.GetInt32("PoslovnaJedinica");
                normativ.Taksa = dr.IsDBNull(dr.GetOrdinal("Taksa")) ? false : dr.GetBoolean("Taksa");
                SetBaseProps<Normativ>(normativ, dr);
                return normativ;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta Normativ iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }



        public static MagistralaNalogStavka CreateMagistralaNalogStavkaFromReader(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            MagistralaNalogStavka stavkaNaloga = null;
            try
            {
                stavkaNaloga = new MagistralaNalogStavka(dr.GetInt32("ID"));
                stavkaNaloga.MaterijalRobaID = dr.IsDBNull(dr.GetOrdinal("MaterijalRobaID")) ? 0 : dr.GetInt32("MaterijalRobaID");
                stavkaNaloga.MagistralaNalogID = dr.IsDBNull(dr.GetOrdinal("MagNalogID")) ? 0 : dr.GetInt32("MagNalogID");
                stavkaNaloga.Kolicina = dr.IsDBNull(dr.GetOrdinal("Kolicina")) ? 0 : dr.GetDecimal("Kolicina");
                stavkaNaloga.Normativ = dr.IsDBNull(dr.GetOrdinal("Normativ")) ? 0 : dr.GetDecimal("Normativ");
                stavkaNaloga.Iznos = dr.IsDBNull(dr.GetOrdinal("Iznos")) ? 0 : dr.GetDecimal("Iznos");
                stavkaNaloga.Cijena = dr.IsDBNull(dr.GetOrdinal("Cijena")) ? 0 : dr.GetDecimal("Cijena");
                stavkaNaloga.Taksa = dr.IsDBNull(dr.GetOrdinal("Taksa")) ? false : dr.GetBoolean("Taksa");
                SetBaseProps<MagistralaNalogStavka>(stavkaNaloga, dr);
                return stavkaNaloga;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška u kreiranju objekta Normativ iz readera.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }
    }
}
