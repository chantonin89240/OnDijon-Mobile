using NUnit.Framework;
using OnDijon.CG.Utils.Helpers;

namespace OnDijon.UnitTest.CG.Utils.Tests
{
    public class RegexTests
    {
        [TestCase("username@domain.com", ExpectedResult = true)]
        [TestCase("user.name@domain.com", ExpectedResult = true)]
        [TestCase("user-name@domain.com", ExpectedResult = true)]
        [TestCase("user.name@domain.fr", ExpectedResult = true)]
        [TestCase("user.name@do-main.fr", ExpectedResult = true)]
        [TestCase("user.name@domain.f", ExpectedResult = false)]
        [TestCase("user.name@domain", ExpectedResult = false)]
        [TestCase("user.name@.domain.com", ExpectedResult = false)]
        [TestCase("user.name-domain.com", ExpectedResult = false)]
        public bool EmailRegexTest(string email)
        {
            return RegexHelper.CheckEmailRegex(email);
        }

        // Password must be at least 8 characters long
        // and must contain at least 1 lowercase, 1 uppercase and 1 digit
        // and must NOT contain any special character
        // (SFD APPF_03_RG_01)
        [TestCase("Azerty12", ExpectedResult = true)]
        [TestCase("12345678", ExpectedResult = false)]
        [TestCase("azertyui", ExpectedResult = false)]
        [TestCase("AZERTYUI", ExpectedResult = false)]
        [TestCase("azerty12", ExpectedResult = false)]
        [TestCase("AZERTY12", ExpectedResult = false)]
        [TestCase("Azerty1", ExpectedResult = false)]
        [TestCase("azerty.12", ExpectedResult = false)]
        [TestCase("azerty-12", ExpectedResult = false)]
        [TestCase("@zerty12", ExpectedResult = false)]
        public bool PasswordRegexTest(string password)
        {
            return RegexHelper.CheckPasswordRegex(password);
        }

        [TestCase("0123456789", ExpectedResult = true)]
        [TestCase("01 23 45 67 89", ExpectedResult = true)]
        [TestCase("+33 1 23 45 67 89", ExpectedResult = true)]
        [TestCase("abcd123456", ExpectedResult = false)]
        [TestCase("ABCD123456", ExpectedResult = false)]
        [TestCase("01234567899", ExpectedResult = false)]
        public bool PhoneNumberRegexTest(string phone)
        {
            return RegexHelper.CheckMobileNumberRegex(phone);
        }

        [TestCase("Jean", ExpectedResult = true)]
        [TestCase("Jean-Pierre", ExpectedResult = true)]
        [TestCase("Léo", ExpectedResult = true)]
        [TestCase("DUPONT", ExpectedResult = true)]
        [TestCase("LE GOFF", ExpectedResult = true)]
        [TestCase("Le Cléac'h", ExpectedResult = true)]
        [TestCase("Kevin92", ExpectedResult = false)]
        public bool NameRegexTest(string name)
        {
            return RegexHelper.CheckAlphabetRegex(name);
        }
    }
}