﻿using PokerStats.GameStructures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PokerStats
{
    public class TablePlayer
    {
        public static void Begin(HandTracker handTracker, int tableCount, int handsToPlayPerTable)
        {
            var startTime = DateTime.Now;

            var processesRemaining = tableCount;
            using ManualResetEvent resetEvent = new ManualResetEvent(false);
            for (int i = 0; i < tableCount; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
                {
                    var tracker = (HandTracker)x;

                    var table = new Table(3, true, 40, 40, tracker);
                    table.PlayHands(handsToPlayPerTable, false);

                    // Safely decrement the counter
                    if (Interlocked.Decrement(ref processesRemaining) == 0)
                    {
                        resetEvent.Set();
                    }

                }), handTracker);
            }

            resetEvent.WaitOne();
            var endTime = DateTime.Now;
            var duration = (endTime - startTime).TotalSeconds;

            Console.WriteLine($"Finished in {duration} seconds");
        }
    }
}
