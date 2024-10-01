using System;
using OnDijon.Modules.Abris.Serv;
using System.Threading.Tasks;
using OnDijon.Modules.Abris.Serv.Interfaces;
using OnDijon.Modules.Abris.ViewModels;
using OnDijon.Modules.Favorites.Entities.Models;
using Prism.Commands;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.Account.Services;
using OnDijon.Modules.Favorites.Services.Interfaces;
using OnDijon.Modules.UsefulContact.ViewsModels;
using OnDijon.Modules.Account.ViewModels;

namespace OnDijon.Modules.Abris.Views
{
    public partial class AbrisDetailView : PopupPage
    {
        private readonly IFavoriteService _favoriteService;
        readonly ISession _session;
        private AbrisViewModel _viewModel;



        public AbrisDetailView(IFavoriteService favoriteService, ISession session, AbrisViewModel viewmodel)
        {
            _favoriteService = favoriteService;
            _session = session;
            Init();
            BindingContext = viewmodel;
            _viewModel = viewmodel;
        }

        private void Init()
        {
            InitializeComponent();

        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();

        }

        private async void DoAddFavCommand(object sender, EventArgs e)
        {
            {
                try
                {
                    if (_session.Profile.Guid != null)
                    {
                        await _favoriteService.AddFavorite(_viewModel.SelectedPlacemark, _session.Profile.Guid);
                    }
                    else
                    {
                        Console.WriteLine("guid null");
                    }
                    await App.Current.MainPage.DisplayAlert("Wahou !", "L'adresse a bien été ajoutée dans vos favoris", "Ok");
                    await PopupNavigation.Instance.PopAsync();

                }
                catch (Exception res)
                {
                    Console.WriteLine(res);
                }

            }
        }

        protected override bool OnBackButtonPressed()
        {
            return !CloseWhenBackgroundIsClicked;
        }

    }
}
