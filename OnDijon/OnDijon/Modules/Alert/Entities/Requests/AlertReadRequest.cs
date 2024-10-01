using System.Collections.Generic;

namespace OnDijon.Modules.Alert.Entities.Requests
{
    public class AlertReadRequest
    {
        public string Key { get; set; }
        public string UserEditId { get; set; }
        public List<AlertsToggleDictionary> AlertsToggleDictionary { get; set; }
    }

    public class AlertsToggleDictionary
    {
        public string EditId { get; set; }
        public bool ReadStatus { get; set; }
    }

}
