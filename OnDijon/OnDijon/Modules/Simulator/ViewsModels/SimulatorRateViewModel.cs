using Newtonsoft.Json;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Simulator.Entities.Models;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OnDijon.Modules.Simulator.ViewsModels
{
    public class SimulatorRateViewModel : BaseViewModel
    {
        private List<DomainSimulatorRateModel> _domainsSimulatorRate;
        public List<DomainSimulatorRateModel> DomainsSimulatorRate 
        {
            get
            {
                return _domainsSimulatorRate;
            }
            set
            {
                _domainsSimulatorRate = value;
                RaisePropertyChanged(nameof(DomainsSimulatorRate));
            }
        }
        public string SimulatorTitle { get; set; }
        public string SimulatorWarningMessage { get; set; }
        public DomainSimulatorRateModel _DomainSimulatorRateSelected { get; set; }
        public DomainSimulatorRateModel DomainSimulatorRateSelected
        {
            get
            {
                return _DomainSimulatorRateSelected;
            }
            set
            {
                _DomainSimulatorRateSelected = value;
                RaisePropertyChanged(nameof(DomainSimulatorRateSelected));
            }
        }

        public ICommand LoadItemsCommand { get; set; }

        public SimulatorRateViewModel(INavigationService navigationService,
                                      ITranslationService translationService,
                                      IPopupService popupService,
                                      ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            LoadItemsCommand = new Command(new Action(() => LoadItems()));
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);

            if (parameters.TryGetValue(Constants.SimulatorRateNavigationParameterKey, out string simulatorRateJson))
            {
                List<DomainSimulatorRateModel> simulatorRateModel = JsonConvert.DeserializeObject<List<DomainSimulatorRateModel>>(simulatorRateJson);
                DomainsSimulatorRate = simulatorRateModel;
            }

            if (parameters.TryGetValue(Constants.SimulatorTitleNavigationParameterKey, out string simulatorTitle))
            {
                SimulatorTitle = simulatorTitle;
            }

            if (parameters.TryGetValue(Constants.SimulatorWarningNavigationParameterKey, out string simulatorWarningMessage))
            {
                SimulatorWarningMessage = simulatorWarningMessage;
            }

            LoadItems();
        }

        private void LoadItems()
        {
            DomainSimulatorRateSelected = DomainsSimulatorRate.FirstOrDefault();
        }
    }
}