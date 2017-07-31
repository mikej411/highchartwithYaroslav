using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Browser.Core.Framework
{
    /// <summary>
    /// Extension methods for finding elements within the DOM.  None of these methods wait for elements to be available.  See WaitUtils for that capability.
    /// </summary>
    public static class SearchUtils
    {
        #region Exists

        /// <summary>
        /// Verifies that an element exists (within the given context) that matches all specified criteria
        /// </summary>
        /// <param name="context"></param>
        /// <param name="by"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static bool Exists(this ISearchContext context, By by, params ICriteria<IWebElement>[] criteria)
        {
            return context.FindElementOrDefault(by, criteria) != null;
        }

        #endregion

        #region FindElement(s)

        /// <summary>
        /// Finds a descendent element that matches the specified criteria.  If the element cannot be found, null is returned.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="by">The by.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The found element, or null</returns>
        public static IWebElement FindElementOrDefault(this ISearchContext context, By by, params ICriteria<IWebElement>[] criteria)
        {
            return FindElementsImpl(context, by, false, true, criteria).FirstOrDefault();
        }

        /// <summary>
        /// Finds the FIRST element that matches the search and criteria.  Throws a NotFoundException if NO
        /// elements match the search criteria.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="by">The by.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>        
        public static IWebElement FindElement(this ISearchContext context, By by, params ICriteria<IWebElement>[] criteria)
        {
            return FindElementsImpl(context, by, true, true, criteria).First();
        }       

        /// <summary>
        /// Finds all the elements that match the specified search condition and criteria.
        /// </summary>
        /// <param name="context">The context in which the search should occur.</param>
        /// <param name="by">The condition by which the search should occur.</param>
        /// <param name="criteria">Additional criteria that the elements must satisfy.</param>
        /// <returns>The elements that match search and criteria, or an empty list if none are found.</returns>
        public static ReadOnlyCollection<IWebElement> FindElements(this ISearchContext context, By by, params ICriteria<IWebElement>[] criteria)
        {
            return FindElementsImpl(context, by, false, false, criteria);
        }

        private static ReadOnlyCollection<IWebElement> FindElementsImpl(ISearchContext context, By by, bool throwIfNoneFound, bool findFirstOnly, params ICriteria<IWebElement>[] criteria)
        {
            // Find ALL elements that match the By clause            
            var elementsMatchingBy = context.FindElements(by);
            var elementsMatchingCriteria = new List<IWebElement>();
            foreach (var i in elementsMatchingBy)
            {
                try
                {
                    if (i.MeetsAll(criteria))
                    {
                        elementsMatchingCriteria.Add(i);
                        // When being called from FindElement (singular), there's no point in iterating the rest of the elements once we've found a match
                        // We still use this method because we want the pretty formatting of the exception message if no matches are found
                        if (findFirstOnly)
                            break;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    // This can happen due to a race condition where the element exists when we query the DOM with the By, but before we check the criteria, the element is removed
                    // If this happens, just move on to the next element.  The current element will NOT
                    // be included in the results.  If the caller WANTS this element to be included, they
                    // should call the WaitForElement method instead                    
                }
            }

            if (!elementsMatchingCriteria.Any() && throwIfNoneFound)
                ThrowNotFoundException(by, elementsMatchingBy, criteria);

            return new ReadOnlyCollection<IWebElement>(elementsMatchingCriteria);
        }

        #endregion

        #region ThrowNotFoundException

        /// <summary>
        /// The main purpose of this method is formatting the message of the Exception that gets thrown.
        /// </summary>
        /// <param name="by">The by.</param>
        /// <param name="elementsMatchingBy">The elements to be considered.</param>
        /// <param name="criteria">The criteria that must be matched.</param>
        /// <exception cref="OpenQA.Selenium.NotFoundException"></exception>
        private static void ThrowNotFoundException(By by, IEnumerable<IWebElement> elementsMatchingBy, IEnumerable<ICriteria<IWebElement>> criteria)
        {
            string msg = null;
            if (elementsMatchingBy == null || !elementsMatchingBy.Any())
            {
                msg = string.Format("Failed to find any elements {0}", by);
            }
            else
            {
                var strElements = string.Join(Environment.NewLine, elementsMatchingBy.Select(p => p.GetAttribute("outerHTML")));
                msg = string.Format("Found {0} elements {1}, but none of them met all criteria: {2}.\n\nElements:\n{3}", elementsMatchingBy.Count(), by, criteria.Description(), strElements);
            }

            throw new NotFoundException(msg);
        }

        #endregion
    }    
}
