using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Modules.Bill.Entities.Dto;
using OnDijon.Modules.Bill.Entities.Models;
using OnDijon.Modules.Bill.Entities.Requests;
using OnDijon.Modules.Bill.Entities.Responses;
using OnDijon.Modules.Bill.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnDijon.Modules.Bill.Services
{
    public class BillService : IBillService
    {
        readonly IHttpService _httpService;

        public BillService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<BillListResponse> GetBills(string userEditId)
        {
            BillListDto sources = await GetBillsAsync(userEditId);
            BillListResponse response = Utils.Translate<BillListResponse, BillListDto>(sources);

            if (response.IsSuccessful())
            {
                string currentState = string.Empty;
                response.Bills = sources.Bills.OrderBy(x => x.State).Select(bill =>
                {
                    BillModel billModel = new BillModel()
                    {
                        Title = bill.Title,
                        DematBill = bill.DematBill,
                        EditId = bill.EditId,
                        FamilyNumber = bill.FamilyNumber,
                        LevyDate = bill.LevyDate,
                        LevyType = bill.LevyType,
                        Number = bill.Number,
                        PayLink = bill.PayLink,
                        State = bill.State,
                        TipiReference = bill.TipiReference,
                        ToPay = bill.ToPay,
                        DownloadLink = bill.DownloadLink
                    };

                    if (currentState != bill.State)
                    {
                        billModel.IsFirstBill = true;
                    }

                    currentState = bill.State;

                    return billModel;
                }).ToList();
            }
            return response;
        }


        private async Task<BillListDto> GetBillsAsync(string userEditId)
        {
            BillListDto _allBills = new BillListDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.BILL_SERVICES, Constants.BILL_GET_BILLS);

                BillRequest data = new BillRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    UserEditId = userEditId
                };
                string json = JsonConvert.SerializeObject(data);
                _allBills = await _httpService.PostAsync<BillListDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
            }
            return _allBills;
        }
    }
}
