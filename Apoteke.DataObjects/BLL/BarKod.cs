using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects.Core;
using MySql.Data.MySqlClient;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_BarKod_Ins", "sp_BarKod_Upd", "sp_BarKod_DelByID",
       "sp_BarKod_SelAll", "sp_BarKod_SelByID")]
    public class BarKod:ApotekaBase<BarKod>
    {
        public BarKod()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public BarKod(int id)
            : base(id)
        { }

        [ObjectProperty("pRobaID", "RobaID")]
        public int RobaID
        {
            get;
            set;
        }

        public Roba Roba
        {
            get { return Roba.GetByID(this.RobaID); }
        }

        [ObjectProperty("pBarKod", "BarKod")]
        public string Kod
        {
            get;
            set;
        }

        public static BarKod Find(string barKod)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_FindBarKod", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlCommandBuilder.DeriveParameters(command);
                    command.Parameters["@pBarKod"].Value = barKod;
                    MySqlDataReader dr = command.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                    if (dr != null && dr.Read())
                        return Factory.CreateBarKodFromReader(dr);
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom trazenja barkoda.", Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static bool Exists(int robaId, string barKod)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_BarKodExist", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pRobaID", MySqlDbType.Int32)).Value = robaId;
                    command.Parameters.Add(new MySqlParameter("@pBarKod", MySqlDbType.VarChar)).Value = barKod;
                    command.Parameters.Add(new MySqlParameter("@pExist", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                    command.ExecuteNonQuery();

                    if ((int)command.Parameters["@pExist"].Value == 1)
                        return true;
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska u provjeri postojanja BarKoda.", Logging.LogEntryLevel.Critical,
                    ex);
            }

            return false;
        }

    }
}
