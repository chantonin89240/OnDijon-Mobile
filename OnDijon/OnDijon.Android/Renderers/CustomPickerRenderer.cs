using Android.Content;
using Android.Text;
using OnDijon.Droid.Renderers;
using OnDijon.Droid.Renderers.Utils;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Picker), typeof(CustomPickerRenderer))]
namespace OnDijon.Droid.Renderers
{
    /// <summary>
    /// Fix font styles on Android
    /// </summary>
    class CustomPickerRenderer : Xamarin.Forms.Platform.Android.AppCompat.PickerRenderer
    {
        public CustomPickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Typeface = FontUtils.GetFont(Element.FontFamily, Element.FontAttributes);
                Control.Ellipsize = TextUtils.TruncateAt.Start;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Control.Typeface = FontUtils.GetFont(Element.FontFamily, Element.FontAttributes);
        }
    }
}