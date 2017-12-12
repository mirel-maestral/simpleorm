using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects.Core;
using Apoteke.DataObjects.BLL;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using Apoteke.DataObjects;
using System.Text.RegularExpressions;
using System.Drawing.Printing;
using System.IO;
using System.IO.Compression;
using System.ComponentModel;
using System.Drawing;

namespace Apoteke.DataObjects
{
    public class ApotekeDB: Apoteke.DataObjects.Core.Database
    {
        //connection stringovi na druge servere i baze
        static string connectionString_192_168_0_3 = "Database=DB_" + GlobalVariable.godina + ";Data Source=192.168.0.3;User Id=fris_rw;Password=italija;charset=utf8";
        static string connectionString_Centralni_Sifarnik = "server=80.65.164.179;Database=Apoteka_2010; uid=as_mir09; password=6asp2nk;charset=utf8;Connect Timeout=15";
        static string connectionString_192_168_1_4_Robno = "Database=Robno_" + GlobalVariable.godina + ";Data Source=192.168.1.4;User Id=as_mir09;Password=6asp2nk;charset=utf8";
        static string connectionString_192_168_0_3_Robno = "Database=Robno_" + "2017" + ";Data Source=192.168.0.3;User Id=fris_rw;Password=italija;charset=utf8";
        static string connectionStringRemoute = "server=" + GlobalVariable.IPjavna + ";Database =" + GlobalVariable.serverBaza + GlobalVariable.godina + "; uid=as_mir09; password=6asp2nk;charset=utf8;Connect Timeout=5";
        //static string connectionStringRemoute = "server=" + GlobalVariable.IPjavna + ";Database =" + GlobalVariable.serverBaza + GlobalVariable.godina + "; uid=Exp2011K; password=NiS6dRL;charset=utf8;Connect Timeout=5";
        
