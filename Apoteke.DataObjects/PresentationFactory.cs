using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects.BLL;
using System.Data;
using System.Windows.Forms;
using MySql.Data.Types;
using Apoteke.DataObjects.Core;

namespace Apoteke.DataObjects
{
    public class PresentationFactory
    {
        public delegate void ProgressUpdateDelegate(object sender,PresentationEventArgs e);
        public static event ProgressUpdateDelegate ProgressUpdate;

        public delegate void BeginTableCreationDelegate(string tableName);
        public static event BeginTableCreationDelegate BeginTableCreation;
        public static event BeginTableCreationDelegate EndTableCreation;


        static PresentationFactory()
        { }

        public static int GetStep(int rowNumber)
        {
            int firstNumber = 0;
            int intStep = 1;
            double step = 1;

            if (rowNumber > 10)
            {
                int.TryParse(rowNumber.ToString().Substring(0, 1), out firstNumber);
                step = firstNumber * Math.Pow(10, rowNumber.ToString().Length - 2);

                if (rowNumber > 100)
                    step = step / 2;

                int.TryParse(step.ToString(), out intStep);
            }

            return intStep;
        }

        public static DataTable GetTableStavkeDokumenta(List<DokumentStavka> stavke)
        {
            DataTable stavkeDt = CreateTableStavkeDokumenta();

            int i = 0;
            foreach (DokumentStavka stavka in stavke)
            {
                i++;
                AddRecordToStavkeDokumenata(stavkeDt, stavka, i);
                
            }

            return stavkeDt;

        }

        public static void AddRecordToStavkeDokumenata(DataTable stavkeDt, DokumentStavka stavka, int redniBr)
        {
            if (stavka == null)
                return;

            DataRow dr = stavkeDt.NewRow();
            dr["ID"] = stavka.ID;
            dr["RB"] = redniBr;
            dr["Sifra Robe"] = stavka.Roba == null ? String.Empty : stavka.Roba.Sifra;
            dr["Mjera"] = stavka.Roba == null ? String.Empty : stavka.Roba.Mjera == null ? String.Empty : stavka.Roba.Mjera.Naziv;
            dr["Roba"] = stavka.Roba == null ? String.Empty : stavka.Roba.Naziv;
            dr["Dokument"] = stavka.Dokument == null ? 0 : stavka.Dokument.BrojDokumenta;
            dr["Kolicina"] = stavka.Kolicina;
            dr["FakturnaCijena"] = stavka.FakturnaCijena;
            dr["FakturnaVrijednost"] = stavka.FakturnaVrijednost;
            dr["Rabat"] = stavka.Rabat;
            dr["Marza"] = stavka.Marza;
            dr["TBR"] = stavka.TBR;
            dr["PDVStopa"] = stavka.PDVStopa;
            dr["UkalkulisaniPDV"] = stavka.UkalkulisaniPDV;
            dr["MPC"] = stavka.MPC;
            dr["MPV"] = stavka.MPV;
           
            AddBaseFieldsToRecord<DokumentStavka>(stavka, dr);
            stavkeDt.Rows.Add(dr);
        }

        public static DataTable GetTableBarKodovi(List<BarKod> barKodovi)
        {
            if (BeginTableCreation != null)
                BeginTableCreation("Barkodovi");

            DataTable barKodoviDt = CreateTableBarKodovi();

            int step = GetStep(barKodovi.Count);

            int i = 0;
            foreach (BarKod bk in barKodovi)
            {
                i++;
                if (i % step == 0 && ProgressUpdate != null)
                    ProgressUpdate(null, new PresentationEventArgs(barKodovi.Count, i, "Nesto"));

                AddRecordToBarKodovi(barKodoviDt, bk);
            }
            
            if (EndTableCreation != null)
                EndTableCreation("Barkodovi");

            return barKodoviDt;
        }

        public static void AddRecordToBarKodovi(DataTable barKodoviDt, BarKod bk)
        {
            if (bk == null)
                return;

            DataRow dr = barKodoviDt.NewRow();
            dr["ID"] = bk.ID;
            dr["Sifra"] = bk.Roba == null ? String.Empty : bk.Roba.Sifra;
            dr["Roba"] = bk.Roba == null ?String.Empty : bk.Roba.Naziv;
            dr["Proizvodjac"] = bk.Roba == null ? String.Empty : bk.Roba.Proizvodjac == null ? String.Empty :bk.Roba.Proizvodjac.Naziv;
            dr["BarKod"] = bk.Kod;

            AddBaseFieldsToRecord<BarKod>(bk, dr);
            barKodoviDt.Rows.Add(dr);
        }

        public static void EditRecordBarKodovi(DataGridViewRow bkDr, BarKod bk)
        {
            if (bk == null)
                return;

            bkDr.Cells["ID"].Value = bk.ID;
            bkDr.Cells["Sifra"].Value = bk.Roba == null ? String.Empty : bk.Roba.Sifra;
            bkDr.Cells["Roba"].Value = bk.Roba == null ? String.Empty : bk.Roba.Naziv;
            bkDr.Cells["Proizvodjac"].Value = bk.Roba == null ? String.Empty : bk.Roba.Proizvodjac == null ? String.Empty : bk.Roba.Proizvodjac.Naziv;
            bkDr.Cells["BarKod"].Value = bk.Kod;
            EditBaseFields<BarKod>(bkDr, bk);
        }

        public static DataTable GetTableSmjene(List<Smjena> smjene)
        {
            if (BeginTableCreation != null)
                BeginTableCreation("Smjene");

            DataTable smjeneDt = CreateTableSmjene();

            int step = GetStep(smjene.Count);

            int i = 0;
            foreach (Smjena smj in smjene)
            {
                i++;
                if (i % step == 0 && ProgressUpdate != null)
                    ProgressUpdate(null, new PresentationEventArgs(smjene.Count, i, "Nesto"));

                AddRecordToSmjene(smjeneDt, smj);
            }

            if (EndTableCreation != null)
                EndTableCreation("Smjene");

            return smjeneDt;
        }

        public static void AddRecordToSmjene(DataTable smjeneDt, Smjena smj)
        {
            if (smj == null)
                return;

            DataRow dr = smjeneDt.NewRow();
            dr["ID"] = smj.ID;
            dr["VrijemeOtvaranja"] =smj.VrijemeOtvaranja;
            dr["VrijemeZatvaranja"] = smj.VrijemeZatvaranja;
            dr["Broj"] = smj.Broj;    
            dr["OdgovornoLice"] = smj.OdgovornoLice == null ? String.Empty : smj.OdgovornoLice.KorisnickoIme;
            dr["Zatvorena"] = smj.Zatvorena;
            dr["Ukupno"] = smj.Ukupno;

            AddBaseFieldsToRecord<Smjena>(smj, dr);
            smjeneDt.Rows.Add(dr);
        }

        public static void EditRecordSmjene(DataGridViewRow smjDr, Smjena smj)
        {
            if (smj == null)
                return;

            smjDr.Cells["ID"].Value = smj.ID;
            smjDr.Cells["VrijemeOtvaranja"].Value = smj.VrijemeOtvaranja;
            smjDr.Cells["VrijemeZatvaranja"].Value = smj.VrijemeZatvaranja;
            smjDr.Cells["BrojSmjene"].Value = smj.Broj;
            smjDr.Cells["OdgovornoLice"].Value = smj.OdgovornoLice == null ? String.Empty : smj.OdgovornoLice.KorisnickoIme;
            smjDr.Cells["Zatvorena"].Value = smj.Zatvorena;
            smjDr.Cells["Ukupno"].Value = smj.Ukupno;
            EditBaseFields<Smjena>(smjDr, smj);
         }

