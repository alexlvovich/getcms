using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GetCms.DataAccess.SqlServer.Extentions
{
    public static class DataReaderExtentions
    {
        public static T Value<T>(this IDataReader row, string columnName, T defaultValue = default(T))
        {
            object o = row[columnName];
            if (o is T) return (T)o;
            return defaultValue;
        }
    }
}
