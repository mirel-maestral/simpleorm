using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Apoteke.DataObjects.Core
{
    public class ObjectFactory
    {
        public ObjectFactory()
        { }

        public static T CreateObject<T>(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            T newObject = default(T);

            if (dr == null)
                return newObject;

            try
            {
                newObject = Activator.CreateInstance<T>();
                Type newObjectType = typeof(T);
                PropertyInfo[] newObjectProperties = newObjectType.GetProperties();
                foreach (PropertyInfo property in newObjectProperties)
                {
                    if ( property.Name == "State")
                        property.SetValue(newObject, ObjectState.Existing, null);
                    object[] attribs = property.GetCustomAttributes(typeof(ObjectPropertyAttribute), true);
                    if (attribs == null || attribs.Length != 1)
                        continue;
                    ObjectPropertyAttribute attrib = attribs[0] as ObjectPropertyAttribute;
                    if (attrib == null)
                        continue;

                    try
                    {
                        if (property.CanWrite && !dr.IsDBNull(dr.GetOrdinal(attrib.FieldName)))
                            if (typeof(System.Char) == property.PropertyType)
                            {
                                Char ch = ' ';
                                if (!String.IsNullOrEmpty(dr[attrib.FieldName].ToString()))
                                    ch = Char.Parse(dr[attrib.FieldName].ToString().Substring(0, 1));

                                property.SetValue(newObject, ch , null);
                            }
                            else
                                property.SetValue(newObject, dr[attrib.FieldName], null);
                    }
                    catch( System.Exception ex)
                    {
                        Logging.Log.Create(String.Format("Nemoguce je setovati {0} polje na objektu {1}",attrib.FieldName, newObjectType.Name) ,
                            Logging.LogEntryLevel.Critical, ex);
                    }
                }
            }
            catch(System.Exception ex)
            {
                Logging.Log.Create("Greska unutar Factory.CreateObject methode.",
                    Logging.LogEntryLevel.Critical, ex);
            }

            return newObject;
        }

        public static void SetBaseProperties<T>(ObjectBase<T> obj, MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            obj.KreatorID = dr["KreatorID"].GetType() == typeof(System.DBNull) ? 0 : dr.GetInt32("KreatorID");
            obj.ModifikatorID = dr["ModifikatorID"].GetType() == typeof(System.DBNull) ? 0 : dr.GetInt32("ModifikatorID");
            if (!dr.IsDBNull(dr.GetOrdinal("VrijemeModifikacije")))
                obj.VrijemeModificiranja = dr.GetDateTime("VrijemeModifikacije");
            if (!dr.IsDBNull(dr.GetOrdinal("VrijemeKreiranja")))
                obj.VrijemeKreiranja = dr.GetDateTime("VrijemeKreiranja");
            obj.State = ObjectState.Existing;
            
        }
    }
}
