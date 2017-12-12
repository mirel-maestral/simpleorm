using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoteke.DataObjects.BLL;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Drawing.Printing;
using System.IO;
using System.IO.Compression;

namespace Apoteke.DataObjects
{
    public class Global
    {
        public bool DisableCache
        {
            get;
            set;
        }
        public bool UseMessaging
        {
            get;
            set;
        }

        public Korisnik LoggedKorisnik
        {
            get;
            set;
        }

        public string NazivApoteke
        {
            get;
            set;
        }

        public int BrojPoslovnice
        {
            get;
            set;
        }

        public string RecentlyUsedJMBG
        {
            get;
            set;
        }

        public DateTime DatumPicker
        {
            get;
            set;
        }

        public Boolean PrviPut
        {
            get;
            set;
        }

        public string POSPrinterName
        {
            get
            {
                foreach (string pName in PrinterSettings.InstalledPrinters)
                    if (pName.StartsWith(this.Konfiguracija["POSPrinterName"]))
                        return pName;
                return null;
            }
        }

        public bool IsPOSPrinterInstalled
        {
            get
            {
                foreach (string pName in PrinterSettings.InstalledPrinters)
                    if (pName.StartsWith(this.Konfiguracija["POSPrinterName"]))
                        return true;
                return false;
            }
        }

        private Dictionary<string, string> _konfiguracija = new Dictionary<string, string>();
        public Dictionary<string, string> Konfiguracija
        {
            get { return _konfiguracija; }
        }

        private static Global _instance = null;

        public static Global Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Global();
                return _instance;
            }
        }

        protected Global()
        {
            LoadKonfiguracija();
        }

        public static bool IsEmail(string email)
        {
            Regex regexept = new Regex(@"^[\w-]+(?:\.[\w-]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7}$",
               RegexOptions.IgnoreCase);

            if (regexept.Match(email).Success)
                return true;

            return false;
        }

      
        public void LoadKonfiguracija()
        {
            _konfiguracija.Clear();
            using (MySqlConnection con = ApotekeDB.Instance.OpenConnection())
            {
                MySqlCommand command = new MySqlCommand("sp_Konfiguracija_SelAll", con);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = command.ExecuteReader();

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string name = dr.IsDBNull(dr.GetOrdinal("Name")) ? String.Empty : dr.GetString(dr.GetOrdinal("Name"));
                        string value = dr.IsDBNull(dr.GetOrdinal("Value")) ? String.Empty : dr.GetString(dr.GetOrdinal("Value"));
                        if (!String.IsNullOrEmpty(name))
                            _konfiguracija.Add(name, value);
                    }
                }
            }                 
            
        }

        public decimal Zaokruzenje(decimal broj, decimal jedinicaZaokruzenja)
        {
            decimal rezZaok = 0;
            decimal rez = 0;

            rez = Math.Round(broj, 1) - broj;

            if (rez < 0)
                rez = rez * (-1);

            if (rez < jedinicaZaokruzenja / 2)
            {
                rezZaok = Math.Round(broj, 1);
            }

            else if (Math.Round(broj, 1) - broj < 0)
            {
                rezZaok = Math.Round(broj, 1) + jedinicaZaokruzenja;
            }
            else if (Math.Round(broj, 1) - broj > 0)
            {
                rezZaok = Math.Round(broj, 1) - jedinicaZaokruzenja;
            }

            return rezZaok;
        }

        public decimal Zaokruzenje(decimal broj)
        {
            decimal jedinicaZaokruzenja = 0;

            if( !String.IsNullOrEmpty( this.Konfiguracija["JedinicaZaokruzenja"]) )
                decimal.TryParse(this.Konfiguracija["JedinicaZaokruzenja"], out jedinicaZaokruzenja);

            if (jedinicaZaokruzenja == 0)
                return 0;

            return Zaokruzenje(broj, jedinicaZaokruzenja);
        }

        public static MemoryStream GZipCompress(string filename)
        {
            Console.WriteLine("Test compression and decompression on file {0}", filename);
            FileStream infile;
            try
            {
                // Open the file as a FileStream object.
                infile = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] buffer = new byte[infile.Length];
                // Read the file to ensure it is readable.
                int count = infile.Read(buffer, 0, buffer.Length);
                if (count != buffer.Length)
                {
                    infile.Close();
                    return null;
                }
                infile.Close();
                MemoryStream ms = new MemoryStream();
                // Use the newly created memory stream for the compressed data.
                GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
                compressedzipStream.Write(buffer, 0, buffer.Length);
                // Close the stream.
                compressedzipStream.Close();

                return ms;

            } 
            catch (System.Exception ex)
            {
                Logging.Log.Create(String.Format("Greska prilikom kompresije fajla {0} .", filename),
                    Logging.LogEntryLevel.Critical, ex);
            }

            return null;
        }

    }
}
