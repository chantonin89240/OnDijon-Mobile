namespace OnDijon.Common.Utils.Enums
{
    public enum AccountStatusCode
    {
        Success = 000,
        ApiKey = 100,
        InvalidCredentials = 200,
        InvalidGender = 301,
        InvalidName = 302,
        InvalidFirstName = 303,
        InvalidMail = 304,
        InvalidBirthday = 305,
        InvalidPassword = 306,
        InvalidPhoneNumber = 307,
        InvalidFixPhoneNumber = 308,
        InvalidCountry = 351,
        InvalidPostalCode = 352,
        InvalidCity = 353,
        InvalidStreet = 354,
        InvalidStreetNumber = 355,
        InvalidStreetNumberComplement = 356,
        InvalidAddressComplement = 357,
        ProfileNotFound = 400,
        UnknownError = 900
    }

    public static class AccountStatusCodeExtension
    {
        public static CallStatusEnum ToCallStatus(this AccountStatusCode accountStatus)
        {
            switch (accountStatus)
            {
                case AccountStatusCode.Success:
                case AccountStatusCode.ApiKey:
                    return CallStatusEnum.Success;
                case AccountStatusCode.InvalidCredentials:
                    return CallStatusEnum.InvalidCredentials;
                case AccountStatusCode.InvalidGender:
                case AccountStatusCode.InvalidName:
                case AccountStatusCode.InvalidFirstName:
                case AccountStatusCode.InvalidBirthday:
                case AccountStatusCode.InvalidPhoneNumber:
                case AccountStatusCode.InvalidFixPhoneNumber:
                case AccountStatusCode.InvalidCountry:
                case AccountStatusCode.InvalidPostalCode:
                case AccountStatusCode.InvalidCity:
                case AccountStatusCode.InvalidStreet:
                case AccountStatusCode.InvalidStreetNumber:
                case AccountStatusCode.InvalidStreetNumberComplement:
                case AccountStatusCode.InvalidAddressComplement:
                case AccountStatusCode.ProfileNotFound:
                    return CallStatusEnum.InvalidInformations;
                case AccountStatusCode.UnknownError:
                    return CallStatusEnum.InternalServerError;
                case AccountStatusCode.InvalidMail:
                    return CallStatusEnum.InvalidMail;
                case AccountStatusCode.InvalidPassword:
                    return CallStatusEnum.InvalidPassword;
                default:
                    return CallStatusEnum.UnknownError;
            }
        }
    }
}
