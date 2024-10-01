using Esri.ArcGISRuntime.Geometry;
using OnDijon.Modules.Report.Entities.Dto;
using OnDijon.Modules.Report.Entities.Request;
using OnDijon.Modules.Report.Entities.Response;
using OnDijon.Common.Entities.Response;
using System.Threading.Tasks;

namespace OnDijon.Modules.Report.Services.Interfaces
{
    public interface IReportService
    {
        /// <summary>
        /// Get all report types available
        /// </summary>
        Task<ReportTypesListResponse> GetReportTypes();

        /// <summary>
        /// Get address suggestions for the given address (autocomplete)
        /// </summary>
        Task<AddressesListResponse> GetSuggestions(string address,int maxResults);

        /// <summary>
        /// Get coordinates for the given address (geocoding)
        /// </summary>
        Task<CoordinatesResponse> GetCoordinatesFromAddress(string address);

        /// <summary>
        /// Get address for the given coordinates (reverse geocoding)
        /// </summary>
        Task<Entities.Response.AddressResponse> GetAddressFromCoordinates(MapPoint coordinates);

        /// <summary>
        /// Send a request to create a report
        /// </summary>
        Task<Response> SendReport(ReportRequest request);

        /// <summary>
        /// Get the report with the given id
        /// </summary>
        Task<DtoResponse<ReportDto>> GetReport(string reportId);

        /// <summary>
        /// Get all reports created or followed by the user
        /// </summary>
        Task<DtoListResponse<ReportDto>> GetReports(string userId);

        /// <summary>
        /// Get all reports near the given coordinates
        /// </summary>
        Task<DtoListResponse<ReportDto>> GetReports(MapPoint coordinates, string userId);

        /// <summary>
        /// Subscribe to a report
        /// </summary>
        Task<Response> SubscribeToReport(SubscribeRequest request);
    }
}
