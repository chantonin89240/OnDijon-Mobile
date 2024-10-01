using System;

namespace OnDijon.Common.Entities
{
    public class CallbackManager<T> where T : Response.Response
    {
        public Action<T> OnSuccess { get; set; }

        public Action<T> OnInvalidCredentials { get; set; }

        public Action<T> OnInvalidInformations { get; set; }

        public Action<T> OnNotFound { get; set; }

        public Action<T> OnError { get; set; }
        public Action<T> AuthError { get; set; }
        public Action<T> ProfileNotFound { get; set; }
    }
}
