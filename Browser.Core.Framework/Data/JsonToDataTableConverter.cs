using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.Core.Framework.Data
{
    /// <summary>
    /// Describes an object that can convert from a json string into a System.Data.DataTable.
    /// </summary>
    public interface IJsonToDataTableConverter
    {
        /// <summary>
        /// Converts from the jsonString into a System.Data.DataTable
        /// </summary>
        /// <param name="jsonString"></param>        
        DataTable Convert(string jsonString);
    }

    /// <summary>
    /// Default implementation of IJsonToDataTableConverter that uses JSON.Net
    /// by calling JsonConvert.DeserializeObject&lt;DataTable&gt;()
    /// </summary>
    public class DefaultJsonToDataTableConverter : IJsonToDataTableConverter
    {
        private JsonSerializerSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultJsonToDataTableConverter"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public DefaultJsonToDataTableConverter(JsonSerializerSettings settings = null)
        {
            _settings = settings ?? new JsonSerializerSettings();
        }

        /// <summary>
        /// Converts from the jsonString into a System.Data.DataTable
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public DataTable Convert(string jsonString)
        {
            return JsonConvert.DeserializeObject<DataTable>(jsonString, _settings);
        }
    }
}
