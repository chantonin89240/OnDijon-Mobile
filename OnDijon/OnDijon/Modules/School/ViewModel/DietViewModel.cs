using AsyncAwaitBestPractices.MVVM;
using OnDijon.Common.Entities;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils.Command;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.School.Entities.Models;
using OnDijon.Modules.School.Entities.Response;
using OnDijon.Modules.SchoolServices.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OnDijon.Modules.School.ViewModel
{
    public class DietViewModel : BaseViewModel
    {

        readonly ISchoolRestaurantBookingConfigurationService MainService;

        private ChildByCityModel _child;
        public ChildByCityModel Child
        {
            get { return _child; }
            set
            {
                Set(ref _child, value);
            }
        }

        private bool _hasOption;
        public bool HasOption
        {
            get { return _hasOption; }
            set
            {
                Set(ref _hasOption, value);
                RaisePropertyChanged(nameof(HasOption));
            }
        }

        private ChildDietModel InitialChildDiet;

        private bool _optionDiet;
        public bool OptionDiet
        {
            get { return _optionDiet; }
            set
            {
                Set(ref _optionDiet, value);
                RaisePropertyChanged(nameof(OptionDiet));
                UpdateModifyStatus();
            }
        }

        private List<string> _possibleStandardDiets;
        public List<string> PossibleStandardDiets
        {
            get { return _possibleStandardDiets; }
            set
            {
                Set(ref _possibleStandardDiets, value);
                RaisePropertyChanged(nameof(PossibleStandardDiets));
            }
        }

        private string _standardDiet;
        public string StandardDiet
        {
            get { return _standardDiet; }
            set
            {
                Set(ref _standardDiet, value);
                RaisePropertyChanged(nameof(StandardDiet));
                UpdateModifyStatus();
            }
        }
        private int _indiceStandardDiet;
        public int IndiceStandardDiet
        {
            get { return _indiceStandardDiet; }
            set
            {
                Set(ref _indiceStandardDiet, value);
                RaisePropertyChanged(nameof(StandardDiet));
            }
        }

        private bool _isModify;
        public bool IsModify
        {
            get
            {

                return _isModify;
            }
            set
            {
                Set(ref _isModify, value);
                RaisePropertyChanged(nameof(IsModify));
            }
        }

        public ICommand GoUpdate { get; set; }
        public ICommand ResetDietCommand { get; set; }

        public DietViewModel(
            INavigationService navigationService,
            ITranslationService translationService,
            IPopupService popupService,
            ISchoolRestaurantBookingConfigurationService service,
            ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {
            MainService = service;
            GoUpdate = new AsyncCommand(async () => await Update());
            ResetDietCommand = new DelegateCommand(ResetDiet);
            IsModify = false;
        }

        public void LoadData()
        {
            //appel pour régime alimentaire
            CallApi(async () =>
            {
                ChildDietResponse response = await MainService.GetChildDietByCityContext(Child.EditId, Child.CityContext);

                ManageApiResponses(response, new DefaultCallbackManager<ChildDietResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        HasOption = response.HasOption;
                        OptionDiet = response.OptionDiet;
                        PossibleStandardDiets = response.PossibleStandardDiets;
                        StandardDiet = response.StandardDiet;
                        if (HasOption)
                        {
                            int indiceDiet = response.PossibleStandardDiets == null ? 0 : response.PossibleStandardDiets.FindIndex(sd => sd.Equals(response.StandardDiet)) + 1;
                            InitialChildDiet = new ChildDietModel()
                            {
                                OptionDiet = response.OptionDiet,
                                PossibleStandardDiets = response.PossibleStandardDiets,
                                StandardDiet = response.StandardDiet,
                                IndiceStandardDiet = indiceDiet
                            };
                            IsModify = false;
                        }
                        else
                        {
                            InitialChildDiet = new ChildDietModel();
                        }
                        IsLoading = false;
                    }
                });
            });
        }

        private async Task Update()
        {
            IsLoading = true;
            await updateDataChildDiet();
            IsLoading = false;
        }

        public async Task updateDataChildDiet()
        {
            Response response = await MainService.UpdateChildDietByCityContext(Child.EditId, StandardDiet, OptionDiet, Child.CityContext);
            ManageApiResponses(response, new DefaultCallbackManager<Response>(PopupService)
            {
                OnSuccess = (res) =>
                {
                    PopupService.Show(PopupEnum.PopupSuccess, res.Message, "OK", LoadData);
                }
            });
        }

        public void ResetDiet()
        {
            OptionDiet = InitialChildDiet.OptionDiet;
            StandardDiet = InitialChildDiet.StandardDiet;
        }

        internal void UpdateModifyStatus()
        {
            if (InitialChildDiet != null)
            {
                IsModify = OptionDiet != InitialChildDiet.OptionDiet || StandardDiet != InitialChildDiet.StandardDiet;
            }
        }
    }
}
