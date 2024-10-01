using OnDijon.Common.Views;
using OnDijon.Modules.Simulator.Entities.Models;
using OnDijon.Modules.Simulator.ViewsModels;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Simulator.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimulatorRatePage : BasePage<SimulatorRateViewModel>
    {
        public SimulatorRatePage()
        {
            InitializeComponent();
        }
    }
}