using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Common.Utils;
using OnDijon.Modules.UsefulContact.Entities.Dto;
using OnDijon.Modules.UsefulContact.Entities.Models;
using OnDijon.Modules.UsefulContact.Entities.Responses;
using OnDijon.Modules.UsefulContact.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OnDijon.Modules.UsefulContact.Entities.Requests;
using System.Linq;
using OnDijon.Common.Entities;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms.Internals;

namespace OnDijon.Modules.UsefulContact.Services
{
    class ContactDomainService : IContactDomainService
    {

        readonly IHttpService _httpService;

        public ContactDomainService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        // Récupération de la liste des domaines de contact
        public async Task<ContactDomainListResponse> GetDomains()
        {

            var sources = await GetDomainsAsync();
            var response = Utils.Translate<ContactDomainListResponse, DomainListDto>(sources);
            if (response.IsSuccessful())
            {
                if (sources.Elements != null)
                {
                    response.ContactDomainList = sources.Elements.Select(item =>
                    {
                        return new ContactDomainModel()
                        {
                            Id = item.EditId,
                            Name = item.Name
                        };
                    }).ToList();
                }
            }
            return response;
        }

        // Récupération de la liste de contact liés à un domaine 
        public async Task<ContactListResponse> SearchContact(string searchText, string idDomain)
        {
            var sources = await SearchContactAsync(searchText, idDomain);
            var response = Utils.Translate<ContactListResponse, ContactListDto>(sources);
            if (response.IsSuccessful())
            {
                if (sources.Elements != null)
                {
                    response.ContactList = sources.Elements;
                    response.ContactList.ForEach(c => c.Address = !string.IsNullOrEmpty(c.Address) && c.Address.IndexOf('\n')  == 1? c.Address.Substring(2) : c.Address);
                }
                else
                {
                    response.ContactList = new List<ContactDto>();
                }
            }
            return response;
        }

        private async Task<DomainListDto> GetDomainsAsync()
        {
            DomainListDto _allDomains = new DomainListDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.UC_SERVICES, Constants.UC_GET_DOMAINSLIST);
                DemandRequest data = new DemandRequest()
                {
                    Key = Constants.ONDIJON_KEY
                };
                string json = JsonConvert.SerializeObject(data);
                _allDomains = await _httpService.PostAsync<DomainListDto>(new Uri(url), json).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _allDomains;
        }


        private async Task<ContactListDto> SearchContactAsync(string searchText, string idDomain)
        {
            ContactListDto _DomainContacts = new ContactListDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.UC_SERVICES, Constants.UC_GET_SEARCHCONTACTSLIST);
                SearchRequest data = new SearchRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    DomainEditId = idDomain,
                    SearchText = searchText
                };
                string json = JsonConvert.SerializeObject(data);
                _DomainContacts = await _httpService.PostAsync<ContactListDto>(new Uri(url), json).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _DomainContacts;
        }
    }
}
