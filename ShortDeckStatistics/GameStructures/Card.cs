using System;
using System.Collections.Generic;
using System.Text;

namespace ShortDeckStatistics.GameStructures
{
    public class Card : IComparable<Card>
    {
        public readonly short Value;
        public readonly short Suit;

        private string _cardString;

        public static readonly string[] CardValues = new string[] { "6", "7", "8", "9", "T", "J", "Q", "K", "A" };
        public static readonly string[] CardSuits = new string[] { "s", "c", "h", "d" };

        public Card(short suit, short value)
        {
            Suit = suit;
            Value = value;

            _cardString = $"{CardValues[Value]}{CardSuits[Suit]}";
        }

        public override string ToString()
        {
            return _cardString;
        }

        public int CompareTo(Card other)
        {
            if (this.Value == other.Value) return 0;

            return this.Value < other.Value ? -1 : 1;
        }
    }
}