        public static DataTable GetTableProizvodjaci(List<Proizvodjac> proizvodjaci)
        {
            if (BeginTableCreation != null)
                BeginTableCreation("Proizvođači");

            DataTable proizvodjaciDt = CreateTableProizvodjaci();

            int step = GetStep(proizvodjaci.Count);

            int i = 0;
            foreach ( Proizvodjac pro in proizvodjaci)
            {
                i++;
                if (i % step == 0 && ProgressUpdate != null)
                    ProgressUpdate(null, new PresentationEventArgs(proizvodjaci.Count, i, "Nesto"));
                AddRecordToProizvodjac(proizvodjaciDt, pro);               
            }

            if (EndTableCreation != null)
                EndTableCreation("Proizvođači");

            return proizvodjaciDt;
        }

        public static DataTable GetTableKorisnici(List<Korisnik> korisnici)
        {
            if (BeginTableCreation != null)
                BeginTableCreation("Korisnici");

            DataTable korisniciDt = CreateTableKorisnici();

            int step = GetStep(korisnici.Count);

            int i = 0;
            foreach (Korisnik korisnik in korisnici)
            {
                i++;
                if (i % step == 0 && ProgressUpdate != null)
                    ProgressUpdate(null, new PresentationEventArgs(korisnici.Count, i, "Nesto"));

                AddRecordToKorisnik(korisniciDt, korisnik);
            }

            if (EndTableCreation != null)
                EndTableCreation("Korisnici");

            return korisniciDt;
            
        }

        private static void AddBaseFieldsToRecord<T>(ApotekaBase<T> obj, DataRow dr)
        {

            dr["Kreator"] = obj.Kreator == null ? String.Empty : obj.Kreator.KorisnickoIme;
            dr["Modifikator"] = obj.Modifikator == null ? String.Empty :  obj.Modifikator.KorisnickoIme;
            dr["VrijemeKreiranja"] = obj.VrijemeKreiranja;
            if (obj.VrijemeModificiranja.Year != 1)
                dr["VrijemeModifikacije"] = obj.VrijemeModificiranja;

        }

        public static void AddRecordToDokument(DataTable dokumentiDt, Dokument doc)
        {
            if (doc == null)
                return;
            DataRow dr = dokumentiDt.NewRow();
            dr["ID"] = doc.ID;
            dr["VrstaDokumenta"] = doc.VrstaDokumenta.ToString();
            dr["DatumDokumenta"] = doc.DatumDokumenta;
            dr["BrojDokumenta"] = doc.BrojDokumenta;
            dr["DatumRacuna"] = doc.DatumRacuna;
            dr["BrojRacuna"] = doc.BrojRacuna;
            dr["Komitent"] = doc.Komitent == null ? String.Empty : doc.Komitent.Naziv;
            dr["SifraKomitenta"] = doc.Komitent == null ? String.Empty : doc.Komitent.Sifra;
            dr["FakturnaVrijednost"] = doc.FakturnaVrijednost;
            dr["Rabat"] = doc.Rabat;
            dr["UlazniPDV"] = doc.UlazniPDV;

            dr["NabavnaVrijednost"] = doc.NabavnaVrijednost;
            dr["Marza"] = doc.Marza;
            dr["UkalkulisaniPDV"] = doc.UkalkulisaniPDV;
            dr["ProdajnaVrijednost"] = doc.ProdajnaVrijednost;
            dr["IznosRacuna"] = doc.FakturnaVrijednost - doc.Rabat + doc.UlazniPDV;
            dr["Zatvoreno"] = doc.Zatvoreno;

            AddBaseFieldsToRecord<Dokument>(doc, dr);
            dokumentiDt.Rows.Add(dr);
        }

        public static void AddRecordToKorisnik(DataTable korisniciDt, Korisnik korisnik)
        {
            if (korisnik == null)
                return;

            DataRow dr = korisniciDt.NewRow();
            dr["ID"] = korisnik.ID;
            dr["Ime"] = korisnik.Ime;
            dr["Prezime"] = korisnik.Prezime;
            dr["KorisnickoIme"] = korisnik.KorisnickoIme;
            dr["Lozinka"] = korisnik.Lozinka;
            dr["Email"] = korisnik.EMail;
            dr["Titula"] = korisnik.Titula == null ? String.Empty : korisnik.Titula.Naziv;
            dr["Admin"] = korisnik.IsAdmin;

            AddBaseFieldsToRecord<Korisnik>(korisnik, dr);
            korisniciDt.Rows.Add(dr);
        }

        public static void EditRecordKorisnik(DataGridViewRow korisnikDr, Korisnik korisnik)
        {
            if (korisnik == null)
                return;

            korisnikDr.Cells["ID"].Value = korisnik.ID;
            korisnikDr.Cells["Ime"].Value = korisnik.Ime;
            korisnikDr.Cells["Prezime"].Value = korisnik.Prezime;
            korisnikDr.Cells["KorisnickoIme"].Value = korisnik.KorisnickoIme;
            korisnikDr.Cells["Lozinka"].Value = korisnik.Lozinka;
            korisnikDr.Cells["Email"].Value = korisnik.EMail;
            korisnikDr.Cells["Titula"].Value = korisnik.Titula == null ? String.Empty : korisnik.Titula.Naziv;
            korisnikDr.Cells["Admin"].Value = korisnik.IsAdmin;
            EditBaseFields<Korisnik>(korisnikDr, korisnik);
        }

        public static void EditRecordDocument(DataGridViewRow dokumentDr, Dokument doc)
        {
            if (doc == null)
                return;

            dokumentDr.Cells["ID"].Value = doc.ID;
            dokumentDr.Cells["VrstaDokumenta"].Value = doc.VrstaDokumenta.ToString();
            dokumentDr.Cells["DatumDokumenta"].Value = doc.DatumDokumenta;
            dokumentDr.Cells["BrojDokumenta"].Value = doc.BrojDokumenta;
            dokumentDr.Cells["DatumRacuna"].Value = doc.DatumRacuna;
            dokumentDr.Cells["BrojRacuna"].Value = doc.BrojRacuna;
            dokumentDr.Cells["Komitent"].Value = doc.Komitent == null ? String.Empty: doc.Komitent.Naziv;
            dokumentDr.Cells["SifraDobavljaca"].Value = doc.Komitent == null ? String.Empty : doc.Komitent.Sifra;
            dokumentDr.Cells["FakturnaVrijednost"].Value = doc.FakturnaVrijednost;
            dokumentDr.Cells["Rabat"].Value = doc.Rabat;
            dokumentDr.Cells["UlazniPDV"].Value = doc.UlazniPDV;

            dokumentDr.Cells["NabavnaVrijednost"].Value = doc.NabavnaVrijednost;
            dokumentDr.Cells["Marza"].Value = doc.Marza;
            dokumentDr.Cells["UkalkulisaniPDV"].Value = doc.UkalkulisaniPDV;
            dokumentDr.Cells["ProdajnaVrijednost"].Value = doc.ProdajnaVrijednost;
            dokumentDr.Cells["IznosRacuna"].Value = doc.FakturnaVrijednost - doc.Rabat + doc.UlazniPDV;
            dokumentDr.Cells["Zatvoreno"].Value = doc.Zatvoreno;
            EditBaseFields<Dokument>(dokumentDr, doc);
        }

