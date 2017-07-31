using System;
using System.Data;
using System.Linq;

namespace Browser.Core.Framework
{
    /// <summary>
    /// Given a data provider, IDataAccessProvider, this class provides a convenient way
    /// to work with data from a data source.
    /// </summary>
    public class DataAccess
    {
        #region Private Properties

        /// <summary>
        /// The data provider to user for the data access.
        /// </summary>
        IDataAccessProvider _daProvider = null;

        #endregion Private Properties

        #region Constructors

        /// <summary>
        /// Constructor that takes a data provider that knows how to connect to the data source.
        /// </summary>
        /// <param name="daProvider"></param>
        public DataAccess(IDataAccessProvider daProvider)
        {
            _daProvider = daProvider;
        }

        #endregion Constructors

        #region Execute

        #region ExecuteNonQuery

        /// <summary>
        /// Execute a SQL statement against our data source.
        /// </summary>
        /// <param name="sql">The SQL to execute.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(string sql)
        {
            return DoExecuteNonQuery(sql, null);
        }

        /// <summary>
        /// Execute a SQL statement against our data source.
        /// </summary>
        /// <param name="sql">The SQL to execute.</param>
        /// <param name="timeout">The time, in seconds, to wait for the command to execute.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(string sql, int timeout)
        {
            return DoExecuteNonQuery(sql, timeout);
        }

        /// <summary>
        /// Execute a SQL statement against our data source.
        /// </summary>
        /// <param name="sql">The SQL to execute.</param>
        /// <param name="timeout">The time, in seconds, to wait for the command to execute. 
        /// If null, uses the default provider command timeout value.</param>
        /// <returns>The number of rows affected.</returns>
        private int DoExecuteNonQuery(string sql, int? timeout = null)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            using (var conn = _daProvider.CreateConnection())
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    if (timeout != null)
                    {
                        cmd.CommandTimeout = timeout.Value;
                    }

