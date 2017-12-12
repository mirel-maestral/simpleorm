using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects;
using Apoteke.DataObjects.Core;
using MySql.Data.MySqlClient;

namespace Apoteke.DataObjects.BLL
{
    [ObjectBinding("sp_Ljekar_Ins", "sp_Ljekar_Upd", "sp_Ljekar_DelByID","sp_Ljekar_SelAll","sp_Ljekar_SelByID")]
    public class Ljekar : ApotekaBase<Ljekar>
    {
        [ObjectProperty("pIme", "Ime")]
        public string Ime
        {
            get;
            set;
        }

        [ObjectProperty("pAktivan", "Aktivan")]
        public bool Aktivan
        {
            get;
            set;
        }

        [ObjectProperty("pSifra", "Sifra")]
        public string Sifra
        {
            get;
            set;
        }

        public Ljekar()
            : this(0)
        {
            this.PoslovnaJedinica = Global.Instance.BrojPoslovnice;
        }

        public Ljekar(int id)
            : base(id)
        { }

        public static bool Exist(string sifra)
        {
            using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
            {
                MySqlCommand command = new MySqlCommand("sp_ValidateSifraLjekara", con);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("@pSifra", MySqlDbType.VarChar)).Value = sifra;
                command.Parameters.Add(new MySqlParameter("@pResult", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                command.ExecuteNonQuery();

                if ((int)command.Parameters["@pResult"].Value == 1)
                    return true;
            }

            return false;
        }
    }
}
