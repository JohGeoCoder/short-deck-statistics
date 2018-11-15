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

            var tableArray = new Table[3];
            for(int i = 2; i < tableArray.Length; i++)
            {
                tableArray[i] = new Table(6);
                tableArray[i].PlayHands(1_000_000, true, 20, 30);
                //tableArray[i].PlayHands(100_000);
            }

            for(int i = 2; i < tableArray.Length; i++)
            {
                tableArray[i].PrintHoleCardWinRatesRankedByBest();
                tableArray[i].PrintWinRatesForPokerHandsMade();
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
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "AKs", "AQs", "AKo", "AJs", "AQo", "ATs", "KQs", "AJo", "99o", "ATo", "KJs", "A9s", "KQo", "KTs", "KJo", "A8s", "A9o", "QJs", "KTo", "QTs", "K9s", "A7s", "A8o", "QJo", "JTs", "QTo", "K9o", "88o", "Q9s", "A6s", "A7o", "K8s", "JTo", "K7s", "Q9o", "J9s", "A6o", "Q8s", "K8o", "T9s", "K6s", "J9o", "K7o", "J8s", "Q8o", "Q7s", "77o", "T9o", "T8s", "K6o", "Q6s", "J7s", "J8o", "98s", "Q7o", "T8o", "T7s", "Q6o", "J7o", "J6s", "97s", "98o", "66o", "T6s", "T7o", "96s", "J6o", "97o", "87s", "T6o", "96o", "86s", "87o", "76s", "86o", "76o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "AKs", "TTo", "AQs", "AKo", "AJs", "AQo", "ATs", "KQs", "AJo", "99o", "KJs", "A9s", "KQo", "ATo", "KTs", "QJs", "A8s", "KJo", "QTs", "A9o", "KTo", "K9s", "A7s", "JTs", "QJo", "A8o", "88o", "QTo", "A6s", "Q9s", "K8s", "K9o", "A7o", "JTo", "J9s", "K7s", "Q8s", "T9s", "A6o", "Q9o", "K6s", "77o", "K8o", "J8s", "J9o", "Q7s", "T8s", "K7o", "T9o", "Q8o", "Q6s", "J7s", "98s", "66o", "K6o", "T7s", "J8o", "T8o", "Q7o", "J6s", "97s", "T6s", "Q6o", "J7o", "98o", "96s", "T7o", "87s", "97o", "J6o", "86s", "T6o", "96o", "76s", "87o", "86o", "76o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "AKs", "TTo", "AQs", "AKo", "AJs", "ATs", "KQs", "AQo", "99o", "KJs", "AJo", "A9s", "KTs", "ATo", "KQo", "QJs", "A8s", "QTs", "KJo", "88o", "K9s", "A7s", "KTo", "A9o", "JTs", "A6s", "QJo", "Q9s", "A8o", "77o", "QTo", "K8s", "J9s", "K7s", "K9o", "JTo", "T9s", "A7o", "Q8s", "66o", "K6s", "J8s", "A6o", "Q9o", "T8s", "Q7s", "K8o", "J9o", "J7s", "Q6s", "T7s", "T9o", "98s", "K7o", "Q8o", "97s", "J6s", "T6s", "J8o", "K6o", "T8o", "Q7o", "96s", "87s", "J7o", "Q6o", "T7o", "98o", "86s", "97o", "J6o", "T6o", "76s", "96o", "87o", "86o", "76o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "AKs", "AQs", "AKo", "99o", "AJs", "ATs", "KQs", "AQo", "KJs", "KTs", "A9s", "88o", "AJo", "QJs", "ATo", "KQo", "A8s", "QTs", "77o", "JTs", "K9s", "A7s", "KJo", "66o", "KTo", "A9o", "A6s", "Q9s", "K8s", "QJo", "J9s", "QTo", "T9s", "A8o", "K7s", "Q8s", "JTo", "J8s", "K9o", "K6s", "T8s", "A7o", "Q7s", "J7s", "Q9o", "T7s", "Q6s", "A6o", "98s", "K8o", "J9o", "T9o", "T6s", "J6s", "97s", "K7o", "Q8o", "96s", "J8o", "T8o", "K6o", "87s", "Q7o", "86s", "J7o", "T7o", "Q6o", "98o", "76s", "T6o", "97o", "J6o", "96o", "87o", "86o", "76o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "AKs", "99o", "AQs", "AJs", "AKo", "88o", "ATs", "KQs", "77o", "KJs", "AQo", "KTs", "A9s", "66o", "QJs", "QTs", "AJo", "A8s", "ATo", "JTs", "KQo", "K9s", "A7s", "KJo", "Q9s", "A6s", "KTo", "K8s", "J9s", "T9s", "A9o", "QJo", "Q8s", "K7s", "QTo", "J8s", "T8s", "K6s", "JTo", "A8o", "Q7s", "J7s", "T7s", "K9o", "Q6s", "A7o", "98s", "T6s", "Q9o", "J6s", "97s", "A6o", "J9o", "T9o", "K8o", "96s", "Q8o", "87s", "K7o", "J8o", "T8o", "86s", "K6o", "Q7o", "76s", "T7o", "J7o", "98o", "Q6o", "T6o", "97o", "J6o", "96o", "87o", "86o", "76o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "99o", "AKs", "88o", "AQs", "77o", "AJs", "ATs", "KQs", "66o", "AKo", "KJs", "KTs", "A9s", "AQo", "QJs", "QTs", "JTs", "A8s", "AJo", "K9s", "ATo", "A7s", "KQo", "Q9s", "A6s", "J9s", "T9s", "K8s", "KJo", "KTo", "Q8s", "K7s", "T8s", "J8s", "A9o", "QJo", "QTo", "K6s", "Q7s", "JTo", "T7s", "J7s", "Q6s", "A8o", "98s", "T6s", "K9o", "J6s", "97s", "A7o", "Q9o", "96s", "T9o", "J9o", "A6o", "87s", "K8o", "86s", "Q8o", "T8o", "K7o", "J8o", "76s", "K6o", "Q7o", "T7o", "J7o", "98o", "Q6o", "T6o", "J6o", "97o", "96o", "87o", "86o", "76o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "99o", "88o", "AKs", "77o", "AQs", "66o", "AJs", "ATs", "KQs", "KJs", "KTs", "AKo", "A9s", "QTs", "QJs", "JTs", "AQo", "A8s", "K9s", "A7s", "Q9s", "AJo", "T9s", "ATo", "J9s", "KQo", "A6s", "K8s", "Q8s", "K7s", "T8s", "J8s", "KJo", "KTo", "K6s", "Q7s", "T7s", "J7s", "QJo", "QTo", "A9o", "JTo", "Q6s", "98s", "T6s", "J6s", "97s", "A8o", "K9o", "96s", "87s", "A7o", "Q9o", "T9o", "J9o", "86s", "K8o", "A6o", "Q8o", "T8o", "76s", "J8o", "K7o", "K6o", "T7o", "Q7o", "J7o", "98o", "Q6o", "T6o", "J6o", "97o", "96o", "87o", "86o", "76o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "99o", "88o", "77o", "66o", "AKs", "AQs", "ATs", "AJs", "KQs", "KJs", "KTs", "QTs", "QJs", "A9s", "JTs", "AKo", "A8s", "K9s", "AQo", "A7s", "Q9s", "T9s", "J9s", "K8s", "A6s", "AJo", "ATo", "Q8s", "KQo", "T8s", "J8s", "K7s", "K6s", "KJo", "Q7s", "T7s", "J7s", "KTo", "Q6s", "QJo", "QTo", "T6s", "98s", "JTo", "J6s", "A9o", "97s", "96s", "A8o", "87s", "K9o", "86s", "T9o", "Q9o", "A7o", "J9o", "76s", "K8o", "A6o", "Q8o", "T8o", "J8o", "K7o", "K6o", "T7o", "Q7o", "J7o", "Q6o", "98o", "T6o", "J6o", "97o", "96o", "87o", "86o", "76o" },
        };

        public static readonly string[] EmotionalCards = new string[]
        {
            "AAo", "KKo", "QQo", "JJo", "TTo", "AKs", "AQs", "AKo", "99o", "AJs", "ATs", "KQs", "AQo", "KJs", "KTs", "A9s", "88o", "AJo", "QJs", "ATo", "KQo", "A8s", "QTs", "77o", "JTs", "K9s", "A7s", "KJo", "66o", "KTo", "A6s", "K8s", "QJo", "J9s", "QTo", "T9s", "JTo", "98s", "87s", "76s"
        };

        private Card[] _sevenCardHand;

        public Card[] HoleCards { get; private set; }
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

        public PokerHand()
        {
            _sevenCardHand = new Card[7];
        }

        public void GeneratePokerHand(Card[] holeCards, Card[] communityCards, int playerCount, bool isVillainEmotional, int keepTopPercentHero, int keepTopPercentVillain)
        {
            _handRank = -1;

            CommunityCards = communityCards;
            HoleCards = holeCards;

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

            IsLiveAsHero = CheckHeroStartingHandCriteria(playerCount, keepTopPercentHero);
            IsLiveAsVillain = CheckVillainStartingHandCriteria(playerCount, keepTopPercentVillain, isVillainEmotional);
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
                if (HoleCardsRepresentation == bestStartingHands[i])
                {
                    isStartingHand = true;
                }
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
                    if (HoleCardsRepresentation == EmotionalCards[i])
                    {
                        isStartingHand = true;
                    }
                }
            }
            else
            {
                var bestStartingHands = BestHoleCardsByPlayerCount[playerCount];
                var highestHandIndexToKeep = (int)(bestStartingHands.Length * ((double)keepTopPercent / 100));

                for (int i = 0; i < highestHandIndexToKeep; i++)
                {
                    if (HoleCardsRepresentation == bestStartingHands[i])
                    {
                        isStartingHand = true;
                    }
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

        public int PlayerCount { get; private set; }

        public Table(int numPlayers)
        {
            PlayerCount = numPlayers;

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

                if(i % 100_000 == 0)
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

            sb.AppendLine("Hole Cards".PadRight(12) + "Win Rate".PadRight(25) + "Loss Rate".PadRight(25) + "Tie Rate");
            for (int i = 0; i < cardArray.Length; i++)
            {
                var holeCards = cardArray[i];
                var winRate = holeCardsAndWinRates[holeCards][0];
                var lossRate = holeCardsAndWinRates[holeCards][1];
                var tieRate = holeCardsAndWinRates[holeCards][2];

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

            var pos = 0;
            foreach (var holeCards in holeCardsAndWinRates)
            {
                var winRate = holeCardsAndWinRates[holeCards.Key];

                cardArray[pos] = holeCards.Key;
                winRateArray[pos] = winRate[0];

                pos++;
            }

            Array.Sort(winRateArray, cardArray);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine();

            for (int j = 0; j < cardArray.Length; j++)
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
            foreach(var holeCards in holeCardsAndWinRates)
            {
                var winRate = holeCardsAndWinRates[holeCards.Key];

                cardArray[pos] = holeCards.Key;
                winRateArray[pos] = winRate[0];

                pos++;
            }

            Array.Sort(winRateArray, cardArray);
            Array.Reverse(cardArray);

            for (var i = 0; i < cardArray.Length; i++){
                cardArray[i] = "\"" + cardArray[i] + "\"";
            }

            sb.Append("new string[] { ");
            sb.Append(string.Join(", ", cardArray));
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

                AllPlayerFullHands[i].GeneratePokerHand(PlayerHoleCards[i], CommunityCards, PlayerCount, isVillainEmotional, keepTopPercentHero, keepTopPercentVillain);
            }
        }

        private void LogHandResults()
        {
            Card biggestHoleCard;
            Card smallestHoleCard;
            string handString;
            int handNumericalRepresentation;

            //Log the hand only if there exists a hand the hero would play.
            var hasHeroHand = false;
            foreach(var hand in AllPlayerFullHands)
            {
                if (hand.IsLiveAsHero)
                {
                    hasHeroHand = true;
                    break;
                }
            }
            if (!hasHeroHand) return;

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
            HandRankCount.Clear();
            foreach (var result in AllPlayerFullHands)
            {
                if (!result.IsLiveAsVillain) continue;

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

            //If the hero would not have played this hand, do not log it.
            if (!strongestHand.IsLiveAsHero)
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
