using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Browser.Core.Framework
{
    /// <summary>
    /// Defines the behavior to use when serializing an object to a DataColumn
    /// </summary>
    public enum DataTableColumnBehavior
    {
        /// <summary>
        /// The default behavior uses a single column for: string, StringBuilder, and all value types (including structs and enums).
        /// For all reference types, the adapter uses reflection and creates a column for each property on the object.
        /// </summary>
        Default,
        /// <summary>
        /// Forces the adapter to create a single column, no matter what the type is.
        /// </summary>
        SingleColumn
    }

    /// <summary>
    /// Defines the behavior for converting a collection of objects to a DataTable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataTableAdapter<T>
    {
        /// <summary>
        /// Converts the collection of items to a DataTable.
        /// </summary>
        /// <param name="items">The items to be converted.</param>
        /// <returns></returns>
        DataTable ToDataTable(IEnumerable<T> items);
    }

    /// <summary>
    /// Base class for converting an IEnumerable into a System.Data.DataTable
    /// 
    /// For "simple types" (value types, string, and stringbuilder) this will create a
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultDataTableAdapter<T> : IDataTableAdapter<T>
    {
        private BindingFlags _bindingFlags;
        private Predicate<PropertyInfo> _propertyFilter;
        private DataTableColumnBehavior _behavior;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDataTableAdapter{T}"/> class.
        /// </summary>
        /// <param name="behavior">The behavior to use when generating columns.</param>
        /// <param name="bindingFlags">The binding flags to use when evaluating the properties to be serialized.</param>
        /// <param name="propertyFilter">The property filter that can limit which properties are serialized.</param>
        public DefaultDataTableAdapter(
            DataTableColumnBehavior behavior = DataTableColumnBehavior.Default,
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance,
            Predicate<PropertyInfo> propertyFilter = null)
        {
            _behavior = behavior;
            _bindingFlags = bindingFlags;
            _propertyFilter = propertyFilter;
        }

        /// <summary>
        /// Converts the specified collection to a System.Data.DataTable
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">items</exception>
        public DataTable ToDataTable(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var table = new DataTable();
            var columns = GetColumns();
            if (!columns.Any())
                throw new InvalidOperationException(
                    "No columns found. Make sure that the object specified has properties, and the property filter does not exclude all properties.");

            foreach (var i in columns)
            {
                table.Columns.Add(i);
            }

            foreach (var i in items)
            {
                var row = table.NewRow();
                foreach (var col in columns)
                {
                    row[col.ColumnName] = GetColumnValue(col, i);
                }
                table.Rows.Add(row);
            }

            return table;
        }

        /// <summary>
        /// Gets the columns to be used in the DataTable.
        /// </summary>
        /// <returns></returns>
        protected virtual DataColumn[] GetColumns()
        {
            if (IsSingleColumn(typeof(T)))
            {
                var dataCol = CreateColumn("Column1", typeof(T));
                return new[] { dataCol };
            }

            List<DataColumn> columns = new List<DataColumn>();
            var props = typeof(T).GetProperties(_bindingFlags);
            foreach (var i in props)
            {
                // If specified, give the property filter a chance to filter out unwanted properties
                if (_propertyFilter == null || _propertyFilter(i))
                {
                    columns.Add(CreateColumn(i.Name, i.PropertyType));
                }
            }

            return columns.ToArray();
        }

        /// <summary>
        /// Creates a DataColumn with the specified name and type.  Also handles nullable types
        /// as a DataColumn does not allow nullable types.  In case of a nullable type, the "inner"
        /// type will be used (note that ANY DataColumn can be nullable).
        /// </summary>
        /// <param name="colName">Name of the column.</param>
        /// <param name="dataType">The datatype of the column.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        protected virtual DataColumn CreateColumn(string colName, Type dataType)
        {
            if (colName == null)
                throw new ArgumentNullException(nameof(colName));
            if (dataType == null)
                throw new ArgumentNullException(nameof(dataType));
            DataColumn dataCol = null;
            Type actualType = dataType;
            var nullableValueType = Nullable.GetUnderlyingType(dataType);
            if (nullableValueType != null)
                actualType = nullableValueType; //nullable types aren't allowed in the DataColumn's DataType

            dataCol = new DataColumn(colName, actualType) { AllowDBNull = true }; //no type is allowed to be set to null in a DataTable, we must use DBNull.Value instead
            return dataCol;
        }

        /// <summary>
        /// Gets the value for a specified column, given the row object.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="row">The object.</param>
        /// <returns>If the column value is null, returns DBNull.Value; otherwise, returns the actual value.</returns>
        /// <exception cref="System.ArgumentException"></exception>
        protected virtual object GetColumnValue(DataColumn column, T row)
        {
            if (row == null)
                return DBNull.Value;

            object retVal = null;

            if (IsSingleColumn(typeof(T)))
            {
                // If we're dealing with a single column, there's no need to use reflection
                retVal = row;
            }
            else
            {
                var prop = typeof(T).GetProperty(column.ColumnName);
                if (prop == null)
                    throw new InvalidOperationException(string.Format(
                        "Could not find the property {0} on type {1}", column.ColumnName, typeof(T).FullName));

                retVal = prop.GetValue(row, null);
            }

            return retVal != null ? retVal : DBNull.Value; //no value in a DataTable can be null, all null values must be set to DBNull.Value
        }

        /// <summary>
        /// Gets whether or not the specified type can be represented by a single DataTable column
        /// By default, this returns <c>true</c> for string, StringBuilder, and all value types; <c>false</c> for everything else.
        /// This may also return true if the caller explicitly asked for a single column.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual bool IsSingleColumn(Type type)
        {
            // If the caller explicitly asked for a single column, do that
            if (_behavior == DataTableColumnBehavior.SingleColumn)
                return true;

            if (type == typeof(string) || type == typeof(StringBuilder))
                return true;

            return type.IsValueType;
        }
    }
}
