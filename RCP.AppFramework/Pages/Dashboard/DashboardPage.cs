using Browser.Core.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using LOG4NET = log4net.ILog;

namespace RCP.AppFramework
{
    public class DashboardPage : RCPPage, IDisposable
    {
        #region constructors
        public DashboardPage(IWebDriver driver) : base(driver)
        {
        }

        #endregion constructors

        #region properties

        private static readonly LOG4NET _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Keep track of the requests that I start so I can clean them up at the end.
        private List<string> activeRequests = new List<string>();

        public override string PageUrl { get { return "Default2.aspx"; } }

        #endregion properties

        #region elements

        // Main page
        public IWebElement EnterACPDActivityBtn { get { return this.FindElement(Bys.ActivitiesListPage.EnterACPDActivityBtn); } }


        #endregion elements

        #region methods: per page

        public override void WaitForInitialize()
        {
            this.WaitUntil(TimeSpan.FromSeconds(10), Criteria.DashboardPage.PageReady);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            try { activeRequests.Clear(); }
            catch (Exception ex) { _log.ErrorFormat("Failed to dispose DashboardPge", activeRequests.Count, ex); }
        }

        #endregion methods: per page

        #region methods: wrappers

        /// <summary>
        /// Clicks the user-specified button or link and then waits for a window to close or open, or a page to load,
        /// depending on the button that was clicked
        /// </summary>
        /// <param name="buttonOrLinkElem">The button element</param>
        public dynamic ClickButtonOrLinkToAdvance(IWebElement buttonOrLinkElem)
        {
            if (Browser.Exists(Bys.ActivitiesListPage.EnterACPDActivityBtn))
            {
                if (buttonOrLinkElem.GetAttribute("id") == EnterACPDActivityBtn.GetAttribute("id"))
                {
                    EnterACPDActivityBtn.Click();
                    Browser.WaitForElement(Bys.EnterCPDActivityPage.EnterACPDFrame, ElementCriteria.IsVisible);
                    EnterCPDActivityPage EAP = new EnterCPDActivityPage(Browser);
                    EAP.WaitForInitialize();
                    Browser.SwitchTo().Frame(EAP.EnterACPDFrame);
                    return EAP;
                }
            }

            else
            {
                throw new Exception("No button or link was found with your passed parameter");
            }

            return null;
        }

        #endregion wrappers


    }
}