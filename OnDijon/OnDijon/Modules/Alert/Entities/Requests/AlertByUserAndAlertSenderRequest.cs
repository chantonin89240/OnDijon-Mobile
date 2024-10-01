namespace OnDijon.Modules.Alert.Entities.Requests
{
    public class AlertByUserAndAlertSenderRequest
    {
        public string Key { get; set; }
        public string UserEditId { get; set; }
        public string SenderAlertEditId { get; set; }
    }
}
