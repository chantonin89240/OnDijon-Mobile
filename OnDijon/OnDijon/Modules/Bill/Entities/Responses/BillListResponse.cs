using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Bill.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.Bill.Entities.Responses
{
    public class BillListResponse : Response
    {
        public List<BillModel> Bills { get; set; }
    }
}
