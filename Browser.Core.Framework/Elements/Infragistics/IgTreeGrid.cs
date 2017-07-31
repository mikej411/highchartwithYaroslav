using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.Core.Framework
{
    /// <summary>
    /// Wrapper for the igTreeGrid control that allows accessing the datasource, etc 
    /// </summary>
    /// <seealso cref="Browser.Core.Framework.IgGrid" />
    public class IgTreeGrid : IgGrid
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IgTreeGrid"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="driver">The driver.</param>
        public IgTreeGrid(IWebElement element, IWebDriver driver)
            : base(element, driver, "igTreeGrid")
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IgTreeGrid"/> class.
        /// Provided for inheritance purposes
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="driver">The driver.</param>
        /// <param name="controlName">Name of the control.</param>
        protected IgTreeGrid(IWebElement element, IWebDriver driver, string controlName)
            :base(element, driver, controlName)
        { }

        #endregion
    }
}
