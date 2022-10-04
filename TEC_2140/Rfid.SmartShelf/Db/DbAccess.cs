using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using Vjp.Rfid.SmartShelf.Models;

namespace Vjp.Rfid.SmartShelf.Db
{

    public class DbAccess : IDbAccess , IDbConnectionFactory , IDisposable

    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //
        private MySqlConnection connect;

        //
        private MySqlCommand command;

        //
        private MySqlDataReader reader;

        //
        private static string connectionString = Properties.Resources.ConnectionString
            .Replace("@DbHost" , ConfigFile.DbHost)
            .Replace("@DbName", ConfigFile.DbName)
            .Replace("@DbUser", ConfigFile.DbUser)
            .Replace("@DbPass", ConfigFile.DbPass);

        //
        private bool bConnected = false;

        //
        private DataTable Dt;

        //
        private int affected_rows;

        //
        private string squery;

        //
        private List<string> parameters;

        public DbAccess()
        {
            Connect();
            Dt = new DataTable();
            parameters = new List<string>();
        }

        private void Connect()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                connect = connection;
                bConnected = true;
            }
            catch (MySqlException ex)
            {
                string exception = "Exception : " + ex.Message.ToString() + "\n\rApplication will close now. \n\r" + squery;
                logger.Error(exception);
                throw;                
                
            }
        }

        public static bool CheckDBConnect()
        {
            bool isConn = false;
            //MySqlConnection connection = null;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    isConn = true;
                }
                //connection = new MySqlConnection(connectionString);
                //connection.Open();
                //isConn = true;

            }
            catch (MySqlException ex)
            {
                string exception = "CheckDBConnect Exception : " + ex.Message.ToString() ;
                logger.Error(exception);
                //throw;

            }
            //finally
            //{
            //    if(connection != null){
            //        if (connection.State == ConnectionState.Open)
            //        {
            //            connection.Close();
            //        }
            //    }
            //}
            return isConn;
        }

        public IDbConnection GetConnection()
        {
            return connect;
        }

        public void CloseConn()
        {
            bConnected = false;
            connect.Close();
            connect.Dispose();
        }

        private void Init(string Query, string[] bindings = null)
        {
            // No connection with database? make connection
            if (bConnected == false)
            {
                Connect();
            }

            // Automatically disposes the MySQLcommand instance
            using (command = new MySqlCommand(Query, connect))
            {
                // 
                Bind(bindings);

                // Prevents SQL injection
                if (parameters.Count > 0)
                {
                    parameters.ForEach(delegate (string parameter)
                    {
                        string[] sparameters = parameter.ToString().Split('\x7F');
                        command.Parameters.AddWithValue(sparameters[0], sparameters[1]);
                    });
                }

                this.squery = Query.ToLower();

                if (squery.Contains("select "))
                {
                    this.Dt = ExecDatatable();
                }
                if (squery.Contains("delete ") || squery.Contains("update ") || squery.Contains("insert "))
                {
                    this.affected_rows = ExecNonquery();
                }

                logger.Info(this.squery);
                // Clear de parameters, 
                this.parameters.Clear();
            }
        }

        public void Bind(string field, string value)
        {
            parameters.Add("@" + field + "\x7F" + value);
        }


        public void Bind(string[] fields)
        {
            if (fields != null)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    Bind(fields[i], fields[i + 1]);
                    i += 1;
                }
            }
        }

        public List<string> BindLocal(string[] fields)
        {
            List<string> result = new List<string>();
            if (fields != null)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    result.Add("@" + fields[i] + "\x7F" + fields[i + 1]);
                    i += 1;
                }
            }

            return result;
        }

        // Example:
        // qBind(new string[] { "12", "John" });
        // nQuery("SELECT * FROM User WHERE ID=@0 AND Name=@1");
        /// <summary>
        /// Bind multiple fields/values without identifier.
        /// </summary>
        /// <param name="fields"></param>
        public void QueryBind(string[] fields)
        {
            if (fields != null)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    Bind(i.ToString(), fields[i]);
                }
            }
        }

        private DataTable ExecDatatable()
        {
            DataTable dt = new DataTable();
            try
            {
                reader = command.ExecuteReader();
                dt.Load(reader);
            }
            catch (MySqlException my)
            {
                string exception = "Exception : " + my.Message.ToString() + "\n\r SQL Query : \n\r" + squery;
                logger.Error(exception);
                throw;
                
            }

            return dt;
        }

        private int ExecNonquery()
        {

            int affected = 0;

            try
            {
                affected = command.ExecuteNonQuery();
            }
            catch (MySqlException my)
            {
                string exception = "Exception : " + my.Message.ToString() + "\n\r SQL Query : \n\r" + squery;

                logger.Error(exception);
                throw;
            }

            return affected;
        }

        public Task<int> ExecNonqueryAsync(string sSQL, string[] bindings = null)
        {
            return Task.Run(() =>
            {
                using (var newConnection = new MySqlConnection(connectionString))
                using (var newCommand = new MySqlCommand(sSQL, newConnection))
                {
                    List<string> _parameters = BindLocal(bindings);

                    newCommand.CommandType = CommandType.Text;

                    // Prevents SQL injection
                    if (parameters.Count > 0)
                    {
                        parameters.ForEach(delegate (string parameter)
                        {
                            string[] sparameters = parameter.ToString().Split('\x7F');
                            command.Parameters.AddWithValue(sparameters[0], sparameters[1]);
                        });
                    }

                    newConnection.Open();
                    return newCommand.ExecuteNonQuery();
                }
            });
        }

        public DataTable GetDataOfTable(string table, string[] bindings = null)
        {
            Init("SELECT * FROM " + table, bindings);
            return this.Dt;
        }

        public List<object> GetDataOfTable(string table, Type t)
        {
            return new List<object>();
        }

        public DataTable Query(string query, string[] bindings = null)
        {
            Init(query, bindings);
            return this.Dt;
        }

        public int NonQuery(string query, string[] bindings = null)
        {
            Init(query, bindings);
            return this.affected_rows;
        }

        public string Single(string query, string[] bindings = null)
        {
            Init(query, bindings);

            if (Dt.Rows.Count > 0)
            {
                return Dt.Rows[0][0].ToString();
            }

            return string.Empty;
        }

        public string[] Row(string query, string[] bindings = null)
        {
            Init(query, bindings);

            string[] row = new string[Dt.Columns.Count];

            if (Dt.Rows.Count > 0)
                for (int i = 0; i++ < Dt.Columns.Count; row[i - 1] = Dt.Rows[0][i - 1].ToString()) ;

            return row;
        }

        public List<string> Column(string query, string[] bindings = null)
        {
            Init(query, bindings);

            List<string> column = new List<string>();

            for (int i = 0; i++ < Dt.Rows.Count; column.Add(Dt.Rows[i - 1][0].ToString())) ;

            return column;
        }

        public void Dispose()
        {
            CloseConn();
        }
    }
    
}
