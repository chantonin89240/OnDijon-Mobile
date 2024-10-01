namespace OnDijon.Modules.CustomContent.Entities.Models
{
    public class CustomContentModel
    {
        public string Title { get; set; }
        public string Description{ get; set; }
        public string Image{ get; set; }
        public string Video{ get; set; }
        public string ExternalLinkTitle{ get; set; }
        public string ExternalLink{ get; set; }


        public bool HaveImageOrVideo
        {
            get
            {
                return !string.IsNullOrEmpty(Image) || !string.IsNullOrEmpty(Video);
            }
        }
    }
}
