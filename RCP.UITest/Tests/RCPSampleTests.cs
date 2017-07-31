using Browser.Core.Framework;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using RCP.AppFramework;
using System.Data;
using OpenQA.Selenium.Support.UI;
using System;

namespace RCP.UITest
{
    //[BrowserMode(BrowserMode.New)]
    [LocalSeleniumTestFixture(BrowserNames.Chrome)]
    //[LocalSeleniumTestFixture(BrowserNames.Firefox)]
    //[LocalSeleniumTestFixture(BrowserNames.InternetExplorer)]
    [RemoteSeleniumTestFixture(BrowserNames.Chrome)]
    //[RemoteSeleniumTestFixture(BrowserNames.Firefox)]
    //[RemoteSeleniumTestFixture(BrowserNames.InternetExplorer)]
    [TestFixture]
    public class RCPSampleTests : TestBase
    {
        #region Constructors
        public RCPSampleTests(string browserName) : base(browserName) { }

        // Remote Selenium Grid Test
        public RCPSampleTests(string browserName, string version, string platform, string hubUri, string extrasUri)
                                    : base(browserName, version, platform, hubUri, extrasUri)
        { }
        #endregion

        /// <summary>
        /// Example of how to override the teardown at the test class level
        /// </summary>
        //public override void TearDown() 
        //{
        //    Browser.Manage().Window.Size = new System.Drawing.Size(1040, 784);
        //    CleanupBrowser();
        //}

        #region Tests

        /// <summary>
        /// Verifying everything within the login page. Any validations that are in place, etc. It also tests that a user is able
        /// to login successfully
        /// </summary>
        [Test]
        [Category("Integration"), Category("IntegrationQA")]
        public void TestLoginPage()
        {
            // Navigate to the login page
            LoginPage LP = Navigation.GoToLoginPage(browser);
            
            // Click the login button without entering in the username or password, and then verify the warning messages
            LP.LoginBtn.Click();
            browser.WaitForElement(Bys.LoginPage.UserNameWarningLbl, ElementCriteria.IsVisible);
            Assert.AreEqual("Please enter your username.", LP.UserNameWarningLbl.Text);
            Assert.True(AssertUtils.VerifyLabel(browser, LP.UserNameWarningLbl, "Please enter your username.", "rgba(255, 0, 0, 1)"),
                "The label's text, display property, or CSS color value is not correct");

            // Enter text in the required fields and verify the warning messages disappear
            LP.UserNameTxt.SendKeys("Not a valid user");
            LP.PasswordTxt.SendKeys("blah");
            LP.PasswordTxt.SendKeys(Keys.Tab);
            // Two ways to check that the warning message disappeared
            // Through selenium:
            Assert.False(LP.UserNameWarningLbl.Displayed);
            // Or through my custom extension method
            Assert.False(Browser.Exists(Bys.LoginPage.UserNameWarningLbl, ElementCriteria.IsVisible));

            // The user above does not exist, so click the Login and verify the system warns the user
            LP.LoginBtn.Click();
            browser.WaitForElement(Bys.LoginPage.LoginUnsuccessfullWarningLbl, ElementCriteria.IsVisible);
            Assert.True(AssertUtils.VerifyLabel(browser, LP.LoginUnsuccessfullWarningLbl,
                "Your login attempt was not successful. Please try again.", "rgba(255, 0, 0, 1)"));

            // Login with a valid user
            LP.UserNameTxt.Clear();
            LP.PasswordTxt.Clear();
            LP.UserNameTxt.SendKeys("ADEULA11");
            LP.PasswordTxt.SendKeys("test");
            LP.PasswordTxt.SendKeys(Keys.Tab);
            LP.ClickButtonOrLinkToAdvance(LP.LoginBtn);
        }

        /// <summary>
        /// Verifying everything within the Enter a New CPD Activity form. Making sure the list boxes contain the correct data, any 
        /// validations that are in place when the Continue button is clicked, various element verification
        /// to login successfully
        /// </summary>
        [Test]
        [Category("Integration"), Category("IntegrationQA")]
        public void TestActivityForm()
        {
            // Navigate to the login page
            LoginPage LP = Navigation.GoToLoginPage(browser);

            // Wrapper to login
            DashboardPage DP = LP.LoginWithUser("ADEULA11", "test");

            // Open the Enter a CPD Activity form
            EnterCPDActivityPage EAP = DP.ClickButtonOrLinkToAdvance(DP.EnterACPDActivityBtn);

            // Verify the contents of the dropdowns
            // Note: DBUtils class can do DB verifications, but we are only doing UI verifications of static data. 
            //List<string> groupLearningActivitiesExpected = DbUtils.GetGroupLearningActivities();
            List<string> groupLearningActivitiesExpected = new List<string>() { "", "Conference", "Journal Club", "Rounds", "Small Group Session" };
            List<string> groupLearningActivitiesActual = ElemGet.SelectElementListItemTextToListString(EAP.Sec1GroupLearnActSel);
            Assert.AreEqual(groupLearningActivitiesExpected, groupLearningActivitiesActual);

            // ToDO: Verify the rest of the elements, any dependencies, validations, etc. (Basically any more functionality 
            // that pertains to this form)
        }

