using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Library.Entities.Model;
using Prism.Navigation;

namespace OnDijon.Modules.Library.ViewModels
{
    public class LibraryCardViewModel : BaseViewModel
    {
        private ReaderAccount _accompt;
        public ReaderAccount Accompt
        {
            get => _accompt;
            set
            {
                IsAccountActive = value != null;
                if (IsAccountActive)
                {
                    FullName = value.FullName;
                    BarCode = string.IsNullOrEmpty(value.BarCode) ? null : value.BarCode;
                }
                Set(ref _accompt, value);
            }
        }

        private bool _isAccountActive;
        public bool IsAccountActive { get => _isAccountActive; set => Set(ref _isAccountActive, value); }

        private string _fullName;
        public string FullName { get => _fullName; set => Set(ref _fullName, value); }

        private string _barCode;
        public string BarCode { get => _barCode; set => Set(ref _barCode, value); }


        public LibraryCardViewModel(INavigationService navigationService,
                                    ITranslationService translationService,
                                    IPopupService popupService,
                                    ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
        }
    }
}
