using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OnDijon.Modules.Dashboard.Entities.Dto.Card;
using OnDijon.Modules.Dashboard.Entities.Request;
using OnDijon.Modules.Dashboard.Services.Interfaces;
using OnDijon.Modules.Account.Entities.Request;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils;
using static OnDijon.Common.Entities.Status;
using OnDijon.Modules.Dashboard.Entities.Response;
using OnDijon.Modules.Dashboard.Entities.Dto;
using System;
using Microsoft.AppCenter.Crashes;
using OnDijon.Modules.Dashboard.Entities.Models;
using OnDijon.Common.Utils.Enums;

namespace OnDijon.Modules.Dashboard.Services
{
    public class DashboardService : IDashboardService
    {
        readonly Common.Utils.Services.Interfaces.IHttpService _httpService;

        public DashboardService(Common.Utils.Services.Interfaces.IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task<DtoListResponse<CardDto>> GetAllCards()
        {
            DtoListResponse<CardDto> response = new DtoListResponse<CardDto> { State = CallStatusEnum.InternalServerError };

            var url = string.Concat(Constants.API_URL, Constants.ONDIJON_SERVICES, Constants.ONDIJON_GET_ALL_CARDS);

            AccountRequest data = new AccountRequest()
            {
                Key = Constants.ONDIJON_KEY
            };
            string payload = JsonConvert.SerializeObject(data);

            var result = await _httpService.PostAsync<CardListDto>(new System.Uri(url), payload);
            if (result.StatusCodes != null && result.StatusCodes.Contains(Code.Success))
            {
                response.State = CallStatusEnum.Success;
                response.Data = result.Cards;
            }
            response.Message = result.StatusMessages?.FirstOrDefault()?.Value;
           
            return response;
        }

        public async Task<WorkDataResponse> GetWorkData(string UserId)
        {
            var sources = await GetWorkDataAsync(UserId);
            var response = Common.Entities.Utils.Translate<WorkDataResponse, WorkDataListDto>(sources);
            if (response.IsSuccessful())
            {
                if (sources.WorkDatas != null)
                {
                    response.WorkDataList = sources.WorkDatas.Select(item =>
                    {
                        return new WorkDataModel()
                        {
                            Count = item.Count,
                            State = item.State
                        };
                    }).ToList();
                }
            }
            return response;
        }

        private async Task<WorkDataListDto> GetWorkDataAsync(string UserId)
        {
            WorkDataListDto _WorkDataList = new WorkDataListDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.WORK_SERVICES, Constants.WORK_GET_DATA);
                WorkDataRequest data = new WorkDataRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    UserEditId = UserId
                };
                string json = JsonConvert.SerializeObject(data);
                _WorkDataList = await _httpService.PostAsync<WorkDataListDto>(new Uri(url), json).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _WorkDataList;
        }

    }
}
