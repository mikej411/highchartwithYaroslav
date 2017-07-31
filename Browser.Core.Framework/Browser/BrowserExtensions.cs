using Browser.Core.Framework.Data;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;

namespace Browser.Core.Framework
{
    /// <summary>
    /// Extension methods on the IWebDriver interface.
    /// </summary>
    public static class BrowserExtensions
    {
        private static readonly string igGrid = "igGrid";
        private static readonly string igDataChart = "igDataChart";

        /// <summary>
        /// Executes the specified javascript.
        /// Javascript objects that are returned are represented as an IDictionary&lt;string, object&gt;
        /// Javascript arrays that are returned are represented as a ReadonlyCollection&lt;object&gt;
        /// Therefore, if the javascript returns an array of objects, it would be represented as ReadonlyCollection&lt;IDictionary&lt;string, object&gt;&gt;
        /// </summary>        
        /// <param name="self">The the browser on which the javascript should be executed.</param>
        /// <param name="js">The javascript that should be executed.</param>
        /// <param name="parameters">(Optional) The parameters that should be passed to the script.  Note: Parameters can be accessed in your script via arguments[x] where x is the index of the parameter.  If you pass an IWebElement, it will be translated to an actual javascript reference to the DOM element.</param>
        /// <returns>The object returned by the javascript call.</returns>        
        /// <exception cref="System.ArgumentException">If the browser does not support javascript execution.</exception>
        public static object ExecuteScript(this IWebDriver self, string js, params object[] parameters)
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if (string.IsNullOrEmpty(js))
                throw new ArgumentException("js");
            var jsExec = self as IJavaScriptExecutor;
            if (jsExec == null)
                throw new ArgumentException(string.Format("The current browser ({0}) does not support javascript", self.GetType().Name));

            return jsExec.ExecuteScript(js, parameters);
        }

        /// <summary>
        /// Checks for the file in the default download directory once every second until the file exists and is not locked or the timeout is reached.
        /// </summary>
        /// <param name="self">The web driver.</param>
        /// <param name="fileNameAndExtension">The name of the file with its file extension that is being downloaded.  This should be a file name only.  It should not include any path.</param>
        /// <param name="fileWaitTimeout">The timeout for this operation to keep trying in milliseconds.  Default is 10000 (10 seconds).</param>
        /// <exception cref="System.TimeoutException">Thrown if the file does not exist within the timeout specified.</exception>
        /// <returns>The fully-qualified path to the file that was downloaded.  Typically this file resides in the user's default download directory.</returns>
        public static string WaitForDownload(this IWebDriver self, string fileNameAndExtension, double fileWaitTimeout = 10000)
        {
            var filepath = SeleniumCoreSettings.DefaultDownloadDirectory + fileNameAndExtension;
            FileUtils.WaitForFile(filepath, fileWaitTimeout);
            return filepath;
        }

        /// <summary>
        /// Gets the capabilities of the browser.
        /// IMPORTANT: This method MIGHT return null!
        /// </summary>
        /// <param name="self">The self.</param>
        /// <returns></returns>
        public static ICapabilities GetCapabilities(this IWebDriver self)
        {
            var tmp = self as IHasCapabilities;
            if (tmp == null)
                return null;

            return tmp.Capabilities;
        }

        #region GoToPage

   

        #endregion

        #region GetDataTable

        /// <summary>        
        /// Gets the FORMATTED data from the html &lt;table/&gt; element as a .NET DataTable.  All cell values are represented
        /// as strings.
        /// - The table MUST have a header row &lt;thead&gt; with &lt;th&gt; elements, or you must pass in column definitions.
        /// 
        /// WARNING: This is an EXPENSIVE method call!  One benchmark took 20 seconds to retrieve 100 rows and 10 columns of data.
        /// Consider using GetDataFromIg* methods for better performance if you don't care about the FORMAT of the data.
        /// </summary>
        /// <param name="self">The Web Browser</param>
        /// <param name="table">An IWebElement that represents an html table element.</param>
        /// <param name="columnDefinitions">(Optional) The columns to be exported.  This parameter can be used if the table does not specify columns using the &lt;thead&gt; element, or if you only want to extract a subset of the columns.</param>
        /// <returns>A DataTable that can be indexed using the column headers.</returns>
        public static DataTable GetDataFromHtmlTable(this IWebDriver self, IWebElement table, params DataColumnDefinition[] columnDefinitions)
        {
            return new HtmlTableToDataTableAdapter()
                .GetDataTable(table, columnDefinitions);
        }

