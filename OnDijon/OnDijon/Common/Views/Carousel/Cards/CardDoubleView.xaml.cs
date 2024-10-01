using OnDijon.Modules.Dashboard.Entities.Dto.Card;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardDoubleView : CardViewBase
    {
        public static readonly BindableProperty NotificationCountProperty = BindableProperty.Create(nameof(NotificationCount), typeof(int), typeof(CardDoubleView), propertyChanged: NotificationCountPropertyChanged);

        public int NotificationCount
        {
            get { return (int)GetValue(NotificationCountProperty); }
            set { SetValue(NotificationCountProperty, value); }
        }


        public CardDoubleView()
        {
            InitializeComponent();
        }

        protected override void OnCardPropertyChanged(CardDto newCardDto)
        {
            base.OnCardPropertyChanged(newCardDto);
            if (newCardDto != null)
            {
                SetImage(newCardDto.ImageUrl);
            }
        }



        private static void NotificationCountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CardDoubleView)bindable;

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