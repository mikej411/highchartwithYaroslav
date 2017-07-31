using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.Core.Framework.Resources
{
    /// <summary>
    /// Base InternetExplorerOptions for a new InternetExplorerDriver.
    /// </summary>
    public class BaseInternetExplorerOptions : InternetExplorerOptions
    {
        /// <summary>
        /// Default constructor that sets some default options.
        /// </summary>
        public BaseInternetExplorerOptions()
            : base()
        {
            //InitialBrowserUrl = "http://www.seleniumhq.org/";

            // This is important to ignore the zoom level or an exception may be thrown from the constructor
            // of InternetExplorerDriver which leaves us no way to cleanup the IEDriverServer.exe
            // Note that it's STILL IMPORTANT that the zoom level be 100%, so we later force the zoom level
            // back to 100%.
            IgnoreZoomLevel = true;
            IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            // We're currently experiencing a problem where click events fail to happen if the browser
            // is not the active (foreground) application.  Based on the blogs below there doesn't seem to be any
            // known workarounds for this issue.  The only proposed solution is to switch to using javascript-based
            // events instead of native events.  EnableNativeEvents=false switches to javascript events.  We're going to 
            // give this a try for a while and see how it goes.
            // http://jimevansmusic.blogspot.com/2012/06/whats-wrong-with-internet-explorer.html
            // http://jimevansmusic.blogspot.com/2013/01/revisiting-native-events-in-ie-driver.html
            EnableNativeEvents = false;
            // Don't auto-dismiss unexpected alerts.  This allows us to log what the alert message is
            UnexpectedAlertBehavior = InternetExplorerUnexpectedAlertBehavior.Ignore;
        }
    }
}