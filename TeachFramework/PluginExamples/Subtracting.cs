using System.Globalization;
using TeachFramework;

namespace PluginExamples
{
    public class Subtracting : AbstractModel
    {
        private int _a;
        private int _b;
        //-------------------------------------------------------------------------------------

        public Subtracting()
        {
            Description = "Subtracting of 2 numbers";
            Steps.Add(0, Step0);
            Steps.Add(1, Step1);
            Steps.Add(2, Step2);
        }
        private UiDescription Step0(UiDescription userData)
        {
            return new UiDescription { { "SignedTextBox", "a", "Enter first num:", string.Empty, true, false } };
        }
        private UiDescription Step1(UiDescription userData)
        {
            _a = int.Parse((string)userData["a"].Value);
            return new UiDescription
                       {
                           {"SignedTextBox", "a", "First num:", _a.ToString(CultureInfo.InvariantCulture)},
                           {"Label", "subOperator", "-", "-"},
                           {"SignedTextBox", "b", "Enter second num:", string.Empty, true}
                       };
        }
        private UiDescription Step2(UiDescription userData)
        {
            _b = int.Parse((string)userData["b"].Value);
            return new UiDescription
                       {
                           {"SignedTextBox", "a", "First num:", _a.ToString(CultureInfo.InvariantCulture)},
                           {"Label", "subOperator", "-", "-"},
                           {"SignedTextBox", "b", "Second num:", _b.ToString(CultureInfo.InvariantCulture)},
                           {"Label", "equalsOperator", "=", "="},
                           {"SignedTextBox", "result", "Result:", (_a - _b).ToString(CultureInfo.InvariantCulture), true, true}
                       };
        }
    }
}
