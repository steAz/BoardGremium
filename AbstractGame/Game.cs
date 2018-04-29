using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace AbstractGame
{
    /// <summary>
    /// Abstract class representing Game
    /// </summary>
    public abstract class Game
    {
        public int BoardWidth { get; }
        public int BoardHeight { get; }
        public BoardState currentBoardState { get; set; }
        public PlayerEnum currentPlayer { get; set; }
        public Enum HumanPlayerFieldType { get; set; } //e.g whitePawn/blackPawn
        public Enum BotPlayerFieldType { get; set; }
        //image of empty board
        public string BoardImageName { get; set; }
        /// <summary>
        /// dictionary holding mapping: boardItem -> itemGraphics e.g WHITE_PAWN -> pawn.jpg
        /// </summary>
        public Dictionary<Enum, string> ItemToGraphicsDict { get; }
        /// <summary>
        /// returns starting boardState for the game
        /// </summary>

        public Game(int boardWidth, int boardHeight, string pathToBoardImage)
        {
            BoardImageName = pathToBoardImage;
            this.BoardHeight = boardHeight;
            this.BoardWidth = boardWidth;
            ItemToGraphicsDict = new Dictionary<Enum, string>();
        }

        public abstract BoardState StartingPosition();
        /// <summary>
        /// returns true if player has won the game, false otherwise
        /// </summary>
        public abstract bool IsGameWon(BoardState bs, PlayerEnum forPlayer);
        /// <summary>
        /// Method returns list of boardStates that could be obtained from initial boardState by performing one move
        /// </summary>
        /// <param name="initial">initial BoardState</param>
        /// <returns></returns>
        public abstract List<BoardState> GetPossibleBoardStates(BoardState initial, PlayerEnum playerType);

        
    }
}
