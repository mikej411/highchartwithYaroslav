using Browser.Core.Framework;
using OpenQA.Selenium;

namespace Browser.Core.Framework
{
    public static class AssertUtils
    {
        /// <summary>
        /// Checks that a label is visible, and has the user-specified text and color
        /// </summary>
        /// <param name="label">The label to verify</param>
        /// <param name="textExpected">The text expected of the label</param>
        /// <param name="colorExpected">The color expected of the label</param>
        public static bool VerifyLabel(IWebDriver browser, IWebElement label, string textExpected, string colorExpected)
        {
            bool existsActual = browser.Exists(By.Id(label.GetAttribute("id")));
            bool displayedActual = label.Displayed;
            string labelTextActual = label.Text.Replace(System.Environment.NewLine, " ");
            string colorValueActual = label.GetCssValue("color");

            if (existsActual = true && displayedActual == true && labelTextActual == textExpected && colorValueActual == colorExpected)
                return true;
            else return false;
        }
    }
}