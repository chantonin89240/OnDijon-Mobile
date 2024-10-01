namespace OnDijon.Common.Utils.Validations
{
    public class IsNotNullOrEmptyRule : PredicateRule<string>
    {
        public IsNotNullOrEmptyRule()
        {
            Predicate = (value) => !string.IsNullOrWhiteSpace(value);
        }
    }
}
