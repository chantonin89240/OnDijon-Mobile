using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatePickerPopupView : PopupPage
    {
        private const int MIN_YEAR = 1900;

        public delegate void DateChangedDelegate(DateTime date);

        private readonly DateChangedDelegate _dateChangedDelegate;

        private DateTime selectedDate;

        public DatePickerPopupView(DateChangedDelegate @delegate, DateTime? date)
        {
            InitializeComponent();
            _dateChangedDelegate = @delegate;
            selectedDate = date ?? DateTime.Today;

            UpdateDayPicker();
            UpdateMonthPicker();
            UpdateYearPicker();
        }

        private void UpdateDayPicker()
        {
            int daysInMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);
            var days = new List<int>();
            for (int day = 1; day <= daysInMonth; day++)
            {
                days.Add(day);
            }
            DayPicker.ItemsSource = days;
            DayPicker.SelectedItem = selectedDate.Day;
        }

        private void UpdateMonthPicker()
        {
            var months = new List<string>();
            for (int month = 1; month <= 12; month++)
            {
                months.Add(DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(month));
            }
            MonthPicker.ItemsSource = months;
            MonthPicker.SelectedIndex = selectedDate.Month - 1;
        }

        private void UpdateYearPicker()
        {
            var years = new List<int>();
            for (int year = DateTime.Today.Year+1; year >= MIN_YEAR; year--)
            {
                years.Add(year);
            }
            YearPicker.ItemsSource = years;
            YearPicker.SelectedItem = selectedDate.Year;
        }

        private void DayChanged(object sender, EventArgs args)
        {
            if (DayPicker.SelectedItem != null)
            {
                int day = (int)DayPicker.SelectedItem;
                SetValidDate(selectedDate.Year, selectedDate.Month, day);
            }
        }

        private void MonthChanged(object sender, EventArgs args)
        {
            int month = MonthPicker.SelectedIndex + 1;
            SetValidDate(selectedDate.Year, month, selectedDate.Day);
            UpdateDayPicker();
        }

        private void YearChanged(object sender, EventArgs args)
        {
            int year = (int)YearPicker.SelectedItem;
            SetValidDate(year, selectedDate.Month, selectedDate.Day);
            UpdateDayPicker();
        }

        private void SetValidDate(int year, int month, int day)
        {
            //make sure day exists in month
            int daysInMonth = DateTime.DaysInMonth(year, month);
            if (day > daysInMonth)
            {
                day = daysInMonth;
            }

            selectedDate = new DateTime(year, month, day);
        }

        private async void OnConfirm(object sender, EventArgs e)
        {
            _dateChangedDelegate(selectedDate);
            await PopupNavigation.Instance.PopAsync();
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return !CloseWhenBackgroundIsClicked;
        }
    }
}