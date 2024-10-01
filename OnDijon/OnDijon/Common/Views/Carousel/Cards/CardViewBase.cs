using System;
using OnDijon.Modules.Dashboard.Entities.Dto.Card;
using Xamarin.Forms;

namespace OnDijon.Common.Views
{
    public abstract class CardViewBase : ContentView
    {

        public static readonly BindableProperty CardProperty = BindableProperty.Create(nameof(Card), typeof(CardDto), typeof(CardViewBase), propertyChanged: CardPropertyChanged);

        public delegate void ActionClickedHandler(CardActionDto action);
        public event ActionClickedHandler ActionClicked;

        public CardDto Card
        {
            get { return (CardDto)GetValue(CardProperty); }
            set { SetValue(CardProperty, value); }
        }

        protected void OnActionClicked(CardActionDto action)
        {
            ActionClicked?.Invoke(action);
        }


        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var action = ((TappedEventArgs)e).Parameter as CardActionDto;

            OnActionClicked(action);
        }

        public void Button_Clicked(object sender, System.EventArgs e)
        {
            Button button = (Button)sender;
            var action = button.CommandParameter as CardActionDto;

            OnActionClicked(action);
        }

        protected virtual void OnCardPropertyChanged(CardDto newCardDto)
        {

        }



        private static void CardPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CardViewBase)bindable;

            view.OnCardPropertyChanged((CardDto)newValue);
        }
    }
}
