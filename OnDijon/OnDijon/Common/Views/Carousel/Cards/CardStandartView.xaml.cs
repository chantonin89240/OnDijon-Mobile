using OnDijon.Modules.Dashboard.Entities.Dto.Card;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardStandartView : CardViewBase
    {
        public static readonly BindableProperty NotificationCountProperty = BindableProperty.Create(nameof(NotificationCount), typeof(int), typeof(CardStandartView), propertyChanged: NotificationCountPropertyChanged);


        public int NotificationCount
        {
            get { return (int)GetValue(CardProperty); }
            set { SetValue(CardProperty, value); }
        }

        public string ImageUrl
        {
            get { return Card.ImageUrl; }
        }


        public CardStandartView()
        {
            InitializeComponent();
        }

        protected override void OnCardPropertyChanged(CardDto newCardDto)
        {
            base.OnCardPropertyChanged(newCardDto);
        }



        private static void NotificationCountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CardStandartView)bindable;

            var notif = (int)newValue;
            view.NotificationCountBadge.IsVisible = notif > 0;
        }



        private void SetImage(string image)
        {
            if (!string.IsNullOrEmpty(image))
            {
                ImageSource imageSource = ImageSource.FromUri(new System.Uri(image));
                Image.Source = imageSource;
            }
        }

    }
}