using OnDijon.Common.ViewModels;
using OnDijon.Modules.Library.Entities.Dto.Model;
using OnDijon.Modules.Library.Entities.Model;
using Prism.Mvvm;

namespace OnDijon.Modules.Library.ViewModels
{
    public class ReservationViewModel : BindableObjectBase
    {
        public ReservationDto Reservation { get; set; }

        public ReservationViewModel(ReservationDto reservation)
        {
            Reservation = reservation;
            string type = Reservation.TypeOfDocument.ToString();
            ImageUrl = DataReference.UrlIconTypeOfDocument.ContainsKey(type) && !string.IsNullOrEmpty(DataReference.UrlIconTypeOfDocument[type]) ? DataReference.UrlIconTypeOfDocument[type] : string.Empty;
            WhenCreatedDescription = "Réservation fait le " + Reservation.WhenCreated.ToString("dd MMMM yyyy");
            AvailableDescription = "Disponible du " + Reservation.WhenAvailableStart.ToString("dd MMMM yyyy") + " au " + Reservation.WhenAvailableEnd.ToString("dd MMMM yyyy");
        }

        private string _imageUrl;
        public string ImageUrl { get => _imageUrl; set => Set(ref _imageUrl, value); }
        public string WhenCreatedDescription { get; set; }
        public string AvailableDescription { get; set; }
    }
}
