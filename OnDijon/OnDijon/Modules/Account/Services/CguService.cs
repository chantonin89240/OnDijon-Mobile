using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using OnDijon.Modules.Account.Entities.Dto;
using OnDijon.Modules.Account.Entities.Models;
using OnDijon.Modules.Account.Entities.Request;
using OnDijon.Modules.Account.Entities.Response;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Utils;
using System;
using System.Threading.Tasks;

namespace OnDijon.Modules.Account.Services
{
    public class CguService : ICguService
    {
        private readonly Common.Utils.Services.Interfaces.IHttpService _httpService;
        readonly ISession _session;

        public CguService(ISession session, Common.Utils.Services.Interfaces.IHttpService httpService)
        {
            _session = session;
            _httpService = httpService;
        }

        public async Task<CguResponse> GetCgu()
        {
            GetCguDto cgu = await GetCguAsync();
            CguResponse response = Common.Entities.Utils.Translate<CguResponse, GetCguDto>(cgu);

            if(response.IsSuccessful())
            {
                response.Cgu = new CguModel()
                {
                    Html = cgu.Content
                };
            }

            return response;
        }

        public async Task<GetCguDto> GetCguAsync()
        {
            GetCguDto _cgu = null;

            try
            {
                string url = string.Concat(Constants.API_URL, Constants.CGU_SERVICES, Constants.CGU_GET);
                string editId = null;

                if (_session.Profile != null)
                {
                    editId = _session.Profile.Guid;
                }

                CguRequest cguR = new CguRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    ProfileEditId = editId
                };

                string json = JsonConvert.SerializeObject(cguR);

                _cgu = await _httpService.PostAsync<GetCguDto>(new Uri(url), json).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return _cgu;
        }
    }
}
