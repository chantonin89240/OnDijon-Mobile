using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Modules.CustomContent.Entities;
using OnDijon.Modules.CustomContent.Entities.Dto;
using OnDijon.Modules.CustomContent.Entities.Models;
using OnDijon.Modules.CustomContent.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OnDijon.Modules.CustomContent.Services
{
    class CustomContentService : ICustomContentService
    {
        readonly IHttpService _httpService;

        public CustomContentService(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task<CustomContentResponse> GetCustomContent(string editId)
        {
            CustomContentDto sources = await GetCustomContentAsync(editId);
            CustomContentResponse response = Utils.Translate<CustomContentResponse, CustomContentDto>(sources);

            if (response.IsSuccessful())
            {
                response.CustomContent = new CustomContentModel()
                {
                    Title = sources.Title,
                    Description = sources.Description,
                    Image = sources.Image,
                    Video = sources.Video,
                    ExternalLinkTitle = sources.ExternalLinkTitle,
                    ExternalLink = sources.ExternalLink,
                };

            }

            return response;
        }

        private async Task<CustomContentDto> GetCustomContentAsync(string editId)
        {
            CustomContentDto _customContent = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ONDIJON_SERVICES, Constants.ONDIJON_GET_CUSTOM_CONTENT);

                CustomContentRequest r = new CustomContentRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    EditId = editId,
                };
                string json = JsonConvert.SerializeObject(r);

                _customContent = await _httpService.PostAsync<CustomContentDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _customContent;
        }
    }
}
