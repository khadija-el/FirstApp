using System;
using System.Linq;

namespace FistApi.Models.seed
{
    public static class BogusGenerator
    {
        public static string Email(int numberOfCharacters = 5) // pass the number of characters for your email to be generated before '@'
        {
            var characters = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            var random = new Random();
            var EmailAddress = new string(Enumerable.Repeat(characters, numberOfCharacters).Select(s => s[random.Next(s.Length)]).ToArray());
            EmailAddress = EmailAddress + "@angular.io";
            return EmailAddress;
        }
        public static string Tel(int numbers = 8)
        {
            var random = new Random();
            var possibilities = Enumerable.Range(1, 100).ToList();
            var result = possibilities.OrderBy(number => random.Next()).Take(numbers).ToArray();
            return String.Join("06", result);
        }
        public static string CreditCardNumber(int numbers)
        {
            string CreditCardNumber = null;
            int nums = numbers;
            Random num = new Random();
            for (int i = 0; i < nums; i++)
            {
                CreditCardNumber = num.Next(9) + CreditCardNumber;
            }

            return CreditCardNumber;
        }

        public static int Number(int min = 1, int max = 4)
        {
            return new Random().Next(min, max);
        }

        public static string Name(int numberOfCharacters = 10)
        {
            string TestNames = null;
            var characters = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz_0123456789";
            var random = new Random();
            char[] value = Enumerable.Repeat(characters, numberOfCharacters)
                .Select(s => s[random.Next(s.Length)])
                .ToArray();
            TestNames = new string(value);
            return TestNames;
        }


    }
}