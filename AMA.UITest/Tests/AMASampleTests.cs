using Browser.Core.Framework;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using AMA.AppFramework;


namespace AMA.UITest
{
    //[BrowserMode(BrowserMode.New)]
    [LocalSeleniumTestFixture(BrowserNames.Chrome)]
    //[LocalSeleniumTestFixture(BrowserNames.Firefox)]
    //[LocalSeleniumTestFixture(BrowserNames.InternetExplorer)]
    [RemoteSeleniumTestFixture(BrowserNames.Chrome)]
    //[RemoteSeleniumTestFixture(BrowserNames.Firefox)]
    //[RemoteSeleniumTestFixture(BrowserNames.InternetExplorer)]
    [TestFixture]
    public class AMASampleTests : TestBase
    {
        #region Constructors
        public AMASampleTests(string browserName) : base(browserName) { }

        // Remote Selenium Grid Test
        public AMASampleTests(string browserName, string version, string platform, string hubUri, string extrasUri)
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
            LP.UserNameTxt.SendKeys("AutoAdmin");
            LP.PasswordTxt.SendKeys("password");
            LP.PasswordTxt.SendKeys(Keys.Tab);
            LP.ClickButtonOrLinkToAdvance(LP.LoginBtn);
        }
        #endregion Tests
    }
}

