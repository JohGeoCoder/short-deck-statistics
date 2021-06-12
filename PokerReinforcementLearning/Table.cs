using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerReinforcementLearning
{
    public class Table
    {
        

        public Player[] Players { get; set; }
        
        public Deck Deck { get; set; }

        public long?[] Flop { get; set; } = new long?[3];
        public long? Turn { get; set; }
        public long? River { get; set; }

        public int ButtonPosition { get; set; }

        


        public Table()
        {
            Players = Enumerable.Range(1, 9).Select(n => new Player
            {
                PlayerId = n
            }).ToArray();

            Deck = new Deck();

            ButtonPosition = 0;
        }

        public void PlayHand()
        {
            DealCards();

            //Preflop Action
            PlayersAct(isPreflop: true);

            DealFlop();

            PlayersAct();

            DealTurn();

            PlayersAct();

            DealRiver();

            PlayersAct();

            Showdown();
        }

        public void DealCards()
        {
            Players = Deck.DealCardsToPlayers(Players);
        }

        public void DealFlop()
        {
            Flop[0] = Deck.Deal();
            Flop[1] = Deck.Deal();
            Flop[2] = Deck.Deal();
        }

        public void DealTurn()
        {
            Turn = Deck.Deal();
        }

        public void DealRiver()
        {
            River = Deck.Deal();
        }

        public void Showdown()
        {
            throw new NotImplementedException();
        }

        public void PlayersAct(bool isPreflop = false)
        {
            //Find the first player with a potential action
            var actionPosition = (isPreflop ? ButtonPosition + 3 : ButtonPosition + 1) % Players.Length;
            while (!Players[actionPosition].HasAction)
            {
                actionPosition = (actionPosition + 1) % Players.Length;
            }

            //Find the player last to act.
            var lastAggressorPosition = actionPosition - 1;
            while (!Players[lastAggressorPosition].HasAction)
            {
                lastAggressorPosition = (lastAggressorPosition - 1) % Players.Length;
            }

            do
            {
                var actingPlayer = Players[actionPosition];

                if(actingPlayer.HasAction)
                {
                    var action = actingPlayer.Act();

                    //Update the last aggressor in the case of a bet or raise.
                    if ((action.Type & (ActionType.Bet | ActionType.Raise)) != 0)
                    {
                        lastAggressorPosition = actionPosition;
                    }
                }

                actionPosition++;
            } while (actionPosition != lastAggressorPosition);
        }
    }
}