        private static ApotekeDB _instance = null;
        public static ApotekeDB Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ApotekeDB();
                return _instance;
            }
        }

        protected ApotekeDB()
            : base()
        {
            _instance = this;
        }

        #region Main methods

        public List<Mjera> GetMjere()
        {
            return GetMjere(false);
        }

        public List<Mjera> GetMjere(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<Mjera>();

            return GetObjectAll<Mjera>("sp_Mjera_SelAll", Factory.CreateMjeraFromReader);
        }

        public Mjera GetMjera(int id)
        {
            return GetMjera(id, false);
        }

        public Mjera GetMjera(int id, bool useReflection)
        {
            if (useReflection)
                return GetObjectByID<Mjera>(id);

            return GetObjectByID<Mjera>(id, "sp_Mjera_SelByID", Factory.CreateMjeraFromReader);
        }

        public List<Dokument> GetDokumenti(bool useReflection, VrstaDokumentaEnum vrstaDokumenta)
        {
            List<Dokument> dokumenti = new List<Dokument>();

            if (useReflection)
            {
                foreach (Dokument doc in GetObjectAll<Dokument>())
                    if (doc.VrstaDokumenta == vrstaDokumenta)
                        dokumenti.Add(doc);

                return dokumenti;
            }

            try
            {   
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_Dokument_SelByVrsta", con);
                    command.Parameters.Add(new MySqlParameter("@pVrstaDokumenta", MySqlDbType.Int32)).Value = (int)vrstaDokumenta;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        dokumenti.Add(Factory.CreateDokumentFromReader(dr));
                }

            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Error while getting Kalkulacije.", Logging.LogEntryLevel.Critical, ex);
            }

            return dokumenti;
        }

        public Proskripcija GetProskripcija(int Id)
        {
            return GetObjectByID<Proskripcija>(Id);
        }

        public Dokument GetDokument(int id)
        {
            return GetDokument(id, false);
        }

        public Dokument GetDokument(int id, bool useReflection)
        {
            if (useReflection)
                return GetObjectByID<Dokument>(id);

            return GetObjectByID<Dokument>(id, "sp_Dokument_SelByID", Factory.CreateDokumentFromReader);
        }

        public List<Komitent> GetKomitenti(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<Komitent>();

            return GetObjectAll<Komitent>("sp_Komitent_SelAll", Factory.CreateKomitentFromReader);
        }
    
        public Komitent GetKomitent(int id)
        {
            return GetKomitent(id, false);
        }

        public Komitent GetKomitent(int id, bool useReflection)
        {
            if (useReflection)
                return GetObjectByID<Komitent>(id);

            return GetObjectByID<Komitent>(id, "sp_Komitent_SelByID", Factory.CreateKomitentFromReader);
        }

        public List<Korisnik> GetKorisnici()
        {
            return GetKorisnici(false);
        }
        public List<Korisnik> GetKorisnici(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<Korisnik>();

            return GetObjectAll<Korisnik>("sp_Korisnik_SelAll", Factory.CreateKorisnikFromReader);
        }

        public List<Proskripcija> GetProskripcije(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<Proskripcija>();

            return GetObjectAll<Proskripcija>("sp_Proskripcija_SelAll", Factory.CreateProskripcijaFromReader);
        }


        public Korisnik GetKorisnik(int id)
        {
            return GetKorisnik(id, false);
        }

        public Korisnik GetKorisnik(int id, bool useReflection)
        {
            if (useReflection)
                return GetObjectByID<Korisnik>(id);

            return GetObjectByID<Korisnik>(id, "sp_Korisnik_SelByID", Factory.CreateKorisnikFromReader);
        }

        public Ljekar GetLjekar(int id)
        {
            return GetLjekar(id, false);
        }

        public Ljekar GetLjekar(int id, bool useReflection)
        {
            if (useReflection)
                return GetObjectByID<Ljekar>(id);

            return GetObjectByID<Ljekar>(id, "sp_Ljekar_SelByID", Factory.CreateLjekarFromReader);
        }

        public List<Ljekar> GetLjekari()
        {
            return GetLjekari(false);
        }
        public List<Ljekar> GetLjekari(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<Ljekar>();

            return GetObjectAll<Ljekar>("sp_Ljekar_SelAll", Factory.CreateLjekarFromReader);
        }
        public List<MagistralaNalog> GetMagistralaNalozi()
        {
            return GetMagistralaNalozi(false);
        }

        public List<MagistralaNalog> GetMagistralaNalozi(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<MagistralaNalog>();

            return GetObjectAll<MagistralaNalog>("sp_MagNalog_SelAll", Factory.CreateNalogFromReader);
        }
        
        public List<MagistralaNalog> GetNalozi(int robaId)
        {
            List<MagistralaNalog> nolozi = new List<MagistralaNalog>();
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_MagNalog_SelByRobaID", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pRobaID", MySqlDbType.Int32)).Value = robaId;
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        nolozi.Add(Factory.CreateNalogFromReader(dr));
                }

            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom loada naloga iz baze.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return nolozi;
        }

        public List<Normativ> GetNormativi(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<Normativ>();

            return GetObjectAll<Normativ>("sp_MagNormativ_SelAll", Factory.CreateNormativFromReader);
        }

        public List<Normativ> GetNormativi(int robaId)
        {
           List<Normativ> normativi = new List<Normativ>();
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_MagNormativ_SelByRobaID", con);
                    command.Parameters.Add(new MySqlParameter("@pRobaID", MySqlDbType.Int32)).Value = robaId;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        normativi.Add(Factory.CreateNormativFromReader(dr));
                }

            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom loada normativa iz baze.", 
                    Logging.LogEntryLevel.Critical, ex);
            }

            return normativi;
        }

        public decimal GetLastFakturnaCijena(int robaId)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_GetLastFakturnaCijena", con);
                    command.Parameters.Add(new MySqlParameter("@pRobaID", MySqlDbType.Int32)).Value = robaId;
                    command.Parameters.Add(new MySqlParameter("@pFakturnaCijena", MySqlDbType.Decimal)).Direction = ParameterDirection.Output;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.ExecuteNonQuery();

                    return (decimal)command.Parameters["@pFakturnaCijena"].Value;
                }

            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom uzimanja zadnje fakturne cijene.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return 0;

        }

        public List<PDVStopa> GetPDVStope()
        {
            return GetPDVStope(false);
        }

        public List<PDVStopa> GetPDVStope(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<PDVStopa>();

            return GetObjectAll<PDVStopa>("sp_PDVStopa_SelAll", Factory.CreatePDVStopaFromReader);
        }

        public PDVStopa GetPDVStopa(int id)
        {
            return GetPDVStopa(id, false);
        }

        public PDVStopa GetPDVStopa(int id, bool useReflection)
        {
            if (useReflection)
                return GetObjectByID<PDVStopa>(id);

            return GetObjectByID<PDVStopa>(id, "sp_PDVStopa_SelByID", Factory.CreatePDVStopaFromReader);
        }

        public List<Recept> GetRecepti()
        {
            return GetObjectAll<Recept>();
        }

        public Recept GetRecept(int Id)
        {
            return GetObjectByID<Recept>(Id);
        }

        public List<Roba> GetProskripcije()
        {
            List<Roba> roba = new List<Roba>();
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_Roba_SelProskripcije", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        roba.Add(Factory.CreateRobaFromReader(dr));
                }

            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Error while getting Proskripcije.", Logging.LogEntryLevel.Critical, ex);
            }

            return roba;
        }

        public List<Roba> GetRoba(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<Roba>();

            return GetObjectAll<Roba>("sp_Roba_SelAll", Factory.CreateRobaFromReader);
        }

        public List<Roba> GetRobaZaMagistralu()
        {
            return GetRobaZaMagistralu(false);
        }

        public List<Roba> GetRobaZaMagistralu(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<Roba>();

            return GetObjectAll<Roba>("sp_RobaMagTip_SelAll", Factory.CreateRobaFromReader);
        }

        public List<Roba> GetRobaByNaziv(string naziv)
        {
            List<Roba> roba = new List<Roba>();
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_Roba_SelByNaziv", con);
                    command.Parameters.Add(new MySqlParameter("@pNaziv", MySqlDbType.VarChar)).Value = naziv;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        roba.Add(Factory.CreateRobaFromReader(dr));
                }

            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Error while getting Roba by Naziv.", Logging.LogEntryLevel.Critical, ex);
            }

            return roba;
        }

        public List<Roba> GetRobaByNaziv2(string naziv)
        {
            List<Roba> roba = new List<Roba>();
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_Roba_SelByNaziv2", con);
                    command.Parameters.Add(new MySqlParameter("@pNaziv", MySqlDbType.VarChar)).Value = naziv;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        roba.Add(Factory.CreateRobaFromReader(dr));
                }

            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Error while getting Roba by Naziv.", Logging.LogEntryLevel.Critical, ex);
            }

            return roba;
        }

        public Roba GetRoba(int Id)
        {
            return GetRoba(Id, true);
        }

        public Roba GetRoba(int id, bool useReflection)
        {
            if ( useReflection)
                return GetObjectByID<Roba>(id);

            return GetObjectByID<Roba>(id, "sp_Roba_SelByID", Factory.CreateRobaFromReader);
        }

        public List<DokumentStavka> GetStavkeDokumenata()
        {
            return GetObjectAll<DokumentStavka>();
        }

        public DokumentStavka GetStavkaDokumenta(int Id)
        {
            return GetObjectByID<DokumentStavka>(Id);
        }

        //public List<VrstaRecepta> GetVrsteRecepata()
        //{
        //    return GetObjectAll<VrstaRecepta>();
        //}
        public List<VrstaRecepta> GetVrsteRecepata()
        {
            return GetVrsteRecepata(false);
        }

        public List<VrstaRecepta> GetVrsteRecepata(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<VrstaRecepta>();

            return GetObjectAll<VrstaRecepta>("sp_VrstaRecepta_SelAll", Factory.CreateVrstaReceptaFromReader);
        }

        //public VrstaRecepta GetVrstaRecepta(int Id)
        //{
        //    return GetObjectByID<VrstaRecepta>(Id);
        //}

        public VrstaRecepta GetVrstaRecepta(int Id)
        {
            return GetVrstaRecepta(Id, false);
        }

        public VrstaRecepta GetVrstaRecepta(int id, bool useReflection)
        {
            if (useReflection)
                return GetObjectByID<VrstaRecepta>(id);

            return GetObjectByID<VrstaRecepta>(id, "sp_VrstaRecepta_SelByID", Factory.CreateVrstaReceptaFromReader);
        }

        public List<Titula> GetTitule()
        {
            return GetObjectAll<Titula>();
        }

        public Titula GetTitula(int id)
        {
            return GetObjectByID<Titula>(id);
        }



        public MagistralaNalog GetMagistralaNalog(int id)
        {
            return GetMagistralaNalog(id, false);
        }

        public MagistralaNalog GetMagistralaNalog(int id, bool useReflection)
        {
            if (id == 0)
                return null;

            if (useReflection)
                return GetObjectByID<MagistralaNalog>(id);

            return GetObjectByID<MagistralaNalog>(id, "sp_MagNalog_SelByID", Factory.CreateNalogFromReader);
        }


        public MagistralaNalogStavka GetMagistralaNalogStavka(int id)
        {
            return GetMagistralaNalogStavka(id, false);
        }

        public MagistralaNalogStavka GetMagistralaNalogStavka(int id, bool useReflection)
        {
            if (useReflection)
                return GetObjectByID<MagistralaNalogStavka>(id);

            return GetObjectByID<MagistralaNalogStavka>(id, "sp_MagStavka_SelByID", Factory.CreateMagistralaNalogStavkaFromReader);
        }

        public List<MagistralaNalogStavka> GetMagistalaNalogStavke()
        {
            return GetMagistalaNalogStavke(false);
        }

        public List<MagistralaNalogStavka> GetMagistalaNalogStavke(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<MagistralaNalogStavka>();

            return GetObjectAll<MagistralaNalogStavka>("sp_MagStavka_SelAll", Factory.CreateMagistralaNalogStavkaFromReader);
        }

        public List<Proizvodjac> GetProizvodjaci()
        {
            return GetProizvodjaci(false);
        }
        
        public List<Proizvodjac> GetProizvodjaci(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<Proizvodjac>();

            return GetObjectAll<Proizvodjac>("sp_Proizvodjac_SelAll", Factory.CreateProizvodjacFromReader);
        }

        public Proizvodjac GetProizvodjac(int id)
        {
            return GetProizvodjac(id, false);
        }

        public Proizvodjac GetProizvodjac(int id, bool useReflection)
        {
            if (id == 0)
                return null;

            if (useReflection)
                return GetObjectByID<Proizvodjac>(id);

            return GetObjectByID<Proizvodjac>(id, "sp_Proizvodjac_SelByID", Factory.CreateProizvodjacFromReader);
        }


        public List<ATC> GetATCs()
        {
            return GetObjectAll<ATC>();
        }

        public List<BarKod> GetBarKodoviByRoba(int robaId)
        {
            List<BarKod> barKodovi = new List<BarKod>();
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_BarKodSelByRobaID", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pRobaID", MySqlDbType.Int32)).Value = robaId;
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        barKodovi.Add(Factory.CreateBarKodFromReader(dr));
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Error while getting BarKod By Roba.", Logging.LogEntryLevel.Critical, ex);
            }

            return barKodovi;
        }

        public List<BarKod> GetBarKodovi(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<BarKod>();

            return GetObjectAll<BarKod>("sp_BarKod_SelAll", Factory.CreateBarKodFromReader);
        }

        public BarKod GetBarKodByID(int Id)
        {
            return GetObjectByID<BarKod>(Id);
        }

        public List<DokumentStavka> GetStavkeDokumenta(int dokumentId)
        {
            List<DokumentStavka> stavke = new List<DokumentStavka>();
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_DokumentStavka_SelByDocumentID", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pDocumentID", MySqlDbType.Int32)).Value = dokumentId;
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        stavke.Add(Factory.CreateStavkaDokumentaFromReader(dr));
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Error while getting Stavka Dokumenta.", Logging.LogEntryLevel.Critical, ex);
            }

            return stavke;
        }

        public List<TipRobe> GetTipRobe()
        {
            return GetObjectAll<TipRobe>();
        }

        public TipRobe GetTipRobe(int id)
        {
            return GetObjectByID<TipRobe>(id);
        }

        public List<Racun> GetRacuniBySmjena(int smjenaId, bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<Racun>();

            List<Racun> racuni = new List<Racun>();
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_Racun_SelBySmjena", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pSmjenaID", MySqlDbType.Int32)).Value = smjenaId;
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        racuni.Add(Factory.CreateRacunFromReader(dr));
                }

            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška prilikom učitavanja Računa.", Logging.LogEntryLevel.Critical, ex);
            }

            return racuni;
        }

        public List<Racun> GetRacuni(DateTime datumOd, DateTime datumDo)
        {
            List<Racun> racuni = new List<Racun>();
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_Racun_SelAllByDate", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pDatumOd", MySqlDbType.Date)).Value = datumOd.Date;
                    command.Parameters.Add(new MySqlParameter("@pDatumDo", MySqlDbType.Date)).Value = datumDo.Date;

                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        racuni.Add(Factory.CreateRacunFromReader(dr));
                }

            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška prilikom filtriranja Računa po datumu.", Logging.LogEntryLevel.Critical, ex);
            }

            return racuni;
        }


        public List<Racun> GetRacuni(int idFrom)
        {
            List<Racun> racuni = new List<Racun>();
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_Racun_SelAllByIDRange", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pIdFrom", MySqlDbType.Int32)).Value = idFrom;

                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        racuni.Add(Factory.CreateRacunFromReader(dr));
                }

            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška prilikom filtriranja Računa od broja do broja.", 
                    Logging.LogEntryLevel.Critical, ex);
            }

            return racuni;
        }


        public List<Racun> GetRacuni(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<Racun>();

            return GetObjectAll<Racun>("sp_Racun_SelAll", Factory.CreateRacunFromReader);
        }

        public Racun GetRacun(int Id)
        {
            return GetObjectByID<Racun>(Id);
        }

        public List<RacunStavka> GetStavkeRacuna(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<RacunStavka>();

            return GetObjectAll<RacunStavka>("sp_StavkaRacuna_SelAll", Factory.CreateStavkaRacunaFromReader);
        }

        public List<RacunStavka> GetStavkeRacunaByRacunID(int RacunID)
        {
            List<RacunStavka> stavkeRacuna = new List<RacunStavka>();
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_StavkaRacunaByRacunID", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pID", MySqlDbType.Int32)).Value = RacunID;
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        stavkeRacuna.Add(Factory.CreateStavkaRacunaFromReader(dr));
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška prilikom učitavanja Stavki Računa.", Logging.LogEntryLevel.Critical, ex);
            }

            return stavkeRacuna;
        }

        public RacunStavka GetStavkuRacuna(int Id)
        {
            return GetObjectByID<RacunStavka>(Id);
        }

        public List<Smjena> GetSmjene()
        {
            return GetSmjene(false);
        }
        public List<Smjena> GetSmjene(bool useReflection)
        {
            if (useReflection)
                return GetObjectAll<Smjena>();

            return GetObjectAll<Smjena>("sp_Smjena_SelAll", Factory.CreateSmjenaFromReader);
        }

        public Smjena GetSmjena(int id)
        {
            return GetSmjena(id, false);
        }

        public Smjena GetSmjena(int id, bool useReflection)
        {
            if (useReflection)
                return GetObjectByID<Smjena>(id);

            return GetObjectByID<Smjena>(id, "sp_Smjena_SelByID", Factory.CreateSmjenaFromReader);
        }
        
        public void FillTable(string procedureName, DataTable dt)
        {
            FillTable(procedureName, dt, null);
        }

        public DataTable GetView(string viewName)
        {
            DataTable dt = new DataTable(viewName);
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("SELECT * FROM " + viewName, con);
                    command.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter da = new MySqlDataAdapter(command);

                    da.Fill(dt);
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška prilikom učitavanja viewa [" + viewName+"]", Logging.LogEntryLevel.Critical, ex);
                return null;
            }

            return dt;
        }

        public string GenerateKzzoExportTxt(DateTime datumOd, DateTime datumDo)
        {
            StringBuilder sbOutputTxt = new StringBuilder();
            string brojApoteke = String.Empty;
            if ( Global.Instance.Konfiguracija.ContainsKey("BrojApoteke") )
                brojApoteke = Global.Instance.Konfiguracija["BrojApoteke"];
            string sifraKzzo = String.Empty;
            if ( Global.Instance.Konfiguracija.ContainsKey("SifraKZZO") )
                sifraKzzo = Global.Instance.Konfiguracija["SifraKZZO"];
            string drzava = String.Empty;

            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_GetKzzoOutput", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pDatumOd", MySqlDbType.Date)).Value = datumOd.Date;
                    command.Parameters.Add(new MySqlParameter("@pDatumDo", MySqlDbType.Date)).Value = datumDo.Date;
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        if (dr.GetInt32("ST_VrstaRecepta") <= 5)
                        {
                            if (dr.IsDBNull(dr.GetOrdinal("Drzava")) || dr.GetInt32("ST_VrstaRecepta") != 3)
                                drzava = "104";
                            else
                                drzava = dr.GetString("Drzava");

                            sbOutputTxt.Append(drzava);
                            sbOutputTxt.Append(sifraKzzo);
                            sbOutputTxt.Append(dr.GetString("ZU_Sifra_01"));
                            sbOutputTxt.Append(dr.GetString("ZU_Sifra_02"));
                            sbOutputTxt.Append(dr.GetString("ZU_Sifra_03"));
                            sbOutputTxt.Append(dr.GetString("LJ_SIFRA"));
                            sbOutputTxt.Append(dr.GetInt32("RACUN").ToString("D6"));
                            sbOutputTxt.Append("  ");
                            sbOutputTxt.Append(dr.GetString("ST_RECEPT"));
                            if (dr.GetString("PON_RECEPT").Contains("0"))
                                sbOutputTxt.Append("   ");
                            else
                                sbOutputTxt.Append(dr.GetString("PON_RECEPT"));//-P1,2,3 ponovljeni recept
                            sbOutputTxt.Append(dr.GetString("AS_SIFRA"));
                            sbOutputTxt.Append(((int)(dr.GetDecimal("PROP_KOLICINA") * 100)).ToString("D10"));//propisana količina
                            sbOutputTxt.Append(((int)(dr.GetDecimal("AS_KOL") * 100)).ToString("D10"));                            
                            sbOutputTxt.Append(((int)(Math.Round(dr.GetDecimal("REF_CIJENA") * 100, 0))).ToString("D7"));
                            sbOutputTxt.Append(dr.GetInt32("ST_PART_PROC").ToString("D3"));
                            sbOutputTxt.Append( ((int)Math.Abs(dr.GetDecimal("PAUSAL") * 100)).ToString("D7"));
                            sbOutputTxt.Append(dr.GetDateTime("RA_DATUM").ToString("ddMMyyyy"));
                            sbOutputTxt.Append(dr.GetString("ST_JMBG").Trim());
                            sbOutputTxt.Append(dr.GetInt32("ST_VrstaRecepta").ToString());
                            sbOutputTxt.Append((dr.GetInt32("BRJED") * 100).ToString("D10"));
                            sbOutputTxt.AppendLine(dr.GetDateTime("DatumPropisivanja").ToString("ddMMyyyy"));
                        
                        
                        }

                   }
                    
                }

                return sbOutputTxt.ToString();
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška prilikom kreiranja izlazne datoteke za kzzo.", Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public bool GenerateKzzoRacuni()
        {

            if (Global.Instance.Konfiguracija["Kanton"] == "Sarajevo")
            {

                try
                {
                    using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                    {
                        MySqlCommand command = new MySqlCommand("sp_GenerateRacuniKZZO", con);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandTimeout = 1200;
                        command.ExecuteNonQuery();
                        return false;
                    }
                }
                catch (System.Exception ex)
                {
                    Logging.Log.Create("Greska prilikom generisanja racuna za KZZO.",
                        Logging.LogEntryLevel.Critical, ex);
                }

                return true;
            }
            else
            {
                try
                {
                    using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                    {
                        MySqlCommand command = new MySqlCommand("sp_zdk_GenerateRacuniKZZO", con);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandTimeout = 1200;
                        command.ExecuteNonQuery();
                        return false;
                    }
                }
                catch (System.Exception ex)
                {
                    Logging.Log.Create("Greska prilikom generisanja racuna za ZDK KZZO.",
                        Logging.LogEntryLevel.Critical, ex);
                }

                return true; 
            }
        }

        public void SubmitInventurnaKolicina(int robaID, decimal inventurnaKolicina)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_SubmitInventurnaKolicina", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("pRobaID", MySqlDbType.Int32)).Value = robaID;
                    command.Parameters.Add(new MySqlParameter("pInventurnaKolicina", MySqlDbType.Decimal)).Value = inventurnaKolicina;
                    command.ExecuteNonQuery();
                    return;
                }

            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom azuriranja Inventurne kolicine.",
                       Logging.LogEntryLevel.Critical, ex);
            }
        }

        public void SubmitInventurneKolicine(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return;

            try
            {
                 using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_SubmitInventurnaKolicina", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("pRobaID", MySqlDbType.Int32));
                    command.Parameters.Add(new MySqlParameter("pInventurnaKolicina", MySqlDbType.Decimal));

                    foreach (DataRow dr in dt.Rows)
                    {
                        command.Parameters["pRobaID"].Value = dr["ID"];
                        command.Parameters["pInventurnaKolicina"].Value = dr["InventurnaKolicina"];
                        command.ExecuteNonQuery();
                    }
                    return;
                }
                
            }
            catch(System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom azuriranja Inventurnih kolicina.",
                       Logging.LogEntryLevel.Critical, ex);
            }
        }

        #endregion

        internal List<Proizvodjac> GetProizvodjac()
        {
            return GetObjectAll<Proizvodjac>();
        }

        public int InsertDokumentRemoute(Dokument dok, string sifra)
        {
            try
            { 
                MySqlConnection connection = new MySqlConnection(connectionStringRemoute);
                connection.Open();

                //uzeti zadnji broj dokumenta, id administratora i provjera postojanja exporta
                MySqlCommand command3 = new MySqlCommand("sp_Dokumenti_Zadnji", connection);
                command3.CommandType = CommandType.StoredProcedure;
                
                command3.Parameters.Add(new MySqlParameter("pBrojDoc", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                command3.Parameters.Add(new MySqlParameter("pKreator", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                command3.Parameters.Add(new MySqlParameter("pKomitent", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                command3.Parameters.AddWithValue("mSifra", Global.Instance.BrojPoslovnice);

                command3.ExecuteNonQuery();
                int brojDoc = (int)command3.Parameters["pBrojDoc"].Value;
                int kreatorID = (int)command3.Parameters["pKreator"].Value;
                int komitentID = 0;
                komitentID = (int)command3.Parameters["pKomitent"].Value;

                if (komitentID == null || komitentID == 0)
                    return -1;

                //insert Dokument
                MySqlCommand command = new MySqlCommand("sp_Dokument_Ins", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;

                command.Parameters.AddWithValue("pVrstaDokumenta", 1);
                command.Parameters.AddWithValue("pDatumDokumenta", dok.DatumDokumenta);
                command.Parameters.AddWithValue("pBrojDokumenta", brojDoc+1);
                command.Parameters.AddWithValue("pKomitentID", komitentID);
                command.Parameters.AddWithValue("pBrojRacuna", Global.Instance.BrojPoslovnice + "/" + dok.BrojDokumenta);
                command.Parameters.AddWithValue("pDatumRacuna", dok.DatumRacuna);
                command.Parameters.AddWithValue("pFakturnaVrijednost", dok.FakturnaVrijednost * (-1));
                command.Parameters.AddWithValue("pRabat", dok.Rabat * (-1));
                command.Parameters.AddWithValue("pUlazniPDV", 0);
                command.Parameters.AddWithValue("pNabavnaVrijednost", dok.NabavnaVrijednost*(-1));
                command.Parameters.AddWithValue("pProdajnaVrijednost", dok.ProdajnaVrijednost * (-1));
                command.Parameters.AddWithValue("pUkalkulisaniPDV", dok.UkalkulisaniPDV * (-1));
                command.Parameters.AddWithValue("pKreatorID", kreatorID);
                command.Parameters.AddWithValue("pZatvoreno", dok.Zatvoreno);
                command.Parameters.AddWithValue("pPoslovnaJedinica", sifra);
                command.Parameters.AddWithValue("pBrojTK", 0);

                command.ExecuteNonQuery();
                int idDoc = (int)command.Parameters["pID"].Value;

                //insert DokumentStavke
                MySqlCommand command2 = new MySqlCommand("sp_DokumentStavka_Ins", connection);
                command2.CommandType = System.Data.CommandType.StoredProcedure;
                
                foreach (DokumentStavka dokStav in dok.Stavke)
                {
                    command2.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                    command2.Parameters.AddWithValue("pRobaID", dokStav.RobaID);
                    command2.Parameters.AddWithValue("pDokumentID", idDoc);
                    command2.Parameters.AddWithValue("pPDVStopaID", dokStav.PDVStopaID);
                    command2.Parameters.AddWithValue("pKolicina", dokStav.Kolicina*(-1));
                    command2.Parameters.AddWithValue("pFakturnaCijena", dokStav.FakturnaCijena);
                    command2.Parameters.AddWithValue("pFakturnaVrijednost", dokStav.FakturnaVrijednost*(-1));
                    command2.Parameters.AddWithValue("pRabat", dokStav.Rabat*(-1));
                    command2.Parameters.AddWithValue("pMarza", dokStav.Marza*(-1));
                    command2.Parameters.AddWithValue("pTBR", dokStav.TBR);
                    command2.Parameters.AddWithValue("pPDVStopa", dokStav.PDVStopa);
                    command2.Parameters.AddWithValue("pUkalkulisaniPDV", dokStav.UkalkulisaniPDV*(-1));
                    command2.Parameters.AddWithValue("pMPC", dokStav.MPC);
                    command2.Parameters.AddWithValue("pMPV", dokStav.MPV*(-1));
                    command2.Parameters.AddWithValue("pKreatorID", kreatorID);
                    command2.Parameters.AddWithValue("pUlaz", 0);
                    command2.Parameters.AddWithValue("pZaliha", 0);
                    command2.Parameters.AddWithValue("pPoslovnaJedinica", sifra);

                    command2.ExecuteNonQuery();                  
                    int idStav = (int)command2.Parameters["pID"].Value;
                    command2.Parameters.Clear();
                }
                connection.Close();
                return idDoc;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom inserta dokumenta.",
                       Logging.LogEntryLevel.Critical, ex);
                return 0;
            }
        }

        public static void FillIPadresPoslovnice(DataTable dt)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString_192_168_0_3_Robno);
                MySqlCommand command = new MySqlCommand("sp_IPadresePoslovnica", connection);
                command.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\nDošlo je do greške pri konekciji na server!!!");
            }
        }

        public static bool IPadresaPoslovnice(DataTable dtPoslovnice, string poslovnica)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString_192_168_1_4_Robno);
                MySqlCommand command = new MySqlCommand("sp_IPadresePoslovnicaBySifra", connection);
                command.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                command.Parameters.AddWithValue("pSifra", poslovnica);
                adapter.Fill(dtPoslovnice);
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\nDošlo je do greške pri konekciji na server!!!");
                return false;
            }
        }

        public static void FilldgvCentralniSifarnik(string procedura, DataTable dt, string uslov, string pUslov)
        {
            try
            {
                MySqlConnection connect = new MySqlConnection(connectionString_Centralni_Sifarnik);
                connect.Open();

                MySqlCommand command = new MySqlCommand(procedura, connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(pUslov, uslov);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connect.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Trenutno se ne možete uspostaviti veza sa serverom\n centralnog šifarnika na Bistriku!!!\nMolimo Vas pokušajte ponovo, u slučaju da ne funkcioniše\npozovite servisera 061/482-932");
            }
        }

        public static void IzvrsiProceduru(string procedura, DateTime uslov, string pUslov, DateTime uslov2, string pUslov2)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(pUslov, uslov);
                command.Parameters.AddWithValue(pUslov2, uslov2);

                command.ExecuteNonQuery();
                mySQLconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void IzvrsiProceduru(string procedura, string uslov, string pUslov, string uslov2, string pUslov2)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(pUslov, uslov);
                command.Parameters.AddWithValue(pUslov2, uslov2);

                command.ExecuteNonQuery();
                mySQLconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void IzvrsiProceduru(string procedura, string uslov, string pUslov, int uslov2, string pUslov2)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(pUslov, uslov);
                command.Parameters.AddWithValue(pUslov2, uslov2);

                command.ExecuteNonQuery();
                mySQLconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void IzvrsiProceduru(string procedura, string uslov, string pUslov)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(pUslov, uslov);

                command.ExecuteNonQuery();
                mySQLconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void IzvrsiProceduru(string procedura)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
                command.CommandType = CommandType.StoredProcedure;

                command.ExecuteNonQuery();
                mySQLconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static int IzvrsiProceduruIntString(string procedura)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
               
                command.ExecuteNonQuery();
                int idDoc = (int)command.Parameters["pID"].Value;
                
                mySQLconn.Close();
                return idDoc;
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message);
                return 0;
            }
        }

        public static void FillDataSet(DataTable dt, string procedura)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
                command.CommandType = CommandType.StoredProcedure;

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                mySQLconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void FillDataSetSaUslovom(DataTable dt, string procedura, string uslov, string pUslov)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(pUslov, uslov);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                mySQLconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void DeleteSaParametrom(string procedura, int uslov, string pUslov)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(pUslov, uslov);

                command.ExecuteNonQuery();
                mySQLconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void DeleteSaParametrom(string procedura, string uslov, string pUslov)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(pUslov, uslov);

                command.ExecuteNonQuery();
                mySQLconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void DeleteSaParametrom(string procedura, int uslov, string pUslov, int uslov2, string pUslov2)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(pUslov, uslov);
                command.Parameters.AddWithValue(pUslov2, uslov2);

                command.ExecuteNonQuery();
                mySQLconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void UpisiVerzijuPrograma()
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand("sp_VerzijaPrograma", mySQLconn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("pIP", GlobalVariable.IPadres);
                command.Parameters.AddWithValue("pVerzija", GlobalVariable.verzija);
                command.Parameters.AddWithValue("pPoslovnica", Global.Instance.Konfiguracija["BrojApoteke"].ToString());


                command.ExecuteNonQuery();
                mySQLconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void InsertNivelacija(DateTime datum, int poslovnica, int kreator, int brojTK, int brojKal, int rucna)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand("sp_NivelacijaNova", mySQLconn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("pDatum", datum);
                command.Parameters.AddWithValue("pPoslovnaJedinica", poslovnica);
                command.Parameters.AddWithValue("pKreatorID", kreator);
                command.Parameters.AddWithValue("pBrojTK", brojTK);
                command.Parameters.AddWithValue("pBrojKal", brojKal);
                command.Parameters.AddWithValue("pRucnaNiv", rucna);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void InsertNivelacijaStavka(int poslovnica, int kreator, string nivelacijaID, decimal novaMPC, int RobaID, decimal zaliha, decimal staraMPC, decimal novaMarza, string brojKal, int rucnaNiv, decimal novaFakturna)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand("sp_NivelacijaStavkaNova", mySQLconn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("pNivID", nivelacijaID);
                command.Parameters.AddWithValue("pPoslovnaJedinica", poslovnica);
                command.Parameters.AddWithValue("pKreatorID", kreator);
                command.Parameters.AddWithValue("pNovaMPC", novaMPC);
                command.Parameters.AddWithValue("pRobaID", RobaID);
                command.Parameters.AddWithValue("pZaliha", zaliha);
                command.Parameters.AddWithValue("pStaraMPC", staraMPC);
                command.Parameters.AddWithValue("pMarza", novaMarza);
                command.Parameters.AddWithValue("pBrojKal", brojKal);
                command.Parameters.AddWithValue("pRucnaNiv", rucnaNiv);
                command.Parameters.AddWithValue("pFakturna", novaFakturna);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void RacuniPeriodZaPOS(DateTime datumOd, DateTime datumDo, int nacinPlacanja, DataTable table)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlDataAdapter adapter = new MySqlDataAdapter("sp_BlagajnaNovaRacuniPOS", mySQLconn);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                adapter.SelectCommand.Parameters.AddWithValue("pDatumOd", datumOd);
                adapter.SelectCommand.Parameters.AddWithValue("pDatumDo", datumDo);
                adapter.SelectCommand.Parameters.AddWithValue("pNacinPlacanja", nacinPlacanja);

                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static bool SaveRecepti(string recept, string jmbg, DateTime datum, string tJMBG, int KreID, DateTime datumpropisivanja, DateTime tekucidatumpropisivanja, int ponovljeni, decimal propisana)
        {
            try
            {

                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand("sp_EditReceptJMBG", mySQLconn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("pBrojRecepta", recept);
                command.Parameters.AddWithValue("nJMBG", jmbg);
                command.Parameters.AddWithValue("pDatum", datum);
                command.Parameters.AddWithValue("tJMBG", tJMBG);
                command.Parameters.AddWithValue("pKreatorID", KreID);
                command.Parameters.AddWithValue("pDatumPropisivanja", datumpropisivanja);
                command.Parameters.AddWithValue("tDatumPropisivanja", tekucidatumpropisivanja);
                command.Parameters.AddWithValue("pPonovljeniRec", ponovljeni);
                command.Parameters.AddWithValue("pPropisanaKol", propisana);

                command.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool SaveReceptiZeDo(string tekuciBrojRecepta, string noviBrojRecepta, DateTime datumRacuna, int korisnik)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand("sp_EditReceptZeDo", mySQLconn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("tekuciBrojRecepta", tekuciBrojRecepta);
                command.Parameters.AddWithValue("noviBrojRecepta", noviBrojRecepta);
                command.Parameters.AddWithValue("pDatumRacuna", datumRacuna);
                command.Parameters.AddWithValue("pKreatorID", korisnik);
                command.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static void UpdateBarKod(string robaID, string noviBarKod, string stariBarKod, int modifikator)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand("sp_UpdateBarKod", mySQLconn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("pRobaID", robaID);
                command.Parameters.AddWithValue("noviBarKod", noviBarKod);
                command.Parameters.AddWithValue("stariBarKod", stariBarKod);
                command.Parameters.AddWithValue("pModifikator", modifikator);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\nKonekcija na server je izgubljena!!!");
            }
        }


        public int ProknjiziDokument(Dokument dok)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString_192_168_0_3);
                connection.Open();

                //uzeti zadnji broj dokumenta, id administratora i provjera postojanja exporta
                //MySqlCommand command3 = new MySqlCommand("sp_Dokumenti_Zadnji", connection);
                //command3.CommandType = CommandType.StoredProcedure;

                //command3.Parameters.Add(new MySqlParameter("pBrojDoc", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                //command3.Parameters.Add(new MySqlParameter("pKreator", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                //command3.Parameters.Add(new MySqlParameter("pKomitent", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                //command3.Parameters.AddWithValue("mSifra", Global.Instance.BrojPoslovnice);

                //command3.ExecuteNonQuery();
                //int brojDoc = (int)command3.Parameters["pBrojDoc"].Value;
                //int kreatorID = (int)command3.Parameters["pKreator"].Value;
                //int komitentID = 0;
                //komitentID = (int)command3.Parameters["pKomitent"].Value;

                //if (komitentID == null || komitentID == 0)
                //    return -1;

                //insert Knjizenje
                MySqlCommand command = new MySqlCommand("sp_KnjizenjeKalukacija", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                //command.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;

                command.Parameters.AddWithValue("mDat", dok.DatumDokumenta);
                command.Parameters.AddWithValue("mBro", dok.BrojDokumenta);
                command.Parameters.AddWithValue("mKom", dok.Komitent.Sifra);
                command.Parameters.AddWithValue("mBrr", dok.BrojRacuna);
                command.Parameters.AddWithValue("mDtr", dok.DatumRacuna);
                command.Parameters.AddWithValue("mDtv", dok.DatumRacuna);
                command.Parameters.AddWithValue("mPJ", Global.Instance.BrojPoslovnice);
                command.Parameters.AddWithValue("mIzn", dok.Iznos);
                command.Parameters.AddWithValue("mPdv", dok.UlazniPDV);
                command.Parameters.AddWithValue("uPdv", dok.UkalkulisaniPDV);
                command.Parameters.AddWithValue("mRac", dok.ProdajnaVrijednost);
                command.Parameters.AddWithValue("mOpe", Global.Instance.LoggedKorisnik.KorisnickoIme);
                command.Parameters.AddWithValue("mIP", GlobalVariable.IPadres);
                
                command.ExecuteNonQuery();
                int idDoc = 77;// (int)command.Parameters["pID"].Value;

                //insert DokumentStavke
                //MySqlCommand command2 = new MySqlCommand("sp_DokumentStavka_Ins", connection);
                //command2.CommandType = System.Data.CommandType.StoredProcedure;

                //foreach (DokumentStavka dokStav in dok.Stavke)
                //{
                //    command2.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                //    command2.Parameters.AddWithValue("pRobaID", dokStav.RobaID);
                //    command2.Parameters.AddWithValue("pDokumentID", idDoc);
                //    command2.Parameters.AddWithValue("pPDVStopaID", dokStav.PDVStopaID);
                //    command2.Parameters.AddWithValue("pKolicina", dokStav.Kolicina * (-1));
                //    command2.Parameters.AddWithValue("pFakturnaCijena", dokStav.FakturnaCijena);
                //    command2.Parameters.AddWithValue("pFakturnaVrijednost", dokStav.FakturnaVrijednost * (-1));
                //    command2.Parameters.AddWithValue("pRabat", dokStav.Rabat * (-1));
                //    command2.Parameters.AddWithValue("pMarza", dokStav.Marza * (-1));
                //    command2.Parameters.AddWithValue("pTBR", dokStav.TBR);
                //    command2.Parameters.AddWithValue("pPDVStopa", dokStav.PDVStopa);
                //    command2.Parameters.AddWithValue("pUkalkulisaniPDV", dokStav.UkalkulisaniPDV * (-1));
                //    command2.Parameters.AddWithValue("pMPC", dokStav.MPC);
                //    command2.Parameters.AddWithValue("pMPV", dokStav.MPV * (-1));
                //    command2.Parameters.AddWithValue("pKreatorID", kreatorID);
                //    command2.Parameters.AddWithValue("pUlaz", 0);
                //    command2.Parameters.AddWithValue("pZaliha", 0);
                //    command2.Parameters.AddWithValue("pPoslovnaJedinica", dokStav.PoslovnaJedinica);

                //    command2.ExecuteNonQuery();
                //    int idStav = (int)command2.Parameters["pID"].Value;
                //    command2.Parameters.Clear();
                //}
                connection.Close();
                return idDoc;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom inserta dokumenta.",
                       Logging.LogEntryLevel.Critical, ex);
                return 0;
            }
        }

        public static void FillDataSetVeleprodaja(DataTable dt, string procedura)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString_192_168_0_3);
                connection.Open();

                MySqlCommand command = new MySqlCommand(procedura, connection);
                command.CommandType = CommandType.StoredProcedure;

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public static void VeleprodajaFiskalizuj(int brojFisk, int kasa, decimal iznos, int brRacuna)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString_192_168_0_3);
                connection.Open();

                MySqlCommand command = new MySqlCommand("sp_VeleprodajaFiskalizuj", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("mBrFisk", brojFisk);
                command.Parameters.AddWithValue("mKasa", kasa);
                command.Parameters.AddWithValue("mIznos", iznos);
                command.Parameters.AddWithValue("mRacun", brRacuna);

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public int InsertVeleprodaja(Dokument dok)
        {
            try
            {                
                MySqlConnection connection = new MySqlConnection(connectionString_192_168_0_3);
                connection.Open();

                //insert Dokument into Roba_Dokumenta
                MySqlCommand command = new MySqlCommand("sp_VeleprodajaInsert", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;

                command.Parameters.AddWithValue("pDatum", dok.DatumDokumenta);
                
                command.ExecuteNonQuery();
                int idDoc = (int)command.Parameters["pID"].Value;

                //insert DokumentStavke into Roba_Promet
                MySqlCommand command2 = new MySqlCommand("sp_VeleprodajaStavkeInsert", connection);
                command2.CommandType = System.Data.CommandType.StoredProcedure;
                decimal DOK_VBP = 0;
                foreach (DokumentStavka dokStav in dok.Stavke)
                {
                    DOK_VBP += Math.Round(dokStav.MPC/((dokStav.PDVStopa+100)/100)*dokStav.Kolicina*(-1),2);
                    command2.Parameters.AddWithValue("pDokBroj", idDoc);
                    command2.Parameters.AddWithValue("pDatum", dok.DatumDokumenta);
                    command2.Parameters.AddWithValue("pRobaSifra", dokStav.Roba.Sifra);
                    command2.Parameters.AddWithValue("pKolicina", dokStav.Kolicina * (-1));
                    command2.Parameters.AddWithValue("pMPC", dokStav.MPC);

                    command2.ExecuteNonQuery();
                    command2.Parameters.Clear();
                }

                //update Roba_Dokumenti set DOK_VBP
                MySqlCommand command3 = new MySqlCommand("sp_VeleprodajaUpdateVBP", connection);
                command3.CommandType = System.Data.CommandType.StoredProcedure;
                command3.Parameters.AddWithValue("pID", idDoc);
                command3.Parameters.AddWithValue("pIznos", DOK_VBP);

                command3.ExecuteNonQuery();

                connection.Close();
                return idDoc;
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom inserta dokumenta veleprodaje.",
                       Logging.LogEntryLevel.Critical, ex);
                return 0;
            }
        }

        public static void UpdateNivelacija(int kreator, int brojTK, int id)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand("sp_NivelacijaUpdate", mySQLconn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("pKreatorID", kreator);
                command.Parameters.AddWithValue("pBrojTK", brojTK);
                //command.Parameters.AddWithValue("pBrojKal", brojKal);
                command.Parameters.AddWithValue("pID", id);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void FillDataSetNivelacije(DataTable dt, string procedura, string uslov, string pUslov, int uslov2, string pUslov2)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(pUslov, uslov);
                command.Parameters.AddWithValue(pUslov2, uslov2);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                mySQLconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void KZZOfiskalizacija(int brojFisk, int id)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand("sp_KzzoFiskalizacija", mySQLconn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("pID", id);
                command.Parameters.AddWithValue("pBrojFisk", brojFisk);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static DateTime GetDatumSaServera()
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand("sp_GetDatumSaServera", mySQLconn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("pDatum", MySqlDbType.DateTime)).Direction = System.Data.ParameterDirection.Output;

                command.ExecuteNonQuery();
                mySQLconn.Close();

                DateTime datum = (DateTime)command.Parameters["pDatum"].Value;

                return datum;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

        public static void ProvjeraStatusaPrintera()
        {
            string fileStatus = "";
            if (GlobalVariable.stringFiskalni.Contains(":"))
                fileStatus = GlobalVariable.stringFiskalni + @"Status.ini";
            else
                fileStatus = GlobalVariable.stringFiskalni + @"ELink 2.0\Status.ini";

            string fileNewStatus = fileStatus.Replace(".ini", "_" + GlobalVariable.brKase + ".ini");
            string line;
            string text = "";
            bool greska = false;
            int count = 0;

            try
            {
                if (File.Exists(fileStatus))
                {
                    File.Copy(fileStatus, fileNewStatus, true);
                    StreamReader file = null;
                    file = new StreamReader(fileNewStatus);

                    while ((line = file.ReadLine()) != null)
                    {
                        text += line + "\n";
                        string[] odgovor = line.Split('=');
                        if (count > 0)
                        {
                            if (odgovor[1] == "0")
                            {
                                greska = true;
                                GlobalVariable.statusPOSservera = false;
                                IspisGreskeStatusaPOS(odgovor[0].Trim());
                                //MessageBox.Show("Greška na printeru: " + odgovor[0] + ".", "Greška!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        count++;
                    }
                    file.Close();
                    if (greska == false)
                    {
                        GlobalVariable.statusPOSservera = true;
                        //MessageBox.Show("Status printera je uredu.\n\n" + text + "", "Ispravan!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    GlobalVariable.statusPOSservera = false;
                    MessageBox.Show("Status printera nije moguće dobiti.\nFajl za status printera ne postoji ili\nputanja do fajla nije moguća.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.statusPOSservera = false;
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public static void IspisGreskeStatusaPOS(string poruka)
        {
            try
            {
                switch (poruka)
                {
                    case "Komunikacija":
                        MessageBox.Show("Komunikaciji sa fiskalnim printerom nije uredu.\nProvjerite da li je ELink driver pokrenut i\nuključen POS printer.", "Greška!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                    case "Displej":
                        MessageBox.Show("Display nije priključen sa fiskalnim printerom ili nema napajanja.", "Greška!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                    case "PrinterError":
                        MessageBox.Show("Došlo je do greške na fiskalnom printeru.\nPrinterError", "Greška!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                    case "DatumVrijeme":
                        MessageBox.Show("Datum nije podešen na fiskalnom printeru\nili je baterija prazna.", "Greška!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                    case "PoklopacPisaca":
                        MessageBox.Show("Poklopac za papir je otvoren na fiskalnom printeru.", "Greška!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                    case "Papir":
                        MessageBox.Show("Nema papira u fiskalnom printeru.\nMolimo Vas za zamijenite obije rolne papira", "Greška!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                    case "SDMemorijaPopunjenost":
                        MessageBox.Show("SD Memorija je popunjena na fiskalnom printeru.", "Greška!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                    case "SDMemorijaIspravnost":
                        MessageBox.Show("SD memorija nije ispravna na fiskalnom printeru.", "Greška!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                    case "FMemorijaPopunjenost":
                        MessageBox.Show("Fiskalna memorija je popunjena na fiskalnom printeru.\nPotrebno je zamijeniti fiskalni modul printera.", "Greška!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                    case "FMemorija50":
                        MessageBox.Show("Fiskalna memorija ima manje od 50 dnevnih zaključaka.\nPotrebno je zamijeniti fiskalni modul printera.", "Greška!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                    case "FMemorijaIspravnost":
                        MessageBox.Show("Fiskalna memorija nije ispravna na fiskalnom printeru.", "Greška!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                    default:
                        MessageBox.Show("Status fiskalnog printera nije uredu.\nZavršite račun i naknadno ga fiskalizujte.", "Greška!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static bool KreiranjeLokalnogDirektorija()
        {
            try
            {
                if (Directory.Exists(@"c:\EPSON"))
                    return true;
                else
                {
                    Directory.CreateDirectory(@"c:\EPSON");
                    return true;
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.statusPOSservera = false;
                MessageBox.Show(ex.Message);
                return false;
            }
        }


        public static void FiskalizacijaRacuna(Racun racun, int korisnikID)
        {
            try
            {
                Database apoteke = new Database();
                MySqlConnection mySQLconn = new MySqlConnection();
                mySQLconn = apoteke.OpenConnection();

                MySqlCommand command = new MySqlCommand("sp_FiskalizacijaUpis_Update", mySQLconn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("pID", racun.ID);
                command.Parameters.AddWithValue("pKorisnikID", korisnikID);
                command.Parameters.AddWithValue("pBrojFisk", racun.BrojFisk);
                command.Parameters.AddWithValue("pBrojKase", racun.BrojKase);
                command.Parameters.AddWithValue("pFiskalniIznos", racun.FiskalniIznos);
                command.Parameters.AddWithValue("pFiskaliDatum", racun.FiskalniDatum);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
