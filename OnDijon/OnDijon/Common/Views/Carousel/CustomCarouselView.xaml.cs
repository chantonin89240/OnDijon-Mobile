using System.Collections.ObjectModel;
using System.Windows.Input;
using OnDijon.Modules.Dashboard.Entities.Dto.Card;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomCarouselView : StackLayout
    {
        public static readonly BindableProperty CardsProperty = BindableProperty.Create(nameof(Cards), typeof(ObservableCollection<CardDto>), typeof(CustomCarouselView), propertyChanged: CardsPropertyChanged);
        public static readonly BindableProperty ActionCommandProperty = BindableProperty.Create(nameof(ActionCommand), typeof(ICommand), typeof(CustomCarouselView));

        public ObservableCollection<CardDto> Cards
        {
            get { return (ObservableCollection<CardDto>)GetValue(CardsProperty); }
            set { SetValue(CardsProperty, value); }
        }

        public ICommand ActionCommand
        {
            get { return (ICommand)GetValue(ActionCommandProperty); }
            set { SetValue(ActionCommandProperty, value); }
        }

        public string PageLabel { get; set; }


        public CustomCarouselView()
        {
            InitializeComponent();
        }



        private void CarouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            UpdatePagination();
        }

        private void CarouselView_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            UpdatePagination();
        }

        private static void CardsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CustomCarouselView)bindable;
            view.UpdatePagination();
        }

        private void Previous_Tapped(object sender, System.EventArgs e)
        {
            if (CarouselView.Position > 0)
            {
                CarouselView.ScrollTo(CarouselView.Position - 1);
            }
        }

        private void Next_Tapped(object sender, System.EventArgs e)
        {
            if (CarouselView.Position < Cards.Count - 1)
            {
                CarouselView.ScrollTo(CarouselView.Position + 1);
            }
        }


        private void CardAlertView_ActionClicked(CardActionDto action)
        {
            ActionCommand?.Execute(action);
        }

        private void UpdatePagination()
        {
            PageCounter.Text = $"{CarouselView.Position + 1} / {Cards?.Count}";
            if (CarouselView.Position != 0)
            {
                LeftArrow.FadeTo(1.0f, 200);
            }
            else
            {
                LeftArrow.FadeTo(0f, 200);
            }
            if (CarouselView.Position != Cards.Count - 1)
            {
                RightArrow.FadeTo(1.0f, 200);
            }
            else
            {
                RightArrow.FadeTo(0f, 200);
            }
        }
    }
}