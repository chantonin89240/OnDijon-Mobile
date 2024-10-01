using System;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.UsefulContact.Entities.Models;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace OnDijon.Modules.UsefulContact.ViewsModels
{
    public class WorkInfosViewModel : BaseViewModel
    {

        #region variables et commandes

        private ContactModel _Contact { get; set; }
        public ContactModel Contact
        {
            get
            {
                return _Contact;
            }
            set
            {
                _Contact = value;
                RaisePropertyChanged(nameof(Contact));
            }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { Set(ref _isRefreshing, value); }
        }

        public ICommand CloseCommand { get; }
        public ICommand CloseViewCommand { get; }
        
        public Action CloseWorkInfosDetailDisplayAction { get; set; }
        #endregion

        public WorkInfosViewModel(INavigationService navigationService,
                                  ITranslationService translationService,
                                  IPopupService popupService,
                                  ILoggerService loggerService  ) : base(navigationService, translationService, popupService, loggerService)
        {
            CloseCommand = new Command(() => NavigationService.GoBackAsync());
            CloseViewCommand = new Command(() => CloseWorkInfosDetailDisplayAction?.Invoke());
        }
        

    }
}