        /// <summary>
        /// Gets a DataTable from an igGrid control by using javascript to get the datasource and then
        /// converting the JSON notation into a .NET DataTable.
        /// </summary>
        /// <param name="self">The Web Browser</param>        
        /// <param name="element">An IWebElement that points to the specified control.</param>        
        /// <param name="columnDefinitions">(Optional) The columns to be retrieved.  The names and types of the columns must be compatible with the names of the properties on the javascript objects that back the ig-control</param>
        /// <returns>A DataTable with all of the data from the datasource of the igGrid control.</returns>
        public static DataTable GetDataFromIgGrid(this IWebDriver self, IWebElement element, params DataColumnDefinition[] columnDefinitions)
        {
            return GetDataFromIgControl(self, igGrid, element, columnDefinitions);
        }

        /// <summary>
        /// Gets a DataTable from an igGrid control by using javascript to get the datasource and then
        /// converting the JSON notation into a .NET DataTable.
        /// </summary>
        /// <param name="self">The Web Browser</param>        
        /// <param name="element">An IWebElement that points to the specified control.</param>
        /// <param name="settings">Settings that control how JSON data is deserialized.</param>
        /// <param name="columnDefinitions">(Optional) The columns to be retrieved.  The names and types of the columns must be compatible with the names of the properties on the javascript objects that back the ig-control</param>
        /// <returns>A DataTable with all of the data from the datasource of the igGrid control.</returns>
        public static DataTable GetDataFromIgGrid(this IWebDriver self, IWebElement element, JsonSerializerSettings settings, params DataColumnDefinition[] columnDefinitions)
        {
            return GetDataFromIgControl(self, igGrid, element, settings, columnDefinitions);
        }

        /// <summary>
        /// Gets a DataTable from an igGrid control by using javascript to get the datasource and then
        /// converting the JSON notation into a .NET DataTable.
        /// </summary>
        /// <param name="self">The Web Browser</param>        
        /// <param name="element">An IWebElement that points to the specified control.</param>
        /// <param name="jsonConverter">The json converter that converts a json string into a DataTable.</param>
        /// <param name="columnDefinitions">(Optional) The columns to be retrieved.  The names and types of the columns must be compatible with the names of the properties on the javascript objects that back the ig-control</param>
        /// <returns>A DataTable with all of the data from the datasource of the igGrid control.</returns>
        public static DataTable GetDataFromIgGrid(this IWebDriver self, IWebElement element, IJsonToDataTableConverter jsonConverter, params DataColumnDefinition[] columnDefinitions)
        {
            return GetDataFromIgControl(self, igGrid, element, jsonConverter, columnDefinitions);
        }

        /// <summary>
        /// Gets a DataTable from an igDataChart control by using javascript to get the datasource and then
        /// converting the JSON notation into a .NET DataTable.
        /// </summary>
        /// <param name="self">The Web Browser</param>
        /// <param name="element">An IWebElement that points to the specified control.</param>
        /// <param name="columnDefinitions">(Optional) The columns to be retrieved.  The names and types of the columns must be compatible with the names of the properties on the javascript objects that back the ig-control</param>
        /// <returns>
        /// A DataTable with all of the data from the datasource of the igDataChart control.
        /// </returns>
        public static DataTable GetDataFromIgDataChart(this IWebDriver self, IWebElement element, params DataColumnDefinition[] columnDefinitions)
        {
            return GetDataFromIgControl(self, igDataChart, element, columnDefinitions);
        }

        /// <summary>
        /// Gets a DataTable from an igDataChart control by using javascript to get the datasource and then
        /// converting the JSON notation into a .NET DataTable.
        /// </summary>
        /// <param name="self">The Web Browser</param>
        /// <param name="element">An IWebElement that points to the specified control.</param>
        /// <param name="settings">Settings that control how JSON data is deserialized.</param>
        /// <param name="columnDefinitions">(Optional) The columns to be retrieved.  The names and types of the columns must be compatible with the names of the properties on the javascript objects that back the ig-control</param>
        /// <returns>
        /// A DataTable with all of the data from the datasource of the igDataChart control.
        /// </returns>
        public static DataTable GetDataFromIgDataChart(this IWebDriver self, IWebElement element, JsonSerializerSettings settings, params DataColumnDefinition[] columnDefinitions)
        {
            return GetDataFromIgControl(self, igDataChart, element, settings, columnDefinitions);
        }

        /// <summary>
        /// Gets a DataTable from an igDataChart control by using javascript to get the datasource and then
        /// converting the JSON notation into a .NET DataTable.
        /// </summary>
        /// <param name="self">The Web Browser</param>
        /// <param name="element">An IWebElement that points to the specified control.</param>
        /// <param name="jsonConverter">The json converter that converts a json string into a DataTable.</param>
        /// <param name="columnDefinitions">(Optional) The columns to be retrieved.  The names and types of the columns must be compatible with the names of the properties on the javascript objects that back the ig-control</param>
        /// <returns>
        /// A DataTable with all of the data from the datasource of the igDataChart control.
        /// </returns>
        public static DataTable GetDataFromIgDataChart(this IWebDriver self, IWebElement element, IJsonToDataTableConverter jsonConverter, params DataColumnDefinition[] columnDefinitions)
        {
            return GetDataFromIgControl(self, igDataChart, element, jsonConverter, columnDefinitions);
        }

