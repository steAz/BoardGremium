using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace BoardGremiumCore.Adugo
{
    static public class AdugoUtils
    {
        public static int BOARD_WIDTH = 5;
        public static int BOARD_HEIGHT = 8;

        public static char RED_CHAR = 'R';
        public static char BLACK_CHAR = 'B';
        public static char KING_CHAR = 'K';
        public static char EMPTY_CHAR = 'E';


        static public AdugoBoardState StartingPosition()
        {
            AdugoBoardState startingBoardState = new AdugoBoardState(BOARD_WIDTH, BOARD_HEIGHT);
            for (int y = 0; y < startingBoardState.Height; y++)
            {
                for (int x = 0; x < startingBoardState.Width; x++)
                {
                    startingBoardState.BoardFields[y][x] = new AdugoField(x, y, FieldType.EMPTY_FIELD, AdugoDirectionType.ALL_DIRECTIONS);
                }
            }
            ////locked pawns
            startingBoardState.BoardFields[5][0].Type = FieldType.LOCKED_FIELD;
            startingBoardState.BoardFields[5][0].DirectionType = AdugoDirectionType.NONE;
            startingBoardState.BoardFields[6][0].Type = FieldType.LOCKED_FIELD;
            startingBoardState.BoardFields[6][0].DirectionType = AdugoDirectionType.NONE;
            startingBoardState.BoardFields[5][4].Type = FieldType.LOCKED_FIELD;
            startingBoardState.BoardFields[5][4].DirectionType = AdugoDirectionType.NONE;
            startingBoardState.BoardFields[6][4].Type = FieldType.LOCKED_FIELD;
            startingBoardState.BoardFields[6][4].DirectionType = AdugoDirectionType.NONE;

            ////jaguar pawn
            startingBoardState.BoardFields[2][2].Type = FieldType.JAGUAR_PAWN;
            startingBoardState.BoardFields[2][2].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;

            ////dog pawns
            startingBoardState.BoardFields[0][0].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[0][0].DirectionType = AdugoDirectionType.DOWN_DOWNRIGHT_RIGHT;
            startingBoardState.BoardFields[1][0].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[1][0].DirectionType = AdugoDirectionType.UP_DOWN_RIGHT;

            startingBoardState.BoardFields[2][0].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[2][0].DirectionType = AdugoDirectionType.UP_DOWN_UPRIGHT_DOWNRIGHT_RIGHT;
            startingBoardState.BoardFields[0][1].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[0][1].DirectionType = AdugoDirectionType.DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[1][1].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[1][1].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;
            startingBoardState.BoardFields[2][1].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[2][1].DirectionType = AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[0][2].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[0][2].DirectionType = AdugoDirectionType.DOWN_DOWNLEFT_DOWNRIGHT_LEFT_RIGHT;
            startingBoardState.BoardFields[1][2].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[1][2].DirectionType = AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[0][3].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[0][3].DirectionType = AdugoDirectionType.DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[1][3].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[1][3].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;
            startingBoardState.BoardFields[2][3].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[2][3].DirectionType = AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[0][4].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[0][4].DirectionType = AdugoDirectionType.DOWN_DOWNLEFT_LEFT;
            startingBoardState.BoardFields[1][4].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[1][4].DirectionType = AdugoDirectionType.UP_DOWN_LEFT;
            startingBoardState.BoardFields[2][4].Type = FieldType.DOG_PAWN;
            startingBoardState.BoardFields[2][4].DirectionType = AdugoDirectionType.UP_DOWN_UPLEFT_DOWNLEFT_LEFT;


            //empty pawns
            startingBoardState.BoardFields[3][0].DirectionType = AdugoDirectionType.UP_DOWN_RIGHT;
            startingBoardState.BoardFields[4][0].DirectionType = AdugoDirectionType.UP_UPRIGHT_RIGHT;
            startingBoardState.BoardFields[3][1].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;
            startingBoardState.BoardFields[4][1].DirectionType = AdugoDirectionType.UP_LEFT_RIGHT;
            startingBoardState.BoardFields[3][2].DirectionType = AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[4][2].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;
            startingBoardState.BoardFields[3][3].DirectionType = AdugoDirectionType.ALL_DIRECTIONS;
            startingBoardState.BoardFields[4][3].DirectionType = AdugoDirectionType.UP_LEFT_RIGHT;
            startingBoardState.BoardFields[3][4].DirectionType = AdugoDirectionType.UP_DOWN_LEFT;
            startingBoardState.BoardFields[4][4].DirectionType = AdugoDirectionType.UP_UPLEFT_LEFT;
            startingBoardState.BoardFields[5][2].DirectionType = AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
            startingBoardState.BoardFields[6][2].DirectionType = AdugoDirectionType.UP_LEFT_RIGHT;
            startingBoardState.BoardFields[5][1].DirectionType = AdugoDirectionType.UPRIGHT_RIGHT;
            startingBoardState.BoardFields[5][3].DirectionType = AdugoDirectionType.UPLEFT_LEFT;
            startingBoardState.BoardFields[6][1].DirectionType = AdugoDirectionType.UP_RIGHT;
            startingBoardState.BoardFields[6][3].DirectionType = AdugoDirectionType.UP_LEFT;
            //startingBoardState.BoardFields[0][4].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[0][6].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[1][3].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[1][4].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[1][5].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[2][3].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[2][4].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[2][5].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[2][6].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[3][3].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[3][4].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[3][5].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[4][3].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[4][4].Type = FieldType.RED_PAWN;
            //startingBoardState.BoardFields[4][6].Type = FieldType.RED_PAWN;
            return startingBoardState;
        }

        //returns true if targetFieldType is in the same team as playerPawn, false otherwise
        public static bool IsTargetSameType(FieldType playerPawn, FieldType targetFieldType)
        {
            if (playerPawn.Equals(FieldType.DOG_PAWN) && (targetFieldType.Equals(FieldType.DOG_PAWN))
                || (playerPawn.Equals(FieldType.JAGUAR_PAWN) && targetFieldType.Equals(FieldType.JAGUAR_PAWN)))
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