        private static void EditBaseFields<T>(DataGridViewRow dgViewRow, ApotekaBase<T> obj)
        {
            dgViewRow.Cells["Modifikator"].Value =obj.Modifikator == null ? String.Empty: obj.Modifikator.KorisnickoIme;
            dgViewRow.Cells["Kreator"].Value = obj.Kreator == null ? String.Empty : obj.Kreator.KorisnickoIme;
            dgViewRow.Cells["VrijemeKreiranja"].Value = obj.VrijemeKreiranja;
            dgViewRow.Cells["VrijemeModifikacije"].Value = obj.VrijemeModificiranja;
        }

        public static DataTable GetTableLjekari(List<Ljekar> ljekari)
        {
            if (BeginTableCreation != null)
                BeginTableCreation("Ljekari");

            DataTable ljekariDt = CreateTableLjekari();

            int step = GetStep(ljekari.Count);

            int i = 0;
            foreach (Ljekar ljekar in ljekari)
            {
                i++;
                if (i % step == 0 && ProgressUpdate != null)
                    ProgressUpdate(null, new PresentationEventArgs(ljekari.Count, i, "Nesto"));

                AddRecordToLjekar(ljekariDt, ljekar);
            }

            if (EndTableCreation != null)
                EndTableCreation("Ljekari");

            return ljekariDt;
        }

        public static void AddRecordToLjekar(DataTable ljekariDt, Ljekar ljekar)
        {
            if (ljekar == null)
                return;

            DataRow dr = ljekariDt.NewRow();
            dr["ID"] = ljekar.ID;
            dr["Ime"] = ljekar.Ime;
            dr["Sifra"] = ljekar.Sifra;
            dr["Aktivan"] = ljekar.Aktivan;

            AddBaseFieldsToRecord<Ljekar>(ljekar, dr);
            ljekariDt.Rows.Add(dr);
        }

        public static void EditRecordLjekar(DataGridViewRow ljekarDr, Ljekar ljekar)
        {
            if (ljekar == null)
                return;

            ljekarDr.Cells["ID"].Value = ljekar.ID;
            ljekarDr.Cells["Ime"].Value = ljekar.Ime;
            ljekarDr.Cells["Sifra"].Value = ljekar.Sifra;
            ljekarDr.Cells["Aktivan"].Value = ljekar.Aktivan;
            EditBaseFields<Ljekar>(ljekarDr, ljekar);
       }

        public static DataTable GetTableKomitenti(List<Komitent> komitenti)
        {
            if (BeginTableCreation != null)
                BeginTableCreation("Komitenti");

            DataTable komitentiDt = CreateTableKomitenti();

            int step = GetStep(komitenti.Count);

            int i = 0;
            foreach (Komitent komitent in komitenti)
            {
                i++;
                if (i % step == 0 && ProgressUpdate != null)
                    ProgressUpdate(null, new PresentationEventArgs(komitenti.Count, i, "Nesto"));

                AddRecordToKomitent(komitentiDt, komitent);
            }

            if (EndTableCreation != null)
                EndTableCreation("Komitenti");

            return komitentiDt;
        }

        public static void AddRecordToKomitent(DataTable komitentiDt, Komitent komitent)
        {
            if (komitent == null)
                return;

            DataRow dr = komitentiDt.NewRow();
            dr["ID"] = komitent.ID;
            dr["Sifra"] = komitent.Sifra;
            dr["Naziv"] = komitent.Naziv;
            dr["Mjesto"] = komitent.Mjesto;
            dr["Adresa"] = komitent.Adresa;
            dr["Racun"] = komitent.Racun;
            dr["PDV"] = komitent.PDV;
            dr["IDB"] = komitent.IDB;

            AddBaseFieldsToRecord<Komitent>(komitent, dr);
            komitentiDt.Rows.Add(dr);
        }

        public static void EditRecordKomitent(DataGridViewRow komitentDr, Komitent komitent)
        {
            if ( komitent == null)
                return;

            komitentDr.Cells["ID"].Value = komitent.ID;
            komitentDr.Cells["Sifra"].Value = komitent.Sifra;
            komitentDr.Cells["Naziv"].Value = komitent.Naziv;
            komitentDr.Cells["Mjesto"].Value = komitent.Mjesto;
            komitentDr.Cells["Adresa"].Value = komitent.Adresa;
            komitentDr.Cells["Racun"].Value = komitent.Racun;
            komitentDr.Cells["PDV"].Value = komitent.PDV;
            komitentDr.Cells["IDB"].Value = komitent.IDB;
            EditBaseFields<Komitent>(komitentDr, komitent);
        }

        public static DataTable GetTableMjere(List<Mjera> mjere)
        {
            if (BeginTableCreation != null)
                BeginTableCreation("Mjere");

            DataTable mjereDt = CreateTableMjere();

            int step = GetStep(mjere.Count);

            int i = 0;
            foreach (Mjera mjera in mjere)
            {
                i++;
                if (i % step == 0 && ProgressUpdate != null)
                    ProgressUpdate(null, new PresentationEventArgs(mjere.Count, i, "Nesto"));
                AddRecordToMjera(mjereDt, mjera);               
            }

            if (EndTableCreation != null)
                EndTableCreation("Mjere");

            return mjereDt;
        }

        public static void AddRecordToMjera(DataTable mjereDt, Mjera mjera)
        {
            if (mjera == null)
                return;

            DataRow dr = mjereDt.NewRow();
            dr["ID"] = mjera.ID;
            dr["Naziv"] = mjera.Naziv;

            AddBaseFieldsToRecord<Mjera>(mjera, dr);
            mjereDt.Rows.Add(dr);
        }

        public static void EditRecordMjera(DataGridViewRow mjeraDr, Mjera mjera)
        {
            if (mjera == null)
                return;

            mjeraDr.Cells["ID"].Value = mjera.ID;
            mjeraDr.Cells["Naziv"].Value = mjera.Naziv;
            EditBaseFields<Mjera>(mjeraDr, mjera);
        }

        public static DataTable GetTablePDVStope(List<PDVStopa> stope)
        {
            if (BeginTableCreation != null)
                BeginTableCreation("PDVStope");

            DataTable pdvStopeDt = CreateTablePDVStope();

            int step = GetStep(stope.Count);

            int i = 0;
            foreach (PDVStopa stopa in stope)
            {
                i++;
                if (i % step == 0 && ProgressUpdate != null)
                    ProgressUpdate(null, new PresentationEventArgs(stope.Count, i, "Nesto"));

                AddRecordToPDVStope(pdvStopeDt, stopa);
            }

            if (EndTableCreation != null)
                EndTableCreation("PDVStope");

            return pdvStopeDt;
        }

        public static void AddRecordToPDVStope(DataTable pdvStopeDt, PDVStopa stopa)
        {
            if (stopa == null)
                return;

            DataRow dr = pdvStopeDt.NewRow();
            dr["ID"] = stopa.ID;
            dr["Naziv"] = stopa.Naziv;
            dr["Opis"] = stopa.Opis;
            dr["TBR"] = stopa.TBR;
            dr["UplatniRacun"] = stopa.UplatniRacun;
            dr["Stopa"] = stopa.Stopa;

            AddBaseFieldsToRecord<PDVStopa>(stopa, dr);
            pdvStopeDt.Rows.Add(dr);
        }

