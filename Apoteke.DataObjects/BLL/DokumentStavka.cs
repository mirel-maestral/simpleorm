using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects;
using Apoteke.DataObjects.Core;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_DokumentStavka_Ins", "sp_DokumentStavka_Upd",
        "sp_DokumentStavka_DelByID", "sp_DokumentStavka_SelAll", "sp_DokumentStavka_SelByID")]
    public class DokumentStavka : ApotekaBase<DokumentStavka>
    {
        [ObjectProperty("pRobaID", "RobaID")]
        public int RobaID
        {
            get;
            set;
        }

        [ObjectProperty("pDokumentID", "DokumentID")]
        public int DokumentID
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

        //[ObjectProperty("pMjeraID", "MjeraID")]
        //public int MjeraID
        //{
        //    get;
        //    set;
        //}

        [ObjectProperty("pPDVStopa", "PDVStopa")]
        public decimal PDVStopa
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

        [ObjectProperty("pFakturnaCijena", "FakturnaCijena")]
        public decimal FakturnaCijena
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

        [ObjectProperty("pRabat", "Rabat")]
        public decimal Rabat
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

        [ObjectProperty("pMPC", "MPC")]
        public decimal MPC
        {
            get;
            set;
        }

        [ObjectProperty("pMPV", "MPV")]
        public decimal MPV
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

        [ObjectProperty("pTBR", "TBR")]
        public string TBR
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

        [ObjectProperty("pZaliha", "Zaliha")]
        public decimal Zaliha
        {
            get;
            set;
        }

        public Dokument Dokument
        {
            get { return Dokument.GetByID(this.DokumentID); }
        }

        public Roba Roba
        {
            get { return Roba.GetByID(this.RobaID); }
        }

        public DokumentStavka()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public DokumentStavka(int id)
            : base(id)
        { }
    }
}
