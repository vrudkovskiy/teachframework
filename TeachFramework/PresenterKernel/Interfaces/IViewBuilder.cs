using System.Collections.Generic;

namespace TeachFramework.Interfaces
{
    public interface IViewBuilder
    {
        IView Build(IEnumerable<UiDescriptionItem> data);
    }
}