        public static DataTable GetTableRoba(List<Roba> roba)
        {
            if (BeginTableCreation != null)
                BeginTableCreation("Prikazivanje robe...");

            DataTable robaDt = CreateTableRoba();

            int step = GetStep(roba.Count);

            int i = 0;
            foreach (Roba singleRoba in roba)
            {
                i++;
                if (i % step == 0 && ProgressUpdate != null)
                    ProgressUpdate(null, new PresentationEventArgs(roba.Count, i, "Nesto"));

                AddRecordToRoba(robaDt, singleRoba);
            }

            if (EndTableCreation != null)
                EndTableCreation(String.Format("Učitano je {0} artikala.",i.ToString()));

            return robaDt;
        }
        public static DataTable GetTableNormativi(List<Normativ> normativi)
        {
            return GetTableNormativi(normativi, Decimal.One);
        }

        public static DataTable GetTableNormativi(List<Normativ> normativi, decimal kolicina)
        {
            if (BeginTableCreation != null)
                BeginTableCreation("Normativi");

            DataTable normativiDt = CreateTableNormativi();

            int step = GetStep(normativi.Count);

            int i = 0;
            foreach (Normativ normativ in normativi)
            {
                i++;
                if (i % step == 0 && ProgressUpdate != null)
                    ProgressUpdate(null, new PresentationEventArgs(normativi.Count, i, "Nesto"));

                AddrecordToNormativ(normativiDt, normativ, kolicina);
            }

            if (EndTableCreation != null)
                EndTableCreation("Normativi");

                return normativiDt;
        }

        public static void AddrecordToNormativ(DataTable normativiDt, Normativ normativ)
        {
            AddrecordToNormativ(normativiDt, normativ, Decimal.One);
        }

        public static void AddrecordToNormativ(DataTable normativiDt, Normativ normativ, decimal kolicina)
        {
            if (normativ == null)
                return;
            try
            {
                DataRow dr = normativiDt.NewRow();
                dr["ID"] = normativ.ID;
                dr["RobaID"] = normativ.RobaID;
                dr["MaterijalID"] = normativ.MaterijalRobaID;

                dr["SifraMaterijala"] = normativ.MaterijalRoba == null ? String.Empty : normativ.MaterijalRoba.Sifra;
                dr["NazivMaterijala"] = normativ.MaterijalRoba == null ? String.Empty : normativ.MaterijalRoba.Naziv;
                dr["SifraRobe"] = normativ.Roba == null ? String.Empty : normativ.Roba.Sifra;
                dr["NazivRobe"] = normativ.Roba == null ? String.Empty : normativ.Roba.Naziv;
                
                dr["Normativ"] = normativ.NormativVrijednost;
                dr["Utrosak"] = normativ.NormativVrijednost * kolicina;
                if ( !normativ.Taksa )
                    dr["Iznos"] = Math.Round(normativ.NormativVrijednost * kolicina * normativ.MaterijalRoba.MPC, 4); 
                else
                    dr["Iznos"] = Math.Round(normativ.NormativVrijednost * kolicina , 4); 

                dr["MaterijalZaliha"] = normativ.MaterijalRoba.Zaliha;
                dr["Taksa"] = normativ.Taksa;
                dr["MPC"] = normativ.MaterijalRoba == null ? Decimal.Zero : normativ.MaterijalRoba.MPC;

                AddBaseFieldsToRecord<Normativ>(normativ, dr);
                normativiDt.Rows.Add(dr);
            }
            catch
            {
            }
        }
        public static DataTable GetTableProskripcije(List<Proskripcija> proskripcije)
        {
            if (BeginTableCreation != null)
                BeginTableCreation("Proskripcije");

            DataTable prosDt = CreateTableProskripcije();

            int step = GetStep(proskripcije.Count);

            int i = 0;
            foreach (Proskripcija proskripcija in proskripcije)
            {
                i++;
                if (i % step == 0 && ProgressUpdate != null)
                    ProgressUpdate(null, new PresentationEventArgs(proskripcije.Count, i, "Nesto"));

                AddrecordToProskripcija(prosDt, proskripcija);
            }

            if (EndTableCreation != null)
                EndTableCreation("Proskripcije");

            return prosDt;
        }

        public static void AddrecordToProskripcija(DataTable prosDt, Proskripcija pros)
        {
            if (pros == null)
                return;

            DataRow dr = prosDt.NewRow();
            dr["ID"] = pros.ID;
            dr["Sifra"] = pros.Sifra;
            dr["Naziv"] = pros.Naziv;
            dr["Vrsta"] = pros.Vrsta == null ? String.Empty : pros.Vrsta.ToString();

            AddBaseFieldsToRecord<Proskripcija>(pros, dr);
            prosDt.Rows.Add(dr);
        }

        public static void AddRecordToProizvodjac(DataTable proDt, Proizvodjac pro)
        {
            if (pro == null)
                return;

            DataRow dr = proDt.NewRow();
            dr["ID"] = pro.ID;
            dr["Sifra"] = pro.Sifra;
            dr["Naziv"] = pro.Naziv;
            dr["Telefon"] = pro.Telefon;
            dr["Adresa"] = pro.Adresa;
            dr["Email"] = pro.Email;
            dr["Grad"] = pro.Grad;
            dr["PTT"] = pro.PTT;

            AddBaseFieldsToRecord<Proizvodjac>(pro, dr);
            proDt.Rows.Add(dr);
        }

        public static void AddRecordToRoba(DataTable robaDt, Roba roba)
        {
            if (roba == null)
                return;

            DataRow dr = robaDt.NewRow();
            dr["ID"] = roba.ID;
            dr["Sifra"] = roba.Sifra;
            dr["Naziv"] = roba.Naziv;
            dr["Proizvodjac"] = roba.Proizvodjac == null ? String.Empty : roba.Proizvodjac.Naziv;
            dr["FakturnaCijena"] = roba.FakturnaCijena;
            dr["PDVStopa"] = roba.PDVStopa == null ? Decimal.Zero : roba.PDVStopa.Stopa;
            dr["Opis"] = roba.Opis;
            dr["Oznaka"] = roba.Oznaka;
            dr["Participacija"] = roba.Participacija;
            dr["ReferalnaCijena"] = roba.ReferalnaCijena;
            dr["ATC"] = roba.ATC;
            dr["Ulaz"] = roba.Ulaz;
            dr["Izlaz"] = roba.Izlaz;
            dr["Zaliha"] = roba.Zaliha;
            dr["PocetnoStanje"] = roba.PocetnoStanje;
            //dr["VrstaProskripcije"] = roba.VrstaProskripcije == null ? String.Empty : roba.VrstaProskripcije.ToString();
            dr["MPC"] = roba.MPC;
            dr["Mjera"] = roba.Mjera == null ? String.Empty : roba.Mjera.Naziv;
            dr["Taksa"] = roba.Taksa;

            AddBaseFieldsToRecord<Roba>(roba, dr);
            robaDt.Rows.Add(dr);
        }

