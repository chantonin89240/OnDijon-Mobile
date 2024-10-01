using OnDijon.Modules.School.Models;
using OnDijon.Common.Utils.Resources;
using OnDijon.Common.Utils.Tools;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace OnDijon.Common.Utils.Converters
{
    public class SchoolRestaurantBioImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as Plat).Bio
            ? ImageTool.FromUri(DMResources.SchoolRestaurantCalendar_Bio_Active_Icon)
            : ImageTool.FromUri(DMResources.SchoolRestaurantCalendar_Bio_Inactive_Icon);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class SchoolRestaurantLocalImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as Plat).ProduitLocaux
            ? ImageTool.FromUri(DMResources.SchoolRestaurantCalendar_Local_Active_Icon)
            : ImageTool.FromUri(DMResources.SchoolRestaurantCalendar_Local_Inactive_Icon);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class SchoolRestaurantPorkImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as Plat).Porc
            ? ImageTool.FromUri(DMResources.SchoolRestaurantCalendar_Pork_Active_Icon)
            : ImageTool.FromUri(DMResources.SchoolRestaurantCalendar_Pork_Inactive_Icon);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SchoolRestaurantFairTradeImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as Plat).CommerceEquitable
            ? ImageTool.FromUri(DMResources.SchoolRestaurantCalendar_FairTrade_Active_Icon)
            : ImageTool.FromUri(DMResources.SchoolRestaurantCalendar_FairTrade_Inactive_Icon);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ImageFromResourcesUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ImageTool.FromUri((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
