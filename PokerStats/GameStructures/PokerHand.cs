using System;
using System.Collections.Generic;
using System.Text;

namespace PokerStats.GameStructures
{
    public class PokerHand
    {
        public static readonly int[] ZeroScore = new int[] { 0, 0, 0, 0 };
        public static readonly int[] ScoreContainer = new int[4];

        public static readonly string[] HandRanks = new string[] { "Error", "High Card", "Pair", "Two Pair", "Three of a Kind", "Straight", "Flush", "Full House", "Four of a Kind", "Straight Flush" };
        public static readonly string[] CardValues = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "Q", "K", "A" };

        private int[] _sevenCardHand;

        public int[] HoleCards;
        public int HoleCardsNumericRepresentation;

        public int[] CommunityCards; //Sorted in descending order by value.

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
            _sevenCardHand = new int[7];
            Table = table;
        }

        public void GeneratePokerHand(int[] holeCards, int[] communityCards, int playerCount, bool isVillainEmotional, int keepTopPercentHero, int keepTopPercentVillain, bool maniacPlay)
        {
            _handRank = -1;

            CommunityCards = communityCards;
            HoleCards = holeCards;
            ManiacPlay = maniacPlay;

            var biggestHoleCard = holeCards[0];
            var smallestHoleCard = holeCards[1];

            if (biggestHoleCard % Table.DeckNumericValueCount < smallestHoleCard % Table.DeckNumericValueCount)
            {
                int temp = biggestHoleCard;
                biggestHoleCard = smallestHoleCard;
                smallestHoleCard = temp;
            }

            HoleCardsNumericRepresentation = PokerHand.ConvertHoleCardsToNumericValue(biggestHoleCard % Table.DeckNumericValueCount, smallestHoleCard % Table.DeckNumericValueCount, biggestHoleCard / Table.DeckNumericValueCount == smallestHoleCard / Table.DeckNumericValueCount, Table);

            //Merge the hole cards and community cards into a sorted 7-card hand.
            var holeCardPos = 0;
            var communityCardPos = 0;
            while (holeCardPos < 2 && communityCardPos < 5)
            {

                if (holeCards[holeCardPos] % Table.DeckNumericValueCount > communityCards[communityCardPos] % Table.DeckNumericValueCount)
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
                scoreData[3]
                + 1_000L * scoreData[2]
                + 100_000_000L * scoreData[1]
                + 100_000_000_000L * scoreData[0];

            return score;
        }

        #region Straight Flush
        private int[] ScoreStraightFlush()
        {
            //Check for sequentiality
            var consecutiveSequenceLength = 0;
            var highestSequentialCardPosition = -1;
            int currentCard = -1;
            int nextCard = -1;
            for (byte i = 0; i < _sevenCardHand.Length; i++)
            {
                currentCard = _sevenCardHand[i];
                nextCard = -1;

                //Populate the next card, taking into consideration that it may be an Ace in the first position.
                if (i < _sevenCardHand.Length - 1)
                {
                    nextCard = _sevenCardHand[i + 1];
                }
                else
                {
                    //We've checked all the cards so far.
                    //The bottom of the straight may be an ace, which would be at the beginning of the 7-card hand.
                    //If an ace of the proper suit does exist, it would be within the first three cards of the 
                    //7-card poker hand. Check the first three cards for an Ace of the proper suit.
                    var firstCard = _sevenCardHand[0];
                    var secondCard = _sevenCardHand[1];
                    var thirdCard = _sevenCardHand[2];

                    if (firstCard % Table.DeckNumericValueCount == Table.DeckNumericValueCount - 1 && firstCard / Table.DeckNumericValueCount == currentCard / Table.DeckNumericValueCount) nextCard = firstCard;
                    else if (secondCard % Table.DeckNumericValueCount == Table.DeckNumericValueCount - 1 && secondCard / Table.DeckNumericValueCount == currentCard / Table.DeckNumericValueCount) nextCard = secondCard;
                    else if (thirdCard % Table.DeckNumericValueCount == Table.DeckNumericValueCount - 1 && thirdCard / Table.DeckNumericValueCount == currentCard / Table.DeckNumericValueCount) nextCard = thirdCard;
                    else
                    {
                        break;
                    }
                }

                //If the current card and next card have the same value, then
                //skip to the next card.
                if (nextCard % Table.DeckNumericValueCount == currentCard % Table.DeckNumericValueCount) continue;

                //If the current card and next card are in sequence, update the sequence count.
                //Otherwise, reset the sequence count.
                //Take into consideration that the next sequential card may be an Ace in the first position.
                if (nextCard / Table.DeckNumericValueCount == currentCard / Table.DeckNumericValueCount && (nextCard % Table.DeckNumericValueCount == currentCard % Table.DeckNumericValueCount - 1 || currentCard % Table.DeckNumericValueCount == 0 && nextCard % Table.DeckNumericValueCount == Table.DeckNumericValueCount - 1))
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

            var subtypeScore = 0;

            if (IsNutStraightFlush()) subtypeScore = 1;

            //The value of this straight flush equals the face value of the highest card in the sequence.
            ScoreContainer[0] = 9;
            ScoreContainer[1] = _sevenCardHand[highestSequentialCardPosition] % Table.DeckNumericValueCount + 1;
            ScoreContainer[2] = 0;
            ScoreContainer[3] = subtypeScore;
            return ScoreContainer;
        }

        private bool IsNutStraightFlush()
        {
            var firstNutHoleCardValue = -1;
            var secondNutHoleCardValue = -1;

            for (short i = 0; i < 3; i++)
            {
                Span<bool> patternArray = stackalloc bool[] { false, false, false, false, false };

                var targetCardValue = CommunityCards[i] % Table.DeckNumericValueCount;

                //Set the target card as the first value in the pattern array
                patternArray[0] = true;

                //Iterate through the remaining community cards.
                //Each subsequent index of the array represents the next consecutive value card.
                //Build the pattern.
                for (int j = i + 1; j < CommunityCards.Length; j++)
                {
                    var relationIndex = targetCardValue - CommunityCards[j] % Table.DeckNumericValueCount;
                    if (relationIndex < 5)
                    {
                        patternArray[relationIndex] = true;
                    }
                }

                //Determine which pattern exists and return true if the 
                //corresponding nut hand matches the hole cards.
                //Return false if otherwise.                
                if (patternArray[1] && patternArray[2])
                {
                    //Case when target card is Ace
                    if (targetCardValue == Table.DeckNumericValueCount - 1)
                    {
                        //If a Jack is in the pattern, a Ten is the nuts
                        if (patternArray[3])
                        {
                            firstNutHoleCardValue = targetCardValue - 4;
                        }
                        else //Jack Ten is the nuts
                        {
                            firstNutHoleCardValue = targetCardValue - 3;
                            secondNutHoleCardValue = targetCardValue - 4;
                        }
                    }
                    //Case when target card is King
                    else if (targetCardValue == Table.DeckNumericValueCount - 2)
                    {
                        //If a Ten is in the pattern, an Ace is the nuts
                        if (patternArray[3])
                        {
                            firstNutHoleCardValue = targetCardValue + 1;
                        }
                        else //Ace Ten is the nuts
                        {
                            firstNutHoleCardValue = targetCardValue + 1;
                            secondNutHoleCardValue = targetCardValue - 3;
                        }
                    }
                    else
                    {
                        firstNutHoleCardValue = targetCardValue + 2;
                        secondNutHoleCardValue = targetCardValue + 1;
                    }
                }
                else if (patternArray[1] && patternArray[3])
                {
                    //Case when target card is Ace
                    if (targetCardValue == Table.DeckNumericValueCount - 1)
                    {
                        //Queen Ten is the nuts
                        firstNutHoleCardValue = targetCardValue - 2;
                        secondNutHoleCardValue = targetCardValue - 4;
                    }
                    else
                    {
                        firstNutHoleCardValue = targetCardValue + 1;
                        secondNutHoleCardValue = targetCardValue - 2;
                    }
                }
                else if (patternArray[1] && patternArray[4])
                {
                    firstNutHoleCardValue = targetCardValue - 2;
                    secondNutHoleCardValue = targetCardValue - 3;
                }
                else if (patternArray[2] && patternArray[3])
                {
                    //Case when target card is Ace
                    if (targetCardValue == Table.DeckNumericValueCount - 1)
                    {
                        firstNutHoleCardValue = targetCardValue - 1;
                        secondNutHoleCardValue = targetCardValue - 4;
                    }
                    else
                    {
                        firstNutHoleCardValue = targetCardValue + 1;
                        secondNutHoleCardValue = targetCardValue - 1;
                    }
                }
                else if (patternArray[2] && patternArray[4])
                {
                    firstNutHoleCardValue = targetCardValue - 1;
                    secondNutHoleCardValue = targetCardValue - 3;
                }
                else if (patternArray[3] && patternArray[4])
                {
                    firstNutHoleCardValue = targetCardValue - 1;
                    secondNutHoleCardValue = targetCardValue - 2;
                }
                else
                {
                    //No pattern found
                }
            }

            if (firstNutHoleCardValue == -1)
            {
                //No pattern found. Look for lower-end straight.
                //For there to be a lower-end nut straight that's not handled
                //by an existing pattern, there must be an Ace involved.
                if (CommunityCards[0] % Table.DeckNumericValueCount == Table.DeckNumericValueCount - 1)
                {
                    //Record the existence of Twos, Threes, fours, and Fives.
                    Span<bool> wheelContents = stackalloc bool[] { false, false, false, false };
                    var wheelCount = 0;

                    for (int i = 1; i < CommunityCards.Length; i++)
                    {
                        var communityCardValue = CommunityCards[i] % Table.DeckNumericValueCount;
                        if (communityCardValue < wheelContents.Length)
                        {
                            if (!wheelContents[communityCardValue])
                            {
                                wheelCount++;
                                wheelContents[communityCardValue] = true;
                            }
                        }
                    }

                    if (wheelCount == 2)
                    {
                        for (int i = wheelContents.Length - 1; i >= 0; i--)
                        {

                            if (!wheelContents[i])
                            {
                                if (firstNutHoleCardValue == -1)
                                {
                                    firstNutHoleCardValue = i;
                                }
                                else if (secondNutHoleCardValue == -1)
                                {
                                    secondNutHoleCardValue = i;
                                }
                            }
                        }
                    }
                }
            }

            //Check if the hole cards contain the nuts.
            if (firstNutHoleCardValue != -1)
            {
                if (secondNutHoleCardValue != -1)
                {
                    return HoleCards[0] % Table.DeckNumericValueCount == firstNutHoleCardValue
                        && HoleCards[1] % Table.DeckNumericValueCount == secondNutHoleCardValue;
                }
                else
                {
                    return HoleCards[0] % Table.DeckNumericValueCount == firstNutHoleCardValue
                        || HoleCards[1] % Table.DeckNumericValueCount == firstNutHoleCardValue;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion StraightFlush

        #region Four of a Kind
        private int[] ScoreFourOfAKind()
        {
            var consecutiveValueLength = 0;
            var fourOfAKindCardValue = -1;
            for (short i = 0; i < _sevenCardHand.Length - 1; i++)
            {
                int currentCard = _sevenCardHand[i];
                int nextCard = _sevenCardHand[i + 1];

                if (currentCard % Table.DeckNumericValueCount == nextCard % Table.DeckNumericValueCount)
                {
                    if (consecutiveValueLength == 0)
                    {
                        fourOfAKindCardValue = currentCard % Table.DeckNumericValueCount;
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
            var kickerValue = 0;
            var pos = 0;
            var kickerCount = 0;
            while (kickerCount < 1)
            {
                var currentCardValue = _sevenCardHand[pos] % Table.DeckNumericValueCount;

                if (currentCardValue != fourOfAKindCardValue)
                {
                    kickerValue = currentCardValue;
                    kickerCount++;
                }

                pos++;
            }

            var kickerVal = kickerValue;

            //Determine subtype:
            //Top Quads
            //Second Quads

            var subtypeScore = 0;

            if (IsNutFourOfAKind()) subtypeScore = 1;

            ScoreContainer[0] = 8;
            ScoreContainer[1] = fourOfAKindCardValue + 1;
            ScoreContainer[2] = kickerVal;
            ScoreContainer[3] = subtypeScore;
            return ScoreContainer;
        }

        public bool IsNutFourOfAKind()
        {
            //Find the value of the biggest pair that has a different card value than the set.
            int biggestCommunityPairValue = -1;
            for (short i = 0; i < CommunityCards.Length - 1; i++)
            {
                int currentCard = CommunityCards[i];
                int nextCard = CommunityCards[i + 1];

                //Detect consecutive identical card values.
                if (currentCard % Table.DeckNumericValueCount == nextCard % Table.DeckNumericValueCount)
                {
                    biggestCommunityPairValue = currentCard % Table.DeckNumericValueCount;
                    break;
                }
            }

            return HoleCards[0] % Table.DeckNumericValueCount == biggestCommunityPairValue
                && HoleCards[1] % Table.DeckNumericValueCount == biggestCommunityPairValue;
        }

        #endregion Four of a Kind

        #region Full House
        private int[] ScoreFullHouse()
        {
            //Find the value of the biggest set
            int biggestSetCardValue = -1;
            var consecutiveValueLength = 0;
            for (short i = 0; i < _sevenCardHand.Length - 1; i++)
            {
                int currentCard = _sevenCardHand[i];
                int nextCard = _sevenCardHand[i + 1];

                //Detect consecutive identical card values.
                if (currentCard % Table.DeckNumericValueCount == nextCard % Table.DeckNumericValueCount)
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
                            if (currentCard % Table.DeckNumericValueCount > biggestSetCardValue)
                            {
                                biggestSetCardValue = currentCard % Table.DeckNumericValueCount;
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
            int biggestPairCardValue = -1;
            for (short i = 0; i < _sevenCardHand.Length - 1; i++)
            {
                int currentCard = _sevenCardHand[i];
                int nextCard = _sevenCardHand[i + 1];

                //skip the cards that have a value that match the biggest set's value.
                if (currentCard % Table.DeckNumericValueCount == biggestSetCardValue) continue;

                //Detect consecutive identical card values.
                if (currentCard % Table.DeckNumericValueCount == nextCard % Table.DeckNumericValueCount)
                {
                    //Keep the highest pair value.
                    if (currentCard % Table.DeckNumericValueCount > biggestPairCardValue)
                    {
                        biggestPairCardValue = currentCard % Table.DeckNumericValueCount;
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

            var subtypeScore = 0;

            if (IsNutFullHouse()) subtypeScore = 1;

            ScoreContainer[0] = 7;
            ScoreContainer[1] = biggestSetCardValue * Table.DeckNumericValueCount + biggestPairCardValue;
            ScoreContainer[2] = 0;
            ScoreContainer[3] = subtypeScore;
            return ScoreContainer;
        }

        private bool IsNutFullHouse()
        {
            var firstNutHoleCardValue = -1;
            var secondNutHoleCardValue = -1;

            var highestNonPairCardValue = -1;
            var pairValueHighest = -1;
            var pairValueLowest = -1;
            var tripsValue = -1;
            var lastPositionPairValue = -1;

            //Determine the board flavor
            for (short i = 0; i < CommunityCards.Length - 1; i++)
            {
                var currentCard = CommunityCards[i];
                var nextCard = CommunityCards[i + 1];

                if (currentCard % Table.DeckNumericValueCount == nextCard % Table.DeckNumericValueCount)
                {
                    //If there are two pair evaluations in a row, that's a 3 of a kind.
                    //There can be only one set on a board. Skip the next position.
                    if (lastPositionPairValue > -1)
                    {
                        tripsValue = nextCard % Table.DeckNumericValueCount;

                        //The existence of trips eliminates a pair.
                        if (pairValueLowest > -1)
                        {
                            pairValueLowest = -1;
                        }
                        else
                        {
                            pairValueHighest = -1;
                        }

                        i++;
                    }
                    else
                    {
                        //Keep track of the highest and lowest pair.
                        if (nextCard % Table.DeckNumericValueCount > pairValueHighest)
                        {
                            pairValueLowest = pairValueHighest;
                            pairValueHighest = nextCard % Table.DeckNumericValueCount;
                        }
                        else if (nextCard % Table.DeckNumericValueCount > pairValueLowest)
                        {
                            pairValueLowest = nextCard % Table.DeckNumericValueCount;
                        }
                    }

                    //Keep track of the pair for this position.
                    lastPositionPairValue = nextCard % Table.DeckNumericValueCount;
                }
                else
                {
                    //Keep track of the highest non-pair card.
                    if (lastPositionPairValue == -1)
                    {
                        if (currentCard % Table.DeckNumericValueCount > highestNonPairCardValue)
                        {
                            highestNonPairCardValue = currentCard % Table.DeckNumericValueCount;
                        }
                    }

                    //This position has no pair. Reset the flag.
                    lastPositionPairValue = -1;
                }
            }

            //Are there trips on the board?
            if (tripsValue > -1)
            {
                //Are the trips Aces?
                if (tripsValue == Table.DeckNumericValueCount - 1)
                {
                    //Is there a pair of kings on the board?
                    if (pairValueHighest == Table.DeckNumericValueCount - 2)
                    {
                        //Then the board is the nut full house
                    }
                    //Else is there a king on the board?
                    else if (highestNonPairCardValue == Table.DeckNumericValueCount - 2)
                    {
                        //Then a king in hand is the nut full house
                        firstNutHoleCardValue = Table.DeckNumericValueCount - 2;
                    }
                    else
                    {
                        //Then pocket Kings is the nut full house
                        firstNutHoleCardValue = Table.DeckNumericValueCount - 2;
                        secondNutHoleCardValue = Table.DeckNumericValueCount - 2;
                    }
                }
                //Else the trips are kings or below.
                else
                {
                    //Is the highest non-pair value card on the board higher than the value of the trips?
                    if (highestNonPairCardValue > tripsValue)
                    {
                        //Then the nut full house is a pocket pair of that highest value
                        firstNutHoleCardValue = highestNonPairCardValue;
                        secondNutHoleCardValue = highestNonPairCardValue;
                    }
                    else
                    {
                        //The nut full house is a pair of pocket Aces
                        firstNutHoleCardValue = Table.DeckNumericValueCount - 1;
                        secondNutHoleCardValue = Table.DeckNumericValueCount - 1;
                    }
                }
            }
            else
            {
                //If the highest non-pair card value is higher than the highest pair value.
                if (highestNonPairCardValue > pairValueHighest)
                {
                    //Then the nut full house is a pocket pair of that highest value
                    firstNutHoleCardValue = highestNonPairCardValue;
                    secondNutHoleCardValue = highestNonPairCardValue;
                }
                else if (highestNonPairCardValue > pairValueLowest)
                {
                    //Then the nut full house is a hand with the pair value and highest non-pair card value.
                    firstNutHoleCardValue = pairValueHighest;
                    secondNutHoleCardValue = highestNonPairCardValue;
                }
                else
                {
                    //Implying there are two pairs on the board, and the highest non-pair card is lower than the lowest pair.
                    //The nut full house is a hand with the value card that matches the highest pair.
                    firstNutHoleCardValue = pairValueHighest;
                }
            }

            return (firstNutHoleCardValue == -1 || HoleCards[0] % Table.DeckNumericValueCount == firstNutHoleCardValue)
                && (secondNutHoleCardValue == -1 || HoleCards[1] % Table.DeckNumericValueCount == secondNutHoleCardValue);
        }

        #endregion Full House

        #region Flush

        private int[] ScoreFlush()
        {
            Span<int> suitCounter = stackalloc int[Table.DeckSuitCount];
            for (short pos = 0; pos < _sevenCardHand.Length; pos++)
            {
                var card = _sevenCardHand[pos];
                suitCounter[card / Table.DeckNumericValueCount]++;
            }

            var flushedSuit = -1;
            for (short suit = 0; suit < suitCounter.Length; suit++)
            {
                if (suitCounter[suit] >= Table.DeckSuitCount + 1)
                {
                    flushedSuit = suit;
                    break;
                }
            }

            if (flushedSuit == -1) return ZeroScore;

            var highestFlushValue = -1;
            Span<int> flushKickers = stackalloc int[4];
            var flushKickerCount = 0;
            for (short pos = 0; pos < _sevenCardHand.Length; pos++)
            {
                var card = _sevenCardHand[pos];
                if (card / Table.DeckNumericValueCount == flushedSuit)
                {
                    if (highestFlushValue == -1)
                    {
                        highestFlushValue = card % Table.DeckNumericValueCount;
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
                Table.DeckNumericValueCount * Table.DeckNumericValueCount * Table.DeckNumericValueCount * flushKickers[0] % Table.DeckNumericValueCount
                + Table.DeckNumericValueCount * Table.DeckNumericValueCount * flushKickers[1] % Table.DeckNumericValueCount
                + Table.DeckNumericValueCount * flushKickers[2] % Table.DeckNumericValueCount
                + flushKickers[3] % Table.DeckNumericValueCount;

            //Determine subtype:
            //Top Hole Flush
            //Bottom Hole Flush
            //Hole Suited Board 3
            //Hole Suited Board 4
            //Hole Suited Board 5
            //Counterfeit
            //Board Flush

            var subtypeScore = 0;

            if (IsNutFlush(flushedSuit)) subtypeScore = 1;

            ScoreContainer[0] = 6;
            ScoreContainer[1] = highestFlushValue;
            ScoreContainer[2] = kickerVal;
            ScoreContainer[3] = subtypeScore;
            return ScoreContainer;
        }

        private bool IsNutFlush(int suitValue)
        {
            var nutHoleCardValue = -1;

            var isHighestCard = true;
            var targetNextCardValue = -1;
            for (short j = 0; j < CommunityCards.Length; j++)
            {
                if (CommunityCards[j] / Table.DeckNumericValueCount != suitValue) continue;

                //If this is the first card of the target suit (also the highest value), check to see
                //if it is a King or smaller. That would mean the nut card is an Ace
                if (isHighestCard)
                {
                    if (CommunityCards[j] % Table.DeckNumericValueCount < Table.DeckNumericValueCount - 1)
                    {
                        nutHoleCardValue = Table.DeckNumericValueCount - 1;
                        break;
                    }

                    //This block only runs once.
                    isHighestCard = false;
                }

                //If there was a previous card to compare the current card to, then check for a gap.
                //Otherwise, set the initial card value.
                if (targetNextCardValue > -1)
                {
                    //If we find a gap, then the value of the card that should be in the gap is the
                    //nut card for the flush.
                    if (targetNextCardValue != CommunityCards[j] % Table.DeckNumericValueCount)
                    {
                        nutHoleCardValue = targetNextCardValue;
                        break;
                    }

                    targetNextCardValue--;
                }
                else
                {
                    targetNextCardValue = CommunityCards[j] % Table.DeckNumericValueCount - 1;
                }

                //If both the target nut hole cards have been identified, then we are done.
                if (nutHoleCardValue > -1)
                {
                    break;
                }
            }

            return HoleCards[0] / Table.DeckNumericValueCount == suitValue && HoleCards[0] % Table.DeckNumericValueCount == nutHoleCardValue
                || HoleCards[1] / Table.DeckNumericValueCount == suitValue && HoleCards[1] % Table.DeckNumericValueCount == nutHoleCardValue;
        }

        #endregion Flush

        #region Straight
        private int[] ScoreStraight()
        {
            //Check for sequentiality
            var consecutiveSequenceLength = 0;
            var lowestSequentialCardPosition = -1;
            var highestSequentialCardPosition = -1;
            for (short i = 0; i < _sevenCardHand.Length; i++)
            {
                var currentCard = _sevenCardHand[i];
                int nextCard = -1;

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
                if (nextCard % Table.DeckNumericValueCount == currentCard % Table.DeckNumericValueCount)
                {
                    continue;
                }
                else if (nextCard % Table.DeckNumericValueCount == currentCard % Table.DeckNumericValueCount - 1 || currentCard % Table.DeckNumericValueCount == 0 && nextCard % Table.DeckNumericValueCount == Table.DeckNumericValueCount - 1)
                {
                    if (consecutiveSequenceLength == 0)
                    {
                        consecutiveSequenceLength = 2;
                        highestSequentialCardPosition = i;
                        lowestSequentialCardPosition = i + 1;
                    }
                    else
                    {
                        consecutiveSequenceLength++;
                        lowestSequentialCardPosition = i;
                    }

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
            //Is Nuts

            var subtypeScore = 0;

            if (IsNutStraight()) subtypeScore = 1;

            //The value of this straight equals the face value of the highest card in the sequence.
            ScoreContainer[0] = 5;
            ScoreContainer[1] = _sevenCardHand[highestSequentialCardPosition] % Table.DeckNumericValueCount;
            ScoreContainer[2] = 0;
            ScoreContainer[3] = subtypeScore;
            return ScoreContainer;
        }

        public bool IsNutStraight()
        {
            int firstNutHoleCardValue = -1;
            int secondNutHoleCardValue = -1;

            for (short i = 0; i < 3; i++)
            {
                Span<bool> patternArray = stackalloc bool[] { false, false, false, false, false };

                int targetCardValue = CommunityCards[i] % Table.DeckNumericValueCount;

                //Set the target card as the first value in the pattern array
                patternArray[0] = true;

                //Iterate through the remaining community cards.
                //Each subsequent index of the array represents the next consecutive value card.
                //Build the pattern.
                for (var j = i + 1; j < CommunityCards.Length; j++)
                {
                    var relationIndex = targetCardValue - CommunityCards[j] % Table.DeckNumericValueCount;
                    if (relationIndex < 5)
                    {
                        patternArray[relationIndex] = true;
                    }
                }

                //Determine which pattern exists and return true if the 
                //corresponding nut hand matches the hole cards.
                //Return false if otherwise.                
                if (patternArray[1] && patternArray[2])
                {
                    //Case when target card is Ace
                    if (targetCardValue == Table.DeckNumericValueCount - 1)
                    {
                        //If a Jack is in the pattern, a Ten is the nuts
                        if (patternArray[3])
                        {
                            firstNutHoleCardValue = targetCardValue - 4;
                        }
                        else //Jack Ten is the nuts
                        {
                            firstNutHoleCardValue = targetCardValue - 3;
                            secondNutHoleCardValue = targetCardValue - 4;
                        }
                    }
                    //Case when target card is King
                    else if (targetCardValue == Table.DeckNumericValueCount - 2)
                    {
                        //If a Ten is in the pattern, an Ace is the nuts
                        if (patternArray[3])
                        {
                            firstNutHoleCardValue = targetCardValue + 1;
                        }
                        else //Ace Ten is the nuts
                        {
                            firstNutHoleCardValue = targetCardValue + 1;
                            secondNutHoleCardValue = targetCardValue - 3;
                        }
                    }
                    else
                    {
                        firstNutHoleCardValue = targetCardValue + 2;
                        secondNutHoleCardValue = targetCardValue + 1;
                    }
                }
                else if (patternArray[1] && patternArray[3])
                {
                    //Case when target card is Ace
                    if (targetCardValue == Table.DeckNumericValueCount - 1)
                    {
                        //Queen Ten is the nuts
                        firstNutHoleCardValue = targetCardValue - 2;
                        secondNutHoleCardValue = targetCardValue - 4;
                    }
                    else
                    {
                        firstNutHoleCardValue = targetCardValue + 1;
                        secondNutHoleCardValue = targetCardValue - 2;
                    }
                }
                else if (patternArray[1] && patternArray[4])
                {
                    firstNutHoleCardValue = targetCardValue - 2;
                    secondNutHoleCardValue = targetCardValue - 3;
                }
                else if (patternArray[2] && patternArray[3])
                {
                    //Case when target card is Ace
                    if (targetCardValue == Table.DeckNumericValueCount - 1)
                    {
                        firstNutHoleCardValue = targetCardValue - 1;
                        secondNutHoleCardValue = targetCardValue - 4;
                    }
                    else
                    {
                        firstNutHoleCardValue = targetCardValue + 1;
                        secondNutHoleCardValue = targetCardValue - 1;
                    }
                }
                else if (patternArray[2] && patternArray[4])
                {
                    firstNutHoleCardValue = targetCardValue - 1;
                    secondNutHoleCardValue = targetCardValue - 3;
                }
                else if (patternArray[3] && patternArray[4])
                {
                    firstNutHoleCardValue = targetCardValue - 1;
                    secondNutHoleCardValue = targetCardValue - 2;
                }
                else
                {
                    //No pattern found
                }
            }

            if (firstNutHoleCardValue == -1)
            {
                //No pattern found. Look for lower-end straight.
                //For there to be a lower-end nut straight that's not handled
                //by an existing pattern, there must be an Ace involved.
                if (CommunityCards[0] % Table.DeckNumericValueCount == Table.DeckNumericValueCount - 1)
                {
                    //Record the existence of Twos, Threes, fours, and Fives.
                    Span<bool> wheelContents = stackalloc bool[] { false, false, false, false };
                    var wheelCount = 0;

                    for (short i = 1; i < CommunityCards.Length; i++)
                    {
                        var communityCardValue = CommunityCards[i] % Table.DeckNumericValueCount;
                        if (communityCardValue < wheelContents.Length)
                        {
                            if (!wheelContents[communityCardValue])
                            {
                                wheelCount++;
                                wheelContents[communityCardValue] = true;
                            }
                        }
                    }

                    if (wheelCount == 2)
                    {
                        for (int i = wheelContents.Length - 1; i >= 0; i--)
                        {

                            if (!wheelContents[i])
                            {
                                if (firstNutHoleCardValue == -1)
                                {
                                    firstNutHoleCardValue = i;
                                }
                                else if (secondNutHoleCardValue == -1)
                                {
                                    secondNutHoleCardValue = i;
                                }
                            }
                        }
                    }
                }
            }

            //Check if the hole cards contain the nuts.
            if (firstNutHoleCardValue != -1)
            {
                if (secondNutHoleCardValue != -1)
                {
                    return HoleCards[0] % Table.DeckNumericValueCount == firstNutHoleCardValue
                        && HoleCards[1] % Table.DeckNumericValueCount == secondNutHoleCardValue;
                }
                else
                {
                    return HoleCards[0] % Table.DeckNumericValueCount == firstNutHoleCardValue
                        || HoleCards[1] % Table.DeckNumericValueCount == firstNutHoleCardValue;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion

        private int[] ScoreThreeOfAKind()
        {
            int biggestSetCardValue = -1;

            short consecutiveValueLength = 0;
            for (short i = 0; i < _sevenCardHand.Length - 1; i++)
            {
                int currentCard = _sevenCardHand[i];
                int nextCard = _sevenCardHand[i + 1];

                //Detect consecutive identical card values.
                if (currentCard % Table.DeckNumericValueCount == nextCard % Table.DeckNumericValueCount)
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
                            if (currentCard % Table.DeckNumericValueCount > biggestSetCardValue)
                            {
                                biggestSetCardValue = currentCard % Table.DeckNumericValueCount;
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

            Span<int> kickerValues = stackalloc int[2];
            short pos = 0;
            short kickerCount = 0;
            while (kickerCount < 2)
            {
                var currentCardValue = _sevenCardHand[pos] % Table.DeckNumericValueCount;

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
            //Second Set //Not necessary. Merged with Low Set.
            //Low Set
            //Board Set

            short subtypeScore = 0;

            //Check for Board Set subtype.
            if (
                (
                    CommunityCards[0] % Table.DeckNumericValueCount == CommunityCards[1] % Table.DeckNumericValueCount
                    && CommunityCards[1] % Table.DeckNumericValueCount == CommunityCards[2] % Table.DeckNumericValueCount
                )
                || (
                    CommunityCards[1] % Table.DeckNumericValueCount == CommunityCards[2] % Table.DeckNumericValueCount
                    && CommunityCards[2] % Table.DeckNumericValueCount == CommunityCards[3] % Table.DeckNumericValueCount
                )
                || (
                    CommunityCards[2] % Table.DeckNumericValueCount == CommunityCards[3] % Table.DeckNumericValueCount
                    && CommunityCards[3] % Table.DeckNumericValueCount == CommunityCards[4] % Table.DeckNumericValueCount
                )
            )
            {
                subtypeScore = 1; //Board Set subtype.
            }

            //Check for Low Set subtype
            if (subtypeScore == 0
                && biggestSetCardValue < CommunityCards[0] % Table.DeckNumericValueCount
            )
            {
                subtypeScore = 2; //Low Set Subtype
            }

            //Check for Top Set subtype
            if (subtypeScore == 0
                && biggestSetCardValue == CommunityCards[0] % Table.DeckNumericValueCount
            )
            {
                subtypeScore = 3; //Top Set subtype
            }

            ScoreContainer[0] = 4;
            ScoreContainer[1] = biggestSetCardValue + 1;
            ScoreContainer[2] = kickerVal;
            ScoreContainer[3] = subtypeScore;
            return ScoreContainer;
        }

        private int[] ScoreTwoPair()
        {
            //Find the biggest pair
            int biggestPairCardValue = -1;
            for (int i = 0; i < _sevenCardHand.Length - 1; i++)
            {
                var currentCard = _sevenCardHand[i];
                int nextCard = _sevenCardHand[i + 1];

                //Detect consecutive identical card values.
                if (currentCard % Table.DeckNumericValueCount == nextCard % Table.DeckNumericValueCount)
                {
                    //Keep the highest pair value.
                    if (currentCard % Table.DeckNumericValueCount > biggestPairCardValue)
                    {
                        biggestPairCardValue = currentCard % Table.DeckNumericValueCount;
                    }

                    //Move to the next card. The next iteration should be outside the newly-found pair.
                    i++;
                }
            }

            //If a pair doesn't exist, then the poker hand cannot be a two pair.
            if (biggestPairCardValue == -1) return ZeroScore;

            //Find the value of the smallest pair
            int smallestPairCardValue = -1;
            for (int i = 0; i < _sevenCardHand.Length - 1; i++)
            {
                var currentCard = _sevenCardHand[i];
                int nextCard = _sevenCardHand[i + 1];

                //skip the cards that have a value that match the biggest set's value.
                if (currentCard % Table.DeckNumericValueCount == biggestPairCardValue) continue;

                //Detect consecutive identical card values.
                if (currentCard % Table.DeckNumericValueCount == nextCard % Table.DeckNumericValueCount)
                {
                    //Keep the highest pair value.
                    if (currentCard % Table.DeckNumericValueCount > smallestPairCardValue)
                    {
                        smallestPairCardValue = currentCard % Table.DeckNumericValueCount;
                    }

                    //Once a second pair is found, we can break out of the loop.
                    break;
                }
            }

            //If a second pair doesn't exist, then the hand cannot be a two pair.
            if (smallestPairCardValue == -1) return ZeroScore;

            //Calculate the kicker score
            int kickerValue = 0;
            short pos = 0;
            short kickerCount = 0;
            while (kickerCount < 1)
            {
                var currentCardValue = _sevenCardHand[pos] % Table.DeckNumericValueCount;

                if (currentCardValue != biggestPairCardValue && currentCardValue != smallestPairCardValue)
                {
                    kickerValue = currentCardValue;
                    kickerCount++;
                }

                pos++;
            }

            int kickerVal = kickerValue;

            //Determine subtype:
            //Top Two
            //Over Two Pair //Not necessary. Merged with Top Two Pair.
            //Highest Top
            //Second Top Two //Not sufficiently defined or necessary. Merged with Low Two Pair.
            //Low Two Pair
            //Under Two Pair //Not necessary. Merged with Low Two Pair.
            //Board Two Pair

            short subtypeScore = 0;

            //Check for Board Two Pair subtype.
            if (
                (
                    CommunityCards[0] % Table.DeckNumericValueCount == CommunityCards[1] % Table.DeckNumericValueCount && CommunityCards[2] % Table.DeckNumericValueCount == CommunityCards[3] % Table.DeckNumericValueCount
                    || CommunityCards[0] % Table.DeckNumericValueCount == CommunityCards[1] % Table.DeckNumericValueCount && CommunityCards[3] % Table.DeckNumericValueCount == CommunityCards[4] % Table.DeckNumericValueCount
                    || CommunityCards[1] % Table.DeckNumericValueCount == CommunityCards[2] % Table.DeckNumericValueCount && CommunityCards[3] % Table.DeckNumericValueCount == CommunityCards[4] % Table.DeckNumericValueCount
                )
                && (
                    (
                        HoleCards[0] % Table.DeckNumericValueCount != HoleCards[1] % Table.DeckNumericValueCount
                        && HoleCards[0] % Table.DeckNumericValueCount != CommunityCards[0] % Table.DeckNumericValueCount
                        && HoleCards[1] % Table.DeckNumericValueCount != CommunityCards[0] % Table.DeckNumericValueCount
                        && HoleCards[0] % Table.DeckNumericValueCount != CommunityCards[2] % Table.DeckNumericValueCount
                        && HoleCards[1] % Table.DeckNumericValueCount != CommunityCards[2] % Table.DeckNumericValueCount
                    )
                    || (
                        HoleCards[0] % Table.DeckNumericValueCount == HoleCards[1] % Table.DeckNumericValueCount
                        && HoleCards[0] % Table.DeckNumericValueCount < CommunityCards[3] % Table.DeckNumericValueCount
                    )
                )
            )
            {
                subtypeScore = 1; //Board Two Pair subtype.
            }

            //Check for Low Two Pair subtype.
            if (subtypeScore == 0
                && (
                    (
                        HoleCards[0] % Table.DeckNumericValueCount != HoleCards[1] % Table.DeckNumericValueCount
                        && HoleCards[0] % Table.DeckNumericValueCount != CommunityCards[0] % Table.DeckNumericValueCount
                    )
                    || (
                        HoleCards[0] % Table.DeckNumericValueCount == HoleCards[1] % Table.DeckNumericValueCount
                        && HoleCards[0] % Table.DeckNumericValueCount < CommunityCards[2] % Table.DeckNumericValueCount
                    )
                )
            )
            {
                subtypeScore = 2; //Low Two Pair subtype.
            }

            //Check for Highest Top subtype.
            if (subtypeScore == 0
                && (
                    (
                        HoleCards[0] % Table.DeckNumericValueCount != HoleCards[1] % Table.DeckNumericValueCount
                        && (
                            (
                                CommunityCards[0] % Table.DeckNumericValueCount == CommunityCards[1] % Table.DeckNumericValueCount
                                && HoleCards[0] % Table.DeckNumericValueCount != CommunityCards[2] % Table.DeckNumericValueCount
                                && HoleCards[1] % Table.DeckNumericValueCount != CommunityCards[2] % Table.DeckNumericValueCount
                            )
                            || (
                                CommunityCards[0] % Table.DeckNumericValueCount != CommunityCards[1] % Table.DeckNumericValueCount
                                && (
                                    (
                                        HoleCards[0] % Table.DeckNumericValueCount == CommunityCards[0] % Table.DeckNumericValueCount
                                        && CommunityCards[1] != CommunityCards[2]
                                        && HoleCards[1] != CommunityCards[1]
                                    )
                                    || (
                                        HoleCards[1] % Table.DeckNumericValueCount == CommunityCards[0] % Table.DeckNumericValueCount
                                        && CommunityCards[1] != CommunityCards[2]
                                    )
                                )
                            )
                        )
                    )
                    || (
                        HoleCards[0] % Table.DeckNumericValueCount == HoleCards[1] % Table.DeckNumericValueCount
                        && (
                            (
                                CommunityCards[0] % Table.DeckNumericValueCount == CommunityCards[1] % Table.DeckNumericValueCount
                                && HoleCards[0] % Table.DeckNumericValueCount < CommunityCards[2] % Table.DeckNumericValueCount
                            )
                            || (
                                CommunityCards[0] % Table.DeckNumericValueCount != CommunityCards[1] % Table.DeckNumericValueCount
                                && HoleCards[0] % Table.DeckNumericValueCount > CommunityCards[0] % Table.DeckNumericValueCount
                            )
                        )
                    )
                )
            )
            {
                subtypeScore = 3; // Highest Top subtype.
            }

            //Check for Top Two Pair subtype.
            if (subtypeScore == 0
                && (
                    (
                        HoleCards[0] % Table.DeckNumericValueCount != HoleCards[1] % Table.DeckNumericValueCount
                        && HoleCards[0] % Table.DeckNumericValueCount == CommunityCards[0] % Table.DeckNumericValueCount
                        && HoleCards[1] % Table.DeckNumericValueCount == CommunityCards[0] % Table.DeckNumericValueCount
                    )
                    || (
                        HoleCards[0] % Table.DeckNumericValueCount == HoleCards[1] % Table.DeckNumericValueCount
                        && CommunityCards[0] % Table.DeckNumericValueCount == CommunityCards[1] % Table.DeckNumericValueCount
                        && HoleCards[0] % Table.DeckNumericValueCount > CommunityCards[2] % Table.DeckNumericValueCount
                    )
                )
            )
            {
                subtypeScore = 4;
            }

            ScoreContainer[0] = 3;
            ScoreContainer[1] = biggestPairCardValue * Table.DeckNumericValueCount + smallestPairCardValue;
            ScoreContainer[2] = kickerVal;
            ScoreContainer[3] = subtypeScore;
            return ScoreContainer;
        }

        private int[] ScorePair()
        {
            //Find the value of the smallest pair
            int pairCardValue = -1;
            for (int i = 0; i < _sevenCardHand.Length - 1; i++)
            {
                var currentCard = _sevenCardHand[i];
                int nextCard = _sevenCardHand[i + 1];

                //Detect consecutive identical card values.
                if (currentCard % Table.DeckNumericValueCount == nextCard % Table.DeckNumericValueCount)
                {
                    //Keep the highest pair value.
                    if (currentCard % Table.DeckNumericValueCount > pairCardValue)
                    {
                        pairCardValue = currentCard % Table.DeckNumericValueCount;
                    }

                    //Once a second pair is found, we can break out of the loop.
                    break;
                }
            }

            if (pairCardValue == -1) return ZeroScore;

            Span<int> kickerValues = stackalloc int[3];
            short pos = 0;
            int kickerCount = 0;
            while (kickerCount < 3)
            {
                var currentCardValue = _sevenCardHand[pos] % Table.DeckNumericValueCount;

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
                CommunityCards[0] % Table.DeckNumericValueCount == CommunityCards[1] % Table.DeckNumericValueCount
                || CommunityCards[1] % Table.DeckNumericValueCount == CommunityCards[2] % Table.DeckNumericValueCount
                || CommunityCards[2] % Table.DeckNumericValueCount == CommunityCards[3] % Table.DeckNumericValueCount
                || CommunityCards[3] % Table.DeckNumericValueCount == CommunityCards[4] % Table.DeckNumericValueCount
            )
            {
                subtypeScore = 1; //Underpair subtype.
            }

            //Check for Underpair subtype.
            if (subtypeScore == 0
                && HoleCards[0] % Table.DeckNumericValueCount == HoleCards[1] % Table.DeckNumericValueCount
                && HoleCards[0] % Table.DeckNumericValueCount < CommunityCards[4] % Table.DeckNumericValueCount)
            {
                subtypeScore = 2; //Underpair subtype.
            }

            //Check for Low Pair subtype.
            if (subtypeScore == 0
                && (
                    HoleCards[0] % Table.DeckNumericValueCount != HoleCards[1] % Table.DeckNumericValueCount
                    && (
                        HoleCards[0] % Table.DeckNumericValueCount == CommunityCards[4] % Table.DeckNumericValueCount
                        || HoleCards[0] % Table.DeckNumericValueCount == CommunityCards[3] % Table.DeckNumericValueCount
                        || HoleCards[0] % Table.DeckNumericValueCount == CommunityCards[2] % Table.DeckNumericValueCount
                        || HoleCards[0] % Table.DeckNumericValueCount == CommunityCards[1] % Table.DeckNumericValueCount
                        || HoleCards[1] % Table.DeckNumericValueCount == CommunityCards[4] % Table.DeckNumericValueCount
                        || HoleCards[1] % Table.DeckNumericValueCount == CommunityCards[3] % Table.DeckNumericValueCount
                        || HoleCards[1] % Table.DeckNumericValueCount == CommunityCards[2] % Table.DeckNumericValueCount
                        || HoleCards[1] % Table.DeckNumericValueCount == CommunityCards[1] % Table.DeckNumericValueCount
                    )
                )
                || (
                    HoleCards[0] % Table.DeckNumericValueCount == HoleCards[1] % Table.DeckNumericValueCount
                    && (
                        HoleCards[0] % Table.DeckNumericValueCount > CommunityCards[4] % Table.DeckNumericValueCount
                        && HoleCards[0] % Table.DeckNumericValueCount < CommunityCards[0] % Table.DeckNumericValueCount
                    )
                )
            )
            {
                subtypeScore = 3; //Low Pair subtype.
            }

            //Check for Top Pair subtype
            if (subtypeScore == 0
                && HoleCards[0] % Table.DeckNumericValueCount != HoleCards[1] % Table.DeckNumericValueCount
                && (
                    HoleCards[0] % Table.DeckNumericValueCount == CommunityCards[0] % Table.DeckNumericValueCount
                    || HoleCards[1] % Table.DeckNumericValueCount == CommunityCards[0] % Table.DeckNumericValueCount
                )
            )
            {
                subtypeScore = 4; //Top Pair subtype.
            }

            //Check for Overpair subtype
            if (subtypeScore == 0
                && HoleCards[0] % Table.DeckNumericValueCount == HoleCards[1] % Table.DeckNumericValueCount
                && HoleCards[0] % Table.DeckNumericValueCount > CommunityCards[0] % Table.DeckNumericValueCount
            )
            {
                subtypeScore = 5; //Overpair subtype.
            }

            ScoreContainer[0] = 2;
            ScoreContainer[1] = pairCardValue + 1;
            ScoreContainer[2] = kickerVal;
            ScoreContainer[3] = subtypeScore;
            return ScoreContainer;
        }

        private int[] ScoreHighCard()
        {
            var highestValueCard = _sevenCardHand[0];

            var kickerVal =
                Table.DeckNumericValueCount * Table.DeckNumericValueCount * Table.DeckNumericValueCount * _sevenCardHand[1] % Table.DeckNumericValueCount
                + Table.DeckNumericValueCount * Table.DeckNumericValueCount * _sevenCardHand[2] % Table.DeckNumericValueCount
                + Table.DeckNumericValueCount * _sevenCardHand[3] % Table.DeckNumericValueCount
                + _sevenCardHand[4] % Table.DeckNumericValueCount;

            ScoreContainer[0] = 1;
            ScoreContainer[1] = highestValueCard % Table.DeckNumericValueCount;
            ScoreContainer[2] = kickerVal;
            ScoreContainer[3] = 0; //Empty subscore type
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

            return $"{CardValues[biggestCardValue]}{CardValues[smallestCardValue]}{(suited ? "s" : "o")}";
        }

        public static int ConvertCardStringToNumericValue(string cardString, Table table)
        {
            if (string.IsNullOrEmpty(cardString)) return 0;

            var holeCardProperties = cardString.ToCharArray();

            if (holeCardProperties.Length != 3) return 0;

            var biggestCardValue = Array.IndexOf(CardValues, holeCardProperties[0].ToString());
            var smallestCardValue = Array.IndexOf(CardValues, holeCardProperties[1].ToString());
            var areSuited = holeCardProperties[2] == 's';

            if (biggestCardValue == -1 || smallestCardValue == -1) return 0;

            var holeCardsNumericRepresentation = ConvertHoleCardsToNumericValue(biggestCardValue, smallestCardValue, areSuited, table);
            return holeCardsNumericRepresentation;
        }
    }
}
