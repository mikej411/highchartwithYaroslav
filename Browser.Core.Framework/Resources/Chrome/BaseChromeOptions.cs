using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.Core.Framework.Resources
{
    /// <summary>
    /// Base ChromeOptions for a new ChromeDriver.
    /// </summary>
    public class BaseChromeOptions : ChromeOptions
    {
        /// <summary>
        /// Default constructor that sets some default options.
        /// </summary>
        public BaseChromeOptions()
            : base()
        {
            SetDefaultDownloadDirectory(SeleniumCoreSettings.DefaultDownloadDirectory);
            SetPromptForDownload(false);
            SetDownloadDirectoryUpgrade(true);
            AddArgument("no-sandbox");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory"></param>
        public virtual void SetDefaultDownloadDirectory(string directory)
        {
            AddUserProfilePreference("download.default_directory", directory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="promptForDownload"></param>
        public virtual void SetPromptForDownload(bool promptForDownload)
        {
            AddUserProfilePreference("download.prompt_for_download", promptForDownload);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="downloadDirectoryUpgrade"></param>
        public virtual void SetDownloadDirectoryUpgrade(bool downloadDirectoryUpgrade)
        {
            AddUserProfilePreference("download.directory_upgrade", downloadDirectoryUpgrade);
        }
    }
}
