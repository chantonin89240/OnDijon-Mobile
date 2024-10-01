namespace OnDijon.Common.Entities.Model
{
    public class CityModel
    {
        public string Name { get; set; }
        public string CodComm { get; set; }
        public string CodePost { get; set; }
        public string ConcatName { get => string.Concat(Name, " (", CodePost, ")"); }
    }
}
