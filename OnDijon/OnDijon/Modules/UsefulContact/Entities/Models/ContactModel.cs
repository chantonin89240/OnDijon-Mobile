using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace OnDijon.Modules.UsefulContact.Entities.Models
{
    public class ContactModel
    {
        public string EditId { get; set; }
        public string ElementType { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public WorkInfosModel WorkInfos { get; set; }
        public ContactInfosModel ContactInfos { get; set; }

        public bool HasOpenInfo
        {
            get
            {
                return ElementType != "infosTravaux" && ContactInfos.OpeningTime != null && ContactInfos.OpeningTime.Any();
            }
        }

        public bool IsOpen
        {
            get
            {
                if (ElementType != "infosTravaux" && ContactInfos.OpeningTime.Any())
                {
                    return ContactInfos.OpeningTime.Where(op => (DateTime.Compare(op.Day, DateTime.Today) == 0) &&
                                ((op.BeginPeriod / 60 < DateTime.Now.Hour) || ((op.BeginPeriod / 60 == DateTime.Now.Hour) && (op.BeginPeriod % 60 < DateTime.Now.Minute))) &&
                                ((op.EndPeriod / 60 > DateTime.Now.Hour) || ((op.EndPeriod / 60 == DateTime.Now.Hour) && (op.EndPeriod % 60 > DateTime.Now.Minute)))
                            ).Any();
                }
                else
                {
                    return true;
                }
            }
        }

        public string IsOpenString
        {
            get
            {
                if (ElementType == "infosTravaux" || !ContactInfos.OpeningTime.Any())
                {
                    return "";
                }
                return IsOpen ? "Ouvert" : "Fermé";
            }
        }

        public Style IsOpenStyle
        {
            get
            {
                if (IsOpen)
                {
                    return Application.Current.Resources["ciFrameLabelGreen"] as Style;
                }
                else
                {
                    return Application.Current.Resources["ciFrameLabelRed"] as Style;
                }
            }
        }

        public string NextOpenHour
        {
            get
            {
                if (!IsOpen)
                {
                    ContactOpeningPeriodModel nextHour = ContactInfos.OpeningTime.Where(op => DateTime.Now < op.Day.AddMinutes(op.BeginPeriod)).FirstOrDefault();
                    if (nextHour != null)
                    {
                        CultureInfo provider = new CultureInfo("fr-FR");
                        return "Prochaine ouverture : " + provider.DateTimeFormat.GetDayName(nextHour.Day.DayOfWeek) + " à " + nextHour.BeginPeriod / 60 + ":" + (nextHour.BeginPeriod % 60).ToString("00");
                    }
                }
                return "";
            }
        }
    }
}
