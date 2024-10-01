using System.Text.RegularExpressions;

namespace OnDijon.Common.Utils.Helpers
{
    public static class RegexHelper
    {
        public static bool CheckEmailRegex(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$");
            Match match = regex.Match(email);
            return match.Success;
        }

        public static bool CheckPasswordRegex(string password)
        {
            Regex regex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$");

            Match match = regex.Match(password);
            return match.Success;
        }

        public static bool CheckMobileNumberRegex(string mobile)
        {
            Regex regex = new Regex(@"^(?:(?:\+|00)33|0)\s*[1-9](?:[\s.-]*\d{2}){4}$");
            Match match = regex.Match(mobile);
            return match.Success;
        }

        public static bool CheckAlphabetRegex(string name)
        {
            Regex regex = new Regex("^[A-Za-zÜ-ü '-]+$");
            bool isMatch = false;
            if (!string.IsNullOrEmpty(name))
            {
                Match match = regex.Match(name);
                isMatch = match.Success;
            }
            return isMatch;
        }
    }
}