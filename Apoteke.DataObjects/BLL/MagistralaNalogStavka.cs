using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects.Core;
using MySql.Data.MySqlClient;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_NalogStavka_Ins", "sp_NalogStavka_Upd", "sp_NalogStavka_DelByID",
       "sp_NalogStavka_SelAll", "sp_NalogStavka_SelByID")]
    public class MagistralaNalogStavka : ApotekaBase<MagistralaNalogStavka>

    {

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

        [ObjectProperty("pMagNalogID", "MagNalogID")]
        public int MagistralaNalogID
        {
            get;
            set;
        }

        public decimal Normativ
        {
            get;
            set;
        }

        public decimal Kolicina
        {
            get;
            set;
        }

        public decimal Iznos
        {
            get;
            set;
        }

        public decimal Cijena
        {
            get;
            set;
        }

        public bool Taksa
        {
            get;
            set;
        }

         public MagistralaNalogStavka()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

         public MagistralaNalogStavka(int id)
            : base(id)
        { }


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
                 MySqlCommand command = new MySqlCommand("sp_MagStavka_Ins", trans.Connection, trans);
                 command.CommandType = System.Data.CommandType.StoredProcedure;
                 command.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                 command.Parameters.Add(new MySqlParameter("pMaterijalRobaID", MySqlDbType.Int32)).Value = this.MaterijalRobaID;
                 command.Parameters.Add(new MySqlParameter("pMagNalogID", MySqlDbType.Int32)).Value = this.MagistralaNalogID;
                 command.Parameters.Add(new MySqlParameter("pNormativ", MySqlDbType.Decimal)).Value = this.Normativ;
                 command.Parameters.Add(new MySqlParameter("pKolicina", MySqlDbType.Decimal)).Value = this.Kolicina;
                 command.Parameters.Add(new MySqlParameter("pCijena", MySqlDbType.Decimal)).Value = this.Cijena;
                 command.Parameters.Add(new MySqlParameter("pIznos", MySqlDbType.Decimal)).Value = this.Iznos;
                 command.Parameters.Add(new MySqlParameter("pModifikatorID", MySqlDbType.Int32)).Value = Global.Instance.LoggedKorisnik.ID;
                 command.Parameters.Add(new MySqlParameter("pKreatorID", MySqlDbType.Int32)).Value = Global.Instance.LoggedKorisnik.ID;
                 command.Parameters.Add(new MySqlParameter("pTaksa", MySqlDbType.Bit)).Value = this.Taksa;
                 command.Parameters.Add(new MySqlParameter("pPoslovnaJedinica", MySqlDbType.Int32)).Value = this.PoslovnaJedinica;
     
                 command.ExecuteNonQuery();

                 int id = (int)command.Parameters["pID"].Value;
                 this.ID = id;
                 this.State = ObjectState.Existing;
                 return true;
             }
             catch (SaveFailedException saveEx)
             {
                 Logging.Log.Create("Insert (Transaction) MagStavkeNaloga. Neko je vec promjenio podatke.",
                     Logging.LogEntryLevel.Critical, saveEx);
                 //throw saveEx;
             }
             catch (System.Exception ex)
             {
                 Logging.Log.Create("Greska prilikom Insert (Transaction) MagStavkeNaloga.",
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
                 MySqlCommand command = new MySqlCommand("sp_MagStavka_Upd", trans.Connection, trans);
                 command.CommandType = System.Data.CommandType.StoredProcedure;
                 command.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Value = this.ID;
                 command.Parameters.Add(new MySqlParameter("pMaterijalRobaID", MySqlDbType.Int32)).Value = this.MaterijalRobaID;
                 command.Parameters.Add(new MySqlParameter("pMagNalogID", MySqlDbType.Int32)).Value = this.MagistralaNalogID;
                 command.Parameters.Add(new MySqlParameter("pNormativ", MySqlDbType.Decimal)).Value = this.Normativ;
                 command.Parameters.Add(new MySqlParameter("pKolicina", MySqlDbType.Decimal)).Value = this.Kolicina;
                 command.Parameters.Add(new MySqlParameter("pCijena", MySqlDbType.Decimal)).Value = this.Cijena;
                 command.Parameters.Add(new MySqlParameter("pIznos", MySqlDbType.Decimal)).Value = this.Iznos;
                 command.Parameters.Add(new MySqlParameter("pModifikatorID", MySqlDbType.Int32)).Value = Global.Instance.LoggedKorisnik.ID;
                 command.Parameters.Add(new MySqlParameter("pTaksa", MySqlDbType.Bit)).Value = this.Taksa;

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
                 Logging.Log.Create("Update (Transaction) MagStavkeNaloga. Neko je vec promjenio podatke.",
                     Logging.LogEntryLevel.Critical, saveEx);
                 //throw saveEx;
             }
             catch (System.Exception ex)
             {
                 Logging.Log.Create("Greska prilikom Update (Transaction) MagStavkeNaloga.",
                     Logging.LogEntryLevel.Critical, ex);
                 //throw ex;
             }

             return false;
         }
    }
}
