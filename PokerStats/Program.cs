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

            //while (input.ToLowerInvariant() != "quit")
            //{
            //input = Console.ReadLine();

            Console.ReadKey();
                Console.Clear();
                handTracker.PrintHoleCardWinRatesRankedByBest(input);
                //handTracker.PrintHoleCardsNumericRankedByBestForArray();
                //handTracker.PrintHoleCardsRankedByBestForArray();

                //if (handTracker.LogPokerHandResults)
                //{
                //    handTracker.PrintWinRatesForPokerHandsMade(input);
                //}
            //}
        }
    }
}
