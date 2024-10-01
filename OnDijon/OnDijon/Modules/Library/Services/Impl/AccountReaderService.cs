using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OnDijon.Modules.Library.Entities.Dto;
using OnDijon.Modules.Library.Entities.Model;
using OnDijon.Modules.Library.Entities.Request;
using OnDijon.Modules.Library.Entities.Response;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services.Interfaces;
using Microsoft.AppCenter.Crashes;
using OnDijon.Common.Entities;
using OnDijon.Common.Utils.Tools;
using OnDijon.Modules.Account.Services.Interfaces;

namespace OnDijon.Modules.Library.Services.Impl
{
    public class AccountReaderService : IAccountReaderService
    {
        readonly IHttpService _httpService;
        readonly ISession _session;

        public AccountReaderService(ISession session, IHttpService httpService)
        {
            _session = session;
            _httpService = httpService;
        }

        private async Task<AssociateReaderAccountDto> AssociateReaderAccountAsync(string login, string password)
        {
            AssociateReaderAccountDto _associateReaderAccount = null;

            try
            {
                string url = string.Concat(Constants.API_URL, Constants.BM_SERVICES, Constants.BM_GET_ASSOCIATEREADERACCOUNT);

                AssociateReaderAccountRequest r = new AssociateReaderAccountRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    EditId = _session.Profile.Guid,
                    Login = login,
                    Password = password
                };
                var json = JsonConvert.SerializeObject(r);

                _associateReaderAccount = await _httpService.PostAsync<AssociateReaderAccountDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _associateReaderAccount;
        }

        public async Task<AssociateReaderAccountResponse> AssociateReaderAccount(string login, string password)
        {
            var sources = await AssociateReaderAccountAsync(login, password);

            var response = Utils.Translate<AssociateReaderAccountResponse, AssociateReaderAccountDto>(sources);
            if (response.IsSuccessful())
            {
                response.Success = sources.Success;
            }
            return response;
        }


        private async Task<DissociateReaderAccountDto> DissociateReaderAccountAsync(string idBorrower)
        {
            DissociateReaderAccountDto _dissociateReaderAccount = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.BM_SERVICES, Constants.BM_GET_DISSOCIATEREADERACCOUNT);

                DissociateReaderAccountRequest r = new DissociateReaderAccountRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    IdBorrower = idBorrower
                };
                var json = JsonConvert.SerializeObject(r);

