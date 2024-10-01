using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using OnDijon.Common.Entities;
using OnDijon.Modules.Services.Entities.Dto;
using OnDijon.Modules.Services.Entities.Models;
using OnDijon.Modules.Services.Entities.Request;
using OnDijon.Modules.Services.Entities.Response;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities.Model;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils;
using OnDijon.Modules.Services.Helpers;
using OnDijon.Modules.Services.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;

namespace OnDijon.Modules.Services.ViewModels
{
    public class ServicesViewModel : ServiceBaseViewModel
    {
        private readonly ISession _session;
        private readonly IServicesService _servicesService;

        private IList<ServiceLayout> _services;
        public IList<ServiceLayout> Services
        {
            get => _services;
            set => Set(ref _services, value);
        }


        private List<CheckboxModel> _favoriteScopes;
        public List<CheckboxModel> FavouriteScopes
        {
            get { return _favoriteScopes; }
            set { Set(ref _favoriteScopes, value); }
        }

        public ICommand TapOnServiceCommand { get; }
        public ICommand ChooseFavouriteCommand { get; }
        public ICommand ChooseScopeFavouriteCommand { get; }

        public string ChooseFavouriteLabelButton => IsChooseFavourite ? "Terminer" : "Choisir mes services favoris";

        public bool IsChooseFavouriteButtonVisibility => _session.IsConnected();

        private bool _isChooseFavourite;
        public bool IsChooseFavourite
        {
            get => _isChooseFavourite;
            set => Set(ref _isChooseFavourite, value);
        }
        

        public ServicesViewModel(INavigationService navigationService,
                                 ITranslationService translationService,
                                 IPopupService popupService,
                                 ISession session,
                                 IServicesService servicesService, 
                                 ILoggerService loggerService) : base(navigationService, translationService, popupService, session, loggerService)
        {
            _session = session;
            _servicesService = servicesService;

            TapOnServiceCommand = new DelegateCommand<ServiceLayout>(service => OnTapOnService(service));

            ChooseFavouriteCommand = new DelegateCommand(() => ChooseFavourite());
            ChooseScopeFavouriteCommand = new DelegateCommand(() => ChooseScopeFavourite());
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            GetServices();
        }

        public void GetServices()
        {
            CallApi(async () =>
            {
                if (_session.IsConnected())
                {
                    FavoriteServiceListResponse response = await _servicesService.GetFavouriteServices(_session.Profile.Guid);
                    ManageApiResponses(response, new DefaultCallbackManager<FavoriteServiceListResponse>(PopupService)
                    {
                        OnSuccess = (res) =>
                        {
                            OnSuccessGetServices(response.Services.ToList());
                            FavouriteScopes = response.Scopes;
                        }
                    });
                }
                else
                {
                    DtoListResponse<ServiceDto> response = await _servicesService.GetAllServices();
                    ManageApiResponses(response, new DefaultCallbackManager<DtoListResponse<ServiceDto>>(PopupService)
                    {
                        OnSuccess = (res) =>
                        {
                            OnSuccessGetServices(response.Data.ToList());
                        }
                    });
                }
            });
        }

        private void OnSuccessGetServices(List<ServiceDto> services)
        {
            Services = ServicesViewModelHelper.TranslateToLayoutService(services.Where(s => s.Visibility != Constants.SERVICE_VISIBLITY_HIDDEN).ToList());
        }

        private void OnTapOnService(ServiceLayout serviceTapped)
        {
            if (IsChooseFavourite)
            {
                serviceTapped.IsFavourite = !serviceTapped.IsFavourite;
            }
            else
            {
                NavigateToService(serviceTapped);
            }
        }

        private void ChooseFavourite()
        {
            IsChooseFavourite = !IsChooseFavourite;
            RaisePropertyChanged(nameof(ChooseFavouriteLabelButton));
            // Save favourite
            if (!IsChooseFavourite)
            {
                SaveFavouriteServices();
            }
        }

        private void ChooseScopeFavourite()
        {
            SaveFavouriteServices();
            PopupService.Show(PopupEnum.PopupSuccess, "Vos préférences ont été enregistrées", "OK");
        }

        private void SaveFavouriteServices()
        {
            var request = new Entities.Request.UpdateFavouriteServiceRequest
            {
                NewFavouriteServices = Services.Where(s => s.IsFavourite),
                NewScopeFavorites = FavouriteScopes.Select(fs => new ScopeRequest() { Title = fs.Title, Favorite = fs.Checked }),
                UserEditId = _session.Profile.Guid.ToString()
            };

            CallApi(async () =>
            {
                var response = await _servicesService.UpdateFavourite(request);
                ManageApiResponses(response, new CallbackManager<Response>
                {
                    OnSuccess = OnSuccesSaveFavouriteServices,
                    OnError = OnErrorSaveFavouriteServices
                });
            });
        }

        private void OnSuccesSaveFavouriteServices(Response obj)
        {
            // nothing, or display toast or popup
            Console.WriteLine("Save favorite services successfully");
            NavigateTo(Locator.DashboardView);
        }

        private void OnErrorSaveFavouriteServices(Response obj)
        {
            Trace(obj, "Impossible de mettre à jour vos favoris, veuillez réessayer ultérieurement", null, true, "Oups !");
            Console.WriteLine("Save favorite services error");
            NavigateTo(Locator.DashboardView);
        }

        public override void Cleanup()
        {
            base.Cleanup();

            IsChooseFavourite = false;
            RaisePropertyChanged(nameof(ChooseFavouriteLabelButton));
        }
    }
}
