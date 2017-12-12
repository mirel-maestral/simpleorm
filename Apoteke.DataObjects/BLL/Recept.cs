using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects;
using Apoteke.DataObjects.Core;
using MySql.Data.MySqlClient;

namespace Apoteke.DataObjects.BLL
{
    public class Recept
    {
        public int StavkaRacunaID
        {
            get;
            set;
        }

        public int LjekarID
        {
            get;
            set;
        }

        public int VrstaID
        {
            get;
            set;
        }

        public string JMBG
        {
            get;
            set;
        }

        public decimal Pausal
        {
            get;
            set;
        }

        public string BrojRecepta
        {
            get;
            set;
        }

        public int Participacija
        {
            get;
            set;
        }

        public int BrJedinica
        {
            get;
            set;
        }

        

        //eso dodao ovo dole
        public DateTime DatumPropisivanja
        {
            get;
            set;
        }
        
        //end eso
        
        //jasenko
        public int PonovljenRecept
        {
            get;
            set;
        }
        public decimal PropisanaKolicina
        {
            get;
            set;
        }
        //end
        
        public RacunStavka StavkaRacuna
        {
            get { return ApotekeDB.Instance.GetStavkuRacuna(this.StavkaRacunaID); }
        }

        public Ljekar Ljekar
        {
            get { return ApotekeDB.Instance.GetLjekar(this.LjekarID); }
        }

        public VrstaRecepta Vrsta
        {
            get { return ApotekeDB.Instance.GetVrstaRecepta(this.VrstaID); }
        }

        public Recept()
        { }

       
        public static string GetNextBrojRecepta()
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_GetNextBrojRecepta", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pBrojRecepta", MySqlDbType.String)).Direction = System.Data.ParameterDirection.Output;
                    command.ExecuteNonQuery();

                    return command.Parameters["@pBrojRecepta"].Value.ToString();
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom trazenja robe.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

        public static bool Exist(string brojRecepta)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_ReceptExist", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlCommandBuilder.DeriveParameters(command);
                    //command.Parameters.Add(new MySqlParameter("@pExist", MySqlDbType.)).Direction = System.Data.ParameterDirection.Output;
                    //command.Parameters.Add(new MySqlParameter("@pBrojRecepta", MySqlDbType.String)).Value = brojRecepta;

                    command.Parameters["@pBrojRecepta"].Value = brojRecepta;
                    
                    command.ExecuteNonQuery();

                    if ((int)command.Parameters["@pExist"].Value == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom trazenja recepta.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return false;
        }


        public bool Equals(Recept obj)
        {
            if (obj.BrojRecepta != this.BrojRecepta)
                return false;
            else if (obj.BrJedinica != this.BrJedinica)
                return false;
            else if (obj.JMBG != this.JMBG)
                return false;
            else if (obj.LjekarID != this.LjekarID)
                return false;
            else if (obj.Pausal != this.Pausal)
                return false;
            else if (obj.Participacija != this.Participacija)
                return false;
            //else if (obj.StavkaRacunaID != this.StavkaRacunaID)
            //    return false;
            else if (obj.VrstaID != this.VrstaID)
                return false;
            
            return true;
        }


        public static bool CanStornoRecept(DateTime datumRecepta)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_CanStornoRecept", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlCommandBuilder.DeriveParameters(command);
                    command.Parameters["@pDatum"].Value = datumRecepta;

                    command.ExecuteNonQuery();

                    if ((int)command.Parameters["@pCanStorno"].Value == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom CanStorno Recept metode.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return false;
        }

        public static bool IsInoPacijent(string jmbg)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_IsInoPacijent", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pExist", MySqlDbType.Int32)).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(new MySqlParameter("@pJmbg", MySqlDbType.String)).Value = jmbg;

                    command.ExecuteNonQuery();

                    if ((int)command.Parameters["@pExist"].Value == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom trazenja InoPacijenta.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return false;
        }

        public static bool PonovljeniExist(int RobaID, int LjekarID, string JMBG, DateTime DatumPropisivanja)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_ReceptPonovljeniExist", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlCommandBuilder.DeriveParameters(command);
                    //command.Parameters.Add(new MySqlParameter("@pExist", MySqlDbType.)).Direction = System.Data.ParameterDirection.Output;
                    //command.Parameters.Add(new MySqlParameter("@pBrojRecepta", MySqlDbType.String)).Value = brojRecepta;

                    command.Parameters["@pJMBG"].Value = JMBG;
                    command.Parameters["@pRobaID"].Value = RobaID;
                    command.Parameters["@pLjekarID"].Value = LjekarID;
                    command.Parameters["@pDatumPropisivanja"].Value = DatumPropisivanja.Date;

                    command.ExecuteNonQuery();

                    if ((int)command.Parameters["@pExist"].Value == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom trazenja ponovljenog recepta.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return false;
        }

        public static bool PonovljeniExist1(int RobaID, int LjekarID, string JMBG, DateTime DatumPropisivanja)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_ReceptPonovljeniExist1", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlCommandBuilder.DeriveParameters(command);
                    //command.Parameters.Add(new MySqlParameter("@pExist", MySqlDbType.)).Direction = System.Data.ParameterDirection.Output;
                    //command.Parameters.Add(new MySqlParameter("@pBrojRecepta", MySqlDbType.String)).Value = brojRecepta;

                    command.Parameters["@pJMBG"].Value = JMBG;
                    command.Parameters["@pRobaID"].Value = RobaID;
                    command.Parameters["@pLjekarID"].Value = LjekarID;
                    command.Parameters["@pDatumPropisivanja"].Value = DatumPropisivanja.Date;

                    command.ExecuteNonQuery();

                    if ((int)command.Parameters["@pExist"].Value == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom trazenja ponovljenog recepta.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return false;
        }

        public static bool PonovljeniExist2(int RobaID, int LjekarID, string JMBG, DateTime DatumPropisivanja)
        {
            try
            {
                using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand("sp_ReceptPonovljeniExist2", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlCommandBuilder.DeriveParameters(command);
                    //command.Parameters.Add(new MySqlParameter("@pExist", MySqlDbType.)).Direction = System.Data.ParameterDirection.Output;
                    //command.Parameters.Add(new MySqlParameter("@pBrojRecepta", MySqlDbType.String)).Value = brojRecepta;

                    command.Parameters["@pJMBG"].Value = JMBG;
                    command.Parameters["@pRobaID"].Value = RobaID;
                    command.Parameters["@pLjekarID"].Value = LjekarID;
                    command.Parameters["@pDatumPropisivanja"].Value = DatumPropisivanja.Date;

                    command.ExecuteNonQuery();

                    if ((int)command.Parameters["@pExist"].Value == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska prilikom trazenja ponovljenog recepta.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return false;
        }
    }
}
