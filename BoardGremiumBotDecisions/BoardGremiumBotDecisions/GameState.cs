using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;

namespace BoardGremiumBotDecisions
{
    class GameState
    {
        public FieldType BotFieldType;
        public BoardState CurrentBoardState;

        public GameState(string botFieldTypeText)
        {
            if (botFieldTypeText.Equals("red"))
                BotFieldType = FieldType.RED_PAWN;
            else
                BotFieldType = FieldType.BLACK_PAWN;

            CurrentBoardState = StartingPosition(9, 9);
        }

        public BoardState StartingPosition(int BoardWidth, int BoardHeight)
        {
            BoardState startingBoardState = new BoardState(BoardWidth, BoardHeight);
            for (int i = 0; i < startingBoardState.Height; i++)
            {
                for (int j = 0; j < startingBoardState.Width; j++)
                {
                    //startingBoardState.SetField(j, i, TablutFieldType.EMPTY_FIELD);
                    startingBoardState.BoardFields[i, j] = new Field(j, i, FieldType.EMPTY_FIELD);
                }
            }
            //black pawns
            startingBoardState.BoardFields[3, 0].Type = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[4, 0].Type = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[5, 0].Type = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[4, 1].Type = FieldType.BLACK_PAWN;

            startingBoardState.BoardFields[3, 8].Type = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[4, 8].Type = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[4, 7].Type = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[5, 8].Type = FieldType.BLACK_PAWN;

            startingBoardState.BoardFields[0, 3].Type = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[0, 4].Type = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[1, 4].Type = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[0, 5].Type = FieldType.BLACK_PAWN;

            startingBoardState.BoardFields[8, 3].Type = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[8, 4].Type = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[7, 4].Type = FieldType.BLACK_PAWN;
            startingBoardState.BoardFields[8, 5].Type = FieldType.BLACK_PAWN;
            //white pawns

            startingBoardState.BoardFields[4, 2].Type = FieldType.RED_PAWN;
            startingBoardState.BoardFields[4, 3].Type = FieldType.RED_PAWN;
            startingBoardState.BoardFields[4, 5].Type = FieldType.RED_PAWN;
            startingBoardState.BoardFields[4, 6].Type = FieldType.RED_PAWN;

            startingBoardState.BoardFields[2, 4].Type = FieldType.RED_PAWN;
            startingBoardState.BoardFields[3, 4].Type = FieldType.RED_PAWN;
            startingBoardState.BoardFields[5, 4].Type = FieldType.RED_PAWN;
            startingBoardState.BoardFields[6, 4].Type = FieldType.RED_PAWN;
            //king
            startingBoardState.BoardFields[4, 4].Type = FieldType.KING;
            return startingBoardState;
        }

        public void ChangeBoardStateAfterMove(DirectionEnum direction, Field field, int numOfFields)
        {
            int xCoord = 0;
            int yCoord = 0;

            switch (direction)
            {
                case DirectionEnum.UP:
                    {
                        yCoord -= numOfFields;
                        break;
                    }
                case DirectionEnum.DOWN:
                    {
                        yCoord += numOfFields;
                        break;
                    }
                case DirectionEnum.RIGHT:
                    {
                        xCoord += numOfFields;
                        break;
                    }
                case DirectionEnum.LEFT:
                    {
                        xCoord -= numOfFields;
                        break;
                    }
            }

            CurrentBoardState.BoardFields[field.Y + yCoord, field.X + xCoord].Type = field.Type; // firstly, changing the destination field
            CurrentBoardState.BoardFields[field.Y, field.X].Type = FieldType.EMPTY_FIELD; // then, changing source field to empty

        }

    }
}
