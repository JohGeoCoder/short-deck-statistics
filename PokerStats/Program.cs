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

            var tableArray = new Table[3];
            for (int i = 2; i < tableArray.Length; i++)
            {
                tableArray[i] = new Table(9, true, 40, 40, true);
                tableArray[i].PlayHands(3_000_000, false);
            }

            //for (int i = 2; i < tableArray.Length; i++)
            //{
            //    tableArray[i].PrintHoleCardWinRatesRankedByBest();

            //    if (tableArray[i].LogPokerHandResults)
            //    {
            //        tableArray[i].PrintWinRatesForPokerHandsMade();
            //    }

            //    //tableArray[i].PrintHoleCardsNumericRankedByBestForArray();
            //}

            var input = "";

            while (input.ToLowerInvariant() != "quit")
            {
                input = Console.ReadLine();

                var table = tableArray[2];

                Console.Clear();
                table.PrintHoleCardWinRatesRankedByBest(input);
                table.PrintHoleCardsNumericRankedByBestForArray();
                table.PrintHoleCardsRankedByBestForArray();

                if (table.LogPokerHandResults)
                {
                    table.PrintWinRatesForPokerHandsMade(input);
                }
            }
        }
    }
}
