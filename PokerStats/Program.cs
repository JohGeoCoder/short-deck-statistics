using PokerStats.GameStructures;
using System;

namespace PokerStats
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Console.WindowHeight = 50;
            //Console.WindowWidth = 200;

            var handTracker = new HandTracker();

            var tableCount = 1;
            var tableArray = new Table[tableCount];
            for (int i = 0; i < tableArray.Length; i++)
            {
                tableArray[i] = new Table(9, true, 40, 40, true, handTracker);
                tableArray[i].PlayHands(3_000_000, false);
            }

            var input = "";

            while (input.ToLowerInvariant() != "quit")
            {
                input = Console.ReadLine();

                var table = tableArray[0];

                Console.Clear();
                handTracker.PrintHoleCardWinRatesRankedByBest(input);
                handTracker.PrintHoleCardsNumericRankedByBestForArray();
                handTracker.PrintHoleCardsRankedByBestForArray();

                if (table.LogPokerHandResults)
                {
                    handTracker.PrintWinRatesForPokerHandsMade(input);
                }
            }
        }
    }
}
