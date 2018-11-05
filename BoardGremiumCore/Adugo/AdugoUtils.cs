using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumCore.Adugo
{
    static public class AdugoUtils
    {
        public static int BOARD_WIDTH = 5;
        public static int BOARD_HEIGHT = 7;

        public static char RED_CHAR = 'R';
        public static char BLACK_CHAR = 'B';
        public static char KING_CHAR = 'K';
        public static char EMPTY_CHAR = 'E';

        static public BoardState StartingPosition()
        {
            BoardState startingBoardState = new BoardState(BOARD_WIDTH, BOARD_HEIGHT);
            for (int i = 0; i < startingBoardState.Width; i++)
            {
                for (int j = 0; j < startingBoardState.Height; j++)
                {
                    startingBoardState.BoardFields[i][j] = new Field(j, i, FieldType.RED_PAWN);
                }
            }
            //black pawns
            //startingBoardState.BoardFields[0][3].Type = FieldType.BLACK_PAWN;
            //startingBoardState.BoardFields[0][4].Type = FieldType.BLACK_PAWN;
            //startingBoardState.BoardFields[0][5].Type = FieldType.BLACK_PAWN;
            //startingBoardState.BoardFields[1][4].Type = FieldType.BLACK_PAWN;

            //startingBoardState.BoardFields[8][3].Type = FieldType.BLACK_PAWN;
            //startingBoardState.BoardFields[8][4].Type = FieldType.BLACK_PAWN;
            //startingBoardState.BoardFields[7][4].Type = FieldType.BLACK_PAWN;
            //startingBoardState.BoardFields[8][5].Type = FieldType.BLACK_PAWN;

            //startingBoardState.BoardFields[3][0].Type = FieldType.BLACK_PAWN;
            //startingBoardState.BoardFields[4][0].Type = FieldType.BLACK_PAWN;
            //startingBoardState.BoardFields[4][1].Type = FieldType.BLACK_PAWN;
            //startingBoardState.BoardFields[5][0].Type = FieldType.BLACK_PAWN;

            //startingBoardState.BoardFields[3][8].Type = FieldType.BLACK_PAWN;
            //startingBoardState.BoardFields[4][8].Type = FieldType.BLACK_PAWN;
            //startingBoardState.BoardFields[4][7].Type = FieldType.BLACK_PAWN;
            //startingBoardState.BoardFields[5][8].Type = FieldType.BLACK_PAWN;
            ////red pawns

            //startingBoardState.BoardFields[2][4].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[3][4].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[5][4].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[6][4].Type = FieldType.RED_PAWN;

            //startingBoardState.BoardFields[4][2].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[4][3].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[4][5].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[4][6].Type = FieldType.RED_PAWN;
            ////king
            //startingBoardState.BoardFields[4][4].Type = FieldType.KING;
            return startingBoardState;
        }

        //returns true if targetFieldType is in the same team as playerPawn, false otherwise
        public static bool IsTargetSameType(FieldType playerPawn, FieldType targetFieldType)
        {
            if (playerPawn.Equals(FieldType.RED_PAWN) && (targetFieldType.Equals(FieldType.RED_PAWN) || targetFieldType.Equals(FieldType.KING))
                || (playerPawn.Equals(FieldType.BLACK_PAWN) && targetFieldType.Equals(FieldType.BLACK_PAWN)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static BoardState ConvertStringToTablutBoardState(string stringRepresentation)
        {
            BoardState result = new BoardState(BOARD_WIDTH, BOARD_HEIGHT);
            string[] arguments = stringRepresentation.Split(',');
            FieldType playerType;
            var enumerator = arguments[1].GetEnumerator();
            //  enumerator.MoveNext();

            int horizontalIndex = 0, verticalIndex = 0;
            while (enumerator.MoveNext())
            {
                char character = enumerator.Current;
                if (character.Equals(BLACK_CHAR))
                {
                    result.BoardFields[horizontalIndex][verticalIndex].Type = FieldType.BLACK_PAWN;
                }
                else if (character.Equals(RED_CHAR))
                {
                    result.BoardFields[horizontalIndex][verticalIndex].Type = FieldType.RED_PAWN;
                }
                else if (character.Equals(KING_CHAR))
                {
                    result.BoardFields[horizontalIndex][verticalIndex].Type = FieldType.KING;
                }
                else
                {
                    result.BoardFields[horizontalIndex][verticalIndex].Type = FieldType.EMPTY_FIELD;
                }

                horizontalIndex++;
                if (horizontalIndex >= BOARD_WIDTH)
                {
                    horizontalIndex = 0;
                    verticalIndex++;
                }

                if (verticalIndex >= BOARD_HEIGHT)
                {
                    //throw new ArgumentOutOfRangeException("Exception thrown while parsing BoardState string representation - string is too long");
                    break;
                }
            }

            return result;
        }
    }
}