        public static void EditRecordRoba(DataGridViewRow robaDr, Roba roba)
        {
            if (roba == null)
                return;

            robaDr.Cells["ID"].Value = roba.ID;
            robaDr.Cells["Sifra"].Value = roba.Sifra;
            robaDr.Cells["Naziv"].Value = roba.Naziv;
            robaDr.Cells["Proizvodjac"].Value = roba.Proizvodjac == null ? String.Empty : roba.Proizvodjac.Naziv;
            robaDr.Cells["FakturnaCijena"].Value = roba.FakturnaCijena;
            robaDr.Cells["PDVStopa"].Value = roba.PDVStopa == null ? Decimal.Zero : roba.PDVStopa.Stopa; 
            robaDr.Cells["Opis"].Value = roba.Opis;
            robaDr.Cells["Oznaka"].Value = roba.Oznaka;
            robaDr.Cells["Participacija"].Value = roba.Participacija;
            robaDr.Cells["ReferalnaCijena"].Value = roba.ReferalnaCijena;
            robaDr.Cells["ATC"].Value = roba.ATC;
            robaDr.Cells["Ulaz"].Value = roba.Ulaz;
            robaDr.Cells["Izlaz"].Value = roba.Izlaz;
            robaDr.Cells["Zaliha"].Value = roba.Zaliha;
            robaDr.Cells["PocetnoStanje"].Value = roba.PocetnoStanje;
            //robaDr.Cells["VrstaProskripcije"].Value = roba.VrstaProskripcije == null ? String.Empty : roba.VrstaProskripcije.ToString();
            robaDr.Cells["MPC"].Value = roba.MPC;
            robaDr.Cells["Mjera"].Value = roba.Mjera == null ? String.Empty : roba.Mjera.Naziv;
            robaDr.Cells["Taksa"].Value = roba.Taksa;

            EditBaseFields<Roba>(robaDr, roba);
       }
        public static void EditRecordProskripcija(DataGridViewRow prosDr, Proskripcija pros)
        {
            if (pros == null)
                return;

            prosDr.Cells["ID"].Value = pros.ID;
            prosDr.Cells["Sifra"].Value = pros.Sifra;
            prosDr.Cells["Naziv"].Value = pros.Naziv;
            prosDr.Cells["Vrsta"].Value = pros.Vrsta == null ? String.Empty : pros.Vrsta.ToString();
            EditBaseFields<Proskripcija>(prosDr, pros);
        }

        public static void EditRecordNormativ(DataGridViewRow normativDr, Normativ normativ)
        {
            EditRecordNormativ(normativDr, normativ, Decimal.One);
        }
        public static void EditRecordNormativ(DataGridViewRow normativDr, Normativ normativ, decimal kolicina)
        {
            if (normativ == null)
                return;

            normativDr.Cells["ID"].Value = normativ.ID;
            normativDr.Cells["RobaID"].Value = normativ.RobaID;
            normativDr.Cells["MaterijalID"].Value = normativ.MaterijalRobaID;
            
            normativDr.Cells["SifraRobe"].Value = normativ.Roba == null ? String.Empty : normativ.Roba.Sifra;
            normativDr.Cells["NazivRobe"].Value = normativ.Roba == null ? String.Empty : normativ.Roba.Naziv;
            normativDr.Cells["SifraMaterijala"].Value = normativ.MaterijalRoba == null ? String.Empty : normativ.MaterijalRoba.Sifra;
            normativDr.Cells["NazivMaterijala"].Value = normativ.MaterijalRoba == null ? String.Empty : normativ.MaterijalRoba.Naziv;
                        
            normativDr.Cells["Normativ"].Value = normativ.NormativVrijednost;
            normativDr.Cells["Utrosak"].Value = normativ.NormativVrijednost * kolicina;
            
            if (!normativ.Taksa)
                normativDr.Cells["Iznos"].Value = Math.Round(normativ.NormativVrijednost * kolicina * normativ.MaterijalRoba.MPC, 4);
            else
                normativDr.Cells["Iznos"].Value = Math.Round(normativ.NormativVrijednost * kolicina, 4); 

            normativDr.Cells["MaterijalZaliha"].Value = normativ.MaterijalRoba.Zaliha;
            normativDr.Cells["Taksa"].Value = normativ.Taksa;
            normativDr.Cells["MPC"].Value = normativ.MaterijalRoba == null ? Decimal.Zero : normativ.MaterijalRoba.MPC;

            EditBaseFields<Normativ>(normativDr, normativ);
        }

        public static void EditRecordProizvodjac(DataGridViewRow prDr, Proizvodjac pro)
        {
            if (pro == null)
                return;

            prDr.Cells["ID"].Value = pro.ID;
            prDr.Cells["Sifra"].Value = pro.Sifra;
            prDr.Cells["Naziv"].Value = pro.Naziv;
            prDr.Cells["Telefon"].Value = pro.Telefon;
            prDr.Cells["Adresa"].Value = pro.Adresa;
            prDr.Cells["Email"].Value = pro.Email;
            prDr.Cells["Grad"].Value = pro.Grad;
            prDr.Cells["PTT"].Value = pro.PTT;
            EditBaseFields<Proizvodjac>(prDr, pro);
        }

        public static DataTable GetTableDokumenti(List<Dokument> dokumenti)
        {
            if (BeginTableCreation != null)
                BeginTableCreation("Dokumenti");

            DataTable dokumentiDt = CreateTableDokumenti();

            int step = GetStep(dokumenti.Count);

            int i = 0;
            foreach (Dokument doc in dokumenti)
            {
                i++;
                if (i % step == 0 && ProgressUpdate != null)
                    ProgressUpdate(null, new PresentationEventArgs(dokumenti.Count, i, "Nesto"));
                AddRecordToDokument(dokumentiDt, doc);
            }

            if (EndTableCreation != null)
                EndTableCreation("Dokumenti");

            return dokumentiDt;
        }

        public static DataTable GetTableTipoviRobe(List<TipRobe> tipovi_robe)
        {
            if (BeginTableCreation != null)
                BeginTableCreation("TipoviRobe");

            DataTable tipoviRobeDt = CreateTableTipoviRobe();

            int step = GetStep(tipovi_robe.Count);

            int i = 0;
            foreach (TipRobe tipRobe in tipovi_robe)
            {
                i++;
                if (i % step == 0 && ProgressUpdate != null)
                    ProgressUpdate(null, new PresentationEventArgs(tipovi_robe.Count, i, "Nesto"));

                AddRecordToTipoviRobe(tipoviRobeDt, tipRobe);
            }
            if (EndTableCreation != null)
                EndTableCreation("TipoviRobe");

            return tipoviRobeDt;
        }


        public static DataTable GetTableNalozi(List<MagistralaNalog> nalozi)
        {
            if (BeginTableCreation != null)
                BeginTableCreation("Nalozi");

            DataTable naloziDt = CreateTableNalozi();

            int step = GetStep(nalozi.Count);

            int i = 0;
            foreach (MagistralaNalog nalog in nalozi)
            {
                i++;
                if (i % step == 0 && ProgressUpdate != null)
                    ProgressUpdate(null, new PresentationEventArgs(nalozi.Count, i, "Nesto"));

                AddRecordToNalozi(naloziDt, nalog);
            }
            if (EndTableCreation != null)
                EndTableCreation("Nalozi");

            return naloziDt;
        }


        public static void AddRecordToTipoviRobe(DataTable tipoviRobeDt, TipRobe tipRobe)
        {
            if (tipRobe == null)
                return;

            DataRow dr = tipoviRobeDt.NewRow();
            dr["ID"] = tipRobe.ID;
            dr["Sifra"] = tipRobe.Sifra;
            dr["Naziv"] = tipRobe.Naziv;

            AddBaseFieldsToRecord<TipRobe>(tipRobe, dr);
            tipoviRobeDt.Rows.Add(dr);
        }

