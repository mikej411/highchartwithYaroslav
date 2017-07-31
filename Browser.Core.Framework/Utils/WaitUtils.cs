using Browser.Core.Framework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.Core.Framework
{      
	/// <summary>
	/// Extension methods that allow waiting for particular critieria to become true.
	/// </summary>
 
	public static class WaitUtils
	{
		/// <summary>
		/// The default timeout used for all methods is 10 seconds (unless otherwise specified).
		/// </summary>
		public static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);        

		#region WaitUntil

		/// <summary>
		/// Waits the until the specified criteria is true. If the default timeout occurs, an exception will be thrown.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="input">The input.</param>
		/// <param name="criteria">The criteria to be met.</param>
		public static void WaitUntil<T>(this T input, ICriteria<T> criteria)
		{
			WaitUntilImpl(CriteriaExtensions.MeetsAll, input, Timeout, criteria);
		}

		/// <summary>
		/// Waits the until the specified criteria is true. If the timeout occurs, an exception will be thrown.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="input">The input.</param>
		/// <param name="timeout">The timeout.</param>
		/// <param name="criteria">The criteria to be met.</param>
		public static void WaitUntil<T>(this T input, TimeSpan timeout, ICriteria<T> criteria)
		{
			WaitUntilImpl(CriteriaExtensions.MeetsAll, input, timeout, criteria);
		}

		/// <summary>
		/// Waits the until ALL of the specified criteria are true. If the default timeout occurs, an exception will be thrown.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="input">The input.</param>
		/// <param name="criteria">The criteria to be met.</param>
		public static void WaitUntilAll<T>(this T input, params ICriteria<T>[] criteria)
		{
			WaitUntilImpl(CriteriaExtensions.MeetsAll, input, Timeout, criteria);
		}

		/// <summary>
		/// Waits the until ALL of the specified criteria are true. If the timeout occurs, an exception will be thrown.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="input">The input.</param>
		/// <param name="timeout">The timeout.</param>
		/// <param name="criteria">The criteria to be met.</param>
		public static void WaitUntilAll<T>(this T input, TimeSpan timeout, params ICriteria<T>[] criteria)
		{
			WaitUntilImpl(CriteriaExtensions.MeetsAll, input, timeout, criteria);
		}

		/// <summary>
		/// Waits the until ANY of the specified criteria are true. If the default timeout occurs, an exception will be thrown.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="input">The input.</param>
		/// <param name="criteria">The criteria to be met.</param>
		public static void WaitUntilAny<T>(this T input, params ICriteria<T>[] criteria)
		{
			WaitUntilImpl(CriteriaExtensions.MeetsAny, input, Timeout, criteria);
		}

		/// <summary>
		/// Waits the until ANY of the specified criteria are true. If the timeout occurs, an exception will be thrown.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="input">The input.</param>
		/// <param name="timeout">The timeout.</param>
		/// <param name="criteria">The criteria to be met.</param>
		public static void WaitUntilAny<T>(this T input, TimeSpan timeout, params ICriteria<T>[] criteria)
		{
			WaitUntilImpl(CriteriaExtensions.MeetsAny, input, timeout, criteria);
		}

		private static void WaitUntilImpl<T>(Func<IEnumerable<ICriteria<T>>, T, bool> impl, T input, TimeSpan timeout, params ICriteria<T>[] criteria)
		{
			// Note: We ignore the input here, we're just using the timing functionality
			// of the wait
			var wait = new DefaultWait<object>(new object());
			wait.Timeout = timeout;
			try
			{
				wait.Until(p =>
				{
					return impl(criteria, input);
				});
			}
			catch (WebDriverTimeoutException ex)
			{
				var failures = criteria.Failures(input);
				throw new WebDriverTimeoutException(string.Format("Timed out after {0} seconds, waiting for {1} criteria.  The following criteria were not met: {2}", wait.Timeout.TotalSeconds, criteria.Count(), failures.Description(input)), ex);
			}
		}
        public static void WaitElementNotVisible(IWebDriver browser, By Bys)
        {
            WebDriverWait wait = new WebDriverWait(browser, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(Bys));
        }

        #endregion

        #region WaitForElement

        /// <summary>
        /// Waits until a descendent element is available that matches the specified criteria.  If the default timeout expires before a matching element is found, an exception is thrown.
        /// </summary>
        /// <param name="context">The context from which the search should be executed.</param>
        /// <param name="by">The by.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public static IWebElement WaitForElement(this ISearchContext context, By by, params ICriteria<IWebElement>[] criteria)
		{
			return WaitForElementImpl(context, by, Timeout, criteria);
		}

		/// <summary>
		/// Waits until a descendent element is available that matches the specified criteria.  If the timeout expires before a matching element is found, an exception is thrown.
		/// </summary>
		/// <param name="context">The context from which the search should be executed.</param>
		/// <param name="by">The by.</param>
		/// <param name="timeout">The timeout.</param>
		/// <param name="criteria">The criteria.</param>
		/// <returns></returns>
		public static IWebElement WaitForElement(this ISearchContext context, By by, TimeSpan timeout, params ICriteria<IWebElement>[] criteria)
		{
			return WaitForElementImpl(context, by, timeout, criteria);
		}        

		#endregion

		#region WaitForSelectElement

		/// <summary>
		/// Waits until a descendent element is available that matches the specified criteria.  If the default timeout expires before a matching element is found, an exception is thrown.
		/// </summary>
		/// <param name="context">The context from which the search should be executed.</param>
		/// <param name="by">The by.</param>
		/// <param name="criteria">The criteria.</param>
		/// <returns></returns>
		public static SelectElement WaitForSelectElement(this ISearchContext context, By by, params ICriteria<IWebElement>[] criteria)
		{
			return WaitForSelectElementImpl(context, by, Timeout, criteria);
		}

		/// <summary>
		/// Waits until a descendent element is available that matches the specified criteria.  If the timeout expires before a matching element is found, an exception is thrown.
		/// </summary>
		/// <param name="context">The context from which the search should be executed.</param>
		/// <param name="by">The by.</param>
		/// <param name="timeout">The timeout.</param>
		/// <param name="criteria">The criteria.</param>
		/// <returns></returns>
		public static SelectElement WaitForSelectElement(this ISearchContext context, By by, TimeSpan timeout, params ICriteria<IWebElement>[] criteria)
		{
			return WaitForSelectElementImpl(context, by, timeout, criteria);
		}        

		#endregion

		#region Private Helpers

		private static SelectElement WaitForSelectElementImpl(ISearchContext context, By by,
			TimeSpan timeout, params ICriteria<IWebElement>[] criteria)
		{
			var element = WaitForElementImpl(context, by, timeout, criteria);
			return new SelectElement(element);
		}

		private static IWebElement WaitForElementImpl(ISearchContext context, By by,
			TimeSpan timeout, IEnumerable<ICriteria<IWebElement>> criteria)
		{
			if (context == null)
				throw new ArgumentNullException("context");
			if (by == null)
				throw new ArgumentNullException("by");

			var wait = new SearchContextWait(context, timeout);            

			IWebElement element = null;           
			IEnumerable<ElementCriteria> failures = Enumerable.Empty<ElementCriteria>();
			string lastMessage = null;
			try
			{
				wait.Until(x =>
				{
					// Find ALL elements that match the By clause
					try
					{
						element = context.FindElement(by, criteria.ToArray());
						return element != null;
					}
					catch(NotFoundException ex)
					{
						lastMessage = ex.Message;
					}
					return false;
				});
			}
			catch (WebDriverTimeoutException)
			{
				// The FindElement method takes care of formatting a nice failure message, so we re-use that message
				throw new NotFoundException(lastMessage);
			}

			return element;
		}            

		#endregion
	}

    /// <summary>
    /// Defines a Wait object that will wait on some criteria of an ISearchContext.
    /// This is preferable to the WebDriverWait, because there are many scenarios where
    /// we want to wait until we find a certain object within an ISearchContext, but we
    /// don't readily have access to the IWebDriver
    /// </summary>
    public class SearchContextWait : DefaultWait<ISearchContext>
    {
        /// <summary>
        /// Gets or sets the default polling interval to use when waiting for criteria.  The default is 500ms, which is the same
        /// as the WebDriverWait class.
        /// </summary>
        public static TimeSpan DefaultPollingInterval = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchContextWait"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="timeout">The timeout.</param>
        public SearchContextWait(ISearchContext context, TimeSpan timeout)
            : base(context, new SystemClock())
        {
            this.Timeout = timeout;
            this.PollingInterval = DefaultPollingInterval;
            // It's important to ignore the NotFoundException or calls to FindElement
            // will immediately halt the test when they throw the NotFoundException
            this.IgnoreExceptionTypes(typeof(NotFoundException));
        }
    }




}
