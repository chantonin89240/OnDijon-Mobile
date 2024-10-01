using OnDijon.Modules.Library.Entities.Dto.Model;
using OnDijon.Modules.Library.Entities.Model;
using System;
using System.Runtime.CompilerServices;
using OnDijon.Common.ViewModels;
using Prism.Mvvm;

namespace OnDijon.Modules.Library.ViewModels
{
    public class LoanViewModel : BindableObjectBase
    {
        private LoanDto _loan;
        public LoanDto Loan { get => _loan; set { _loan = value; Update(); } }

        private void Update()
        {
        
            string type = Loan.TypeOfDocument.ToString();
            if (string.IsNullOrEmpty(ImageUrl))
                ImageUrl = DataReference.UrlIconTypeOfDocument.ContainsKey(type) && !string.IsNullOrEmpty(DataReference.UrlIconTypeOfDocument[type]) ? DataReference.UrlIconTypeOfDocument[type] : string.Empty;
            DateDescription = "Jusqu'au " + Loan.ReturnDate.ToString("dd MMMM yyyy")
                + " - " + (Loan.IsLate ? ((DateTime.Today - Loan.ReturnDate).Days + " jours de retard") : ("reste " + (Loan.ReturnDate - DateTime.Today).Days + " jours"));
            IsCurrentLoan = Loan.ReturnDate > DateTime.Today;
            Location = Loan.Location;
            Publisher = Loan.Publisher;
            IsLate = Loan.IsLate;
            IsSoonLate = Loan.IsSoonLate;
            Title = Loan.Title;
            Author = Loan.Author;
            CanRenew = Loan.CanRenew;
        }


        private string _imageUrl;
        public string ImageUrl { get => _imageUrl; set => Set(ref _imageUrl, value); }

        private string _dateDescription;
        public string DateDescription { get => _dateDescription; set => Set(ref _dateDescription, value); }

        public bool _isCurrentLoan;
        public bool IsCurrentLoan { get => _isCurrentLoan; set => Set(ref _isCurrentLoan, value); }


        private string _location;
        public string Location { get => _location; set => Set(ref _location, value); }

        private string _publisher;
        public string Publisher { get => _publisher; set => Set(ref _publisher, value); }

        private bool _isLate;
        public bool IsLate { get => _isLate; set => Set(ref _isLate, value); }


        private bool _isSoonLate;
        public bool IsSoonLate { get => _isSoonLate; set => Set(ref _isSoonLate, value); }

        private string _title;
        public string Title { get => _title; set => Set(ref _title, value); }

        private string _author;
        public string Author { get => _author; set => Set(ref _author, value); }

        private bool _canRenew;
        public bool CanRenew { get => _canRenew; set => Set(ref _canRenew, value); }
        
       

    }
}
