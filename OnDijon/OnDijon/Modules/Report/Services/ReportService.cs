using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Http;
using Esri.ArcGISRuntime.Tasks.Geocoding;
using OnDijon.Modules.Report.Entities.Dto;
using OnDijon.Modules.Report.Entities.Request;
using OnDijon.Modules.Report.Entities.Response;
using OnDijon.Common.Utils.Enums;
using OnDijon.Modules.Report.Services.Interfaces;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.UI;
using OnDijon.Common.Entities.Dto;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;

namespace OnDijon.Modules.Report.Services
{
    public class ReportService : IReportService
    {
        private const string ReportTypesKey = "report_types";

        private readonly Common.Utils.Services.Interfaces.IHttpService _httpService;
        private readonly ICacheService _cacheService;
        private readonly IUserIdService _userIdService;

        public ReportService(Common.Utils.Services.Interfaces.IHttpService httpService, ICacheService cacheService, IUserIdService userIdService)
        {
            _httpService = httpService;
            _cacheService = cacheService;
            _userIdService = userIdService;
        }

        public async Task<ReportTypesListResponse> GetReportTypes()
        {
            //try get report types from cache
            var reportTypes = await _cacheService.Get<IList<ReportTypeDto>>(ReportTypesKey, CacheType.InMemory);

            //get report types from server if not in cache
            if (reportTypes == null || !reportTypes.Any())
            {
                string reportTypesUrl = string.Concat(Constants.API_URL, Constants.REPORT_SERVICES, Constants.API_REPORT_TYPES);
                string userId = await _userIdService.GetUserId();

                ReportTypeRequest notifRequest = new ReportTypeRequest()
                {
                    Key = Common.Utils.Constants.ONDIJON_KEY,
                    ProfileEditId = userId
                };

                string json = JsonConvert.SerializeObject(notifRequest);

                var reportTypeList = await _httpService.PostAsync<ReportTypeListDto>(new Uri(reportTypesUrl), json).ConfigureAwait(false);

                ReportTypesListResponse responseReportTypeList = Common.Entities.Utils.Translate<ReportTypesListResponse, ReportTypeListDto>(reportTypeList);

                if (responseReportTypeList.IsSuccessful())
                {
                    responseReportTypeList.ReportTypes = reportTypeList.ReportTypesList;
                    //put report types in cache
                    await _cacheService.Put(ReportTypesKey, responseReportTypeList.ReportTypes, CacheType.InMemory);
                    return responseReportTypeList;
                }
                else
                {
                    return new ReportTypesListResponse { State = responseReportTypeList.State, Message = "Impossible de récupérer les types de signalement" };
                }
            }
            else
            {
                return new ReportTypesListResponse { State = CallStatusEnum.Success, ReportTypes = reportTypes };
            }
        }

        public async Task<AddressesListResponse> GetSuggestions(string address, int maxResults =5)
        {
            AddressesListResponse result = new AddressesListResponse();

            try
            {
                var geocoder = await GeocoderFactory.GetAsync();
                IReadOnlyList<SuggestResult> suggestions = await geocoder.SuggestAsync(address, new SuggestParameters { MaxResults = maxResults });
                if (suggestions.Count > 0)
                {
                    result.State = CallStatusEnum.Success;
                    result.Suggestions = new List<string>();
                    foreach (SuggestResult suggestion in suggestions)
                    {
                        result.Suggestions.Add(suggestion.Label);
                    }
                }
            }
            catch (ArcGISWebException e)
            {
                Debug.WriteLine(e.Message);
                result.State = e.ToCallStatus();
                result.Message = "Impossible de récupérer les suggestions";
            }

            return result;
        }

        public async Task<Entities.Response.AddressResponse> GetAddressFromCoordinates(MapPoint coordinates)
        {
            Entities.Response.AddressResponse result = new Entities.Response.AddressResponse();

            try
            {
                var geocoder = await GeocoderFactory.GetAsync();
                var addresses = await geocoder.ReverseGeocodeAsync(coordinates);
                if (addresses.Count > 0)
                {
                    result.State = CallStatusEnum.Success;
                    result.Address = addresses[0].Label;
                }
                else
                {
                    result.State = CallStatusEnum.InvalidInformations;
                }
            }
            catch (ArcGISWebException e)
            {
                Debug.WriteLine(e.Message);
                result.State = e.ToCallStatus();
                result.Message = "Impossible de récupérer l'adresse depuis les coordonnées";
            }

            return result;
        }

        public async Task<CoordinatesResponse> GetCoordinatesFromAddress(string address)
        {
            CoordinatesResponse result = new CoordinatesResponse();

            GeocodeParameters geocodeParams = new GeocodeParameters { OutputSpatialReference = SpatialReference.Create(4326) };

            try
            {
                var geocoder = await GeocoderFactory.GetAsync();
                IReadOnlyList<GeocodeResult> addresses = await geocoder.GeocodeAsync(address, geocodeParams);
                if (addresses.Count > 0)
                {
                    result.State = CallStatusEnum.Success;
                    MapPoint firstLocation = addresses[0].DisplayLocation;
                    result.X = firstLocation.X;
                    result.Y = firstLocation.Y;
                }
                else
                {
                    result.State = CallStatusEnum.InvalidInformations;
                }
            }
            catch (ArcGISWebException e)
            {
                Debug.WriteLine(e.Message);
                result.State = e.ToCallStatus();
                result.Message = "Impossible de récupérer les coordonnées depuis l'adresse";
            }

            return result;
        }

