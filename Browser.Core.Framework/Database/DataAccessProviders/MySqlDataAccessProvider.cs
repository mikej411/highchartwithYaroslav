using MySql.Data.MySqlClient;
using System;
using System.Data.Common;

namespace Browser.Core.Framework
{
    /// <summary>
    /// An MySql implementation of the DataAccessProvider.
    /// </summary>
    public class MySqlDataAccessProvider : DataAccessProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">The string used to connect to the data source.</param>
        public MySqlDataAccessProvider(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Using the connection string, connect to a data source.
        /// </summary>
        /// <returns>A connection to the data source.</returns>
        public override DbConnection CreateConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        /// <summary>
        /// Create a DataAdapter for the data source.
        /// </summary>
        /// <returns>A DataAdapter for the data source.</returns>
        public override DbDataAdapter CreateAdapter()
        {
            return new MySqlDataAdapter();
        }

        /// <summary>
        /// Given a data adapter, create a single table command builder.
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns>A commmand builder.</returns>
        public override DbCommandBuilder CreateCommandBuilder(DbDataAdapter adapter)
        {
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));

            return new MySqlCommandBuilder((MySqlDataAdapter)adapter);
        }
    }
}
