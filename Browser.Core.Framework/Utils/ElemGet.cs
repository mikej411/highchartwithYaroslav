using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Browser.Core.Framework
{
    /// <summary>
    /// A utility class retreiving data and attributes from elements
    /// </summary>
    public static class ElemGet
    {
        #region Checkbox

        /// <summary>
        /// Determines whether a list item within a dropdown has been checked or not
        /// </summary>
        /// <param name="elem">The dropdown element</param>
        /// <param name="itemName">The text attribute value of the list item element</param>
        /// <returns>True if checkbox is checked, false if not</returns>
        public static bool CheckMarkDisplayedOnListItem(IWebElement elem, string itemName)
        {
            bool result = false;

            // Find the parent element of the drop down. So we can then find a child DIV element
            var ParentElement = elem.FindElement(By.XPath(".."));
            // This DIV element contains the list items, whether filtered or not. If values are filtered, there is code in the line after
            // this to handle that. i.e. class<>hidden
            var DivElem = ParentElement.FindElement(By.CssSelector("ul[role=listbox]"));
            // Store all li elements within the Div element
            var lists = DivElem.FindElements(By.CssSelector("li:not([class=hidden])"));
            foreach (var list in lists)
            {
                // if the current list item's text value in the for loop equals the users passed parameter itemName
                if (list.Text == itemName)
                {
                    // Find the check mark element. find span class=glyphicon
                    var checkMarkElem = list.FindElement(By.ClassName("glyphicon"));
                    if (checkMarkElem.Displayed)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        #endregion Checkbox

        #region DropDownListButtons

        /// <summary>
        /// Returns the text of an LI element from a dropdown control
        /// </summary>
        /// <param name="elem">The element from which you are selecting from. It must be of type IWebElement, not SelectElement</param>
        /// <param name="index">The index (zero based) of the LI (list item) that you want to grab the text from</param>
        public static string GetListItemTextByIndex(IWebElement elem, int index)
        {
            // Find the parent element of the drop down. So we can then find a child DIV element
            var ParentElement = elem.FindElement(By.XPath(".."));

            // This DIV element contains the list items, whether filtered or not. If values are filtered, there is code in the line after
            // this to handle that. i.e. class<>hidden
            var DivElem = ParentElement.FindElement(By.CssSelector("ul[role=listbox]"));

            // Store all li elements within the Div element that do not have a class that is either equal to "hidden" or "selected hidden"
            var lists = DivElem.FindElements(By.CssSelector("li:not([class=hidden]):not([class='selected hidden'])"));

            return lists[index].Text;
        }

        /// <summary>
        /// Returns Datatable representing single-select SelectElement
        /// </summary>
        /// <param name="elem">The element to grab the list of items from. It must be of type SelectElement</param>
        public static DataTable SingleSelectItemTextToDataTable(SelectElement elem)
        {
            return elem.Options.Select(p => p.Text.Trim()).ToDataTable<string>("Text");
        }

        /// <summary>
        /// Returns a Datatable representing multi-select select element. The control must be expanded for this method to work, as the Text property of the
        /// LI items do not get populated until the drop down is expanded
        /// </summary>
        /// <param name="elem">The element to grab the list of items from. It must be of type IWebElement, not SelectElement</param>
        public static DataTable MultiSelectListItemTextToDataTable(IWebElement elem)
        {
            // Find the parent element of the drop down. So we can then find a child DIV element
            var ParentElement = elem.FindElement(By.XPath(".."));
            // This DIV element contains the list items, whether filtered or not. If values are filtered, there is code in the line after
            // this to handle that. i.e. class<>hidden
            var DivElem = ParentElement.FindElement(By.CssSelector("ul[role=listbox]"));
            // Store all li elements within the Div element that do not have a class that is either equal to "hidden" or "selected hidden"
            var lists = DivElem.FindElements(By.CssSelector("li:not([class=hidden]):not([class='selected hidden'])"));
            return lists.Select(p => p.Text.Trim()).ToDataTable<string>("Text");
        }

        /// <summary>
        /// Returns a List<string> of the contents of a multi-select IWebElement. The control must be expanded for this method to
        /// work, as the Text property of the LI items do not get populated until the drop down is expanded
        /// </summary>
        /// <param name="elem">The element to grab the list of items from. It must be of type IWebElement, not SelectElement</param>
        public static List<string> MultiSelectListItemTextToListString(IWebElement elem)
        {
            // Find the parent element of the drop down. So we can then find a child DIV element
            var ParentElement = elem.FindElement(By.XPath(".."));

            // This DIV element contains the list items, whether filtered or not. If values are filtered, there is code in the line after
            // this to handle that. i.e. class<>hidden
            var DivElem = ParentElement.FindElement(By.CssSelector("ul[role=listbox]"));

            // Store all li elements within the Div element that do not have a class that is either equal to "hidden" or "selected hidden"
            var lists = DivElem.FindElements(By.CssSelector("li:not([class='hidden']):not([class='selected hidden'])"));

            return lists.Select(p => p.Text).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableBodyElem"></param>
        /// <returns></returns>
        public static bool GridContainsRecord(IWebElement tableBodyElem, string expectedText)
        {
            bool result = false;

            // Store all TR (rows) from the table into a variable
            IList<IWebElement> allRows = tableBodyElem.FindElements(By.TagName("tr"));
            foreach (var row in allRows)
            {
                // Get cell under the first column
                IWebElement cell = row.FindElements(By.TagName("td"))[0];

                if(cell.FindElements(By.TagName("a")).Count > 0)
                {
                    IWebElement aElem = cell.FindElement(By.TagName("a"));
                    if (aElem.Text == expectedText)
                    {
                        result = true;
                        break;
                    }
                }




            }

            return result;
        }

        /// <summary>
        /// Returns a Datatable from a SelectElement. Note that this method trims consecutive spaces from the list of strings. So if you are comparing this
        /// list to anything else, make sure you trim the consecutive spaces off of that comparison object as well
        /// </summary>
        /// <param name="elem">The element to grab the list from. Must be an element with a Select tag in the HTML</param>
        public static DataTable SelectElementListItemTextToDataTable(SelectElement elem)
        {
            // Removing consecutive spaces from any item in the dropdown list. We have to do this because Firefox automatically removes consecutive spaces
            // Note that chrome does not.
            //List<String> itemsWithoutConsecutiveSpaces = OtherUtils.RemoveConsecutiveSpacesFromList(elem);
            //return itemsWithoutConsecutiveSpaces.ToDataTable<string>("Text");

            return elem.Options.Select(p => p.GetAttribute("textContent")).ToDataTable<string>("Text");
        }

        /// <summary>
        /// Returns a Datatable from your Select Element. Note that this method trims consecutive spaces from the list of strings. So if you are comparing this
        /// list to anything else, make sure you trim the consecutive spaces off of that comparison object as well
        /// </summary>
        /// <param name="elem">The element to grab the list from. Must be an element with a Select tag in the HTML</param>
        public static List<string> SelectElementListItemTextToListString(SelectElement elem)
        {
            return elem.Options.Select(p => p.GetAttribute("textContent")).ToList();
        }

        /// <summary>
        /// Returns a Datatable from your Select Element, with a user-specified parameter to trim any text in the string
        /// </summary>
        /// <param name="elem">The element to grab the list from. Must be an element with a Select tag in the HTML</param>
        /// <param name="textToRemove">The text you want to remove from the string</param>
        public static DataTable SelectElementListItemTextTrimmedToDataTable(SelectElement elem, string textToRemove)
        {
            return elem.Options.Select(p => p.Text.Replace(textToRemove, string.Empty)).ToDataTable<string>("Text");
        }

        /// <summary>
        /// Returns a list of strings from your Select Element. Until this code is refactored to become faster,  Only use this
        /// on a small list, preferably under 30 items. Otherwise, it will take a long time to complete. An alternative is to use
        /// the SelectElementListItemTextToDataTable method
        /// </summary>
        /// <param name="elem">The element to grab the list from. Must be anelement with a Select tag in the HTML</param>
        public static List<string> SelectElementListItemTextToArray(SelectElement elem)
        {
            // IList<string> orgTypesActual = new List<string>();
            // orgTypesActual = OrgPage.GetDropDownItemsViaIList(OrgPage.CreateUpdateOrgFormOrgTypeSelectItem1);
            List<string> DropDownItems = new List<string>();
            for (var i = 0; i < elem.Options.Count; i++)
            {
                DropDownItems.Add(elem.Options[i].Text);
            }
            return DropDownItems;
        }

        #endregion DropDownListButtons

        #region Grids

        /// <summary>
        /// Returns a datatable from a grid. Alternative to <see cref="BrowserExtensions.GetDataFromIgGrid(IWebDriver, IWebElement, DataColumnDefinition[])"/>
        /// </summary>
        /// <param name="gridColumnsClass">A class must be created that contains the columns names of the grid of type string. For an example, see the 
        /// SamplePage.NameAgeTableColumns. Once this class is crated, then pass the text: "typeof(NameAgeTableColumns)"</param>
        /// <param name="gridBodyElem">The grid body element</param>
        public static DataTable GridToDatatable(Type gridColumnsClass, IWebElement gridBodyElem)
        {
            //Make Generic - pass in property specs, more simple for QA to define than json objects
            var gridProperties = Activator.CreateInstance(gridColumnsClass);

            //Build DataTable with columns relative to passed class
            DataTable gridData = new DataTable();
            var fields = ((TypeInfo)gridColumnsClass).DeclaredFields;
            foreach (FieldInfo item in fields)
            {
                gridData.Columns.Add(item.Name);
            }

            //Get table rows 
            IList<IWebElement> allRows = gridBodyElem.FindElements(By.TagName("tr"));

            foreach (IWebElement row in allRows)
            {
                //Find all cells in the row
                //IList<IWebElement> cells = row.FindElements(By.XPath(".//*[local-name(.)='th' or local-name(.)='td']"));
                IList<IWebElement> cells = row.FindElements(By.TagName("td"));
                List<string> cellContent = new List<string>();

                foreach (IWebElement cell in cells)
                {
                    cellContent.Add(cell.Text);
                }

                //Add row content to data table
                gridData.Rows.Add(cellContent.ToArray());
                cellContent.Clear();
            }

            return gridData;
        }

        #endregion Grids

        #region General

        /// <summary>
        /// This method checks whether an element is currently visible to the eye on the screen. Selenium's Display property did not accomplish this,
        /// so I created this method.
        /// </summary>
        /// <param name="browser">The driver instance</param>
        /// <param name="elem">The element to verify</param>
        public static bool WebElementVisibleOnScreen(IWebDriver browser, IWebElement elem)
        {
            // See: http://darrellgrainger.blogspot.com/2013/05/is-element-on-visible-screen.html
            int x = browser.Manage().Window.Size.Width;
            int y = browser.Manage().Window.Size.Height;
            int x2 = elem.Size.Width + elem.Location.X;
            int y2 = elem.Size.Height + elem.Location.Y;

            return x2 <= x && y2 <= y;
        }

        #endregion General
    }


}