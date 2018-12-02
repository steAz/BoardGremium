using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;
using BoardGremiumBotDecisions.Tablut;

namespace BoardGremiumBotDecisions.AI
{
    public class Bot
    {
        public Game Game { get; }
        public TablutBoardState TheBestTablutBoardState { get; set; }
        public List<TablutBoardState> AllTheBestBoardStates { get; }
        public int TheBestDepth { get; set; }

        public virtual int Heuristic(TablutBoardState bs)
        {
            return 0;
        }

        public Bot()
        {

        }

        public Bot(Game game)
        {
            this.Game = game;
            AllTheBestBoardStates = new List<TablutBoardState>();
        }
        /// <summary>
        /// The simplest implementation of makingMove - pics random possible move and returns it
        /// </summary>
        public virtual TablutBoardState MakeMove()
        {
            List<TablutBoardState> possibleMoves = Game.GetPossibleBoardStates(Game.CurrentTablutBoardState, Game.BotPlayerFieldType);
            Random rnd = new Random();
            int randNumber = rnd.Next(possibleMoves.Count);
            return possibleMoves[randNumber];
        }
    }
}
