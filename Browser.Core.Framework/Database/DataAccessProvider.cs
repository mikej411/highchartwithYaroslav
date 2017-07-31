using System.Data.Common;

namespace Browser.Core.Framework
{
    /// <summary>
    /// Used by the DataAccess class, this interface represents a provider
    /// that knows how to connect to a data source given a connection string.
    /// </summary>
    public interface IDataAccessProvider
    {
        /// <summary>
        /// The string used to connect to the data source.
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Using the connection string, connect to a data source.
        /// </summary>
        /// <returns></returns>
        DbConnection CreateConnection();

        /// <summary>
        /// Create a DataAdapter for the data source.
        /// </summary>
        /// <returns></returns>
        DbDataAdapter CreateAdapter();

        /// <summary>
        /// Given a data adapter, create a single tabel command builder.
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns></returns>
        DbCommandBuilder CreateCommandBuilder(DbDataAdapter adapter);
    }

    /// <summary>
    /// An abstarct class that implements the IDataAccessProvider interface.
    /// This is an abstract provider that can connect to a data source 
    /// given a connection string.
    /// </summary>
    public abstract class DataAccessProvider : IDataAccessProvider
    {
        /// <summary>
        /// The string used to connect to the data source.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The string used to connect to the data source.</param>
        public DataAccessProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Using the connection string, connect to a data source.
        /// </summary>
        /// <returns>A connection to the data source.</returns>
        public abstract DbConnection CreateConnection();

        /// <summary>
        /// Create a DataAdapter for the data source.
        /// </summary>
        /// <returns>A DataAdapter for the data source.</returns>
        public abstract DbDataAdapter CreateAdapter();

        /// <summary>
        /// Given a data adapter, create a single table command builder.
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns>A commmand builder.</returns>
        public abstract DbCommandBuilder CreateCommandBuilder(DbDataAdapter adapter);
    }
}
