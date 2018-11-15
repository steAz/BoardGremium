using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGremiumRESTservice.Adugo
{
    public class AdugoGame
    {
        public int BoardWidth { get; }
        public int BoardHeight { get; }
        public AdugoBoardState CurrentBoardState { get; set; }
        public PlayerEnum CurrentPlayer { get; set; }
        public Enum HumanPlayerFieldType { get; set; } //e.g dogPawn/jaguarPawn
        public Enum BotPlayerFieldType { get; set; }


        public AdugoGame(FieldType humanPawn, int boardWidth, int boardHeight)
        {
            this.BoardHeight = boardHeight;
            this.BoardWidth = boardWidth;
            this.CurrentBoardState = StartingPosition();
            this.HumanPlayerFieldType = humanPawn;
            if (humanPawn.Equals(FieldType.JAGUAR_PAWN))
                this.BotPlayerFieldType = FieldType.DOG_PAWN;
            else if(humanPawn.Equals(FieldType.DOG_PAWN))
                this.BotPlayerFieldType = FieldType.JAGUAR_PAWN;

        }

        public AdugoBoardState StartingPosition()
        {
            var startingBoardState = new AdugoBoardState(BoardWidth, BoardHeight);
            for (var y = 0; y < startingBoardState.Height; y++)
            {
                for (var x = 0; x < startingBoardState.Width; x++)
                {
                    startingBoardState.BoardFields[y, x] = new AdugoField(x, y, FieldType.EMPTY_FIELD, AdugoDirectionType.ALL_DIRECTIONS);
                }
            }
            ////locked pawns
            startingBoardState.BoardFields[5, 0].Type = FieldType.LOCKED_FIELD;
            startingBoardState.BoardFields[5, 0].DirectionType = AdugoDirectionType.NONE;
            startingBoardState.BoardFields[6, 0].Type = FieldType.LOCKED_FIELD;
            startingBoardState.BoardFields[6, 0].DirectionType = AdugoDirectionType.NONE;
            startingBoardState.BoardFields[5, 4].Type = FieldType.LOCKED_FIELD;
            startingBoardState.BoardFields[5, 4].DirectionType = AdugoDirectionType.NONE;
            startingBoardState.BoardFields[6, 4].Type = FieldType.LOCKED_FIELD;
            startingBoardState.BoardFields[6, 4].DirectionType = AdugoDirectionType.NONE;

            ////jaguar pawn
            startingBoardState.BoardFields[2, 2].Type = FieldType.JAGUAR_PAWN;
            startingBoardState.BoardFields[2, 2].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;

            ////dog pawns
            startingBoardState.BoardFields[0, 0].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[0, 0].DirectionType = AdugoDirectionType.DOWN_DOWNRIGHT_RIGHT;
            startingBoardState.BoardFields[1, 0].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[1, 0].DirectionType = AdugoDirectionType.UP_DOWN_RIGHT;

            startingBoardState.BoardFields[2, 0].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[2, 0].DirectionType = AdugoDirectionType.UP_DOWN_UPRIGHT_DOWNRIGHT_RIGHT;
            startingBoardState.BoardFields[0, 1].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[0, 1].DirectionType = AdugoDirectionType.DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[1, 1].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[1, 1].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;
            startingBoardState.BoardFields[2, 1].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[2, 1].DirectionType = AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[0, 2].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[0, 2].DirectionType = AdugoDirectionType.DOWN_DOWNLEFT_DOWNRIGHT_LEFT_RIGHT;
            startingBoardState.BoardFields[1, 2].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[1, 2].DirectionType = AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[0, 3].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[0, 3].DirectionType = AdugoDirectionType.DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[1, 3].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[1, 3].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;
            startingBoardState.BoardFields[2, 3].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[2, 3].DirectionType = AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[0, 4].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[0, 4].DirectionType = AdugoDirectionType.DOWN_DOWNLEFT_LEFT;
            startingBoardState.BoardFields[1, 4].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[1, 4].DirectionType = AdugoDirectionType.UP_DOWN_LEFT;
            startingBoardState.BoardFields[2, 4].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[2, 4].DirectionType = AdugoDirectionType.UP_DOWN_UPLEFT_DOWNLEFT_LEFT;


            //empty pawns
            startingBoardState.BoardFields[3, 0].DirectionType = AdugoDirectionType.UP_DOWN_RIGHT;
            startingBoardState.BoardFields[4, 0].DirectionType = AdugoDirectionType.UP_UPRIGHT_RIGHT;
            startingBoardState.BoardFields[3, 1].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;
            startingBoardState.BoardFields[4, 1].DirectionType = AdugoDirectionType.UP_LEFT_RIGHT;
            startingBoardState.BoardFields[3, 2].DirectionType = AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[4, 2].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;
            startingBoardState.BoardFields[3, 3].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;
            startingBoardState.BoardFields[4, 3].DirectionType = AdugoDirectionType.UP_LEFT_RIGHT;
            startingBoardState.BoardFields[3, 4].DirectionType = AdugoDirectionType.UP_DOWN_LEFT;
            startingBoardState.BoardFields[4, 4].DirectionType = AdugoDirectionType.UP_UPLEFT_LEFT;
            startingBoardState.BoardFields[5, 2].DirectionType = AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[6, 2].DirectionType = AdugoDirectionType.UP_LEFT_RIGHT;
            startingBoardState.BoardFields[5, 1].DirectionType = AdugoDirectionType.UPRIGHT_RIGHT;
            startingBoardState.BoardFields[5, 3].DirectionType = AdugoDirectionType.UPLEFT_LEFT;
            startingBoardState.BoardFields[6, 1].DirectionType = AdugoDirectionType.UP_RIGHT;
            startingBoardState.BoardFields[6, 3].DirectionType = AdugoDirectionType.UP_LEFT;
            return startingBoardState;
        }




    }
}