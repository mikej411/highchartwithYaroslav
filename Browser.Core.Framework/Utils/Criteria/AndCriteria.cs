using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.Core.Framework
{
    /// <summary>
    /// A composite criteria that is met only if ALL sub-criteria are met
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Browser.Core.Framework.ICriteria{T}" />
    public class AndCriteria<T> : ICriteria<T>
    {
        private IEnumerable<ICriteria<T>> criteria;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndCriteria{T}"/> class.
        /// </summary>
        /// <param name="criteria">The "sub-criteria".  This criteria is considered met only if ALL sub-criteria are met.</param>
        public AndCriteria(params ICriteria<T>[] criteria)
        {
            this.criteria = criteria;
        }

        /// <summary>
        /// Gets the description that is displayed in the test output if this criteria is not met.
        /// </summary>        
        public string Description
        {
            get { return string.Format("({0})", string.Join(" AND ", criteria.Select(p => p.Description))); }
        }

        /// <summary>
        /// Gets the description that is displayed in the test output if this criteria is not met.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public string GetDescription(T input)
        {
            return string.Format("({0})", string.Join(" AND ", criteria.Select(p => p.GetDescription(input))));
        }

        /// <summary>
        /// Determines whether the specified input meets this criteria (by ensuring that it meets ALL sub-criteria).
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public bool IsMet(T input)
        {
            return this.criteria.MeetsAll(input);
        }
    }
}
