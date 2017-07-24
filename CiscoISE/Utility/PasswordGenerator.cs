using System;
using System.Linq;

namespace CiscoISE.Utility
{
    public class PasswordGenerator
    {
        private static readonly Random randomGenerator = new Random();
        private static readonly object syncLock = new object();

        private const string UpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowerCase = "abcdefghijklmnopqrstuvwxyz";
        private const string Numeric = "0123456789";
        private const string Special = "!@#$^&*()[]\\,.-+_=`~;:'\"";
        private const string AllChars = UpperCase + LowerCase + Numeric + Special;

        private static string pickSome(int count, string fromString)
        {
            var result = "";

            if (count == 0)
                return result;

            lock (syncLock) {
                while ((count--) > 0)
                {
                    result += fromString[randomGenerator.Next(0, fromString.Length - 1)];
                }
            }

            return result;
        }

        public static string GenerateStrongGuestPassword()
        {
            // TODO : Get password requirements from Cisco ISE directly

            int minimumCharacters = 4;
            int minimumLowerCase = 0;
            int minimumUpperCase = 0;
            int minimumNumeric = 4;
            int minimumSpecial = 0;

            int desiredLength = Math.Max(16, minimumCharacters);

            var tempPassword =
                pickSome(minimumLowerCase, UpperCase) +
                pickSome(minimumUpperCase, LowerCase) +
                pickSome(minimumNumeric, Numeric) +
                pickSome(minimumSpecial, Special);

            var needed = desiredLength - tempPassword.Length;
            if(needed > 0)
                tempPassword += pickSome(needed, AllChars);

            var passwordLength = tempPassword.Length;

            // TODO : Choose better scramble mechanism
            var scramble = tempPassword.ToArray();
            for (var i=0; i<desiredLength*4; i++)
            {
                var posA = randomGenerator.Next(0, passwordLength - 1);
                var posB = randomGenerator.Next(0, passwordLength - 1);
                var hold= scramble[posA];
                scramble[posA] = scramble[posB];
                scramble[posB] = hold;
            }
            var result = new string(scramble);

            return result;
        }
    }
}
