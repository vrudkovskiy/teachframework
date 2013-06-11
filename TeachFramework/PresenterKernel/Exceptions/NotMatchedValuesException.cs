using System;
using System.Collections.Generic;

namespace TeachFramework.Exceptions
{
    public class NotMatchedValuesException : Exception
    {
        public new string Message { get; private set; }

        public NotMatchedValuesException(IEnumerable<string> isNotMatchedValues)
            : base("Деякі типи даних, які необхідно порівняти, не мають процедури порівняння")
        {
            Message = "Програма вимагає порівняння різних типів даних(вказані імена контролів):" + Environment.NewLine;
            foreach (var valueName in isNotMatchedValues)
                Message += @" - " + valueName + Environment.NewLine;
        }
    }
}
