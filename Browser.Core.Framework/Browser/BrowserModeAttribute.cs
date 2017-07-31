using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Browser.Core.Framework
{
    /// <summary>
    /// Specifies the mode in which the browser should be initialized
    /// </summary>
    public enum BrowserMode
    {        
        /// <summary>
        /// Re-use an existing browser session.  This saves time by not tearing down and re-creating the browser between each test.
        /// </summary>
        Reuse,
        /// <summary>
        /// Create a new browser session
        /// </summary>
        New
    }

    /// <summary>
    /// Specifies the mode in which the browser should be initialized
    /// <![CDATA[http://www.nunit.org/index.php?p=property&r=2.4.8]]>
    /// </summary>    
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple=false)]
    public class BrowserModeAttribute : PropertyAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserModeAttribute"/> class.
        /// Note that the value passed to the PropertyAttribute base class must be a
        /// 1. string
        /// 2. int
        /// 3. double
        /// The base class can only support these PRIMITIVE types
        /// </summary>
        /// <param name="mode">The mode.</param>
        public BrowserModeAttribute(BrowserMode mode = BrowserMode.Reuse)
            :base(SeleniumCoreSettings.BrowserModeKey, mode.ToString())
        {            
        }        
    }    
}
