using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.AppFramework
{
    public class IGGridElement : IWrapsElement
    {
        #region Fields

        private IWebElement _elem;

        #endregion Fields

        #region Constructors

        public IGGridElement(IWebElement elem)
        {
            _elem = elem;
        }

        #endregion Constructors

        #region IWrapsElement implementation

        public IWebElement WrappedElement
        {
            get
            {
                return _elem;
            }
        }

        #endregion IWrapsElement implementation

        #region Common IWebElement calls

        public bool Displayed
        {
            get
            {
                return _elem.Displayed;
            }
        }

        #endregion Common IWebElement calls

        #region Row methods

        /// <summary>
        /// Gets the grid's header row element
        /// </summary>
        /// <returns>The "thead>tr" element</returns>
        public IWebElement GetHeaderRow()
        {
            return _elem.FindElement(By.CssSelector("thead>tr"));
        }

        /// <summary>
        /// Get a row by physical number
        /// </summary>
        /// <param name="row">The row number</param>
        /// <returns>The "tr" element</returns>
        public IWebElement GetRow(int row)
        {
            return _elem.FindElement(By.CssSelector("tr:nth-child(" + row + ")"));
        }

        /// <summary>
        /// Get a row by physical number
        /// </summary>
        /// <param name="row">The row number</param>
        /// <returns>The "tr" element</returns>
        public IWebElement GetRowBody(int row)
        {
            return _elem.FindElement(By.CssSelector("tbody>tr:nth-child(" + row + ")"));
        }

        /// <summary>
        /// Get a row by primary key (if the grid's primaryKey property is set)
        /// </summary>
        /// <param name="primaryKey">The primary key value</param>
        /// <returns>The "tr" element</returns>
        public IWebElement GetRow(string primaryKey)
        {
            return _elem.FindElement(By.CssSelector("tr[data-id=\"" + primaryKey + "\"]"));
        }

        #endregion Row methods

        #region Column methods

        /// <summary>
        /// Get the cell for a column in a row by "tr" element
        /// </summary>
        /// <param name="row">The row's "tr" element (from GetRow())</param>
        /// <param name="col">The column number</param>
        /// <returns>The "td" element</returns>
        public IWebElement GetColumn(IWebElement row, int col)
        {
            return row.FindElement(By.CssSelector("td:nth-child(" + col + ")"));
        }

        /// <summary>
        /// Gets the cell for a column in a row by row number
        /// </summary>
        /// <param name="row">The row number</param>
        /// <param name="col">The column number</param>
        /// <returns>The "td" element</returns>
        public IWebElement GetColumn(int row, int col)
        {
            return this.GetColumn(this.GetRow(row), col);
        }


        /// <summary>
        /// Gets the cell for a column in a row by row number
        /// </summary>
        /// <param name="row">The row number</param>
        /// <param name="col">The column number</param>
        /// <returns>The "td" element</returns>
        public IWebElement GetColumnBody(int row, int col)
        {
            return this.GetColumn(this.GetRowBody(row), col);
        }

        /// <summary>
        /// Gets the cell for a column in a row by primary key
        /// </summary>
        /// <param name="primaryKey">The primary key</param>
        /// <param name="col">The column number</param>
        /// <returns>The "td" element</returns>
        public IWebElement GetColumn(string primaryKey, int col)
        {
            return this.GetColumn(this.GetRow(primaryKey), col);
        }

        /// <summary>
        /// Get the header cell for the given column
        /// </summary>
        /// <param name="col">The column number</param>
        /// <returns>The "th" element</returns>
        public IWebElement GetColumnHeader(int col)
        {
            return this.GetHeaderRow().FindElement(By.CssSelector("th:nth-child(" + col + ")"));
        }

        public IList<string> GetAllColumnHeaders()
        {
            return new List<string>(_elem.FindElements(By.ClassName("ui-iggrid-headertext")).Select(t => t.Text));
        }

        #endregion Column methods

        #region Text methods

        /// <summary>
        /// Tests for a string of text in the grid
        /// </summary>
        /// <param name="text">Search string</param>
        /// <returns>true if string is found</returns>
        public bool Contains(string text)
        {
            return _elem.Text.Contains(text);
        }

        #endregion Text methods
    }
}