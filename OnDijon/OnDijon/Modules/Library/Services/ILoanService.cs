using OnDijon.Modules.Library.Entities.Dto.Model;
using OnDijon.Modules.Library.Entities.Response;
using System.Threading.Tasks;

namespace OnDijon.Modules.Library.Services
{
    public interface ILoanService
    {
        Task<RenewLoanResponse> RenewLoan(LoanDto loan);
    }
}
