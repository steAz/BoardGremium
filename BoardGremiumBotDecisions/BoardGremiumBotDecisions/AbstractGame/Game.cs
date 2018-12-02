using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using BoardGremiumBotDecisions.Tablut;


namespace AbstractGame
{
    /// <summary>
    /// Abstract class representing Game
    /// </summary>
    public abstract class Game
    {
        public int BoardWidth { get; }
        public int BoardHeight { get; }
        public TablutBoardState CurrentTablutBoardState { get; set; }
        public PlayerEnum currentPlayer { get; set; }
        public FieldType HumanPlayerFieldType { get; set; } //e.g whitePawn/blackPawn
        public FieldType BotPlayerFieldType { get; set; }
        //image of empty tablutBoard
        public string BoardImageName { get; set; }
        /// <summary>
        /// dictionary holding mapping: boardItem -> itemGraphics e.g WHITE_PAWN -> pawn.jpg
        /// </summary>
        public Dictionary<Enum, string> ItemToGraphicsDict { get; set; }
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

        public abstract TablutBoardState StartingPosition();
        /// <summary>
        /// returns true if player has won the game, false otherwise
        /// </summary>
        public abstract bool IsGameWon(TablutBoardState bs, PlayerEnum forPlayer);
        /// <summary>
        /// Method returns list of boardStates that could be obtained from initial boardState by performing one move
        /// </summary>
        /// <param name="initial">initial TablutBoardState</param>
        /// <returns></returns>
        public abstract List<TablutBoardState> GetPossibleBoardStates(TablutBoardState initial, FieldType playerFieldType);

        public abstract int CalculateMaximumPossibleRange(TablutBoardState initial, Field pawn, DirectionEnum direction);
        public abstract void MovePawn(TablutBoardState tablutBoard, Field field, DirectionEnum direction, int numberOfFields);


    }
}
