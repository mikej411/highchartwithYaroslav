using OpenQA.Selenium.Firefox;

namespace Browser.Core.Framework.Resources
{
    /// <summary>
    /// Base FirefoxOptions for a new FirefoxDriver.
    /// </summary>
    public class BaseFirefoxOptions : FirefoxOptions
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public BaseFirefoxOptions()
        {
            IsMarionette = false;
        }
    }
}
