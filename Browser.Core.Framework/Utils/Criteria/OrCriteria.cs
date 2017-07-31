using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.Core.Framework
{
    /// <summary>
    /// A composite criteria that is met only if ANY sub-criteria are met
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Browser.Core.Framework.ICriteria{T}" />
    public class OrCriteria<T> : ICriteria<T>
    {
        private IEnumerable<ICriteria<T>> criteria;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrCriteria{T}"/> class.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public OrCriteria(params ICriteria<T>[] criteria)
        {
            this.criteria = criteria;
        }

        /// <summary>
        /// Gets the description that is printed in the error logs if this criteria is not met.
        /// </summary>
        public string Description
        {
            get { return string.Format("({0})", string.Join(" OR ", criteria.Select(p => p.Description))); }
        }

        /// <summary>
        /// Gets the description that is printed in the error logs if this criteria is not met.
        /// This overload of Description can provide additional context given the current input
        /// that is being compared against the criteria.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public string GetDescription(T input)
        {
            return string.Format("({0})", string.Join(" OR ", criteria.Select(p => p.GetDescription(input))));
        }

        /// <summary>
        /// Determines whether the specified input matches this criteria.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public bool IsMet(T input)
        {
            return this.criteria.MeetsAny(input);
        }
    }
}
