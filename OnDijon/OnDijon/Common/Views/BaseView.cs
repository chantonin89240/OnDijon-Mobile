using Xamarin.Forms;

namespace OnDijon.Common.Views
{
    [System.Obsolete("Utiliser BasePage puis nettoyer le code behind", false)]
    public class BaseView : ContentPage
    {

        /*
         * TODO:
         * Il serait judicieux de modifier la BaseView en BaseView<T> where T : BaseViewModel
         * 
         * Ajouter une property pour avoir le viewmodel de la page
         * protected T ViewModel { get; private set; }
         * 
         * instancier cette propriété dans le constructeur de la BaseView
         * ViewModel = SimpleIoc.Default.GetInstance<T>();
         * 
         * Créer une méthode virtual (ou asbtract) OnViewAppearing() et OnViewDisappearing()
         * et l'appeler dans le OnAppearing/OnDisappearing de la BaseView ce qui aura pour but 
         *  - OnViewAppearing() : de lancer les chargements de data depuis le ViewModel et non depuis la vue
         *  - OnViewDisappearing() : de lancer le cleanup au besoin depuis le ViewModel et non côté vue
         * 
         */
        public BaseView()
        {
            this.BackgroundColor = Color.FromHex("#1A3972");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var topBarView = this.FindByName<TopBarView>("TopBarView");
            topBarView?.OnAppearing();

        }
    }
}
