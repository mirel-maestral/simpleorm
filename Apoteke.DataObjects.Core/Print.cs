using System;
using System.Drawing;
//using System.Windows.Forms;
using System.Data;

using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Management;

namespace Apoteke.DataObjects.Core
{
    public class Print
    {
        //#region ("Printanje Računa")
        //string Naslov = "";
        //string Stavke = "";
        //string End = "";

        //void SetRacNaslov()
        //{
        //    Naslov = "JU APOTEKE SARAJEVO\t \n";
        //    Naslov += "S.H. Muvekita 11, Sarajevo 71000\n";
        //    Naslov += "pdv: 200280090003\n";
        //    Naslov += "Galas 'ILIDZA'\n";
        //    Naslov += "E. Bogunica Carlija 15\n";
        //    //Naslov += "10.07.2007 11:40:12";

        //    Naslov += "\n";
        //    // ovim centrira text
        //    //			for(int i=0; i< 12;i++)
        //    //			{
        //    //				Naslov += " ";
        //    //			}
        //    Naslov += "Racun br: " + _rowFromTable["BRO"].ToString();

        //    for (int i = 0; i < 21 - int.Parse(("Racun br: " + _rowFromTable["BRO"]).ToString().Length.ToString()); i++)
        //    {
        //        Naslov += " ";
        //    }

        //    // Automatski računa koliko praznih mjesta treba da bude izmedju broja računa i datuma

        //    Naslov += System.DateTime.Now.ToShortDateString() + " " +
        //            System.DateTime.Now.ToShortTimeString();
        //    Naslov += "\n";
        //    Naslov += "----------------------------------------\n";
        //    Naslov += "Artikal    J.mj.          Cijena   Iznos\n";
        //    Naslov += "PDV (%-iznos)  Kolicina  (sa P D V - om)\n";
        //    Naslov += "----------------------------------------\n";
        //}

        //void SetRacStavke()
        //{
        //    double _iznos = 0.00;
        //    double _iznosPDV = 0.00;
        //    Stavke = "";

        //    foreach (DataGridViewRow drx in dataGridView1.Rows)
        //    {
        //        _iznos = double.Parse(drx.Cells["IZNOS"].Value.ToString());
        //        _iznosPDV = (_iznos / 100) * 17;

        //        // Računanje ukupnog iznosa, PDV-a i iznosa bez PDV-a
        //        UkupanIznos += _iznos;
        //        UkupanPDV += _iznosPDV;
        //        //
        //        //

        //        Stavke += drx.Cells["NAZ"].Value.ToString() + "\n";
        //        Stavke += "17%  " + string.Format("{0:0.00}", _iznosPDV) + "    " +
        //            drx.Cells["MJE"].Value.ToString() + "     " + string.Format("{0:F2}", drx.Cells["KOL"].Value)
        //            + "     " + string.Format("{0:0.00}", drx.Cells["MPC"].Value) +
        //            "    " + string.Format("{0:0.00}", drx.Cells["IZNOS"].Value) + "\n";
        //    }
        //    UkupanIznosBezPDV = UkupanIznos - UkupanPDV;
        //}

        //void SetRacEnd()
        //{
        //    string _alignRight = (char)27 + "" + (char)97 + "" + "2";
        //    string _alignLeft = (char)27 + "" + (char)97 + "" + "0";
        //    End = "----------------------------------------\n";
        //    //			End += "     Ukupan iznos bez PDV-a:       "+UkupanIznosBezPDV.ToString()+"\n";
        //    //			End += "         Ukupan iznos PDV-a:       "+UkupanPDV.ToString()+"\n";
        //    //			End += "     Ukupan iznos sa PDV-om:       "+UkupanIznos.ToString()+"\n";
        //    End += _alignRight;
        //    End += "Ukupan iznos bez PDV-a:       " + string.Format("{0:F2}", UkupanIznosBezPDV) + "\n";
        //    End += "Ukupan iznos PDV-a:       " + string.Format("{0:F2}", UkupanPDV) + "\n";
        //    End += "Ukupan iznos sa PDV-om:       " + string.Format("{0:0.00}", UkupanIznos) + "\n";
        //    End += "----------------------------------------\n";
        //    End += "        PLA" + (char)172 + "ENO GOTOVINOM:         " + string.Format("{0:F2}", txtIznosUkupno.Text) + "\n";
        //    End += "          Primljeni novac:         " + string.Format("{0:F2}", txtPrimljeniNovac.Text) + "\n";
        //    End += "          Iznos za povrat:         " + string.Format("{0:F2}", txtIznosPovrat.Text) + "\n";
        //    End += _alignLeft;

