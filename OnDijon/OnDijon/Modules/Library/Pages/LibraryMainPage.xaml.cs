using OnDijon.Common.Views;
using OnDijon.Modules.Library.Entities.Model;
using OnDijon.Modules.Library.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Library.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LibraryMainPage : BasePage<LibraryMainViewModel>
    {

        public LibraryMainPage()
        {
            InitializeComponent();
        }
        
        void OnCurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            PageCounter.Text = "" + (ViewModel.AccountChoice.IndexOf((ReaderAccount)Carousel.CurrentItem) + 1) + " / " + ViewModel.AccountChoice.Count;
            LeftArrow.IsVisible = ViewModel.AccountChoice.IndexOf((ReaderAccount)Carousel.CurrentItem) != 0;
            RightArrow.IsVisible = (ViewModel.AccountChoice.IndexOf((ReaderAccount)Carousel.CurrentItem) + 1) != ViewModel.AccountChoice.Count;
        }

        protected override bool OnBackButtonPressed()
        {
	        return base.OnBackButtonPressed();
        }

    }
    
    public class LibraryCardsDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CardTemplate { get; set; }
        public DataTemplate AddCardTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((ReaderAccount)item).TypeAccount == BmCardType.NewCard ? AddCardTemplate : CardTemplate;
        }
    }

}