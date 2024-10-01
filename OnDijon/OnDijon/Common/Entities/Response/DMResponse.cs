using OnDijon.Common.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Common.Entities.Response
{
    public class DMResponse
    {
        public List<string> StatusCode { get; set; }
        public List<StatusMessage> Message { get; set; }

     
    }
}