        public static void EditRecordNalog(DataGridViewRow nalogDr, MagistralaNalog nalog)
        {
            if (nalog == null)
                return;

            nalogDr.Cells["ID"].Value = nalog.ID;
            nalogDr.Cells["Roba"].Value = nalog.Roba == null ? String.Empty : nalog.Roba.Naziv;
            nalogDr.Cells["RobaID"].Value = nalog.RobaID;
            nalogDr.Cells["SifraRobe"].Value = nalog.Roba == null ? String.Empty : nalog.Roba.Sifra;
            nalogDr.Cells["Datum"].Value = nalog.Datum;
            nalogDr.Cells["Kolicina"].Value = nalog.Kolicina;
            nalogDr.Cells["VrijednostSastojaka"].Value = nalog.VrijednostSastojaka;
            nalogDr.Cells["IznosTaksi"].Value = nalog.IznosTaksi;
            nalogDr.Cells["Cijena"].Value = nalog.Cijena;
            nalogDr.Cells["Iznos"].Value = nalog.Iznos;
            nalogDr.Cells["Odstupanje"].Value = nalog.Odstupanje;
            nalogDr.Cells["PoslovnaJedinica"].Value = nalog.PoslovnaJedinica;
            nalogDr.Cells["Storniran"].Value = nalog.Storniran;

            EditBaseFields<MagistralaNalog>(nalogDr, nalog);
        }

        public static void AddRecordToNalozi(DataTable naloziDt, MagistralaNalog nalog)
        {
            if (nalog == null)
                return;

            DataRow dr = naloziDt.NewRow();
            dr["ID"] = nalog.ID;
            dr["Roba"] = nalog.Roba == null ? String.Empty : nalog.Roba.Naziv;
            dr["RobaID"] = nalog.RobaID;
            dr["SifraRobe"] = nalog.Roba == null ? String.Empty : nalog.Roba.Sifra;
            dr["Datum"] = nalog.Datum;
            dr["Kolicina"] = nalog.Kolicina;
            dr["VrijednostSastojaka"] = nalog.VrijednostSastojaka;
            dr["IznosTaksi"] = nalog.IznosTaksi;
            dr["Cijena"] = nalog.Cijena;
            dr["Iznos"] = nalog.Iznos;
            dr["Odstupanje"] = nalog.Odstupanje;
            dr["PoslovnaJedinica"] = nalog.PoslovnaJedinica;
            dr["Storniran"] = nalog.Storniran;

            AddBaseFieldsToRecord<MagistralaNalog>(nalog, dr);
            naloziDt.Rows.Add(dr);
        }

        public static void EditRecordTipoviRobe(DataGridViewRow trDr, TipRobe tipRobe)
        {
            if (tipRobe == null)
                return;

            trDr.Cells["ID"].Value = tipRobe.ID;
            trDr.Cells["Sifra"].Value = tipRobe.Sifra;
            trDr.Cells["Naziv"].Value = tipRobe.Naziv;
            EditBaseFields<TipRobe>(trDr, tipRobe);
        }

        public static DataTable GetTableRacuni(List<Racun> racuni)
        {
            if (BeginTableCreation != null)
                BeginTableCreation("Prikazivanje racuna...");

            DataTable racuniDt = CreateTableRacuni();
            int step = GetStep(racuni.Count);
      
            int i = 0;
            foreach (Racun racun in racuni)
            {
                i++;
                if (i % step == 0 && ProgressUpdate != null)
                    ProgressUpdate(null, new PresentationEventArgs(racuni.Count, i, "Nesto"));
                AddRecordToRacun(racuniDt, racun);
            }
          
            if (EndTableCreation != null)
                EndTableCreation(String.Format("Broj učitanih računa je {0}.",i.ToString()));
           
            return racuniDt;
        }

        public static void AddRecordToRacun(DataTable racuniDt, Racun racun)
        {
            if (racun == null)
                return;

            DataRow dr = racuniDt.NewRow();
            dr["ID"] = racun.ID;
            dr["Datum"] = racun.Datum;
            dr["Vrijednost"] = racun.Vrijednost;
            dr["Iznos"] = racun.Iznos + racun.Zaokruzenje - racun.IznosPopusta + racun.DoplataSaPDV; //222222222222
            dr["Isprintan"] = racun.Isprintan;
            dr["Storniran"] = racun.Storniran;
            dr["NacinPlacanja"] = racun.NacinPlacanja.ToString();
            dr["Komitent"] = racun.KomitentID == 0 ? String.Empty : racun.Komitent.Naziv;
            dr["Smjena"] = racun.Smjena == null ? 0 : racun.Smjena.Broj;
            //jasenko tring
            dr["BrojFisk"] = racun.BrojFisk;
            dr["BrojKase"] = racun.BrojKase;
            dr["FiskalniIznos"] = racun.FiskalniIznos;
            dr["FiskalniDatum"] = racun.FiskalniDatum;
            
            AddBaseFieldsToRecord<Racun>(racun, dr);
            racuniDt.Rows.Add(dr);
        }

        public static void EditRecordRacun(DataGridViewRow racunDr, Racun racun)
        {
            if (racun == null)
                return;

            racunDr.Cells["ID"].Value = racun.ID;
            racunDr.Cells["Datum"].Value = racun.Datum;
            racunDr.Cells["Vrijednost"].Value = racun.Vrijednost;
            racunDr.Cells["Iznos"].Value = racun.Iznos + racun.Zaokruzenje - racun.IznosPopusta+racun.DoplataSaPDV ;
            racunDr.Cells["NacinPlacanja"].Value = racun.NacinPlacanja.ToString();
            racunDr.Cells["Smjena"].Value = racun.Smjena == null ? 0 :racun.Smjena.Broj;
            racunDr.Cells["Isprintan"].Value = racun.Isprintan;
            racunDr.Cells["Storniran"].Value = racun.Storniran;
            racunDr.Cells["Komitent"].Value = racun.Komitent == null ? String.Empty : racun.Komitent.Naziv;
            //jasenko tring
            racunDr.Cells["BrojFisk"].Value = racun.BrojFisk;
            racunDr.Cells["BrojKase"].Value = racun.BrojKase;
            racunDr.Cells["FiskalniIznos"].Value = racun.FiskalniIznos;
            racunDr.Cells["FiskalniDatum"].Value = racun.FiskalniDatum;
 
            EditBaseFields<Racun>(racunDr, racun);
        }

        public static DataTable GetTableStavkeRacuna(List<RacunStavka> stavkeRacuna)
        {
            if (BeginTableCreation != null)
                BeginTableCreation("Prikazivanje Stavki racuna...");

            DataTable stavkeRacunaDt = CreateTableStavkeRacuna();

            int step = GetStep(stavkeRacuna.Count);

            int i = 0;
            foreach (RacunStavka stavka in stavkeRacuna)
            {
                i++;
                if (i % step == 0 && ProgressUpdate != null)
                    ProgressUpdate(null, new PresentationEventArgs(stavkeRacuna.Count, i, "Nesto"));
                AddRecordToStavkeRacuna(stavkeRacunaDt, stavka,i);
            }

            if (EndTableCreation != null)
                EndTableCreation(String.Format("Učitano je {0} stavki.",i.ToString()));

            return stavkeRacunaDt;
        }

