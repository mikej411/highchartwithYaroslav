using NUnit.Framework;

namespace Browser.Core.Framework
{
    /// <summary>
    /// An NUnit test that intends to locally test in a browser.
    /// </summary>
    public class LocalSeleniumTestFixtureAttribute : TestFixtureAttribute
    {
        /// <summary>
        /// Constructor of a local test.
        /// </summary>
        /// <param name="browserName">The name of the browser to test from <see cref="BrowserNames"/></param>
        public LocalSeleniumTestFixtureAttribute(string browserName) : base(browserName)
        {
            Category = $"Local.{browserName}";
        }
    }
}
