namespace OnDijon.Modules.Account.Entities.Response
{
    public class AuthenticationResponse : Common.Entities.Response.Response
    {
        public string Guid { get; set; }
        public string AuthenticationUrl { get; set; }
    }
}
