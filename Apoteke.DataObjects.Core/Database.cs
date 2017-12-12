using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Configuration;
using Logging;
using System.Data;
using System.Net;

namespace Apoteke.DataObjects.Core
{
    
    public class Database
    {
        protected delegate T CreateObjectFromReaderMethod<T>(MySqlDataReader dr);

        #region Properties
        /// <summary>
        /// Server Name
        /// </summary>
        private string _server = String.Empty;
        public string Server
        {
            get { return _server; }
        }

        /// <summary>
        /// Naziv baze.
        /// </summary>
        private string _database = String.Empty;
        public string DatabaseName
        {
            get { return _database; }
        }

        /// <summary>
        /// Korisničko ime.
        /// </summary>
        private string _username = String.Empty;
        public string UserName
        {
            get { return _username; }
        }

        /// <summary>
        /// Lozinka.
        /// </summary>
        private string _password = String.Empty;
        public string Password
        {
            get { return _password; }
        }
        
        /// <summary>
        /// Database Connection String
        /// </summary>
        public string ConnectionString
        {
            get;
            set;
        }
        #endregion

        public Database()
        {
            // Citanje Connection stringa iz config fajla
            if (GlobalVariable.connectionStringID == 0)
            {
                GlobalVariable.connectionString = "server=" + GlobalVariable.IPjavna + ";Database =" + GlobalVariable.serverBaza + GlobalVariable.godina + "; uid=as_mir09; password= 6asp2nk;charset=utf8;Connect Timeout=5";
                GlobalVariable.startFormText = " " + GlobalVariable.apoteka + " - " + GlobalVariable.IPjavna;
                GlobalVariable.IPcompa = false;
            }
            if (GlobalVariable.connectionStringID == 1)
            {
                GlobalVariable.connectionString = "server=192.168.1.4;Database = Apoteka_2009; uid=as_mir09; password= 6asp2nk;charset=utf8;Connect Timeout=5";
                GlobalVariable.startFormText = " 2009";
                GlobalVariable.godina = "2009";
                GlobalVariable.IPcompa = false;
            }
            if (GlobalVariable.connectionStringID == 2)
            {
                GlobalVariable.connectionString = "server=192.168.1.4;Database = Apoteka_2010; uid=as_mir09; password= 6asp2nk;charset=utf8;Connect Timeout=5";
                GlobalVariable.startFormText = " 2010";
                GlobalVariable.godina = "2010";
                GlobalVariable.IPcompa = false;
           
            }
            if (GlobalVariable.connectionStringID == 3)
            {
                GlobalVariable.connectionString = "server=192.168.1.4;Database = Apoteka_2011; uid=as_mir09; password= 6asp2nk;charset=utf8;Connect Timeout=5";
                GlobalVariable.startFormText = " 2011";                
                GlobalVariable.godina = "2011";                
                GlobalVariable.IPcompa = false;
            }
            if (GlobalVariable.connectionStringID == 4)
            {
                GlobalVariable.connectionString = "server=192.168.1.4;Database = Apoteka_2012; uid=as_mir09; password= 6asp2nk;charset=utf8;Connect Timeout=5";
                GlobalVariable.startFormText = " 2012";
                GlobalVariable.godina = "2012";
                GlobalVariable.IPcompa = false;
            }

            if (GlobalVariable.connectionStringID == 5)
            {
                GlobalVariable.connectionString = "server=192.168.1.4;Database = Apoteka_2013; uid=as_mir09; password= 6asp2nk;charset=utf8;Connect Timeout=5";
                GlobalVariable.startFormText = " 2013";
                GlobalVariable.godina = "2013";
                GlobalVariable.IPcompa = false;
            }
            if (GlobalVariable.connectionStringID == 6)
            {
                GlobalVariable.connectionString = "server=192.168.1.4;Database = Apoteka_2014; uid=as_mir09; password= 6asp2nk;charset=utf8;Connect Timeout=5";
                GlobalVariable.startFormText = " 2014";
                GlobalVariable.godina = "2014";
                GlobalVariable.IPcompa = false;
            }

            if (GlobalVariable.connectionStringID == 7)
            {
                GlobalVariable.connectionString = "server=192.168.1.4;Database = Apoteka_2015; uid=as_mir09; password= 6asp2nk;charset=utf8;Connect Timeout=5";
                GlobalVariable.startFormText = " 2015";
                GlobalVariable.godina = "2015";
                GlobalVariable.IPcompa = false;
            }

            if (GlobalVariable.connectionStringID == 8)
            {
                GlobalVariable.connectionString = "server=192.168.1.4;Database = Apoteka_2016; uid=as_mir09; password= 6asp2nk;charset=utf8;Connect Timeout=5";
                GlobalVariable.startFormText = " 2016";
                GlobalVariable.godina = "2016";
                GlobalVariable.IPcompa = false;
            }

            if (GlobalVariable.connectionStringID == 9)
            {
                GlobalVariable.connectionString = "server=192.168.1.4;Database = Apoteka_2017; uid=as_mir09; password= 6asp2nk;charset=utf8;Connect Timeout=5";
                GlobalVariable.startFormText = " 2017";
                GlobalVariable.godina = "2017";
                GlobalVariable.IPcompa = false;
            }




            if (String.IsNullOrEmpty(GlobalVariable.connectionString))
                throw new ApplicationException("Connection string has not been set.");

        }

