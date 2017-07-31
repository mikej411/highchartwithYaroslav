using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.Core.Framework.Data
{    
    /// <summary>
    /// Converts an Infragistics IgniteUI control (igGrid, igDataChart, etc) to a System.Data.DataTable
    /// </summary>
    public class InfragisticsControlToDataTableAdapter : IWebElementToDataTableAdapter
    {
        private IWebDriver _browser;
        private string _elementType;
        private IJsonToDataTableConverter _jsonConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfragisticsControlToDataTableAdapter"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        /// <param name="elementType">Type of the element.</param>
        /// <param name="jsonConverter">The json deserializer.</param>
        /// <exception cref="System.ArgumentNullException">browser</exception>
        /// <exception cref="System.ArgumentException">elementType</exception>
        public InfragisticsControlToDataTableAdapter(IWebDriver browser, string elementType, IJsonToDataTableConverter jsonConverter = null)
        {
            if (browser == null)
                throw new ArgumentNullException("browser");
            if (string.IsNullOrEmpty(elementType))
                throw new ArgumentException("elementType");

            _browser = browser;
            _elementType = elementType;
            _jsonConverter = jsonConverter ?? new DefaultJsonToDataTableConverter();
        }

        /// <summary>
        /// Gets a System.Data.DataTable for the specified IWebElement
        /// </summary>
        /// <param name="element">The element for which a DataTable is desired.</param>
        /// <param name="columnDefinitions">(Optional) The columns to be included in the DataTable.  If this parameter is omitted, all columns will be returned.</param>
        /// <returns>
        /// A System.Data.DataTable representation of the IWebElement (this works best for things like an html table or select element)
        /// </returns>
        public DataTable GetDataTable(IWebElement element, params DataColumnDefinition[] columnDefinitions)
        {
            DataTable jsonDataTable = GetJSONDataTable(element);
            var dtBuilder = new DataTableBuilder(columnDefinitions);
            if (!dtBuilder.Columns.Any())
                AddDefaultColumns(dtBuilder, jsonDataTable);

            foreach (DataRow row in jsonDataTable.Rows)
            {
                var newRow = new Dictionary<string, object>();
                foreach (DataColumnDefinition column in dtBuilder.Columns)
                {
                    newRow[column.Name] = row[column.Name];
                }
                dtBuilder.AddRow(newRow);
            }

            return dtBuilder.ToTable();
        }

        private void AddDefaultColumns(DataTableBuilder builder, DataTable jsonDataTable)
        {
            foreach (DataColumn i in jsonDataTable.Columns)
            {
                builder.AddColumn(new DataColumnDefinition(i.ColumnName, i.DataType));
            } 
        }

        private DataTable GetJSONDataTable(IWebElement element)
        {
            //JSON.stringify is necessary to get the raw JSON text back. Otherwise, Selenium parses it into an ICollection of
            //IDictionaries, and returns every value as an unparsed string.
            string jsText = string.Format("return JSON.stringify($(arguments[0]).{0}('option', 'dataSource'));", _elementType);

            var jsonText = _browser.ExecuteScript(jsText, element) as string;
            return _jsonConverter.Convert(jsonText);
        }
    }
}
