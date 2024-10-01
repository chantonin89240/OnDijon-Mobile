using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.Rating.Entities.Dto;
using OnDijon.Modules.Rating.Entities.Request;
using OnDijon.Modules.Rating.Entities.Response;
using OnDijon.Modules.Rating.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OnDijon.Modules.Rating.Services
{
    public class RatingService : IRatingService
    {

        private readonly Common.Utils.Services.Interfaces.IHttpService _httpService;
        readonly ISession _session;

        public RatingService(ISession session, Common.Utils.Services.Interfaces.IHttpService httpService)
        {
            _session = session;
            _httpService = httpService;
        }

        public async Task<GetSessionRatingResponse> GetActualRatingSession()
        {
            GetSessionRatingDto session = await GetActualRatingSessionAsync();
            GetSessionRatingResponse response = Common.Entities.Utils.Translate<GetSessionRatingResponse, GetSessionRatingDto>(session);
            if (response.IsSuccessful())
            {
                response.EditId = session.EditId;
                response.BeginDatePublication = session.BeginDatePublication;
                response.EndDatePublication = session.EndDatePublication;
                response.HasSession = session.HasSession;
                response.Incrementation = Int32.Parse(session.Incrementation);
                response.NumberVisitDashboard = Int32.Parse(session.NumberVisitDashboard);
                response.PublicationDate = session.PublicationDate;
            }
            return response;
        }

        public async Task<GetSessionRatingDto> GetActualRatingSessionAsync()
        {
            GetSessionRatingDto dto = new GetSessionRatingDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.RATING_SERVICES, Constants.RATING_GET_ACTUAL_SESSION);
                string editId = null;
                if (_session.Profile != null)
                {
                    editId = _session.Profile.Guid;
                }
                GetSessionRatingRequest sessionR = new GetSessionRatingRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    EditId = editId
                };

                string json = JsonConvert.SerializeObject(sessionR);
                dto = await _httpService.PostAsync<GetSessionRatingDto>(new Uri(url), json).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return dto;
        }


        public async Task<Response> SendRatingSession(string sessionEditId, int note, string comment)
        {
            WsDMDto session = await SendRatingAsync(sessionEditId, note, comment);
            Response response = Common.Entities.Utils.Translate<Response, WsDMDto>(session);
            return response;
        }

        public async Task<WsDMDto> SendRatingAsync(string sessionEditId, int note, string comment)
        {
            WsDMDto dto = new WsDMDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.RATING_SERVICES, Constants.RATING_SEND_RATING);
                string editId = null;
                if (_session.Profile != null)
                {
                    editId = _session.Profile.Guid;
                }
                PostSessionRatingRequest cguR = new PostSessionRatingRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    EditIdUser = editId,
                    EditIdSession = sessionEditId,
                    Comment = comment,
                    Note = note.ToString()
                };

                string json = JsonConvert.SerializeObject(cguR);
                dto = await _httpService.PostAsync<WsDMDto>(new Uri(url), json).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return dto;
        }

    }
}
