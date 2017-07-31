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
    /// Converts an html &lt;table/&gt; element to a System.Data.DataTable
    /// </summary>
    public class HtmlTableToDataTableAdapter : IWebElementToDataTableAdapter
    {
        /// <summary>
        /// Gets a System.Data.DataTable for the specified IWebElement
        /// </summary>
        /// <param name="element">The element for which a DataTable is desired.</param>
        /// <param name="columnDefinitions">(Optional) The columns to be included in the DataTable.  If this parameter is omitted, all columns will be returned.</param>
        /// <returns>
        /// A System.Data.DataTable representation of the IWebElement (this works best for things like an html table or select element)
        /// </returns>
        /// <exception cref="System.ArgumentException"></exception>
        public DataTable GetDataTable(IWebElement element, params DataColumnDefinition[] columnDefinitions)
        {
            var dtBuilder = new DataTableBuilder(columnDefinitions);
            if (!dtBuilder.Columns.Any())
                AddDefaultColumns(element, dtBuilder);

            var rows = element.FindElements(By.CssSelector("tr"));
            foreach (var row in rows)
            {
                var cells = row.FindElements(By.CssSelector("td"));
                // We might fail to find any cells if we're inside a header row.
                // We'll just ignore any that we find.
                // Note that it's possible for a table to have multiple header rows, 
                if (cells.Any())
                {
                    if (cells.Count != dtBuilder.Columns.Count())
                        throw new ArgumentException(string.Format("The number of <td/> elements found ({0}) does not match the number of columns specified ({1}).", cells.Count, dtBuilder.Columns.Count()));

                    var newRow = new Dictionary<string, object>();
                    for (int i = 0; i < dtBuilder.Columns.Count(); i++)
                    {
                        var column = dtBuilder.Columns.ElementAt(i);
                        newRow[column.Name] = cells.ElementAt(i).Text;
                    }
                    dtBuilder.AddRow(newRow);
                }
            }

            return dtBuilder.ToTable();
        }

        private void AddDefaultColumns(IWebElement element, DataTableBuilder builder)
        {            
            var thElements = element.FindElements(By.CssSelector("th"));
            if (!thElements.Any())
                throw new ArgumentException("No columns found.  Make sure the table contains a <tr> with at least one <th> in it, or manually specify the columns by passing in ColumnDefinitions.");

            var columns = new List<DataColumnDefinition>();
            foreach (var i in thElements)
            {
                builder.AddColumn(new DataColumnDefinition(i.Text, typeof(string)));
            }            
        }
    }
}
