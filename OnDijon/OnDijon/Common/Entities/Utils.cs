using Newtonsoft.Json;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using static OnDijon.Common.Entities.Status;

namespace OnDijon.Common.Entities
{

    public class DefaultCallbackManager<T> : CallbackManager<T>  where T : Common.Entities.Response.Response
    {
         
        public DefaultCallbackManager(IPopupService popupService)
        {
            OnSuccess = (res) =>
            {
                popupService.Show(PopupEnum.PopupSuccess, "Opération réalisée avec succès", res.Message, "Ok");

            };
            OnInvalidInformations = (res) =>
            {
                //popupService.Show(PopupEnum.PopupError, "Attention", res.Message, "Retour");

            };
            OnNotFound = (res) =>
            {
                popupService.Show(PopupEnum.PopupError, "Un problème est survenu", res.Message, "Retour");

            };
        }
    }
    public class Status
    {
        public class Code
        {
            public const string Success = "000";
            public const string Error = "900";
            public const string AccessKey = "100";
            public const string AuthCheck = "200";
            public const string GenderField = "301";
            public const string NameField = "302";
            public const string FirstNameField = "303";
            public const string MailField = "304";
            public const string BirthdayField = "305";
            public const string PasswordField = "306";
            public const string ErrorEditId = "307";
            public const string ProfileNotFound = "400";
            public const string AuthError = "511";
            public const string ExternalError = "520";
        }

        public static IDictionary<string, string> Message = new Dictionary<string, string>()
            {
                { Code.Success,"Opération réalisée avec succès"},
                { Code.AccessKey,"La clé d'authentifation n'est pas valide"},
                { Code.AuthCheck,"Authentification réalisée avec succès"},
                { Code.AuthError, "L'authentification à échouée" },
                { Code.ExternalError, "Erreur du serveur externe"},
                { Code.ErrorEditId, "L'editId ne retourne aucun compte"},
                { Code.Error, "Erreur"},
                { Code.GenderField, ""},
                { Code.NameField, ""},
                { Code.FirstNameField, ""},
                { Code.MailField, "Il existe déjà un compte avec cette adresse mail"},
                { Code.BirthdayField, ""},
                { Code.PasswordField, "Le mot de passe choisi est trop court, il doit contenir au moins 8 caractères dont au moins une majuscule, une minuscule et un chiffre"},
                { Code.ProfileNotFound, ""},
        };
    }

    public class Utils
    {

        public static A Translate<A, B>(B source) where A : Response.Response, new() where B : WsDMDto
        {
            A response = new A();
            if (source == null || source.StatusCodes == null)
            {

                response.State = CallStatusEnum.ServiceUnavailable;
                response.Message = "L'appel aux serveurs onDijon n'a pu aboutir.";
            }
            else
            {
                var code = source.StatusCodes.FirstOrDefault();
                if (code != null)
                {
                    string strMessage = string.Empty;
                    if (source.StatusMessages != null)
                    {
                        var message = source.StatusMessages.FirstOrDefault(m => m.Key == code);
                        if (message == null)
                        {
                            if (Message.ContainsKey(code))
                            {
                                strMessage = Message[code];
                            }
                            else
                            {
                                strMessage = code == Code.Success ? "Opération réalisée avec succès" : "Erreur indisponible";
                            }
                        }
                        else
                        {
                            strMessage = message.Value;
                        }

                    }
                    response.State = ToCallStatus(code);
                    response.Message = strMessage;

                }
                else
                {
                    response.State = CallStatusEnum.UnknownError;
                    response.Message = "Erreur indisponible";
                }

            }
            return response;
        }



        private static CallStatusEnum ToCallStatus(string code)
        {
            switch (code)
            {
                case Code.Success:
                    return CallStatusEnum.Success;
                case Code.AccessKey:
                case Code.AuthCheck:
                    return CallStatusEnum.InvalidCredentials;
                case Code.ExternalError:
                    return CallStatusEnum.ServiceUnavailable;
                case Code.Error:
                    return CallStatusEnum.InvalidInformations;
                case Code.MailField:
                    return CallStatusEnum.InvalidMail;
                case Code.PasswordField:
                    return CallStatusEnum.InvalidPassword;
                case Code.AuthError:
                    return CallStatusEnum.AuthError;
                case Code.ProfileNotFound:
                    return CallStatusEnum.ProfileNotFound;
                default:
                    return CallStatusEnum.UnknownError;
            }
        }


        public static List<StatusMessage> GetMessage(List<string> codes)
        {
            List<StatusMessage> statusMessage = new List<StatusMessage>();
            if (codes != null)
            {
                codes.ForEach(c =>
               {
                   statusMessage.Add(new StatusMessage() { Key = c, Value = Message.ContainsKey(c) ? Message[Code.Success] : "StatusCode non reconnu" });
               });
            }
            else
            {
                statusMessage.Add(new StatusMessage() { Key = "Error", Value = "StatusCode absent" });
            }
            return statusMessage;
        }


        public class CustomBoolConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(((bool)value) ? 1 : 0);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return reader.Value.ToString() == "1";
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(bool);
            }
        }
    }
}
