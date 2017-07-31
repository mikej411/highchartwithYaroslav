using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Browser.Core.Framework
{
    /// <summary>
    /// A utility class for useful collection logic.
    /// </summary>
    public static class CollectionUtils
    {
        /// <summary>
        /// Determine if an IEnumerable is Null or Empty.
        /// </summary>
        /// <typeparam name="T">Generic type of IEnumerable items.</typeparam>
        /// <param name="enumerable">IEnumerable of objects.</param>
        /// <returns>True if the enumerable is Null or empty.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return (enumerable == null || !enumerable.Any());
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements by using
        /// the default equality comparer for their type. Also true if both sequences are null.
        /// </summary>
        /// <typeparam name="T">The type of sequence to compare.</typeparam>
        /// <param name="a">The first sequence.</param>
        /// <param name="b">The second seqeunce.</param>
        /// <returns>True if the two source sequences are both null or of equal length and their corresponding
        /// elements are equal according to the default equality comparer for their type;
        /// otherwise, false.</returns>
        public static bool SequenceEqualOrNull<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            return Enumerable.SequenceEqual(a, b);
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements by using
        /// the given equality comparer for their type. Also true if both sequences are null.
        /// </summary>
        /// <typeparam name="T">The type of sequence to compare.</typeparam>
        /// <param name="a">The first sequence.</param>
        /// <param name="b">The second seqeunce.</param>
        /// <returns>True if the two source sequences are both null or of equal length and their corresponding
        /// elements are equal according to the given equality comparer for their type;
        /// otherwise, false.</returns>
        public static bool SequenceEqualOrNull<T>(this IEnumerable<T> a, IEnumerable<T> b, IEqualityComparer<T> comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            return Enumerable.SequenceEqual(a, b, comparer);
        }

        /// <summary>
        /// Creates a DataTable equivalent of an IEnumerable for a single column.  The DataColumn.DataType will
        /// be typeof(T).  This method works BEST for value types like int, string, DateTime, etc, but will “work” for any type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self">The self.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>A DataTable equivalent of an IEnumerable for a single column.</returns>
        /// <exception cref="System.ArgumentNullException">self, columnName</exception>
        /// <exception cref="System.ArgumentException">if columnName is empty</exception>
        public static DataTable ToDataTable<T>(this IEnumerable<T> self, string columnName)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (columnName == null)
                throw new ArgumentNullException();
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException(nameof(columnName));

            var adapter = new DefaultDataTableAdapter<T>(DataTableColumnBehavior.SingleColumn);
            var dt = adapter.ToDataTable(self);
            dt.Columns[0].ColumnName = columnName;
            return dt;
        }

        /// <summary>
        /// Creates a DataTable for the specified IEnumerable.  For value types (and string/stringbuilder) this method will create a datatable
        /// with a single column.  For complex types, it will use reflection and create a column for each property.  The type of each column
        /// will match the property type (even if the property type is a complex type).
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="self">The collection</param>
        /// <returns>A DataTable equivalent of an IEnumerable.</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> self)
        {
            return new DefaultDataTableAdapter<T>().ToDataTable(self);
        }
    }
}
