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

            var tableCount = 20;
            var processesRemaining = tableCount;
            using ManualResetEvent resetEvent = new ManualResetEvent(false);
            for (int i = 0; i < tableCount; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
                {
                    var tracker = (HandTracker)x;

                    var table = new Table(9, true, 40, 40, tracker);
                    table.PlayHands(1_000_000, false);

                    // Safely decrement the counter
                    if (Interlocked.Decrement(ref processesRemaining) == 0)
                    {
                        resetEvent.Set();
                    }

                }), handTracker);
            }

            resetEvent.WaitOne();


            //var tableArray = new Table[tableCount];
            //for (int i = 0; i < tableArray.Length; i++)
            //{
            //    tableArray[i] = new Table(9, true, 40, 40, true, handTracker);
            //    tableArray[i].PlayHands(3_000_000, false);
            //}

            Console.WriteLine("FINISHED");

            var input = "";

            while (input.ToLowerInvariant() != "quit")
            {
                input = Console.ReadLine();

                Console.Clear();
                handTracker.PrintHoleCardWinRatesRankedByBest(input);
                handTracker.PrintHoleCardsNumericRankedByBestForArray();
                handTracker.PrintHoleCardsRankedByBestForArray();

                if (handTracker.LogPokerHandResults)
                {
                    handTracker.PrintWinRatesForPokerHandsMade(input);
                }
            }
        }
    }
}
