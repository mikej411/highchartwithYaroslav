using OpenQA.Selenium;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Browser.Core.Framework
{
    /// <summary>
    /// Wrapper for the igDataChart control that allows accessing the datasource, series datasource, etc
    /// </summary>
    /// <seealso cref="Browser.Core.Framework.IgDataSourceElement" />
    public class IgDataChart : IgDataSourceElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IgDataChart"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="driver">The driver.</param>
        public IgDataChart(IWebElement element, IWebDriver driver)
            : base(element, driver, "igDataChart")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IgDataChart"/> class.
        /// Provided for inheritance purposes
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="driver">The driver.</param>
        /// <param name="controlName">Name of the control.</param>
        protected IgDataChart(IWebElement element, IWebDriver driver, string controlName)
            :base(element, driver, controlName)
        { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the DataSource for an individual series.  The igDataChart supports defining a single dataSource for all series, or individual data sources for each series.
        /// If the series does not has a DataSource explicitly defined then this method will return null, and you should call GetDataSource instead.
        /// </summary>
        /// <typeparam name="T">The object type for each item in the DataSource.</typeparam>
        /// <param name="seriesTitle">The title of the series you want to retrieve.</param>
        /// <returns>The DataSource as an IEnumerable of T.</returns>
        public IEnumerable<T> GetSeriesDataSource<T>(string seriesTitle)
        {
            string scriptString =
                @"var result;
                var series = $(arguments[0]).igDataChart('option', 'series');
	            for (var x = 0; x < series.length; x++) {
		            if (series[x].title == arguments[1] && series[x].hasOwnProperty('dataSource')) {
			            result = series[x].dataSource;
                        break;
		            }
	            }
                if (result != null)
                    return JSON.stringify(result);
                else
                    return null;";
            scriptString = Regex.Replace(scriptString, @"\t|\n|\r", ""); //I kept the script string in a readable format in the C# code, this line makes it a valid js script format
            var jsResult = WrappedDriver.ExecuteScript(scriptString, WrappedElement, seriesTitle) as string;
            IEnumerable<T> dataSource = null;
            if (jsResult != null)
                dataSource = JsonConvert.DeserializeObject<IEnumerable<T>>(jsResult);
            return dataSource;
        }

        #endregion
    }
}
