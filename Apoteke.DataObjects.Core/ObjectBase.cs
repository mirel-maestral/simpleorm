using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace Apoteke.DataObjects.Core
{
    public class ObjectBase<T>
    {
        #region Fields and Properties
        private int _id = 0;

        [ObjectProperty("pID","ID")]
        public int ID
        {
            get{return _id;}
            set { _id = value; }
        }

        [ObjectProperty("pVrijemeKreiranja", "VrijemeKreiranja")]
        public DateTime VrijemeKreiranja
        {
            get;
            set;
        }

        [ObjectProperty("pVrijemeModifikacije", "VrijemeModifikacije")]
        public DateTime VrijemeModificiranja
        {
            get;
            set;
        }

        [ObjectProperty("pKreatorID", "KreatorID")]
        public int KreatorID
        {
            get;
            set;
        }

        [ObjectProperty("pModifikatorID", "ModifikatorID")]
        public int ModifikatorID
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

        public ObjectState State
        {
            get;
            set;
        }

        public ObjectBase(int id)
        {
            _id = id;
            this.State = ObjectState.New;
        }
        #endregion

        #region Methods
        public virtual void Save()
        {
            Type objectType = typeof(T);

            object[] objectBindingattribs = objectType.GetCustomAttributes(typeof(ObjectBindingAttribute), true);
            if (objectBindingattribs == null)
                return;

            ObjectBindingAttribute objectBindingAttrib = (ObjectBindingAttribute)objectBindingattribs[0];

            try
            {
                if (this.State == ObjectState.Existing)
                {
                    MySqlCommand updateCommand = PrepeareCommand(objectBindingAttrib.UpdateStoredProcedure);

                    //if (updateCommand.Parameters.Contains("@pVrijemeModifikacije"))
                    //    updateCommand.Parameters["@pVrijemeModifikacije"].Value = DateTime.Now;

                    updateCommand.Connection.Open();
                    updateCommand.ExecuteNonQuery();
                    if ( (int)updateCommand.Parameters["@pError"].Value == -1 )
                        throw new SaveFailedException(String.Format("Someone has changed data already."));

                    if (updateCommand.Parameters.Contains("@pVrijemeModifikacije"))
                        this.VrijemeModificiranja =DateTime.Parse(updateCommand.Parameters["@pVrijemeModifikacije"].Value.ToString());

                    updateCommand.Connection.Close();

                }
                else if (this.State == ObjectState.New)
                {
                    MySqlCommand insertCommand = PrepeareCommand(objectBindingAttrib.InsertStoredProcedure);

                    insertCommand.Connection.Open();
                    insertCommand.ExecuteNonQuery();
                    insertCommand.Connection.Close();

                    PropertyInfo prop = objectType.GetProperty("ID");
                    object[] fieldAttribs = prop.GetCustomAttributes(typeof(ObjectPropertyAttribute), true);
                    if (fieldAttribs == null || fieldAttribs.Length == 0)
                        throw new ApplicationException("Field binding attribute on " + prop.Name + " is missing.");

                    ObjectPropertyAttribute fieldAttrib = (ObjectPropertyAttribute)fieldAttribs[0];

                    _id = (int)insertCommand.Parameters["@" + fieldAttrib.ParameterName].Value;
                    if (insertCommand.Parameters.Contains("@pVrijemeKreiranja"))
                        this.VrijemeKreiranja = DateTime.Parse(insertCommand.Parameters["@pVrijemeKreiranja"].Value.ToString());

                    this.State = ObjectState.Existing;
                }
            }
            catch(System.Exception ex)
            {
                Logging.Log.Create("Error in the ObjectBase Save method.", Logging.LogEntryLevel.Critical, ex);
                throw;
                //throw new SaveFailedException(String.Format("Error while trying to save data for object:{0} of state:{1}",
                //    objectType.Name, this.State.ToString()), ex);
            }

        }

        public virtual void Delete()
        {
            Type objectType = typeof(T);

            object[] objectBindingattribs = objectType.GetCustomAttributes(typeof(ObjectBindingAttribute), true);
            if (objectBindingattribs == null)
                return;

            ObjectBindingAttribute objectBindingAttrib = (ObjectBindingAttribute)objectBindingattribs[0];

            MySqlCommand updateCommand = PrepeareCommand(objectBindingAttrib.DeleteStoredProcedure);

            updateCommand.Connection.Open();
            updateCommand.ExecuteNonQuery();
            updateCommand.Connection.Close();
        }

        private MySqlCommand PrepeareCommand( string procedureName)
        {
            Database db = new Database();
            MySqlCommand command = null;

            using (MySqlConnection con = db.OpenConnection())
            {

                command = new MySqlCommand(procedureName, con);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlCommandBuilder.DeriveParameters(command);
                
            }

            Type objectType = typeof(T);

            PropertyInfo[] objectProps = objectType.GetProperties();
            if (objectProps == null)
                return null;

            foreach (PropertyInfo prop in objectProps)
            {
                object[] fieldAttribs = prop.GetCustomAttributes(typeof(ObjectPropertyAttribute), true);
                if (fieldAttribs == null || fieldAttribs.Length == 0)
                    continue;

                ObjectPropertyAttribute fieldAttrib = (ObjectPropertyAttribute)fieldAttribs[0];

                if (command.Parameters.Contains("@"+ fieldAttrib.ParameterName))
                    command.Parameters["@" + fieldAttrib.ParameterName].Value = prop.GetValue(this, null);

            }

            return command;
        }
        #endregion

    }
}
