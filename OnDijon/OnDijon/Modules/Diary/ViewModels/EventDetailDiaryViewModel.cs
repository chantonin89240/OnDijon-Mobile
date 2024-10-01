using System.Threading.Tasks;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Diary.Entities.Model;
using OnDijon.Modules.Diary.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System;
using Prism.Common;
using AsyncAwaitBestPractices.MVVM;

namespace OnDijon.Modules.Diary.ViewModels
{
    public class EventDetailDiaryViewModel : BaseViewModel
    {
        private IDiaryService DiaryService;

        private EventModel _eventDetail;
        public EventModel EventDetail
        {
            get { return _eventDetail; }
            set { Set(ref _eventDetail, value); }
        }


        private string _diaryEditId;
        public string DiaryEditId
        {
            get { return _diaryEditId; }
            set
            {
                Set(ref _diaryEditId, value);
            }
        }

        public ICommand CloseCommand { get; }
        public ICommand CloseViewCommand { get; }
        public ICommand LinkCommand { get; }
        public ICommand LaunchSearchFromOtherPageCommand { get; }
        public ICommand LaunchSearchFromDetailViewCommand { get; }

        #region EventDiaryListViewModel => ParentEventDiaryListViewModel

        private EventDiaryListViewModel _parentEventDiaryListViewModel;

        public EventDiaryListViewModel ParentEventDiaryListViewModel
        {
            get => _parentEventDiaryListViewModel;
            set => SetProperty(ref _parentEventDiaryListViewModel, value);
        }

        #endregion

        public EventDetailDiaryViewModel(INavigationService navigationService,
                                         ITranslationService translationService,
                                         IPopupService popupService,
                                         IDiaryService diaryService,
                                         ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            DiaryService = diaryService;

            CloseCommand = new Command(() => NavigationService.GoBackAsync());
            LinkCommand = new Command(async () => await Browser.OpenAsync(EventDetail.InfoLink, BrowserLaunchMode.SystemPreferred));
            CloseViewCommand = new Command(() => ParentEventDiaryListViewModel.IsEventDetailDisplay = false);
            LaunchSearchFromOtherPageCommand = new DelegateCommand<string>(LaunchSearchFromOtherPage);
            LaunchSearchFromDetailViewCommand = new DelegateCommand<string>(LaunchSearchFromDetailView);
        }

        public override Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<EventModel>(Constants.EventNavigationParameterKey, out var eventDetail))
            {
                EventDetail = eventDetail;
            }
            return base.OnNavigatedToAsync(parameters);
        }

        private void LaunchSearchFromOtherPage(string query)
        {
	        // DO refacto : passer par navigation parameters
            INavigationParameters param = new NavigationParameters
            {
                { Constants.QueryNavigationParameterKey, query}
            };
            NavigationService.NavigateAsync(Locator.EventListPage,param);
        }

        private void LaunchSearchFromDetailView(string query)
        {
	        // Refacto on accede directement au parent 
            ParentEventDiaryListViewModel.CommeWithQuery = true;
            ParentEventDiaryListViewModel.LaunchSearchFromOtherPage(query);
            ParentEventDiaryListViewModel.IsEventDetailDisplay = false;
        }

    }
}
