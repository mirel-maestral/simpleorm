using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects.Core;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_MagNormativ_Ins", "sp_MagNormativ_Upd", "sp_MagNormativ_DelByID",
        "sp_MagNormativ_SelAll", "sp_MagNormativ_SelByID")]
    public class Normativ : ApotekaBase<Normativ>
    {

        public Roba Roba
        {
            get { return Roba.GetByID(this.RobaID); }
        }
        
        [ObjectProperty("pRobaID", "RobaID")]
        public int RobaID
        {
            get;
            set;
        }

        public Roba MaterijalRoba
        {
            get { return Roba.GetByID(this.MaterijalRobaID); }
        }
        
        
        [ObjectProperty("pMaterijalRobaID", "MaterijalRobaID")]
        public int MaterijalRobaID
        {
            get;
            set;
        }


        [ObjectProperty("pNormativ", "Normativ")]
        public decimal NormativVrijednost
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
        
        [ObjectProperty("pTaksa", "Taksa")]
        public bool Taksa
        {
            get;
            set;
        }


        public Normativ()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public Normativ(int id)
            : base(id)
        { }

        public static List<Normativ> GetAll()
        {
            return ApotekeDB.Instance.GetNormativi(false);
        }

        public static List<Normativ> GetAllByRoba(int robaId)
        {
            return ApotekeDB.Instance.GetNormativi(robaId);
        }

    }
}
