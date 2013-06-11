using System;
using System.Collections.Generic;

namespace TeachFramework.Exceptions
{
    public class NotExistingControlException : Exception
    {
        public new string Message { get; private set; }
        public NotExistingControlException(IEnumerable<string> notExistingControls)
        {
            Message = "Програма потребує неіснуючі в системі контроли:" + Environment.NewLine;
            foreach (var control in notExistingControls)
                Message += " - " + control + Environment.NewLine;
        }
    }
}
