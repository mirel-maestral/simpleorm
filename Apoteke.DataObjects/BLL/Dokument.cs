using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects;
using Apoteke.DataObjects.Core;
using MySql.Data.MySqlClient;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_Dokument_Ins", "sp_Dokument_Upd", "sp_Dokument_DelByID", "sp_Dokument_SelAll", "sp_Dokument_SelByID")]
    public class Dokument: ApotekaBase<Dokument>
    {
        [ObjectProperty("pVrstaDokumenta", "VrstaDokumenta")]
        public VrstaDokumentaEnum VrstaDokumenta
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

        [ObjectProperty("pBrojRacuna", "BrojRacuna")]
        public string BrojRacuna
        {
            get;
            set;
        }

        [ObjectProperty("pDatumRacuna", "DatumRacuna")]
        public DateTime DatumRacuna
        {
            get;
            set;
        }

        [ObjectProperty("pRabat", "Rabat")]
        public decimal Rabat
        {
            get;
            set;
        }

        [ObjectProperty("pUlazniPDV", "UlazniPDV")]
        public decimal UlazniPDV
        {
            get;
            set;
        }

        public decimal Iznos
        {
           get{return (this.FakturnaVrijednost - this.Rabat + this.UlazniPDV); }
        }

        [ObjectProperty("pDatumDokumenta", "DatumDokumenta")]
        public DateTime DatumDokumenta
        {
            get;
            set;
        }

        [ObjectProperty("pBrojDokumenta", "BrojDokumenta")]
        public int BrojDokumenta
        {
            get;
            set;
        }

        [ObjectProperty("pBrojTK", "BrojTK")]
        public int BrojTK
        {
            get;
            set;
        }

        [ObjectProperty("pFakturnaVrijednost", "FakturnaVrijednost")]
        public decimal FakturnaVrijednost
        {
            get;
            set;
        }

        [ObjectProperty("pZatvoreno", "Zatvoreno")]
        public bool Zatvoreno
        {
            get;
            set;
        }

        [ObjectProperty("pNabavnaVrijednost", "NabavnaVrijednost")]
        public decimal NabavnaVrijednost
        {
            get;
            set;
        }

        [ObjectProperty("pMarza", "Marza")]
        public decimal Marza
        {
            get;
            set;
        }

        [ObjectProperty("pUkalkulisaniPDV", "UkalkulisaniPDV")]
        public decimal UkalkulisaniPDV
        {
            get;
            set;
        }

        [ObjectProperty("pProdajnaVrijednost", "ProdajnaVrijednost")]
        public decimal ProdajnaVrijednost
        {
            get;
            set;
        }

        //jasenko
        [ObjectProperty("pExportovano", "Exportovano")]
        public int Exportovano
        {
            get;
            set;
        }
        //end

        public Komitent Komitent
        {
            get { return Komitent.GetByID(this.KomitentID); }
        }

        public List<DokumentStavka> Stavke
        {
            get { return ApotekeDB.Instance.GetStavkeDokumenta(this.ID); }
        }

        public Dokument()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public Dokument(int id)
            : base(id)
        { }

        

        public static int GetNextBrojDokumenta(VrstaDokumentaEnum vrstaDokumenta)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_GetNextBrojDokumenta", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pVrstaDokumenta", MySqlDbType.Int32)).Value = (int)vrstaDokumenta;
                    command.Parameters.Add(new MySqlParameter("@pNextBrojDokumenta", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                    command.ExecuteNonQuery();

                    return (int)command.Parameters["@pNextBrojDokumenta"].Value;
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Error while trying to exec GetNextBrojDokumenta.", Logging.LogEntryLevel.Critical, ex);
            }

            return -1;
        }

        public static Dokument GetByID(int id)
        {
            return ApotekeDB.Instance.GetDokument(id);
        }
    }
}