        public bool CheckConnection()
        {
            using (MySqlConnection con = this.OpenConnection())
            {
                if (con == null)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Kreira novu konekcija i otvara je.
        /// </summary>
        /// <returns></returns>
        public MySqlConnection OpenConnection()
        {
            MySqlConnection con = null;
            

            // Inicijalizacija konekcije
            try
            {
                if (String.IsNullOrEmpty(GlobalVariable.connectionString))
                    return null;

                con = new MySqlConnection(GlobalVariable.connectionString);
                con.Open();
                return con;
            }
            catch (Exception ex)
            {
                //Log.Create("Greska prilikom otvaranja konekacije!", LogEntryLevel.Critical, ex);
            }

            return null;
        }

        #region Methods

        /// <summary>
        /// Load objects of type T using reflection.
        /// </summary>
        /// <typeparam name="T">Type of objects to return.</typeparam>
        /// <returns>List of objects of type T</returns>
        protected virtual List<T> GetObjectAll<T>()
        {
            Type objectType = typeof(T);

            object[] objectBindingattribs = objectType.GetCustomAttributes(typeof(ObjectBindingAttribute), true);
            if (objectBindingattribs == null)
                throw new ApplicationException("ObjectBindingAttribute is not defined for type " + objectType.Name + ".");

            ObjectBindingAttribute objectBindingAttrib = objectBindingattribs[0] as ObjectBindingAttribute;
            if (objectBindingAttrib == null)
                throw new ApplicationException("ObjectBindingAttribute cast error for type " + objectType.Name + ".");

            List<T> objList = new List<T>();

            try
            {
                using (MySqlConnection con = this.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand(objectBindingAttrib.SelectAllStoredProcedure, con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlDataReader dr = command.ExecuteReader();

                    if (dr == null)
                        return null;
                    while (dr.Read())
                        objList.Add(ObjectFactory.CreateObject<T>(dr));
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Create("Greska prilikom pozivanja GetObjectAll<T>() metode!", LogEntryLevel.Critical, ex);
            }

            return objList;
        }

        /// <summary>
        /// Load object of type T from database.
        /// </summary>
        /// <typeparam name="T">Type of object to return.</typeparam>
        /// <param name="Id">unique identifier or primary key value</param>
        /// <returns>Object of type T</returns>
        protected virtual T GetObjectByID<T>(int Id)
        {
            Type objectType = typeof(T);

            object[] objectBindingattribs = objectType.GetCustomAttributes(typeof(ObjectBindingAttribute), true);
            if (objectBindingattribs == null)
                throw new ApplicationException("ObjectBindingAttribute is not defined for type " + objectType.Name + ".");

            ObjectBindingAttribute objectBindingAttrib = objectBindingattribs[0] as ObjectBindingAttribute;
            if (objectBindingAttrib == null)
                throw new ApplicationException("ObjectBindingAttribute cast error for type " + objectType.Name + ".");

            T obj = default(T);

            try
            {
                using (MySqlConnection con = this.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand(objectBindingAttrib.SelectByIDStoredProcedure, con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("@pID", MySqlDbType.Int32)).Value = Id;
                    MySqlDataReader dr = command.ExecuteReader(System.Data.CommandBehavior.SingleRow);

                    if (dr == null)
                        return obj;
                    dr.Read();
                    obj = ObjectFactory.CreateObject<T>(dr);
                   
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Create("Greska prilikom pozivanja GetObjectAll<T>() metode!", LogEntryLevel.Critical, ex);
            }

            return obj;
        }

        private void ParseConnectionString()
        {
            if (String.IsNullOrEmpty(ConnectionString))
                return;

             MySqlConnectionStringBuilder conStringBuilder = new MySqlConnectionStringBuilder(ConnectionString);
            _server = conStringBuilder.Server;
            _database = conStringBuilder.Database;
            _password = conStringBuilder.Password;
            _username = conStringBuilder.UserID;
        }

        /// <summary>
        /// Load objects from database using MySqlDataReader.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="useReflection">If true we are going to use reflection while creating objects.</param>
        /// <param name="procedureName">Stored Procedure name.</param>
        /// <param name="method">Method which should create object of type T from data reader.</param>
        /// <returns>List of objects.</returns>
        protected virtual List<T> GetObjectAll<T>(string procedureName, CreateObjectFromReaderMethod<T> method)
        {
            List<T> objList = new List<T>();
            try
            {
                using (MySqlConnection con = this.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand(procedureName, con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        T obj = method(dr);
                        if (obj != null)
                            objList.Add(obj);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška prilikom učitavanja objekta " + typeof(T).Name,
                    Logging.LogEntryLevel.Critical, ex);
            }

            return objList;
        }

        protected virtual T GetObjectByID<T>(int Id, string procedureName, CreateObjectFromReaderMethod<T> method)
        {
            try
            {
                using (MySqlConnection con = this.OpenConnection())
                {
                    MySqlCommand command = new MySqlCommand(procedureName, con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("pID", MySqlDbType.Int32)).Value = Id;
                    MySqlDataReader dr = command.ExecuteReader(CommandBehavior.SingleRow);
                    if (!dr.HasRows)
                        return default(T);
                    dr.Read();
                    return method(dr);
                    
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greška prilikom učitavanja objekta " + typeof(T).Name,
                    Logging.LogEntryLevel.Critical, ex);
            }

            return default(T);
        }
       
        /// <summary>
        /// Fill table with data from database.
        /// </summary>
        /// <param name="procedureName">Stored procedure for use to pull data.</param>
        /// <param name="dt">DataTable to fill.</param>
        /// <param name="parameters">Stored Prc's parameters</param>
        public virtual void FillTable(string procedureName, DataTable dt, Dictionary<string, object> parameters)
        {
            MySqlCommand command = null;

            try
            {
                using (MySqlConnection con = this.OpenConnection())
                {
                    command = new MySqlCommand(procedureName, con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (parameters != null && parameters.Count > 0)
                    {
                        MySqlCommandBuilder.DeriveParameters(command);
                        foreach (System.Collections.Generic.KeyValuePair<string, object> par in parameters)
                        {
                            if (String.IsNullOrEmpty(par.Key))
                                continue;
                            string parameterName = "@" + par.Key;
                            if (command.Parameters.Contains(parameterName))
                                command.Parameters[parameterName].Value = par.Value;
                        }
                    }

                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    command.CommandTimeout = 3600;
                    da.Fill(dt);
                }
            }
            catch (System.Exception ex)
            {
                Logging.Log.Create("Greska u FillTable methodi.",
                    Logging.LogEntryLevel.Critical, ex);
            }
        }
        #endregion
    }
}
