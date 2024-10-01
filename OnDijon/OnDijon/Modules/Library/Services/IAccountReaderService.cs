using OnDijon.Modules.Library.Entities.Response;
using System.Threading.Tasks;

namespace OnDijon.Modules.Library.Services
{
    public interface IAccountReaderService
    {
        Task<AssociateReaderAccountResponse> AssociateReaderAccount(string login, string password);
        Task<DissociateReaderAccountResponse> DissociateReaderAccount(string idBorrower);
        Task<AutoConnectUrlResponse> GetAutoConnectUrl(string uId);
        Task<ReaderAccountResponse> GetAccountByProfil();
        Task<BorrowerInformationResponse> GetBorrowerInformation(string editId);
        Task<BorrowerInformationResponse> UpdateBorrowerPassword(string editId, string newPassword);

    }
}
