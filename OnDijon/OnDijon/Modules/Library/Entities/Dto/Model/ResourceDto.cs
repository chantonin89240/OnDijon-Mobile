using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.Library.Entities.Dto.Model
{
    public class ResourceDto : WsDMDto
    {
        public string Id { get; set; }
        public string Isbn { get; set; }
        public int UId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Creator { get; set; }
        public string PublicationDate { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Availability { get; set; }
        public string OnlineLink { get; set; }
    }
}
