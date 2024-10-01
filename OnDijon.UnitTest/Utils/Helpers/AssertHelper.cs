using NUnit.Framework;

namespace OnDijon.UnitTest.Utils.Helpers
{
    public static class AssertHelper
    {
        public static void IsNotNullAndNotEmpty(string value)
        {
            Assert.IsNotNull(value);
            Assert.IsNotEmpty(value);
        }
    }
}
