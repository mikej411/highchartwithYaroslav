using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Browser.Core.Framework
{
    /// <summary>
    /// A utility class for manipulating elements
    /// </summary>
    public static class ElemSet
    {
        #region DropDownListButtons

        /// <summary>
        /// Chrome browser is failing in tests when simply using multiselectdropdown.click(). For some reason in this browser, whenever a dropdown
        /// gets expanded, a new element is created in the HTML with a classname of dropdown-backdrop. This new element blocks Selenium from all of the
        /// controls within the form, so if the user tries to do a multiselectdropdown.click(), selenium says it failed to click on that dropdown, and
        /// instead says the dropdown-backdrop would get the click. So the below method takes that into account and works to open and close the control
        /// </summary>
        /// <param name="elem">The button element of the multi-select dropdown</param>
        public static void ClickMultiSelectDropDownButton(IWebDriver browser, IWebElement elem)
        {
            if (browser.FindElements(By.ClassName("dropdown-backdrop")).Count > 0)
            {
                var form = browser.FindElement(By.ClassName("dropdown-backdrop"));
                form.Click();
                Thread.Sleep(0500);
            }
            else
            {
                elem.Click();
            }
        }

        /// <summary>
        /// Selects an item by name in a dropdown that is already expanded. This is an alternative to Selenium's built-in Select class Select methods.
        /// Use it instead when you want to select an item while the control is expanded. It will be left expanded upon completion if the list
        /// is multi-select, allowing the user to test specific test steps with the control expanded.
        /// </summary>
        /// <param name="elem">The element from which you are selecting from. It must be of type IWebElement, not SelectElement</param>
        /// <param name="itemName">The item to choose</param>
        public static void ClickListItemByName(IWebElement elem, string itemName)
        {
            // Find the parent element of the drop down. So we can then find a child DIV element
            var ParentElement = elem.FindElement(By.XPath(".."));
            
            // This DIV element contains the list items, whether filtered or not. If values are filtered, there is code in the line after
            // this to handle that. i.e. class<>hidden
            var DivElem = ParentElement.FindElement(By.CssSelector("ul[role=listbox]"));
            
            // Store all li elements within the Div element
            var liElems = DivElem.FindElements(By.CssSelector("li:not([class=hidden])")).ToList();
            foreach (var liElem in liElems)
            {
                // if the current list item's text value in the for loop equals the users passed parameter itemName
                if (liElem.Text == itemName)
                {
                    // Finding the A tag because IE has trouble clicking on LI items. So we are going to click on the A tag inside of the LI item for all browsers
                    IWebElement aTagOfLIElem = liElem.FindElement(By.TagName("a"));
                    aTagOfLIElem.Click();
                    Thread.Sleep(0200);
                    break;
                }
            }
        }

        /// <summary>
        /// Clicks an item by index in a dropdown that is already expanded. This is an alternative to Selenium's built-in Select class Select methods.
        /// Use it instead when you want to click an item while the control is expanded. It will be left expanded upon completion if the list
        /// is multi-select, allowing the user to test specific test steps with the control expanded.
        /// </summary>
        /// <param name="elem">The IWebElement you are clicking the item from</param>
        /// <param name="index">The index of the item you want to choose</param>
        public static void ClickListItemByIndex(IWebElement elem, int index)
        {
            // Find the parent element of the drop down. So we can then find a child DIV element
            var ParentElement = elem.FindElement(By.XPath(".."));
            
            // This DIV element contains the list items, whether filtered or not. If values are filtered, there is code in the line after
            // this to handle that. i.e. class<>hidden
            var DivElem = ParentElement.FindElement(By.CssSelector("ul[role=listbox]"));
            
            // Store all li elements within the Div element that do not have a class that is either equal to "hidden" or "selected hidden"
            var liElems = DivElem.FindElements(By.CssSelector("li:not([class=hidden]):not([class='selected hidden'])"));
            
            // Finding the A tag because IE has trouble clicking on LI items. So we are going to click on the A tag inside of the LI item for all browsers
            IWebElement aTagOfLIElem = liElems[index].FindElement(By.TagName("a"));
            aTagOfLIElem.Click();
            Thread.Sleep(0200);
        }

        /// <summary>
        /// Selects a random item in a single select dropdown. It will not select the already selected item.
        /// </summary>
        /// <param name="elem">The SelectElement that contains the items</param>
        public static string SelectRandomItemFromSingleSelectDropDown(SelectElement selectElem)
        {
            for (var i = 0; i < 100; i++)
            {
                Random random = new Random();
                int randomInt = DataUtils.GetRandomIntegerWithinRange(0, selectElem.Options.Count);
                if (selectElem.SelectedOption.Text != selectElem.Options[randomInt].Text)
                {
                    selectElem.SelectByIndex(randomInt);
                    break;
                }
            }
            return selectElem.SelectedOption.Text;
        }

        /// <summary>
        /// Selects a user-specified number of random items in a multi-select SelectElement. If you need to select a lot of items in a dropdown that
        /// contains a large list, then this method might take a while. In that case, you can Use SelectRandomListItems with IWebElement instead.
        /// </summary>
        /// <param name="elem">The IWebElement that contains the items</param>
        /// <param name="numberOfItemsToSelect"></param>
        public static string selectRandomItemsFromMultiSelectDropDown(SelectElement selectElem, int numberOfItemsToSelect)
        {
            string selectedOptions = null;
            List<string> listOfSelectedOptions = new List<string>();
            Random random = new Random();
            int randomInt = 0;
            List<int> listOfIntsUsed = new List<int>();
            int countOfItemsInList = selectElem.Options.Count;

            for (var i = 0; i < numberOfItemsToSelect; i++)
            {
                // If all the items in the list are already selected, exit above for loop
                if (selectElem.AllSelectedOptions.Count == countOfItemsInList)
                {
                    break;
                }

                for (var j = 0; j < 100; j++)
                {
                    if (listOfIntsUsed.Contains(randomInt))
                    {
                        randomInt = DataUtils.GetRandomIntegerWithinRange(0, selectElem.Options.Count);
                    }
                    if (!listOfIntsUsed.Contains(randomInt))
                    {
                        break;
                    }
                }

                selectElem.SelectByIndex(randomInt);
                listOfIntsUsed.Add(randomInt);
            }

            // Store all the elements selected above into a comma separated string
            IList<IWebElement> selectedElements = selectElem.AllSelectedOptions;

            foreach (var elem in selectedElements)
            {
                listOfSelectedOptions.Add(elem.Text);
            }

            selectedOptions = string.Join(", ", listOfSelectedOptions);
            return selectedOptions;
        }

        /// <summary>
        /// Selects random items from single or multi-select IWebElement dropdowns list that was expanded from a button. The dropdown must be expanded
        /// </summary>
        /// <param name="elem">The button you are selecting the item from. It must be of type IWebElement, not SelectElement</param>
        /// <param name="index">The index of the item you want to choose</param>
        /// <param name="lowRange">Starting integer</param>
        /// <param name="highRange">Highest integer minus 1</param>
        public static void SelectRandomListItems(IWebElement dropDownBtn, int lowRange, int highRange)
        {
            int randomInt = DataUtils.GetRandomIntegerWithinRange(lowRange, highRange);
            SelectListItemByIndex(dropDownBtn, randomInt);
        }

        /// <summary>
        /// Alternative to ClickListItemByIndex. This will select the item regardless of it was selected in the first place or not. ClickListItemByIndex 
        /// can instead deselect items.
        /// </summary>
        /// <param name="elem">The IWebElement button you are selecting the item from</param>
        /// <param name="index">The index of the item you want to choose</param>
        public static void SelectListItemByIndex(IWebElement elem, int index)
        {
            // Find the parent element of the drop down. So we can then find a child DIV element
            var ParentElement = elem.FindElement(By.XPath(".."));
            
            // This DIV element contains the list items, whether filtered or not. If values are filtered, there is code in the line after
            // this to handle that. i.e. class<>hidden
            var DivElem = ParentElement.FindElement(By.CssSelector("ul[role=listbox]"));
            
            // Store all li elements within the Div element that do not have a class that is either equal to "hidden" or "selected hidden"
            var listItems = DivElem.FindElements(By.CssSelector("li:not([class=hidden]):not([class='selected hidden'])"));

            // If the item is already selected, click it twice so it is still selected
            if (listItems[index].GetAttribute("class") != "selected")
            {
                //list[index].Click();
                // Finding the A tag because IE has trouble clicking on LI items. So we are going to click on the A tag inside of the LI item for all browsers
                var aTagOfLIItem = listItems[index].FindElement(By.TagName("a"));
                aTagOfLIItem.Click();
            }
        }

        /// <summary>
        /// Clicks on the dropdown button to expand it, selects an item by it's index, then closes the dropdown. This is an extension to SelectListItemByIndex.
        /// NOTE: If the item is already selected, it will stay selected because the method conditions it so the item will be clicked twice in this case.
        /// </summary>
        /// <param name="elem">The IWebElement button you are selecting the item from</param>
        /// <param name="index">The index of the item you want to choose</param>
        public static void ChooseListItemByIndex(IWebDriver browser, IWebElement elem, int index)
        {
            ElemSet.ClickMultiSelectDropDownButton(browser, elem);

            // Find the parent element of the drop down. So we can then find a child DIV element
            var ParentElement = elem.FindElement(By.XPath(".."));
            
            // This DIV element contains the list items, whether filtered or not. If values are filtered, there is code in the line after
            // this to handle that. i.e. class<>hidden
            var DivElem = ParentElement.FindElement(By.CssSelector("ul[role=listbox]"));
            
            // Store all li elements within the Div element that do not have a class that is either equal to "hidden" or "selected hidden"
            var listItems = DivElem.FindElements(By.CssSelector("li:not([class=hidden]):not([class='selected hidden'])"));

            // If the item is not already selected, then click it
            if (listItems[index].GetAttribute("class") != "selected")
            {
                //list[index].Click();
                // Finding the A tag because IE has trouble clicking on LI items. So we are going to click on the A tag inside of the LI item for all browsers
                var aTagOfLIItem = listItems[index].FindElement(By.TagName("a"));
                aTagOfLIItem.Click();
            }

            if (elem.GetAttribute("aria-expanded") == "true")
            {
                ElemSet.ClickMultiSelectDropDownButton(browser, elem);
            }
        }

        /// <summary>
        /// Clicks on the dropdown button to expand it, selects an item by name, then closes the dropdown. This is an extension to SelectListItemByIndex. 
        /// NOTE: If the item is already selected, it will stay selected because the method conditions it so the item will be clicked twice in this case.
        /// </summary>
        /// <param name="browser">The driver instance</param>
        /// <param name="elem">The button you are selecting the item from. It must be of type IWebElement, not SelectElement</param>
        /// <param name="name">The name of the item you want to click on</param>
        public static void ChooseListItemByName(IWebDriver browser, IWebElement elem, string itemName)
        {
            ElemSet.ClickMultiSelectDropDownButton(browser, elem);

            // Find the parent element of the drop down. So we can then find a child DIV element
            var ParentElement = elem.FindElement(By.XPath(".."));
            
            // This DIV element contains the list items, whether filtered or not. If values are filtered, there is code in the line after
            // this to handle that. i.e. class<>hidden
            var DivElem = ParentElement.FindElement(By.CssSelector("ul[role=listbox]"));
            
            // Store all li elements within the Div element that do not have a class that is either equal to "hidden" or "selected hidden"
            var listItemElems = DivElem.FindElements(By.CssSelector("li:not([class=hidden]):not([class='selected hidden'])")).ToList();

            foreach (var item in listItemElems)
            {
                // if the current list item's text value in the for loop equals the users passed parameter itemName
                if (item.Text == itemName)
                {
                    // If the item is not selected already, then select it
                    if (item.GetAttribute("class") != "selected")
                    {
                        // Finding the A tag because IE has trouble clicking on LI items. So we are going to click on the A tag inside of the LI item for all browsers
                        var aTagOfLIItem = item.FindElement(By.TagName("a"));
                        aTagOfLIItem.Click();
                        break;
                    }
                }
            }

            if (elem.GetAttribute("aria-expanded") == "true")
            {
                ElemSet.ClickMultiSelectDropDownButton(browser, elem);
            }
        }

        /// <summary>
        /// Clicks on the dropdown button to expand it, clicks on the selected item, then closes the dropdown.
        /// </summary>
        /// <param name="elem">The IWebElement button you are selecting the item from</param>
        /// <param name="index">The index of the item you want to choose</param>
        public static void DeselectListItemByIndex(IWebDriver browser, IWebElement elem, int index)
        {
            ElemSet.ClickMultiSelectDropDownButton(browser, elem);

            // Find the parent element of the drop down. So we can then find a child DIV element
            var ParentElement = elem.FindElement(By.XPath(".."));
            
            // This DIV element contains the list items, whether filtered or not. If values are filtered, there is code in the line after
            // this to handle that. i.e. class<>hidden
            var DivElem = ParentElement.FindElement(By.CssSelector("ul[role=listbox]"));
            
            // Store all li elements within the Div element that do not have a class that is either equal to "hidden" or "selected hidden"
            var listItems = DivElem.FindElements(By.CssSelector("li:not([class=hidden]):not([class='selected hidden'])"));

            // If the item is already selected, click it to deselect it
            if (listItems[index].GetAttribute("class") == "selected")
            {
                //list[index].Click();
                // Finding the A tag because IE has trouble clicking on LI items. So we are going to click on the A tag inside of the LI item for all browsers
                var aTagOfLIItem = listItems[index].FindElement(By.TagName("a"));
                aTagOfLIItem.Click();
            }

            if (elem.GetAttribute("aria-expanded") == "true")
            {
                ElemSet.ClickMultiSelectDropDownButton(browser, elem);
            }
        }

        /// <summary>
        /// Clicks on the dropdown button to expand it, clicks on the selected item to deselect it (if it is not already deselected), then closes the dropdown.
        /// </summary>
        /// <param name="elem">The IWebElement button you are selecting the item from</param>
        /// <param name="itemName">The name of the item you want to deselect</param>
        public static void DeselectListItemByName(IWebDriver browser, IWebElement elem, string itemName)
        {
            ElemSet.ClickMultiSelectDropDownButton(browser, elem);

            // Find the parent element of the drop down. So we can then find a child DIV element
            var ParentElement = elem.FindElement(By.XPath(".."));
            
            // This DIV element contains the list items, whether filtered or not. If values are filtered, there is code in the line after
            // this to handle that. i.e. class<>hidden
            var DivElem = ParentElement.FindElement(By.CssSelector("ul[role=listbox]"));
            
            // Store all li elements within the Div element that do not have a class that is either equal to "hidden" or "selected hidden"
            var listItemElems = DivElem.FindElements(By.CssSelector("li:not([class=hidden]):not([class='selected hidden'])"));

            foreach (var item in listItemElems)
            {
                // if the current list item's text value in the for loop equals the users passed parameter itemName
                if (item.Text == itemName)
                {
                    // If the item is already selected, click to deselect it
                    //item.Click();
                    // Finding the A tag because IE has trouble clicking on LI items. So we are going to click on the A tag inside of the LI item for all browsers
                    var aTagOfLIItem = item.FindElement(By.TagName("a"));
                    aTagOfLIItem.Click();
                    break;
                }
            }

            if (elem.GetAttribute("aria-expanded") == "true")
            {
                ElemSet.ClickMultiSelectDropDownButton(browser, elem);
            }
        }

        #endregion DropDownListButtons

        #region TextBox

        /// <summary>
        /// This should be used to enter text into a text box for all text inside bootstrap forms. The javascript is needed to run so that the entered values
        /// do not get lost after the text is written into the text boxes. This issue occurs in IE all the time, and in Firefox less frequently. For background
        /// on the subject, see (http://stackoverflow.com/questions/9505588/selenium-webdriver-is-clearing-out-fields-after-sendkeys-had-previously-populate)
        /// If you want to see this issue, use SendKeys (without this method) inside a bootstrap form, then click Save. Notice
        /// that you receive an AJAX error, and the Web App log (if DEV provides one for your Web App) will show that there were no values in the text boxes when
        /// Save was clicked.
        /// </summary>
        /// <param browser="The driver instance"></param>
        /// <param elem="The element to enter text into"></param>
        /// <param clearText="Whether you want to clear the text before you enter text or not"></param>
        /// <param text="The exact string you want to enter"></param>
        public static void EnterText(IWebDriver browser, IWebElement elem, bool clearText, string text)
        {
            elem.Click();

            if (clearText == true)
            {
                elem.Clear();
            }
            elem.SendKeys(text);

            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)browser;
            jsExecutor.ExecuteScript("$(arguments[0]).change();", elem);
        }

        #endregion TextBox

        #region Grids

        /// <summary>
        /// Clicks any cell inside of a grid by the cell text of that cell
        /// </summary>
        /// <param gridElem="The driver instance"></param>
        /// <param cellText="The exact cell text inside the cell"></param>
        public static void ClickCellByCellText(IWebElement gridElem, string cellText)
        {
            //Get table rows 
            IList<IWebElement> allRows = gridElem.FindElements(By.TagName("tr"));

            foreach (IWebElement row in allRows)
            {
                //Find all cells in the row
                IList<IWebElement> cells = row.FindElements(By.XPath(".//*[local-name(.)='th' or local-name(.)='td']"));
                List<string> cellContent = new List<string>();

                foreach (IWebElement cell in cells)
                {
                    if (cell.GetAttribute("textContent") == cellText)
                    {
                        cell.Click();
                    }
                }
            }
        }

        /// <summary>
        /// Hovers over any cell inside of a grid by the cell text of that cell
        /// </summary>
        /// <param gridElem="The driver instance"></param>
        /// <param cellText="The exact cell text inside the cell"></param>
        public static void HoverOverCellByCellText(IWebDriver browser, IWebElement gridElem, string cellText)
        {
            //Get table rows 
            IList<IWebElement> allRows = gridElem.FindElements(By.TagName("tr"));

            foreach (IWebElement row in allRows)
            {
                //Find all cells in the row
                IList<IWebElement> cells = row.FindElements(By.XPath(".//*[local-name(.)='th' or local-name(.)='td']"));
                List<string> cellContent = new List<string>();

                foreach (IWebElement cell in cells)
                {
                    if (cell.GetAttribute("textContent") == cellText)
                    {
                        Actions action = new Actions(browser);
                        action.MoveToElement(cell).Perform();
                    }
                }
            }
        }

        public static void ClickRowByRowNumber(IWebDriver browser, IWebElement gridElem, int rowNumber)
        {
            IWebElement rowToClick = gridElem.FindElements(By.TagName("tr"))[rowNumber - 1];
            rowToClick.Click();
        }

        public static void HoverOverRowByRowNumber(IWebDriver browser, IWebElement gridElem, int rowNumber)
        {
            IWebElement rowToClick = gridElem.FindElements(By.TagName("tr"))[rowNumber - 1];
            Actions action = new Actions(browser);
            action.MoveToElement(rowToClick).Perform();
        }

        /// <summary>
        /// Returns the row count of any grid
        /// </summary>
        /// <param gridElem="The driver instance"></param>
        public static int GetRowCount(IWebElement gridElem)
        {
            if (gridElem.FindElements(By.TagName("tr")).Count == 0)
            {
                return 0;
            }
            int rowCount = gridElem.FindElements(By.TagName("tr")).Count;
            return rowCount;
        }

        #endregion Grids

        #region Buttons


        #endregion Buttons

        #region General

        public static void DragAndDropToElement(IWebDriver browser, IWebElement sourceElem, IWebElement destinationElem, int x, int y)
        {
            int Width = destinationElem.Size.Width;
            int Height = destinationElem.Size.Height;
            Console.WriteLine(Width);
            Console.WriteLine(Height);
            int MyX = (Width * x) / 100;//spot to drag to is at x of the width
            int MyY = (Height * y) / 100; ;//spot to drag to is at y of the height

            if (x == -1)
            {
                Actions builder = new Actions(browser);
                IAction dragAndDrop = builder.ClickAndHold(sourceElem)
                .MoveToElement(destinationElem)
                    .Release()
                    .Build();
                dragAndDrop.Perform();
            }
            else
            {
                Actions builder = new Actions(browser);
                IAction dragAndDrop = builder.ClickAndHold(sourceElem)
                .MoveToElement(destinationElem, MyX, MyY) // TODO: See the begining of this method for details on MyX and MyY
                   .Release(destinationElem)
                   .Build();
                dragAndDrop.Perform();
            }
        }

        /// <summary>
        /// Scrolls horizontally or vertically to a specified element within a frame that contains a scroll bar
        /// <param name="browser">The driver</param>
        /// <param name="divElem">The div element that contains the scroll bar must be passed here</param>
        /// <param name="elemToScrollTo">The element the tester wants to scroll to</param>
        /// <param name="HorizontalOrVertical">'Horizontal' or 'Vertical'</param>
        /// </summary>
        public static void ScrollToElementWithinFrame(IWebDriver browser, IWebElement divElem, IWebElement elemToScrollTo, string HorizontalOrVertical)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)browser;

            if (HorizontalOrVertical == "Vertical")
            {
                js.ExecuteScript("arguments[0].scrollTop = arguments[1];", divElem, elemToScrollTo.Location.Y);
            }

            if (HorizontalOrVertical == "Horizontal")
            {
                // Scroll inside the popup frame element vertically. See the following...
                // http://stackoverflow.com/questions/22709200/selenium-webdriver-scrolling-inside-a-div-popup
                js.ExecuteScript("arguments[0].scrollLeft = arguments[1];", divElem, elemToScrollTo.Location.X);
            }
        }

        /// <summary>
        /// Scrolls horizontally or vertically to a specified element within a frame that contains a scroll bar
        /// <param name="browser">The driver</param>
        /// <param name="divElem">The div element that contains the scroll bar must be passed here</param>
        /// <param name="xOrYCoordinate">The X or the Y coordinate</param>
        /// <param name="HorizontalOrVertical">'Horizontal' or 'Vertical'</param>
        /// </summary>
        public static void ScrollToWithinFrame(IWebDriver browser, IWebElement divElem, int xOrYCoordinate, string HorizontalOrVertical)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)browser;

            if (HorizontalOrVertical == "Vertical")
            {
                js.ExecuteScript("arguments[0].scrollTop = arguments[1];", divElem, xOrYCoordinate);
            }

            if (HorizontalOrVertical == "Horizontal")
            {
                // Scroll inside the popup frame element vertically. See the following...
                // http://stackoverflow.com/questions/22709200/selenium-webdriver-scrolling-inside-a-div-popup
                js.ExecuteScript("arguments[0].scrollLeft = arguments[1];", divElem, xOrYCoordinate);
            }
        }

        /// <summary>
        /// Scrolls vertically to a specified element within the active window. This only scrolls on the window scroll bar, not any scroll bars embedded
        /// scroll bars
        /// <param name="browser">The driver</param>
        /// <param name="divElem">The element to scroll to</param>
        /// </summary>
        public static void ScrollToElement(IWebDriver browser, IWebElement elem)
        {
            ((IJavaScriptExecutor)browser).ExecuteScript("window.scrollTo(0," + elem.Location.Y + ")");
        }

        #endregion General
    }
}

