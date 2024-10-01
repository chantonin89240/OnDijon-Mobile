namespace OnDijon.Common.Utils.Validators.Interface
{
    public interface IValidator
    {
        string PropertyName { get; }
        string Message { get; }
        bool Validate();
    }
}
