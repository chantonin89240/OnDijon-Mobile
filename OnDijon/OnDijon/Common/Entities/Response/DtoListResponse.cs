using System.Collections.Generic;

namespace OnDijon.Common.Entities.Response
{
    public class DtoListResponse<T> : Response where T : Dto.Dto
    {
        public IList<T> Data { get; set; }
    }
}
