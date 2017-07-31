using Browser.Core.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using LOG4NET = log4net.ILog;
using System.Threading;

namespace RCP.AppFramework
{
    public class ActivitiesListPage : RCPPage, IDisposable
    {
        #region constructors
        public ActivitiesListPage(IWebDriver driver) : base(driver)
        {
        }

        #endregion constructors

        #region properties

        private static readonly LOG4NET _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Keep track of the requests that I start so I can clean them up at the end.
        private List<string> activeRequests = new List<string>();

        public override string PageUrl { get { return "0/CPDActivities.aspx"; } }

        #endregion properties

        #region elements

        // Main page
        public IWebElement EnterACPDActivityBtn { get { return this.FindElement(Bys.ActivitiesListPage.EnterACPDActivityBtn); } }
        public IWebElement ActivityTblBody { get { return this.FindElement(Bys.ActivitiesListPage.ActivityTblBody); } }
        // Delete Activity Form
        public IWebElement DeleteActivityForm { get { return this.FindElement(Bys.ActivitiesListPage.DeleteActivityForm); } }
        public IWebElement DeleteActivityFormOkBtn { get { return this.FindElement(Bys.ActivitiesListPage.DeleteActivityFormOkBtn); } }
        public IWebElement DeleteActivityFormCancelBtn { get { return this.FindElement(Bys.ActivitiesListPage.DeleteActivityFormCancelBtn); } }




        #endregion elements

        #region methods: per page

        public override void WaitForInitialize()
        {
            this.WaitUntil(TimeSpan.FromSeconds(10), Criteria.ActivitiesListPage.PageReady);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            try { activeRequests.Clear(); }
            catch (Exception ex) { _log.ErrorFormat("Failed to dispose HomePage", activeRequests.Count, ex); }
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

            if (Browser.Exists(Bys.ActivitiesListPage.DeleteActivityFormOkBtn))
            {
                if (buttonOrLinkElem.GetAttribute("id") == DeleteActivityFormOkBtn.GetAttribute("id"))
                {
                    DeleteActivityFormOkBtn.Click();
                    WaitUtils.WaitElementNotVisible(Browser, Bys.ActivitiesListPage.DeleteActivityFormOkBtn);
                    Browser.WaitForElement(Bys.ActivitiesListPage.ActivityTblBody, ElementCriteria.IsVisible);
                    // Adding a sleep here, havent figured out what to wait for, for it to work yet
                    Thread.Sleep(4000);
                    return null;
                }
            }
          
            else
            {
                throw new Exception("No button or link was found with your passed parameter");
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupActivityName"></param>
        public void DeleteActivityFromGrid(string groupActivityName)
        {
            // First find the hyperlink of the group activity we want to delete
            string xpathForGroupActivityHyperLnk = string.Format("//a[text()='{0}']", groupActivityName);          
            IWebElement groupActivityHyperLnk = ActivityTblBody.FindElement(By.XPath(xpathForGroupActivityHyperLnk));

            // Next get the cell and then the row where this activity exists 
            IWebElement groupActivityCell = groupActivityHyperLnk.FindElement(By.XPath(".."));
            IWebElement groupActivityRow = groupActivityCell.FindElement(By.XPath(".."));

            // Finally, get the X button within th row
            IWebElement xBtn = groupActivityRow.FindElement(By.XPath("//input[@title='Delete']"));

            xBtn.Click();
            Browser.WaitForElement(Bys.ActivitiesListPage.DeleteActivityFormOkBtn, ElementCriteria.IsVisible);
            ClickButtonOrLinkToAdvance(DeleteActivityFormOkBtn);
        }




        // This XPath line selects the first TD element with the exact text (or that contains the text) that is in the orgName variable
        //string xPathVariable = "//td[./text()='yourtext']";
        //string xPathVariable = "//td[contains(text(),'yourtext')]";
        //string xPathVariable = string.Format("//div[contains(.,'{0}')]", textOfCell);
        //IWebElement organizationNameTDCell = gridElem.FindElement(By.XPath(xPathVariable));

        // Mulitple elements or multiple attributes
        //string xpathString = string.Format("//span[text()='{0}' and @class=\"ui-iggrid-headertext\"]", textOfHeaderCell);

        // Attribute does not equal
        //IWebElement lists = Browser.FindElement(By.CssSelector("li:not([class=hidden])"));

        #endregion wrappers


    }
}