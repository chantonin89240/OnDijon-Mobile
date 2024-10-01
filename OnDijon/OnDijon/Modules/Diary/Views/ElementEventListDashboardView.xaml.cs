using OnDijon.Modules.Diary.Entities.Model;
using OnDijon.Modules.Diary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace OnDijon.DM.Diary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ElementEventListDashboardView : Frame
    {
        public static readonly BindableProperty EventProperty = BindableProperty.Create(nameof(Event), typeof(EventModel), typeof(ElementEventListDashboardView), propertyChanged: EventPropertyChanged);
        public EventModel Event
        {
            get { return (EventModel)GetValue(EventProperty); }
            set { SetValue(EventProperty, value); }
        }

        public ElementEventListDashboardView()
        {
            InitializeComponent();
            EventUpdate(Event);
        }

        private void EventUpdate(EventModel currentEvent)
        {
            if (currentEvent != null)
            {
                this.IsVisible = true;
                DateDay.Text = currentEvent.StartDate.Value.ToString("dd");
                DateMonth.Text = currentEvent.StartDate.Value.ToString("MMMM").Substring(0,3);
                DiaryEvent.Text = currentEvent.DiaryName;
                TitleEvent.Text = currentEvent.Title;
                ImageEvent.Source = currentEvent.ImageThumbnail;
            }
            else
            {
                this.IsVisible = false;
            }
        }

        private static void EventPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (ElementEventListDashboardView)bindable;
            view.EventUpdate((EventModel)newValue);
        }
    }
}