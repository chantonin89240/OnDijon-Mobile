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
using OnDijon.Common.Services;
using Prism.Commands;

namespace OnDijon.Modules.Favorites.ViewModels
{
    public class ModifyFavoritesViewModel : BaseViewModel
    {
        #region prop

        readonly ISession _session;
        readonly IFavoriteService _FavoriteService;


        //public List<FavoriteModel> FavAddress { get; set; }

        readonly INavigationService _NavigationService;

        private bool _favoriteLayoutIsVisible;
        public bool FavoriteLayoutIsVisible { get => _favoriteLayoutIsVisible; set => Set(ref _favoriteLayoutIsVisible, value); }
        public DelegateCommand SaveCommand{ get; set; }


        private string rue;

        public string Rue { get => rue; set => Set(ref rue, value); }

        private int codePostal;

        public int CodePostal { get => codePostal; set => Set(ref codePostal, value); }

        private string ville;
        private int profilId;


        public string Ville { get => ville; set => Set(ref ville, value); }
        public int ProfilId { get => profilId; set => Set(ref profilId, value); }

        public static FavoriteModel SelectedAddress { get; private set; }

        #endregion

        # region ctor

        public ModifyFavoritesViewModel(INavigationService navigationService,
                                   ITranslationService translationService,
                                   IFavoriteService favoriteService,
                                   IPopupService popupService,
                                   ISession session,
                                   ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            _NavigationService = navigationService;
            _FavoriteService = favoriteService;
            SaveCommand = new DelegateCommand(async () => await DoSave());

        }

        #endregion

        #region OnNavigatedToAsync

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);

            if (parameters.TryGetValue("SelectedAddress", out FavoriteModel selectedAddress))
            {
                SelectedAddress = selectedAddress;
                Rue = SelectedAddress.Rue;
                CodePostal = SelectedAddress.CodePostal;
                Ville = SelectedAddress.Ville;
                ProfilId = SelectedAddress.ProfilId;
            }
        }


        private async Task DoSave()
        {
            SelectedAddress.CodePostal = CodePostal;
            SelectedAddress.Rue = Rue;
            SelectedAddress.Ville = Ville;
            SelectedAddress.ProfilId = ProfilId;

            await _FavoriteService.UpdateAddressAsync(SelectedAddress);

            // Naviguer vers la page de modification des favoris en utilisant la méthode personnalisée "NavigateTo"
            await _NavigationService.NavigateTo(Locator.FavoritesPage);
        }
        #endregion

    }
}
