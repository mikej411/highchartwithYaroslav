using Browser.Core.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using LOG4NET = log4net.ILog;
using System.Threading;

namespace RCP.AppFramework
{
    public class EnterCPDActivityPage : RCPPage, IDisposable
    {
        #region constructors
        public EnterCPDActivityPage(IWebDriver driver) : base(driver)
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

        public IWebElement EnterACPDFrame { get { return this.FindElement(Bys.EnterCPDActivityPage.EnterACPDFrame); } }
        public SelectElement Sec1GroupLearnActSel { get { return new SelectElement(this.WaitForElement(Bys.EnterCPDActivityPage.Sec1GroupLearnActSel)); } }
        public IWebElement ActivityAccrYesRdo { get { return this.FindElement(Bys.EnterCPDActivityPage.ActivityAccrYesRdo); } }
        public IWebElement ActivityAccrNoRdo { get { return this.FindElement(Bys.EnterCPDActivityPage.ActivityAccrNoRdo); } }
        public IWebElement HoursTxt { get { return this.FindElement(Bys.EnterCPDActivityPage.HoursTxt); } }
        public IWebElement GroupActivityNameTxt { get { return this.FindElement(Bys.EnterCPDActivityPage.GroupActivityNameTxt); } }
        public IWebElement DateTxt { get { return this.FindElement(Bys.EnterCPDActivityPage.DateTxt); } }
        public IWebElement WhatDidYouLearnTxt { get { return this.FindElement(Bys.EnterCPDActivityPage.WhatDidYouLearnTxt); } }
        public IWebElement WhatAdditLearningTxt { get { return this.FindElement(Bys.EnterCPDActivityPage.WhatAdditLearningTxt); } }
        public IWebElement WhatChangesTxt { get { return this.FindElement(Bys.EnterCPDActivityPage.WhatChangesTxt); } }
        public IWebElement AddFilesBtn { get { return this.FindElement(Bys.EnterCPDActivityPage.AddFilesBtn); } }
        public IWebElement CancelBtn { get { return this.FindElement(Bys.EnterCPDActivityPage.CancelBtn); } }
        public IWebElement SendToHoldingBtn { get { return this.FindElement(Bys.EnterCPDActivityPage.SendToHoldingBtn); } }
        public IWebElement ContinueBtn { get { return this.FindElement(Bys.EnterCPDActivityPage.ContinueBtn); } }
        public IWebElement SubmitBtn { get { return this.FindElement(Bys.EnterCPDActivityPage.SubmitBtn); } }
        public IWebElement CloseBtn { get { return this.FindElement(Bys.EnterCPDActivityPage.CloseBtn); } }


        #endregion elements

        #region methods: per page

        public override void WaitForInitialize()
        {
            this.WaitUntil(TimeSpan.FromSeconds(10), Criteria.EnterCPDActivityPage.PageReady);
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
        /// When the Enter a CPD Activity form is open, this will fill the form with randomized data. We can
        /// refactor this later if we want to use data in XML how other projects are using. However, this returns
        /// an object with the randomized data, so it can be accessed if need be.
        /// </summary>
        /// <param name="isActivityAccredited">"true" is you want credits, "false" is not</param>
        /// <returns></returns>
        public SelfLearningObject FillFormEnterACPDActivity(bool isActivityAccredited)
        {
            string activityGroup = ElemSet.SelectRandomItemFromSingleSelectDropDown(Sec1GroupLearnActSel);

            Browser.WaitForElement(Bys.EnterCPDActivityPage.ActivityAccrYesRdo, ElementCriteria.IsVisible);
            if (isActivityAccredited)
            {
                ActivityAccrYesRdo.Click();
            }
            else
            {
                ActivityAccrNoRdo.Click();
            }

            Browser.WaitForElement(Bys.EnterCPDActivityPage.HoursTxt, ElementCriteria.IsEnabled);
            // Adding a sleep here. Havent researched further yet to figure out what criteria needs to be waited for
            Thread.Sleep(2000);
            int hours = DataUtils.GetRandomIntegerWithinRange(1, 20);
            HoursTxt.SendKeys(hours.ToString());

            string groupActivityName = DataUtils.GetRandomString(10);
            GroupActivityNameTxt.SendKeys(groupActivityName);

            string dateCompleted = DateTime.Now.ToString("MM/dd/yyyy");
            DateTxt.SendKeys(dateCompleted);

            string whatWasLearned = DataUtils.GetRandomSentence(20);
            WhatDidYouLearnTxt.SendKeys(whatWasLearned);

            string whatAdditionalLearning = DataUtils.GetRandomSentence(20);
            WhatAdditLearningTxt.SendKeys(whatAdditionalLearning);

            string changesPlanned = DataUtils.GetRandomSentence(20);
            WhatChangesTxt.SendKeys(changesPlanned);

            return new SelfLearningObject(activityGroup, isActivityAccredited, hours, groupActivityName, dateCompleted, whatWasLearned,
                whatAdditionalLearning, changesPlanned);            
        }

        /// <summary>
        /// Clicks the user-specified button or link and then waits for a window to close or open, or a page to load,
        /// depending on the button that was clicked
        /// </summary>
        /// <param name="buttonOrLinkElem">The button element</param>
        public dynamic ClickButtonOrLinkToAdvance(IWebElement buttonOrLinkElem)
        {
            if (Browser.Exists(Bys.EnterCPDActivityPage.ContinueBtn))
            {
                if (buttonOrLinkElem.GetAttribute("id") == ContinueBtn.GetAttribute("id"))
                {
                    ContinueBtn.Click();
                    Browser.WaitForElement(Bys.EnterCPDActivityPage.SubmitBtn, ElementCriteria.IsVisible);
                    return null;
                }
            }

            else if (Browser.Exists(Bys.EnterCPDActivityPage.SubmitBtn))
            {
                if (buttonOrLinkElem.GetAttribute("id") == SubmitBtn.GetAttribute("id"))
                {
                    SubmitBtn.Click();
                    Browser.WaitForElement(Bys.EnterCPDActivityPage.CloseBtn, ElementCriteria.IsVisible);
                    return null;
                }
            }

            else if (Browser.Exists(Bys.EnterCPDActivityPage.CloseBtn))
            {
                if (buttonOrLinkElem.GetAttribute("id") == CloseBtn.GetAttribute("id"))
                {
                    CloseBtn.Click();
                    Browser.WaitForElement(Bys.DashboardPage.EnterACPDActivityBtn, ElementCriteria.IsVisible);
                    return null;
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

    #region additional classes

    public class SelfLearningObject
    {
        public string GroupLearningActivity { get; set; }
        public bool IsActivityAccredited { get; set; }
        int HoursSpent { get; set; }
        public string GroupActivityName { get; set; }
        public string DateCompleted { get; set; }
        public string WhatWasLearned { get; set; }
        public string WhatWasLearnedAdditionally { get; set; }
        public string WhatChangesArePlanned { get; set; }

        public SelfLearningObject(string groupLearningActivity, bool isActivityAccredited, int hoursSpent, string groupActivityName,
            string dateCompleted, string whatWasLearned, string whatWasLearnedAdditionally, string whatChangesArePlanned)
        {
            GroupLearningActivity = groupLearningActivity;
            IsActivityAccredited = isActivityAccredited;
            HoursSpent = hoursSpent;
            GroupActivityName = groupActivityName;
            DateCompleted = dateCompleted;
            WhatWasLearned = whatWasLearned;
            WhatWasLearnedAdditionally = whatWasLearnedAdditionally;
            WhatChangesArePlanned = whatChangesArePlanned;

        }

   
    }

    #endregion additional classes

}