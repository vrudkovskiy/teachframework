using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Forms;
using TeachFramework.Interfaces;

namespace TeachFramework.Controls
{
    public partial class StepsContainer : UserControl, IDataControl
    {
        public const string ControlDescription = "StepsContainer";
        private const string AnyVariantIsNotChosenMessage = "Не вибраний жоден варіант";

        public StepsContainer()
        {
            InitializeComponent();
        }
        public StepsContainer(string text)
        {
            InitializeComponent();
            SetSteps(text);
        }

        public void SetSteps(string text)
        {
            var steps = text.Split(new[] { '|' });
            _steps.Items.AddRange(steps);
        }

        #region [ IDataControl Members ]

        public string ControlType
        {
            get { return ControlDescription; }
        }

        public string ControlName
        {
            get { return Name; }
            set { Name = value; }
        }

        public object Value
        {
            get { return new StepVariants(new[] { _steps.SelectedItem.ToString() }); }
            set
            {
                if (value == null) return;
                int index;
                var possibleVariants = ((StepVariants)value).Variants;
                if ((index = _steps.Items.IndexOf(possibleVariants.ElementAt(0))) != -1)
                    _steps.SelectedIndex = index;
                else
                    _steps.SelectedItem = "";
            }
        }

        public bool ReadOnly
        {
            //get { return !_steps.Enabled; }
            set { _steps.Enabled = !value; }
        }

        #endregion

        #region [ IValidateable Members ]

        public void ValidateItem()
        {
            if (_steps.SelectedItem == null)
                throw new ValidationException(AnyVariantIsNotChosenMessage);
        }

        #endregion
    }
}
