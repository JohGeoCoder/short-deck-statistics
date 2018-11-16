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
            Console.WindowWidth = 200;

            var tableArray = new Table[3];
            for (int i = 2; i < tableArray.Length; i++)
            {
                tableArray[i] = new Table(6, false);
                tableArray[i].PlayHands(1_000_000, true, 10, 30);
            }

            for (int i = 2; i < tableArray.Length; i++)
            {
                tableArray[i].PrintHoleCardWinRatesRankedByBest();
                tableArray[i].PrintWinRatesForPokerHandsMade();
                //tableArray[i].PrintHoleCardsNumericRankedByBestForArray();
            }

            Console.ReadKey();


            //var emotionalCards = PokerHand.EmotionalCards;

            //foreach(var emotionalCardString in emotionalCards)
            //{
            //    Console.Write(emotionalCardString + " ");
            //}
            //Console.WriteLine();

            //var emotionalCardsNumeric = new int[emotionalCards.Length];
            //for(var i = 0; i < emotionalCards.Length; i++)
            //{
            //    var cardString = emotionalCards[i];
            //    //emotionalCardsNumeric[i] = $"{PokerHand.ConvertCardStringToNumericValue(cardString)}";
            //    emotionalCardsNumeric[i] = PokerHand.ConvertCardStringToNumericValue(cardString);
            //}

            //foreach(var numericCardString in emotionalCardsNumeric)
            //{
            //    Console.Write(numericCardString + " ");

            //}
            //Console.WriteLine();

            //var convertedBackCards = new string[emotionalCards.Length];
            //for(int i = 0; i < emotionalCardsNumeric.Length; i++)
            //{
            //    var numericValue = emotionalCardsNumeric[i];
            //    convertedBackCards[i] = PokerHand.ConvertHoleCardsNumericValueToString(numericValue);
            //}

            //foreach(var convertedBack in convertedBackCards)
            //{
            //    Console.Write(convertedBack + " ");
            //}

            //var sb = new StringBuilder();


            //sb.Append("new int[] { ");
            //sb.Append(string.Join(", ", emotionalCardsNumeric));
            //sb.Append(" },");

            //Console.WriteLine(sb.ToString());

            //Console.ReadKey();
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

        public static readonly int[][] BestHoleCardsNumericByPlayerCount = new int[][]
        {
            new int[0],
            new int[0],
            new int[] { 160, 140, 120, 100, 80, 159, 157, 158, 155, 156, 60, 153, 139, 154, 137, 138, 152, 151, 135, 136, 119, 149, 150, 134, 117, 133, 118, 40, 147, 148, 116, 99, 132, 145, 115, 146, 131, 98, 129, 97, 144, 114, 113, 130, 20, 79, 127, 128, 96, 95, 112, 111, 78, 126, 77, 109, 93, 94, 110, 59, 76, 75, 0, 108, 92, 91, 57, 58, 73, 74, 90, 55, 56, 39, 72, 37, 54, 38, 19, 36, 18 },
            new int[] { 160, 140, 120, 100, 80, 159, 157, 158, 155, 156, 60, 139, 153, 154, 137, 138, 135, 151, 152, 119, 136, 149, 117, 40, 134, 150, 133, 99, 118, 147, 116, 148, 145, 115, 131, 98, 132, 20, 146, 97, 129, 79, 113, 114, 144, 127, 95, 130, 0, 77, 96, 111, 78, 128, 112, 93, 109, 59, 75, 126, 94, 57, 76, 91, 110, 73, 58, 92, 108, 55, 39, 74, 56, 37, 90, 72, 19, 54, 38, 36, 18 },
            new int[] { 160, 140, 120, 100, 80, 159, 157, 158, 60, 155, 139, 156, 153, 137, 154, 135, 138, 40, 151, 119, 152, 117, 149, 136, 99, 20, 133, 134, 147, 118, 150, 0, 145, 115, 116, 131, 97, 148, 98, 79, 129, 113, 132, 146, 95, 127, 77, 114, 111, 144, 96, 93, 130, 59, 109, 75, 78, 128, 57, 112, 91, 73, 94, 76, 126, 55, 39, 110, 58, 37, 92, 74, 108, 19, 56, 90, 72, 54, 38, 36, 18 },
            new int[] { 160, 140, 120, 100, 80, 159, 60, 157, 158, 155, 139, 40, 153, 156, 137, 20, 135, 119, 151, 138, 0, 154, 117, 99, 152, 149, 136, 133, 147, 134, 118, 115, 145, 97, 150, 131, 79, 116, 98, 113, 129, 95, 148, 77, 127, 132, 111, 146, 59, 93, 75, 109, 114, 96, 144, 57, 78, 73, 91, 130, 55, 39, 112, 128, 94, 76, 37, 126, 110, 19, 58, 74, 92, 108, 56, 72, 90, 54, 38, 36, 18 },
            new int[] { 160, 140, 120, 100, 80, 60, 159, 40, 157, 20, 158, 139, 155, 0, 153, 137, 156, 135, 119, 117, 151, 99, 138, 154, 149, 133, 152, 147, 136, 115, 79, 97, 145, 134, 118, 131, 113, 116, 129, 77, 95, 150, 98, 127, 111, 59, 75, 93, 148, 109, 57, 132, 73, 91, 146, 114, 78, 55, 96, 39, 144, 37, 130, 112, 76, 19, 128, 94, 126, 58, 110, 74, 92, 108, 56, 72, 90, 54, 38, 36, 18 },
            new int[] { 160, 140, 120, 100, 80, 60, 40, 159, 20, 0, 157, 139, 155, 158, 153, 137, 135, 119, 117, 156, 99, 151, 138, 149, 154, 133, 147, 79, 115, 97, 152, 136, 145, 131, 118, 113, 77, 129, 134, 95, 127, 116, 98, 111, 75, 59, 93, 150, 109, 57, 73, 91, 55, 148, 39, 132, 37, 78, 146, 114, 96, 19, 144, 130, 76, 112, 94, 128, 126, 74, 58, 110, 92, 108, 56, 72, 90, 54, 38, 36, 18 },
            new int[] { 160, 140, 120, 100, 80, 60, 40, 20, 0, 159, 157, 139, 155, 153, 137, 158, 119, 135, 117, 99, 151, 156, 149, 133, 138, 147, 79, 115, 97, 145, 154, 131, 77, 136, 113, 129, 152, 95, 127, 118, 134, 111, 75, 59, 93, 116, 98, 109, 57, 73, 91, 150, 55, 39, 37, 148, 132, 19, 78, 96, 114, 146, 144, 130, 76, 112, 94, 128, 126, 74, 58, 110, 92, 108, 56, 72, 90, 38, 54, 36, 18 },
            new int[] { 160, 140, 120, 100, 80, 60, 40, 20, 0, 159, 157, 139, 155, 153, 137, 119, 135, 117, 99, 158, 151, 149, 133, 156, 147, 79, 115, 97, 145, 138, 131, 77, 113, 129, 95, 154, 127, 136, 111, 152, 75, 93, 118, 59, 109, 134, 57, 116, 98, 73, 91, 55, 39, 37, 150, 19, 148, 78, 132, 96, 114, 146, 144, 130, 76, 112, 94, 128, 126, 74, 110, 58, 92, 108, 56, 72, 90, 38, 54, 36, 18 },
        };

        public static readonly string[] EmotionalCards = new string[]
        {
            "AAo", "KKo", "QQo", "JJo", "TTo", "AKs", "AQs", "AKo", "99o", "AJs", "ATs", "KQs", "AQo", "KJs", "KTs", "A9s", "88o", "AJo", "QJs", "ATo", "KQo", "A8s", "QTs", "77o", "JTs", "K9s", "A7s", "KJo", "66o", "KTo", "A6s", "K8s", "QJo", "J9s", "QTo", "T9s", "JTo", "98s", "87s", "76s"
        };

        public static readonly int[] EmotionalCardsNumeric = new int[] { 160, 140, 120, 100, 80, 159, 157, 158, 60, 155, 153, 139, 156, 137, 135, 151, 40, 154, 119, 152, 138, 149, 117, 20, 99, 133, 147, 136, 0, 134, 145, 131, 118, 97, 116, 79, 98, 59, 39, 19 };

        private Card[] _sevenCardHand;

        public Card[] HoleCards { get; private set; }
        public int HoleCardsNumericRepresentation { get; private set; }

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
            
            if (biggestHoleCard.Value < smallestHoleCard.Value)
            {
                Card temp = biggestHoleCard;
                biggestHoleCard = smallestHoleCard;
                smallestHoleCard = temp;
            }
            
            HoleCardsNumericRepresentation = PokerHand.ConvertHoleCardsToNumericValue(biggestHoleCard.Value, smallestHoleCard.Value, biggestHoleCard.Suit == smallestHoleCard.Suit);

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

            var bestStartingHands = BestHoleCardsNumericByPlayerCount[playerCount];
            var highestHandIndexToKeep = (int)(bestStartingHands.Length * ((double)keepTopPercent / 100));

            for (int i = 0; i < highestHandIndexToKeep; i++)
            {
                if (HoleCardsNumericRepresentation == bestStartingHands[i])
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
                    if (HoleCardsNumericRepresentation == EmotionalCardsNumeric[i])
                    {
                        isStartingHand = true;
                    }
                }
            }
            else
            {
                var bestStartingHands = BestHoleCardsNumericByPlayerCount[playerCount];
                var highestHandIndexToKeep = (int)(bestStartingHands.Length * ((double)keepTopPercent / 100));

                for (int i = 0; i < highestHandIndexToKeep; i++)
                {
                    if (HoleCardsNumericRepresentation == bestStartingHands[i])
                    {
                        isStartingHand = true;
                    }
                }
            }

            return isStartingHand;
        }

        public static int ConvertHoleCardsToNumericValue(int biggestCardValue, int smallestCardValue, bool areSuited)
        {
            return (biggestCardValue * 9 + smallestCardValue) * 2 + (areSuited ? 1 : 0);
        }

        public static string ConvertHoleCardsNumericValueToString(int holeCardsNumericValue)
        {
            bool suited = holeCardsNumericValue % 2 == 1;

            int biggestCardValue = (holeCardsNumericValue / 2) / 9;
            int smallestCardValue = (holeCardsNumericValue / 2) % 9;

            return $"{Card.CardValues[biggestCardValue]}{Card.CardValues[smallestCardValue]}{(suited ? "s" : "o")}";
        }

        public static int ConvertCardStringToNumericValue(string cardString)
        {
            var holeCardProperties = cardString.ToCharArray();
            var biggestCardValue = Array.IndexOf(Card.CardValues, holeCardProperties[0].ToString());
            var smallestCardValue = Array.IndexOf(Card.CardValues, holeCardProperties[1].ToString());
            var areSuited = holeCardProperties[2] == 's';

            var holeCardsNumericRepresentation = ConvertHoleCardsToNumericValue(biggestCardValue, smallestCardValue, areSuited);
            return holeCardsNumericRepresentation;
        }
    }

    public class Table
    {
        private readonly Random NumberGenerator = new Random((int)DateTime.Now.Ticks);
        private long[][] HoleCardsWinCounter;
        private long[][] HoleCardsDealtCounter;
        private long[][] HoleCardsTieCounter;

        private readonly Dictionary<int, int[]> HandsMadeCount = new Dictionary<int, int[]>();
        private readonly Dictionary<int, int[]> HandsWonCount = new Dictionary<int, int[]>();
        private readonly Dictionary<int, int[]> HandsTiedCount = new Dictionary<int, int[]>();

        private readonly Dictionary<long, int> HandRankCount = new Dictionary<long, int>();
        private readonly Dictionary<long, PokerHand[]> HandsWithRank = new Dictionary<long, PokerHand[]>();
        private readonly Dictionary<int, double> HoleCardWinRate = new Dictionary<int, double>();

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

            var cardNumericArray = new int[holeCardsAndWinRates.Count];
            var winRateArray = new double[holeCardsAndWinRates.Count];
            holeCardsAndWinRates.Keys.CopyTo(cardNumericArray, 0);

            var pos = 0;
            foreach (var holeCards in holeCardsAndWinRates)
            {
                var winRate = holeCardsAndWinRates[holeCards.Key];

                cardNumericArray[pos] = holeCards.Key;
                winRateArray[pos] = winRate[0];

                pos++;
            }

            Array.Sort(winRateArray, cardNumericArray);
            Array.Reverse(winRateArray);
            Array.Reverse(cardNumericArray);

            sb.AppendLine("Hole Cards".PadRight(12) + "Win Rate".PadRight(25) + "Loss Rate".PadRight(25) + "Tie Rate");
            for (int i = 0; i < cardNumericArray.Length; i++)
            {
                var holeCardsNumeric = cardNumericArray[i];
                var winRate = holeCardsAndWinRates[holeCardsNumeric][0];
                var lossRate = holeCardsAndWinRates[holeCardsNumeric][1];
                var tieRate = holeCardsAndWinRates[holeCardsNumeric][2];

                if (winRate == 0.0) continue;

                var holeCardsString = PokerHand.ConvertHoleCardsNumericValueToString(holeCardsNumeric);

                sb.AppendLine($"{holeCardsString.PadRight(12)}{winRate.ToString().PadRight(25)}{lossRate.ToString().PadRight(25)}{tieRate.ToString()}");
            }
            sb.AppendLine();

            Console.WriteLine(sb.ToString());
        }

        public void PrintWinRatesForPokerHandsMade()
        {
            var holeCardsAndWinRates = GetHoleCardsAndWinRates();

            var holeCardsArray = new int[holeCardsAndWinRates.Count];
            var winRateArray = new double[holeCardsAndWinRates.Count];
            holeCardsAndWinRates.Keys.CopyTo(holeCardsArray, 0);

            var holeCardsCount = 0;
            foreach (var holeCards in holeCardsAndWinRates)
            {
                // [win rate, loss rate, tie rate]
                var holeCardsResultStats = holeCardsAndWinRates[holeCards.Key];

                if (holeCardsResultStats[0] == 0.0) continue;

                holeCardsArray[holeCardsCount] = holeCards.Key;
                winRateArray[holeCardsCount] = holeCardsResultStats[0];

                holeCardsCount++;
            }

            Array.Sort(winRateArray, holeCardsArray);
            Array.Reverse(winRateArray);
            Array.Reverse(holeCardsArray);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine();

            for (int j = 0; j < holeCardsCount; j++)
            {
                var targetCard = holeCardsArray[j];
                var handsMadeArray = HandsMadeCount[targetCard];
                var handsWonArray = HandsWonCount[targetCard];
                var handsTiedArray = HandsTiedCount[targetCard];

                var totalCardAppearances = 0;
                for (int i = 0; i < handsMadeArray.Length; i++)
                {
                    totalCardAppearances += handsMadeArray[i];
                }

                var targetCardString = PokerHand.ConvertHoleCardsNumericValueToString(targetCard);

                sb.AppendLine(targetCardString.PadRight(8) + "Rank".PadRight(20) + "Rank Chance".PadRight(25) + "Rank Count".PadRight(15) + "Rank Win Count".PadRight(18) + "Rank Tie Count".PadRight(18) + "Rank Win Rate".PadRight(25) + "Rank Tie Rate".PadRight(25) + "Rank Win Index");
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

        public void PrintHoleCardsNumericRankedByBestForArray()
        {
            var holeCardsAndWinRates = GetHoleCardsAndWinRates();

            StringBuilder sb = new StringBuilder();

            
            var holeCardNumericArray = new int[holeCardsAndWinRates.Count];
            var winRateArray = new double[holeCardsAndWinRates.Count];
            holeCardsAndWinRates.Keys.CopyTo(holeCardNumericArray, 0);

            var pos = 0;
            foreach (var holeCards in holeCardsAndWinRates)
            {

                var holeCardsNumeric = holeCards.Key;
                var winRate = holeCardsAndWinRates[holeCardsNumeric];

                winRateArray[pos] = winRate[0];

                holeCardNumericArray[pos] = holeCardsNumeric;

                pos++;
            }

            Array.Sort(winRateArray, holeCardNumericArray);
            Array.Reverse(holeCardNumericArray);

            var holeCardArray = new string[holeCardsAndWinRates.Count];
            for (var i = 0; i < holeCardNumericArray.Length; i++)
            {
                holeCardArray[i] = holeCardNumericArray[i].ToString();
            }

            sb.Append("new int[] { ");
            sb.Append(string.Join(", ", holeCardArray));
            sb.Append(" },");

            Console.WriteLine(sb.ToString());
        }

        private Dictionary<int, double[]> GetHoleCardsAndWinRates()
        {
            var holeCardsAndWinRates = new Dictionary<int, double[]>();

            for (int smallCardValue = 0; smallCardValue < HoleCardsWinCounter.Length; smallCardValue++)
            {
                for (int bigCardValue = smallCardValue; bigCardValue < HoleCardsWinCounter[0].Length; bigCardValue++)
                {
                    var dealCount = HoleCardsDealtCounter[smallCardValue][bigCardValue];
                    var winCount = HoleCardsWinCounter[smallCardValue][bigCardValue];
                    var tieCount = HoleCardsTieCounter[smallCardValue][bigCardValue];
                    var lossCount = dealCount - winCount - tieCount;

                    var winRate = dealCount == 0 ? 0 : (double)winCount / dealCount;
                    var tieRate = dealCount == 0 ? 0 : (double)tieCount / dealCount;
                    var lossRate = dealCount == 0 ? 0 : (double)lossCount / dealCount;

                    var holeCardsNumeric = PokerHand.ConvertHoleCardsToNumericValue(bigCardValue, smallCardValue, false);


                    holeCardsAndWinRates.Add(holeCardsNumeric, new double[] { winRate, lossRate, tieRate });
                }
            }

            for (int bigCardValue = 0; bigCardValue < HoleCardsWinCounter.Length; bigCardValue++)
            {
                for (int smallcardValue = 0; smallcardValue < bigCardValue; smallcardValue++)
                {
                    var dealCount = HoleCardsDealtCounter[bigCardValue][smallcardValue];
                    var winCount = HoleCardsWinCounter[bigCardValue][smallcardValue];
                    var tieCount = HoleCardsTieCounter[bigCardValue][smallcardValue];
                    var lossCount = dealCount - winCount - tieCount;

                    var winRate = dealCount == 0 ? 0 : (double)winCount / dealCount;
                    var tieRate = dealCount == 0 ? 0 : (double)tieCount / dealCount;
                    var lossRate = dealCount == 0 ? 0 : (double)lossCount / dealCount;

                    var holeCardsNumeric = PokerHand.ConvertHoleCardsToNumericValue(bigCardValue, smallcardValue, true);

                    holeCardsAndWinRates.Add(holeCardsNumeric, new double[] { winRate, lossRate, tieRate });
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
            int holeCardsNumeric;

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

            bool isSuited;

            //Log all hands made. Even the folded ones.
            for (int handIndex = 0; handIndex < AllPlayerFullHands.Length; handIndex++)
            {
                var hand = AllPlayerFullHands[handIndex];

                //Biggest card first
                biggestHoleCard = hand.HoleCards[0];
                smallestHoleCard = hand.HoleCards[1];
                if (biggestHoleCard.Value < smallestHoleCard.Value)
                {
                    Card temp = biggestHoleCard;
                    biggestHoleCard = smallestHoleCard;
                    smallestHoleCard = temp;
                }

                isSuited = biggestHoleCard.Suit == smallestHoleCard.Suit;
                holeCardsNumeric = PokerHand.ConvertHoleCardsToNumericValue(biggestHoleCard.Value, smallestHoleCard.Value, isSuited);

                if (isSuited)
                {
                    HoleCardsDealtCounter[biggestHoleCard.Value][smallestHoleCard.Value]++;
                }
                else
                {
                    HoleCardsDealtCounter[smallestHoleCard.Value][biggestHoleCard.Value]++;
                }

                if (!HandsMadeCount.ContainsKey(holeCardsNumeric))
                {
                    HandsMadeCount.Add(holeCardsNumeric, new int[10]);
                    HandsWonCount.Add(holeCardsNumeric, new int[10]);
                    HandsTiedCount.Add(holeCardsNumeric, new int[10]);
                }

                HandsMadeCount[holeCardsNumeric][hand.HandRank / 100_000_000_000L]++;
            }

            //Determine the strongest hand among the live hands as villains.
            PokerHand strongestHand = null;
            PokerHand strongestHandForHero = null;
            HandRankCount.Clear();
            for (int handIndex = 0; handIndex < AllPlayerFullHands.Length; handIndex++)
            {
                var hand = AllPlayerFullHands[handIndex];

                if (!ManiacPlay && !hand.IsLiveAsVillain) continue;

                var handRank = hand.HandRank;
                if (!HandRankCount.ContainsKey(handRank))
                {
                    HandRankCount.Add(handRank, 0);
                }

                if (!HandsWithRank.ContainsKey(handRank))
                {
                    HandsWithRank.Add(handRank, new PokerHand[PlayerCount]);
                }
                HandsWithRank[handRank][HandRankCount[handRank]] = hand;
                HandRankCount[handRank]++;

                if (!ManiacPlay && hand.IsLiveAsHero)
                {
                    if(strongestHandForHero == null || handRank > strongestHandForHero.HandRank)
                    {
                        strongestHandForHero = hand;
                    }
                }

                if (strongestHand == null || handRank > strongestHand.HandRank)
                {
                    strongestHand = hand;
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

            isSuited = biggestHoleCard.Suit == smallestHoleCard.Suit;
            holeCardsNumeric = PokerHand.ConvertHoleCardsToNumericValue(biggestHoleCard.Value, smallestHoleCard.Value, isSuited);

            if (isTie)
            {
                if (isSuited)
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
                    HandsTiedCount[pokerHand.HoleCardsNumericRepresentation][pokerHand.HandRank / 100_000_000_000L]++;
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

                HandsWonCount[holeCardsNumeric][strongestHand.HandRank / 100_000_000_000L]++;
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
