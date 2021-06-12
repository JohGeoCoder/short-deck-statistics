using PokerStats.GameStructures;
using System;
using System.Threading;

namespace PokerStats
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Console.WindowHeight = 50;
            //Console.WindowWidth = 200;

            var handTracker = new HandTracker(true);
            TablePlayer.Begin(handTracker, 30, 1_000_000);

            var input = "";

            //Console.ReadKey();
            Console.Clear();
            //handTracker.PrintHoleCardWinRatesRankedByBest(input);
            //handTracker.PrintHoleCardsNumericRankedByBestForArray();
            //handTracker.PrintHoleCardsRankedByBestForArray();

            //if (handTracker.LogPokerHandResults)
            //{
            //    handTracker.PrintWinRatesForPokerHandsMade(input);
            //}

            while(input != "exit")
            {
                Console.Clear();
                Console.WriteLine("Enter a hand");
                input = Console.ReadLine();
                if (input == "exit") continue;

                handTracker.PrintHoleCardWinRatesRankedByBest(input);
                Console.ReadKey();
            }
        }
    }
}
