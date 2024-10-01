using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using OnDijon.Modules.Services.Entities.Dto;

namespace OnDijon.Modules.Services.Entities.Models
{
    public class ServiceLayout : ServiceDto, INotifyPropertyChanged
    {
        public int Row { get; set; }
        public int Column { get; set; }

        #region bool => IsFavourite
    
        private bool _isFavourite;

        public override bool IsFavourite
        {
            get => _isFavourite;
            set => SetField(ref _isFavourite, value);
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