        //    End += "\n                 -Hvala-        \n";
        //}

        //void PrintRac()
        //{
        //    try
        //    {
        //        //			   	char a = (char)27;
        //        //			   	char b = (char)97;
        //        //			   	char c = (char)1;

        //        //string test = a+""+b+""+c;
        //        string set_on_start = (char)27 + "" + (char)64;
        //        string cut = (char)29 + "" + (char)86 + "" + (char)1;

        //        //PrintThroughDriver.SendStringToPrinter(Printer, set_on_start);
        //        PrintThroughDriver.SendStringToPrinter(Printer, Naslov);
        //        PrintThroughDriver.SendStringToPrinter(Printer, Stavke);
        //        PrintThroughDriver.SendStringToPrinter(Printer, End);
        //        PrintThroughDriver.SendStringToPrinter(Printer, "\n\n\n\n\n\n\n");//\n\n\n\n\n\n" );
        //        PrintThroughDriver.SendStringToPrinter(Printer, cut);

        //    }
        //    catch (Exception e2)
        //    {
        //        MessageBox.Show(e2.Message);
        //    }
        //}

        public bool PrinterSpreman(string printerName)
        {
            string set_on_start = (char)27 + "" + (char)64;
            PrintThroughDriver.SendStringToPrinter(printerName, set_on_start);
            //PrintStatTest();
            PrinterStatus chkPrinter = Wmis.GetStatus2(printerName);

            CancelPrintJob();

            // Printanje samo ako je printer spreman
            if (chkPrinter == PrinterStatus.Idle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //void IspisRacuna()
        //{
        //    if (PrinterSpreman())
        //    {
        //        SetRacNaslov();
        //        SetRacStavke();
        //        SetRacEnd();
        //        PrintRac();

        //        SetEndParameters();

        //        _rowFromTable["P"] = "1";
        //        //MySql_class.Query("UPDATE Mal_rac SET P = TRUE where ID = '"+
        //        //                  _rowFromTable["ID"].ToString()+"' ");
        //        //dataGridView1.SelectedRows[0].Cells[e.ColumnIndex].Value = true;

        //        DialogResult = DialogResult.OK;
        //        this.Close();
        //    }
        //    else
        //    {
        //        CancelPrintJob();
        //        MessageBox.Show("Došlo je do greške pri printanju!");
        //    }
        //    //MessageBox.Show(Naslov+Stavke+End);
        //}

        //void BtnPrintClick(object sender, EventArgs e)
        //{
        //    if (dataGridView1.Rows.Count > 0)
        //    {
        //        if (txtPrimljeniNovac.Text.Trim(',', '0').Length != 0 &&
        //           txtIznosPovrat.Text.Trim(',', '0').Length != 0)
        //        {

        //            IspisRacuna();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Niste unijeli iznos primljenog novca!");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Niste unijeli artikal!");
        //    }
        //}

        //void BtnPrintStatClick(object sender, EventArgs e)
        //{
        //    System.IntPtr lhPrinter = new System.IntPtr();

        //    PrintThroughDriver.OpenPrinter2("Generic", ref lhPrinter, 0);

        //    PRINTERINFO2 PI = new PRINTERINFO2();

        //    int Need = 0;
        //    PrintThroughDriver.GetPrinter(lhPrinter, 2, IntPtr.Zero, 0, ref Need);

        //    IntPtr pintStruct = Marshal.AllocCoTaskMem(Need);

        //    int SizeOf = Need;
        //    PrintThroughDriver.GetPrinter(lhPrinter, 2, pintStruct, SizeOf, ref Need);

        //    PI = (PRINTERINFO2)Marshal.PtrToStructure(pintStruct, typeof(PRINTERINFO2));

        //    MessageBox.Show(PI.Status.ToString());

        //    // Deallocate the memory.
        //    Marshal.FreeCoTaskMem(pintStruct);

        //    //MessageBox.Show(PrintThroughDriver.GetPrinter(lhPrinter,2,ref PI,Marshal.SizeOf(PI),ref Need).ToString() );


        //    //MessageBox.Show("Error "+Marshal.GetLastWin32Error());


        //    PrintThroughDriver.ClosePrinter2(lhPrinter);

        //    // Ovo radi al posalje prvo print i onda javi da ne radi, javlja i windows poruku!
        //    //			string set_on_start = (char)27+""+(char)64;			   	
        //    //			PrintThroughDriver.SendStringToPrinter(Printer, set_on_start);
        //    //			Wmis.GetStatus();
        //}

        public void CancelPrintJob()
        {
            string printerName = "Generic";
            System.Collections.Specialized.StringCollection printJobCollection = new System.Collections.Specialized.StringCollection();
            string searchQuery = "SELECT * FROM Win32_PrintJob WHERE Name like '" + printerName + "%'";

            /*searchQuery can also be mentioned with where Attribute,
                but this is not working in Windows 2000 / ME / 98 machines 
                and throws Invalid query error*/
            ManagementObjectSearcher searchPrintJobs =
                      new ManagementObjectSearcher(searchQuery);
            ManagementObjectCollection prntJobCollection = searchPrintJobs.Get();
            foreach (ManagementObject prntJob in prntJobCollection)
            {
                System.String jobName = prntJob.Properties["Name"].Value.ToString();

                //Job name would be of the format [Printer name], [Job ID]
                char[] splitArr = new char[1];
                splitArr[0] = Convert.ToChar(",");
                string _printerName = jobName.Split(splitArr)[0];
                int prntJobID = Convert.ToInt32(jobName.Split(splitArr)[1]);
                string documentName = prntJob.Properties["Document"].Value.ToString();
                if (String.Compare(_printerName, printerName, true) == 0)
                {
                    printJobCollection.Add(documentName);
                    //performs a action similar to the cancel 
                    //operation of windows print console
                    prntJob.Delete();
                    //isActionPerformed = true; 
                    break;
                }
            }
        }

        //void IspisKrozFajl()
        //{
        //    System.Text.StringBuilder strBuild = new System.Text.StringBuilder();
        //    string cut = (char)29 + "" + (char)86 + "" + (char)1;

        //    SetRacNaslov();
        //    SetRacStavke();
        //    SetRacEnd();

        //    string racun_name = "Racuni/Racun_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".txt";
        //    TextWriter tx = new StreamWriter(racun_name);//Racun_"+DateTime.Now.ToString()+".txt" );
        //    tx.Write(Naslov);
        //    tx.Write(Stavke);
        //    tx.Write(End);
        //    tx.Write("\n\n\n\n\n\n\n");
        //    tx.Write(cut);
        //    tx.Close();

        //    if (PrinterSpreman())
        //    {
        //        PrintThroughDriver.SendFileToPrinter(Printer, racun_name);

        //        SetEndParameters();
        //        DialogResult = DialogResult.OK;
        //        this.Close();
        //    }
        //    else
        //    {
        //        CancelPrintJob();
        //        MessageBox.Show("Došlo je do greške pri printanju!");
        //    }
        //}

        //void BtnPrint2Click(object sender, EventArgs e)
        //{
        //    IspisKrozFajl();
        //}
        //#endregion

        
    }

    #region svePotrebnoZaPrint
    // Structure and API declarions:
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

    public struct PRINTERINFO2
    {

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pServerName;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pPrinterName;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pShareName;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pPortName;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDriverName;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pComment;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pLocation;

        public IntPtr pDevMode;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pSepFile;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pPrintProcessor;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDatatype;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pParameters;

        public IntPtr pSecurityDescriptor;

        public int Attributes;

        public int Priority;

        public int DefaultPriority;

        public int StartTime;

        public int UntilTime;

        public int Status;

        public int cJobs;

        public int AveragePPM;

    }


    public class PrintThroughDriver
    {

        // Structure and API declarions:

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]

        internal class DOCINFOA
        {

            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;

        }

        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

        internal static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, Int32 pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

        internal static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

        internal static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

        internal static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

        internal static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

        internal static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

        internal static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        [DllImport("kernel32.dll", EntryPoint = "GetLastError", SetLastError = false, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

        internal static extern Int32 GetLastError();

        // Ovo je novo
        [DllImport("winspool.drv", EntryPoint = "OpenPrinter", CharSet = CharSet.Unicode, ExactSpelling = false,

       CallingConvention = CallingConvention.StdCall)]

        public static extern long OpenPrinter2(string pPrinterName, ref IntPtr
        phPrinter, int pDefault);

        [DllImport(

       "winspool.drv", EntryPoint = "ClosePrinter", CharSet = CharSet.Unicode, ExactSpelling = true,

       CallingConvention = CallingConvention.StdCall)]

        public static extern long ClosePrinter2(IntPtr hPrinter);

        [DllImport(

       "winspool.drv", EntryPoint = "GetPrinter", CharSet = CharSet.Auto)]

        public static extern bool GetPrinter(IntPtr hPrinter, int Level,
            //ref PRINTERINFO2 pPrinter
        IntPtr pPrinter, int cbBuf, ref int pcbNeeded);
        // do ovog dijela


        // SendBytesToPrinter()

        // When the function is given a printer name and an unmanaged array

        // of bytes, the function sends those bytes to the print queue.

        // Returns true on success, false on failure.

        private static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {

            Int32 dwError = 0, dwWritten = 0;

            IntPtr hPrinter = new IntPtr(0);

            DOCINFOA di = new DOCINFOA();

            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = "C# Debug Print RAW Document";

            di.pDataType = "RAW";

            // Open the printer.

            if (OpenPrinter(szPrinterName, out hPrinter, 0))
            {

                // Start a document.

                if (StartDocPrinter(hPrinter, 1, di))
                {

                    // Start a page.

                    if (StartPagePrinter(hPrinter))
                    {

                        // Write your bytes.

                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);

                        EndPagePrinter(hPrinter);

                    }

                    EndDocPrinter(hPrinter);

                }

                ClosePrinter(hPrinter);

            }

            // If you did not succeed, GetLastError may give more information

            // about why not.

            if (bSuccess == false)
            {

                dwError = GetLastError();

            }

            return bSuccess;

        }

        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            try
            {
                IntPtr pBytes;

                Int32 dwCount;

                // How many characters are in the string?

                dwCount = szString.Length;

                // Assume that the printer is expecting ANSI text, and then convert

                // the string to ANSI text.

                pBytes = Marshal.StringToCoTaskMemAnsi(szString);

                // Send the converted ANSI string to the printer.

                SendBytesToPrinter(szPrinterName, pBytes, dwCount);

                Marshal.FreeCoTaskMem(pBytes);
                return true;
            }
            catch (Exception exept)
            {
                //MessageBox.Show(exept.Message);
                return false;
            }
        }

        public static bool SendFileToPrinter(string szPrinterName, string szFileName)
        {
            // Open the file.
            FileStream fs = new FileStream(szFileName, FileMode.Open);
            // Create a BinaryReader on the file.
            BinaryReader br = new BinaryReader(fs);
            // Dim an array of bytes big enough to hold the file's contents.
            Byte[] bytes = new Byte[fs.Length];
            bool bSuccess = false;
            // Your unmanaged pointer.
            IntPtr pUnmanagedBytes = new IntPtr(0);
            int nLength;

            nLength = Convert.ToInt32(fs.Length);
            // Read the contents of the file into the array.
            bytes = br.ReadBytes(nLength);
            // Allocate some unmanaged memory for those bytes.
            pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
            // Copy the managed byte array into the unmanaged array.
            Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
            // Send the unmanaged bytes to the printer.
            bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength);
            // Free the unmanaged memory that you allocated earlier.
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            return bSuccess;
        }