                _dissociateReaderAccount = await _httpService.PostAsync<DissociateReaderAccountDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _dissociateReaderAccount;
        }

        public async Task<DissociateReaderAccountResponse> DissociateReaderAccount(string idBorrower)
        {
          
            var sources = await DissociateReaderAccountAsync(idBorrower);

            var response = Utils.Translate<DissociateReaderAccountResponse, DissociateReaderAccountDto>(sources);
            if (response.IsSuccessful())
            {


                response.Success = sources.Success;
            }
            return response;
        }
         
        public async Task<AutoConnectUrlDto> GetAutoConnectUrlAsync(string uId)
        {
            AutoConnectUrlDto _autoConnectUrl = new AutoConnectUrlDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.BM_SERVICES, Constants.BM_GET_AUTOCONNECTURL);

                AutoConnectUrlRequest r = new AutoConnectUrlRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    Host = Constants.INTERNAL_BM_URL,
                    UidUser = uId
                };
                var json = JsonConvert.SerializeObject(r);

                _autoConnectUrl = await _httpService.PostAsync<AutoConnectUrlDto>(new Uri(url), json).ConfigureAwait(false);
             }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _autoConnectUrl;
        }

        public async Task<AutoConnectUrlResponse> GetAutoConnectUrl(string uId)
        {
            var sources = await GetAutoConnectUrlAsync(uId);

            var response = Utils.Translate<AutoConnectUrlResponse, AutoConnectUrlDto>(sources);
            if (response.IsSuccessful())
            {
                //_autoConnectUrl.re
                    
                   // https://internal-bm.dijon.fr/search.aspx?SC=CATALOGUE&QUERY=AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA&preauth=dc7a42e1c4171b3788c3facd1838f297089946175ce4091fd5bfb21dfdb39988&uniqueId=127350&timestamp=12%3a27%3a40&forcelogon=FromMyDijon&v=1.2.6604
          

                response.Url = sources.Url.Replace("https://internal-bm.dijon.fr/?", " https://internal-bm.dijon.fr/search.aspx?SC=CATALOGUE&QUERY=______________________&");
            }
            return response;
        }

        public async Task<ReaderAccountResponse> GetAccountByProfil()
        {
            var sources = await GetAccounts();

            var response = Utils.Translate<ReaderAccountResponse,IdentityAccountsDto>(sources);
            if (response.IsSuccessful())
            {
                int i = 0;
                response.UserAccount = sources.Accounts.Select(item =>
                {
                    return new ReaderAccount()
                    {
                        IdBorrower = item.IdBorrower,
                        Name = item.Name,
                        Civility = item.Gender,
                        FirstName = item.FirstName,
                        //Uid = item.Uid,
                        BarCode = item.BarCode,
                        TypeAccount = BmCardType.Borrower,
                        Color = CardTool.GetColor(i++, CardTool.ColorsBM),
                        ImageUri = RecipeUIConstants.AvatarNeutralSource,
                        ImageSource = ImageTool.convertSourceImage(RecipeUIConstants.AvatarNeutralSource)
                    };
                }).ToList();

            }
            return response;
        }





        private async Task<IdentityAccountsDto> GetAccounts()
        {
            IdentityAccountsDto result = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.BM_SERVICES, Constants.BM_GET_IDENTITYACCOUNTBYPROFIL);

                ReaderAccountRequest r = new ReaderAccountRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    EditId = _session.Profile.Guid
                };
                var json = JsonConvert.SerializeObject(r);

                result = await _httpService.PostAsync<IdentityAccountsDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return result;
        }


        public async Task<BorrowerInformationResponse> GetBorrowerInformation(string editId)
        {
            var sources = await LoadBorrowerInformation(editId);

            var response = Utils.Translate<BorrowerInformationResponse, BorrowerInformationDto>(sources);
            if (response.IsSuccessful())
            {
                response.ImageUri = RecipeUIConstants.GetAvatar(sources.UserAccount.UserInformation.Gender == "F", int.Parse(sources.UserAccount.UserInformation.Age));
                response.UserInformation = sources.UserAccount.UserInformation;
                response.Loans = sources.UserAccount.Loans;
                response.Reservations = sources.UserAccount.Reservations;
            }
            return response;
        }

        private string GetIconByTypeOfDocument(string typeOfDocument)
        {
            return DataReference.UrlIconTypeOfDocument.ContainsKey(typeOfDocument) && !string.IsNullOrEmpty(DataReference.UrlIconTypeOfDocument[typeOfDocument]) ? DataReference.UrlIconTypeOfDocument[typeOfDocument] : string.Empty;
        }

        private async Task<BorrowerInformationDto> LoadBorrowerInformation(string editId)
        {
            BorrowerInformationDto result = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.BM_SERVICES, Constants.BM_GET_BorrowerInformation);
                AccountBorrowerRequest r = new AccountBorrowerRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    BorrowerEditId = editId
                };
                var json = JsonConvert.SerializeObject(r);

                result = await _httpService.PostAsync<BorrowerInformationDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return result;
        }

        public async Task<BorrowerInformationResponse> UpdateBorrowerPassword(string editId, string newPassword)
        {
            var sources = await UpdateBorrower(editId, newPassword);

            var response = Utils.Translate<BorrowerInformationResponse, BorrowerInformationDto>(sources);
            if (response.IsSuccessful())
            {
                response.ImageUri = RecipeUIConstants.GetAvatar(sources.UserAccount.UserInformation.Gender == "F", int.Parse(sources.UserAccount.UserInformation.Age));
                response.UserInformation = sources.UserAccount.UserInformation;
                response.Loans = sources.UserAccount.Loans;
                response.Reservations = sources.UserAccount.Reservations;
            }
            return response;
        }

        private async Task<BorrowerInformationDto> UpdateBorrower(string editId, string newPassword)
        {
            BorrowerInformationDto result = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.BM_SERVICES, Constants.BM_UPDATE_BorrowerPassword);
                UpdateBorrowerRequest r = new UpdateBorrowerRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    BorrowerEditId = editId,
                    Password = newPassword,
                };
                var json = JsonConvert.SerializeObject(r);

                result = await _httpService.PostAsync<BorrowerInformationDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return result;
        }

        //[OperationContract]
        //[FaultContract(typeof(GetIdentityAccounts))]
        //[WebInvoke(Method = "POST", UriTemplate = "GetIdentityAccountByProfil",
        //  RequestFormat = WebMessageFormat.Json,
        //  ResponseFormat = WebMessageFormat.Json)]
        //GetIdentityAccounts GetIdentityAccountByProfil(AccountByProfilBean data);
    }
}
