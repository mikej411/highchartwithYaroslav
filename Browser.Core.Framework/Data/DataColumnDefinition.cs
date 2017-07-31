using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.Core.Framework
{
    /// <summary>
    /// Defines the name and type of a System.Data.DataColumn, and optionally
    /// specifies a value converter that can be used when loading data.
    /// </summary>
    public class DataColumnDefinition
    {        
        /// <summary>
        /// The name of the column
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The type of the column
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// (Optional) A value converter that will be called when populating data in the column.
        /// </summary>
        public Func<object, object> ValueConverter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataColumnDefinition"/> class.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <param name="type">The type of data in the column.</param>
        /// <param name="valueConverter">(Optional) A value converter that will be called when populating data in the column.</param>
        /// <exception cref="System.ArgumentException">name</exception>
        /// <exception cref="System.ArgumentNullException">type</exception>
        public DataColumnDefinition(string name, Type type, Func<object, object> valueConverter = null)
        {            
            if (type == null)
                throw new ArgumentNullException("type");

            Name = name;
            Type = type;
            ValueConverter = valueConverter;
        }
        
        /// <summary>
        /// Converts the value using the ValueConverter property (if set).  If the ValueConverter
        /// property is null, the value itself is returned with no conversion.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ConvertValue(object value)
        {
            if (ValueConverter == null)
                return value;

            return ValueConverter(value);
        }
    }
}
