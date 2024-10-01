using System;
using OnDijon.Modules.Dashboard.Entities.Dto.Card;
using Xamarin.Forms;

namespace OnDijon.Common.Utils
{
    public class CarouselCardDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CardAlert { get; set; }
        public DataTemplate CardStandart { get; set; }
        public DataTemplate CardVertical { get; set; }
        public DataTemplate CardDouble { get; set; }
        public DataTemplate CardError { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            CardDto card = (CardDto)item;
            if (card == null)
            {
                throw new NotSupportedException("item isn't a CardDto !");
            }

            switch (card.Type.ToUpper())
            {
                case "DOUBLE":
                    return CardDouble;
                case "ALERTE":
                    return CardAlert;
                case "STANDARD":
                    return CardStandart;
                case "VERTICAL":
                    return CardVertical;
                case "ERROR":
                    return CardError;
                default:
                    throw new NotSupportedException("No DataTemplate associated with this type");
            }
        }
    }
}
