using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.Core.Framework
{
    /// <summary>
    /// Wrapper for interacting with the App.config file
    /// TODO: Should this live elsewhere?  Is there a better way to do this?
    /// Private for now so it forces us to make this decision before using it elsewhere :)
    /// </summary>
    static class AppSettings
    {
        /// <summary>
        /// Gets the value from the app.config or web.config file or returns a default value if none exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T GetEnumOrDefault<T>(string key, T defaultValue)
            where T : struct
        {
            return GetValueOrDefault<T>(key, defaultValue, p => (T)Enum.Parse(typeof(T), p));
        }

        /// <summary>
        /// Gets the value from the app.config or web.config file or returns a default value if none exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="ensureTrailing">(Optional) If specified, ensures that the returned string "EndsWith" this value</param>
        /// <returns></returns>
        public static string GetStringOrDefault(string key, string defaultValue, string ensureTrailing = null)
        {
            var value = GetValueOrDefault<string>(key, defaultValue, p => p);
            if (!string.IsNullOrEmpty(ensureTrailing) && !value.EndsWith(ensureTrailing))
                value += ensureTrailing;

            return value;
        }        

        /// <summary>
        /// Gets the value from the app.config or web.config file or returns a default value if none exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key in the AppSettings section of the config file.</param>
        /// <param name="defaultValue">The default value to return if the config value doesn't exist.</param>
        /// <param name="converter">The converter.</param>
        /// <returns></returns>
        public static T GetValueOrDefault<T>(string key, T defaultValue, Func<string, T> converter = null)
        {
            T value = defaultValue;            
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {                
                value = converter(ConfigurationManager.AppSettings[key]);
            }
            
            return value;
        }
    }
}
