using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apoteke.DataObjects.Core
{
    public class GlobalVariable
    {
        public static int connectionStringID = 9;
        public static string connectionString = "";
        public static string startFormText = "Baza nije ucitana!!!";
        public static string IPjavna = "";
        public static string apoteka = "Nije ucitana!!!";
        public static string godina = "2017";
        public static bool IPcompa = false;
        public static string IPadres = "";
        public static string serverBaza = "";
        //verzija kase
        public static string verzija = "01.05";
        //podaci za fiskalnu kasu
        public static string stringFiskalni = "";
        public static string stringFiskalniLocal = "";
        public static int operater = 0;
        public static string lozinkaOper = "";
        public static int brKase = 0;
        public static bool statusPOSservera = false;
        public static int POSname = 0; //1- EPSON(+ERP); 2- TRING(+Favourite); 3- DATECS;
    }
}
