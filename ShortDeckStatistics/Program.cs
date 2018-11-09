using System;

namespace ShortDeckStatistics
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var shuffledDeck = ShuffleDeck();

            for (int cardIndex = 0; cardIndex < shuffledDeck.Length; cardIndex++)
            {
                Console.WriteLine(shuffledDeck[cardIndex]);
            }

            Console.ReadKey();
        }

        public static Card[] ShuffleDeck()
        {
            var rng = new Random((int)DateTime.Now.Ticks);

            var shuffledDeck = new Card[36];

            //Populate the deck
            for (short suit = 0; suit < 4; suit++)
            {
                for (short value = 0; value < 9; value++)
                {
                    int slot = suit * 9 + value;
                    shuffledDeck[slot] = new Card(suit, value);
                }
            }

            //Shuffle the deck.
            var cardIndexPosition = shuffledDeck.Length;
            while(cardIndexPosition > 1)
            {
                cardIndexPosition--;
                int cardPositionToSwap = rng.Next(shuffledDeck.Length + 1);
                var swapCard = shuffledDeck[cardPositionToSwap];
                shuffledDeck[cardPositionToSwap] = shuffledDeck[cardIndexPosition];
                shuffledDeck[cardIndexPosition] = swapCard;
            }

            return shuffledDeck;
        }

    }

    public class Card : IComparable<Card>
    {
        public short Value { get; private set; }
        public short Suit { get; private set; }

        public Card(short suit, short value)
        {
            Suit = suit;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Suit} - {Value}";
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

        private long RankHand()
        {
            var score = ScoreStraightFlush();
            score += score > 0 ? 0 : ScoreFourOfAKind();
            score += score > 0 ? 0 : ScoreFlush();
            score += score > 0 ? 0 : ScoreFullHouse();
            score += score > 0 ? 0 : ScoreThreeOfAKind();
            score += score > 0 ? 0 : ScoreStraight();
            score += score > 0 ? 0 : ScoreTwoPair();
            score += score > 0 ? 0 : ScorePair();
            score += score > 0 ? 0 : ScoreHighCard();

            return score;
        }

        private long ScoreStraightFlush()
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
            if (consecutiveSequenceLength < 5) return 0;

            //Check for suitedness.
            var allSuited = true;
            for (int i = highestSequentialCardPosition; i < highestSequentialCardPosition + 4; i++)
            {
                var currentCard = _sevenCardHand[i];
                var nextCard = _sevenCardHand[i + 1];

                allSuited = currentCard.Suit == nextCard.Suit;

                if (!allSuited) break;
            }

            //If the 5 consecutive cards are not suited, then the poker hand cannot be a straight flush.
            if (!allSuited) return 0;

            //The value of this straight flush equals the face value of the highest card in the sequence.
            return _sevenCardHand[highestSequentialCardPosition].Value;
        }

        private long ScoreFourOfAKind()
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
            if (consecutiveValueLength < 4) return 0;

            return fourOfAKindCardValue;
        }

        private long ScoreFlush()
        {
            var highestFlushValue = 0;

            //Check the poker hand for a flush in each of the suits.
            for(int suit = 0; suit < 4; suit++)
            {
                var suitCount = 0;

                //Iterate through the poker hand and count the cards with the target suit.
                //Additionally, keep track of the highest face value of that suit.
                for(int cardPos = 0; cardPos < _sevenCardHand.Length; cardPos++)
                {
                    var card = _sevenCardHand[cardPos];

                    //Skip the cards that do not match the target suit.
                    if (card.Suit != suit) continue;

                    suitCount++;

                    if (card.Value > highestFlushValue) highestFlushValue = card.Value;
                }

                if (suitCount >= 5) break;
                else
                {
                    highestFlushValue = 0;
                }
            }

            return highestFlushValue;
        }

        private long ScoreFullHouse()
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
            if (biggestSetCardValue == -1) return 0;

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
            if (biggestPairCardValue == -1) return 0;

            return biggestSetCardValue * 9 + biggestPairCardValue;
        }

        private long ScoreThreeOfAKind()
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

            //If a set doesn't exist, then the poker hand cannot be a 3 of a kind.
            if (biggestSetCardValue == -1) return 0;

            return biggestSetCardValue;
        }

        private long ScoreStraight()
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
            if (consecutiveSequenceLength < 5) return 0;

            //The value of this straight equals the face value of the highest card in the sequence.
            return _sevenCardHand[highestSequentialCardPosition].Value;
        }

        public long ScoreTwoPair()
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
            if (biggestPairCardValue == -1) return 0;

            //Find the value of the biggest set


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
            if (smallestPairCardValue == -1) return 0;

            return biggestPairCardValue * 9 + smallestPairCardValue;
        }

        public long ScorePair()
        {

        }
    }
}
