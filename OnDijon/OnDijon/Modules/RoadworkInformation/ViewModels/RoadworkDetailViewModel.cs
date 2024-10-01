using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.ViewModels;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Modules.RoadworkInformation.Entities.Models;
using Xamarin.Forms;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using OnDijon.Modules.Account.Services.Interfaces;
using Prism.Navigation;

namespace OnDijon.Modules.RoadworkInformation.ViewModels
{
    public class RoadworkDetailViewModel : BaseViewModel
    {
        readonly ISession _session;

        #region variables et commandes

        private RoadworkInfoModel _RoadworkDetail;
        public RoadworkInfoModel RoadworkDetail
        {
            get { return _RoadworkDetail; }
            set { Set(ref _RoadworkDetail, value); }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { Set(ref _isRefreshing, value); }
        }

        public ICommand CloseCommand { get; }
        public ICommand CloseViewCommand { get; }

        #region RoadworkInformationViewModel => ParentRoadworkInformationViewModel

        private RoadworkInformationViewModel _parentRoadworkInformationViewModel;

        public RoadworkInformationViewModel ParentRoadworkInformationViewModel
        {
            get => _parentRoadworkInformationViewModel;
            set => SetProperty(ref _parentRoadworkInformationViewModel, value);
        }

        #endregion
        #endregion

        public RoadworkDetailViewModel(INavigationService navigationService,
                                       ITranslationService translationService,
                                       IPopupService popupService,
                                       ISession session,
                                       ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            CloseCommand = new AsyncCommand(NavigationService.GoBackAsync);
            CloseViewCommand = new Command(() => ParentRoadworkInformationViewModel.DisplayRoadworkDetail = false);
        }

    }
}