        internal struct DOCINFO
        {

            [MarshalAs(UnmanagedType.LPWStr)]
            public string pDocName;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string pOutputFile;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string pDataType;

        }

    }

    #endregion svePotrebnoZaPrint

    public enum PrinterStatus
    {
        Other = 1,
        Unknown,
        Idle,
        Printing,
        Warmup,
        Stopped,
        printing,
        Offline
    }

    public class Wmis
    {
        public static void GetStatus()
        {
            PrinterStatus stat;
            if ((stat = GetPrinterStat("Generic")) != 0) // UNC or a local name
            {
                //MessageBox.Show(stat.ToString());
            }
            //else
                //MessageBox.Show("Failed to get status");
        }
        public static PrinterStatus GetStatus2(string printerName)
        {
            PrinterStatus stat;
            if ((stat = GetPrinterStat(printerName.Trim())) != 0) // UNC or a local name
            {
                return stat;
            }
            else
            {
                return PrinterStatus.Unknown;
            }
        }
        static PrinterStatus GetPrinterStat(string printerDevice)
        {
            PrinterStatus ret = 0;
            string path = "win32_printer.DeviceId='" + printerDevice + "'";
            using (ManagementObject printer = new ManagementObject(path))
            {
                printer.Get();
                PropertyDataCollection printerProperties = printer.Properties;
                PrinterStatus st =
                (PrinterStatus)Convert.ToInt32(printer.Properties["PrinterStatus"].Value);
                ret = st;
            }
            return ret;
        }
    }
}
