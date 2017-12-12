using System.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Apoteke.DataObjects.Core
{
    /// <summary>
    /// UniversalComparer Class
    /// 
    /// This comparer class is capable to work with any type of object and any combination of properties, 
    /// supports both ascending and descending sorts.
    /// </summary>
    /// 
    /// <example>
    /// 
    /// - suppose we have created the following Person class:
    /// 
    ///     class Person
    ///     {
    ///         public string FirstName;
    ///         public string LastName;
    ///         public DateTime BirthDate;
    ///
    ///         public Person(string firstName, string lastName)
    ///         {
    ///             this.FirstName = firstName;
    ///             this.LastName = lastName;
    ///         }
    ///     }
    ///
    /// - sort an array of Person objects on the (LastName, FirstName) sort key:
    /// 
    ///     Person[] persons = new Person[] 
    ///     { 
    ///         new Person("John", "Smith"),
    ///         new Person("Ann", "Smith"),
    ///         new Person("John", "Douglas")
    ///     };
    /// 
    ///     UniversalComparer<Person> comp = new UniversalComparer<Person>("LastName, FirstName");
    ///     Array.Sort<Person>(persons, comp);
    /// 
    /// 
    /// - sort the persons array in descending order separately on each field:
    /// 
    ///     UniversalComparer<Person> comp = new UniversalComparer<Person>("LastName DESC, FirstName DESC");
    ///     Array.Sort<Person>(persons, comp);
    /// 
    /// </example>
    public class UniversalComparer<T> : IComparer<T>
    {
        private SortKey[] sortKeys;

        /// <summary>
        /// UniversalComparer Constructor
        /// </summary>
        /// <param name="sort"></param>
        public UniversalComparer(string sort)
        {
            Type type = typeof(T);

            // Split the list of properties.
            string[] props = sort.Split(',');

            // Prepare the array that holds information on sort criteria.
            sortKeys = new SortKey[props.Length];

            // Parse the sort string.
            for (int i = 0; i < props.Length; i++)
            {
                // Get the N-th member name.
                string memberName = props[i].Trim();
                if (memberName.ToLower().EndsWith(" desc"))
                {
                    // Discard the DESC qualifier.
                    sortKeys[i].Descending = true;
                    memberName = memberName.Remove(memberName.Length - 5).TrimEnd();
                }
                // Search for a field or a property with this name.
                sortKeys[i].FieldInfo = type.GetField(memberName);
                if (sortKeys[i].FieldInfo == null)
                {
                    sortKeys[i].PropertyInfo = type.GetProperty(memberName);
                }
            }
        }

        /// <summary>
        /// Compare Method
        /// 
        /// This procedure is invoked when comparing two objects.
        /// Implementation of IComparer.Compare.
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public int Compare(object o1, object o2)
        {
            return Compare((T)o1, (T)o2);
        }

        /// <summary>
        /// Compare Method
        /// 
        /// This procedure is invoked when comparing two objects.
        /// Implementation of IComparer<T>.Compare
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public int Compare(T o1, T o2)
        {
            // Deal with simplest cases first.
            if (o1 == null)
            {
                // Two null objects are equal.
                if (o2 == null)
                {
                    return 0;
                }

                // A null object is less than any non-null object.
                return -1;
            }
            else if (o2 == null)
            {
                // Any non-null object is greater than a null object.
                return 1;
            }

            // Iterate over all the sort keys.
            for (int i = 0; i < sortKeys.Length; i++)
            {
                object value1 = null; object value2 = null;
                SortKey sortKey = sortKeys[i];
                // Read either the field or the property.
                if (sortKey.FieldInfo != null)
                {
                    value1 = sortKey.FieldInfo.GetValue(o1);
                    value2 = sortKey.FieldInfo.GetValue(o2);
                }
                else
                {
                    value1 = sortKey.PropertyInfo.GetValue(o1, null);
                    value2 = sortKey.PropertyInfo.GetValue(o2, null);
                }

                int res = 0;
                if (value1 == null & value2 == null)
                {
                    // Two null objects are equal.
                    res = 0;
                }
                else if (value1 == null)
                {
                    // A null object is always less than a non-null object.
                    res = -1;
                }
                else if (value2 == null)
                {
                    // Any object is greater than a null object.
                    res = 1;
                }
                else
                {
                    // Compare the two values, assuming that they support IComparable.
                    res = ((IComparable)value1).CompareTo(value2);
                }

                // If values are different, return this value to caller.
                if (res != 0)
                {
                    // Negate it if sort direction is descending.
                    if (sortKey.Descending)
                    {
                        res = -res;
                    }

                    return res;
                }
            }
            // If we get here the two objects are equal.
            return 0;
        }

        /// <summary>
        /// SortKey Struct
        /// 
        /// Nested type to store detail on sort keys
        /// </summary>
        private struct SortKey
        {
            // Only one of the following fields is used.
            public FieldInfo FieldInfo;
            public PropertyInfo PropertyInfo;

            // true if sort is descending.
            public bool Descending;
        }
    }
}
