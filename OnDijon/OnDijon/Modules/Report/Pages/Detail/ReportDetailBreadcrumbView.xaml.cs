
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Report.Pages.Detail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportDetailBreadcrumbView : Grid
    {
        public static readonly BindableProperty DateProperty = BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(ReportDetailBreadcrumbView), propertyChanged: DatePropertyChanged);
        public static readonly BindableProperty IsLastElementProperty = BindableProperty.Create(nameof(IsLastElement), typeof(bool), typeof(ReportDetailBreadcrumbView), propertyChanged: IsLastElementPropertyChanged);

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public bool IsLastElement
        {
            get { return (bool)GetValue(IsLastElementProperty); }
            set { SetValue(IsLastElementProperty, value); }
        }



        public ReportDetailBreadcrumbView()
        {
            InitializeComponent();
        }

        
        private static void DatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (ReportDetailBreadcrumbView)bindable;

            view.Date = (DateTime)newValue;
            // Because binding doesn't work 
            view.DateLabel.Text = view.Date.ToString("dd/MM/yyyy");
        }


        private static void IsLastElementPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (ReportDetailBreadcrumbView)bindable;

            view.IsLastElement = (bool)newValue;
            view.HorizontalSeparator.IsVisible = !view.IsLastElement;
        }

    }
}