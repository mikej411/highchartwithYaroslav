using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.Core.Framework.Data
{
    /// <summary>
    /// Provides methods for building a System.Data.DataTable object based on a collection of DataColumnDefinitions.
    /// </summary>
    public class DataTableBuilder
    {
        private List<DataColumnDefinition> _columns = new List<DataColumnDefinition>();        
        private List<IDictionary<string, object>> _rows = new List<IDictionary<string, object>>();

        /// <summary>
        /// Gets the columns that have been defined for this datatable.  Use the AddColumn method
        /// to define a new column.
        /// </summary>        
        public IEnumerable<DataColumnDefinition> Columns
        {
            get { return _columns; }
        }        

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableBuilder"/> class.
        /// </summary>
        /// <param name="columnDefinitions">The column definitions.</param>
        /// <exception cref="System.ArgumentException">columnDefinitions</exception>
        public DataTableBuilder(params DataColumnDefinition[] columnDefinitions)
        {
            if (columnDefinitions != null)
            {
                foreach (var i in columnDefinitions)
                {
                    AddColumn(i);
                }
            }            
        }    
        
        /// <summary>
        /// Adds a column
        /// </summary>
        /// <param name="columnDef"></param>
        public void AddColumn(DataColumnDefinition columnDef)
        {
            if (string.IsNullOrEmpty(columnDef.Name))
                throw new ArgumentException("All columns must have a name");
            if (_columns.Any(p => p.Name == columnDef.Name))
                throw new DuplicateNameException(string.Format("A column with the name {0} already exists.", columnDef.Name));

            _columns.Add(columnDef);
        }                     

        /// <summary>
        /// Adds a row.
        /// </summary>
        /// <returns>A dictionary where the key is the column name and the value is the column value.  Note that the column value should be the "uncoverted" value.  If the DataColumnDefinition defines a converter, the value will be converted when calling ToTable().</returns>
        public void AddRow(IDictionary<string, object> row)
        {            
            var invalidColumns = row.Keys.Where(p => !_columns.Select(q => q.Name).Contains(p));
            if (invalidColumns.Any())
                throw new InvalidOperationException(string.Format("The following columns were not found: {0}.", string.Join(", ", invalidColumns)));
                
            _rows.Add(row);
        }

        /// <summary>
        /// Creates the DataTable from the rows and columns specified.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataTable ToTable(string name = null)
        {
            var columnLookup = _columns.ToDictionary(p => p.Name);
            var table = new DataTable(name);
            foreach(var i in _columns)
            {
                table.Columns.Add(new DataColumn(i.Name, i.Type));
            }

            foreach(var i in _rows)
            {
                var row = table.NewRow();
                foreach(var kvp in i)
                {
                    row[kvp.Key] = ConvertValue(kvp.Key, kvp.Value, columnLookup);
                }
                table.Rows.Add(row);
            }

            return table;
        }    
    
        private object ConvertValue(string columnName, object value, IDictionary<string, DataColumnDefinition> columnLookup)
        {            
            if (!columnLookup.ContainsKey(columnName))
                return value;

            return columnLookup[columnName].ConvertValue(value);
        }        
    }   
}
