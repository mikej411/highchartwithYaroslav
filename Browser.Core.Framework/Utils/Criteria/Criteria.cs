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
    /// Interface that defines whether certain conditions are met for a given object.
    /// This interface was created in order to create friendlier error logs when Selenium times
    /// out waiting for an object or searching for an object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICriteria<in T>
    {
        /// <summary>
        /// Gets the description that is printed in the error logs if this criteria is not met.
        /// </summary>        
        string Description { get; }
        /// <summary>
        /// Gets the description that is printed in the error logs if this criteria is not met.
        /// This overload of Description can provide additional context given the current input
        /// that is being compared against the criteria.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        string GetDescription(T input);
        /// <summary>
        /// Determines whether the specified input matches this criteria.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        bool IsMet(T input);
    }

    /// <summary>
    /// Static helper methods for combining criteria (AND/OR/NOT)
    /// </summary>
    public static class Criteria
    {
        /// <summary>
        /// Negates the specified criteria
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static ICriteria<T> NOT<T>(ICriteria<T> criteria)
        {
            return new Criteria<T>(p =>
            {
                return !criteria.IsMet(p);
            }, string.Format("NOT({0})", criteria.Description));
        }

        /// <summary>
        /// Creates an ICriteria that returns true when ALL of the specified criteria
        /// are true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static ICriteria<T> AND<T>(params ICriteria<T>[] criteria)
        {
            return new AndCriteria<T>(criteria);
        }

        /// <summary>
        /// Convenience method for AND'ing two criteria together
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static ICriteria<T> AND<T>(this ICriteria<T> self, ICriteria<T> other)
        {
            return new AndCriteria<T>(new[] { self, other });
        }       

        /// <summary>
        /// Creates an ICriteria that returns true when ANY of the specified criteria
        /// are true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public static ICriteria<T> OR<T>(params ICriteria<T>[] criteria)
        {
            return new OrCriteria<T>(criteria);
        }

        /// <summary>
        /// Convenience method for OR'ing two criteria together
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static ICriteria<T> OR<T>(this ICriteria<T> self, ICriteria<T> other)
        {
            return new OrCriteria<T>(new[] { self, other });
        }        
    }

    /// <summary>
    /// Class that uses a lambda expression to represent a criteria for a particular object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Criteria<T> : ICriteria<T>
    {
        private Func<T, bool> _isMet;

        /// <summary>
        /// Gets the description for this critieria.  This will be printed in the output
        /// if a test fails because a criteria was not matched.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the description that is printed in the error logs if this criteria is not met.
        /// This overload of Description can provide additional context given the current input
        /// that is being compared against the criteria.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public string GetDescription(T input)
        {
            return string.Format("{0} ({1})", Description, IsMet(input));
        }

        /// <summary>
        /// Returns a value that indicates whether or not the criteria was met
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsMet(T input)
        {
            return _isMet(input);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Criteria{T}"/> class.
        /// </summary>
        /// <param name="isMet">A lambda expression that will evaluate to true if the criteria is met.</param>
        /// <param name="description">The description that will be printed in the error logs if this criteria is not met.</param>
        /// <exception cref="System.ArgumentNullException">
        /// isMet
        /// or
        /// description
        /// </exception>
        public Criteria(Func<T, bool> isMet, string description)
        {
            if (isMet == null)
                throw new ArgumentNullException("isMet");
            if (description == null)
                throw new ArgumentNullException("description");

            this._isMet = isMet;
            this.Description = description;
        }
    }

    /// <summary>
    /// Extension methods for making it easier to work with collections of criteria.
    /// </summary>
    public static class CriteriaExtensions
    {
        /// <summary>
        /// Gets a value that indicates whether ALL the criteria were met
        /// </summary>
        /// <param name="criteria">The criteria to be evaluated.</param>
        /// <param name="input">The input against which the criteria will be evaluated.</param>
        /// <returns></returns>
        public static bool MeetsAll<T>(this IEnumerable<ICriteria<T>> criteria, T input)
        {
            return criteria.All(p => p.IsMet(input));
        }

        /// <summary>
        /// Gets a value that indicates whether ALL the criteria were met
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self">The input against which the criteria will be evaluated.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public static bool MeetsAll<T>(this T self, params ICriteria<T>[] criteria)
        {
            return criteria.MeetsAll(self);
        }

        /// <summary>
        /// Gets a value that indicates whether ANY of the criteria were met
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria">The criteria to be evaluated.</param>
        /// <param name="input">The input against which the criteria will be evaluated.</param>
        /// <returns></returns>
        public static bool MeetsAny<T>(this IEnumerable<ICriteria<T>> criteria, T input)
        {
            return criteria.Any(p => p.IsMet(input));
        }

        /// <summary>
        /// Gets a value that indicates whether ANY of the criteria were met
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self">The element against which the criteria will be evaluated.</param>
        /// <param name="criteria">The criteria to be evaluated.</param>
        /// <returns></returns>
        public static bool MeetsAny<T>(this T self, params ICriteria<T>[] criteria)
        {
            return criteria.MeetsAny(self);
        }

        /// <summary>
        /// Gets all of the criteria that were not met by the element.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<ICriteria<T>> Failures<T>(this IEnumerable<ICriteria<T>> criteria, T input)
        {
            return criteria.Where(p => !p.IsMet(input));
        }

        /// <summary>
        /// Gets the default description for the specified criteria
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self">The the criteria.</param>
        /// <returns></returns>
        public static string Description<T>(this IEnumerable<ICriteria<T>> self)
        {
            return string.Join(", ", self.Select(p => p.Description));
        }

        /// <summary>
        /// Gets the description for the specified criteria given the specified input.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self">The criteria.</param>
        /// <param name="input">The input against which the criteria are/were/will be evaluated.</param>
        /// <returns></returns>
        public static string Description<T>(this IEnumerable<ICriteria<T>> self, T input)
        {
            return string.Join(", ", self.Select(p => p.GetDescription(input)));
        }
    }
}
