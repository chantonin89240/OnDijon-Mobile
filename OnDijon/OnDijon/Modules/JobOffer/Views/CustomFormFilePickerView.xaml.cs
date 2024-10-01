using OnDijon.Common.Utils.Fonts;
using OnDijon.Common.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.JobOffer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomFormFilePickerView : StackLayout
    {
        public static readonly BindableProperty FileContentProperty = BindableProperty.Create(nameof(FileContent), typeof(string), typeof(CustomFormFilePickerView), defaultBindingMode: BindingMode.TwoWay);
        //public static readonly BindableProperty FileContentProperty = BindableProperty.Create(nameof(FileContent), typeof(byte[]), typeof(CustomFormFilePickerView), defaultBindingMode: BindingMode.TwoWay);
        public static readonly BindableProperty FileNameProperty = BindableProperty.Create(nameof(FileName), typeof(string), typeof(CustomFormFilePickerView), defaultBindingMode: BindingMode.TwoWay, propertyChanged: FileNamePropertyChanged);
        public static readonly BindableProperty CancelReinitializeProperty = BindableProperty.Create(nameof(CancelReinitialize), typeof(bool), typeof(CustomFormFilePickerView));
        public static readonly BindableProperty ErrorsProperty = BindableProperty.Create(nameof(Errors), typeof(IList<string>), typeof(CustomFormFilePickerView), defaultBindingMode: BindingMode.TwoWay, propertyChanged: ErrorsPropertyChanged);
        public static readonly BindableProperty SupportedExtensionsProperty = BindableProperty.Create(nameof(SupportedExtensions), typeof(IList<string>), typeof(CustomFormFilePickerView));


        /// <summary>Liste des extensions des pièces justificatives acceptées.</summary>
        public static readonly IEnumerable<string> SupportDocumentExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".pdf", ".doc", ".docx", ".odt", ".rtf", ".txt", ".ods", ".xls", ".xlsx", ".msg", ".csv" };
        /*public byte[] FileContent
        {
            get { return (byte[])GetValue(FileContentProperty); }
            set { SetValue(FileContentProperty, value); }
        } */
        public string FileContent
        {
            get { return (string)GetValue(FileContentProperty); }
            set { SetValue(FileContentProperty, value); }
        }

        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        public bool CancelReinitialize
        {
            get { return (bool)GetValue(CancelReinitializeProperty); }
            set { SetValue(CancelReinitializeProperty, value); }
        }

        public IList<string> Errors
        {
            get { return (IList<string>)GetValue(ErrorsProperty); }
            set { SetValue(ErrorsProperty, value); }
        }

        public IList<string> SupportedExtensions
        {
            get { return (IList<string>)GetValue(SupportedExtensionsProperty); }
            set { SetValue(SupportedExtensionsProperty, value); }
        }

        private bool _isErrorVisible;
        public bool IsErrorVisibile
        {
            get { return _isErrorVisible; }
            set
            {
                _isErrorVisible = value;
                OnPropertyChanged(nameof(IsErrorVisibile));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public CustomFormFilePickerView()
        {
            InitializeComponent();
            CreateErrorMessage();
        }

        private static void FileNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CustomFormFilePickerView)bindable;
            view.SelectedFileName.Text = newValue?.ToString();
            if (newValue != null)
            {
                view.SelectedFileName.IsVisible = true;
            }
            else
            {
                view.SelectedFileName.IsVisible = false;
            }
        }

        private async void SelectFile(object sender, EventArgs e)
        {
            try
            {
                CancelReinitialize = true;
                var file = await FilePicker.PickAsync();
                if (file != null)
                {
                    foreach(var extension in SupportedExtensions)
                    {
                        //if (file.FileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                        if (file.FileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                        {
                            FileContent = Convert.ToBase64String(System.IO.File.ReadAllBytes(file.FullPath));
                            FileName = file.FileName;
                            IsErrorVisibile = false;
                        }
                    }


                }
            }
            catch (Exception ex)
            {

            }
        }

        private void CreateErrorMessage()
        {
            //this.Children.Clear();

            Color errorColor = (Color)Application.Current.Resources["ErrorColor"];

            // Error label
            Label errorLabel = new Label { BindingContext = this, VerticalOptions = LayoutOptions.Center, TextColor = errorColor };
            errorLabel.SetBinding(Label.TextProperty, new Binding(nameof(ErrorMessage)));

            // Error image
            var source = new FontImageSource
            {
                Glyph = MaterialDesignIcons.Alert,
                FontFamily = (OnPlatform<string>)Application.Current.Resources["MaterialDesignIcons"],
                Size = 20,
                Color = errorColor
            };
            Image image = new Image { Source = source, WidthRequest = 14, HeightRequest = 14 };

            // Error StackLayout
            StackLayout errorStackLayout = new StackLayout { BindingContext = this, Orientation = StackOrientation.Horizontal };
            errorStackLayout.Children.Add(image);
            errorStackLayout.Children.Add(errorLabel);
            errorStackLayout.SetBinding(StackLayout.IsVisibleProperty, new Binding(nameof(IsErrorVisibile)));

            StackLayout stack = new StackLayout { Orientation = StackOrientation.Vertical };

            stack.Children.Add(errorStackLayout);

            this.Children.Add(stack);

        }

        private static void ErrorsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CustomFormFilePickerView)bindable;
            var errors = (IList<string>)newValue;
            if (errors != null && errors.Any())
            {
                view.IsErrorVisibile = true;
                view.ErrorMessage = errors.First();

                var scrollView = ViewHelper.SearchScrollView(view) as ScrollView;
                if (scrollView != null)
                {
                    // scroll to error form
                    scrollView.ScrollToAsync(scrollView.X, view.Y, false);
                }
            }
            else
            {
                view.IsErrorVisibile = false;
                view.ErrorMessage = string.Empty;
            }
        }

    }
}