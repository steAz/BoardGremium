using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGremiumRESTservice.Adugo
{
    public class AdugoGame : Game
    {
        public static int BOARD_WIDTH = 5;
        public static int BOARD_HEIGHT = 7;
        public AdugoGame(FieldType humanPawn)
            : base(BOARD_WIDTH, BOARD_HEIGHT)
        {
            this.currentBoardState = StartingPosition();
            this.HumanPlayerFieldType = humanPawn;
            if (humanPawn.Equals(FieldType.JAGUAR_PAWN))
                this.BotPlayerFieldType = FieldType.DOG_PAWN;
            else if(humanPawn.Equals(FieldType.DOG_PAWN))
                this.BotPlayerFieldType = FieldType.JAGUAR_PAWN;

        }

        public override BoardState StartingPosition()
        {
            return null;
        }
        /// <summary>
        /// returns true if player has won the game, false otherwise
        /// </summary>
        public override bool IsGameWon(BoardState bs, PlayerEnum forPlayer)
        {
            return false;
        }
        /// <summary>
        /// Method returns list of boardStates that could be obtained from initial boardState by performing one move
        /// </summary>
        /// <param name="initial">initial BoardState</param>
        /// <returns></returns>
        public override List<BoardState> GetPossibleBoardStates(BoardState initial, PlayerEnum playerType)
        {
            return null;
        }

        public override int CalculateMaximumPossibleRange(BoardState initial, Field pawn, DirectionEnum direction)
        {
            return 0;
        }
        public override void MovePawn(BoardState board, Field field, DirectionEnum direction, int numberOfFields)
        {

        }


    }
}