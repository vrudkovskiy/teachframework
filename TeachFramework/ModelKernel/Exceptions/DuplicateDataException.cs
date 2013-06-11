using System;

namespace TeachFramework.Exceptions
{
    /// <summary>
    /// Occures, when Datas with same names was added to user interface description
    /// </summary>
    public class DuplicateDataException : Exception
    {
        /// <summary>
        /// Represents DuplicateDataException
        /// </summary>
        /// <param name="duplicatedName"></param>
        public DuplicateDataException(string duplicatedName)
            : base("У програмі дублюються імена контролів - " + duplicatedName)
        { }
    }
}
