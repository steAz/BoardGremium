using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractGame
{
    public class Bot
    {
        public Game Game { get; }
        public int Heuristics(BoardState bs)
        {
            return 0;
        }

        public Bot(Game game)
        {
            this.Game = game;
        }
        /// <summary>
        /// The simplest implementation of makingMove - pics random possible move and returns it
        /// </summary>
        public BoardState MakeMove()
        {
            List<BoardState> possibleMoves = Game.GetPossibleBoardStates(Game.currentBoardState, PlayerEnum.BOT_PLAYER);
            Random rnd = new Random();
            int randNumber = rnd.Next(possibleMoves.Count);
            return possibleMoves[randNumber];
        }
    }
}