                    var result = cmd.ExecuteNonQuery();
                    return result;
                }
            }
        }

        #endregion ExecuteNonQuery

        #endregion Execute

        #region Get

        #region GetDataValue

        /// <summary>
        /// Given a SQL command, return a single data value from some table, column and row.
        /// </summary>
        /// <param name="sql">The SQL that should return exactly one data value.</param>
        /// <returns>The value found at a given table, colun and row.</returns>
        public object GetDataValue(string sql)
        {
            return DoGetDataValue(sql, null);
        }

        /// <summary>
        /// Given a SQL command, return a single data value from some table, column and row.
        /// </summary>
        /// <param name="sql">The SQL that should return exactly one data value.</param>
        /// <param name="timeout">The time, in seconds, to wait for the command to execute.</param>
        /// <returns>The value found at a given table, colun and row.</returns>
        public object GetDataValue(string sql, int timeout)
        {
            return DoGetDataValue(sql, timeout);
        }

        /// <summary>
        /// Given a SQL command, return a single data value from some table, column and row.
        /// </summary>
        /// <param name="sql">The SQL that should return exactly one data value.</param>
        /// <param name="timeout">The time, in seconds, to wait for the command to execute. 
        /// If null, uses the default provider command timeout value.</param>
        /// <returns>The value found at a given table, colun and row.</returns>
        private object DoGetDataValue(string sql, int? timeout = null)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            var dataRow = DoGetDataRow(sql, timeout);
            return dataRow[0];
        }

        #endregion GetDataValue

        #region GetDataValue

        /// <summary>
        /// Given a SQL command, return a stongly typed single data value from some table, column and row.
        /// </summary>
        /// <param name="sql">The SQL that should return exactly one data value.</param>
        /// <returns>The strongly typed value found at a given table, colun and row.</returns>
        public T GetDataValue<T>(string sql)
        {
            return DoGetDataValue<T>(sql, null);
        }

        /// <summary>
        /// Given a SQL command, return a stongly typed single data value from some table, column and row.
        /// </summary>
        /// <param name="sql">The SQL that should return exactly one data value.</param>
        /// <param name="timeout">The time, in seconds, to wait for the command to execute.</param>
        /// <returns>The strongly typed value found at a given table, colun and row.</returns>
        public T GetDataValue<T>(string sql, int timeout)
        {
            return DoGetDataValue<T>(sql, timeout);
        }

        /// <summary>
        /// Given a SQL command, return a stongly typed single data value from some table, column and row.
        /// </summary>
        /// <param name="sql">The SQL that should return exactly one data value.</param>
        /// <param name="timeout">The time, in seconds, to wait for the command to execute. 
        /// If null, uses the default provider command timeout value.</param>
        /// <returns>The strongly typed value found at a given table, colun and row.</returns>
        private T DoGetDataValue<T>(string sql, int? timeout = null)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            var dataRow = DoGetDataRow(sql, timeout);
            return dataRow.Field<T>(0);
        }

        #endregion GetDataValue

        #region GetDataRow

        /// <summary>
        /// Given a SQL command, return a single row from a data source table.
        /// </summary>
        /// <param name="sql">The SQL that should return exactly one row from a data source.</param>
        /// <returns>One data row from the data source.</returns>
        public DataRow GetDataRow(string sql)
        {
            return DoGetDataRow(sql, null);
        }

        /// <summary>
        /// Given a SQL command, return a single row from a data source table.
        /// </summary>
        /// <param name="sql">The SQL that should return exactly one row from a data source.</param>
        /// <param name="timeout">The time, in seconds, to wait for the command to execute.</param>
        /// <returns>One data row from the data source.</returns>
        public DataRow GetDataRow(string sql, int timeout)
        {
            return DoGetDataRow(sql, timeout);
        }

        /// <summary>
        /// Given a SQL command, return a single row from a data source table.
        /// </summary>
        /// <param name="sql">The SQL that should return exactly one row from a data source.</param>
        /// <param name="timeout">The time, in seconds, to wait for the command to execute. 
        /// If null, uses the default provider command timeout value.</param>
        /// <returns>One data row from the data source.</returns>
        private DataRow DoGetDataRow(string sql, int? timeout = null)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            var dataTable = DoGetDataTable(sql, timeout);

            var rows = dataTable.AsEnumerable();
            if (rows.Count() == 0) throw new InvalidOperationException("Query returned no rows.");
            if (rows.Count() > 1) throw new InvalidOperationException("Query returned more than one row.");

            return rows.Single();
        }

        #endregion GetDataRow

        #region GetDataTable

        /// <summary>
        /// Given a SQL command, return many rows from a data source table.
        /// </summary>
        /// <param name="sql">The SQL that can return many rows from a data source.</param>
        /// <returns>A data table that represents many rows from a data source.</returns>
        public DataTable GetDataTable(string sql)
        {
            return DoGetDataTable(sql, null);
        }

        /// <summary>
        /// Given a SQL command, return many rows from a data source table.
        /// </summary>
        /// <param name="sql">The SQL that can return many rows from a data source.</param>
        /// <param name="timeout">The time, in seconds, to wait for the command to execute.</param>
        /// <returns>A data table that represents many rows from a data source.</returns>
        public DataTable GetDataTable(string sql, int timeout)
        {
            return DoGetDataTable(sql, timeout);
        }

        /// <summary>
        /// Given a SQL command, return many rows from a data source table.
        /// </summary>
        /// <param name="sql">The SQL that can return many rows from a data source.</param>
        /// <param name="timeout">The time, in seconds, to wait for the command to execute. 
        /// If null, uses the default provider command timeout value.</param>
        /// <returns>A data table that represents many rows from a data source.</returns>
        private DataTable DoGetDataTable(string sql, int? timeout = null)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            var dataSet = DoGetDataSet(sql, timeout);
            return dataSet.Tables[0];
        }

        #endregion GetDataTable

        #region GetDataSet

        /// <summary>
        /// Given a SQL command that can query many tables from the same data source, 
        /// return many tables into a DataSet.
        /// </summary>
        /// <param name="sql">The SQL that can return many rows from many tables from a data source</param>
        /// <returns>A DataSet representing many rows from many tables.</returns>
        public DataSet GetDataSet(string sql)
        {
            return DoGetDataSet(sql, null);
        }

        /// <summary>
        /// Given a SQL command that can query many tables from the same data source, 
        /// return many tables into a DataSet.
        /// </summary>
        /// <param name="sql">The SQL that can return many rows from many tables from a data source</param>
        /// <param name="timeout">The time, in seconds, to wait for the command to execute.</param>
        /// <returns>A DataSet representing many rows from many tables.</returns>
        public DataSet GetDataSet(string sql, int timeout)
        {
            return DoGetDataSet(sql, timeout);
        }

        /// <summary>
        /// Given a SQL command that can query many tables from the same data source, 
        /// return many tables into a DataSet.
        /// </summary>
        /// <param name="sql">The SQL that can return many rows from many tables from a data source</param>
        /// <param name="timeout">The time, in seconds, to wait for the command to execute. 
        /// If null, uses the default provider command timeout value.</param>
        /// <returns>A DataSet representing many rows from many tables.</returns>
        private DataSet DoGetDataSet(string sql, int? timeout = null)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            using (var conn = _daProvider.CreateConnection())
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    if (timeout != null)
                    {
                        cmd.CommandTimeout = timeout.Value;
                    }

                    var adapater = _daProvider.CreateAdapter();
                    adapater.SelectCommand = cmd;

                    var dataSet = new DataSet("dataSet");
                    adapater.Fill(dataSet);

                    return dataSet;
                }
            }
        }

        #endregion GetDataSet

        #endregion Get


    }

}
