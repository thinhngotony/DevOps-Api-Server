using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Vjp.Rfid.SmartShelf.Db
{
    public interface IDbAccess
    {
        void CloseConn();
        void Bind(string field, string value);
        void Bind(string[] fields);
        void QueryBind(string[] fields);
        DataTable GetDataOfTable(string table, string[] bindings = null);
        List<object> GetDataOfTable(string table, Type t);
        DataTable Query(string query, string[] bindings = null);

        int NonQuery(string query, string[] bindings = null);
        string Single(string query, string[] bindings = null);
        string[] Row(string query, string[] bindings = null);
        List<string> Column(string query, string[] bindings = null);

        Task<int> ExecNonqueryAsync(string sSQL, string[] bindings = null);

    }
}
