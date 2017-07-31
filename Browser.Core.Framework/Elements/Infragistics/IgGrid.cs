using OpenQA.Selenium;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading;

namespace Browser.Core.Framework
{
    /// <summary>
    /// Wrapper for the igGrid control that allows accessing the datasource, scrolling virtualized elements into view, etc
    /// </summary>
    /// <seealso cref="Browser.Core.Framework.IgDataSourceElement" />
    public class IgGrid : IgDataSourceElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IgGrid"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="driver">The driver.</param>
        public IgGrid(IWebElement element, IWebDriver driver)
            : base(element, driver, "igGrid")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IgGrid"/> class.
        /// Provided for inheritance purposes
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="driver">The driver.</param>
        /// <param name="controlName">Name of the control.</param>
        protected IgGrid(IWebElement element, IWebDriver driver, string controlName)
            :base(element, driver, controlName)
        { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Scrolls the specified row into view.  This method should be used to scroll virtualized grid rows into view before interacting with them.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        public void ScrollGridRowIntoView(int rowIndex)
        {
            //There is a bug where the 'virtualScrollTo' js function won't correctly scroll a row into view, the workaround is to scroll to the first index, wait, then scroll to the real index
            WrappedDriver.ExecuteScript(@"$(arguments[0]).igGrid('virtualScrollTo', arguments[1]);", WrappedElement, 0);
            Thread.Sleep(1000);
            WrappedDriver.ExecuteScript(@"$(arguments[0]).igGrid('virtualScrollTo', arguments[1]);", WrappedElement, rowIndex);
        }

        /// <summary>
        /// Returns a collection containing only the objects that are visible in the grid and in the order they appear (i.e. filtering and sorting is applied).
        /// </summary>
        /// <typeparam name="T">The object type for each item in the DataView.  This should be the same as the type you would use for GetDataSource.</typeparam>
        /// <returns></returns>
        public virtual IEnumerable<T> GetDataView<T>()
        {
            var jsResult = GetDataViewJSON();
            var dataView = JsonConvert.DeserializeObject<IEnumerable<T>>(jsResult);
            return dataView;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Returns a json array consisting of only the objects that are visible in the grid and in the order they appear (i.e. filtering and sorting is applied).
        /// </summary>
        /// <returns></returns>
        protected string GetDataViewJSON()
        {
            string jsText = @"return JSON.stringify($(arguments[0]).data('igGrid').dataSource.dataView());";
            return WrappedDriver.ExecuteScript(jsText, WrappedElement) as string;
        }

        #endregion
    }
}
