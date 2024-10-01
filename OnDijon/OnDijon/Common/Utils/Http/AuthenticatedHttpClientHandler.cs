using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace OnDijon.Common.Utils.Http
{
    public class AuthenticatedHttpClientHandler : HttpClientHandler
    {
        public AuthenticatedHttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = ValidateServerCertificate;
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //string publicKey = string.Empty;
            //string thumbprint = string.Empty;
            //string cn = certificate.Subject.Split(',').First(s => s.Contains("CN="))?.Replace("CN=", null);

            //if (!string.IsNullOrEmpty(cn) && Constants.CertificatePublicKey.ContainsKey(cn))
            //{
            //    publicKey = Constants.CertificatePublicKey[cn].Replace(" ", null).ToUpper(CultureInfo.InvariantCulture);
            //    thumbprint = Constants.CertificateThumbrint[cn];

            //    //X509Certificate2 highestCACertificate = chain.ChainElements[chain.ChainElements.Count - 1].Certificate;
            //    return publicKey == certificate?.GetPublicKeyString() /*highestCACertificate.Verify() && thumbprint == highestCACertificate.Thumbprint &&*/;
            //}

            return true;
        }
    }
}
