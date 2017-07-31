using System.Configuration;

namespace RCP.AppFramework
{
    public static class Constants
    {

        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Constants));

        /// <summary>
        /// The connection string to the database of the Web Application
        /// </summary>
        public static readonly string SQLconnString = ConfigurationManager.AppSettings["SQLConnectionString"];

        /// <summary>
        /// The folder that contains the Selenium binaries (remote web browser drivers)
        /// </summary>
        //public  static readonly string SELENIUM_BIN_PATH = @"c:\Selenium\bin";

    }
}