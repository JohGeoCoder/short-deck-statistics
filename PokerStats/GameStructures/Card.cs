using System;
using System.Collections.Generic;
using System.Text;

namespace PokerStats.GameStructures
{
    public class Card : IComparable<Card>
    {
        public readonly short Value;
        public readonly short Suit;

        public Card(short suit, short value)
        {
            Suit = suit;
            Value = value;
        }

        public int CompareTo(Card other)
        {
            if (this.Value == other.Value) return 0;

            return this.Value < other.Value ? -1 : 1;
        }
    }
}
