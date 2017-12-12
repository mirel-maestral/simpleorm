using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apoteke.DataObjects.Core
{
    public class ObjectBindingAttribute: System.Attribute
    {
        #region Properties
        public string InsertStoredProcedure
        {
            get;
            set;
        }

        public string DeleteStoredProcedure
        {
            get;
            set;
        }

        public string UpdateStoredProcedure
        {
            get;
            set;
        }

        public string SelectAllStoredProcedure
        {
            get;
            set;
        }

        public string SelectByIDStoredProcedure
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public ObjectBindingAttribute(string insertStoredProcName, string updateStoredProcName,
                                        string deleteStoredProcName, string selectAllStoredProcName, string selectByIDStoredProcName)
        {
            InsertStoredProcedure = insertStoredProcName;
            UpdateStoredProcedure = updateStoredProcName;
            DeleteStoredProcedure = deleteStoredProcName;
            SelectByIDStoredProcedure = selectByIDStoredProcName;
            SelectAllStoredProcedure = selectAllStoredProcName;
           
        }
        #endregion
    }
}
