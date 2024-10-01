using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces.Front;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using System.Threading.Tasks;

namespace OnDijon.Common.Services
{
    class PhotoService : IPhotoService
    {
        public async Task<byte[]> Open(PhotoSourceEnum source)
        {
            MediaFile photo = null;

            switch (source)
            {
                case PhotoSourceEnum.Camera:
                    photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        PhotoSize = PhotoSize.Small,
                        CompressionQuality = 90
                    });
                    break;
                case PhotoSourceEnum.PhotoLibrary:
                    photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Small,
                        CompressionQuality = 90
                    });
                    break;
            }

            if (photo != null)
            {
                return StreamToBytes(photo.GetStream());
            }
            else
            {
                return null;
            }
        }

        private static byte[] StreamToBytes(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
