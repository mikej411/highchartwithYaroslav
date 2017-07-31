using System;
using System.IO;

namespace Browser.Core.Framework
{    
    /// <summary>
    /// Well-known settings for Selenium.Core.  These values are "keys" for retreiving the actual value
    /// 
    /// Typically, these are specified in the app.config file, but some may also be accessible via an attribute
    /// that can be applied to a test or test class.
    /// </summary>
    public static class SeleniumCoreSettings
    {
        #region Driver

        /// <summary>
        /// Specifies the location of the web driver executables.
        /// </summary>        
        public static readonly string DriverLocationKey = "DriverLocation";
        /// <summary>
        /// The default driver location is .\Drivers
        /// </summary>
        public static readonly string DriverLocationDefault = @"C:\chromedriver";
        /// <summary>
        /// Gets the location of the selenium drivers.  This property is guaranteed to have a trailing slash.
        /// </summary>        
        public static string DriverLocation { get; private set; }
        
        /// <summary>
        /// Specifies the location where driver logs will be saved.
        /// </summary>        
        public static readonly string DriverLogsLocationKey = "DriverLogsLocation";
        /// <summary>
        /// The default driver logs location is .\TestResults
        /// </summary>
        public static readonly string DriverLogsLocationDefault = @".\TestResults";
        /// <summary>
        /// Gets the driver logs location.  This property is guaranteed to have a trailing slash.
        /// </summary>        
        public static string DriverLogsLocation { get; private set; }

        #endregion Driver

        #region Mode

        /// <summary>
        /// Identifies whether to create a new browser instance or reuse an existing instance.  The default is Reuse.
        /// </summary>        
        public static readonly string BrowserModeKey = "BrowserMode";
        /// <summary>
        /// The default BrowserMode is BrowserMode.Reuse because it saves several seconds per test to avoid shutting down
        /// and re-initializing the browser.
        /// </summary>
        public static readonly BrowserMode BrowserModeDefault = Browser.Core.Framework.BrowserMode.New;
        /// <summary>
        /// Gets the browser mode to be used if not overridden at the Class or Method level by the BrowserModeAttribute.
        /// </summary>        
        public static BrowserMode BrowserMode { get; private set; }

        #endregion Mode

        #region Download

        /// <summary>
        /// The default download location that the tested browser will use when downloading a file.
        /// </summary>
        public static readonly string DefaultDownloadDirectoryKey = "DefaultDownloadDirectory";  //Environment.CurrentDirectory; 
        /// <summary>
        /// The default download directory that all downloads will be saved to.
        /// </summary>
        public static string DefaultDownloadDirectory { get; private set; }

        #endregion Download

        #region Grid

        /// <summary>
        /// Specifies the location of the Selenium Hub.
        /// </summary>
        public static readonly string HubUriKey = "HubUri";
        /// <summary>
        /// The default location of the Selenium Hub.
        /// </summary>
        public static string DefaultHubUri = "http://10.0.0.3:4444/wd/hub";
        /// <summary>
        /// Gets the location of the Selenium Hub.
        /// </summary>
        public static string HubUri { get; private set; }

        /// <summary>
        /// Specifies the location for Selenium Extras.
        /// </summary>
        public static readonly string ExtrasUriKey = "ExtrasUri";
        /// <summary>
        /// The default location of Selenium Extras.
        /// </summary>
        public static string DefaultExtrasUri = "http://10.0.0.115:3000/";
        /// <summary>
        /// Gets the location of Selenium Extras.
        /// </summary>
        public static string ExtrasUri { get; private set; }

        #endregion Grid

        #region Timeout

        /// <summary>
        /// Specifies the command timeout.
        /// </summary>      
        public static readonly string CommandTimeoutKey = "CommandTimeout";
        /// <summary>
        /// The default command timeout.
        /// </summary>
        public static TimeSpan DefaultCommandTImeout = TimeSpan.FromMinutes(3);
        /// <summary>
        /// Gets the command timeout.
        /// </summary>
        public static TimeSpan CommandTimeout { get; private set; }

        #endregion Timeout

        #region Screenshot

        /// <summary>
        /// Specifies the location where screenshots will be saved.
        /// </summary>        
        public static readonly string ScreenshotLocationKey = "ScreenShotLocation";
        /// <summary>                                        
        /// The default screenshot location is .\TestResults
        /// </summary>
        public static readonly string ScreenshotLocationDefault = @".\TestResults";
        /// <summary>
        /// Gets the screenshot location.  This property is guaranteed to have a trailing slash.
        /// </summary>        
        public static string ScreenshotLocation { get; private set; }


        /// <summary>
        /// Specifies the format to be used when naming screenshots.  Do NOT include the file extension.  All screenshots are saved as png files.
        /// Acceptable replacement tokens are:
        /// {driverName} - ChromeDriver
        /// {fullDriverName} - OpenQA.Selenium.Chrome.ChromeDriver
        /// {testName} - TestA
        /// {fullTestNameWithDriver} - CompName.Test.ClassA(OpenQA.Selenium.Chrome.ChromeDriver).TestA
        /// {className} - ClassA
        /// {fullClassName} - CompName.Test.ClassA
        /// {sessionId} - UUID of session
        /// {browserName} - e.g. chrome
        /// </summary>
        public static readonly string ScreenShotNameFormatKey = "ScreenShotNameFormat";
        /// <summary>
        /// The default value to use when formatting screenshot names
        /// </summary>
        public static readonly string ScreenShotNameFormatDefault = "{className}.{testName}.{browserName}";
        /// <summary>
        /// Gets the screen shot name format to use when saving screenshots.
        /// </summary>        
        public static string ScreenShotNameFormat { get; private set; }

        #endregion Screenshot

        static SeleniumCoreSettings()
        {
            // Driver
            DriverLocation = Environment.ExpandEnvironmentVariables(
                AppSettings.GetStringOrDefault(DriverLocationKey, DriverLocationDefault, ensureTrailing: @"\"));
            DriverLogsLocation = Environment.ExpandEnvironmentVariables(
                AppSettings.GetStringOrDefault(DriverLogsLocationKey, DriverLogsLocationDefault, ensureTrailing: @"\"));

            // Mode
            BrowserMode = AppSettings.GetEnumOrDefault<BrowserMode>(BrowserModeKey, BrowserModeDefault);

            // Download
            DefaultDownloadDirectory = Environment.ExpandEnvironmentVariables(
                AppSettings.GetStringOrDefault(DefaultDownloadDirectoryKey, Environment.CurrentDirectory, ensureTrailing: @"\"));

            // Grid
            HubUri = AppSettings.GetStringOrDefault(HubUriKey, DefaultHubUri);
            ExtrasUri = AppSettings.GetStringOrDefault(ExtrasUriKey, DefaultExtrasUri, ensureTrailing: @"/");

            // Timeout
            CommandTimeout = AppSettings.GetValueOrDefault<TimeSpan>(CommandTimeoutKey, DefaultCommandTImeout);

            // Screenshot
            ScreenshotLocation = AppSettings.GetStringOrDefault(ScreenshotLocationKey, ScreenshotLocationDefault, ensureTrailing: @"\");
            ScreenShotNameFormat = AppSettings.GetStringOrDefault(ScreenShotNameFormatKey, ScreenShotNameFormatDefault);
        }
    }
}
