using System;
using CoreGraphics;
using OnDijon.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Frame), typeof(CustomFrameRenderer))]
namespace OnDijon.iOS.Renderers
{
    /// <summary>
    /// Make iOS frame shadows look more like Android cardviews
    /// </summary>
    public class CustomFrameRenderer : FrameRenderer
    {
        public override void Draw(CGRect rect)
        {
            try
            {
                base.Draw(rect);

                if (Element.HasShadow)
                {
                    Layer.ShadowColor = UIColor.Gray.CGColor;
                    Layer.ShadowOpacity = 0.25f;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}