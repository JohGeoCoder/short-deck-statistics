using System;
using System.Collections.Generic;
using System.Text;

namespace PokerStats.GameStructures
{
    public class Table
    {
        public int[] BestHoleCardsNumericHero;
        public int[] BestHoleCardsNumericVillain;

        private Random NumberGenerator = new Random((int)DateTime.Now.Ticks);


        private Card[] Deck;
        private Card[] CommunityCards = new Card[5];
        private Card[][] PlayerHoleCards;
        private PokerHand[] AllPlayerFullHands;

        private int PlayerCount;
        private bool ManiacPlay;

        private int KeepTopPercentHero;
        private int KeepTopPercentVillain;

        private HandTracker HandTracker;

        private Dictionary<long, int> HandRankCount = new Dictionary<long, int>();
        private Dictionary<long, PokerHand[]> HandsWithRank = new Dictionary<long, PokerHand[]>();

        public Table(int numPlayers, bool maniacPlay, int keepTopPercentHero, int keepTopPercentVillain, HandTracker handTracker)
        {
            HandTracker = handTracker;

            PlayerCount = numPlayers;
            ManiacPlay = maniacPlay;

            KeepTopPercentHero = keepTopPercentHero;
            KeepTopPercentVillain = keepTopPercentVillain;

            //Sort the emotional cards by numeric value.
            Array.Sort(HandTracker.EmotionalCardsNumeric);

            //Sort the Hero's starting hands.
            var bestStartingHands = HandTracker.BestHoleCardsNumericByPlayerCount[PlayerCount];
            var bestStartingHandHeroCount = (int)(bestStartingHands.Length * ((double)keepTopPercentHero / 100));
            BestHoleCardsNumericHero = new int[bestStartingHandHeroCount];
            Array.Copy(bestStartingHands, BestHoleCardsNumericHero, bestStartingHandHeroCount);
            Array.Sort(BestHoleCardsNumericHero);

            //Sort the Villain's starting hands.
            var bestStartingHandVillainCount = (int)(bestStartingHands.Length * ((double)keepTopPercentVillain / 100));
            BestHoleCardsNumericVillain = new int[bestStartingHandVillainCount];
            Array.Copy(bestStartingHands, BestHoleCardsNumericVillain, bestStartingHandVillainCount);
            Array.Sort(BestHoleCardsNumericVillain);

            //Sort the best starting hands by numeric value.
            //for (int i = 2; i < HandTracker.BestHoleCardsNumericByPlayerCount.Length; i++)
            //{
            //    Array.Sort(HandTracker.BestHoleCardsNumericByPlayerCount[i]);
            //}

            //Populate the deck
            Deck = new Card[HandTracker.DeckSuitCount * HandTracker.DeckNumericValueCount];
            for (short suit = 0; suit < HandTracker.DeckSuitCount; suit++)
            {
                for (short value = 0; value < HandTracker.DeckNumericValueCount; value++)
                {
                    int slot = suit * HandTracker.DeckNumericValueCount + value;
                    Deck[slot] = new Card(suit, value);
                }
            }

            PlayerHoleCards = new Card[numPlayers][];
            for (int i = 0; i < PlayerHoleCards.Length; i++)
            {
                PlayerHoleCards[i] = new Card[2];
            }

            AllPlayerFullHands = new PokerHand[numPlayers];
            for (int i = 0; i < numPlayers; i++)
            {
                AllPlayerFullHands[i] = new PokerHand(this);
            }
        }

        public void PlayHand(bool isVillainEmotional)
        {
            ShuffleDeck();

            DealCardsToPlayers();

            //Populate community cards.
            GetCommunityCards();

            //Generate the 7-card poker hands.
            GeneratePokerHands(isVillainEmotional, KeepTopPercentHero, KeepTopPercentVillain);

            //Log each hand's play.
            LogHandResults();
        }

        public void PlayHands(long iterations, bool isVillainEmotional)
        {
            for (long i = 0L; i < iterations; i++)
            {
                PlayHand(isVillainEmotional);

                if (i % 100_000 == 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Player Count: { PlayerCount } - Hands Remaining: {iterations - i}");
                }
            }
        }