        public static void AddRecordToStavkeRacuna(DataTable stavkeRacunaDt, RacunStavka stavkaRacuna)
        {
            AddRecordToStavkeRacuna(stavkeRacunaDt, stavkaRacuna, 0);
        }
        public static void AddRecordToStavkeRacuna(DataTable stavkeRacunaDt, RacunStavka stavkaRacuna, int rb)
        {
            if (stavkaRacuna == null)
                return;

            DataRow dr = stavkeRacunaDt.NewRow();
            dr["ID"] = stavkaRacuna.ID;
            dr["RacunID"] = stavkaRacuna.RacunID;
            dr["RobaID"] = stavkaRacuna.RobaID;
            dr["Roba"] = stavkaRacuna.Roba == null ? String.Empty : stavkaRacuna.Roba.Naziv;
            dr["Sifra"] = stavkaRacuna.Roba == null ? String.Empty : stavkaRacuna.Roba.Sifra;
            dr["Kolicina"] = stavkaRacuna.Kolicina;
            dr["Participacija"] = stavkaRacuna.Participacija > 0 ? ((decimal)stavkaRacuna.Participacija / 100) : 0;
            dr["ReferalnaCijena"] = stavkaRacuna.ReferalnaCijena;
            dr["PDVStopa"] = stavkaRacuna.PDVStopa > 0 ? (stavkaRacuna.PDVStopa / 100) : 0;
            dr["Cijena"] = stavkaRacuna.Cijena;
            dr["Vrijednost"] = stavkaRacuna.Vrijednost;
            dr["Iznos"] = stavkaRacuna.Iznos;
            dr["Ljekar"] = stavkaRacuna.Ljekar == null ? String.Empty :stavkaRacuna.Ljekar.Sifra;
            dr["Vrsta"] = stavkaRacuna.Vrsta == null ? String.Empty : stavkaRacuna.Vrsta.ID.ToString();
            dr["JMBG"] = stavkaRacuna.JMBG;
            dr["Pausal"] = stavkaRacuna.Pausal;
            dr["BrojRecepta"] = stavkaRacuna.BrojRecepta;
            dr["RB"] = rb;
            //jasenko test
            dr["PonovljenRecept"] = stavkaRacuna.PonovljenRecept;
            dr["PropisanaKolicina"] = stavkaRacuna.PropisanaKolicina;
            
            AddBaseFieldsToRecord<RacunStavka>(stavkaRacuna, dr);
            stavkeRacunaDt.Rows.Add(dr);
        }

        public static void EditRecordStavkeRacuna(DataGridViewRow racunStavkeDr, RacunStavka stavkaRacuna)
        {
            if (stavkaRacuna == null)
                return;

            racunStavkeDr.Cells["ID"].Value = stavkaRacuna.ID;
            racunStavkeDr.Cells["RacunID"].Value = stavkaRacuna.RacunID;
            racunStavkeDr.Cells["RobaID"].Value = stavkaRacuna.RobaID;
            racunStavkeDr.Cells["Sifra"].Value = stavkaRacuna.Roba.Sifra;
            racunStavkeDr.Cells["Roba"].Value = stavkaRacuna.Roba.Naziv;
            racunStavkeDr.Cells["Kolicina"].Value = stavkaRacuna.Kolicina;
            racunStavkeDr.Cells["Participacija"].Value = stavkaRacuna.Participacija > 0 ? ((decimal)stavkaRacuna.Participacija / 100) : 0; ;
            racunStavkeDr.Cells["ReferalnaCijena"].Value = stavkaRacuna.ReferalnaCijena;
            racunStavkeDr.Cells["PDVStopa"].Value = stavkaRacuna.PDVStopa > 0 ? (stavkaRacuna.PDVStopa / 100) : 0;
            racunStavkeDr.Cells["Cijena"].Value = stavkaRacuna.Cijena;
            racunStavkeDr.Cells["Vrijednost"].Value = stavkaRacuna.Vrijednost;
            racunStavkeDr.Cells["Iznos"].Value = stavkaRacuna.Iznos;
            racunStavkeDr.Cells["Ljekar"].Value = stavkaRacuna.Ljekar == null ? String.Empty : stavkaRacuna.Ljekar.Sifra;
            racunStavkeDr.Cells["Vrsta"].Value = stavkaRacuna.Vrsta == null ? String.Empty : stavkaRacuna.Vrsta.ID.ToString();
            racunStavkeDr.Cells["JMBG"].Value = stavkaRacuna.JMBG;
            racunStavkeDr.Cells["Pausal"].Value = stavkaRacuna.Pausal;
            racunStavkeDr.Cells["BrojRecepta"].Value = stavkaRacuna.BrojRecepta;
            //jasenko test
            racunStavkeDr.Cells["PonovljenRecept"].Value = stavkaRacuna.PonovljenRecept;
            racunStavkeDr.Cells["PropisanaKolicina"].Value = stavkaRacuna.PropisanaKolicina;

            EditBaseFields<RacunStavka>(racunStavkeDr, stavkaRacuna);
        }



