using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerReinforcementLearning
{
    public class Deck
    {
        private const long MASK_ACE         = 0b_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_111;
        private const long MASK_TWO         = 0b_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_111_000;
        private const long MASK_THREE       = 0b_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_111_000_000;
        private const long MASK_FOUR        = 0b_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_111_000_000_000;
        private const long MASK_FIVE        = 0b_0000_0000_0000_0000_000_000_000_000_000_000_000_000_111_000_000_000_000;
        private const long MASK_SIX         = 0b_0000_0000_0000_0000_000_000_000_000_000_000_000_111_000_000_000_000_000;
        private const long MASK_SEVEN       = 0b_0000_0000_0000_0000_000_000_000_000_000_000_111_000_000_000_000_000_000;
        private const long MASK_EIGHT       = 0b_0000_0000_0000_0000_000_000_000_000_000_111_000_000_000_000_000_000_000;
        private const long MASK_NINE        = 0b_0000_0000_0000_0000_000_000_000_000_111_000_000_000_000_000_000_000_000;
        private const long MASK_TEN         = 0b_0000_0000_0000_0000_000_000_000_111_000_000_000_000_000_000_000_000_000;
        private const long MASK_JACK        = 0b_0000_0000_0000_0000_000_000_111_000_000_000_000_000_000_000_000_000_000;
        private const long MASK_QUEEN       = 0b_0000_0000_0000_0000_000_111_000_000_000_000_000_000_000_000_000_000_000;
        private const long MASK_KING        = 0b_0000_0000_0000_0000_111_000_000_000_000_000_000_000_000_000_000_000_000;
        private const long MASK_HEARTS      = 0b_0000_0000_0000_1111_000_000_000_000_000_000_000_000_000_000_000_000_000;
        private const long MASK_DIAMONDS    = 0b_0000_0000_1111_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
        private const long MASK_SPADES      = 0b_0000_1111_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
        private const long MASK_CLUBS       = 0b_1111_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;

        private const long MASK_CARD        = 0b_0000_0000_0000_0000_111_111_111_111_111_111_111_111_111_111_111_111_111;
        private const long MASK_SUIT        = 0b_1111_1111_1111_1111_000_000_000_000_000_000_000_000_000_000_000_000_000;

        private const long BLANK            = 0b_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;

        private const long ACE              = 0b_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_001;
        private const long TWO              = 0b_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_001_000;
        private const long THREE            = 0b_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_001_000_000;
        private const long FOUR             = 0b_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_001_000_000_000;
        private const long FIVE             = 0b_0000_0000_0000_0000_000_000_000_000_000_000_000_000_001_000_000_000_000;
        private const long SIX              = 0b_0000_0000_0000_0000_000_000_000_000_000_000_000_001_000_000_000_000_000;
        private const long SEVEN            = 0b_0000_0000_0000_0000_000_000_000_000_000_000_001_000_000_000_000_000_000;
        private const long EIGHT            = 0b_0000_0000_0000_0000_000_000_000_000_000_001_000_000_000_000_000_000_000;
        private const long NINE             = 0b_0000_0000_0000_0000_000_000_000_000_001_000_000_000_000_000_000_000_000;
        private const long TEN              = 0b_0000_0000_0000_0000_000_000_000_001_000_000_000_000_000_000_000_000_000;
        private const long JACK             = 0b_0000_0000_0000_0000_000_000_001_000_000_000_000_000_000_000_000_000_000;
        private const long QUEEN            = 0b_0000_0000_0000_0000_000_001_000_000_000_000_000_000_000_000_000_000_000;
        private const long KING             = 0b_0000_0000_0000_0000_001_000_000_000_000_000_000_000_000_000_000_000_000;
        private const long HEARTS           = 0b_0000_0000_0000_0001_000_000_000_000_000_000_000_000_000_000_000_000_000;
        private const long DIAMONDS         = 0b_0000_0000_0001_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
        private const long SPADES           = 0b_0000_0001_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
        private const long CLUBS            = 0b_0001_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;

        private static readonly long[] VALUES = new long[] { ACE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING };
        private static readonly long[] SUITS = new long[] { HEARTS, DIAMONDS, SPADES, CLUBS };

        private long[] Cards = new long[52];

        private static readonly Random NumberGenerator = new((int)DateTime.Now.Ticks);

        private int TopPosition = 0;

        public Deck()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    var cardValue = SUITS[i] + VALUES[j];

                    Cards[i * 13 + j] = cardValue;
                }
            }
        }

        public Player[] DealCardsToPlayers(Player[] players)
        {
            Cards = Shuffle(Cards);

            var playersWithCards = new Player[players.Length];

            var position = 0;
            foreach(var player in players)
            {
                playersWithCards[position] = player.ReceiveHoleCards(Deal(), Deal());
                position++;
            }

            return playersWithCards;
        }

        public long Deal() => Cards[TopPosition++];

        private long[] Shuffle(long[] cards)
        {
            TopPosition = 0;

            //Shuffle the deck.
            var cardIndexPosition = cards.Length;
            while (cardIndexPosition > 1)
            {
                cardIndexPosition--;
                int cardPositionToSwap = NumberGenerator.Next(cards.Length);
                var swapCard = cards[cardPositionToSwap];
                cards[cardPositionToSwap] = cards[cardIndexPosition];
                cards[cardIndexPosition] = swapCard;
            }

            return cards;
        }
    }
}
