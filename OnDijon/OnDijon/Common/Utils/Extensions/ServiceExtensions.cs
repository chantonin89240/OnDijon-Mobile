using OnDijon.Modules.Services.Entities.Dto;

namespace OnDijon.Common.Utils.Extensions
{
    public static class ServiceExtensions
    {
        public static string GetPageKeyByServiceCode(this ServiceDto service)
        {
            if (Constants.PageKeyByCodeService.ContainsKey(service.Code))
            {
                return Constants.PageKeyByCodeService[service.Code];
            }
            throw new System.NotSupportedException($"No page for service code : {service.Code} ({service.Title})");
        }
    }
}
