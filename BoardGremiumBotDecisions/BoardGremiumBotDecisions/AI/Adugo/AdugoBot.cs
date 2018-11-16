using BoardGremiumBotDecisions.Adugo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumBotDecisions.AI.Adugo
{
    public class AdugoBot : Bot // this class maybe in the future will be useless, because of AlfaBetaBot and MinMaxBot for Adugo
    {
        public new AdugoGame Game { get; }
        public new AdugoBoardState TheBestBoardState { get; set; }
        public new List<AdugoBoardState> AllTheBestBoardStates { get; }

        public int Heuristic(AdugoBoardState bs)
        {
            return 0;
        }

        public AdugoBot(AdugoGame game)
        {
            this.Game = game;
            AllTheBestBoardStates = new List<AdugoBoardState>();
        }
        /// <summary>
        /// The simplest implementation of makingMove - pics random possible move and returns it
        /// </summary>
        public new AdugoBoardState MakeMove()
        {
            var possibleBoardStates = Game.GetPossibleAdugoBoardStates(Game.CurrentBoardState, Game.BotPlayerFieldType);
            var rnd = new Random();
            var randNumber = rnd.Next(possibleBoardStates.Count);
            return possibleBoardStates[randNumber];
        }
    }
}
