
using OnDijon.Common.Utils.Resources;
using OnDijon.Common.Utils.Tools;
using OnDijon.Modules.School.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.School.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DishView : StackLayout
    {
        public static readonly BindableProperty DishProperty = BindableProperty.Create(nameof(Dish), typeof(Plat), typeof(DishView), propertyChanged: DishPropertyChanged);
        public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(string), typeof(DishView), propertyChanged: IconPropertyChanged);

        public Plat Dish
        {
            get { return (Plat)GetValue(DishProperty); }
            set { SetValue(DishProperty, value); }
        }

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public DishView()
        {
            InitializeComponent();
        }

        private static void DishPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (DishView)bindable;
            var dish = (Plat)newValue;
            if (dish != null)
            {
                view.DishName.Text = dish.Nom;
                
                var porkIcon = dish.Porc ? DMResources.SchoolRestaurantCalendar_Pork_Active_Icon : DMResources.SchoolRestaurantCalendar_Pork_Inactive_Icon;
                view.PorkIcon.IsVisible = dish.Porc;
                view.PorkIcon.Source = ImageTool.FromUri(porkIcon);

                var localIcon = dish.ProduitLocaux ? DMResources.SchoolRestaurantCalendar_Local_Active_Icon : DMResources.SchoolRestaurantCalendar_Local_Inactive_Icon;
                view.LocalIcon.IsVisible = dish.ProduitLocaux;
                view.LocalIcon.Source = ImageTool.FromUri(localIcon);

                var bioIcon = dish.Bio ? DMResources.SchoolRestaurantCalendar_Bio_Active_Icon : DMResources.SchoolRestaurantCalendar_Bio_Inactive_Icon;
                view.BioIcon.IsVisible = dish.Bio;
                view.BioIcon.Source = ImageTool.FromUri(bioIcon);

                var fairTradeIcon = dish.CommerceEquitable ? DMResources.SchoolRestaurantCalendar_FairTrade_Active_Icon : DMResources.SchoolRestaurantCalendar_FairTrade_Inactive_Icon;
                view.FairTradeIcon.IsVisible = dish.CommerceEquitable;
                view.FairTradeIcon.Source = ImageTool.FromUri(fairTradeIcon);
            }
        }

        private static void IconPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (DishView)bindable;
            view.DishIcon.Source = ImageTool.FromUri(newValue?.ToString());
        }
    }
}