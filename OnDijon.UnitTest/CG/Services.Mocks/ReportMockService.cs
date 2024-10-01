using Esri.ArcGISRuntime.Geometry;
using OnDijon.CG.Entities.Dto.Reporting;
using OnDijon.CG.Entities.Request.Reporting;
using OnDijon.CG.Entities.Response.Reporting;
using OnDijon.CG.Enums;
using OnDijon.CG.Services.Interfaces;
using OnDijon.Common.Entities.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.CG.Services.Mocks
{
    class ReportMockService : IReportService
    {
        public Task<AddressResponse> GetAddressFromCoordinates(MapPoint coordinates)
        {
            return Task.Run(() =>
            {
                return new AddressResponse { Address = "Test", State = CallStatusEnum.Success };
            });
        }

        public Task<CoordinatesResponse> GetCoordinatesFromAddress(string address)
        {
            return Task.Run(() =>
            {
                return new CoordinatesResponse { X = 12, Y = 34, State = CallStatusEnum.Success };
            });
        }

        public Task<DtoResponse<ReportDto>> GetReport(string reportId)
        {
            return Task.Run(() =>
            {
                return new DtoResponse<ReportDto> { Data = new ReportDto(), State = CallStatusEnum.Success };
            });
        }

        public Task<DtoListResponse<ReportDto>> GetReports(string userId)
        {
            return Task.Run(() =>
            {
                return new DtoListResponse<ReportDto> { Data = new List<ReportDto>(), State = CallStatusEnum.Success };
            });
        }

        public Task<DtoListResponse<ReportDto>> GetReports(MapPoint coordinates, string userId)
        {
            return Task.Run(() =>
            {
                return new DtoListResponse<ReportDto> { Data = new List<ReportDto>(), State = CallStatusEnum.Success };
            });
        }

        public Task<ReportTypesListResponse> GetReportTypes()
        {
            return Task.Run(() =>
            {
                var reportTypes = new List<ReportTypeDto>
                {
                    new ReportTypeDto { Id = 1, Name = "Test 1", Code = "T1" },
                    new ReportTypeDto { Id = 2, Name = "Test 2", Code = "T2" },
                    new ReportTypeDto { Id = 3, Name = "Test 3", Code = "T3" }
                };
                return new ReportTypesListResponse { ReportTypes = reportTypes, State = CallStatusEnum.Success };
            });
        }

        public Task<AddressesListResponse> GetSuggestions(string address)
        {
            return Task.Run(() =>
            {
                var suggestions = new List<string> { "Adresse 1", "Adresse 2", "Adresse 3" };
                return new AddressesListResponse { Suggestions = suggestions, State = CallStatusEnum.Success };
            });
        }

        public Task<Response> SendReport(ReportRequest request)
        {
            return Task.Run(() =>
            {
                return new Response { State = CallStatusEnum.Success };
            });
        }

        public Task<Response> SubscribeToReport(SubscribeRequest request)
        {
            return Task.Run(() =>
            {
                return new Response { State = CallStatusEnum.Success };
            });
        }
    }
}
