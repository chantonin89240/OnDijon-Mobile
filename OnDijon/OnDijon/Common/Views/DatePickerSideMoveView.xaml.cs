using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatePickerSideMoveView : StackLayout
    {

        public static readonly BindableProperty DateProperty = BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(DatePickerSideMoveView), defaultBindingMode: BindingMode.TwoWay, propertyChanged: DatePropertyChanged);
        public static readonly BindableProperty MinDateProperty = BindableProperty.Create(nameof(MinDate), typeof(DateTime), typeof(DatePickerSideMoveView), propertyChanged: MinDatePropertyChanged);
        public static readonly BindableProperty MaxDateProperty = BindableProperty.Create(nameof(MaxDate), typeof(DateTime), typeof(DatePickerSideMoveView), propertyChanged: MaxDatePropertyChanged);
        public static readonly BindableProperty IgnoreWeekendProperty = BindableProperty.Create(nameof(MaxDate), typeof(bool), typeof(DatePickerSideMoveView), true, propertyChanged: MaxDatePropertyChanged);

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value);}
        }
        public DateTime MinDate
        {
            get { return (DateTime)GetValue(MinDateProperty); }
            set { SetValue(MinDateProperty, value); }
        }
        public DateTime MaxDate
        {
            get { return (DateTime)GetValue(MaxDateProperty); }
            set { SetValue(MaxDateProperty, value); }
        }
        public bool IgnoreWeekend
        {
            get { return (bool)GetValue(IgnoreWeekendProperty); }
            set { SetValue(IgnoreWeekendProperty, value); }
        }

        public event EventHandler<DateChangedEventArgs> DateSelected;

        public DatePickerSideMoveView()
        {
            InitializeComponent();
        }

        private static void DatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (DatePickerSideMoveView)bindable;
            view.MainDatePicker.Date = (DateTime)newValue;
        }
        private static void MinDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (DatePickerSideMoveView)bindable;
            view.MainDatePicker.MinimumDate = (DateTime)newValue;
        }
        private static void MaxDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (DatePickerSideMoveView)bindable;
            view.MainDatePicker.MaximumDate = (DateTime)newValue;
        }

        private void MainDatePicker_PropertyChanged(object sender, EventArgs e)
        {
            OnDateChanged(MainDatePicker.Date);
        }

        private void Left_Tapped(object sender, EventArgs e)
        {
            int dayAdded = IgnoreWeekend && Date.AddDays(-1).DayOfWeek == DayOfWeek.Sunday ? -3 : -1;
            if (MinDate <= Date.AddDays(dayAdded))
            {
                OnDateChanged(Date.AddDays(dayAdded));
            }
        }

        private void Right_Tapped(object sender, EventArgs e)
        {
            int dayAdded = IgnoreWeekend && Date.AddDays(1).DayOfWeek == DayOfWeek.Saturday ? 3 : 1;
            if (MaxDate >= Date.AddDays(dayAdded))
            {
                OnDateChanged(Date.AddDays(dayAdded));
            }
        }


        private void OnDateChanged(DateTime newDate)
        {
            var oldDate = Date;
            Date = newDate;
            DateSelected?.Invoke(this, new DateChangedEventArgs(oldDate, newDate));

            DatePickerRightButton.Opacity = MaxDate <= Date ? 0.5 : 1;
            DatePickerLeftButton.Opacity = MinDate >= Date ? 0.5 : 1;
        }
    }
}