        /// <summary>
        /// Gets a DataTable from an Infragistics control by using javascript to get the datasource and then
        /// converting the JSON notation into a .NET DataTable.
        /// </summary>
        /// <param name="self">The Web Browser</param>
        /// <param name="elementType">The type of Infragistics control (igGrid, igDataChart, etc)</param>
        /// <param name="element">An IWebElement that points to the specified control.</param>
        /// <param name="columnDefinitions">(Optional) The columns to be retrieved.  The names and types of the columns must be compatible with the names of the properties on the javascript objects that back the ig-control</param>
        /// <returns></returns>
        public static DataTable GetDataFromIgControl(this IWebDriver self, string elementType, IWebElement element, params DataColumnDefinition[] columnDefinitions)
        {
            return new InfragisticsControlToDataTableAdapter(self, elementType, new DefaultJsonToDataTableConverter())
                .GetDataTable(element, columnDefinitions);
        }

        /// <summary>
        /// Gets a DataTable from an Infragistics control by using javascript to get the datasource and then
        /// converting the JSON notation into a .NET DataTable.
        /// </summary>
        /// <param name="self">The Web Browser</param>
        /// <param name="elementType">The type of Infragistics control (igGrid, igDataChart, etc)</param>
        /// <param name="element">An IWebElement that points to the specified control.</param>
        /// <param name="settings">Settings that control how JSON data is deserialized.</param>
        /// <param name="columnDefinitions">(Optional) The columns to be retrieved.  The names and types of the columns must be compatible with the names of the properties on the javascript objects that back the ig-control</param>
        /// <returns></returns>
        public static DataTable GetDataFromIgControl(this IWebDriver self, string elementType, IWebElement element, JsonSerializerSettings settings, params DataColumnDefinition[] columnDefinitions)
        {
            return new InfragisticsControlToDataTableAdapter(self, elementType, new DefaultJsonToDataTableConverter(settings))
                .GetDataTable(element, columnDefinitions);
        }

        /// <summary>
        /// Gets a DataTable from an Infragistics control by using javascript to get the datasource and then
        /// converting the JSON notation into a .NET DataTable.
        /// </summary>
        /// <param name="self">The Web Browser</param>
        /// <param name="elementType">The type of Infragistics control (igGrid, igDataChart, etc)</param>
        /// <param name="element">An IWebElement that points to the specified control.</param>
        /// <param name="jsonConverter">The json converter that converts a json string into a DataTable.</param>
        /// <param name="columnDefinitions">(Optional) The columns to be retrieved.  The names and types of the columns must be compatible with the names of the properties on the javascript objects that back the ig-control</param>
        /// <returns>
        /// A DataTable with all of the data from the datasource of the infragistics control.
        /// </returns>
        public static DataTable GetDataFromIgControl(this IWebDriver self, string elementType, IWebElement element, IJsonToDataTableConverter jsonConverter, params DataColumnDefinition[] columnDefinitions)
        {
            return new InfragisticsControlToDataTableAdapter(self, elementType, jsonConverter)
                .GetDataTable(element, columnDefinitions);
        }

        public static List<RandomChartRootObject> GetRandomChartData(IWebDriver browser, IWebElement chart)
        {
            string json = GetDataSourceJSON(browser, chart);
            return SerializationUtils.ChartDeserializerVerifReport(json);
        }

        public static List<CountryChartRootObject> GetCountryChartData(IWebDriver browser, IWebElement chart)
        {
            string json = GetDataSourceJSON(browser, chart);
            return SerializationUtils.ChartDeserializerCountry(json);
        }

        // ME 6/28/17: Using JQuery in the console. The below is the same as pasting the following into the Console tab of DEV tools of a browser:
        // JSON.stringify($(<IDOfElement>).igDataChart('option', 'dataSource'));
        // Or you could not stringify it, and see the Object Array itself by using: $(<IDOfElement>).igDataChart('option', 'dataSource')
        // See an example of a igDataChart here:
        // https://www.igniteui.com/data-chart/bar-and-column-series or here: https://www.igniteui.com/data-chart/composite-chart
        // We are stringifying it so that we can convert the resulting JSON to C# classes. You can go to http://json2csharp.com/ to see this manually
        private static string GetDataSourceJSON(IWebDriver browser, IWebElement _element)
        {
            // NOTE: For some charts, the text "dataSource" needs to be replaced with "series"
            string jsText = string.Format("return JSON.stringify($(arguments[0]).{0}('option', 'dataSource'));", "igDataChart");
            return browser.ExecuteScript(jsText, _element) as string;
        }
        #endregion
    }
}
