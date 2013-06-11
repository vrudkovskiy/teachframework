using System;

namespace TeachFramework.Exceptions
{
    /// <summary>
    /// Occures, when collection doesn't have requested Data
    /// </summary>
    public class NoSuchDataException : Exception
    {
        private const string RequestString = "У колеції немає Data з ім’ям ";

        /// <summary>
        /// Represents NoSuchDataException
        /// </summary>
        /// <param name="dataName"></param>
        public NoSuchDataException(string dataName)
            : base(RequestString + dataName)
        { }
    }
}
