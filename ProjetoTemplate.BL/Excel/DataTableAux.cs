using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ProjetoTemplate.BL.Excel
{
    public class DataTableAux
    {
        public DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                if (info.CustomAttributes.Count() > 0)
                {
                    if (info.GetCustomAttributes(typeof(DisplayNameAttribute), true).Count() > 0)
                        dt.Columns.Add(new DataColumn(info.Name, GetNullableType(info.PropertyType)));
                }
            }

            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                var index = 0;
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    if (info.CustomAttributes.Count() > 0)
                    {
                        if (info.GetCustomAttributes(typeof(DisplayNameAttribute), true).Count() > 0)
                        {
                            if (!IsNullableType(info.PropertyType))
                            {
                                row[info.Name] = info.GetValue(t, null);
                            }
                            else
                                row[info.Name] = (info.GetValue(t, null) ?? DBNull.Value);
                        }

                        index++;
                    }
                }
                dt.Rows.Add(row);
            }

            return dt;
        }

        public Type GetNullableType(Type t)
        {
            Type returnType = t;
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                returnType = Nullable.GetUnderlyingType(t);
            }
            return returnType;
        }

        public bool IsNullableType(Type type)
        {
            return (type == typeof(string) ||
                    type.IsArray ||
                    (type.IsGenericType &&
                     type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
        }
    }
}
