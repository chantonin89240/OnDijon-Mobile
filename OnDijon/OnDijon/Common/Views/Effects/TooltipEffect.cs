using System.Linq;
using Xamarin.Forms;

/// <summary>
/// http://www.xamboy.com/2019/03/01/showing-tooltips-in-xamarin-forms/
/// </summary>
namespace OnDijon.Common.Views.Effects
{
    public static class TooltipEffect
    {
        public static readonly BindableProperty HasTooltipProperty =
          BindableProperty.CreateAttached("HasTooltip", typeof(bool), typeof(TooltipEffect), false, propertyChanged: OnHasTooltipChanged);
        public static readonly BindableProperty TextColorProperty =
          BindableProperty.CreateAttached("TextColor", typeof(Color), typeof(TooltipEffect), Color.Black);
        public static readonly BindableProperty BackgroundColorProperty =
          BindableProperty.CreateAttached("BackgroundColor", typeof(Color), typeof(TooltipEffect), Color.White);
        public static readonly BindableProperty TextProperty =
          BindableProperty.CreateAttached("Text", typeof(string), typeof(TooltipEffect), string.Empty);
        public static readonly BindableProperty FontSizeProperty =
          BindableProperty.CreateAttached("FontSize", typeof(double), typeof(TooltipEffect), 14d);

        private static View _view;

        public static bool GetHasTooltip(BindableObject view)
        {
            return (bool)view.GetValue(HasTooltipProperty);
        }

        public static void SetHasTooltip(BindableObject view, bool value)
        {
            view.SetValue(HasTooltipProperty, value);
        }

        public static Color GetTextColor(BindableObject view)
        {
            return (Color)view.GetValue(TextColorProperty);
        }

        public static void SetTextColor(BindableObject view, Color value)
        {
            view.SetValue(TextColorProperty, value);
        }

        public static Color GetBackgroundColor(BindableObject view)
        {
            return (Color)view.GetValue(BackgroundColorProperty);
        }

        public static void SetBackgroundColor(BindableObject view, Color value)
        {
            view.SetValue(BackgroundColorProperty, value);
        }

        public static string GetText(BindableObject view)
        {
            return (string)view.GetValue(TextProperty);
        }

        public static void SetText(BindableObject view, string value)
        {
            view.SetValue(TextProperty, value);
        }

        public static double GetFontSize(BindableObject view)
        {
            return (double)view.GetValue(FontSizeProperty);
        }

        public static void SetFontSize(BindableObject view, double value)
        {
            view.SetValue(FontSizeProperty, value);
        }

        static void OnHasTooltipChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is View view))
            {
                return;
            }

            bool hasTooltip = (bool)newValue;
            if (hasTooltip)
            {
                view.Effects.Add(new ControlTooltipEffect());
                _view = view;
            }
            else
            {
                var toRemove = view.Effects.FirstOrDefault(e => e is ControlTooltipEffect);
                if (toRemove != null)
                {
                    view.Effects.Remove(toRemove);
                }
            }
        }

        public static void Remove()
        {
            if (_view != null)
            {
                SetHasTooltip(_view, false);
            }
        }
    }

    class ControlTooltipEffect : RoutingEffect
    {
        public ControlTooltipEffect() : base("OnDijon.TooltipEffect")
        {

        }
    }
}
