using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using OnDijon.Common.Entities;
using OnDijon.Modules.Report.Entities.Dto;
using OnDijon.Modules.Report.Entities.Request;
using OnDijon.Modules.Report.Entities.Response;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils.Services.Interfaces;
using Xamarin.Forms;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Report.Services.Interfaces;
using Prism.Navigation;
using System.Threading.Tasks;
using OnDijon.Common.Utils;
using Newtonsoft.Json;

namespace OnDijon.Modules.Report.ViewModels
{
    public class ReportDetailViewModel : BaseViewModel
    {
        private readonly IReportService _reportService;
        private readonly IUserIdService _userIdService;

        public ObservableCollection<ReportHistoryDtoWrapper> ReportHistoryList
        {
            get
            {
                var historyDtos = Report?.HistoryList?.Select(h => new ReportHistoryDtoWrapper(h))?.ToList();
                if (!(historyDtos?.Any() ?? false))
                    return new ObservableCollection<ReportHistoryDtoWrapper>();
                
                historyDtos.OrderBy(x => x.Date);
                historyDtos.Last().IsLastItem = true;

                return new ObservableCollection<ReportHistoryDtoWrapper>(historyDtos);
            }
        }
       

        private ReportDto _report;
        public ReportDto Report
        {
            get { return _report; }
            set
            {
                _report = value;
                RaisePropertyChanged(nameof(Report));
                RaisePropertyChanged(nameof(PhotoAvailable));
                RaisePropertyChanged(nameof(ReportHistoryList));
            }
        }

        private bool _isBtnSubscribeVisible;

        public bool IsBtnSubscribeVisible
        {
            get { return _isBtnSubscribeVisible; }
            set { RaisePropertyChanged(nameof(IsBtnSubscribeVisible)); }
        }

        public bool PhotoAvailable => Report?.PhotoUrl != null;

        public ICommand SubscribeCommand { get; }

        public ICommand CloseCommand { get; }

        public ReportDetailViewModel(INavigationService navigationService,
                                     ITranslationService translationService,
                                     IPopupService popupService,
                                     IReportService reportService,
                                     IUserIdService userIdService, ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _reportService = reportService;
            _userIdService = userIdService;

            SubscribeCommand = new Command(SubscribeToReport);

            CloseCommand = new Command(() => NavigationService.GoBackAsync());
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
            if(parameters.TryGetValue(Constants.ReportDetailNavigationKey,out ReportDto reportDetail))
            {
                Report = reportDetail;
                if (reportDetail.IsOwner != null)
                {
                    IsBtnSubscribeVisible = (bool)!reportDetail.IsOwner;
                }
            }
            else if (parameters.TryGetValue(Constants.ReportDetailNotificationItemIdKey, out string notificationItemId))
            {
                GetReport(notificationItemId);
            }
            
        }
        
        

        public void GetReport(string reportId)
        {
            CallApi(async () =>
            {
                var response = await _reportService.GetReport(reportId);
                ManageApiResponses(response, new CallbackManager<DtoResponse<ReportDto>>
                {
                    OnSuccess = (res) => GetReportType(res.Data)
                });
            });
        }

        private void GetReportType(ReportDto report)
        {
            CallApi(async () =>
            {
                ReportTypesListResponse response = await _reportService.GetReportTypes();
                ManageApiResponses(response, new CallbackManager<ReportTypesListResponse>
                {
                    OnSuccess = (res) =>
                    {
                        var type = res.ReportTypes.FirstOrDefault(t => t.Code.Equals(report.TypeCode));
                        report.TypeIconUrl = type?.ImageUrl;
                        report.TypeName = type?.Name ?? string.Empty;
                        Report = report;
                    }
                });
            });
        }

        private void SubscribeToReport()
        {
            CallApi(async () =>
            {
                var request = new SubscribeRequest { ReportId = Report.Id, RegistrationToken = await _userIdService.GetUserId() };
                var response = await _reportService.SubscribeToReport(request);
                ManageApiResponses(response, new CallbackManager<Response>
                {
                    OnSuccess = SubscribeToReportOnSuccess
                });
            });
        }

        private void SubscribeToReportOnSuccess(Response response)
        {
            PopupService.Show(PopupEnum.PopupSuccess,
                "Signalement confirmé",
                "Merci pour votre action citoyenne !",
                "Vous pouvez désormais suivre son traitement dans Suivre mes signalements",
                "Revenir aux services",
                () =>
                {
                    NavigateTo(Locator.DashboardView);
                });
        }
    }
    
    public class ReportHistoryDtoWrapper : ReportHistoryDto
    {
        public ReportHistoryDtoWrapper(ReportHistoryDto reportHistoryDto)
        {
            Date = reportHistoryDto.Date;
            Description = reportHistoryDto.Description;
            Status = reportHistoryDto.Status;
            Message = reportHistoryDto.Message;
            IsLastItem = false;
        }

        public bool IsLastItem { get; set; }
    }
                                           
}
