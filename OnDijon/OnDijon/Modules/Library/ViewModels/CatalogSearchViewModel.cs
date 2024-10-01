using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities;
using OnDijon.Modules.Library.Entities.Model;
using OnDijon.Modules.Library.Entities.Response;
using OnDijon.Modules.Library.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using OnDijon.Common.Extensions;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;
using OnDijon.Common.Utils;
using Newtonsoft.Json;
using AsyncAwaitBestPractices.MVVM;

namespace OnDijon.Modules.Library.ViewModels
{
    public class CatalogSearchViewModel : BaseViewModel
    {
        readonly ISession _session;
        readonly IDocumentService _documentService;

        public IList<ReaderAccount> AccountReaderList { get; set; }

        private string _querySearch;
        public string QuerySearch
        {
            get => _querySearch;
            set
            {
                Set(ref _querySearch, value?.ToLower());
                Suggest();
            }
        }

        private IList<string> _suggestion;
        public IList<string> Suggestions { get => _suggestion; private set => Set(ref _suggestion, value); }

        private bool _suggestionsVisible;
        public bool SuggestionsVisible { get => _suggestionsVisible; set => Set(ref _suggestionsVisible, value); }

        private bool _existingMore;
        public bool ExistingMore { get => _existingMore; set => Set(ref _existingMore, value); }

        private int PageIndex { get; set; }
        private int MaxPageIndex { get; set; }

        private bool SendSearchQuery { get; set; }

        private IList<Resource> _resources;
        public IList<Resource> Resources { get => _resources; private set => Set(ref _resources, value); }

        public ICommand SelectResourceCommand { get; }
        public ICommand SuggestionSelectedCommand { get; }

        public ICommand LoadMoreResourcesCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public CatalogSearchViewModel(INavigationService navigationService,
                                      ITranslationService translationService,
                                      IPopupService popupService,
                                      ISession session,
                                      IDocumentService demandService,
                                      ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            _documentService = demandService;

            ExistingMore = false;
            SendSearchQuery = false;
            PageIndex = 0;
            SelectResourceCommand = new AsyncCommand<Resource>(ResourceClick);
            LoadMoreResourcesCommand = new DelegateCommand(LoadMoreResources);
            SearchCommand = new DelegateCommand(SearchClick);
            SuggestionSelectedCommand = new DelegateCommand<string>(query => SuggestionSelected(query));
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            if (parameters.TryGetValue(Constants.ReaderListNavigationParameterKey, out string readerListJson))
            {
                var readerListModel = JsonConvert.DeserializeObject<IList<ReaderAccount>>(readerListJson);
                AccountReaderList = readerListModel;
            }
        }

        public async Task ResourceClick(Resource resource)
        {
            if (!string.IsNullOrEmpty(resource.OnlineLink))
            {
                if (resource.OnlineLink.StartsWith("https"))
                {
	                // Refacto use navigation parameters 
                    //App.Locator.StreamingOnline.Url = resource.OnlineLink;

                    var navParam = new NavigationParameters();
                    navParam.Add(Constants.StreamingOnlineUrlNavigationParameterKey, resource.OnlineLink);
                    await NavigationService.NavigateTo(Locator.StreamingOnlinePage, navigationParameters:navParam);
                }
                else
                {
                    await Browser.OpenAsync(resource.OnlineLink, BrowserLaunchMode.SystemPreferred);
                }
            }
            else
            {
	            // Refacto use navigation parameters
	            var navParams = new NavigationParameters();
	            navParams.Add(Constants.CatalogDetailResourceNavigationParameterKey, resource);
	            navParams.Add(Constants.CatalogDetailAccountReaderListNavigationParameterKey, AccountReaderList);
                await NavigationService.NavigateTo(Locator.CatalogDetailPage,navigationParameters: navParams);
            }
        }

        private void LoadMoreResources()
        {
            ExistingMore = false;
            PageIndex++;
            SearchQuery();
        }


        public override void Cleanup()
        {
            base.Cleanup();
            QuerySearch = null;
            Resources = null;
            Suggestions = null;
            SuggestionsVisible = false;
            SendSearchQuery = false;
        }

        public void Suggest()
        {
            // if (!SearchLaunched)
            if (!string.IsNullOrEmpty(QuerySearch) && !SendSearchQuery)
            {
                GetSuggestions();
                //SearchLaunched = true;
                SuggestionsVisible = true;
            }
        }

        public void GetSuggestions()
        {
            CallApi(async () =>
            {
                SuggestionResponse response = await _documentService.GetSuggestions(QuerySearch);
                ManageApiResponses(response, new DefaultCallbackManager<SuggestionResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (res.Suggestions.Any())
                        {
                            Suggestions = res.Suggestions;
                        }
                    },
                    OnInvalidInformations = delegate { /*Aucune suggestion retournée : ne rien faire*/ },
                });
            });
        }

        public void SuggestionSelected(string query)
        {
            //si nouvelle recherche
            if (!string.IsNullOrEmpty(query) && QuerySearch != query)
            {
                PageIndex = 0;
                Resources = new List<Resource>();
                SendSearchQuery = true;
                QuerySearch = query;

                //SearchLaunched = false;
                //Call Search API
                if (!string.IsNullOrEmpty(QuerySearch))
                {
                    SearchQuery();
                    SendSearchQuery = false;
                }
            }
        }
        public void SearchClick()
        {
            Resources = new List<Resource>();
            SearchQuery();
        }

        public void SearchQuery()
        {
            SuggestionsVisible = false;
            CallApi(async () =>
            {
                SearchResponse response = await _documentService.Search(QuerySearch, PageIndex, "-1");
                ManageApiResponses(response, new DefaultCallbackManager<SearchResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (res.Resources.Any())
                        {
                            //if (Resources == null)
                            //{
                            //    Resources = new List<Resource>();
                            //}
                            //res.Resources.ForEach(Resources.Add);
                            Resources = Resources.Concat(res.Resources).ToList();
                        }
                        MaxPageIndex = res.PageMax;
                        ExistingMore = PageIndex < MaxPageIndex;
                    },
                });
            });
        }
    }
}
