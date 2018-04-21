using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractGame
{
    public abstract class Bot
    {
        private Game Game { get; }
        public abstract int Heuristics(BoardState bs);
        /// <summary>
        /// The simplest implementation of makingMove - pics random possible move and returns it
        /// </summary>
        public BoardState MakeMove(BoardState startingBoardState)
        {
            List<BoardState> possibleMoves = Game.GetPossibleBoardStates(startingBoardState);
            Random rnd = new Random();
            int randNumber = rnd.Next(possibleMoves.Count);
            return possibleMoves[randNumber];
        }
    }
}
