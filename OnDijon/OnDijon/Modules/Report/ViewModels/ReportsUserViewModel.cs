using Esri.ArcGISRuntime.Mapping;
using OnDijon.Common.Entities;
using OnDijon.Modules.Report.Entities.Dto;
using OnDijon.Modules.Report.Entities.Response;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils.UI;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using OnDijon.Common.Extensions;
using Xamarin.Forms;
using OnDijon.Modules.Report.Services.Interfaces;
using OnDijon.Common.ViewModels;
using Prism.Navigation;
using OnDijon.Common.Utils;
using Newtonsoft.Json;
using AsyncAwaitBestPractices.MVVM;
using System.Threading.Tasks;
using OnDijon.Common.Services;

namespace OnDijon.Modules.Report.ViewModels
{
    public class ReportsUserViewModel : BaseViewModel
    {
        private readonly IReportService _reportService;
        private readonly IUserIdService _userIdService;

        public ICommand GetReportsCommand { get; }

        public ICommand GoToReportCommand { get; }

        public ICommand GoToReportTypeCommand { get; }

        private IList<ReportDto> _reports;
        public IList<ReportDto> Reports 
        {
            get { return _reports; }
            set 
            {
                _reports = value;
                RaisePropertyChanged(nameof(Reports));
                ReportCount = $"Vous avez déclaré {Reports?.Count ?? 0} {((Reports?.Count ?? 0) > 1 ? "signalements" : "signalement")}";
            } 
        }

        private string _reportCount;
        public string ReportCount
        {
            get { return _reportCount; }
            set
            {
                _reportCount = value;
                RaisePropertyChanged(nameof(ReportCount));
            }
        }

        public bool DoDisplayReportsEmpty => Reports == null || Reports.Count == 0;

        public bool DoDisplayList
        {
            get => _doDisplayList;
            set
            {
                Set(ref _doDisplayList, value);
                RaisePropertyChanged(nameof(DoDisplayReportsEmpty));
                RaisePropertyChanged(nameof(CanDisplayList));
                RaisePropertyChanged(nameof(CanDisplayMap));
            }
        }

        public bool CanDisplayList
        {
            get => !DoDisplayReportsEmpty && DoDisplayList;
        }

        public bool CanDisplayMap
        {
            get => !DoDisplayReportsEmpty && !DoDisplayList;
        }

        private Map _map;
        private bool _doDisplayList;

        public Map Map
        {
            get => _map;
            private set => Set(ref _map, value);
        }

        public ReportsUserViewModel(INavigationService navigationService,
                                    ITranslationService translationService,
                                    IPopupService popupService,
                                    IReportService reportService,
                                    IUserIdService userIdService, ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _reportService = reportService;
            _userIdService = userIdService;

            GetReportsCommand = new AsyncCommand(GetReports);

            GoToReportCommand = new AsyncCommand<ReportDto>((report) => OnReportCommand(report)) ;

            GoToReportTypeCommand = new AsyncCommand(async () => await NavigationService.NavigateTo(Locator.ReportTypeView));
        }

        private async Task OnReportCommand(ReportDto dto)
        {
            
            await NavigationService.NavigateTo(Locator.ReportDetailView, navigationParameters: NavigationParametersFactory(dto, Constants.ReportDetailNavigationKey));
        }

        private async Task GetReports()
        {
            CallApi(async () =>
            {
                string userId = await _userIdService.GetUserId();
                DtoListResponse<ReportDto> response = await _reportService.GetReports(userId);
                ManageApiResponses(response, new CallbackManager<DtoListResponse<ReportDto>>
                {
                    OnSuccess = async (res) =>
                    {
                        Reports = res.Data;
                        await GetReportsIcons();
                    }
                });
            });
        }

        private async Task GetReportsIcons()
        {
            CallApi(async () =>
            {
                ReportTypesListResponse response = await _reportService.GetReportTypes();
                ManageApiResponses(response, new CallbackManager<ReportTypesListResponse>
                {
                    OnSuccess = (res) =>
                    {
                        foreach (ReportDto report in Reports)
                        {
                            ReportTypeDto type = res.ReportTypes.FirstOrDefault(t => t.Code.Equals(report.TypeCode));
                            report.TypeIconUrl = type?.ImageUrl;
                            report.TypeName = type?.Name ?? string.Empty;
                        }
                        RaisePropertyChanged(nameof(Reports));
                        RaisePropertyChanged(nameof(DoDisplayReportsEmpty));
                        RaisePropertyChanged(nameof(CanDisplayList));
                        RaisePropertyChanged(nameof(CanDisplayMap));
                    }
                });
            });
        }

        public void InitMap()
        {
            try
            {
                Map = new Map(Basemap.CreateOpenStreetMap())
                {
                    MinScale = MapUtils.MIN_MAP_SCALE
                };
            }
            catch (Exception ex)
            {
                ShowError("Erreur lors de l'initialisation de la carte", "Action impossible", ex);
            }
        }
    }
}
