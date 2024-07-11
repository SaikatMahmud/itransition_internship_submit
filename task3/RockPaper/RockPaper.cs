using Org.BouncyCastle.Crypto.Digests;
using System.Globalization;
using System.Security.AccessControl;
using System.Security.Cryptography;

namespace RockPaper
{
    public class RockPaper
    {
        public static void StartGame(string[] arr, int arrLen)
        {
            var table = new Table(arr);
            var random = new Random();
            var computerMove = random.Next(arrLen);
            var hmac = new Hash();
            var HMACKey = hmac.GetKey();
            string HMACString = hmac.GetHMAC(arr[computerMove], HMACKey);

            while (true)
            {
                Console.WriteLine($"HMAC: {HMACString}");
                Console.WriteLine("Available moves:");
                for (int i = 1; i <= arrLen; i++)
                {
                    Console.WriteLine($"{i} - {arr[i - 1]}");
                }
                Console.WriteLine("0 - exit \n? - help");

                Console.Write("Enter your move: ");
                var userInput = Console.ReadLine();
                if (userInput == "?")
                {
                    table.ShowTable();
                    continue;
                }
                int selected;
                if (userInput == "0") return;
                else if (string.IsNullOrEmpty(userInput) || !Int32.TryParse(userInput, out selected) || (selected < 1 || selected > arrLen))
                {
                    Console.WriteLine(Error.InvalidUserInput);
                    continue;
                }
                else
                {
                    Console.WriteLine($"Your move: {arr[selected - 1]} \nComputer move: {arr[computerMove]}");
                    Console.WriteLine($"You {Rule.GetResultString(selected - 1, computerMove, arrLen)}!");
                    Console.WriteLine($"HMAC key: {HMACKey}");
                    return;
                }
            }
        }

        static void Main(string[] args)
        {
            int argLen = args.Length;
            if (argLen == 0)
            {
                Console.WriteLine(Error.NoMovesFound);
                return;
            }
            else if (argLen % 2 == 0 || argLen < 3)
            {
                Console.WriteLine(Error.InvalidMovesLength);
                return;
            }
            else if (args.GroupBy(x => x.ToLower()).Any(g => g.Count() > 1))
            {
                Console.WriteLine(Error.DuplicateMoves);
                return;
            }
            
            StartGame(args, argLen);
            return;
        }
    }

}