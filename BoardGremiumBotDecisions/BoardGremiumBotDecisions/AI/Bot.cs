using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;

namespace BoardGremiumBotDecisions.AI
{
    public class Bot
    {
        public Game Game { get; }
        public BoardState TheBestBoardState { get; set; }
        public List<BoardState> AllTheBestBoardStates { get; }
        public int TheBestDepth { get; set; }

        public virtual int Heuristic(BoardState bs)
        {
            return 0;
        }

        public Bot()
        {

        }

        public Bot(Game game)
        {
            this.Game = game;
            AllTheBestBoardStates = new List<BoardState>();
        }
        /// <summary>
        /// The simplest implementation of makingMove - pics random possible move and returns it
        /// </summary>
        public virtual BoardState MakeMove()
        {
            List<BoardState> possibleMoves = Game.GetPossibleBoardStates(Game.currentBoardState, Game.BotPlayerFieldType);
            Random rnd = new Random();
            int randNumber = rnd.Next(possibleMoves.Count);
            return possibleMoves[randNumber];
        }
    }
}
