using System;
using System.Collections.Generic;

namespace TeachFramework.Exceptions
{
    public class NotComparableDataValueException : Exception
    {
        private const string ProgramCrushedMessage = "Помилка. Програма завершить виконання.";
        private const string NotComparableTypeMessage = "Немає правила, за яким програма повинна порівнювати дані.";
        private const string TypeMessage = "Типи даних, для яких не задані правила порівняння: ";
        public new string Message { get; private set; }

        public NotComparableDataValueException(IEnumerable<object> invalidValues)
            : base("Some types, which need comparison, don't implement interface IComparable")
        {
            Message = ProgramCrushedMessage + Environment.NewLine +
                      NotComparableTypeMessage + Environment.NewLine +
                      TypeMessage;
            foreach (var obj in invalidValues)
                Message += Environment.NewLine + " - " + obj.GetType();
        }
    }
}
