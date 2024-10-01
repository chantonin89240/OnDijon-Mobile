using FFImageLoading.Svg.Forms;
using OnDijon.Common.Utils.Resources;
using OnDijon.Common.Views.Extensions;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OnDijon.Common.Utils.Tools
{
    public static class ImageTool
    {
        public static ImageSource FromUri(string name)
        {
            if (!Path.HasExtension(name))
                name += ".png";
            return ImageSource.FromUri(new Uri(DMResources.Configuration_ImageSourceUriBase + name));
        }

        public static async Task<byte[]> DownloadImageAsync(string imageUrl)
        {
            try
            {   
                HttpClient _httpClient = new HttpClient();
                byte[] image = await _httpClient.GetByteArrayAsync(imageUrl);
                return image;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ImageSource convertSourceImage(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }

            return source.EndsWith(".svg", StringComparison.OrdinalIgnoreCase)
                ? SvgImageSource.FromResource(source, typeof(ImageResourceExtension).Assembly)
                : ImageSource.FromResource(source, typeof(ImageResourceExtension).Assembly);
        }
    }
}
