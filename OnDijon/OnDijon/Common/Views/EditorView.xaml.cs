using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditorView : Frame
    {

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(EditorView), string.Empty, BindingMode.TwoWay, propertyChanged: TextPropertyChanged);
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(EditorView), propertyChanged: PlaceholderPropertyChanged);
        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(EditorView), propertyChanged: MaxLengthPropertyChanged);

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

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }


        public EditorView()
        {
            InitializeComponent();

            Editor.PropertyChanged += Editor_PropertyChanged;
        }

        private void Editor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TextProperty.PropertyName)
            {
                Text = Editor.Text;
            }
        }

        private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (EditorView)bindable;
            view.Editor.Text = (string)newValue;
        }

        private static void PlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (EditorView)bindable;
            view.Editor.Placeholder = (string)newValue;
        }

        private static void MaxLengthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (EditorView)bindable;
            view.Editor.MaxLength = (int)newValue;
        }


    }
}