        public async Task<Response> SendReport(ReportRequest request)
        {
            request.Key = Common.Utils.Constants.ONDIJON_KEY;
            request.ProfileEditId = await _userIdService.GetUserId();

            var uri = new Uri(string.Concat(Constants.API_URL, Constants.REPORT_SERVICES, Constants.API_REPORT_CREATE));
            var json = JsonConvert.SerializeObject(request);
            var source = await _httpService.PostAsync<WsDMDto>(uri, json);
            var response = Common.Entities.Utils.Translate<Response, WsDMDto>(source);
            if (!response.IsSuccessful())
            {
                response.Message = "Impossible d'envoyer le signalement";
            }
            return response;
        }

        public async Task<DtoResponse<ReportDto>> GetReport(string reportId)
        {
            var sources = await GetReportAsync(int.Parse(reportId));

            var response = Common.Entities.Utils.Translate<DtoResponse<ReportDto>, ReportGetDto>(sources);

            if (response.IsSuccessful())
            {
                response.Data = sources.Report;
            }

            return response;
        }

        private async Task<ReportGetDto> GetReportAsync(int reportId)
        {
            ReportGetDto reportGetDto = null;

            try
            {
                string url = string.Concat(Constants.API_URL, Constants.REPORT_SERVICES, Constants.API_REPORT_GET);
                string userId = await _userIdService.GetUserId();

                ReportGetRequest request = new ReportGetRequest()
                {
                    Key = Common.Utils.Constants.ONDIJON_KEY,
                    UserId = userId,
                    ReportId = reportId
                };

                string json = JsonConvert.SerializeObject(request);

                reportGetDto = await _httpService.PostAsync<ReportGetDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return reportGetDto;

        }

        public async Task<DtoListResponse<ReportDto>> GetReports(string userId)
        {
            var sources = await GetReportsAsync(userId);

            var response = Common.Entities.Utils.Translate<DtoListResponse<ReportDto>, ReportsListDto>(sources);

            if (response.IsSuccessful())
            {
                response.Data = sources.ReportsList.OrderByDescending(r => r.Date).ToList();
            }

            return response;
        }

        private async Task<ReportsListDto> GetReportsAsync(string userId)
        {
            ReportsListDto _reportsList = null;

            try
            {
                string url = string.Concat(Constants.API_URL, Constants.REPORT_SERVICES, Constants.API_REPORTS_USER);

                ReportsListRequest request = new ReportsListRequest()
                {
                    Key = Common.Utils.Constants.ONDIJON_KEY,
                    UserId = userId
                };

                string json = JsonConvert.SerializeObject(request);

                _reportsList = await _httpService.PostAsync<ReportsListDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return _reportsList;

        }

        public async Task<DtoListResponse<ReportDto>> GetReports(MapPoint coordinates, string userId)
        {
            var sources = await GetReportsAsync(coordinates, userId);

            var response = Common.Entities.Utils.Translate<DtoListResponse<ReportDto>, ReportsListDto>(sources);

            if (response.IsSuccessful())
            {
                response.Data = sources.ReportsList;
            }

            return response;
        }

        private async Task<ReportsListDto> GetReportsAsync(MapPoint coordinates, string userId)
        {
            ReportsListDto _reportsList = null;

            try
            {
                string url = string.Concat(Constants.API_URL, Constants.REPORT_SERVICES, Constants.API_REPORTS_AREA);

                //format coordinates using invariant culture to have a dot as decimal separator
                var lon = coordinates.X.ToString(CultureInfo.InvariantCulture);
                var lat = coordinates.Y.ToString(CultureInfo.InvariantCulture);

                ReportsByCoordRequest request = new ReportsByCoordRequest()
                {
                    Key = Common.Utils.Constants.ONDIJON_KEY,
                    UserId = userId,
                    Longitude = lon,
                    Latitude = lat
                };

                string json = JsonConvert.SerializeObject(request);

                _reportsList = await _httpService.PostAsync<ReportsListDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return _reportsList;

        }

        public async Task<Response> SubscribeToReport(SubscribeRequest request)
        {
            var uri = new Uri(string.Concat(Constants.API_URL, Constants.REPORT_SERVICES, Constants.API_REPORTS_SUBSCRIBE));

            request.Key = Common.Utils.Constants.ONDIJON_KEY;
            request.ProfileEditId = await _userIdService.GetUserId();

            string json = JsonConvert.SerializeObject(request);
            var source = await _httpService.PostAsync<WsDMDto>(uri, json).ConfigureAwait(false);

            var response = Common.Entities.Utils.Translate<Response, WsDMDto>(source);

            if (!response.IsSuccessful())
            {
                response.Message = "Impossible de s'abonner au signalement";
            }

            return response;
        }

        private static class GeocoderFactory
        {
            // The LocatorTask provides geocoding services via a service
            private static LocatorTask _geocoder;

            private static async Task<LocatorTask> CreateAsync()
            {
                // Initialize the LocatorTask with the provided service Uri
                return await LocatorTask.CreateAsync(new Uri(Constants.GEOCODAGE_URL));
            }

            public static async Task<LocatorTask> GetAsync()
            {
                return _geocoder ?? (_geocoder = await CreateAsync());
            }
        }
    }
}
