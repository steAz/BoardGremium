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
        public static string JAGUAR_CHAR = "J";
        public static string DOG_CHAR = "D";
        public static string LOCKED_CHAR = "L";

        public static string JAGUAR_STRING = "JAGUAR";
        public static string DOG_STRING = "DOG";

        public static string ALL_DIRECTIONS_STRING = "AD";
        public static string UP_UPLEFT_LEFT_STRING = "U_UL_L";
        public static string UP_UPRIGHT_RIGHT_STRING = "U_UR_R";
        public static string UP_LEFT_RIGHT_STRING = "U_L_R";
        public static string UP_DOWN_LEFT_RIGHT_STRING = "U_D_L_R";
        public static string UP_DOWN_LEFT_STRING = "U_D_L";
        public static string UP_DOWN_RIGHT_STRING = "U_D_R";
        public static string DOWN_DOWNLEFT_LEFT_STRING = "D_DL_L";
        public static string DOWN_DOWNRIGHT_RIGHT_STRING = "D_DR_R";
        public static string DOWN_LEFT_RIGHT_STRING = "D_L_R";
        public static string DOWN_UPLEFT_LEFT_STRING = "D_UL_L";
        public static string DOWN_UPRIGHT_RIGHT_STRING = "D_UR_R";
        public static string DOWN_DOWNLEFT_DOWNRIGHT_LEFT_RIGHT_STRING = "D_DL_DR_L_R";
        public static string UP_DOWN_UPRIGHT_DOWNRIGHT_RIGHT_STRING = "U_D_UR_DR_R";
        public static string UP_DOWN_UPLEFT_DOWNLEFT_LEFT_STRING = "U_D_UL_DL_L";
        public static string UP_LEFT_STRING = "U_L";
        public static string UP_RIGHT_STRING = "U_R";
        public static string UPRIGHT_RIGHT_STRING = "UR_R";
        public static string UPLEFT_LEFT_STRING = "UL_L";
        public static string NONE_STRING = "N";


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

        public static AdugoBoardState ConvertStringToAdugoBoardState(string stringRepresentation)
        {
            AdugoBoardState result = new AdugoBoardState(BOARD_WIDTH, BOARD_HEIGHT);
            string[] arguments = stringRepresentation.Split(',');

            //var enumerator = stringRepresentation.GetEnumerator();
            var enumerator = arguments[1].GetEnumerator();
            int horizontalIndex = 0, verticalIndex = 0;
            bool tupleInProgress = true; // string starts with tuple
            string tupleFieldTypeDirectionType = string.Empty;
            while (enumerator.MoveNext())
            {
                var character = enumerator.Current;
                if (character == '%')
                {
                    tupleInProgress = false;
                }

                if (tupleInProgress)
                {
                    tupleFieldTypeDirectionType += character; // building string: FieldType_CHAR|DirectionType_STRING
                }
                else // after building tuple, we can fill in AdugoField in BoardFields
                {
                    var tupleParams = tupleFieldTypeDirectionType.Split('|'); // 0 - FieldType_CHAR,  1 - DirectionType_STRING,
                    var fieldTypeChar = tupleParams[0];
                    var directionTypeString = tupleParams[1];

                    if (fieldTypeChar.Equals(JAGUAR_CHAR))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].Type = FieldType.JAGUAR_PAWN;
                    }
                    else if (fieldTypeChar.Equals(DOG_CHAR))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].Type = FieldType.DOG_PAWN;
                    }
                    else if (fieldTypeChar.Equals(LOCKED_CHAR))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].Type = FieldType.LOCKED_FIELD;
                    }
                    else if (fieldTypeChar.Equals(EMPTY_CHAR))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].Type = FieldType.EMPTY_FIELD;
                    }

                    if (directionTypeString.Equals(UP_UPRIGHT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_UPRIGHT_RIGHT;
                    }
                    else if (directionTypeString.Equals(UPLEFT_LEFT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.UPLEFT_LEFT;
                    }
                    else if (directionTypeString.Equals(ALL_DIRECTIONS_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.ALL_DIRECTIONS;
                    }
                    else if (directionTypeString.Equals(UP_DOWN_LEFT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
                    }
                    else if (directionTypeString.Equals(UP_DOWN_LEFT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_DOWN_LEFT;
                    }
                    else if (directionTypeString.Equals(UPRIGHT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.UPRIGHT_RIGHT;
                    }
                    else if (directionTypeString.Equals(UP_DOWN_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_DOWN_RIGHT;
                    }
                    else if (directionTypeString.Equals(UP_LEFT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_LEFT_RIGHT;
                    }
                    else if (directionTypeString.Equals(UP_DOWN_UPLEFT_DOWNLEFT_LEFT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_DOWN_UPLEFT_DOWNLEFT_LEFT;
                    }
                    else if (directionTypeString.Equals(UP_DOWN_UPRIGHT_DOWNRIGHT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_DOWN_UPRIGHT_DOWNRIGHT_RIGHT;
                    }
                    else if (directionTypeString.Equals(UP_LEFT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_LEFT;
                    }
                    else if (directionTypeString.Equals(UP_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_RIGHT;
                    }
                    else if (directionTypeString.Equals(UP_UPLEFT_LEFT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_UPLEFT_LEFT;
                    }
                    else if (directionTypeString.Equals(DOWN_DOWNLEFT_LEFT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.DOWN_DOWNLEFT_LEFT;
                    }
                    else if (directionTypeString.Equals(DOWN_DOWNRIGHT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.DOWN_DOWNRIGHT_RIGHT;
                    }
                    else if (directionTypeString.Equals(DOWN_LEFT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.DOWN_LEFT_RIGHT;
                    }
                    else if (directionTypeString.Equals(DOWN_UPLEFT_LEFT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.DOWN_UPLEFT_LEFT;
                    }
                    else if (directionTypeString.Equals(DOWN_UPRIGHT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.DOWN_UPRIGHT_RIGHT;
                    }
                    else if (directionTypeString.Equals(DOWN_DOWNLEFT_DOWNRIGHT_LEFT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.DOWN_DOWNLEFT_DOWNRIGHT_LEFT_RIGHT;
                    }
                    else if (directionTypeString.Equals(NONE_STRING))
                    {
                        result.BoardFields[verticalIndex][horizontalIndex].DirectionType =
                            AdugoDirectionType.NONE;
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

                    tupleFieldTypeDirectionType = string.Empty;
                    tupleInProgress = true; // new tuple will be built in the next iterations
                }
            }

            return result;
        }
    }
}
