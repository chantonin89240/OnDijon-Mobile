using Newtonsoft.Json;
using OnDijon.Common.Utils.Enums;

namespace OnDijon.Common.Entities.Response
{
    public class Response
    {
        public CallStatusEnum State { get; set; }

        public string Message { get; set; }

        public bool IsSuccessful()
        {
            return State == CallStatusEnum.Success;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
