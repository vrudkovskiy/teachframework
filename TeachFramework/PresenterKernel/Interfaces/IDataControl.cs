namespace TeachFramework.Interfaces
{
    public interface IDataControl : IValidateable
    {
        string ControlType { get; }
        string ControlName { get; }
        object Value { get; set; }
    }
}
