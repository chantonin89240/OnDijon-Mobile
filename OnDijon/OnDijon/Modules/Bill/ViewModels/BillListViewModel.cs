using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Common.Entities;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.Bill.Entities.Models;
using OnDijon.Modules.Bill.Entities.Responses;
using OnDijon.Modules.Bill.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OnDijon.Modules.Bill.ViewModels
{
    public class BillListViewModel : BaseViewModel
    {
        readonly ISession _session;
        readonly IBillService _BillService;

        private List<BillModel> _billList;
        public List<BillModel> BillList
        {
            get => _billList;
            set => Set(ref _billList, value);
        }

        public ICommand PayLinkCommand { get; }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            GetBillList();
        }

        public void GetBillList()
        {
            CallApi(async () =>
            {
                BillListResponse resp = await _BillService.GetBills(_session.Profile.Guid);
                ManageApiResponses(resp, new DefaultCallbackManager<BillListResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (res.Bills.Any())
                        {
                            BillList = res.Bills;
                        }
                        else
                        {
                            PopupService.Show(PopupEnum.PopupError, "Aucune facture n'est disponible", "OK",
                            () => { NavigateTo(Locator.DashboardView); });
                        }
                    }
                });
            });
        }

        public BillListViewModel(INavigationService navigationService, ITranslationService translationService, IPopupService popupService, ISession session, IBillService billService, ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {
            BillList = new List<BillModel>();
            _session = session;
            _BillService = billService;

            PayLinkCommand = new Command<string>((url) =>
            {
                if (!string.IsNullOrEmpty(url))
                {
                    Browser.OpenAsync(new Uri(url), BrowserLaunchMode.SystemPreferred);
                }
            });
        }
    }
}
