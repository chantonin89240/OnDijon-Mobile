using OnDijon.Common.Utils.Services.Interfaces;
using System.Threading.Tasks;
using OnDijon.Common.Entities.Dto;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Modules.Account.Services.Interfaces;
using System.Linq;

namespace OnDijon.Common.Utils.Services
{
    public class VersionService : IVersionService
    {
        readonly IHttpService _httpService;
        readonly ISession _session;

        public VersionService(ISession session, IHttpService httpService, IPopupService popupService)
        {
            _session = session;
            _httpService = httpService;
        }

        public async Task<AppVersionStateResponse> GetAppVersionState( string lastVersion)
        {
            var sources = await GetAppVersionStateAsync(lastVersion);

            var response = Entities.Utils.Translate<AppVersionStateResponse, AppVersionStateDto>(sources);
            if (response.IsSuccessful())
            {
                response.Versionning = new Versionning()
                {
                    Code = sources.Versionning.Code,
                    Message = sources.Versionning.Message,
                    Name = sources.Versionning.Name,
                    State = sources.Versionning.State,
                };
                response.Versionning.Notes = sources.Versionning.Notes.Select(i => new AppVersionNote()
                {
                    Description = i.Description,
                    Image = i.Image,
                    Title = i.Title
                });
            }
            return response;
        }

        private async Task<AppVersionStateDto> GetAppVersionStateAsync(string lastVersion)
        {
            AppVersionStateDto _appVersionStateDto = null;

            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ONDIJON_SERVICES, Constants.ONDIJON_GET_APP_VERSION_STATE_WITH_LAST);

                AppVersionStateWithLastRequest r = new AppVersionStateWithLastRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    AppCode = AppInfo.VersionString,
                    LastVersion = lastVersion
                };
                var json = JsonConvert.SerializeObject(r);

                _appVersionStateDto = await _httpService.PostAsync<AppVersionStateDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _appVersionStateDto;
        }

    }

    //dto
    public class AppVersionStateDto : WsDMDto
    {
        [JsonProperty("Versionning")]
        public VersionningDto Versionning { get; set; }
    }

    public class VersionningDto
    {
        [JsonProperty("Code")]
        public string Code { get; set; }
        [JsonProperty("Message")]
        public string Message { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("State")]
        public string State { get; set; }
        [JsonProperty("Notes")]
        public IEnumerable<AppVersionNoteDto> Notes { get; set; }
    }

    public class AppVersionNoteDto
    {
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("Image")]
        public string Image { get; set; }
    }



    //response
    public class AppVersionStateResponse : Response
    {
        public Versionning Versionning { get; set; }
    }

    public class Versionning
    {
        [JsonProperty("Code")]
        public string Code { get; set; }
        [JsonProperty("Message")]
        public string Message { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("State")]
        public string State { get; set; }
        [JsonProperty("Notes")]
        public IEnumerable<AppVersionNote> Notes { get; set; }
    }

    public class AppVersionNote
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }

    //request
    public class AppVersionStateWithLastRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }
        [JsonProperty("AppCode")]
        public string AppCode { get; set; }
        [JsonProperty("LastVersion")]
        public string LastVersion { get; set; }
    }

}
