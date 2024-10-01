
using System;
using System.Reflection;

using FFImageLoading.Svg.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views.Extensions
{
    /// <summary>
    /// Get image source from assembly resource by using partial name.
    /// </summary>
    /// <example>
    /// <ff:SvgCachedImage Source="{ImageFromResource file_name.svg}" />
    /// <Image Source="{ImageFromResource file_name.png}" />
    /// <Image Source="{ImageFromResource file_name.png, Assembly=MyAssembly}" />
    /// </example>
    /// <remarks>
    /// Source="resource://{AssemblyName}.{PathName}.{FileName}"
    /// </remarks>
    //https://docs.microsoft.com/fr-fr/xamarin/xamarin-forms/user-interface/images?tabs=windows#using-xaml
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Source))
            {
                return null;
            }

            Assembly assembly = typeof(ImageResourceExtension).GetTypeInfo().Assembly;

            return Source.EndsWith(".svg", StringComparison.OrdinalIgnoreCase)
                ? SvgImageSource.FromResource(Source, assembly)
                : ImageSource.FromResource(Source, assembly);
        }
    }



}
