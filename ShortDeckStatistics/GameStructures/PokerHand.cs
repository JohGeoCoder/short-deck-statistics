﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShortDeckStatistics.GameStructures
{
    public class PokerHand
    {
        public static readonly int[] ZeroScore = new int[] { 0, 0, 0 };
        public static readonly int[] ScoreContainer = new int[3];

        public static readonly string[] HandRanks = new string[] { "Error", "High Card", "Pair", "Two Pair", "Three of a Kind", "Straight", "Flush", "Full House", "Four of a Kind", "Straight Flush" };

        private Card[] _sevenCardHand;

        public Card[] HoleCards;
        public int HoleCardsNumericRepresentation;

        public Card[] CommunityCards; //Sorted in descending order.

        private long _handRank = -1L;
        public long HandRank
        {
            get
            {
                if (_handRank == -1L)
                {
                    _handRank = RankHand();
                }

                return _handRank;
            }
        }

        public bool IsLiveAsHero;
        public bool IsLiveAsVillain;
        public bool ManiacPlay;

        public Table Table;

        public PokerHand(Table table)
        {
            _sevenCardHand = new Card[7];
            Table = table;
        }

        public void GeneratePokerHand(Card[] holeCards, Card[] communityCards, int playerCount, bool isVillainEmotional, int keepTopPercentHero, int keepTopPercentVillain, bool maniacPlay)
        {
            _handRank = -1;

            CommunityCards = communityCards;
            HoleCards = holeCards;
            ManiacPlay = maniacPlay;

            var biggestHoleCard = holeCards[0];
            var smallestHoleCard = holeCards[1];

            if (biggestHoleCard.Value < smallestHoleCard.Value)
            {
                Card temp = biggestHoleCard;
                biggestHoleCard = smallestHoleCard;
                smallestHoleCard = temp;
            }

            HoleCardsNumericRepresentation = PokerHand.ConvertHoleCardsToNumericValue(biggestHoleCard.Value, smallestHoleCard.Value, biggestHoleCard.Suit == smallestHoleCard.Suit, Table);

            //Merge the hole cards and community cards into a sorted 7-card hand.
            var holeCardPos = 0;
            var communityCardPos = 0;
            while (holeCardPos < 2 && communityCardPos < 5)
            {

                if (holeCards[holeCardPos].Value > communityCards[communityCardPos].Value)
                {
                    _sevenCardHand[holeCardPos + communityCardPos] = holeCards[holeCardPos];
                    holeCardPos++;
                }
                else
                {
                    _sevenCardHand[holeCardPos + communityCardPos] = communityCards[communityCardPos];
                    communityCardPos++;
                }
            }
            while (holeCardPos < 2)
            {
                _sevenCardHand[holeCardPos + communityCardPos] = holeCards[holeCardPos];
                holeCardPos++;
            }
            while (communityCardPos < 5)
            {
                _sevenCardHand[holeCardPos + communityCardPos] = communityCards[communityCardPos];
                communityCardPos++;
            }

            if (!maniacPlay)
            {
                IsLiveAsHero = CheckHeroStartingHandCriteria(playerCount, keepTopPercentHero);
                IsLiveAsVillain = CheckVillainStartingHandCriteria(playerCount, keepTopPercentVillain, isVillainEmotional);
            }
        }

        public override string ToString()
        {
            return $"{HoleCards[0]} {HoleCards[1]} - {CommunityCards[0]} {CommunityCards[1]} {CommunityCards[2]} {CommunityCards[3]} {CommunityCards[4]} - Hand Score: {HandRank}";
        }

        private long RankHand()
        {
            var scoreData = ScoreStraightFlush();
            if (scoreData[0] == 0) scoreData = ScoreFourOfAKind();
            if (scoreData[0] == 0) scoreData = ScoreFullHouse();
            if (scoreData[0] == 0) scoreData = ScoreFlush();
            if (scoreData[0] == 0) scoreData = ScoreStraight();
            if (scoreData[0] == 0) scoreData = ScoreThreeOfAKind();
            if (scoreData[0] == 0) scoreData = ScoreTwoPair();
            if (scoreData[0] == 0) scoreData = ScorePair();
            if (scoreData[0] == 0) scoreData = ScoreHighCard();

            var score =
                1_000L * scoreData[2]
                + 100_000_000L * scoreData[1]
                + 100_000_000_000L * scoreData[0];

            return score;
        }

        private int[] ScoreStraightFlush()
        {
            //Check for sequentiality
            var consecutiveSequenceLength = 0;
            var highestSequentialCardPosition = -1;
            Card currentCard = null;
            Card nextCard = null;
            for (int i = 0; i < _sevenCardHand.Length; i++)
            {
                currentCard = _sevenCardHand[i];
                nextCard = null;

                //Populate the next card, taking into consideration that it may be an Ace in the first position.
                if (i < _sevenCardHand.Length - 1)
                {
                    nextCard = _sevenCardHand[i + 1];
                }
                else
                {
                    var firstCard = _sevenCardHand[0];
                    var secondCard = _sevenCardHand[1];
                    var thirdCard = _sevenCardHand[2];

                    if (firstCard.Value == 8 && firstCard.Suit == currentCard.Suit) nextCard = firstCard;
                    else if (secondCard.Value == 8 && secondCard.Suit == currentCard.Suit) nextCard = secondCard;
                    else if (thirdCard.Value == 8 && thirdCard.Suit == currentCard.Suit) nextCard = thirdCard;
                    else
                    {
                        break;
                    }
                }

                //If the current card and next card are in sequence, update the sequence count.
                //Otherwise, reset the sequence count.
                //Take into consideration that the next sequential card may be an Ace in the first position.
                if (nextCard.Suit == currentCard.Suit && (nextCard.Value == currentCard.Value - 1 || currentCard.Value == 0 && nextCard.Value == 8))
                {
                    if (consecutiveSequenceLength == 0)
                    {
                        consecutiveSequenceLength = 2;
                        highestSequentialCardPosition = i;
                    }
                    else consecutiveSequenceLength++;

                    if (consecutiveSequenceLength == 5) break;
                }
                else
                {
                    consecutiveSequenceLength = 0;
                    highestSequentialCardPosition = -1;
                }
            }

            //If there is not a 5-card sequence, then the poker hand cannot be a straight flush.
            if (consecutiveSequenceLength < 5) return ZeroScore;

            //Determine subtype:
            //Nuts
            //Low SF

            //The value of this straight flush equals the face value of the highest card in the sequence.
            ScoreContainer[0] = 9;
            ScoreContainer[1] = _sevenCardHand[highestSequentialCardPosition].Value + 1;
            ScoreContainer[2] = 0;
            return ScoreContainer;
        }

        private int[] ScoreFourOfAKind()
        {
            var consecutiveValueLength = 0;
            var fourOfAKindCardValue = -1;
            for (int i = 0; i < _sevenCardHand.Length - 1; i++)
            {
                var currentCard = _sevenCardHand[i];
                Card nextCard = _sevenCardHand[i + 1];

                if (currentCard.Value == nextCard.Value)
                {
                    if (consecutiveValueLength == 0)
                    {
                        fourOfAKindCardValue = currentCard.Value;
                        consecutiveValueLength = 2;
                    }
                    else
                    {
                        consecutiveValueLength++;

                        if (consecutiveValueLength == 4) break;
                    }
                }
                else
                {
                    fourOfAKindCardValue = -1;
                    consecutiveValueLength = 0;
                }
            }

            //If there are no four consecutive cards with the same value, then this is not a 4-of-a-kind hand.
            if (consecutiveValueLength < 4) return ZeroScore;

            //Calculate the kicker score
            var kickerValues = new short[1];
            var pos = 0;
            var kickerCount = 0;
            while (kickerCount < 1)
            {
                var currentCardValue = _sevenCardHand[pos].Value;

                if (currentCardValue != fourOfAKindCardValue)
                {
                    kickerValues[kickerCount] = currentCardValue;
                    kickerCount++;
                }

                pos++;
            }

            var kickerVal = kickerValues[0];

            //Determine subtype:
            //Top Quads
            //Second Quads

            ScoreContainer[0] = 8;
            ScoreContainer[1] = fourOfAKindCardValue + 1;
            ScoreContainer[2] = kickerVal;
            return ScoreContainer;
        }

        private int[] ScoreFullHouse()
        {
            //Find the value of the biggest set
            short biggestSetCardValue = -1;
            var consecutiveValueLength = 0;
            for (int i = 0; i < _sevenCardHand.Length - 1; i++)
            {
                var currentCard = _sevenCardHand[i];
                Card nextCard = _sevenCardHand[i + 1];

                //Detect consecutive identical card values.
                if (currentCard.Value == nextCard.Value)
                {
                    //Count consecutive cards with identical values.
                    if (consecutiveValueLength == 0)
                    {
                        consecutiveValueLength = 2;
                    }
                    else
                    {
                        consecutiveValueLength++;

                        //If three consecutive cards with the same value are found, then log the set.
                        //We shouldn't need to consider a 4 of a kind, because a previously called method should have short-circuited the process and skipped this method.
                        if (consecutiveValueLength == 3)
                        {
                            if (currentCard.Value > biggestSetCardValue)
                            {
                                biggestSetCardValue = currentCard.Value;
                            }

                            //Move to the next card. The next iteration should be outside the newly-found set.
                            i++;
                            consecutiveValueLength = 0;
                        };
                    }
                }
                else
                {
                    consecutiveValueLength = 0;
                }
            }

            //If a set doesn't exist, then the poker hand cannot be a full house.
            if (biggestSetCardValue == -1) return ZeroScore;

            //Find the value of the biggest pair that has a different card value than the set.
            short biggestPairCardValue = -1;
            for (int i = 0; i < _sevenCardHand.Length - 1; i++)
            {
                var currentCard = _sevenCardHand[i];
                Card nextCard = _sevenCardHand[i + 1];

                //skip the cards that have a value that match the biggest set's value.
                if (currentCard.Value == biggestSetCardValue) continue;

                //Detect consecutive identical card values.
                if (currentCard.Value == nextCard.Value)
                {
                    //Keep the highest pair value.
                    if (currentCard.Value > biggestPairCardValue)
                    {
                        biggestPairCardValue = currentCard.Value;
                    }

                    //Move to the next card. The next iteration should be outside the newly-found pair.
                    i++;
                }
            }

            //If a pair doesn't exist, then the poker hand cannot be a full house.
            if (biggestPairCardValue == -1) return ZeroScore;

            //Determine subtype:
            //Nut Boat
            //Top Set Low Pair
            //Underset
            //Second Set
            //Board Set Top Pair
            //Board Set Bottom Pair
            //Board Set Overpair
            //Board Set Underpair
            //One Hole Top Set
            //One Hole Bottom Set
            //Board Boat
            //Counterfeit


            ScoreContainer[0] = 7;
            ScoreContainer[1] = biggestSetCardValue * Table.DeckNumericValueCount + biggestPairCardValue;
            ScoreContainer[2] = 0;
            return ScoreContainer;
        }

        private int[] ScoreFlush()
        {
            var suitCounter = new int[Table.DeckSuitCount];
            for (int pos = 0; pos < _sevenCardHand.Length; pos++)
            {
                var card = _sevenCardHand[pos];
                suitCounter[card.Suit]++;
            }

            var flushedSuit = -1;
            for (int suit = 0; suit < suitCounter.Length; suit++)
            {
                if (suitCounter[suit] >= Table.DeckSuitCount + 1)
                {
                    flushedSuit = suit;
                    break;
                }
            }

            if (flushedSuit == -1) return ZeroScore;

            var highestFlushValue = -1;
            var flushKickers = new Card[4];
            var flushKickerCount = 0;
            for (int pos = 0; pos < _sevenCardHand.Length; pos++)
            {
                var card = _sevenCardHand[pos];
                if (card.Suit == flushedSuit)
                {
                    if (highestFlushValue == -1)
                    {
                        highestFlushValue = card.Value;
                    }
                    else
                    {
                        flushKickers[flushKickerCount] = card;
                        flushKickerCount++;

                        if (flushKickerCount == 4) break;
                    }
                }
            }

            var kickerVal =
                Table.DeckNumericValueCount * Table.DeckNumericValueCount * Table.DeckNumericValueCount * flushKickers[0].Value
                + Table.DeckNumericValueCount * Table.DeckNumericValueCount * flushKickers[1].Value
                + Table.DeckNumericValueCount * flushKickers[2].Value
                + flushKickers[3].Value;

            //Determine subtype:
            //Top Hole Flush
            //Bottom Hole Flush
            //Hole Suited Board 3
            //Hole Suited Board 4
            //Hole Suited Board 5
            //Counterfeit
            //Board Flush

            ScoreContainer[0] = 6;
            ScoreContainer[1] = highestFlushValue;
            ScoreContainer[2] = kickerVal;
            return ScoreContainer;
        }

        private int[] ScoreStraight()
        {
            //Check for sequentiality
            var consecutiveSequenceLength = 0;
            var highestSequentialCardPosition = -1;
            for (int i = 0; i < _sevenCardHand.Length; i++)
            {
                var currentCard = _sevenCardHand[i];
                Card nextCard = null;

                //Populate the next card, taking into consideration that it may be an Ace in the first position.
                if (i < _sevenCardHand.Length - 1)
                {
                    nextCard = _sevenCardHand[i + 1];
                }
                else
                {
                    nextCard = _sevenCardHand[0];
                }

                //If the current card and next card are in sequence, update the sequence count.
                //Otherwise, reset the sequence count.
                //Take into consideration that the next sequential card may be an Ace in the first position.
                if (nextCard.Value == currentCard.Value - 1 || currentCard.Value == 0 && nextCard.Value == 8)
                {
                    if (consecutiveSequenceLength == 0)
                    {
                        consecutiveSequenceLength = 2;
                        highestSequentialCardPosition = i;
                    }
                    else consecutiveSequenceLength++;

                    if (consecutiveSequenceLength == 5) break;
                }
                else
                {
                    consecutiveSequenceLength = 0;
                    highestSequentialCardPosition = -1;
                }
            }

            //If there is not a 5-card sequence, then the poker hand cannot be a straight flush.
            if (consecutiveSequenceLength < 5) return ZeroScore;

            //Determine subtype:
            //Two Top
            //One Top
            //One Card Top
            //One Card Bottom
            //One Card Middle
            //Two Bottom
            //Filling Gaps
            //Board Straight

            //The value of this straight equals the face value of the highest card in the sequence.
            ScoreContainer[0] = 5;
            ScoreContainer[1] = _sevenCardHand[highestSequentialCardPosition].Value;
            ScoreContainer[2] = 0;
            return ScoreContainer;
        }

        private int[] ScoreThreeOfAKind()
        {
            short biggestSetCardValue = -1;

            var consecutiveValueLength = 0;
            for (int i = 0; i < _sevenCardHand.Length - 1; i++)
            {
                var currentCard = _sevenCardHand[i];
                Card nextCard = _sevenCardHand[i + 1];

                //Detect consecutive identical card values.
                if (currentCard.Value == nextCard.Value)
                {
                    //Count consecutive cards with identical values.
                    if (consecutiveValueLength == 0)
                    {
                        consecutiveValueLength = 2;
                    }
                    else
                    {
                        consecutiveValueLength++;

                        //If three consecutive cards with the same value are found, then log the set.
                        //We shouldn't need to consider a 4 of a kind, because a previously called method should have short-circuited the process and skipped this method.
                        if (consecutiveValueLength == 3)
                        {
                            if (currentCard.Value > biggestSetCardValue)
                            {
                                biggestSetCardValue = currentCard.Value;
                            }

                            //Once a set is found, we can break out. The instance of a "double set", which is a full-house, should have been taken care of by a previous method.
                            break;
                        };
                    }
                }
                else
                {
                    consecutiveValueLength = 0;
                }
            }

            if (biggestSetCardValue == -1) return ZeroScore;

            var kickerValues = new short[2];
            var pos = 0;
            var kickerCount = 0;
            while (kickerCount < 2)
            {
                var currentCardValue = _sevenCardHand[pos].Value;

                if (currentCardValue != biggestSetCardValue)
                {
                    kickerValues[kickerCount] = currentCardValue;
                    kickerCount++;
                }

                pos++;
            }

            var kickerVal =
                Table.DeckNumericValueCount * kickerValues[0]
                + kickerValues[1];

            //Determine subtype:
            //Top Set
            //Second Set
            //Low Set
            //Board Set

            ScoreContainer[0] = 4;
            ScoreContainer[1] = biggestSetCardValue + 1;
            ScoreContainer[2] = kickerVal;
            return ScoreContainer;
        }

        private int[] ScoreTwoPair()
        {
            //Find the biggest pair
            short biggestPairCardValue = -1;
            for (int i = 0; i < _sevenCardHand.Length - 1; i++)
            {
                var currentCard = _sevenCardHand[i];
                Card nextCard = _sevenCardHand[i + 1];

                //Detect consecutive identical card values.
                if (currentCard.Value == nextCard.Value)
                {
                    //Keep the highest pair value.
                    if (currentCard.Value > biggestPairCardValue)
                    {
                        biggestPairCardValue = currentCard.Value;
                    }

                    //Move to the next card. The next iteration should be outside the newly-found pair.
                    i++;
                }
            }

            //If a pair doesn't exist, then the poker hand cannot be a two pair.
            if (biggestPairCardValue == -1) return ZeroScore;

            //Find the value of the smallest pair
            short smallestPairCardValue = -1;
            for (int i = 0; i < _sevenCardHand.Length - 1; i++)
            {
                var currentCard = _sevenCardHand[i];
                Card nextCard = _sevenCardHand[i + 1];

                //skip the cards that have a value that match the biggest set's value.
                if (currentCard.Value == biggestPairCardValue) continue;

                //Detect consecutive identical card values.
                if (currentCard.Value == nextCard.Value)
                {
                    //Keep the highest pair value.
                    if (currentCard.Value > smallestPairCardValue)
                    {
                        smallestPairCardValue = currentCard.Value;
                    }

                    //Once a second pair is found, we can break out of the loop.
                    break;
                }
            }

            //If a second pair doesn't exist, then the hand cannot be a two pair.
            if (smallestPairCardValue == -1) return ZeroScore;

            //Calculate the kicker score
            var kickerValues = new short[1];
            var pos = 0;
            var kickerCount = 0;
            while (kickerCount < 1)
            {
                var currentCardValue = _sevenCardHand[pos].Value;

                if (currentCardValue != biggestPairCardValue && currentCardValue != smallestPairCardValue)
                {
                    kickerValues[kickerCount] = currentCardValue;
                    kickerCount++;
                }

                pos++;
            }

            var kickerVal = kickerValues[0];

            //Determine subtype:
            //Top Two
            //Over Two Pair
            //Highest Top
            //Second Top Two
            //Low Two Pair
            //Under Two Pair
            //Board Two Pair

            ScoreContainer[0] = 3;
            ScoreContainer[1] = biggestPairCardValue * Table.DeckNumericValueCount + smallestPairCardValue;
            ScoreContainer[2] = kickerVal;
            return ScoreContainer;
        }

        private int[] ScorePair()
        {
            //Find the value of the smallest pair
            short pairCardValue = -1;
            for (int i = 0; i < _sevenCardHand.Length - 1; i++)
            {
                var currentCard = _sevenCardHand[i];
                Card nextCard = _sevenCardHand[i + 1];

                //Detect consecutive identical card values.
                if (currentCard.Value == nextCard.Value)
                {
                    //Keep the highest pair value.
                    if (currentCard.Value > pairCardValue)
                    {
                        pairCardValue = currentCard.Value;
                    }

                    //Once a second pair is found, we can break out of the loop.
                    break;
                }
            }

            if (pairCardValue == -1) return ZeroScore;

            var kickerValues = new short[3];
            var pos = 0;
            var kickerCount = 0;
            while (kickerCount < 3)
            {
                var currentCardValue = _sevenCardHand[pos].Value;

                if (currentCardValue != pairCardValue)
                {
                    kickerValues[kickerCount] = currentCardValue;
                    kickerCount++;
                }

                pos++;
            }

            var kickerVal =
                Table.DeckNumericValueCount * Table.DeckNumericValueCount * kickerValues[0]
                + Table.DeckNumericValueCount * kickerValues[1]
                + kickerValues[2];

            //Determine subtype:
            //Overpair
            //Top Pair
            //Second Pair //Not sufficiently defined or necessary. Merged with low pair.
            //Low Pair
            //Underpair
            //Board Pair
            //Counterfeit //Can't determine because 2pair would be on the board.

            var subtypeScore = 0;

            //Check for Board Pair subtype.
            if (
                CommunityCards[0].Value == CommunityCards[1].Value
                || CommunityCards[1].Value == CommunityCards[2].Value
                || CommunityCards[2].Value == CommunityCards[3].Value
                || CommunityCards[3].Value == CommunityCards[4].Value
            )
            {
                subtypeScore = 1; //Underpair subtype.
            }

            //Check for Underpair subtype.
            if(subtypeScore == 0
                && HoleCards[0].Value == HoleCards[1].Value
                && HoleCards[0].Value < CommunityCards[4].Value)
            {
                subtypeScore = 2; //Underpair subtype.
            }

            //Check for Low Pair subtype.
            if (subtypeScore == 0
                && (
                    HoleCards[0].Value != HoleCards[1].Value
                    && (
                        HoleCards[0].Value == CommunityCards[4].Value
                        || HoleCards[0].Value == CommunityCards[3].Value
                        || HoleCards[0].Value == CommunityCards[2].Value
                        || HoleCards[0].Value == CommunityCards[1].Value
                        || HoleCards[1].Value == CommunityCards[4].Value
                        || HoleCards[1].Value == CommunityCards[3].Value
                        || HoleCards[1].Value == CommunityCards[2].Value
                        || HoleCards[1].Value == CommunityCards[1].Value
                    )
                )
                || (
                    HoleCards[0].Value == HoleCards[1].Value
                    && (
                        HoleCards[0].Value > CommunityCards[4].Value
                        && HoleCards[0].Value < CommunityCards[0].Value
                    )
                )
            )
            {
                subtypeScore = 3; //Low Pair subtype.
            }

            //Check for Top Pair subtype
            if (subtypeScore == 0
                && HoleCards[0].Value != HoleCards[1].Value
                && (
                    HoleCards[0].Value == CommunityCards[0].Value
                    || HoleCards[1].Value == CommunityCards[0].Value
                )
            )
            {
                subtypeScore = 4; //Top Pair subtype.
            }

            //Check for Overpair subtype
            if(subtypeScore == 0
                && HoleCards[0].Value == HoleCards[1].Value
                && HoleCards[0].Value > CommunityCards[0].Value
            )
            {
                subtypeScore = 5; //Overpair subtype.
            }

            ScoreContainer[0] = 2;
            ScoreContainer[1] = pairCardValue + 1;
            ScoreContainer[2] = kickerVal;
            return ScoreContainer;
        }

        private int[] ScoreHighCard()
        {
            var highestValueCard = _sevenCardHand[0];

            var kickerVal =
                Table.DeckNumericValueCount * Table.DeckNumericValueCount * Table.DeckNumericValueCount * _sevenCardHand[1].Value
                + Table.DeckNumericValueCount * Table.DeckNumericValueCount * _sevenCardHand[2].Value
                + Table.DeckNumericValueCount * _sevenCardHand[3].Value
                + _sevenCardHand[4].Value;

            ScoreContainer[0] = 1;
            ScoreContainer[1] = highestValueCard.Value;
            ScoreContainer[2] = kickerVal;
            return ScoreContainer;
        }

        private bool CheckHeroStartingHandCriteria(int playerCount, int keepTopPercent)
        {
            var isStartingHand = false;

            var bestholeCards = Table.BestHoleCardsNumericHero;

            int lowerBound = 0;
            int upperBound = bestholeCards.Length;
            int pos = (lowerBound + upperBound) / 2;
            while (lowerBound <= upperBound && HoleCardsNumericRepresentation != bestholeCards[pos])
            {
                if (HoleCardsNumericRepresentation < bestholeCards[pos])
                {
                    upperBound = pos - 1;
                }
                else
                {
                    lowerBound = pos + 1;
                }

                pos = (lowerBound + upperBound) / 2;
            }

            isStartingHand = bestholeCards[pos] == HoleCardsNumericRepresentation;

            return isStartingHand;
        }

        private bool CheckVillainStartingHandCriteria(int playerCount, int keepTopPercent, bool isVillainEmotional)
        {
            var isStartingHand = false;

            if (isVillainEmotional)
            {
                int lowerBound = 0;
                int upperBound = Table.EmotionalCardsNumeric.Length;
                int pos = (lowerBound + upperBound) / 2;
                while (lowerBound <= upperBound && HoleCardsNumericRepresentation != Table.EmotionalCardsNumeric[pos])
                {
                    if (HoleCardsNumericRepresentation < Table.EmotionalCardsNumeric[pos])
                    {
                        upperBound = pos - 1;
                    }
                    else
                    {
                        lowerBound = pos + 1;
                    }

                    pos = (lowerBound + upperBound) / 2;
                }

                isStartingHand = Table.EmotionalCardsNumeric[pos] == HoleCardsNumericRepresentation;
            }
            else
            {
                var bestholeCards = Table.BestHoleCardsNumericVillain;

                int lowerBound = 0;
                int upperBound = bestholeCards.Length;
                int pos = (lowerBound + upperBound) / 2;
                while (lowerBound <= upperBound && HoleCardsNumericRepresentation != bestholeCards[pos])
                {
                    if (HoleCardsNumericRepresentation < bestholeCards[pos])
                    {
                        upperBound = pos - 1;
                    }
                    else
                    {
                        lowerBound = pos + 1;
                    }

                    pos = (lowerBound + upperBound) / 2;
                }

                isStartingHand = bestholeCards[pos] == HoleCardsNumericRepresentation;
            }

            return isStartingHand;
        }

        public static int ConvertHoleCardsToNumericValue(int biggestCardValue, int smallestCardValue, bool areSuited, Table table)
        {
            return (biggestCardValue * table.DeckNumericValueCount + smallestCardValue) * 2 + (areSuited ? 1 : 0);
        }

        public static string ConvertHoleCardsNumericValueToString(int holeCardsNumericValue, Table table)
        {
            bool suited = holeCardsNumericValue % 2 == 1;

            int biggestCardValue = (holeCardsNumericValue / 2) / table.DeckNumericValueCount;
            int smallestCardValue = (holeCardsNumericValue / 2) % table.DeckNumericValueCount;

            return $"{Card.CardValues[biggestCardValue]}{Card.CardValues[smallestCardValue]}{(suited ? "s" : "o")}";
        }

        public static int ConvertCardStringToNumericValue(string cardString, Table table)
        {
            if (string.IsNullOrEmpty(cardString)) return 0;

            var holeCardProperties = cardString.ToCharArray();

            if (holeCardProperties.Length != 3) return 0;

            var biggestCardValue = Array.IndexOf(Card.CardValues, holeCardProperties[0].ToString());
            var smallestCardValue = Array.IndexOf(Card.CardValues, holeCardProperties[1].ToString());
            var areSuited = holeCardProperties[2] == 's';

            if (biggestCardValue == -1 || smallestCardValue == -1) return 0;

            var holeCardsNumericRepresentation = ConvertHoleCardsToNumericValue(biggestCardValue, smallestCardValue, areSuited, table);
            return holeCardsNumericRepresentation;
        }
    }
}
