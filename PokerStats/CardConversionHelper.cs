using PokerStats.GameStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokerStats
{
    public static class CardConversionHelper
    {
        public static int ConvertHoleCardsToNumericValue(int biggestCardValue, int smallestCardValue, bool areSuited)
        {
            return (biggestCardValue * HandTracker.DeckNumericValueCount + smallestCardValue) * 2 + (areSuited ? 1 : 0);
        }

        public static string ConvertHoleCardsNumericValueToString(int holeCardsNumericValue)
        {
            bool suited = holeCardsNumericValue % 2 == 1;

            int biggestCardValue = (holeCardsNumericValue / 2) / HandTracker.DeckNumericValueCount;
            int smallestCardValue = (holeCardsNumericValue / 2) % HandTracker.DeckNumericValueCount;

            return $"{HandTracker.CardValues[biggestCardValue]}{HandTracker.CardValues[smallestCardValue]}{(suited ? "s" : "o")}";
        }

        public static int ConvertCardStringToNumericValue(string cardString)
        {
            if (string.IsNullOrEmpty(cardString)) return 0;

            var holeCardProperties = cardString.ToCharArray();

            if (holeCardProperties.Length != 3) return 0;

            var biggestCardValue = Array.IndexOf(HandTracker.CardValues, holeCardProperties[0].ToString());
            var smallestCardValue = Array.IndexOf(HandTracker.CardValues, holeCardProperties[1].ToString());
            var areSuited = holeCardProperties[2] == 's';

            if (biggestCardValue == -1 || smallestCardValue == -1) return 0;

            var holeCardsNumericRepresentation = ConvertHoleCardsToNumericValue(biggestCardValue, smallestCardValue, areSuited);
            return holeCardsNumericRepresentation;
        }
    }
}
