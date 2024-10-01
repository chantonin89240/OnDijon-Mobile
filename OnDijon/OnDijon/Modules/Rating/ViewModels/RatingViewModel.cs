
using OnDijon.Common.Entities;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Rating.Views;
using OnDijon.Modules.Rating.Entities.Model;
using OnDijon.Modules.Rating.Entities.Response;
using OnDijon.Modules.Rating.Services.Interfaces;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OnDijon.Modules.Rating.ViewModels
{
    public class RatingViewModel : BaseViewModel
    {

        #region variables
        private IRatingService _ratingService;
        private readonly ICacheService _cacheService;

        private IPopupViewSettings _settings;

        private string CacheRatingKey = "cache_rating";
        private bool WsVerify = false;
        private bool InactiveForThisSession = false;

        private RatingSessionModel Session;

        private bool _displayBottomButtons = false;
        public bool DisplayBottomButtons
        {
            get => _displayBottomButtons; set => Set(ref _displayBottomButtons, value);
        }

        private bool _displayQuestion1 = false;
        public bool DisplayQuestion1
        {
            get => _displayQuestion1; set => Set(ref _displayQuestion1, value);
        }

        private bool _displayQuestion2 = false;
        public bool DisplayQuestion2
        {
            get => _displayQuestion2; set => Set(ref _displayQuestion2, value);
        }

        private bool _displayQuestion3 = false;
        public bool DisplayQuestion3
        {
            get => _displayQuestion3; set => Set(ref _displayQuestion3, value);
        }

        private bool _closeWhenBackgroundIsClicked = true;
        public bool CloseWhenBackgroundIsClicked
        {
            get => _closeWhenBackgroundIsClicked; set => Set(ref _closeWhenBackgroundIsClicked, value);
        }

        private bool _displayComment = false;
        public bool DisplayComment
        {
            get => _displayComment; set => Set(ref _displayComment, value);
        }

        private bool _enableValidate = false;
        public bool EnableValidate
        {
            get => _enableValidate; set => Set(ref _enableValidate, value);
        }


        private int _note = 0;
        public int Note
        {
            get => _note;
            set
            {
                Set(ref _note, value);
                UpdateDisplay();
            }
        }


        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                EnableValidate = value.Length > 0;
                Set(ref _comment, value);
            }
        }

        public ICommand VerifyRatingSessionCommand;
        public ICommand SendCommand;
        #endregion

        public RatingViewModel(INavigationService navigationService,
            ITranslationService translationService,
            IPopupService popupService,
            ICacheService cacheService,
            IRatingService ratingService,
            ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {
            _ratingService = ratingService;
            _cacheService = cacheService;

            VerifyRatingSessionCommand = new Command(async () => await VerifyRatingSessionAsync());
            SendCommand = new Command(() => SendRating());

        }

        private async Task AddOneCountRatingAsync()
        {
            Session.VisitCount++;
            if (Session.VisitCount == Session.NumberVisitDashboard || ((Session.VisitCount - Session.NumberVisitDashboard) % Session.Incrementation) == 0)
            {
                PopupService.Show(new RatingPopupView(this));
            }
            await _cacheService.Put<RatingSessionModel>(CacheRatingKey, Session, CacheType.Default);
        }

        private void UpdateDisplay()
        {
            DisplayComment = true;
            EnableValidate = true;
            DisplayQuestion1 = false; DisplayQuestion2 = false; DisplayQuestion3 = false;

            if (Note < 7)
            {
                DisplayQuestion1 = true;
            }
            else if (Note < 9)
            {
                DisplayQuestion2 = true;
            }
            else
            {
                DisplayQuestion3 = true;
            }
        }

        public async Task VerifyRatingSessionAsync()
        {
            if (!InactiveForThisSession && WsVerify)
            {
                if (Session != null && Session.BeginDatePublication < DateTime.Now && Session.EndDatePublication > DateTime.Now)
                {
                    await AddOneCountRatingAsync();
                }
            }
            else if (!InactiveForThisSession)
            {
                CallApi(async () =>
                {
                    Session = await _cacheService.Get<RatingSessionModel>(CacheRatingKey, CacheType.Default);

                    GetSessionRatingResponse response = await _ratingService.GetActualRatingSession();
                    ManageApiResponses(response, new DefaultCallbackManager<GetSessionRatingResponse>(PopupService)
                    {
                        OnSuccess = async (res) =>
                        {
                            if (Session == null || (Session != null && res.EditId != Session.EditId))
                            {
                                if (res.HasSession)
                                {
                                    Session = new RatingSessionModel()
                                    {
                                        EditId = res.EditId,
                                        BeginDatePublication = res.BeginDatePublication,
                                        EndDatePublication = res.EndDatePublication,
                                        HasSession = res.HasSession,
                                        Incrementation = res.Incrementation,
                                        NumberVisitDashboard = res.NumberVisitDashboard,
                                        PublicationDate = res.PublicationDate,
                                        VisitCount = 0
                                    };
                                }
                                else
                                {
                                    Session = null;
                                    InactiveForThisSession = true;
                                }
                            }
                            WsVerify = true;
                            if (Session != null)
                            {
                                await AddOneCountRatingAsync();
                            }
                        },
                        OnInvalidInformations = (res) =>
                        {
                            InactiveForThisSession = true;
                        }
                    });
                });
            }

        }

        public void SendRating()
        {
            CallApi(async () =>
            {
                Response response = await _ratingService.SendRatingSession(Session.EditId, Note, Comment);
                ManageApiResponses(response, new DefaultCallbackManager<Response>(PopupService)
                {
                    OnSuccess = async (res) =>
                    {
                        await _cacheService.Delete<RatingSessionModel>(CacheRatingKey, CacheType.Default);
                        Session = null;
                        InactiveForThisSession = true;
                        if (Note > 6)
                        {
                            PopupService.Show(PopupEnum.PopupSuccess, "Merci pour votre participation !", "OK");
                        }
                        else
                        {
                            PopupService.Show(PopupEnum.PopupSuccess, "Merci pour votre participation ! Votre retour sera bien pris en compte dans le cadre de notre politique d’amélioration continue.", "OK");
                        }

                    },
                    OnError = (res) =>
                    {
                        PopupService.Show(PopupEnum.PopupError, "Une erreur est survenue", "Retour");
                    }
                });
            });
        }

        public void UpdateSettings(IPopupViewSettings settings)
        {
            _settings = settings;
            DisplayBottomButtons = settings.DisplayBottomButtons;
            CloseWhenBackgroundIsClicked = settings.CloseWhenBackgroundIsClicked;
        }

    }
}
