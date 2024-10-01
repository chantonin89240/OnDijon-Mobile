using OnDijon.Modules.CustomContent.Entities.Models;

namespace OnDijon.Modules.CustomContent.Entities
{
    public class CustomContentResponse : Common.Entities.Response.Response
    {
        public CustomContentModel CustomContent { get; set; }
    }
}
