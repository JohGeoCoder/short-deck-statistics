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
            Console.WindowHeight = 50;
            Console.WindowWidth = 400;

            var tableArray = new Table[3];
            for(int i = 2; i < tableArray.Length; i++)
            {
                tableArray[i] = new Table(6, true);
                tableArray[i].PlayHands(1_000_000, true, 20, 30);
            }

            for(int i = 2; i < tableArray.Length; i++)
            {
                //tableArray[i].PrintHoleCardWinRatesRankedByBest();
                //tableArray[i].PrintWinRatesForPokerHandsMade();
                tableArray[i].PrintHoleCardsNumericRankedByBestForArray();
            }

            Console.ReadKey();
        }
    }

    public class Card : IComparable<Card>
    {
        public readonly short Value;
        public readonly short Suit;

        private string _cardString;

        public static readonly string[] CardValues = new string[] {"6", "7", "8", "9", "T", "J", "Q", "K", "A" };
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
            if (other == null) return 1;

            if (this.Value < other.Value) return -1;
            else if (this.Value > other.Value) return 1;
            else return 0;
        }
    }

    public class PokerHand
    {
        public static readonly int[] ZeroScore = new int[] { 0, 0, 0 };
        public static readonly int[] ScoreContainer = new int[3];

        public static readonly string[] HandRanks = new string[] { "Error", "High Card", "Pair", "Two Pair", "Straight", "Three of a Kind", "Full House", "Flush", "Four of a Kind", "Straight Flush" };
        public static readonly Dictionary<int, string> HoleCardRepresentations = new Dictionary<int, string>();

        public static readonly string[][] BestHoleCardsByPlayerCount = new string[][]
        {
            new string[] { }, //Zero players
            new string[] { }, //One player
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "AKs", "AKo", "AQs", "AJs", "AQo", "99o", "ATs", "KQs", "AJo", "ATo", "KTs", "KQo", "KJs", "A9s", "KJo", "A8s", "QJs", "A9o", "KTo", "QTs", "QJo", "K9s", "88o", "A8o", "A7s", "QTo", "JTs", "Q9s", "K9o", "K8s", "A6s", "A7o", "JTo", "K7s", "Q9o", "Q8s", "J9s", "A6o", "K8o", "T9s", "77o", "K6s", "K7o", "J9o", "Q8o", "Q7s", "J8s", "T8s", "T9o", "Q6s", "K6o", "J8o", "J7s", "98s", "Q7o", "66o", "T8o", "J6s", "T7s", "Q6o", "97s", "J7o", "98o", "T7o", "T6s", "87s", "J6o", "97o", "96s", "T6o", "86s", "96o", "87o", "76s", "86o", "76o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "AKs", "AQs", "AKo", "AJs", "AQo", "KQs", "99o", "ATs", "KJs", "AJo", "KQo", "A9s", "KTs", "ATo", "QJs", "A8s", "KJo", "QTs", "88o", "JTs", "KTo", "A9o", "QJo", "K9s", "A7s", "QTo", "A6s", "A8o", "Q9s", "JTo", "K8s", "77o", "K9o", "A7o", "K7s", "J9s", "Q8s", "T9s", "Q9o", "A6o", "K6s", "J8s", "K8o", "J9o", "Q7s", "T8s", "66o", "K7o", "Q8o", "T9o", "J7s", "Q6s", "98s", "T7s", "K6o", "J8o", "T8o", "97s", "J6s", "Q7o", "T6s", "87s", "Q6o", "J7o", "98o", "96s", "T7o", "86s", "97o", "J6o", "T6o", "76s", "96o", "87o", "86o", "76o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "AKs", "AKo", "AQs", "99o", "AJs", "KQs", "AQo", "ATs", "KJs", "KTs", "AJo", "KQo", "QJs", "A9s", "88o", "QTs", "ATo", "A8s", "KJo", "JTs", "77o", "KTo", "K9s", "A7s", "A9o", "QJo", "Q9s", "QTo", "66o", "A6s", "K8s", "J9s", "A8o", "T9s", "JTo", "K7s", "K9o", "Q8s", "A7o", "J8s", "T8s", "K6s", "Q9o", "Q7s", "A6o", "K8o", "J7s", "Q6s", "98s", "J9o", "T7s", "T9o", "K7o", "Q8o", "97s", "J6s", "J8o", "T6s", "K6o", "96s", "T8o", "87s", "Q7o", "J7o", "98o", "T7o", "86s", "Q6o", "97o", "76s", "J6o", "T6o", "87o", "96o", "86o", "76o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "AKs", "99o", "AQs", "AKo", "KQs", "AJs", "88o", "ATs", "AQo", "KJs", "77o", "KTs", "QJs", "QTs", "KQo", "AJo", "A9s", "66o", "JTs", "ATo", "A8s", "KJo", "A7s", "K9s", "KTo", "Q9s", "QJo", "A6s", "J9s", "T9s", "A9o", "K8s", "QTo", "K7s", "Q8s", "JTo", "T8s", "K6s", "A8o", "J8s", "K9o", "Q7s", "A7o", "J7s", "98s", "T7s", "Q6s", "Q9o", "97s", "T9o", "A6o", "J9o", "J6s", "K8o", "T6s", "87s", "K7o", "Q8o", "96s", "J8o", "86s", "T8o", "K6o", "Q7o", "76s", "T7o", "J7o", "98o", "Q6o", "97o", "T6o", "J6o", "96o", "87o", "86o", "76o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "99o", "AKs", "88o", "AQs", "77o", "AKo", "KQs", "AJs", "66o", "KJs", "ATs", "AQo", "KTs", "QJs", "JTs", "A9s", "QTs", "KQo", "A8s", "AJo", "K9s", "ATo", "A7s", "KJo", "A6s", "J9s", "KTo", "Q9s", "T9s", "QJo", "K8s", "K7s", "T8s", "Q8s", "QTo", "J8s", "A9o", "K6s", "JTo", "98s", "T7s", "Q7s", "A8o", "J7s", "Q6s", "K9o", "97s", "J6s", "A7o", "T6s", "T9o", "96s", "J9o", "Q9o", "87s", "A6o", "86s", "K8o", "Q8o", "K7o", "T8o", "J8o", "76s", "K6o", "Q7o", "T7o", "J7o", "98o", "Q6o", "97o", "T6o", "J6o", "87o", "96o", "86o", "76o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "99o", "88o", "AKs", "77o", "66o", "AQs", "KQs", "AJs", "AKo", "KJs", "ATs", "QJs", "KTs", "AQo", "QTs", "JTs", "A9s", "KQo", "AJo", "A8s", "K9s", "A7s", "T9s", "Q9s", "J9s", "ATo", "K8s", "A6s", "KJo", "QJo", "Q8s", "J8s", "K7s", "KTo", "T8s", "QTo", "JTo", "T7s", "98s", "K6s", "Q7s", "J7s", "A9o", "97s", "Q6s", "J6s", "T6s", "96s", "87s", "A8o", "K9o", "86s", "T9o", "A7o", "Q9o", "J9o", "K8o", "A6o", "76s", "Q8o", "T8o", "K7o", "J8o", "K6o", "T7o", "Q7o", "98o", "J7o", "Q6o", "97o", "T6o", "J6o", "87o", "96o", "86o", "76o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "99o", "88o", "77o", "66o", "AKs", "AQs", "KQs", "AJs", "ATs", "KJs", "AKo", "QJs", "KTs", "JTs", "QTs", "A9s", "AQo", "A8s", "K9s", "KQo", "A7s", "T9s", "J9s", "A6s", "K8s", "Q9s", "Q8s", "AJo", "KJo", "T8s", "J8s", "ATo", "K7s", "QJo", "K6s", "Q7s", "T7s", "KTo", "QTo", "J7s", "98s", "JTo", "Q6s", "97s", "T6s", "J6s", "96s", "87s", "A9o", "86s", "A8o", "T9o", "K9o", "76s", "Q9o", "A7o", "J9o", "K8o", "A6o", "T8o", "Q8o", "J8o", "K7o", "K6o", "T7o", "Q7o", "J7o", "98o", "Q6o", "97o", "T6o", "J6o", "96o", "87o", "86o", "76o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "99o", "88o", "77o", "66o", "AKs", "AQs", "AJs", "KQs", "ATs", "KJs", "QJs", "KTs", "QTs", "JTs", "AKo", "A9s", "A7s", "A8s", "T9s", "K9s", "AQo", "Q9s", "J9s", "KQo", "A6s", "K7s", "Q8s", "AJo", "K8s", "T8s", "J8s", "K6s", "Q7s", "KJo", "J7s", "ATo", "QJo", "98s", "Q6s", "T7s", "KTo", "JTo", "97s", "QTo", "T6s", "J6s", "87s", "96s", "86s", "A9o", "76s", "A8o", "J9o", "T9o", "K9o", "Q9o", "A7o", "A6o", "K8o", "T8o", "Q8o", "J8o", "K7o", "K6o", "T7o", "Q7o", "J7o", "98o", "Q6o", "97o", "J6o", "T6o", "87o", "96o", "86o", "76o" },
        };

        public static readonly int[][] BestHoleCardsNumeric = new int[][]
        {
            new int[0],
            new int[0],
            new int[81],
            new int[81],
            new int[81],
            new int[81],
            new int[81],
            new int[81],
            new int[81],
            new int[81]
        };

        public static readonly string[] EmotionalCards = new string[]
        {
            "AAo", "KKo", "QQo", "JJo", "TTo", "AKs", "AQs", "AKo", "99o", "AJs", "ATs", "KQs", "AQo", "KJs", "KTs", "A9s", "88o", "AJo", "QJs", "ATo", "KQo", "A8s", "QTs", "77o", "JTs", "K9s", "A7s", "KJo", "66o", "KTo", "A6s", "K8s", "QJo", "J9s", "QTo", "T9s", "JTo", "98s", "87s", "76s"
        };

        public static readonly int[] EmotionalCardsNumeric = new int[EmotionalCards.Length];

        private Card[] _sevenCardHand;

        public Card[] HoleCards { get; private set; }
        public int HoleCardsNumericRepresentation { get; private set; }
        public string HoleCardsRepresentation { get; private set; }

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

        public bool IsLiveAsHero;
        public bool IsLiveAsVillain;
        public bool ManiacPlay;

        public PokerHand()
        {
            _sevenCardHand = new Card[7];
        }

        public void GeneratePokerHand(Card[] holeCards, Card[] communityCards, int playerCount, bool isVillainEmotional, int keepTopPercentHero, int keepTopPercentVillain, bool maniacPlay)
        {
            _handRank = -1;

            CommunityCards = communityCards;
            HoleCards = holeCards;
            ManiacPlay = maniacPlay;

            var biggestHoleCard = holeCards[0];
            var smallestHoleCard = holeCards[1];
            string handString;
            
            if (biggestHoleCard.Value < smallestHoleCard.Value)
            {
                Card temp = biggestHoleCard;
                biggestHoleCard = smallestHoleCard;
                smallestHoleCard = temp;
            }

            var handNumericalRepresentation = (biggestHoleCard.Value * 9 + smallestHoleCard.Value) * 2 + (biggestHoleCard.Suit == smallestHoleCard.Suit ? 1 : 0);
            if (!PokerHand.HoleCardRepresentations.ContainsKey(handNumericalRepresentation))
            {
                var handStringArray = new string[3];
                handStringArray[0] = Card.CardValues[biggestHoleCard.Value];
                handStringArray[1] = Card.CardValues[smallestHoleCard.Value];
                if (biggestHoleCard.Suit == smallestHoleCard.Suit)
                {
                    handStringArray[2] = "s";
                }
                else
                {
                    handStringArray[2] = "o";
                }

                handString = string.Join("", handStringArray);
                PokerHand.HoleCardRepresentations.Add(handNumericalRepresentation, handString);
            }
            HoleCardsRepresentation = PokerHand.HoleCardRepresentations[handNumericalRepresentation];
            HoleCardsNumericRepresentation = (biggestHoleCard.Value * 9 + smallestHoleCard.Value) * 2 + (biggestHoleCard.Suit == smallestHoleCard.Suit ? 1 : 0);

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

            ScoreContainer[0] = 8;
            ScoreContainer[1] = fourOfAKindCardValue + 1;
            ScoreContainer[2] = kickerVal;
            return ScoreContainer;
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

            if (flushedSuit == -1) return ZeroScore;

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

            ScoreContainer[0] = 7;
            ScoreContainer[1] = highestFlushValue;
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
                    if(currentCard.Value > biggestPairCardValue)
                    {
                        biggestPairCardValue = currentCard.Value;
                    }

                    //Move to the next card. The next iteration should be outside the newly-found pair.
                    i++;
                }
            }

            //If a pair doesn't exist, then the poker hand cannot be a full house.
            if (biggestPairCardValue == -1) return ZeroScore;

            ScoreContainer[0] = 6;
            ScoreContainer[1] = biggestSetCardValue * 9 + biggestPairCardValue;
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
                100 * kickerValues[0]
                + 1 * kickerValues[1];

            ScoreContainer[0] = 5;
            ScoreContainer[1] = biggestSetCardValue + 1;
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

            //The value of this straight equals the face value of the highest card in the sequence.
            ScoreContainer[0] = 4;
            ScoreContainer[1] = _sevenCardHand[highestSequentialCardPosition].Value;
            ScoreContainer[2] = 0;
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

            ScoreContainer[0] = 3;
            ScoreContainer[1] = biggestPairCardValue * 9 + smallestPairCardValue;
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

            ScoreContainer[0] = 2;
            ScoreContainer[1] = pairCardValue + 1;
            ScoreContainer[2] = kickerVal;
            return ScoreContainer;
        }

        private int[] ScoreHighCard()
        {
            var highestValueCard = _sevenCardHand[0];

            var kickerVal = 
                1_000_000 * _sevenCardHand[1].Value
                + 10_000 * _sevenCardHand[2].Value
                + 100 * _sevenCardHand[3].Value
                + 1 * _sevenCardHand[4].Value;

            ScoreContainer[0] = 1;
            ScoreContainer[1] = highestValueCard.Value;
            ScoreContainer[2] = kickerVal;
            return ScoreContainer;
        }

        private bool CheckHeroStartingHandCriteria(int playerCount, int keepTopPercent)
        {
            var isStartingHand = false;

            var bestStartingHands = BestHoleCardsByPlayerCount[playerCount];
            var highestHandIndexToKeep = (int)(bestStartingHands.Length * ((double)keepTopPercent / 100));

            for (int i = 0; i < highestHandIndexToKeep; i++)
            {
                //if (HoleCardsRepresentation == bestStartingHands[i])
                //{
                //    isStartingHand = true;
                //}
            }

            return isStartingHand;
        }

        private bool CheckVillainStartingHandCriteria(int playerCount, int keepTopPercent, bool isVillainEmotional)
        {
            var isStartingHand = false;

            if (isVillainEmotional)
            {
                for (int i = 0; i < EmotionalCards.Length; i++)
                {
                    //if (HoleCardsRepresentation == EmotionalCards[i])
                    //{
                    //    isStartingHand = true;
                    //}
                }
            }
            else
            {
                var bestStartingHands = BestHoleCardsByPlayerCount[playerCount];
                var highestHandIndexToKeep = (int)(bestStartingHands.Length * ((double)keepTopPercent / 100));

                for (int i = 0; i < highestHandIndexToKeep; i++)
                {
                    //if (HoleCardsRepresentation == bestStartingHands[i])
                    //{
                    //    isStartingHand = true;
                    //}
                }
            }

            return isStartingHand;
        }
    }

    public class Table
    {
        private readonly Random NumberGenerator = new Random((int)DateTime.Now.Ticks);
        private long[][] HoleCardsWinCounter;
        private long[][] HoleCardsDealtCounter;
        private long[][] HoleCardsTieCounter;

        private readonly Dictionary<string, int[]> HandsMadeCount = new Dictionary<string, int[]>();
        private readonly Dictionary<string, int[]> HandsWonCount = new Dictionary<string, int[]>();
        private readonly Dictionary<string, int[]> HandsTiedCount = new Dictionary<string, int[]>();

        private readonly Dictionary<long, int> HandRankCount = new Dictionary<long, int>();
        private readonly Dictionary<long, PokerHand[]> HandsWithRank = new Dictionary<long, PokerHand[]>();
        private readonly Dictionary<string, double> HoleCardWinRate = new Dictionary<string, double>();

        private Card[] Deck;
        private Card[] CommunityCards = new Card[5];
        private Card[][] PlayerHoleCards;
        private PokerHand[] AllPlayerFullHands;

        private readonly int PlayerCount;
        private readonly bool ManiacPlay;

        public Table(int numPlayers, bool maniacPlay)
        {
            PlayerCount = numPlayers;
            ManiacPlay = maniacPlay;

            HoleCardsWinCounter = new long[9][];
            for(int i = 0; i < HoleCardsWinCounter.Length; i++)
            {
                HoleCardsWinCounter[i] = new long[9];
            }

            HoleCardsDealtCounter = new long[9][];
            for (int i = 0; i < HoleCardsDealtCounter.Length; i++)
            {
                HoleCardsDealtCounter[i] = new long[9];
            }

            HoleCardsTieCounter = new long[9][];
            for(int i = 0; i < HoleCardsTieCounter.Length; i++)
            {
                HoleCardsTieCounter[i] = new long[9];
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

            PlayerHoleCards = new Card[numPlayers][];
            for (int i = 0; i < PlayerHoleCards.Length; i++)
            {
                PlayerHoleCards[i] = new Card[2];
            }

            AllPlayerFullHands = new PokerHand[numPlayers];
            for(int i = 0; i < numPlayers; i++)
            {
                AllPlayerFullHands[i] = new PokerHand();
            }
        }

        public void PlayHand(bool isVillainEmotional, int keepTopPercentHero, int keepTopPercentVillain)
        {
            ShuffleDeck();

            DealCardsToPlayers();

            //Populate community cards.
            GetCommunityCards();

            //Generate the 7-card poker hands.
            GeneratePokerHands(isVillainEmotional, keepTopPercentHero, keepTopPercentVillain);

            //Log each hand's play.
            LogHandResults();
        }

        public void PlayHands(long iterations, bool isVillainEmotional, int keepTopPercentHero, int keepTopPercentVillain)
        {
            for(int i = 0; i < iterations; i++)
            {
                PlayHand(isVillainEmotional, keepTopPercentHero, keepTopPercentVillain);

                if(i % 10_000 == 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Player Count: { PlayerCount } - Hands Remaining: {iterations - i}");
                }
            }
        }

        public void PrintHoleCardWinRatesRankedByBest()
        {
            var holeCardsAndWinRates = GetHoleCardsAndWinRates();

            StringBuilder sb = new StringBuilder();

            var cardArray = new string[holeCardsAndWinRates.Count];
            var winRateArray = new double[holeCardsAndWinRates.Count];
            holeCardsAndWinRates.Keys.CopyTo(cardArray, 0);

            var pos = 0;
            foreach (var holeCards in holeCardsAndWinRates)
            {
                var winRate = holeCardsAndWinRates[holeCards.Key];

                cardArray[pos] = holeCards.Key;
                winRateArray[pos] = winRate[0];

                pos++;
            }

            Array.Sort(winRateArray, cardArray);
            Array.Reverse(winRateArray);
            Array.Reverse(cardArray);

            sb.AppendLine("Hole Cards".PadRight(12) + "Win Rate".PadRight(25) + "Loss Rate".PadRight(25) + "Tie Rate");
            for (int i = 0; i < cardArray.Length; i++)
            {
                var holeCards = cardArray[i];
                var winRate = holeCardsAndWinRates[holeCards][0];
                var lossRate = holeCardsAndWinRates[holeCards][1];
                var tieRate = holeCardsAndWinRates[holeCards][2];

                if (winRate == 0.0) continue;

                sb.AppendLine($"{holeCards.PadRight(12)}{winRate.ToString().PadRight(25)}{lossRate.ToString().PadRight(25)}{tieRate.ToString()}");
            }
            sb.AppendLine();

            Console.WriteLine(sb.ToString());
        }

        public void PrintWinRatesForPokerHandsMade()
        {
            var holeCardsAndWinRates = GetHoleCardsAndWinRates();

            var cardArray = new string[holeCardsAndWinRates.Count];
            var winRateArray = new double[holeCardsAndWinRates.Count];
            holeCardsAndWinRates.Keys.CopyTo(cardArray, 0);

            var holeCardsCount = 0;
            foreach (var holeCards in holeCardsAndWinRates)
            {
                // [win rate, loss rate, tie rate]
                var holeCardsResultStats = holeCardsAndWinRates[holeCards.Key];

                if (holeCardsResultStats[0] == 0.0) continue;

                cardArray[holeCardsCount] = holeCards.Key;
                winRateArray[holeCardsCount] = holeCardsResultStats[0];

                holeCardsCount++;
            }

            Array.Sort(winRateArray, cardArray);
            Array.Reverse(winRateArray);
            Array.Reverse(cardArray);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine();

            for (int j = 0; j < holeCardsCount; j++)
            {
                var targetCard = cardArray[j];
                var handsMadeArray = HandsMadeCount[targetCard];
                var handsWonArray = HandsWonCount[targetCard];
                var handsTiedArray = HandsTiedCount[targetCard];

                var totalCardAppearances = 0;
                for (int i = 0; i < handsMadeArray.Length; i++)
                {
                    totalCardAppearances += handsMadeArray[i];
                }

                sb.AppendLine(targetCard.PadRight(8) + "Rank".PadRight(20) + "Rank Chance".PadRight(25) + "Rank Count".PadRight(15) + "Rank Win Count".PadRight(18) + "Rank Tie Count".PadRight(18) + "Rank Win Rate".PadRight(25) + "Rank Tie Rate".PadRight(25) + "Rank Win Index");
                for (int i = 1; i < handsMadeArray.Length; i++)
                {
                    var handRank = PokerHand.HandRanks[i].PadRight(20);

                    var handRankPossibility = (totalCardAppearances == 0 ? 0 : (double)handsMadeArray[i] / totalCardAppearances);
                    var handRankPossibilityString = $"{handRankPossibility}".PadRight(25);
                    var handRankCount = $"{handsMadeArray[i]}".PadRight(15);
                    var handWinCount = $"{handsWonArray[i]}".PadRight(18);
                    var handTieCount = $"{handsTiedArray[i]}".PadRight(18);

                    var handWinRate = (handsMadeArray[i] == 0 ? 0 : (double)handsWonArray[i] / handsMadeArray[i]);
                    var handWinRateString = $"{handWinRate}".PadRight(25);

                    var handTieRate = (handsMadeArray[i] == 0 ? 0 : (double)handsTiedArray[i] / handsMadeArray[i]);
                    var handTieRateString = $"{handTieRate}".PadRight(25);

                    var winIndex = handRankPossibility * (handWinRate + handTieRate);
                    var winIndexString = $"{winIndex}";
                    sb.AppendLine($"        {handRank}{handRankPossibilityString}{handRankCount}{handWinCount}{handTieCount}{handWinRateString}{handTieRateString}{winIndexString}");
                }
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
        }

        public void PrintHoleCardsRankedByBestForArray()
        {
            var holeCardsAndWinRates = GetHoleCardsAndWinRates();

            StringBuilder sb = new StringBuilder();

            var cardArray = new string[holeCardsAndWinRates.Count];
            var winRateArray = new double[holeCardsAndWinRates.Count];
            holeCardsAndWinRates.Keys.CopyTo(cardArray, 0);

            var pos = 0;
            foreach (var holeCards in holeCardsAndWinRates)
            {
                var winRate = holeCardsAndWinRates[holeCards.Key];

                cardArray[pos] = holeCards.Key;
                winRateArray[pos] = winRate[0];

                pos++;
            }

            Array.Sort(winRateArray, cardArray);
            Array.Reverse(cardArray);

            for (var i = 0; i < cardArray.Length; i++)
            {
                cardArray[i] = "\"" + cardArray[i] + "\"";
            }

            sb.Append("new string[] { ");
            sb.Append(string.Join(", ", cardArray));
            sb.Append(" },");

            Console.WriteLine(sb.ToString());
        }

        public void PrintHoleCardsNumericRankedByBestForArray()
        {
            var holeCardsAndWinRates = GetHoleCardsAndWinRates();

            StringBuilder sb = new StringBuilder();

            var holeCardArray = new string[holeCardsAndWinRates.Count];
            var holeCardNumericArray = new int[holeCardsAndWinRates.Count];
            var winRateArray = new double[holeCardsAndWinRates.Count];
            holeCardsAndWinRates.Keys.CopyTo(holeCardArray, 0);

            var pos = 0;
            foreach (var holeCards in holeCardsAndWinRates)
            {

                var holeCardsString = holeCards.Key;
                var winRate = holeCardsAndWinRates[holeCardsString];

                holeCardArray[pos] = holeCardsString;
                winRateArray[pos] = winRate[0];

                var holeCardProperties = holeCardsString.ToCharArray();
                var biggestCardValue = Array.IndexOf(Card.CardValues, holeCardProperties[0].ToString());
                var smallestCardValue = Array.IndexOf(Card.CardValues, holeCardProperties[1].ToString());
                var suitedValue = holeCardProperties[2] == 's' ? 1 : 0;

                var holeCardsNumericRepresentation = (biggestCardValue * 9 + smallestCardValue) * 2 + suitedValue;
                holeCardNumericArray[pos] = holeCardsNumericRepresentation;

                pos++;
            }

            Array.Sort(winRateArray, holeCardNumericArray);
            Array.Reverse(holeCardNumericArray);

            for (var i = 0; i < holeCardNumericArray.Length; i++)
            {
                holeCardArray[i] = holeCardNumericArray[i].ToString();
            }

            sb.Append("new int[] { ");
            sb.Append(string.Join(", ", holeCardArray));
            sb.Append(" },");

            Console.WriteLine(sb.ToString());
        }

        private Dictionary<string, double[]> GetHoleCardsAndWinRates()
        {
            var holeCardsAndWinRates = new Dictionary<string, double[]>();

            for (int i = 0; i < HoleCardsWinCounter.Length; i++)
            {
                for (int j = i; j < HoleCardsWinCounter[0].Length; j++)
                {
                    var dealCount = HoleCardsDealtCounter[i][j];
                    var winCount = HoleCardsWinCounter[i][j];
                    var tieCount = HoleCardsTieCounter[i][j];
                    var lossCount = dealCount - winCount - tieCount;

                    var winRate = dealCount == 0 ? 0 : (double)winCount / dealCount;
                    var tieRate = dealCount == 0 ? 0 : (double)tieCount / dealCount;
                    var lossRate = dealCount == 0 ? 0 : (double)lossCount / dealCount;
                    holeCardsAndWinRates.Add($"{Card.CardValues[j]}{Card.CardValues[i]}o", new double[] { winRate, lossRate, tieRate });
                }
            }

            for (int i = 0; i < HoleCardsWinCounter.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    var dealCount = HoleCardsDealtCounter[i][j];
                    var winCount = HoleCardsWinCounter[i][j];
                    var tieCount = HoleCardsTieCounter[i][j];
                    var lossCount = dealCount - winCount - tieCount;

                    var winRate = dealCount == 0 ? 0 : (double)winCount / dealCount;
                    var tieRate = dealCount == 0 ? 0 : (double)tieCount / dealCount;
                    var lossRate = dealCount == 0 ? 0 : (double)lossCount / dealCount;
                    holeCardsAndWinRates.Add($"{Card.CardValues[i]}{Card.CardValues[j]}s", new double[] { winRate, lossRate, tieRate });
                }
            }

            return holeCardsAndWinRates;
        }

        private void DealCardsToPlayers()
        {
            //Deal cards
            for (int i = 0; i < PlayerHoleCards.Length * PlayerHoleCards[0].Length; i++)
            {
                var player = i % PlayerHoleCards.Length;
                var holeCard = i / PlayerHoleCards.Length;

                PlayerHoleCards[player][holeCard] = Deck[i];
            }
        }

        private void GetCommunityCards()
        {
            int numCardsDealtToPlayers = PlayerHoleCards.Length * PlayerHoleCards[0].Length;
            
            Array.Copy(Deck, numCardsDealtToPlayers, CommunityCards, 0, 5);

            //Sort community cards in descending order
            Array.Sort(CommunityCards);
            Array.Reverse(CommunityCards);
        }

        private void GeneratePokerHands(bool isVillainEmotional, int keepTopPercentHero, int keepTopPercentVillain)
        {
            for (int i = 0; i < PlayerHoleCards.Length; i++)
            {
                var playerHoleCards = PlayerHoleCards[i];

                var firstHoleCard = playerHoleCards[0];
                var secondHoleCard = playerHoleCards[1];

                if (firstHoleCard.Value < secondHoleCard.Value)
                {
                    playerHoleCards[0] = secondHoleCard;
                    playerHoleCards[1] = firstHoleCard;
                }

                AllPlayerFullHands[i].GeneratePokerHand(PlayerHoleCards[i], CommunityCards, PlayerCount, isVillainEmotional, keepTopPercentHero, keepTopPercentVillain, ManiacPlay);
            }
        }

        private void LogHandResults()
        {
            Card biggestHoleCard;
            Card smallestHoleCard;
            string handString;
            int handNumericalRepresentation;

            if (!ManiacPlay)
            {
                //Log the hand only if there exists a hand the hero would play.
                var hasHeroHand = false;
                foreach (var hand in AllPlayerFullHands)
                {
                    if (hand.IsLiveAsHero)
                    {
                        hasHeroHand = true;
                        break;
                    }
                }
                if (!hasHeroHand) return;
            }

            //Log all hands made. Even the folded ones.
            foreach (var hand in AllPlayerFullHands)
            {
                //Biggest card first
                biggestHoleCard = hand.HoleCards[0];
                smallestHoleCard = hand.HoleCards[1];
                if (biggestHoleCard.Value < smallestHoleCard.Value)
                {
                    Card temp = biggestHoleCard;
                    biggestHoleCard = smallestHoleCard;
                    smallestHoleCard = temp;
                }

                handNumericalRepresentation = (biggestHoleCard.Value * 9 + smallestHoleCard.Value) * 2 + (biggestHoleCard.Suit == smallestHoleCard.Suit ? 1 : 0);
                handString = PokerHand.HoleCardRepresentations[handNumericalRepresentation];
                
                if (biggestHoleCard.Suit == smallestHoleCard.Suit)
                {
                    HoleCardsDealtCounter[biggestHoleCard.Value][smallestHoleCard.Value]++;
                }
                else
                {
                    HoleCardsDealtCounter[smallestHoleCard.Value][biggestHoleCard.Value]++;
                }

                if (!HandsMadeCount.ContainsKey(handString))
                {
                    HandsMadeCount.Add(handString, new int[10]);
                    HandsWonCount.Add(handString, new int[10]);
                    HandsTiedCount.Add(handString, new int[10]);
                }

                HandsMadeCount[handString][hand.HandRank / 100_000_000_000L]++;
            }

            //Determine the strongest hand among the live hands as villains.
            PokerHand strongestHand = null;
            PokerHand strongestHandForHero = null;
            HandRankCount.Clear();
            foreach (var result in AllPlayerFullHands)
            {
                if (!ManiacPlay && !result.IsLiveAsVillain) continue;

                var handRank = result.HandRank;
                if (!HandRankCount.ContainsKey(handRank))
                {
                    HandRankCount.Add(handRank, 0);
                }

                if (!HandsWithRank.ContainsKey(handRank))
                {
                    HandsWithRank.Add(handRank, new PokerHand[PlayerCount]);
                }
                HandsWithRank[handRank][HandRankCount[handRank]] = result;
                HandRankCount[handRank]++;

                if (!ManiacPlay && result.IsLiveAsHero)
                {
                    if(strongestHandForHero == null || handRank > strongestHandForHero.HandRank)
                    {
                        strongestHandForHero = result;
                    }
                }

                if (strongestHand == null || handRank > strongestHand.HandRank)
                {
                    strongestHand = result;
                }
            }

            //If none of the hands meet the Villain Starting Hands criteria, do not log this hand.
            if(strongestHand == null)
            {
                return;
            }

            //If there is no hand that the hero would play, then do not log a win or tie.
            if(!ManiacPlay && strongestHandForHero == null)
            {
                return;
            }

            //If the hero loses, do not log the hand as a win or tie.
            if (!ManiacPlay && strongestHand.HandRank != strongestHandForHero.HandRank)
            {
                return;
            }

            //Check for tie
            var isTie = HandRankCount[strongestHand.HandRank] > 1;

            //Biggest card first
            biggestHoleCard = strongestHand.HoleCards[0];
            smallestHoleCard = strongestHand.HoleCards[1];
            if (biggestHoleCard.Value < smallestHoleCard.Value)
            {
                Card temp = biggestHoleCard;
                biggestHoleCard = smallestHoleCard;
                smallestHoleCard = temp;
            }

            handNumericalRepresentation = (biggestHoleCard.Value * 9 + smallestHoleCard.Value) * 2 + (biggestHoleCard.Suit == smallestHoleCard.Suit ? 1 : 0);
            handString = PokerHand.HoleCardRepresentations[handNumericalRepresentation];

            if (isTie)
            {
                if (biggestHoleCard.Suit == smallestHoleCard.Suit)
                {
                    HoleCardsTieCounter[biggestHoleCard.Value][smallestHoleCard.Value]++;
                }
                else
                {
                    HoleCardsTieCounter[smallestHoleCard.Value][biggestHoleCard.Value]++;
                }

                //Mark all tied hands as a win
                var tieingPokerHands = HandsWithRank[strongestHand.HandRank];
                for(int i = 0; i < HandRankCount[strongestHand.HandRank]; i++)
                {
                    var pokerHand = tieingPokerHands[i];
                    HandsTiedCount[pokerHand.HoleCardsRepresentation][pokerHand.HandRank / 100_000_000_000L]++;
                }
            }
            else
            {
                if (biggestHoleCard.Suit == smallestHoleCard.Suit)
                {
                    HoleCardsWinCounter[biggestHoleCard.Value][smallestHoleCard.Value]++;
                }
                else
                {
                    HoleCardsWinCounter[smallestHoleCard.Value][biggestHoleCard.Value]++;
                }

                HandsWonCount[handString][strongestHand.HandRank / 100_000_000_000L]++;
            }
        }

        private void ShuffleDeck()
        {
            //Shuffle the deck.
            var cardIndexPosition = Deck.Length;
            while (cardIndexPosition > 1)
            {
                cardIndexPosition--;
                int cardPositionToSwap = NumberGenerator.Next(Deck.Length);
                var swapCard = Deck[cardPositionToSwap];
                Deck[cardPositionToSwap] = Deck[cardIndexPosition];
                Deck[cardIndexPosition] = swapCard;
            }
        }
    }
}
