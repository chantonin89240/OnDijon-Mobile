using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Modules.Library.Entities.Dto;
using OnDijon.Modules.Library.Entities.Model;
using OnDijon.Modules.Library.Entities.Request;
using OnDijon.Modules.Library.Entities.Response;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnDijon.Modules.Library.Services.Impl
{
    public class DocumentService : IDocumentService
    {
        readonly IHttpService _httpService;

        public DocumentService(IHttpService httpService)
        {
            _httpService = httpService;
        }

     

        public async Task<LibraryDocResponse> GetImageUrl(string recordId)
        {
            var sources = await GetImageUrlAsync(recordId);

            var response = Utils.Translate<LibraryDocResponse, LibraryDocDto>(sources);
            if (response.IsSuccessful())
            {
                response.Picture = sources.Picture;
            }
            return response;
        }


        public async Task<LibraryDocDto> GetImageUrlAsync(string recordId)
        {
            LibraryDocDto _libraryDoc = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.BM_SERVICES, Constants.BM_GET_Image);

                ImageRequest r = new ImageRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    RecordId = recordId
                };
                var json = JsonConvert.SerializeObject(r);
                _libraryDoc = await _httpService.PostAsync<LibraryDocDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return _libraryDoc;
        }

        public async Task<SuggestionResponse> GetSuggestions(string query)
        {
            SuggestionDto sources = await GetSuggestionsAsync(query);

            var response = Utils.Translate<SuggestionResponse, SuggestionDto>(sources);
            if (response.IsSuccessful())
            {
                response.Suggestions = sources.Suggestions;
            }
            return response;
        }

        public async Task<SuggestionDto> GetSuggestionsAsync(string query)
        {
            SuggestionDto _suggestion = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.BM_SERVICES, Constants.BM_GET_SUGGESTIONS);

                SuggestionRequest r = new SuggestionRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    Search = query
                };
                var json = JsonConvert.SerializeObject(r);
                _suggestion = await _httpService.PostAsync<SuggestionDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return _suggestion;
        }

        public async Task<SearchResponse> Search(string query, int pageNumber, string resultSize)
        {
            SearchDto sources = await SearchAsync(query, pageNumber, resultSize);

            SearchResponse response = Utils.Translate<SearchResponse, SearchDto>(sources);
            if (response.IsSuccessful())
            {
                response.Page = sources.Page;
                response.PageMax = sources.PageMax;
                response.Resources = sources.Resources.Select(item => new Resource()
                {
                    Availability = item.Availability,
                    Creator = item.Creator,
                    Description = item.Description,
                    Id = item.Id,
                    Image = item.Image,
                    Isbn = item.Isbn,
                    OnlineLink = item.OnlineLink,
                    PublicationDate = item.PublicationDate,
                    Title = item.Title,
                    Type = item.Type,
                    UId = item.UId,
                }).ToList();
            }
            return response;
        }

        public async Task<SearchDto> SearchAsync(string query, int pageNumber, string resultSize)
        {
            SearchDto _search = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.BM_SERVICES, Constants.BM_GET_SEARCH);

                SearchRequest r = new SearchRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    QueryString = query,
                    PageNumber = pageNumber,
                    ResultSize = resultSize
                };
                var json = JsonConvert.SerializeObject(r);
                _search = await _httpService.PostAsync<SearchDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            
            return _search;
        }

        public async Task<HoldingResponse> GetHoldings(string recordId)
        {
            HoldingsDto sources = await GetHoldingsAsync(recordId);

            HoldingResponse response = Utils.Translate<HoldingResponse, HoldingsDto>(sources);
            if (response.IsSuccessful())
            {
                response.Holdings = sources.Holdings.Select(item => new Holding()
                {
                    Barcode = item.Barcode,
                    RecordId = item.RecordId,
                    BookingTooltip = item.BookingTooltip,
                    Category = item.Category,
                    HoldingId = item.HoldingId,
                    HoldingPlace = item.HoldingPlace,
                    IsAvailable = item.IsAvailable,
                    IsConsultable = item.IsConsultable,
                    IsExpo = item.IsExpo,
                    IsLoanable = item.IsLoanable,
                    IsReservable = item.IsReservable,
                    IsSerial = item.IsSerial,
                    IsTransmissible = item.IsTransmissible,
                    Localisation = item.Localisation,
                    NbResa = item.NbResa,
                    NewItem = item.NewItem,
                    Section = item.Section,
                    SectionCode = item.SectionCode,
                    Site = item.Site,
                    SiteAddress = item.SiteAddress,
                    SiteCode = item.SiteCode,
                    Statut = item.Statut,
                    Type = item.Type,
                    WhenBack = item.WhenBack
                }).ToList();
            }
            return response;
        }

        public async Task<HoldingsDto> GetHoldingsAsync(string recordId)
        {
            HoldingsDto _holdings = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.BM_SERVICES, Constants.BM_GET_HOLDINGS);

                HoldingRequest r = new HoldingRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    RecordId = recordId,
                };
                var json = JsonConvert.SerializeObject(r);
                _holdings = await _httpService.PostAsync<HoldingsDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return _holdings;
        }      
    }
}
