using System;
using System.IO;
using System.Net;

namespace RewardsPls
{
    internal class Program
    {
        static string GenerateRandomString(int length)
        {
            const string characterSet = "abcdefghijklmnopqrstuvwxyz1234567890";
            string result = "";

            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(0, characterSet.Length - 1);
                result += characterSet[index];
            }

            return result;
        }

        static void Main(string[] args)
        {
            Console.Title = "RewardsPls";

            string epuid;
            try
            {
                epuid = File.ReadAllText("EPuid.txt");
            }
            catch
            {
                Console.WriteLine("EPuid.txt could not be read. This file must be accessible and must contain your Rewards account's EPuid.");
                goto Exit;
            }

            WebClient webClient = new WebClient();

            try
            {
                dynamic stats = Rewards.ReportActivity(webClient, epuid, "");
                Console.WriteLine("Current balance: {0}", (int)stats["RewardsSessionData"]["Balance"]);

                bool level2 = (bool)stats["RewardsSessionData"]["IsLevel2"];
                Console.WriteLine("Is level 2: {0}", level2);

                Console.WriteLine("Acquiring daily points...");

                if (level2)
                {
                    for (int i = 0; i < 150; i += 5)
                    {
                        if (i < 20)
                            stats = Rewards.ReportActivity(webClient, epuid, GenerateRandomString(20), Rewards.UserAgent.Edge);

                        if (i < 100)
                            stats = Rewards.ReportActivity(webClient, epuid, GenerateRandomString(20), Rewards.UserAgent.Android);

                        stats = Rewards.ReportActivity(webClient, epuid, GenerateRandomString(20), Rewards.UserAgent.Firefox);
                    }
                }
                else
                {
                    stats = Rewards.ReportActivity(webClient, epuid, GenerateRandomString(20), Rewards.UserAgent.Edge);

                    for (int i = 0; i < 50; i += 5)
                    {
                        stats = Rewards.ReportActivity(webClient, epuid, GenerateRandomString(20), Rewards.UserAgent.Firefox);
                    }
                }

                Console.WriteLine("New balance: {0}", (int)stats["RewardsSessionData"]["Balance"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while generating Rewards points: {0}", ex.Message);
            }

        Exit:
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey(true);
        }
    }
}
