using OpenQA.Selenium.Firefox;

namespace Browser.Core.Framework.Resources
{
    /// <summary>
    /// Base FirefoxProfile for a new FirefoxDriver.
    /// </summary>
    public class BaseFirefoxProfile : FirefoxProfile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFirefoxProfile"/> class.
        /// </summary>
        public BaseFirefoxProfile() 
            : base()
        {
            SetPreference("browser.startup.homepage", "http://www.seleniumhq.org/");

            SetDownloadPreferences();
            SetProxyPreferences();
        }

        #region Download Preferences

        /// <summary>
        /// Set the profile download preferences.
        /// </summary>
        public virtual void SetDownloadPreferences()
        {
            SetPreference("browser.download.dir", SeleniumCoreSettings.DefaultDownloadDirectory);
            SetPreference("browser.download.folderList", 2);
            SetPreference("browser.helperApps.neverAsk.saveToDisk",
                "vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/json)");
        }

        #endregion Download Preferences

        #region Proxy Preferences

        /// <summary>
        /// Set the profile proxy preferences.
        /// </summary>
        public virtual void SetProxyPreferences()
        {
            // NOTE: proxy settings needed before using the grid.

            //SetPreference("network.proxy.backup.ftp", "firewall..com");
            //SetPreference("network.proxy.backup.ftp_port", 8080);
            //SetPreference("network.proxy.backup.socks", "firewall..com");
            //SetPreference("network.proxy.backup.socks_port", 8080);
            //SetPreference("network.proxy.backup.ssl", "firewall..com");
            //SetPreference("network.proxy.backup.ssl_port", 8080);
            //SetPreference("network.proxy.ftp", "firewall..com");
            //SetPreference("network.proxy.ftp_port", 8080);
            //SetPreference("network.proxy.http", "firewall..com");
            //SetPreference("network.proxy.http_port", 8080);
            //SetPreference("network.proxy.no_proxies_on",
            //    "localhost, 127.0.0.1, *.com, *.com, *ftp*..com, , ,<local>");
            //SetPreference("network.proxy.share_proxy_settings", true);
            //SetPreference("network.proxy.socks", "firewall..com");
            //SetPreference("network.proxy.socks_port", 8080);
            //SetPreference("network.proxy.ssl", "firewall..com");
            //SetPreference("network.proxy.ssl_port", 8080);
            //SetPreference("network.proxy.type", 1);
        }

        #endregion Proxy Preferences
    }
}
