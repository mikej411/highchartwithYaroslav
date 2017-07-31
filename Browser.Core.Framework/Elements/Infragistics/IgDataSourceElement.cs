using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using OpenQA.Selenium.Internal;
using Newtonsoft.Json;

namespace Browser.Core.Framework
{
    /// <summary>
    /// Base class for Infragistics DOM elements that contain a datasource
    /// </summary>
    /// <seealso cref="OpenQA.Selenium.Internal.IWrapsElement" />
    /// <seealso cref="OpenQA.Selenium.Internal.IWrapsDriver" />
    public abstract class IgDataSourceElement : IWrapsElement, IWrapsDriver
    {
        #region Fields

        private IWebElement _element;
        private IWebDriver _driver;
        private string _controlName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IgDataSourceElement"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="driver">The driver.</param>
        /// <param name="controlName">Name of the control.</param>
        /// <exception cref="System.ArgumentNullException">
        /// element
        /// or
        /// driver
        /// or
        /// controlName
        /// </exception>
        public IgDataSourceElement(IWebElement element, IWebDriver driver, string controlName)
        {            
            if (element == null)
                throw new ArgumentNullException("element");
            if (driver == null)
                throw new ArgumentNullException("driver");
            if (controlName == null)
                throw new ArgumentNullException("controlName");
            _element = element;
            _driver = driver;
            _controlName = controlName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="T:OpenQA.Selenium.IWebElement" /> wrapped by this object.
        /// </summary>
        public IWebElement WrappedElement
        {
            get { return _element; }
        }

        /// <summary>
        /// Gets the <see cref="T:OpenQA.Selenium.IWebDriver" /> used to find this element.
        /// </summary>
        public IWebDriver WrappedDriver
        {
            get { return _driver; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the data source of the element.  This method retrieves the raw json data from the control
        /// and then uses JSON.NET to deserialize the data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IEnumerable<T> GetDataSource<T>()
        {
            var jsonData = GetDataSourceJSON();
            var dataSource = JsonConvert.DeserializeObject<IEnumerable<T>>(jsonData);
            return dataSource;
        }

        /// <summary>
        /// Returns true if the ig control has been initialized; false if otherwise.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsInitialized()
        {
            string jsText = string.Format("return $(arguments[0]).data('{0}') != null;", _controlName);
            return (Boolean)_driver.ExecuteScript(jsText, _element);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets the data source from the ig control by calling JSON.stringify on the dataSource property.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDataSourceJSON()
        {
            string jsText = string.Format("return JSON.stringify($(arguments[0]).{0}('option', 'dataSource'));", _controlName);
            return _driver.ExecuteScript(jsText, _element) as string;
        }

        #endregion
    }
}
