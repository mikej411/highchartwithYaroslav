using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.Core.Framework
{
    /// <summary>
    /// Extension methods for IWebElement
    /// </summary>
    public static class WebElementExtensions
    {
        /// <summary>
        /// Clicks the element and wait for a file to download.
        /// </summary>
        /// <param name="self">The web element to be clicked.</param>
        /// <param name="browser">The browser.</param>
        /// <param name="fileName">Name of the file which will be downloaded (this should be a file name only.  It should not include any path).</param>
        /// <param name="timeout">The timeout.  This is how long the method will wait for the file to exist before throwing an exception.</param>
        /// <param name="deleteFirstIfExists">if set to <c>true</c> and the file already exists (prior to download), it will automatically be deleted.</param>
        /// <returns>The fully-qualified path of the downloaded file.</returns>
        /// <exception cref="System.TimeoutException">Thrown if the file does not exist within the timeout specified.</exception>
        public static string ClickAndWaitForDownload(this IWebElement self, IWebDriver browser, string fileName, TimeSpan timeout, bool deleteFirstIfExists)
        {
            var fullPath = SeleniumCoreSettings.DefaultDownloadDirectory + fileName;
            if (File.Exists(fullPath) && deleteFirstIfExists)
            {
                File.Delete(fullPath);
            }
            self.Click();
            return browser.WaitForDownload(fileName, timeout.TotalMilliseconds);            
        }
    }
}
