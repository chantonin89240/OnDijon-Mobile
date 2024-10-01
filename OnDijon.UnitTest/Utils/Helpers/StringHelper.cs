using System;
using System.Linq;

namespace OnDijon.UnitTest.Utils.Helpers
{
    public static class StringHelper
    {
        private static readonly Random random = new Random();

        public static string RandomString(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
