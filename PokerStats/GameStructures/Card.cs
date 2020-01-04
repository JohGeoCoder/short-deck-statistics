using System;
using System.Collections.Generic;
using System.Text;

namespace PokerStats.GameStructures
{
    public struct Card
    {
        public readonly short Value;
        public readonly short Suit;

        public Card(short suit, short value)
        {
            Suit = suit;
            Value = value;
        }
    }
}
