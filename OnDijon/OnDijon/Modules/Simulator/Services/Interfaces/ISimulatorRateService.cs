using OnDijon.Modules.Simulator.Entities.Responses;
using System.Threading.Tasks;

namespace OnDijon.Modules.Simulator.Services.Interfaces
{
    public interface ISimulatorRateService
    {
        Task<SimulatorRateResponse> GetSimulatorRate(int childNumber, decimal annualIncome, bool dijon, decimal qF, string cityContextId);
        Task<CityContextResponse> GetAllCityContext(string service);
    }
}