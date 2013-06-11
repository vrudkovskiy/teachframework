using System;
using System.Collections.Generic;

namespace TeachFramework
{
    /// <summary>
    /// Steps container's settings
    /// </summary>
    public class StepVariants : IComparable
    {
        /// <summary>
        /// All variants
        /// </summary>
        public ICollection<string> Variants { get; private set; }

        /// <summary>
        /// Represents StepVariants
        /// </summary>
        public StepVariants() { }

        /// <summary>
        /// Represents StepVariants
        /// </summary>
        public StepVariants(IEnumerable<string> variants)
        {
            Variants = new List<string>();
            foreach (var variant in variants)
                Add(variant);
        }

        /// <summary>
        /// Adds variant
        /// </summary>
        public void Add(string variant)
        {
            if (!Variants.Contains(variant))
                Variants.Add(variant);
        }

        #region IComparable Members

        /// <summary>
        /// Returns '0' if current object contains one of the variants from input object, else return '-1'
        /// </summary>
        public int CompareTo(object obj)
        {
            foreach (var variant in Variants)
                if (((StepVariants)obj).Variants.Contains(variant))
                    return 0;
            return -1;
        }

        #endregion
    }
}
