using OnDijon.Common.Utils.Resources;
using OnDijon.Common.Services.Interfaces;

namespace OnDijon.Common.Services
{
    public class TranslationService : ITranslationService
    {
        public string GetString(string key)
        {
            return Resources.ResourceManager.GetString(key);
        }
    }
}
