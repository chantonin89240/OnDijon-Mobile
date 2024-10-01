using OnDijon.Common.Entities;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.Demands.Entities.Models;
using OnDijon.Modules.Demands.Entities.Responses;
using OnDijon.Modules.Demands.Services;
using OnDijon.Modules.Demands.Services.Interfaces;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using OnDijon.Modules.Services.Entities.Models;
using System.Windows.Input;
using OnDijon.Modules.Favorites.Services.Interfaces;
using OnDijon.Modules.Favorites.Entities.Models;
using OnDijon.Common.Extensions;
using OnDijon.Modules.Favorites.Entities.Response;
using OnDijon.Common.Utils;
using System.Linq;
using Newtonsoft.Json;
using OnDijon.Modules.JobOffer.Entities.Models;
using OnDijon.Modules.JobOffer.Entities.Responses;
using Prism.Commands;

namespace OnDijon.Modules.Favorites.ViewModels
{
    public class FavoritesViewModel : BaseViewModel
    {
        #region prop

        readonly ISession _session;
        readonly IFavoriteService _FavoriteService;
        readonly INavigationService _NavigationService;

        private List<FavoriteModel> _favAdress;
        public List<FavoriteModel> FavAddress 
        { get
            {
                return _favAdress;
            }
            set
            {
                Set(ref _favAdress, value);
            }
        }

        private bool _favoriteLayoutIsVisible;
        public bool FavoriteLayoutIsVisible { get => _favoriteLayoutIsVisible; set => Set(ref _favoriteLayoutIsVisible, value); }

        public DelegateCommand<FavoriteModel> GoToModifyFavoritesCommand { get; set; }
        public DelegateCommand<FavoriteModel> DeleteFavCommand { get; set; }
        public DelegateCommand GoToHomeCommand { get; set; }



        #endregion

        #region ctor

        public FavoritesViewModel(INavigationService navigationService,
                                   ITranslationService translationService,
                                   IPopupService popupService,
                                   ISession session,
                                   IFavoriteService favoriteService,
                                   ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            _FavoriteService = favoriteService;
            _NavigationService = navigationService;
            FavAddress = new List<FavoriteModel>();
            InitializeCommands();
        }

        public void InitializeCommands()
        {
            GoToModifyFavoritesCommand = new DelegateCommand<FavoriteModel>(async (address) => await DoGoToModifyFavoritesCommand(address));
            DeleteFavCommand = new DelegateCommand<FavoriteModel>((FavoriteModel favorite) => DoDeleteFavCommand(favorite));
            GoToHomeCommand = new DelegateCommand(async () => { await _NavigationService.NavigateTo(Locator.DashboardView); });
        }


        #endregion

        #region OnNavigatedToAsync

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            GetFavAdressList();
            Console.WriteLine(FavoriteLayoutIsVisible);

            if (FavAddress == null || FavAddress.Count == 0)
            {
                FavoriteLayoutIsVisible = false;
            }
            else
            {
                FavoriteLayoutIsVisible = true;
            }

        }

        #endregion

        #region Functions

        public void GetFavAdressList()
        {
            CallApi(async () =>
            {
                FavoriteAddressesListResponse resp = await _FavoriteService.GetFavorites(_session.Profile.Guid);
                ManageApiResponses(resp, new DefaultCallbackManager<FavoriteAddressesListResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        FavAddress = res.FavAdressesList;
                    }
                });
            });

        }

        private async Task DoGoToModifyFavoritesCommand(FavoriteModel selectedAddress)
        {
                // Créer les paramètres de navigation avec l'adresse sélectionnée
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("SelectedAddress", selectedAddress);

                // Naviguer vers la page de modification des favoris en utilisant la méthode personnalisée "NavigateTo"
                await _NavigationService.NavigateTo(Locator.ModifyFavoritesPage, navigationParameters: parameters);
        }


        private async void DoDeleteFavCommand(FavoriteModel favorite)
        {
            var result = await App.Current.MainPage.DisplayAlert("Attention !", "Êtes-vous sur de vouloir supprimer ce favoris?", "Oui", "Non");

            if (result)
            {
                try
                {
                    await _FavoriteService.DeleteFavAsync(favorite);
                    IsLoading = true;
                    await _NavigationService.NavigateTo(Locator.FavoritesPage);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        #endregion

    }
}
