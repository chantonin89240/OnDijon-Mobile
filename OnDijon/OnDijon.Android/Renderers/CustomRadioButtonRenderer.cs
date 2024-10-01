using System.ComponentModel;

using Android.Content;

using OnDijon.Droid.Renderers;
using OnDijon.Droid.Renderers.Utils;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RadioButton), typeof(CustomRadioButtonRenderer))]
namespace OnDijon.Droid.Renderers
{
    /// <summary>
    /// Fix font styles on Android
    /// </summary>
    class CustomRadioButtonRenderer : RadioButtonRenderer
    {
        public CustomRadioButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<RadioButton> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Typeface = FontUtils.GetFont(Element.FontFamily, Element.FontAttributes);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Control.Typeface = FontUtils.GetFont(Element.FontFamily, Element.FontAttributes);
        }
    }
}