        /// <summary>
        /// Verifying that a user can add an activity, and that the activity gets saved within the system
        /// </summary>
        [Test]
        [Category("Integration"), Category("IntegrationQA")]
        public void AddActivity()
        {
            // Navigate to the login page
            LoginPage LP = Navigation.GoToLoginPage(browser);

            // Wrapper to login to the system. Hardcoded for now. Can implement Arbab's dynamic user logic later
            DashboardPage DP = LP.LoginWithUser("ADEULA11", "test");

            // Open the Enter a CPD Activity form
            EnterCPDActivityPage EAP = DP.ClickButtonOrLinkToAdvance(DP.EnterACPDActivityBtn);

            // Fill the form with random data. NOTE: Can make this compatible with existing Premier framework so that the form 
            // fills with existing XML data that other members have created
            SelfLearningObject SLO = EAP.FillFormEnterACPDActivity(true);

            EAP.ClickButtonOrLinkToAdvance(EAP.ContinueBtn);
            EAP.ClickButtonOrLinkToAdvance(EAP.SubmitBtn);
            EAP.ClickButtonOrLinkToAdvance(EAP.CloseBtn);

            // Navigate to the Activities List page and verify that the activity is showing in the table
            ActivitiesListPage AP = RCPPage.NavigateThroughMenuItems(browser, Bys.RCPPage.Menu_MyCPDActivitiesList);

            // To verify the activity is in the table, we use the SelfLearningObject, which contains the randomized name we gave the group
            // ToDo: We can do a lot more verifications here, such as verifying the record added has the correct information, etc.
            Assert.True(ElemGet.GridContainsRecord(AP.ActivityTblBody, SLO.GroupActivityName), "The activity has not been added to the grid");

            // Cleanup. Delete the activity and make sure it got removed from the table
            AP.DeleteActivityFromGrid(SLO.GroupActivityName);
            Assert.False(ElemGet.GridContainsRecord(AP.ActivityTblBody, SLO.GroupActivityName), "The activity has not been deleted");


        }


        [Test]
        public void test()
        {

            browser.Navigate().GoToUrl("https://www.igniteui.com/data-chart/bar-and-column-series");        
            IWebElement igChartElem = (new WebDriverWait(browser, TimeSpan.FromSeconds(30)))
                .Until(ExpectedConditions.ElementIsVisible(By.Id("columnChart")));
            //IWebElement elem = browser.FindElement(By.Id("columnChart"));
            DataTable igChartDT = BrowserExtensions.GetDataFromIgDataChart(browser, igChartElem);
            List<CountryChartRootObject> igChartObj = BrowserExtensions.GetCountryChartData(browser, igChartElem);





            browser.Navigate().GoToUrl("https://rcpsc.releasecandidate-community360.net/login.aspx?action=enablelogin");

            IWebElement userNameTxt = (new WebDriverWait(browser, TimeSpan.FromSeconds(30)))
                .Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_ContentPlaceHolder1_cpLoginAspx_ctl00_LoginControl1_LTLogin_UserName")));
            userNameTxt.SendKeys("JasonOt_learner5_19_1");
            browser.FindElement(By.Id("ctl00_ContentPlaceHolder1_cpLoginAspx_ctl00_LoginControl1_LTLogin_Password")).SendKeys("test");
            browser.FindElement(By.Id("ctl00_ContentPlaceHolder1_cpLoginAspx_ctl00_LoginControl1_LTLogin_Login")).Click();

            IWebElement highchartsElem = (new WebDriverWait(browser, TimeSpan.FromSeconds(30)))
                .Until(ExpectedConditions.ElementIsVisible(By.Id("EPAChart")));

            // the below does not work with Highcharts. It only works with igniteUICharts. I need methods to get the chart data from highcharts now
            DataTable highchartsDT = BrowserExtensions.GetDataFromIgDataChart(browser, highchartsElem);
            List<CountryChartRootObject> highchartsObj = BrowserExtensions.GetCountryChartData(browser, highchartsElem);
        }
        #endregion Tests
    }
}

