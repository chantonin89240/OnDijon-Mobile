using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OnDijon.Modules.Library.Entities.Dto;
using OnDijon.Modules.Library.Entities.Response;
using OnDijon.Modules.Library.Entities.Request;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services.Interfaces;
using Microsoft.AppCenter.Crashes;
using OnDijon.Common.Entities;
using OnDijon.Modules.Library.Entities.Dto.Model;
using OnDijon.Modules.Account.Services.Interfaces;

namespace OnDijon.Modules.Library.Services.Impl
{
    public class LoanService : ILoanService
    {
        readonly IHttpService _httpService;
        readonly ISession _session;

        public LoanService(IHttpService httpService, ISession session)
        {
            _httpService = httpService;
            _session = session;
        }


        private async Task<RenewLoanInformationDto> RenewLoanAsync(LoanDto loan)
        {
            RenewLoanInformationDto _renewLoan = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.BM_SERVICES, Constants.BM_GET_RENEWLOAN);

                RenewLoanRequest r = new RenewLoanRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    EditId = _session.Profile.Guid,
                    IdBorrower = loan.IdBorrower,
                    IdLoan = loan.Id,
                    IdRecord = loan.RecordId
                };
                var json = JsonConvert.SerializeObject(r);
                _renewLoan = await _httpService.PostAsync<RenewLoanInformationDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return _renewLoan;
        }

        public async Task<RenewLoanResponse> RenewLoan(LoanDto loan)
        {
            var sources = await RenewLoanAsync(loan);

            var response = Utils.Translate<RenewLoanResponse, RenewLoanInformationDto>(sources);
            if (response.IsSuccessful())
            {
                response.RenewLoan = sources.RenewLoan;
                //response.RenewLoan = new RenewLoan()
                //{
                //    IdBorrower = sources.RenewLoan.IdBorrower,
                //    FirstName = sources.RenewLoan.FirstName,
                //    IsRenewed = sources.RenewLoan.IsRenewed,
                //    LoanId = sources.RenewLoan.LoanId,
                //    NotRenewedReason = sources.RenewLoan.NotRenewedReason,
                //    Name = sources.RenewLoan.Name
                //};
            }
            return response;
        }

   
    }
}
