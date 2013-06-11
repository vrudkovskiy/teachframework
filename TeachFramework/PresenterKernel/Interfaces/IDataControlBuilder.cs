namespace TeachFramework.Interfaces
{
    public interface IDataControlBuilder
    {
        string Description { get; }
        IDataControl Create(UiDescriptionItem data);
    }
}
