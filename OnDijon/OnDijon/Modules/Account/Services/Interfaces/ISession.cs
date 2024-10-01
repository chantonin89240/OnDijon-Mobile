using OnDijon.Common.Services.Interfaces;
using OnDijon.Modules.Report.Entities.Request;
using OnDijon.Modules.Account.Entities.Models;

namespace OnDijon.Modules.Account.Services.Interfaces
{
    public interface ISession : ICleanup
    {
        ProfileModel Profile { get; set; }

        bool IsConnected();

        ReportRequest ReportRequest { get; set; }
    }
}
