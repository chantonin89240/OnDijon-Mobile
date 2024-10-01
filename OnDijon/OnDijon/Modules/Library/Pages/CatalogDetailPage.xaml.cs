using OnDijon.Common.Views;
using OnDijon.Modules.Library.ViewModels;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Library.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogDetailPage : BasePage<CatalogDetailViewModel>
    {
  

        public CatalogDetailPage()
        {
            InitializeComponent();
        }

      
        // Refacto, ce replacement en binding n'est pas simple en effet, 
        // le binding sur le IsEnabled n'est pas fonctionnel quand il y a aussi un binding sur une Command,
        // vu que le IsEnabled est Bindé par défaut sur le CanExecute de la Command,
        // Ici il fallait donc créer dans le view model la fonction CanExecute pour la commande concernée 
        // private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        // {
        //     switch (e.PropertyName)
        //     {
        //         case nameof(ViewModel.Holdings):
        //             //TODO
        //             //((Button)sender).IsVisible = ViewModel.Holdings.First().IsReservable;
        //             var idx = 0;
        //             
        //             StackHolding.Children.ToList().ForEach(block =>
        //             {
        //                 ((Grid)block).Children.Last().IsEnabled = ViewModel.Holdings.ElementAt(idx).IsReservable;
        //                 idx++;
        //             });
        //             break;
        //         default:
        //             break;
        //     }
        // }

     

    }
}