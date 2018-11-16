using AbstractGame;
using BoardGremiumBotDecisions.Tablut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGremiumBotDecisions.Adugo;

namespace BoardGremiumBotDecisions
{
    public class MessagesConverter
    {

        public static string TABLUT_STRING = "TABLUT";
        public static string ADUGO_STRING = "ADUGO";

        public static string RED_STRING = "RED";
        public static string BLACK_STRING = "BLACK";
        public static string JAGUAR_STRING = "JAGUAR";
        public static string DOG_STRING = "DOG";

        public static string EMPTY_CHAR = "E";
        public static string JAGUAR_CHAR = "J";
        public static string DOG_CHAR = "D";
        public static string LOCKED_CHAR = "L";

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

        //message should be equal to "RED" or "BLACK"
        public static FieldType PlayerPawnFromMessage(string message)
        {
            if (RED_STRING.Equals(message))
            {
                return FieldType.RED_PAWN;
            }
            else if (BLACK_STRING.Equals(message))
            {
                return FieldType.BLACK_PAWN;
            }
            else if (JAGUAR_STRING.Equals(message))
            {
                return FieldType.JAGUAR_PAWN;
            }
            else if (DOG_STRING.Equals(message))
            {
                return FieldType.DOG_PAWN;
            }
            else
            {
                throw new ArgumentException("Wrong format of string message while converting to TablutFieldType.");
            }
        }

        public static BoardState ConvertStringToTablutBoardState(string stringRepresentation)
        {
            BoardState result = new BoardState(TablutUtils.BOARD_WIDTH, TablutUtils.BOARD_HEIGHT);
            string[] arguments = stringRepresentation.Split(',');
            var enumerator = arguments[1].GetEnumerator();

            int horizontalIndex = 0, verticalIndex = 0;
            while (enumerator.MoveNext())
            {
                char character = enumerator.Current;
                if (character.Equals(TablutUtils.BLACK_CHAR))
                {
                    result.BoardFields[horizontalIndex, verticalIndex].Type = FieldType.BLACK_PAWN;
                }
                else if (character.Equals(TablutUtils.RED_CHAR))
                {
                    result.BoardFields[horizontalIndex, verticalIndex].Type = FieldType.RED_PAWN;
                }
                else if (character.Equals(TablutUtils.KING_CHAR))
                {
                    result.BoardFields[horizontalIndex, verticalIndex].Type = FieldType.KING;
                }
                else
                {
                    result.BoardFields[horizontalIndex, verticalIndex].Type = FieldType.EMPTY_FIELD;
                }

                horizontalIndex++;
                if (horizontalIndex >= TablutUtils.BOARD_WIDTH)
                {
                    horizontalIndex = 0;
                    verticalIndex++;
                }

                if (verticalIndex >= TablutUtils.BOARD_HEIGHT)
                {
                    //throw new ArgumentOutOfRangeException("Exception thrown while parsing BoardState string representation - string is too long");
                    break;
                }
            }

            return result;
        }

        public static AdugoBoardState ConvertStringToAdugoBoardState(string stringRepresentation)
        {
            AdugoBoardState result = new AdugoBoardState(AdugoUtils.BOARD_WIDTH, AdugoUtils.BOARD_HEIGHT);
            string[] arguments = stringRepresentation.Split(',');

            //var enumerator = stringRepresentation.GetEnumerator();
            var enumerator = arguments[1].GetEnumerator();
            int horizontalIndex = 0, verticalIndex = 0;
            var tupleInProgress = true; // string starts with tuple
            var tupleFieldTypeDirectionType = string.Empty;
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
                        result.BoardFields[verticalIndex, horizontalIndex].Type = FieldType.JAGUAR_PAWN;
                    }
                    else if (fieldTypeChar.Equals(DOG_CHAR))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].Type = FieldType.DOG_PAWN;
                    }
                    else if (fieldTypeChar.Equals(LOCKED_CHAR))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].Type = FieldType.LOCKED_FIELD;
                    }
                    else if (fieldTypeChar.Equals(EMPTY_CHAR))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].Type = FieldType.EMPTY_FIELD;
                    }

                    if (directionTypeString.Equals(UP_UPRIGHT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_UPRIGHT_RIGHT;
                    }
                    else if (directionTypeString.Equals(UPLEFT_LEFT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.UPLEFT_LEFT;
                    }
                    else if (directionTypeString.Equals(ALL_DIRECTIONS_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.ALL_DIRECTIONS;
                    }
                    else if (directionTypeString.Equals(UP_DOWN_LEFT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_DOWN_LEFT_RIGHT;
                    }
                    else if (directionTypeString.Equals(UP_DOWN_LEFT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_DOWN_LEFT;
                    }
                    else if (directionTypeString.Equals(UPRIGHT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.UPRIGHT_RIGHT;
                    }
                    else if (directionTypeString.Equals(UP_DOWN_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_DOWN_RIGHT;
                    }
                    else if (directionTypeString.Equals(UP_LEFT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_LEFT_RIGHT;
                    }
                    else if (directionTypeString.Equals(UP_DOWN_UPLEFT_DOWNLEFT_LEFT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_DOWN_UPLEFT_DOWNLEFT_LEFT;
                    }
                    else if (directionTypeString.Equals(UP_DOWN_UPRIGHT_DOWNRIGHT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_DOWN_UPRIGHT_DOWNRIGHT_RIGHT;
                    }
                    else if (directionTypeString.Equals(UP_LEFT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_LEFT;
                    }
                    else if (directionTypeString.Equals(UP_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_RIGHT;
                    }
                    else if (directionTypeString.Equals(UP_UPLEFT_LEFT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.UP_UPLEFT_LEFT;
                    }
                    else if (directionTypeString.Equals(DOWN_DOWNLEFT_LEFT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.DOWN_DOWNLEFT_LEFT;
                    }
                    else if (directionTypeString.Equals(DOWN_DOWNRIGHT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.DOWN_DOWNRIGHT_RIGHT;
                    }
                    else if (directionTypeString.Equals(DOWN_LEFT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.DOWN_LEFT_RIGHT;
                    }
                    else if (directionTypeString.Equals(DOWN_UPLEFT_LEFT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.DOWN_UPLEFT_LEFT;
                    }
                    else if (directionTypeString.Equals(DOWN_UPRIGHT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.DOWN_UPRIGHT_RIGHT;
                    }
                    else if (directionTypeString.Equals(DOWN_DOWNLEFT_DOWNRIGHT_LEFT_RIGHT_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.DOWN_DOWNLEFT_DOWNRIGHT_LEFT_RIGHT;
                    }
                    else if (directionTypeString.Equals(NONE_STRING))
                    {
                        result.BoardFields[verticalIndex, horizontalIndex].DirectionType =
                            AdugoDirectionType.NONE;
                    }

                    horizontalIndex++;
                    if (horizontalIndex >= AdugoUtils.BOARD_WIDTH)
                    {
                        horizontalIndex = 0;
                        verticalIndex++;
                    }

                    if (verticalIndex >= AdugoUtils.BOARD_HEIGHT)
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
