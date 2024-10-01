using OnDijon.Modules.Library.Entities.Response;
using System.Threading.Tasks;

namespace OnDijon.Modules.Library.Services
{
    public interface IDocumentService
    {
        Task<LibraryDocResponse> GetImageUrl(string recordId);
        Task<SuggestionResponse> GetSuggestions(string query);
        Task<SearchResponse> Search(string query, int pageNumber, string resultSize);
        Task<HoldingResponse> GetHoldings(string recordId);
        
    }
}
