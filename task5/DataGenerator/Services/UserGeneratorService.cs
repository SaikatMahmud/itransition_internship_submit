using Bogus;
using DataGenerator.Models;
using System;

namespace DataGenerator.Services
{
    public static class UserGeneratorService
    {
        private static char[] _alphabet;
        private static Random _random;

        public static char[] ReadAlphabetFile(string region)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "alphabet", $"{region}.txt");
            string fileContent = File.ReadAllText(filePath);
            return fileContent.ToCharArray();
        }

        public static List<User> GetUsers(int page, string region, double error, int seed)
        {
            int recordsPerPage = 20;
            if (page != 1)
            {
                recordsPerPage = 10;
            }
            var userFaker = new Faker<User>(region)
                .RuleFor(u => u.UserId, f => f.Random.Guid())
                .RuleFor(u => u.Name, f => f.Person.FullName)
                .RuleFor(u => u.Address, f => f.Address.Country() + ", " + f.Address.City() + ", " + f.Address.StreetAddress())
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumberFormat());

            User GenerateFakeUser(int seedValue)
            {
                return userFaker.UseSeed(seedValue).Generate();
            }
            var users = Enumerable.Range(1, recordsPerPage)
            .Select(i => GenerateFakeUser(seed + (page - 1) * recordsPerPage + i))
            .ToList();

            if (error > 0)
            {
                _alphabet = ReadAlphabetFile(region);
                var errorSeed = (seed + page * recordsPerPage) + (int)error;
                users = MakeError(users, error, errorSeed);
            }
            return users;
        }
        private static List<User> MakeError(List<User> users, double errorCount, int seed)
        {
            _random = new Random(seed);

            foreach (var user in users)
            {
                int nameLength = user.Name.Length;
                int addressLength = user.Address.Length;

                for (int i = 0; i < Math.Floor(errorCount); i++) //times function
                {
                    if (_random.Next(0, 2) == 0) user.Name = GenerateError(user.Name, nameLength); //randomly select a property
                    else user.Address = GenerateError(user.Address, addressLength);
                }
                if (_random.NextDouble() < errorCount % 1)
                {
                    if (_random.Next(0, 2) == 0) user.Name = GenerateError(user.Name, nameLength);
                    else user.Address = GenerateError(user.Address, addressLength);
                }
            }
            return users;
        }

        private static string GenerateError(string data, int dataLength)
        {
            int maxLength = (int)(dataLength * 1.5);
            int minLength = dataLength / 2;
            var errorFunction = GetRandomErrorFunction();
            data = errorFunction(data);
            if (data.Length > maxLength)
            {
                data = data.Substring(0, maxLength);
            }
            else if (data.Length < minLength)
            {
                data += new string(_alphabet[_random.Next(_alphabet.Length)], minLength - data.Length);
            }
            return data;
        }
        private static Func<string, string> GetRandomErrorFunction()
        {
            Func<string, string>[] methods = { DeleteCharacter, AddCharacter, SwapCharacters };
            return methods[_random.Next(methods.Length)];
        }

        private static string DeleteCharacter(string originalValue)
        {
            if (originalValue.Length == 0) return originalValue;
            var position = _random.Next(originalValue.Length);
            return originalValue.Substring(0, position) + originalValue.Substring(position + 1);
        }

        private static string AddCharacter(string originalValue)
        {
            var position = _random.Next(originalValue.Length + 1);
            var character = _alphabet[_random.Next(_alphabet.Length)];
            return originalValue.Substring(0, position) + character + originalValue.Substring(position);
        }

        private static string SwapCharacters(string originalValue)
        {
            if (originalValue.Length < 2) return originalValue;

            var position = _random.Next(originalValue.Length - 1);
            var char1 = originalValue[position];
            var char2 = originalValue[position + 1];
            return originalValue.Substring(0, position) + char2 + char1 + originalValue.Substring(position + 2);
        }

        /*       private static Func<string, string> Times(double times, Func<string, string> function)
               {
                   return (inputString) =>
                   {
                       for (int i = 0; i < Math.Floor(times); i++)
                       {
                           inputString = function(inputString);
                       }
                       if (_random.NextDouble() < times % 1)
                       {
                           inputString = function(inputString);
                       }
                       return inputString;
                   };
               }*/
    }
}
