using AsyncAwaitBestPractices.MVVM;
using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.Simulator.Entities.Models;
using OnDijon.Modules.Simulator.Entities.Responses;
using OnDijon.Modules.Simulator.Services.Interfaces;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OnDijon.Modules.Simulator.ViewsModels
{
    public class SimulatorRateFormViewModel : BaseViewModel
    {
        readonly ISession _session;
        readonly ISimulatorRateService _SimulatorRateService;

        public string ChildNumberString { get; set; }
        public string IncomeString { get; set; }
        public string QFString { get; set; }
        public int ChildNumber { get; set; }
        public decimal Income { get; set; }
        public decimal QF { get; set; }
        public bool Resident { get; set; } = true;

        private CityContextModel _cityContext;
        public CityContextModel CityContext 
        { 
            get 
            { 
                return _cityContext; 
            } 
            set 
            {
                Set(ref _cityContext, value);
                RaisePropertyChanged(nameof(CityContext));
                SetIncomeChildVisible();
            } 
        }

        private IList<CityContextModel> _cities;
        public IList<CityContextModel> Cities
        {
            get => _cities;
            set
            {
                _cities = value;
                if (_cities != null)
                {
                    RaisePropertyChanged(nameof(Cities));

                    if (Cities.Count() == 1) 
                    {
                        CityContext = Cities.First();
                        CityIsVisible = false;
                    }
                }
            }
        }

        private bool _cityIsVisible = true;
        public bool CityIsVisible
        {
            get => _cityIsVisible;
            set => Set(ref _cityIsVisible, value);
        }

        private bool _childIsVisible;
        public bool ChildIsVisible
        {
            get => _childIsVisible;
            set => Set(ref _childIsVisible, value);
        }

        private bool _incomeIsVisible;
        public bool IncomeIsVisible
        {
            get => _incomeIsVisible;
            set => Set(ref _incomeIsVisible, value);
        }

        private bool _incomeLayoutErrorIsVisible;
        public bool IncomeLayoutErrorIsVisible
        {
            get => _incomeLayoutErrorIsVisible;
            set => Set(ref _incomeLayoutErrorIsVisible, value);
        }

        private bool _childNumberLayoutErrorIsVisible;
        public bool ChildNumberLayoutErrorIsVisible
        {
            get => _childNumberLayoutErrorIsVisible;
            set => Set(ref _childNumberLayoutErrorIsVisible, value);
        }

        private bool _qFLayoutErrorIsVisible;
        public bool QFLayoutErrorIsVisible
        {
            get => _qFLayoutErrorIsVisible;
            set => Set(ref _qFLayoutErrorIsVisible, value);
        }

        private bool _cityLayoutErrorIsVisible;
        public bool CityLayoutErrorIsVisible
        {
            get => _cityLayoutErrorIsVisible;
            set => Set(ref _cityLayoutErrorIsVisible, value);
        }

        public ICommand CalculateButtonCommand { get; set; }

        public SimulatorRateFormViewModel(INavigationService navigationService,
                                          ITranslationService translationService,
                                          IPopupService popupService,
                                          ISession session,
                                          ISimulatorRateService simulatorRateService,
                                          ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            _SimulatorRateService = simulatorRateService;
            CalculateButtonCommand = new AsyncCommand(CalculateCommand);
        }

        public override async Task OnNavigatedToAsyncNew(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsyncNew(parameters);
            GetCities();
        }

        public void GetCities()
        {
            CallApi(async () =>
            {
                CityContextResponse response = await _SimulatorRateService.GetAllCityContext(Constants.SimulatorRateCode);

                ManageApiResponses(response, new DefaultCallbackManager<CityContextResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        Cities = response.Cities;
                    }
                });
            });
        }

        public void SetIncomeChildVisible()
        {
            ChildIsVisible = CityContext.IsDoubleCompute;
            IncomeIsVisible = CityContext.IsDoubleCompute;
        }

        public void Simulate()
        {
            CallApi(async () =>
            {
                SimulatorRateResponse response = await _SimulatorRateService.GetSimulatorRate(ChildNumber, Income, Resident, QF, CityContext.Id);

                ManageApiResponses(response, new DefaultCallbackManager<SimulatorRateResponse>(PopupService)
                {
                    OnSuccess = async (res) =>
                    {
                        INavigationParameters param = new NavigationParameters
                        {
                            { Constants.SimulatorRateNavigationParameterKey, JsonConvert.SerializeObject(res.DomainSimulatorRate) },
                            { Constants.SimulatorTitleNavigationParameterKey, res.Title },
                            { Constants.SimulatorWarningNavigationParameterKey, res.WarningMessage }
                        };

                        await NavigationService.NavigateAsync(Locator.SimulatorRatePage,param);
                    }
                });
            });
        }

        private async Task CalculateCommand()
        {
            ChildNumberLayoutErrorIsVisible = false;
            IncomeLayoutErrorIsVisible = false;
            QFLayoutErrorIsVisible = false;
            CityLayoutErrorIsVisible = CityContext == null;

            if ((!int.TryParse(ChildNumberString, out int childNumber) || string.IsNullOrWhiteSpace(ChildNumberString)) && ChildIsVisible)
            {
                ChildNumberLayoutErrorIsVisible = true;
            }

            if ((!decimal.TryParse(IncomeString, out decimal income) || string.IsNullOrWhiteSpace(IncomeString) || income <= 0) && IncomeIsVisible)
            {
                IncomeLayoutErrorIsVisible = true;
            }

            if (!decimal.TryParse(QFString, out decimal qf) || string.IsNullOrWhiteSpace(QFString) || qf <= 0)
            {
                QFLayoutErrorIsVisible = true;
            }

            bool isOk = !CityLayoutErrorIsVisible && !QFLayoutErrorIsVisible && !IncomeLayoutErrorIsVisible && !ChildNumberLayoutErrorIsVisible;

            if (isOk)
            {
                ChildNumber = childNumber;
                Income = income;
                QF = qf;
                Simulate();
            }
        }
    }
}
