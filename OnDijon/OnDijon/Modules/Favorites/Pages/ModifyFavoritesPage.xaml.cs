﻿using OnDijon.Common.Views;
using OnDijon.Modules.Favorites.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Favorites.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifyFavoritesPage : BasePage<ModifyFavoritesViewModel>
    {
        public ModifyFavoritesPage()
        {
            InitializeComponent();
        }

    }
}