using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace ShortDeckStatistics
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WindowHeight = 100;

            var table = new Table(9);
            table.PlayHands(100_000_000);

            Console.WriteLine(table);

            Console.ReadKey();
        }
    }

    public class Card : IComparable<Card>
    {
        public short Value { get; private set; }
        public short Suit { get; private set; }

        public static readonly string[] CardValues = new string[] {"6", "7", "8", "9", "T", "J", "Q", "K", "A" };
        public static readonly string[] CardSuits = new string[] { "s", "c", "h", "d" };

        public Card(short suit, short value)
        {
            Suit = suit;
            Value = value;
        }

        public override string ToString()
        {
            return $"{CardValues[Value]}{CardSuits[Suit]}";
        }

        public int CompareTo(Card other)
        {
            if (other == null) return 1;

            return this.Value.CompareTo(other.Value);
        }
    }

    public class PokerHand
    {
        private Card[] _sevenCardHand;

        public Card[] HoleCards { get; private set; }
        public Card[] CommunityCards { get; private set; }

        private long _handRank = -1L;
        public long HandRank
        {
            get
            {
                if(_handRank == -1L)
                {
                    _handRank = RankHand();
                }

                return _handRank;
            }
        }

        public PokerHand(Card[] holeCards, Card[] communityCards)
        {
            HoleCards = holeCards;
            CommunityCards = communityCards;

            _sevenCardHand = new Card[7];
            Array.Copy(HoleCards, _sevenCardHand, HoleCards.Length);
            Array.Copy(CommunityCards, 0, _sevenCardHand, HoleCards.Length, CommunityCards.Length);
            Array.Sort(_sevenCardHand);
            Array.Reverse(_sevenCardHand);
        }

        public override string ToString()
        {
            return $"{HoleCards[0]} {HoleCards[1]} - {CommunityCards[0]} {CommunityCards[1]} {CommunityCards[2]} {CommunityCards[3]} {CommunityCards[4]} - Hand Score: {HandRank}";
        }

        private long RankHand()
        {
            var scoreData = ScoreStraightFlush();
            if (scoreData[0] == 0) scoreData = ScoreFourOfAKind();
            if (scoreData[0] == 0) scoreData = ScoreFlush();
            if (scoreData[0] == 0) scoreData = ScoreFullHouse();
            if (scoreData[0] == 0) scoreData = ScoreThreeOfAKind();
            if (scoreData[0] == 0) scoreData = ScoreStraight();
            if (scoreData[0] == 0) scoreData = ScoreTwoPair();
            if (scoreData[0] == 0) scoreData = ScorePair();
            if (scoreData[0] == 0) scoreData = ScoreHighCard();

            var score =
                1L * scoreData[2]
                + 100_000_000L * scoreData[1]
                + 100_000_000_000L * scoreData[0];

            return score;
        }

        private int[] ScoreStraightFlush()
        {
            //Check for sequentiality
            var consecutiveSequenceLength = 0;
            var highestSequentialCardPosition = -1;
            for (int i = 0; i < _sevenCardHand.Length; i++)
            {
                var currentCard = _sevenCardHand[i];
                Card nextCard = null;

                //Populate the next card, taking into consideration that it may be an Ace in the first position.
                if(i < _sevenCardHand.Length - 1)
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
            if (consecutiveSequenceLength < 5) return new int[] { 0, 0, 0 };

            //Check for suitedness.
            var allSuited = true;
            for (int i = highestSequentialCardPosition; i < highestSequentialCardPosition + 4; i++)
            {
                var currentCard = _sevenCardHand[i];
                Card nextCard = null; 

                //Populate the next card, taking into consideration that it may be an Ace.
                if (i == _sevenCardHand.Length - 1)
                {
                    var firstCard = _sevenCardHand[0];
                    var secondCard = _sevenCardHand[1];
                    var thirdCard = _sevenCardHand[2];

                    if (firstCard.Value == 8 && firstCard.Suit == currentCard.Suit) nextCard = firstCard;
                    else if (secondCard.Value == 8 && secondCard.Suit == currentCard.Suit) nextCard = secondCard;
                    else if (thirdCard.Value == 8 && thirdCard.Suit == currentCard.Suit) nextCard = thirdCard;
                    else
                    {
                        allSuited = false;
                        break;
                    }
                }
                else
                {
                    nextCard = _sevenCardHand[i + 1];
                }

                allSuited = currentCard.Suit == nextCard.Suit;

                if (!allSuited) break;
            }

            //If the 5 consecutive cards are not suited, then the poker hand cannot be a straight flush.
            if (!allSuited) return new int[] { 0, 0, 0 };

            //The value of this straight flush equals the face value of the highest card in the sequence.
            return new int[] { 9, _sevenCardHand[highestSequentialCardPosition].Value + 1, 0 };
        }

        private int[] ScoreFourOfAKind()
        {
            var consecutiveValueLength = 0;
            var fourOfAKindCardValue = -1;
            for (int i = 0; i < _sevenCardHand.Length - 1; i++)
            {
                var currentCard = _sevenCardHand[i];
                Card nextCard = _sevenCardHand[i + 1];

                if(currentCard.Value == nextCard.Value)
                {
                    if(consecutiveValueLength == 0)
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
            if (consecutiveValueLength < 4) return new int[] { 0, 0, 0 };

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


            return new int[] { 8, fourOfAKindCardValue + 1, kickerVal };
        }

        private int[] ScoreFlush()
        {
            var suitCounter = new int[4];
            for(int pos = 0; pos < _sevenCardHand.Length; pos++)
            {
                var card = _sevenCardHand[pos];
                suitCounter[card.Suit]++;
            }

            var flushedSuit = -1;
            for (int suit = 0; suit < suitCounter.Length; suit++)
            {
                if (suitCounter[suit] >= 5)
                {
                    flushedSuit = suit;
                    break;
                }
            }

            if (flushedSuit == -1) return new int[] { 0, 0, 0 };

            var highestFlushValue = -1;
            var flushKickers = new Card[4];
            var flushKickerCount = 0;
            for(int pos = 0; pos < _sevenCardHand.Length; pos++)
            {
                var card = _sevenCardHand[pos];
                if (card.Suit == flushedSuit)
                {
                    if(highestFlushValue == -1)
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
                1_000_000 * flushKickers[0].Value
                + 10_000 * flushKickers[1].Value
                + 100 * flushKickers[2].Value
                + 1 * flushKickers[3].Value;

            return new int[] { 7, highestFlushValue, kickerVal };
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
            if (biggestSetCardValue == -1) return new int[] { 0, 0, 0 };

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
                    if(currentCard.Value > biggestPairCardValue)
                    {
                        biggestPairCardValue = currentCard.Value;
                    }

                    //Move to the next card. The next iteration should be outside the newly-found pair.
                    i++;
                }
            }

            //If a pair doesn't exist, then the poker hand cannot be a full house.
            if (biggestPairCardValue == -1) return new int[] { 0, 0, 0 };

            return new int[] { 6, biggestSetCardValue * 9 + biggestPairCardValue, 0 };
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

            if (biggestSetCardValue == -1) return new int[] { 0, 0, 0 };

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
                100 * kickerValues[0]
                + 1 * kickerValues[1];

            return new int[] { 5, biggestSetCardValue + 1, kickerVal };
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
            if (consecutiveSequenceLength < 5) return new int[] { 0, 0, 0 };

            //The value of this straight equals the face value of the highest card in the sequence.
            return new int[] { 4, _sevenCardHand[highestSequentialCardPosition].Value, 0 };
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
            if (biggestPairCardValue == -1) return new int[] { 0, 0, 0 };

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
            if (smallestPairCardValue == -1) return new int[] { 0, 0, 0 };

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

            return new int[] { 3, biggestPairCardValue * 9 + smallestPairCardValue, kickerVal };
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

            if (pairCardValue == -1) return new int[] { 0, 0, 0 };

            var kickerValues = new short[3];
            var pos = 0;
            var kickerCount = 0;
            while(kickerCount < 3)
            {
                var currentCardValue = _sevenCardHand[pos].Value;

                if(currentCardValue != pairCardValue)
                {
                    kickerValues[kickerCount] = currentCardValue;
                    kickerCount++;
                }

                pos++;
            }

            var kickerVal =
                10_000 * kickerValues[0]
                + 100 * kickerValues[1]
                + 1 * kickerValues[2];

            return new int[] { 2, pairCardValue, kickerVal };
        }

        private int[] ScoreHighCard()
        {
            var highestValueCard = _sevenCardHand[0];

            var kickerVal = 
                1_000_000 * _sevenCardHand[1].Value
                + 10_000 * _sevenCardHand[2].Value
                + 100 * _sevenCardHand[3].Value
                + 1 * _sevenCardHand[4].Value;

            return new int[] { 1, highestValueCard.Value, kickerVal };
        }
    }

    public class Table
    {
        private Random numberGenerator;
        private long[][] HandWinTracker;
        private long[][] HandPlayTracker;
        private Card[] Deck;

        public int PlayerCount { get; private set; }

        public Table(int numPlayers)
        {
            PlayerCount = numPlayers;
            numberGenerator = new Random((int)DateTime.Now.Ticks);

            HandWinTracker = new long[9][];
            for(int i = 0; i < HandWinTracker.Length; i++)
            {
                HandWinTracker[i] = new long[9];
            }

            HandPlayTracker = new long[9][];
            for (int i = 0; i < HandPlayTracker.Length; i++)
            {
                HandPlayTracker[i] = new long[9];
            }

            //Populate the deck
            Deck = new Card[36];
            for (short suit = 0; suit < 4; suit++)
            {
                for (short value = 0; value < 9; value++)
                {
                    int slot = suit * 9 + value;
                    Deck[slot] = new Card(suit, value);
                }
            }
        }

        public void PlayHand()
        {
            var players = new Card[PlayerCount][];
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new Card[2];
            }

            var communityCards = new Card[5];

            var shuffledDeck = ShuffleDeck();

            //Deal cards
            for (int i = 0; i < players.Length * players[0].Length; i++)
            {
                var player = i % players.Length;
                var holeCard = i / players.Length;

                players[player][holeCard] = shuffledDeck[i];
            }

            //Populate community cards
            var cardsDealt = players.Length * players[0].Length;
            Array.Copy(shuffledDeck, cardsDealt, communityCards, 0, 5);


            //Generate the 7-card poker hands.
            var handResults = new PokerHand[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                handResults[i] = new PokerHand(players[i], communityCards);
            }

            Card biggestHoleCard;
            Card smallestHoleCard;

            //Log each hand's play.
            foreach(var hand in handResults)
            {
                biggestHoleCard = hand.HoleCards[0];
                smallestHoleCard = hand.HoleCards[1];
                if (biggestHoleCard.Value < smallestHoleCard.Value)
                {
                    Card temp = biggestHoleCard;
                    biggestHoleCard = smallestHoleCard;
                    smallestHoleCard = temp;
                }

                if (biggestHoleCard.Suit == smallestHoleCard.Suit)
                {
                    HandPlayTracker[biggestHoleCard.Value][smallestHoleCard.Value]++;
                }
                else
                {
                    HandPlayTracker[smallestHoleCard.Value][biggestHoleCard.Value]++;
                }
            }

            PokerHand strongestHand = null;
            foreach(var result in handResults)
            {
                if(strongestHand == null || result.HandRank > strongestHand.HandRank)
                {
                    strongestHand = result;
                }
            }

            biggestHoleCard = strongestHand.HoleCards[0];
            smallestHoleCard = strongestHand.HoleCards[1];
            if(biggestHoleCard.Value < smallestHoleCard.Value)
            {
                Card temp = biggestHoleCard;
                biggestHoleCard = smallestHoleCard;
                smallestHoleCard = temp;
            }

            if(biggestHoleCard.Suit == smallestHoleCard.Suit)
            {
                HandWinTracker[biggestHoleCard.Value][smallestHoleCard.Value]++;
            }
            else
            {
                HandWinTracker[smallestHoleCard.Value][biggestHoleCard.Value]++;
            }
        }

        public void PlayHands(long iterations)
        {
            for(int i = 0; i < iterations; i++)
            {
                PlayHand();

                if(i % 100_000 == 0)
                {
                    Console.WriteLine($"{i} hands evaluated");
                }
            }
        }

        public override string ToString()
        {
            var dictionary = new Dictionary<string, double>();

            for(int i = 0; i < HandWinTracker.Length; i++)
            {
                for(int j = i; j < HandWinTracker[0].Length; j++)
                {
                    var winRate = HandPlayTracker[i][j] == 0 ? 0 : (double)HandWinTracker[i][j] / HandPlayTracker[i][j];
                    dictionary.Add($"{Card.CardValues[j]}{Card.CardValues[i]}o", winRate);
                }
            }

            for (int i = 0; i < HandWinTracker.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    var winRate = HandPlayTracker[i][j] == 0 ? 0 : (double)HandWinTracker[i][j] / HandPlayTracker[i][j];
                    dictionary.Add($"{Card.CardValues[i]}{Card.CardValues[j]}s", winRate);
                }
            }

            StringBuilder sb = new StringBuilder();

            var cardArray = new string[dictionary.Count];
            var winRateArray = new double[dictionary.Count];
            dictionary.Keys.CopyTo(cardArray, 0);
            dictionary.Values.CopyTo(winRateArray, 0);
            Array.Sort(winRateArray, cardArray);
            for(int i = 0; i < cardArray.Length; i++)
            {
                sb.AppendLine($"{cardArray[i].PadRight(5)} - {winRateArray[i]}");
            }

            return sb.ToString();
        }

        private Card[] ShuffleDeck()
        {
            //Shuffle the deck.
            var cardIndexPosition = Deck.Length;
            while (cardIndexPosition > 1)
            {
                cardIndexPosition--;
                int cardPositionToSwap = numberGenerator.Next(Deck.Length);
                var swapCard = Deck[cardPositionToSwap];
                Deck[cardPositionToSwap] = Deck[cardIndexPosition];
                Deck[cardIndexPosition] = swapCard;
            }

            return Deck;
        }
    }
}
