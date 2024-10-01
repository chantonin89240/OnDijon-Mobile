using Xamarin.Forms;

namespace OnDijon.Common.Utils.Helpers
{
    public static class ViewHelper
    {
        public static Element SearchScrollView(Element element)
        {
            if (element is ScrollView)
            {
                return element;
            }
            else
            {
                return element.Parent != null ? SearchScrollView(element.Parent) : null;
            }
        }

    }
}
