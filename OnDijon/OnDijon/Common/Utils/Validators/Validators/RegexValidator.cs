using System;
using System.Text.RegularExpressions;

namespace OnDijon.Common.Utils.Validators.Validators
{
    public class RegexValidator : ValidatorBase<string>
    {
        string regexPattern;

        public RegexValidator(string propertyName, Func<string> propertyValueFunc, string regexPattern, string message) 
            : base(propertyName, propertyValueFunc, message)
        {
            this.regexPattern = regexPattern;
        }

        protected override bool Validate(string value)
            => Regex.IsMatch(value ?? "", regexPattern);
    }
}
