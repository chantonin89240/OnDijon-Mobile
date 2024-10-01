using System.Collections.Generic;
using System.Linq;

namespace OnDijon.Common.Utils.Validations
{
    public class AnyRule : PredicateRule<IEnumerable<string>>
    {
        public AnyRule()
        {
            Predicate = (value) => value != null && value.Any();
        }
    }
}
