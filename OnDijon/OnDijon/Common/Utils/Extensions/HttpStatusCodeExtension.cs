using OnDijon.Common.Utils.Enums;
using System.Net;

namespace OnDijon.Common.Utils.Extensions
{
    public static class HttpStatusCodeExtension
    {
        public static CallStatusEnum ToCallStatus(this HttpStatusCode httpStatus)
        {
            switch (httpStatus)
            {
                case HttpStatusCode.OK:
                    return CallStatusEnum.Success;
                case HttpStatusCode.BadRequest:
                    return CallStatusEnum.InvalidInformations;
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    return CallStatusEnum.InvalidCredentials;
                case HttpStatusCode.NotFound:
                    return CallStatusEnum.NotFound;
                case HttpStatusCode.InternalServerError:
                    return CallStatusEnum.InternalServerError;
                case HttpStatusCode.ServiceUnavailable:
                    return CallStatusEnum.ServiceUnavailable;
                default:
                    return CallStatusEnum.UnknownError;
            }
        }
    }
}
