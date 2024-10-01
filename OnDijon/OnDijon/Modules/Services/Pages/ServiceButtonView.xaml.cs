using FFImageLoading.Svg.Forms;
using OnDijon.Common.Utils.Fonts;
using OnDijon.Common.Views.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Services.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceButtonView : StackLayout
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(ServiceButtonView), propertyChanged: TitlePropertyChanged);
        public static readonly BindableProperty NotificationCountProperty = BindableProperty.Create(nameof(NotificationCount), typeof(int), typeof(ServiceButtonView), propertyChanged: NotificationCountPropertyChanged);
        public static readonly BindableProperty ImageNameProperty = BindableProperty.Create(nameof(ImageName), typeof(string), typeof(ServiceButtonView), propertyChanged: ImageNamePropertyChanged);
        public static readonly BindableProperty IsFavouriteProperty = BindableProperty.Create(nameof(IsFavourite), typeof(bool), typeof(ServiceButtonView), propertyChanged: IsFavouritePropertyChanged);
        public static readonly BindableProperty IsFavouriteVisibleProperty = BindableProperty.Create(nameof(IsFavouriteVisible), typeof(bool), typeof(ServiceButtonView), propertyChanged: IsFavouriteVisiblePropertyChanged);

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public int NotificationCount
        {
            get { return (int)GetValue(NotificationCountProperty); }
            set { SetValue(NotificationCountProperty, value); }
        }

        public string ImageName
        {
            get { return (string)GetValue(ImageNameProperty); }
            set { SetValue(ImageNameProperty, value); }
        }

        public bool IsFavourite
        {
            get { return (bool)GetValue(IsFavouriteProperty); }
            set { SetValue(IsFavouriteProperty, value); }
        }

        public bool IsFavouriteVisible
        {
            get { return (bool)GetValue(IsFavouriteVisibleProperty); }
            set { SetValue(IsFavouriteVisibleProperty, value); }
        }


        public ServiceButtonView()
        {
            InitializeComponent();

            Label.Text = Title;

            NotificationContainer.IsVisible = NotificationCount > 0 ? true : false;
            NotificationCountLabel.Text = NotificationCount.ToString();

            SetImage(ImageName);
        }



        private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (ServiceButtonView)bindable;

            view.Label.Text = (string)newValue;
        }

        private static void NotificationCountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (ServiceButtonView)bindable;

            view.NotificationContainer.IsVisible = (int)newValue > 0 ? true : false;
            view.NotificationCountLabel.Text = ((int)newValue).ToString();
        }

        private static void ImageNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (ServiceButtonView)bindable;

            view.SetImage((string)newValue);
        }


        private static void IsFavouriteVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (ServiceButtonView)bindable;

            view.FavouriteContainer.IsVisible = (bool)newValue;
        }


        private static void IsFavouritePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (ServiceButtonView)bindable;
            bool isFavourite = (bool)newValue;
            var source = new FontImageSource
            {
                Glyph = isFavourite ? MaterialDesignIcons.Star : MaterialDesignIcons.StarOutline,
                FontFamily = (OnPlatform<string>)Application.Current.Resources["MaterialDesignIcons"],
                Size = 20,
                Color = isFavourite ? (Color)Application.Current.Resources["FavouriteColor"] : Color.Black
            };
            view.FavouriteImage.Source = source;
        }


        private void SetImage(string image)
        {
            if (!string.IsNullOrEmpty(image))
            {
                ImageSource imageSource;
                if (image.StartsWith("http"))
                {
                    if (image.EndsWith(".png") || image.EndsWith(".jpg") || image.EndsWith(".jpeg"))
                    {
                        imageSource = ImageSource.FromUri(new System.Uri(image));
                    }
                    else
                    {
                        imageSource = SvgImageSource.FromUri(new System.Uri(image));
                    }
                }
                else
                {
                    imageSource = SvgImageSource.FromResource($"OnDijon.Assets.{image}", typeof(ImageResourceExtension).Assembly);
                }
                Image.Source = imageSource;
            }
        }

    }
}