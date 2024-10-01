
using System;
using OnDijon.Common.Views;
using OnDijon.Modules.School.Entities.Models;
using OnDijon.Modules.School.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.School.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchoolHomePage : BaseView
    {
        public SchoolHomePage()
        {
            InitializeComponent();
        }
    }


    public class CardsDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ChildTemplate { get; set; }
        public DataTemplate RestaurantTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((ChildCardModel)item).Type == SchoolCardType.Restaurant ? RestaurantTemplate : ChildTemplate;
        }
    }
}