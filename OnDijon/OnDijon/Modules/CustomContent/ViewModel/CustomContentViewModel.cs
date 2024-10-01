using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Common.Entities;
using OnDijon.Modules.CustomContent.Entities;
using OnDijon.Modules.CustomContent.Entities.Models;
using OnDijon.Modules.CustomContent.Services.Interfaces;
using System.Windows.Input;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading.Tasks;
using Prism.Xaml;
using OnDijon.Common.Utils;

namespace OnDijon.Modules.CustomContent.ViewModel
{
    public class CustomContentViewModel : BaseViewModel
    {
        readonly ICustomContentService _customContentService;

        #region Accessors
        private CustomContentModel _customContentModel;
        public CustomContentModel CustomContentModel
        {
            get { return _customContentModel; }
            set
            {
                Set(ref _customContentModel, value);
            }
        }
        private string _contentType;
        public string ContentType
        {
            get { return _contentType; }
            set
            {
                Set(ref _contentType, value);
            }
        }
        
        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                Set(ref _description, value);
            }
        }

        private string _image;
        public string Image
        {
            get { return _image; }
            set
            {
                Set(ref _image, value);
            }
        }

        private string _video;
        public string Video
        {
            get { return _video; }
            set
            {
                Set(ref _video, value);
            }
        }
        
        private string _externalLink;
        public string ExternalLink
        {
            get { return _externalLink; }
            set
            {
                Set(ref _externalLink, value);
            }
        }
        
        private string _externalLinkTitle;
        public string ExternalLinkTitle
        {
            get { return _externalLinkTitle; }
            set
            {
                Set(ref _externalLinkTitle, value);
            }
        }
        public ICommand CloseCommand { get; }
        public ICommand ExternalLinkCommand { get; }


        #endregion

        public CustomContentViewModel(INavigationService navigationService, ITranslationService translationService, IPopupService popupService, ICustomContentService customContentService, ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {
            _customContentService = customContentService;
            CloseCommand = new Command(() => NavigationService.GoBackAsync());
            ExternalLinkCommand = new Command<string>((url) =>
            {
                if (!string.IsNullOrEmpty(url))
                    Launcher.OpenAsync(new System.Uri(url));
            });
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            if(parameters.TryGetValue(Constants.ServiceNavigationKey,out string customContentId)){
                GetCustomContent(customContentId);
            }
            else if (parameters.TryGetValue(GenericNavigationParameterKey, out string genericNavigationParameter))
            {
                GetCustomContent(genericNavigationParameter);
            }
            await base.OnNavigatedToAsync(parameters);
        }

        public override void Cleanup()
        {
            base.Cleanup();
            CustomContentModel = null;
        }


        public void GetCustomContent(string customContentId)
        {
            IsLoading = true;
            CallApi(async () =>
            {
                CustomContentResponse response = await _customContentService.GetCustomContent(customContentId);
                ManageApiResponses(response, new DefaultCallbackManager<CustomContentResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        CustomContentModel = new CustomContentModel()
                        {
                            Title = res.CustomContent.Title,
                            Description = res.CustomContent.Description,
                            Image = res.CustomContent.Image,
                            Video = res.CustomContent.Video,
                            ExternalLinkTitle = res.CustomContent.ExternalLinkTitle,
                            ExternalLink = res.CustomContent.ExternalLink,
                        };
                        IsLoading = false;
                    }
                });
            });
        }
    }
}
