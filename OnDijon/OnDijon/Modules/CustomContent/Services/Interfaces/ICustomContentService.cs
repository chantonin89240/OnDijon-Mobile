using OnDijon.Modules.CustomContent.Entities;
using System.Threading.Tasks;

namespace OnDijon.Modules.CustomContent.Services.Interfaces
{
    public interface ICustomContentService
    {
        Task<CustomContentResponse> GetCustomContent(string editId);
    }
}
