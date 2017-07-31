using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.Core.Framework
{
    /// <summary>
    /// Defines typical criteria for interacting with IWebElements.  These criteria
    /// can be passed as parameters to some of the "Wait" methods.
    /// </summary>
    public class ElementCriteria : Criteria<IWebElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementCriteria"/> class.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <param name="description">The description.</param>
        /// <exception cref="System.ArgumentNullException">
        /// description
        /// or
        /// func
        /// </exception>
        private ElementCriteria(Func<IWebElement, bool> func, string description)
            : base(func, description)
        {
        }

        /// <summary>
        /// Ensures that the IWebElement has the specified attribute defined.
        /// </summary>
        /// <param name="attributeName">The name of the attribute</param>
        /// <returns></returns>
        public static ElementCriteria HasAttribute(string attributeName)
        {
            return new ElementCriteria(new Func<IWebElement, bool>(p =>
            {
                return p.GetAttribute(attributeName) != null;
            }),
                string.Format("HasAttribute: {0}", attributeName));
        }

        /// <summary>
        /// Ensures that the element's attribute matches the specified value
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="value">The value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> the value is case-insensitive.</param>
        /// <returns></returns>
        public static ElementCriteria AttributeValue(string attributeName, string value, bool ignoreCase = false)
        {
            return AttributeValue(attributeName, new string[] { value }, ignoreCase);
        }

        /// <summary>
        /// Ensures that the element's attribute matches AT LEAST ONE of the specified values
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="values">The values.</param>
        /// <param name="ignoreCase">if set to <c>true</c> the values are case-insensitive.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">values</exception>
        /// <exception cref="ArgumentException">You must specify at least one value;values</exception>
        public static ElementCriteria AttributeValue(string attributeName, IEnumerable<string> values, bool ignoreCase = false)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (!values.Any())
                throw new ArgumentException("You must specify at least one value", "values");

            var msg = string.Format("Attribute: '{0}' IN [{1}]", attributeName, string.Join(", ", values));
            if (values.Count() == 1)
                msg = string.Format("Attribute: '{0}' = '{1}'", attributeName, values.First());

            return new ElementCriteria(new Func<IWebElement, bool>(p =>
                {
                    return values.Any(q => string.Compare(q, p.GetAttribute(attributeName), ignoreCase) == 0);
                }), msg);
        }

        /// <summary>
        /// Ensures that the attribute exists and has some type of value (i.e. is not null or empty)
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <returns></returns>
        public static ElementCriteria AttributeHasValue(string name)
        {
            return new ElementCriteria(p =>
            {
                return p.GetAttribute(name) != null;
            }, string.Format("Attribute has value: {0}", name));
        }

        /// <summary>
        /// Ensures that the element is visible (not hidden)
        /// </summary>
        public static readonly ElementCriteria IsVisible = new ElementCriteria(p =>
        {
            return p.Displayed;
        }, "IsVisible");

        /// <summary>
        /// Ensures that the element is enabled
        /// </summary>
        public static readonly ElementCriteria IsEnabled = new ElementCriteria(p =>
        {
            return p.Enabled;
        }, "IsEnabled");

        /// <summary>
        /// Ensures that the element is selected.  This only applies to input elements
        /// such as checkboxes, options in a select, and radio buttons.
        /// </summary>
        public static readonly ElementCriteria IsSelected = new ElementCriteria(p =>
        {
            return p.Selected;
        }, "IsSelected");

        /// <summary>
        /// Ensures that the select element (dropdown) has at LEAST one item in it's dropdown list.
        /// </summary>
        public static ElementCriteria SelectElementHasItems = new ElementCriteria(p =>
        {
            if (p.TagName.ToLower() != "select")
                return false;

            var tmp = new SelectElement(p);
            return tmp.Options.Any();
        }, "<select> element must have at LEAST one option");

        /// <summary>
        /// Ensures that the text of the element matches the specified value
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> the comparison is case-insensitive.</param>
        /// <returns></returns>
        public static ElementCriteria Text(string value, bool ignoreCase = false)
        {
            return new ElementCriteria(p =>
            {
                return string.Compare(p.Text, value, ignoreCase) == 0;
            }, string.Format("Element.Text='{0}'", value));
        }

        /// <summary>
        /// Ensures that the text of the element matches AT LEAST ONE of the specified values
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">values</exception>
        /// <exception cref="ArgumentException">You must specify at least one value;values</exception>
        public static ElementCriteria Text(IEnumerable<string> values, bool ignoreCase = false)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (!values.Any())
                throw new ArgumentException("You must specify at least one value", "values");

            var msg = string.Format("Element.Text: IN [{0}]", string.Join(", ", values));
            if (values.Count() == 1)
                msg = string.Format("Element.Text: ='{0}'", values.First());

            return new ElementCriteria(p =>
            {
                return values.Any(q => string.Compare(p.Text, q, ignoreCase) == 0);
            }, msg);
        }

        /// <summary>
        /// Ensures that the element has ANY text (i.e. is not null or empty)
        /// </summary>
        public static ElementCriteria HasText = new ElementCriteria(p =>
        {
            return !string.IsNullOrEmpty(p.Text);
        }, "Element.Text != nullOrEmpty");

        /// <summary>
        /// Gets a criteria that indicates whether or not an element exists within the given scope
        /// </summary>
        /// <param name="context"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public static ElementCriteria Exists(ISearchContext context, By by)
        {
            return new ElementCriteria(p =>
            {
                return context.FindElementOrDefault(by) != null;
            }, string.Format("Exists: {0}", by));
        }
    }
}
