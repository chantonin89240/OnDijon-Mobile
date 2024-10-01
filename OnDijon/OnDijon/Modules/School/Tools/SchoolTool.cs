using OnDijon.Modules.School.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.School.Tools
{
    public static class SchoolTool
    {
        public static List<AppointmentRuleModel> GetAppointmentRules()
        {
            List<AppointmentRuleModel> AppointmentRules = new List<AppointmentRuleModel>
            {
                //Règles Dijon
                new AppointmentRuleModel() { AppointmentId = "S111", OppositeAppointmentid = "RS", OppositeValue = false, ClosingReason = "Votre enfant ne peut pas être inscrit en \"12 h 30 ou 13 h 15\" s'il est inscrit en Restauration scolaire" },
                new AppointmentRuleModel() { AppointmentId = "RS", OppositeAppointmentid = "S111", OppositeValue = false, ClosingReason = "Votre enfant ne peut pas être inscrit en Restauration scolaire s'il est inscrit en \"12 h 30 ou 13 h 15\"" },
                new AppointmentRuleModel() { AppointmentId = "S555", OppositeAppointmentid = "S777", OppositeValue = false, ClosingReason = "Votre enfant est encore inscrit en périscolaire soir" },
                new AppointmentRuleModel() { AppointmentId = "S777", OppositeAppointmentid = "S555", OppositeValue = true, ClosingReason = "Votre enfant doit être inscrit au tap pour s'inscrire en périscolaire soir" },
                new AppointmentRuleModel() { AppointmentId = "S112", OppositeAppointmentid = "S113", OppositeValue = false, ClosingReason = "Votre enfant ne peut pas être inscrit \"11 h 50 - 12 h 30\" s'il est inscrit en \"13 h 15 - 13 h 40\"" },
                new AppointmentRuleModel() { AppointmentId = "RS", OppositeAppointmentid = "S113", OppositeValue = false, ClosingReason = "Votre enfant ne peut pas être inscrit en Restauration scolaire s'il est inscrit en \"13 h 15 - 13 h 40\"" },
                new AppointmentRuleModel() { AppointmentId = "RS", OppositeAppointmentid = "S112", OppositeValue = false, ClosingReason = "Votre enfant ne peut pas être inscrit en Restauration scolaire s'il est inscrit en \"11 h 50 - 12 h 30\"" },
                new AppointmentRuleModel() { AppointmentId = "S113", OppositeAppointmentid = "S112", OppositeValue = false, ClosingReason = "Votre enfant ne peut pas être inscrit \"13 h 15 - 13 h 40\" s'il est inscrit en \"11 h 50 - 12 h 30\"" },
                new AppointmentRuleModel() { AppointmentId = "S112", OppositeAppointmentid = "RS", OppositeValue = false, ClosingReason = "Votre enfant ne peut pas être inscrit \"11 h 50 - 12 h 30\" s'il est inscrit en Restauration scolaire" },
                new AppointmentRuleModel() { AppointmentId = "S113", OppositeAppointmentid = "RS", OppositeValue = false, ClosingReason = "Votre enfant ne peut pas être inscrit \"13 h 15 - 13 h 40\" s'il est inscrit en Restauration scolaire" },
            };
            return AppointmentRules;
        }
    }
}
