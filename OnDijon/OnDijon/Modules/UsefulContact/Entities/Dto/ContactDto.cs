namespace OnDijon.Modules.UsefulContact.Entities.Dto
{
    public class ContactDto
    {
        public string EditId { get; set; }
        public string ElementType { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public WorkInfosDto WorkInfos { get; set; }
        public ContactInfosDto ContactInfos { get; set; }
    }
}
