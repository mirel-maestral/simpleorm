using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apoteke.DataObjects.Core
{
    public class ObjectPropertyAttribute: System.Attribute
    {
        public string ParameterName
        {
            get;
            set;
        }

        public string FieldName
        {
            get;
            set;
        }

        public ObjectPropertyAttribute(string parameterName, string fieldName):base()
        {
            ParameterName = parameterName;
            FieldName = fieldName;
        }
    }
}
