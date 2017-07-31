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
    /// Describes an adapter that can generate a System.Data.DataTable, given an IWebElement
    /// </summary>
    public interface IWebElementToDataTableAdapter
    {
        /// <summary>
        /// Gets a System.Data.DataTable for the specified IWebElement
        /// </summary>
        /// <param name="element">The element for which a DataTable is desired.</param>
        /// <param name="columnDefinitions">(Optional) The columns to be included in the DataTable.  If this parameter is omitted, all columns will be returned.</param>
        /// <returns>A System.Data.DataTable representation of the IWebElement (this works best for things like an html table or select element)</returns>
        DataTable GetDataTable(IWebElement element, params DataColumnDefinition[] columnDefinitions);
    }        
}
