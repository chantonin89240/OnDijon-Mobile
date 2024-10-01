namespace OnDijon.Common.Utils.Validations
{
    public interface IValidatable
    {
        bool IsValid { get; set; }

        bool Validate();
    }
}
