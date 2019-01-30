using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GetCms.DataAccess.SqlServer
{
    public class BaseDataAccess
    {
        #region CONSTS
        public readonly ILogger _logger;
        public const int MIN_POOL_CONNECTIONS = 3;
        public const int MAX_POOL_CONNECTIONS = 5;
        private static readonly TimeSpan DAL_INIT_RETRY_INTERVAL = TimeSpan.FromSeconds(15);

        #endregion CONSTS

        #region STATIC MEMBERS
        private string _connectionString = null;
        private Hashtable m_StubTable = new Hashtable();
        private bool disposed = false; // to detect redundant calls

        #endregion STATIC MEMBERS

        #region .CTOR

        public BaseDataAccess(string connectionString, ILoggerFactory loggerFactory)
        {
            this._connectionString = connectionString;
            this._logger = loggerFactory.CreateLogger(this.GetType().Name);
        }


        #endregion CTOR

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        #region GetConnection

        public SqlParameter[] RowToParam(DataRow row, ArrayList param)
        {
            try
            {
                ArrayList result = new ArrayList(param.Count);
                foreach (string paramName in param)
                {
                    object val = null;
                    string propertyName = paramName.Substring(1);
                    if (row.Table.Columns.Contains(propertyName) == false || row.IsNull(propertyName) == false)
                    {
                        PropertyInfo pi = row.GetType().GetProperty(propertyName);
                        val = pi.GetValue(row, null); ;
                        if (val == DBNull.Value)
                            val = null;
                    }
                    result.Add(CreateSqlParameter(paramName, val));
                }
                return (SqlParameter[])result.ToArray(typeof(SqlParameter));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private static SqlParameter[] GetSqlParameters(ArrayList parameters)
        {
            if (parameters == null)
                return null;

            int index = 0;
            SqlParameter[] sqlParameters = new SqlParameter[parameters.Count];
            foreach (SqlParameter parameter in parameters)
                sqlParameters[index++] = parameter;

            return sqlParameters;
        }
        #endregion GetConnection


        #region Execute
        public void ExecuteDataTable(string commandText, SqlParameter[] parameters, DataTable table)
        {
            ExecuteDataTable(commandText, parameters, table, out m_StubTable, false);
        }
        public void ExecuteDataTable(string commandText, SqlParameter[] parameters, DataTable table, out Hashtable output)
        {
            ExecuteDataTable(commandText, parameters, table, out output, true);
        }
        private void ExecuteDataTable(string commandText, SqlParameter[] parameters, DataTable table, out Hashtable output, bool returnOutput)
        {
            SqlCommand cmd = null;
            SqlDataAdapter adapter = null;
            DataTable dt = null;
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(_connectionString);
                cmd = GetSqlCommand(commandText, parameters, connection);
                adapter = new SqlDataAdapter(cmd);

                dt = table;
                adapter.Fill(dt);
                if (returnOutput)
                    output = AssignOutputParameters(parameters);
                else
                    output = null;
            }
            catch (SqlException e)
            {
                _logger.LogError($"ExecuteDataTable error: {e.Message}, stack: {e.StackTrace}");
                if (dt != null)
                {
                    try
                    {
                        dt.Dispose();
                    }
                    catch (Exception ee)
                    {
                        _logger.LogError($"ExecuteDataTable dispose error: {ee.Message}, stack: {ee.StackTrace}");
                        throw;
                    }
                    finally
                    {
                        dt = null;
                    }
                }

                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"ExecuteDataTable error: {e.Message}, stack: {e.StackTrace}");
                if (dt != null)
                {
                    try
                    {
                        dt.Dispose();
                    }
                    catch (Exception ee)
                    {
                        _logger.LogError($"ExecuteDataTable dispose error: {ee.Message}, stack: {ee.StackTrace}");
                        throw;
                    }
                    finally
                    {
                        dt = null;
                    }
                }


                throw;
            }
            finally
            {
                if (adapter != null)
                {
                    adapter.Dispose();
                    adapter = null;
                }
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    connection.Dispose();
                    connection = null;
                }
            }
        }

        public object ExecuteScalar(string commandText, SqlParameter[] parameters)
        {
            return ExecuteScalar(commandText, parameters, out m_StubTable, false);
        }
        public object ExecuteScalar(string commandText, SqlParameter[] parameters, out Hashtable output)
        {
            return ExecuteScalar(commandText, parameters, out output, true);
        }
        private object ExecuteScalar(string commandText, SqlParameter[] parameters, out Hashtable output, bool returnOutput)
        {
            SqlCommand cmd = null;
            SqlConnection connection = null;
            try
            {
                object result;
                connection = new SqlConnection(_connectionString);
                connection.Open();
                cmd = GetSqlCommand(commandText, parameters, connection);
                result = cmd.ExecuteScalar();
                if (returnOutput)
                    output = AssignOutputParameters(parameters);
                else
                    output = null;
                return result;
            }
            catch (SqlException e)
            {
                _logger.LogError($"ExecuteScalar error: {e.Message}, stack: {e.StackTrace}");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"ExecuteScalar error: {e.Message}, stack: {e.StackTrace}");
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    connection.Dispose();
                    connection = null;
                }
            }
        }
        public void ExecuteDataset(string commandText, SqlParameter[] parameters, DataSet ds)
        {
            ExecuteDataset(commandText, parameters, ds, out m_StubTable, false);
        }
        public void ExecuteDataset(string commandText, SqlParameter[] parameters, DataSet ds, out Hashtable output)
        {
            ExecuteDataset(commandText, parameters, ds, out output, true);
        }
        private void ExecuteDataset(string commandText, SqlParameter[] parameters, DataSet ds, out Hashtable output, bool returnOutput)
        {
            SqlCommand cmd = null;
            SqlDataAdapter adapter = null;
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                cmd = GetSqlCommand(commandText, parameters, connection);
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                if (returnOutput)
                    output = AssignOutputParameters(parameters);
                else
                    output = null;
            }
            catch (SqlException e)
            {
                _logger.LogError($"ExecuteDataset error: {e.Message}, stack: {e.StackTrace}");
                if (ds != null)
                {
                    try
                    {
                        ds.Dispose();
                    }
                    catch (Exception ee)
                    {
                        _logger.LogError($"ExecuteDataset dispose error: {ee.Message}, stack: {ee.StackTrace}");
                        throw;
                    }
                    finally
                    {
                        ds = null;
                    }
                }
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"ExecuteDataset error: {e.Message}, stack: {e.StackTrace}");
                if (ds != null)
                {
                    try
                    {
                        ds.Dispose();
                    }
                    catch (Exception ee)
                    {
                        _logger.LogError($"ExecuteDataset dispose error: {ee.Message}, stack: {ee.StackTrace}");
                        throw;
                    }
                    finally
                    {
                        ds = null;
                    }
                }
                throw;
            }
            finally
            {
                if (adapter != null)
                {
                    adapter.Dispose();
                    adapter = null;
                }
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    connection.Dispose();
                    connection = null;
                }
            }
        }

        public int ExecuteNonQuery(string commandText, SqlParameter[] parameters)
        {
            return ExecuteNonQuery(commandText, parameters, out m_StubTable, false);
        }
        public int ExecuteNonQuery(string commandText, SqlParameter[] parameters, out Hashtable output)
        {
            return ExecuteNonQuery(commandText, parameters, out output, true);
        }
        public int ExecuteNonQuery(string commandText, SqlParameter[] parameters, out Hashtable output, bool returnOutput)
        {
            int valueToReturn;
            SqlCommand cmd = null;
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();
                cmd = GetSqlCommand(commandText, parameters, connection);
                valueToReturn = cmd.ExecuteNonQuery();
                if (returnOutput)
                    output = AssignOutputParameters(parameters);
                else
                    output = null;


            }
            catch (SqlException e)
            {
                _logger.LogError($"ExecuteNonQuery error: {e.Message}, stack: {e.StackTrace}");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"ExecuteNonQuery error: {e.Message}, stack: {e.StackTrace}");
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    connection.Dispose();
                    connection = null;
                }
            }
            return valueToReturn;
        }

        public async Task<int> ExecuteNonQueryAsync(string commandText, SqlParameter[] parameters)
        {
            int valueToReturn;
            SqlCommand cmd = null;
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();
                cmd = GetSqlCommand(commandText, parameters, connection);
                valueToReturn = await cmd.ExecuteNonQueryAsync();
                //if (returnOutput)
                //    output = AssignOutputParameters(parameters);
                //else
                //    output = null;


            }
            catch (SqlException e)
            {
                _logger.LogError($"ExecuteNonQueryAsync error: {e.Message}, stack: {e.StackTrace}");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"ExecuteNonQueryAsync error: {e.Message}, stack: {e.StackTrace}");
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    connection.Dispose();
                    connection = null;
                }
            }
            return valueToReturn;
        }

        //public async Task<IEnumerable<T>> ExecuteReaderAsync<T>(Func<IDataReader, IEnumerable<T>> reader, string usp, params SqlParameter[] parameters)
        //{
        //    return await ExecuteCommand(async command => reader(await command.ExecuteReaderAsync()).ToArray(), usp, parameters);
        //}

        ///// <summary>
        ///// Executes commands asynchronously to the database.
        ///// This method is generic enough to be used for any command.
        ///// </summary>
        //private async Task<T> ExecuteCommand<T>(Func<SqlCommand, Task<T>> execute, string usp, params SqlParameter[] parameters)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        using (var command = new SqlCommand(usp, connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddRange(parameters);

        //            await connection.OpenAsync();
        //            return await execute(command);
        //        }
        //    }
        //}
        public async Task<SqlDataReader> ExecuteDataReaderAsync(string commandText, SqlParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = GetSqlCommand(commandText, parameters, connection);

            return await cmd.ExecuteReaderAsync();
        }
        private int ms_nCommandTimeOut = 30;

        /// <summary>
        /// Gets or sets the wait time(in seconds) before terminating the attempt to execute a command and generating an error.
        /// The default is 30 second. Can not be changed in run time you should set the value before constructing the class.
        /// </summary>
        public int DbCommandTimeOut
        {
            get
            {
                return ms_nCommandTimeOut;
            }
            set
            {
                ms_nCommandTimeOut = value;
            }
        }

        private void AddParameters(SqlCommand cmd, SqlParameter[] parameters)
        {
            if (parameters != null)
                foreach (SqlParameter parameter in parameters)
                    if (!cmd.Parameters.Contains(parameter.ParameterName))
                        cmd.Parameters.Add(parameter);
        }
        private Hashtable AssignOutputParameters(SqlParameter[] parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters", "You can not ask for ouput parameters because they was not supplied at all");
            else
            {
                Hashtable result = new Hashtable();
                foreach (SqlParameter p in parameters)
                    if (p.Direction == ParameterDirection.Output)
                        if (p.Value == DBNull.Value)
                            result.Add(p.ParameterName, null);
                        else
                            result.Add(p.ParameterName, p.Value);
                return result;
            }
        }
        public static SqlParameter CreateSqlParameter(string name, object _value)
        {
            return new SqlParameter(name, CheckNull(_value));
        }

        public SqlCommand GetSqlCommand(string commandText, SqlParameter[] parameters, SqlConnection connection)
        {


            SqlCommand cmd = new SqlCommand(commandText);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = DbCommandTimeOut;
            cmd.Connection = connection;
            AddParameters(cmd, parameters);
            return cmd;
        }
        #endregion Execute

        #region Private conversion helpers
        public static string ArrayToCsvString(Array arr)
        {
            if (arr == null || arr.Length == 0)
                return null;
            StringBuilder sbTypeBuilder = new StringBuilder();
            bool bFirst = true;
            foreach (object obj in arr)
            {
                if (!bFirst) sbTypeBuilder.Append(",");
                sbTypeBuilder.Append(obj.ToString());
                bFirst = false;
            }

            return sbTypeBuilder.ToString();
        }

        // was IsNull (nivs)
        public static object GetValueOrNull(object source)
        {
            // regular null
            if (source == null)
                return DBNull.Value;

            // empty array
            if ((source.GetType().IsArray) && ((Array)source).Length == 0)
                return DBNull.Value;

            // empty string
            if ((source is string) && (((string)source).Length == 0))
                return DBNull.Value;

            // minimal DateTime
            if ((source is DateTime) && DateTime.MinValue.Equals(source))
                return DBNull.Value;

            // empty Guid
            if ((source is Guid) && Guid.Empty.Equals(source))
                return DBNull.Value;

            // else, return original value
            return source;
        }

        public static string EnumToCsvString(Enum _enum)
        {
            StringBuilder sbTypeBuilder = new StringBuilder();
            bool bFirst = true;
            string enumValues = _enum.ToString();
            foreach (string str in enumValues.Split((char)44))
            {
                //              int i = (int)Enum.Parse(_enum.GetType(), str, true);
                //              if(!bFirst) sbTypeBuilder.Append(",");
                //              sbTypeBuilder.Append(i);
                //              bFirst = false;
                Enum e = (Enum)Enum.Parse(_enum.GetType(), str, true);
                int enumValue = int.Parse(Enum.Format(_enum.GetType(), e, "d"));
                if (!bFirst) sbTypeBuilder.Append(",");
                sbTypeBuilder.Append(enumValue);
                bFirst = false;

            }
            return sbTypeBuilder.ToString();
        }

        public static object CheckNull(object obj)
        {
            /*if (obj != null && obj is short && ((short)obj).Equals(-1))
                return DBNull.Value;
            if (obj != null && obj is byte && ((byte)obj).Equals(-1))
                    return DBNull.Value;
            if (obj != null && obj is int && ((int)obj).Equals(-1))
                return DBNull.Value;
            if (obj != null && obj is long && ((long)obj).Equals(-1))
                return DBNull.Value;
            if (obj != null && obj is double && ((double)obj).Equals(-1))
                return DBNull.Value;
            if (obj != null && obj is float && ((float)obj).Equals(-1))
                return DBNull.Value;*/
            if (obj == null)
                return DBNull.Value;
            if (obj is string && ((string)obj).Equals(string.Empty))
                return null;
            if (obj is DateTime && ((DateTime)obj).Equals(DateTime.MinValue))
                return DBNull.Value;
            if (obj is Guid && ((Guid)obj).Equals(Guid.Empty))
                return DBNull.Value;
            if (obj is Array && ((Array)obj).Length == 0)
                return DBNull.Value;
            return obj;
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose-only, i.e. non-finalizable logic
                }

                // shared cleanup logic
                disposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