        private void DealCardsToPlayers()
        {
            //Deal cards
            for (byte i = 0; i < PlayerHoleCards.Length * PlayerHoleCards[0].Length; i++)
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
            Array.Sort(CommunityCards, (cardA, cardB) => {
                return cardA.Value.CompareTo(cardB.Value);
            });
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

                isSuited = biggestHoleCard.Suit == smallestHoleCard.Suit;
                holeCardsNumeric = CardConversionHelper.ConvertHoleCardsToNumericValue(biggestHoleCard.Value, smallestHoleCard.Value, isSuited);

                //Determines which side of hole card matrix to log the cards.
                //One side of the diagonal represents suited holde cards. The 
                //diagonal itself and the other side represents unsuited hole
                //cards.
                if (isSuited)
                {
                    HandTracker.HoleCardsDealtCounter[biggestHoleCard.Value][smallestHoleCard.Value]++;
                }
                else
                {
                    HandTracker.HoleCardsDealtCounter[smallestHoleCard.Value][biggestHoleCard.Value]++;
                }

                if (HandTracker.LogPokerHandResults)
                {
                    var handRank = hand.HandRank / 100_000_000_000L;
                    HandTracker.HandsMadeCount[holeCardsNumeric][handRank]++;
                }
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

                if (HandTracker.LogPokerHandResults)
                {
                    if (!HandsWithRank.ContainsKey(handRank))
                    {
                        HandsWithRank.Add(handRank, new PokerHand[PlayerCount]);
                    }
                    HandsWithRank[handRank][HandRankCount[handRank]] = hand;
                }

                HandRankCount[handRank]++;

                if (!ManiacPlay && hand.IsLiveAsHero)
                {
                    if (strongestHandForHero == null || handRank > strongestHandForHero.HandRank)
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
            if (strongestHand == null)
            {
                return;
            }

            //If there is no hand that the hero would play, then do not log a win or tie.
            if (!ManiacPlay && strongestHandForHero == null)
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

            isSuited = biggestHoleCard.Suit == smallestHoleCard.Suit;
            holeCardsNumeric = CardConversionHelper.ConvertHoleCardsToNumericValue(biggestHoleCard.Value, smallestHoleCard.Value, isSuited);

            if (isTie)
            {
                if (isSuited)
                {
                    HandTracker.HoleCardsTieCounter[biggestHoleCard.Value][smallestHoleCard.Value]++;
                }
                else
                {
                    HandTracker.HoleCardsTieCounter[smallestHoleCard.Value][biggestHoleCard.Value]++;
                }
            }
            else
            {
                if (biggestHoleCard.Suit == smallestHoleCard.Suit)
                {
                    HandTracker.HoleCardsWinCounter[biggestHoleCard.Value][smallestHoleCard.Value]++;
                }
                else
                {
                    HandTracker.HoleCardsWinCounter[smallestHoleCard.Value][biggestHoleCard.Value]++;
                }
            }

            if (HandTracker.LogPokerHandResults)
            {
                if (isTie)
                {
                    //Mark all tied hands as a win
                    var tieingPokerHands = HandsWithRank[strongestHand.HandRank];
                    for (int i = 0; i < HandRankCount[strongestHand.HandRank]; i++)
                    {
                        var pokerHand = tieingPokerHands[i];
                        HandTracker.HandsTiedCount[pokerHand.HoleCardsNumericRepresentation][pokerHand.HandRank / 100_000_000_000L]++;
                    }
                }
                else
                {
                    HandTracker.HandsWonCount[holeCardsNumeric][strongestHand.HandRank / 100_000_000_000L]++;
                }
            }
        }

        private void ShuffleDeck()
        {
            //Put all the cards back in the deck.
            var cardIndexPosition = Deck.Length;
            while (cardIndexPosition > 1)
            {
                cardIndexPosition--;
            }

            //Shuffle the deck.
            cardIndexPosition = Deck.Length;
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
