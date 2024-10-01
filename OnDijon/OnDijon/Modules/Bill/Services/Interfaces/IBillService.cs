using OnDijon.Modules.Bill.Entities.Responses;
using System.Threading.Tasks;

namespace OnDijon.Modules.Bill.Services.Interfaces
{
    public interface IBillService
    {
        Task<BillListResponse> GetBills(string userEditId);
    }
}
