using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.Abris.Entities.Models
{
    public class AbrisModel
    {
        public string DataSetId { get; set; }
        public string RecordId { get; set; }
        public string Extensible { get; set; }
        public string Quartier { get; set; }
        public string Nom { get; set; }
        public int Aire { get; set; }
        public double GeoPointLat { get; set; }
        public double GeoPointLon { get; set; }
        public int NbPlaces { get; set; }
        public int NbPlacesInitial { get; set; }
        public string CodComm { get; set; }

    }
}
