using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using Apoteke.DataObjects;
using System.Text.RegularExpressions;
using System.Drawing.Printing;
using System.IO;
using System.IO.Compression;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace Apoteke.DataObjects.Core
{
    public class DBAlati
    {     
        //public static bool SaveRecepti(string recept, string jmbg, DateTime datum, string tJMBG, int KreID, DateTime datumpropisivanja, DateTime tekucidatumpropisivanja)
        //{
        //    try
        //    {
                
        //        Database apoteke = new Database();
        //        MySqlConnection mySQLconn = new MySqlConnection();
        //        mySQLconn = apoteke.OpenConnection();

        //        MySqlCommand command = new MySqlCommand("sp_EditReceptJMBG", mySQLconn);
        //        command.CommandType = System.Data.CommandType.StoredProcedure;

        //        command.Parameters.AddWithValue("pBrojRecepta", recept);
        //        command.Parameters.AddWithValue("nJMBG", jmbg);
        //        command.Parameters.AddWithValue("pDatum", datum);
        //        command.Parameters.AddWithValue("tJMBG", tJMBG);
        //        command.Parameters.AddWithValue("pKreatorID", KreID);
        //        command.Parameters.AddWithValue("pDatumPropisivanja", datumpropisivanja);
        //        command.Parameters.AddWithValue("tDatumPropisivanja", tekucidatumpropisivanja);

        //        command.ExecuteNonQuery();
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public static void ReceptPretraga(DataTable dt, string recept)
        //{
        //    Database apoteke = new Database();
        //    MySqlConnection mySQLconn = new MySqlConnection();
        //    mySQLconn = apoteke.OpenConnection();

        //    MySqlDataAdapter adapter = new MySqlDataAdapter("sp_ReceptPretraga" ,mySQLconn);
        //    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

        //    adapter.SelectCommand.Parameters.AddWithValue("pBrojRecepta", recept);

        //    adapter.Fill(dt);
          
        //}

        //public static void FillRecepti(DataTable dt)
        //{
        //    Database apoteke = new Database();
        //    MySqlConnection mySQLconn = new MySqlConnection();
        //    mySQLconn = apoteke.OpenConnection();

        //    MySqlDataAdapter adapter = new MySqlDataAdapter("sp_FillReceptPretraga", mySQLconn);
        //    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

        //    adapter.Fill(dt);
        //}

        //public static void FillIPadresPoslovnice(DataTable dt)
        //{
        //    try
        //    {
        //        string connectionString_192_168_0_3 = "Database=Robno_2011;Data Source=192.168.0.3;User Id=fris_rw;Password=italija;charset=utf8";
        //        MySqlConnection connection = new MySqlConnection(connectionString_192_168_0_3);
        //        MySqlCommand command = new MySqlCommand("sp_IPadresePoslovnica", connection);
        //        command.CommandType = CommandType.StoredProcedure;
        //        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
        //        adapter.Fill(dt);
        //        connection.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message + "\n\nDošlo je do greške pri konekciji na server!!!");
        //    }
        //}

        //public static void UpdateBarKod(string robaID, string noviBarKod, string stariBarKod, int modifikator)
        //{
        //    try
        //    {
        //        Database apoteke = new Database();
        //        MySqlConnection mySQLconn = new MySqlConnection();
        //        mySQLconn = apoteke.OpenConnection();

        //        MySqlCommand command = new MySqlCommand("sp_UpdateBarKod", mySQLconn);
        //        command.CommandType = System.Data.CommandType.StoredProcedure;

        //        command.Parameters.AddWithValue("pRobaID", robaID);
        //        command.Parameters.AddWithValue("noviBarKod", noviBarKod);
        //        command.Parameters.AddWithValue("stariBarKod", stariBarKod);
        //        command.Parameters.AddWithValue("pModifikator", modifikator);
        //        command.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message + "\n\nKonekcija na server je izgubljena!!!");
        //    }
        //}

        //public static void FilldgvCentralniSifarnik(string procedura, DataTable dt, string uslov, string pUslov)
        //{
        //    try
        //    {

        //        string connectionString = "server=80.65.164.179;Database=Apoteka_2010; uid=as_mir09; password=6asp2nk;charset=utf8;Connect Timeout=15";
        //        MySqlConnection connect = new MySqlConnection(connectionString);
        //        connect.Open();

        //        MySqlCommand command = new MySqlCommand(procedura, connect);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue(pUslov, uslov);

        //        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
        //        adapter.Fill(dt);
        //        connect.Close();

        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Trenutno se ne možete uspostaviti veza sa serverom\n centralnog šifarnika na Bistriku!!!\nMolimo Vas pokušajte ponovo, u slučaju da ne funkcioniše\npozovite servisera 061/482-932");
        //    }
        //}

        //public static bool SaveReceptiZeDo(string tekuciBrojRecepta, string noviBrojRecepta, DateTime datumRacuna, int korisnik)
        //{
        //    try
        //    {
        //        Database apoteke = new Database();
        //        MySqlConnection mySQLconn = new MySqlConnection();
        //        mySQLconn = apoteke.OpenConnection();

        //        MySqlCommand command = new MySqlCommand("sp_EditReceptZeDo", mySQLconn);
        //        command.CommandType = System.Data.CommandType.StoredProcedure;

        //        command.Parameters.AddWithValue("tekuciBrojRecepta", tekuciBrojRecepta);
        //        command.Parameters.AddWithValue("noviBrojRecepta", noviBrojRecepta);
        //        command.Parameters.AddWithValue("pDatumRacuna", datumRacuna);
        //        command.Parameters.AddWithValue("pKreatorID", korisnik);
        //        command.ExecuteNonQuery();
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        
        //public static void RacuniPeriodZaPOS(DateTime datumOd, DateTime datumDo, int nacinPlacanja, DataTable table)
        //{
        //    Database apoteke = new Database();
        //    MySqlConnection mySQLconn = new MySqlConnection();
        //    mySQLconn = apoteke.OpenConnection();

        //    MySqlDataAdapter adapter = new MySqlDataAdapter("sp_BlagajnaNovaRacuniPOS", mySQLconn);
        //    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

        //    adapter.SelectCommand.Parameters.AddWithValue("pDatumOd", datumOd);
        //    adapter.SelectCommand.Parameters.AddWithValue("pDatumDo", datumDo);
        //    adapter.SelectCommand.Parameters.AddWithValue("pNacinPlacanja", nacinPlacanja);

        //    adapter.Fill(table);
        //}

        //public static void FillDataSet(DataTable dt, string procedura)
        //{
        //    try
        //    {
        //        Database apoteke = new Database();
        //        MySqlConnection mySQLconn = new MySqlConnection();
        //        mySQLconn = apoteke.OpenConnection();

        //        MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
        //        command.CommandType = CommandType.StoredProcedure;

        //        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
        //        adapter.Fill(dt);
        //        mySQLconn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //public static void FillDataSetSaUslovom(DataTable dt, string procedura, string uslov, string pUslov)
        //{
        //    try
        //    {
        //        Database apoteke = new Database();
        //        MySqlConnection mySQLconn = new MySqlConnection();
        //        mySQLconn = apoteke.OpenConnection();

        //        MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue(pUslov, uslov);

        //        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
        //        adapter.Fill(dt);
        //        mySQLconn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //public static void InsertNivelacija(DateTime datum, int poslovnica, int kreator, int brojTK)
        //{
        //    try
        //    {
        //        Database apoteke = new Database();
        //        MySqlConnection mySQLconn = new MySqlConnection();
        //        mySQLconn = apoteke.OpenConnection();

        //        MySqlCommand command = new MySqlCommand("sp_NivelacijaNova", mySQLconn);
        //        command.CommandType = System.Data.CommandType.StoredProcedure;

        //        command.Parameters.AddWithValue("pDatum", datum);
        //        command.Parameters.AddWithValue("pPoslovnaJedinica", poslovnica);
        //        command.Parameters.AddWithValue("pKreatorID", kreator);
        //        command.Parameters.AddWithValue("pBrojTK", brojTK);

        //        command.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //public static void InsertNivelacijaStavka(int poslovnica, int kreator, string nivelacijaID, decimal novaMPC, int RobaID, decimal zaliha, decimal staraMPC, decimal novaMarza)
        //{
        //    try
        //    {
        //        Database apoteke = new Database();
        //        MySqlConnection mySQLconn = new MySqlConnection();
        //        mySQLconn = apoteke.OpenConnection();

        //        MySqlCommand command = new MySqlCommand("sp_NivelacijaStavkaNova", mySQLconn);
        //        command.CommandType = System.Data.CommandType.StoredProcedure;

        //        command.Parameters.AddWithValue("pNivID", nivelacijaID);
        //        command.Parameters.AddWithValue("pPoslovnaJedinica", poslovnica);
        //        command.Parameters.AddWithValue("pKreatorID", kreator);
        //        command.Parameters.AddWithValue("pNovaMPC", novaMPC);
        //        command.Parameters.AddWithValue("pRobaID", RobaID);
        //        command.Parameters.AddWithValue("pZaliha", zaliha);
        //        command.Parameters.AddWithValue("pStaraMPC", staraMPC);
        //        command.Parameters.AddWithValue("pMarza", novaMarza);

        //        command.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //public static void DeleteSaParametrom(string procedura, string uslov, string pUslov)
        //{
        //    try
        //    {
        //        Database apoteke = new Database();
        //        MySqlConnection mySQLconn = new MySqlConnection();
        //        mySQLconn = apoteke.OpenConnection();

        //        MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue(pUslov, uslov);

        //        command.ExecuteNonQuery();
        //        mySQLconn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //public static void DeleteSaParametrom(string procedura, int uslov, string pUslov)
        //{
        //    try
        //    {
        //        Database apoteke = new Database();
        //        MySqlConnection mySQLconn = new MySqlConnection();
        //        mySQLconn = apoteke.OpenConnection();

        //        MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue(pUslov, uslov);

        //        command.ExecuteNonQuery();
        //        mySQLconn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //public static void IzvrsiProceduru(string procedura)
        //{
        //    try
        //    {
        //        Database apoteke = new Database();
        //        MySqlConnection mySQLconn = new MySqlConnection();
        //        mySQLconn = apoteke.OpenConnection();

        //        MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
        //        command.CommandType = CommandType.StoredProcedure;

        //        command.ExecuteNonQuery();
        //        mySQLconn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //public static void IzvrsiProceduru(string procedura, string uslov, string pUslov)
        //{
        //    try
        //    {
        //        Database apoteke = new Database();
        //        MySqlConnection mySQLconn = new MySqlConnection();
        //        mySQLconn = apoteke.OpenConnection();

        //        MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue(pUslov, uslov);

        //        command.ExecuteNonQuery();
        //        mySQLconn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //public static void IzvrsiProceduru(string procedura, string uslov, string pUslov, int uslov2, string pUslov2)
        //{
        //    try
        //    {
        //        Database apoteke = new Database();
        //        MySqlConnection mySQLconn = new MySqlConnection();
        //        mySQLconn = apoteke.OpenConnection();

        //        MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue(pUslov, uslov);
        //        command.Parameters.AddWithValue(pUslov2, uslov2);

        //        command.ExecuteNonQuery();
        //        mySQLconn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //public static void IzvrsiProceduru(string procedura, string uslov, string pUslov, string uslov2, string pUslov2)
        //{
        //    try
        //    {
        //        Database apoteke = new Database();
        //        MySqlConnection mySQLconn = new MySqlConnection();
        //        mySQLconn = apoteke.OpenConnection();

        //        MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue(pUslov, uslov);
        //        command.Parameters.AddWithValue(pUslov2, uslov2);

        //        command.ExecuteNonQuery();
        //        mySQLconn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //public static void IzvrsiProceduru(string procedura, DateTime uslov, string pUslov, DateTime uslov2, string pUslov2)
        //{
        //    try
        //    {
        //        Database apoteke = new Database();
        //        MySqlConnection mySQLconn = new MySqlConnection();
        //        mySQLconn = apoteke.OpenConnection();

        //        MySqlCommand command = new MySqlCommand(procedura, mySQLconn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue(pUslov, uslov);
        //        command.Parameters.AddWithValue(pUslov2, uslov2);

        //        command.ExecuteNonQuery();
        //        mySQLconn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //public static void UpisiVerzijuPrograma(string IP, string verzija, string poslovnica)
        //{
        //    try
        //    {
        //        Database apoteke = new Database();
        //        MySqlConnection mySQLconn = new MySqlConnection();
        //        mySQLconn = apoteke.OpenConnection();

        //        MySqlCommand command = new MySqlCommand("sp_VerzijaPrograma", mySQLconn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("pIP", IP);
        //        command.Parameters.AddWithValue("pVerzija", verzija);
        //        command.Parameters.AddWithValue("pPoslovnica", poslovnica);


        //        command.ExecuteNonQuery();
        //        mySQLconn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //public static bool IPadresaPoslovnice(DataTable dtPoslovnice, string poslovnica)
        //{
        //    try
        //    {
        //        string connectionString_192_168_0_3 = "Database=Robno_2011;Data Source=192.168.1.4;User Id=as_mir09;Password=6asp2nk;charset=utf8";
        //        MySqlConnection connection = new MySqlConnection(connectionString_192_168_0_3);
        //        MySqlCommand command = new MySqlCommand("sp_IPadresePoslovnicaBySifra", connection);
        //        command.CommandType = CommandType.StoredProcedure;
        //        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
        //        command.Parameters.AddWithValue("pSifra", poslovnica);
        //        adapter.Fill(dtPoslovnice);
        //        connection.Close();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message + "\n\nDošlo je do greške pri konekciji na server!!!");
        //        return false;
        //    }
        //}

    }
}
