using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using OnDijon.Modules.Library.Entities.Dto.Model;

namespace OnDijon.Modules.Library.Entities.Dto
{
    public class RenewLoanInformationDto : WsDMDto
    {
        [JsonProperty("RenewLoan")]
        public RenewLoanDto RenewLoan { get; set; }
    }
}
