using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerReinforcementLearning
{
    public class Player
    {
        public int PlayerId;
        public long? HoleOne;
        public long? HoleTwo;

        public int ChipStack { get; set; }

        public bool IsAllIn => ChipStack == 0;
        public bool IsFolded;

        public bool HasAction => !IsAllIn && !IsFolded;

        public Player()
        {
        }

        public Player ReceiveHoleCards(long hole1, long hole2)
        {
            HoleOne = hole1;
            HoleTwo = hole2;

            return this;
        }

        public Action Act()
        {
            return new Action();
        }

    }

    public class Action
    {
        public ActionType Type { get; set; }

        public Action()
        {

        }
    }

    [Flags]
    public enum ActionType
    {
        OutOfHand = 1,
        Fold = 2,
        Check = 4,
        Call = 8,
        Bet = 16,
        Raise = 32
    }

}