        private static DataTable CreateTableKorisnici()
        {
            DataTable dt = new DataTable("Korisnici");
            dt.Columns.Add(new DataColumn("Ime", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Prezime", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Titula", typeof(System.String)));
            dt.Columns.Add(new DataColumn("KorisnickoIme", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Lozinka", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Email", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Admin", typeof(System.Boolean)));
            AddBaseFields(dt);
            
            return dt;
        }

        private static DataTable CreateTableLjekari()
        {
            DataTable dt = new DataTable("Ljekari");
            dt.Columns.Add(new DataColumn("Sifra", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Aktivan", typeof(System.Boolean)));
            dt.Columns.Add(new DataColumn("Ime", typeof(System.String)));
            AddBaseFields(dt);

            return dt;
        }

        private static DataTable CreateTableKomitenti()
        {
            DataTable dt = new DataTable("Komitenti");
            dt.Columns.Add(new DataColumn("Sifra", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Naziv", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Mjesto", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Adresa", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Racun", typeof(System.String)));
            dt.Columns.Add(new DataColumn("PDV", typeof(System.String)));
            dt.Columns.Add(new DataColumn("IDB", typeof(System.String)));
            AddBaseFields(dt);

            return dt;
        }

        private static DataTable CreateTableMjere()
        {
            DataTable dt = new DataTable("Mjere");
            dt.Columns.Add(new DataColumn("Naziv", typeof(System.String)));
            AddBaseFields(dt);

            return dt;
        }

        private static DataTable CreateTablePDVStope()
        {
            DataTable dt = new DataTable("PDVStope");
            dt.Columns.Add(new DataColumn("Naziv", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Stopa", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("TBR", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Opis", typeof(System.String)));
            dt.Columns.Add(new DataColumn("UplatniRacun", typeof(System.String)));
            AddBaseFields(dt);

            return dt;
        }

        private static DataTable CreateTableRoba()
        {
            DataTable dt = new DataTable("Roba"); 
            dt.Columns.Add(new DataColumn("Sifra", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Naziv", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Proizvodjac", typeof(System.String)));
            
            dt.Columns.Add(new DataColumn("FakturnaCijena", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("PDVStopa", typeof(System.String)));
            //dt.Columns.Add(new DataColumn("Kolicina", typeof(System.Decimal)));

            dt.Columns.Add(new DataColumn("Opis", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Oznaka", typeof(System.Char)));
            dt.Columns.Add(new DataColumn("Participacija", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("ReferalnaCijena", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("ATC", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Ulaz", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Izlaz", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Zaliha", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("PocetnoStanje", typeof(System.Decimal)));
            //dt.Columns.Add(new DataColumn("VrstaProskripcije", typeof(System.String)));
            dt.Columns.Add(new DataColumn("MPC", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Mjera", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Taksa", typeof(System.Boolean)));
            AddBaseFields(dt);

            return dt;
        }

        private static DataTable CreateTableDokumenti()
        {
            DataTable dt = new DataTable("Dokumenti");

            dt.Columns.Add(new DataColumn("VrstaDokumenta", typeof(System.String)));
            dt.Columns.Add(new DataColumn("DatumDokumenta", typeof(System.DateTime)));
            dt.Columns.Add(new DataColumn("BrojDokumenta", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("DatumRacuna", typeof(System.DateTime)));
            dt.Columns.Add(new DataColumn("BrojRacuna", typeof(System.String)));
         
            dt.Columns.Add(new DataColumn("Komitent", typeof(System.String)));
            dt.Columns.Add(new DataColumn("SifraKomitenta", typeof(System.String)));
            dt.Columns.Add(new DataColumn("FakturnaVrijednost", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Rabat", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("UlazniPDV", typeof(System.Decimal)));

            dt.Columns.Add(new DataColumn("NabavnaVrijednost", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Marza", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("UkalkulisaniPDV", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("IznosRacuna", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("ProdajnaVrijednost", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Zatvoreno", typeof(System.Boolean)));

            AddBaseFields(dt);

            return dt;
        }

        private static DataTable CreateTableProizvodjaci()
        {
            DataTable dt = new DataTable("Dokumenti");

            dt.Columns.Add(new DataColumn("Sifra", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Naziv", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Telefon", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Grad", typeof(System.String)));
            dt.Columns.Add(new DataColumn("PTT", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Adresa", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Email", typeof(System.String)));

            AddBaseFields(dt);
            return dt;
        }

        private static DataTable CreateTableProskripcije()
        {
            DataTable dt = new DataTable("Proskripcije");
            dt.Columns.Add(new DataColumn("Sifra", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Naziv", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Vrsta", typeof(System.String)));
            AddBaseFields(dt);

            return dt;
        }

        private static void AddBaseFields(DataTable dt)
        {
            if (dt == null)
                return;

            dt.Columns.Add(new DataColumn("ID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("Modifikator", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Kreator", typeof(System.String)));
            dt.Columns.Add(new DataColumn("VrijemeKreiranja", typeof(System.DateTime)));
            dt.Columns.Add(new DataColumn("VrijemeModifikacije", typeof(System.DateTime)));
        }

        private static DataTable CreateTableNalozi()
        {
            DataTable dt = new DataTable("Nalozi");
            dt.Columns.Add(new DataColumn("Roba", typeof(System.String)));
            dt.Columns.Add(new DataColumn("SifraRobe", typeof(System.String)));
            dt.Columns.Add(new DataColumn("RobaID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("Datum", typeof(System.DateTime)));
            dt.Columns.Add(new DataColumn("Kolicina", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("VrijednostSastojaka", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("IznosTaksi", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Cijena", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Iznos", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Odstupanje", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("PoslovnaJedinica", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("Storniran", typeof(System.Boolean)));
            
            AddBaseFields(dt);
            return dt;
        }

        private static DataTable CreateTableNormativi()
        {
            DataTable dt = new DataTable("Normativi");
            
            dt.Columns.Add(new DataColumn("RobaID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("MaterijalID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("SifraRobe", typeof(System.String)));
            dt.Columns.Add(new DataColumn("NazivRobe", typeof(System.String)));
            dt.Columns.Add(new DataColumn("SifraMaterijala", typeof(System.String)));
            dt.Columns.Add(new DataColumn("NazivMaterijala", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Normativ", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Utrosak", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Iznos", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("MaterijalZaliha", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Taksa", typeof(System.Boolean)));
            dt.Columns.Add(new DataColumn("MPC", typeof(System.Decimal)));

            AddBaseFields(dt);
            return dt;
        }
        private static DataTable CreateTableBarKodovi()
        {
            DataTable dt = new DataTable("BarKodovi");
            dt.Columns.Add(new DataColumn("Sifra", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Roba", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Proizvodjac", typeof(System.String)));
            dt.Columns.Add(new DataColumn("BarKod", typeof(System.String)));
            AddBaseFields(dt);

            return dt;
        }
        private static DataTable CreateTableStavkeDokumenta()
        {
            DataTable dt = new DataTable("StavkeDokumenta");
            dt.Columns.Add(new DataColumn("RB", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("Sifra Robe", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Roba", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Dokument", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("Kolicina", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("FakturnaCijena", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("FakturnaVrijednost", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Rabat", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Marza", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("TBR", typeof(System.String)));
            dt.Columns.Add(new DataColumn("PDVStopa", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("UkalkulisaniPDV", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("MPC", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("MPV", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Mjera", typeof(System.String)));
            AddBaseFields(dt);

            return dt;
        }
        private static DataTable CreateTableSmjene()
        {
            DataTable dt = new DataTable("Smjene");
            dt.Columns.Add(new DataColumn("VrijemeOtvaranja", typeof(System.DateTime)));
            dt.Columns.Add(new DataColumn("VrijemeZatvaranja", typeof(System.DateTime)));
            dt.Columns.Add(new DataColumn("Broj", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("OdgovornoLice", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Ukupno", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Zatvorena", typeof(System.Boolean)));
            AddBaseFields(dt);

            return dt;
        }

        private static DataTable CreateTableTipoviRobe()
        {
            DataTable dt = new DataTable("TipoviRobe");
            dt.Columns.Add(new DataColumn("Sifra", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Naziv", typeof(System.String)));

            AddBaseFields(dt);

            return dt;
        }

        private static DataTable CreateTableRacuni()
        {
            DataTable dt = new DataTable("Racuni");
            dt.Columns.Add(new DataColumn("Datum", typeof(System.DateTime)));
            dt.Columns.Add(new DataColumn("Vrijednost", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Iznos", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Smjena", typeof(System.String)));
            dt.Columns.Add(new DataColumn("NacinPlacanja", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Isprintan", typeof(System.Boolean)));
            dt.Columns.Add(new DataColumn("Storniran", typeof(System.Boolean)));
            dt.Columns.Add(new DataColumn("Komitent", typeof(System.String)));
            //jasenko tring
            dt.Columns.Add(new DataColumn("BrojFisk", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("BrojKase", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("FiskalniIznos", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("FiskalniDatum", typeof(System.DateTime)));

            AddBaseFields(dt);

            return dt;
        }

        private static DataTable CreateTableStavkeRacuna()
        {
            DataTable dt = new DataTable("Racuni");
            dt.Columns.Add(new DataColumn("RacunID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("RobaID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("RB", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("Roba", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Kolicina", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("ReferalnaCijena", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Participacija", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("PDVStopa", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Cijena", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Vrijednost", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Iznos", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Ljekar", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Vrsta", typeof(System.String)));
            dt.Columns.Add(new DataColumn("JMBG", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Pausal", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("BrojRecepta", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Sifra", typeof(System.String)));
            //jasenko test
            dt.Columns.Add(new DataColumn("PonovljenRecept", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("PropisanaKolicina", typeof(System.Decimal)));
            
            AddBaseFields(dt);

            return dt;
        }
    }
}
