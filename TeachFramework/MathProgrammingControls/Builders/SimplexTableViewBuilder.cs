using TeachFramework.Interfaces;
using TeachFramework.MathProgramming;

namespace TeachFramework.Controls
{
    public class SimplexTableViewBuilder : IDataControlBuilder
    {
        #region IDataControlBuilder Members

        public string Description
        {
            get { return SimplexTableView.ControlDescription; }
        }

        public IDataControl Create(UiDescriptionItem data)
        {
            SimplexTableView tableView;
            if (data.Value == null && data.ControlSettings == null)
                tableView = new SimplexTableView();
            else
                tableView = data.Value != null && data.CheckRequired == false
                           ? new SimplexTableView((SimplexTable)data.Value)
                           : new SimplexTableView((SimplexTableViewSettings)data.ControlSettings);
            tableView.ControlName = data.Name;
            tableView.ReadOnly = !data.Editable;

            return tableView;
        }

        #endregion
    }
}
