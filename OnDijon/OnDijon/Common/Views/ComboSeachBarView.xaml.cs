using System;
using System.Windows.Input;
using Prism.Commands;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ComboSearchBarView : Frame
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ComboSearchBarView), defaultBindingMode: BindingMode.TwoWay, propertyChanged: TextPropertyChanged);
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ComboSearchBarView), propertyChanged: PlaceholderPropertyChanged);
        public static readonly BindableProperty SearchCommandProperty = BindableProperty.Create(nameof(SearchCommand), typeof(ICommand), typeof(ComboSearchBarView));
        public static readonly BindableProperty DisplayOnFocusProperty = BindableProperty.Create(nameof(DisplayOnFocus), typeof(bool), typeof(ComboSearchBarView), defaultValue: false);
        public static readonly BindableProperty RefreshOnTextChangedProperty = BindableProperty.Create(nameof(RefreshOnTextChanged), typeof(bool), typeof(ComboSearchBarView), defaultValue: false);
        public static readonly BindableProperty HideDisplayOnUnfocusProperty = BindableProperty.Create(nameof(HideDisplayOnUnfocus), typeof(bool), typeof(ComboSearchBarView), defaultValue: false);


        public bool HideDisplayOnUnfocus
        {
            get { return (bool)GetValue(HideDisplayOnUnfocusProperty); }
            set
            {
                if(value == true)
                {
                    SearchEntry.Unfocus();
                }               
                SetValue(HideDisplayOnUnfocusProperty, value);
            }

        }

        public bool RefreshOnTextChanged
        {
            get { return (bool)GetValue(RefreshOnTextChangedProperty); }
            set { SetValue(RefreshOnTextChangedProperty, value); }

        }
        public bool DisplayOnFocus
        {
            get { return (bool)GetValue(DisplayOnFocusProperty); }
            set { SetValue(DisplayOnFocusProperty, value); }
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public ICommand SearchButtonCommand { get; set; }

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public ComboSearchBarView()
        {
            InitializeComponent();
            SearchButtonCommand = new DelegateCommand(SearchAction);
            ImageButton.Command = SearchButtonCommand;
            SearchEntry.ReturnCommand = SearchButtonCommand;
            SearchEntry.Focused += SearchEntry_Focused;
        }


        private void SearchEntry_Focused(object sender, FocusEventArgs e)
        {
            if (DisplayOnFocus)
            {
                SearchAction();

            }
        }

        public void SearchAction()
        {
            if (SearchCommand != null)
            {
                SearchCommand.Execute(Text);
            }
        }

        private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (ComboSearchBarView)bindable;
            view.SearchEntry.Text = newValue?.ToString();
        }

        private static void PlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (ComboSearchBarView)bindable;
            view.SearchEntry.Placeholder = newValue?.ToString();
        }

        void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            Text = e.NewTextValue;
            TextChanged?.Invoke(this, e);
            if (RefreshOnTextChanged)
            {
                SearchAction();
            }

        }

    }
}