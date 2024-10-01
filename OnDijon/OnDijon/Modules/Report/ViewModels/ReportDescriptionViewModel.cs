using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Modules.Account.Services.Interfaces;
using Plugin.Media.Abstractions;
using SkiaSharp;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Windows.Input;
using OnDijon.Common.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using AsyncAwaitBestPractices.MVVM;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Modules.Report.Entities.Dto;

namespace OnDijon.Modules.Report.ViewModels
{
    public class ReportDescriptionViewModel : ReportBaseViewModel
    {
        private const int MAX_PHOTO = 1;

        private readonly ISession _session;
        private readonly IPhotoService _photoService;
        private readonly IOrientationService _orientationService;
        private bool _takePhotoMode;
        private DisplayRotation _orientation;
        private LayoutOptions _photoBtnHorizontalLayout;
        private LayoutOptions _photoBtnVerticalLayout;
        private Thickness _photoBtnMargin;

        private DisplayRotation Orientation
        {
            get => _orientation;
            set
            {
                if (Set(ref _orientation, value))
                {
                    // value is different
                    if (value == DisplayRotation.Rotation0)
                    {
                        PhotoBtnHorizontalLayout = LayoutOptions.Center;
                        PhotoBtnVerticalLayout = LayoutOptions.End;
                        PhotoBtnMargin = new Thickness(0d, 0d, 0d, 40d);
                    }
                    else if (value == DisplayRotation.Rotation90)
                    {
                        PhotoBtnHorizontalLayout = LayoutOptions.Start;
                        PhotoBtnVerticalLayout = LayoutOptions.Center;
                        PhotoBtnMargin = new Thickness(40d, 0d, 0d, 0d);
                    }
                    else if (value == DisplayRotation.Rotation270)
                    {
                        PhotoBtnHorizontalLayout = LayoutOptions.End;
                        PhotoBtnVerticalLayout = LayoutOptions.Center;
                        PhotoBtnMargin = new Thickness(0d, 0d, 40d, 0d);
                    }
                    else
                    {
                        PhotoBtnHorizontalLayout = LayoutOptions.Center;
                        PhotoBtnVerticalLayout = LayoutOptions.Start;
                    }
                }
            }
        }

        public LayoutOptions PhotoBtnHorizontalLayout
        {
            get => _photoBtnHorizontalLayout;
            set => Set(ref _photoBtnHorizontalLayout, value);
        }

        public LayoutOptions PhotoBtnVerticalLayout
        {
            get => _photoBtnVerticalLayout;
            set => Set(ref _photoBtnVerticalLayout, value);
        }

        public Thickness PhotoBtnMargin
        {
            get => _photoBtnMargin;
            set => Set(ref _photoBtnMargin, value);
        }

