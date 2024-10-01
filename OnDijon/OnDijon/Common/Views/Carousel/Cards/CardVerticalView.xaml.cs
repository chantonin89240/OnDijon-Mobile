using OnDijon.Modules.Dashboard.Entities.Dto.Card;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardVerticalView : CardViewBase
    {
        public static readonly BindableProperty NotificationCountProperty = BindableProperty.Create(nameof(NotificationCount), typeof(int), typeof(CardVerticalView), propertyChanged: NotificationCountPropertyChanged);


        public int NotificationCount
        {
            get { return (int)GetValue(CardProperty); }
            set { SetValue(CardProperty, value); }
        }

        public string ImageUrl
        {
            get { return Card.ImageUrl; }
        }


        public CardVerticalView()
        {
            InitializeComponent();
        }

        protected override void OnCardPropertyChanged(CardDto newCardDto)
        {
            base.OnCardPropertyChanged(newCardDto);
        }


        private static void NotificationCountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CardVerticalView)bindable;

            var notif = (int)newValue;
            view.NotificationCountBadge.IsVisible = notif > 0;
        }

    }
}