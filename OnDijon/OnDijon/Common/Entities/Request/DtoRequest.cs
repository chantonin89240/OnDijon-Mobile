using Newtonsoft.Json;

namespace OnDijon.Common.Entities.Request
{
    public abstract class DtoRequest
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