        public string Description
        {
            get { return _session.ReportRequest.ReportContent.Description; }
            set
            {
                _session.ReportRequest.ReportContent.Description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        public bool TakePhotoMode
        {
            get => _takePhotoMode;
            set
            {
                if (value == _takePhotoMode) return;
                _takePhotoMode = value;
                RaisePropertyChanged();
            }
        }

        private bool _canAddPhoto;
        public bool CanAddPhoto
        {
            get
            {
                return _canAddPhoto;
            }
            set { Set(ref _canAddPhoto, value); }
        }

        private bool _isPhotoActionVisible;
        public bool IsPhotoActionVisible
        {
            get => _isPhotoActionVisible;
            set => Set(ref _isPhotoActionVisible, value);
        }


        private IList<ImageSource> _photoSources;
        public IList<ImageSource> PhotoSources {
            get { return _photoSources; }
            set
            {
                var photos = _session.ReportRequest.ReportContent.Photos;
                CanAddPhoto = photos == null || photos.Count < MAX_PHOTO;
                IsPhotoActionVisible = false;
                Set(ref _photoSources, value);
            }
        }

        public ICommand AddPhotoCommand { get; }

        public ICommand RemovePhotoCommand { get; }

        public ICommand GoToNextPageCommand { get; }
        public ICommand DisplayPhotoLayoutCommand { get; }

        public ICommand TakePhotoCommand { get; }

        public ICommand SwitchPhotoModeCommand { get; }

        public ReportDescriptionViewModel(INavigationService navigationService,
                                          ITranslationService translationService,
                                          IPopupService popupService,
                                          ISession session,
                                          IPhotoService photoService,
                                          ILoggerService loggerService) : base(navigationService, translationService, popupService, session, loggerService)
        {
            _session = session;
            _photoService = photoService;
            _orientationService = DependencyService.Get<IOrientationService>(); ;

            AddPhotoCommand = new AsyncCommand<PhotoSourceEnum>(async source => await AddPhoto(source));

            RemovePhotoCommand = new Command<ImageSource>(source => RemovePhoto(source));

            GoToNextPageCommand = new AsyncCommand(async () => await NavigationService.NavigateAsync(Locator.ReportSummaryView));
            DisplayPhotoLayoutCommand = new Command(OnDisplayPhotoLayoutCommand);
            CanAddPhoto = true;
            GoToNextPageCommand = new Command(() => NavigateTo(Locator.ReportSummaryView));

            SwitchPhotoModeCommand = new Command(() => TakePhotoMode = true);

            TakePhotoCommand = new Command<MediaCapturedEventArgs>((args) =>
            {
                var data = RotateImage(args.ImageData, args.Rotation);
                data = ResizeImage(data, 1920, 1080);
                AddPhoto(data);
                TakePhotoMode = false;
            });
            Orientation = DisplayRotation.Rotation0;
            _orientationService.DisplayRotationChanged += OnDisplayRotationChanged;
        }


        private void OnDisplayRotationChanged(object sender, DisplayRotation rotation)
        {
            Orientation = rotation;
        }

        private byte[] RotateImage(byte[] imageData, double rotation)
        {
            if (null == imageData)
            {
                return null;
            }

            if (Orientation == DisplayRotation.Rotation90)
            {
                // default rotation is 90°, so no rotation to apply
                rotation -= 90d;
            }
            else if (Orientation == DisplayRotation.Rotation270)
            {
                // flip rotation so we go full 180° rotation
                rotation += 90d;
            }

            while (rotation >= 360d)
            {
                rotation -= 360d;
            }

            while (rotation <= -360d)
            {
                rotation += 360d;
            }

            using (var image = SKImage.FromEncodedData(imageData))
            {
                var width = image.Width;
                var height = image.Height;

                if (Math.Abs(Math.Abs(rotation) - 90d) < double.Epsilon)
                {
                    // we rotate from 90 degree
                    width = image.Height;
                    height = image.Width;
                }
                using (var targetImage = new SKBitmap(width, height))
                {
                    using (var canvas = new SKCanvas(targetImage))
                    {
                        canvas.Clear();
                        canvas.Translate(targetImage.Width / 2f, targetImage.Height / 2f);
                        canvas.RotateDegrees((float)rotation);
                        canvas.Translate(-image.Width / 2f, -image.Height / 2f);
                        canvas.DrawImage(image, 0, 0);
                    }

                    using (var img = SKImage.FromBitmap(targetImage))
                    {
                        using (var raw = img.Encode(SKEncodedImageFormat.Jpeg, 100))
                        {
                            return raw.ToArray();
                        }
                    }
                }
            }
        }

        private byte[] ResizeImage(byte[] imageData, int maxWidth, int maxHeight)
        {
            if (null == imageData)
            {
                return null;
            }

            using (var srcImage = SKImage.FromEncodedData(imageData))
            {
                if (srcImage.Width <= maxWidth && srcImage.Height <= maxHeight)
                {
                    // no need to downscale, returns
                    return imageData;
                }

                var targetScale = 1.0f;

                if (srcImage.Width > maxWidth)
                {
                    targetScale = (float)maxWidth / srcImage.Width;
                }

                if (srcImage.Height > maxHeight && ((float)maxHeight / srcImage.Height) < targetScale)
                {
                    targetScale = (float)maxHeight / srcImage.Height;
                }

                // now scale that to the size that we want
                SKImageInfo desired = new SKImageInfo((int)(srcImage.Width * targetScale),
                    (int)(srcImage.Height * targetScale));
                using (SKImage resizedImage = SKImage.Create(desired))
                {
                    srcImage.ScalePixels(resizedImage.PeekPixels(), SKFilterQuality.High);

                    using (var raw = resizedImage.Encode(SKEncodedImageFormat.Jpeg, 100))
                    {
                        return raw.ToArray();
                    }
                }
            } // !using srcImage
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            IsPhotoActionVisible = false;
            _orientationService.Start(SensorSpeed.Default);
        }

        public override Task OnNavigatedFromAsync(INavigationParameters parameters)
        {
            return base.OnNavigatedFromAsync(parameters);
            _orientationService.Stop();
        }

        
        public void OnDisplayPhotoLayoutCommand()
        {
            IsPhotoActionVisible = true;
            CanAddPhoto = false;
        }

        private async Task AddPhoto(PhotoSourceEnum source)
        {
            byte[] photoBytes = null;

            try
            {
                photoBytes = await _photoService.Open(source);
            }
            catch (MediaPermissionException)
            {
                PopupService.Show(PopupEnum.PopupError, "Action impossible", "Votre permission est requise pour effectuer cette action", "OK");
            }

            AddPhoto(photoBytes);
        }

        private void AddPhoto(byte[] photoBytes)
        {
            if (photoBytes != null)
            {
                _session.ReportRequest.ReportContent.Photos = _session.ReportRequest.ReportContent.Photos ?? new List<byte[]>();
                _session.ReportRequest.ReportContent.Photos.Add(photoBytes);
                UpdatePhotoSources();
            }
        }

        private void RemovePhoto(ImageSource source)
        {
            int index = Convert.ToInt32(source.StyleId);
            _session.ReportRequest.ReportContent.Photos.RemoveAt(index);
            UpdatePhotoSources();
        }

        private void UpdatePhotoSources()
        {
            PhotoSources = _session.ReportRequest.ReportContent.Photos.Select((bytes, index) =>
            {
                ImageSource photoSource = ImageSource.FromStream(() => new MemoryStream(bytes));
                //on stocke l'indice dans l'objet pour pouvoir le supprimer par la suite
                photoSource.StyleId = index.ToString();
                return photoSource;
            }).ToList();

            RaisePropertyChanged(nameof(PhotoSources));
        }

        public override void Cleanup()
        {
            _orientationService.Stop();
            PhotoSources = null;
            base.Cleanup();
        }
    }
}
