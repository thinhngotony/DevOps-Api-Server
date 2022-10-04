using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjp.Rfid.SmartShelf.Helper
{
    public static class DataRowExtensions
    {
        public static T FieldOrDefaultValue<T>(this DataRow row , string columnName)
        {
            return row.IsNull(columnName) ? default (T) : row.Field<T>(columnName);
        }
    }
}
