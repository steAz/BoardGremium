using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BoardGremiumRESTservice;
using BoardGremiumRESTservice.Adugo;
using BoardGremiumRESTservice.Tablut;

namespace BoardGremiumRESTservice.Utils
{
    public static class MessagesConverterUtils
    {
        public static string TABLUT_STRING = "TABLUT";
        public static string ADUGO_STRING = "ADUGO";

        public static string RED_STRING = "RED";
        public static string BLACK_STRING = "BLACK";
        public static string JAGUAR_STRING = "JAGUAR";
        public static string DOG_STRING = "DOG";

        public static string HUMAN_STRING = "HUMAN";
        public static string BOT_STRING = "BOT";

        public static string RED_CHAR = "R";
        public static string BLACK_CHAR = "B";
        public static string KING_CHAR = "K";
        public static string EMPTY_CHAR = "E";
        public static string JAGUAR_CHAR= "J";
        public static string DOG_CHAR = "D";
        public static string LOCKED_CHAR = "L";

        public static string UP_STRING = "UP";
        public static string DOWN_STRING = "DOWN";
        public static string LEFT_STRING = "LEFT";
        public static string RIGHT_STRING = "RIGHT";

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

        public static string FIRST_JOINED_STRING = "First joined";
        public static string FIRST_NOT_JOINED_STRING = "First not joined";

        //message should be equal to "RED" or "BLACK"
        public static FieldType PlayerPawnFromMessage(string message)
        {
            if(RED_STRING.Equals(message))
            {
                return FieldType.RED_PAWN;
            } else if(BLACK_STRING.Equals(message))
            {
                return FieldType.BLACK_PAWN;
            }else if (JAGUAR_STRING.Equals(message))
            {
                return FieldType.JAGUAR_PAWN;
            }else if(DOG_STRING.Equals(message))
            {
                return FieldType.DOG_PAWN;
            }
            else
            {
                throw new ArgumentException("Wrong format of string message while converting to TablutFieldType.");
            }
        }

        public static FieldType EnemyPawnFromMessage(string message)
        {
            if (RED_STRING.Equals(message))
            {
                return FieldType.BLACK_PAWN;
            }
            else if (BLACK_STRING.Equals(message))
            {
                return FieldType.RED_PAWN;
            }
            else if (JAGUAR_STRING.Equals(message))
            {
                return FieldType.DOG_PAWN;
            }
            else if (DOG_STRING.Equals(message))
            {
                return FieldType.JAGUAR_PAWN;
            }
            else
            {
                throw new ArgumentException("Wrong format of string message while converting to TablutFieldType.");
            }
        }

        public static string MessageFromPlayerPawn(FieldType playerPawn)
        {
            if(playerPawn.Equals(FieldType.RED_PAWN))
            {
                return RED_STRING;
            }else if(playerPawn.Equals(FieldType.BLACK_PAWN))
            {
                return BLACK_STRING;
            }
            else if (playerPawn.Equals(FieldType.JAGUAR_PAWN))
            {
                return JAGUAR_STRING;
            }
            else if (playerPawn.Equals(FieldType.DOG_PAWN))
            {
                return DOG_STRING;
            }
            else
            {
                throw new ArgumentException("Wrong value of Enum while converting from TablutFieldType");
            }
        }

        public static string GetEnemyColor(string playerColor)
        {
            if(playerColor.Equals(RED_STRING))
            {
                return BLACK_STRING;
            }else if(playerColor.Equals(BLACK_STRING))
            {
                return RED_STRING;
            }
            else if (playerColor.Equals(DOG_STRING))
            {
                return JAGUAR_STRING;
            }
            else if (playerColor.Equals(JAGUAR_STRING))
            {
                return DOG_STRING;
            }
            else
            {
                throw new ArgumentException("Wrong value of playerColor while getting enemy color");
            }
        }

        public static string ConvertTablutGameStateToString(TablutGameState tgs)
        {
            string result = "";
            if ((FieldType)tgs.game.HumanPlayerFieldType == FieldType.BLACK_PAWN)
            {
                result += BLACK_STRING + ",";
            }
            else
            {
                result += RED_STRING + ",";
            }
            int boardWidth = tgs.game.BoardWidth;
            int boardHeight = tgs.game.BoardHeight;
            for(int i=0; i < boardHeight; i++)
            {
                for(int j=0; j < boardWidth; j++)
                {
                    Field f = tgs.game.currentBoardState.BoardFields[i, j];
                    if ((FieldType)f.Type == FieldType.BLACK_PAWN)
                    {
                        result += BLACK_CHAR;
                    }
                    else if ((FieldType)f.Type == FieldType.RED_PAWN)
                    {
                        result += RED_CHAR;
                    }
                    else if ((FieldType)f.Type == FieldType.KING)
                    {
                        result += KING_CHAR;
                    }
                    else
                    {
                        result += EMPTY_CHAR;
                    }
                }
            }
            return result;
        }

        public static string ConvertAdugoGameStateToString(AdugoGameState ags)
        {
            var result = "";
            if ((FieldType)ags.Game.HumanPlayerFieldType == FieldType.JAGUAR_PAWN)
            {
                result += JAGUAR_STRING + ",";
            }
            else
            {
                result += DOG_STRING + ",";
            }
            var boardWidth = ags.Game.BoardWidth;
            var boardHeight = ags.Game.BoardHeight;
            for (var y = 0; y < boardHeight; y++)
            {
                for (var x = 0; x < boardWidth; x++)
                {
                    var f = ags.Game.CurrentBoardState.BoardFields[y, x];
                    switch ((FieldType)f.Type)
                    {
                        case FieldType.JAGUAR_PAWN:
                            result += JAGUAR_CHAR;
                            break;
                        case FieldType.DOG_PAWN:
                            result += DOG_CHAR;
                            break;
                        case FieldType.LOCKED_FIELD:
                            result += LOCKED_CHAR;
                            break;
                        case FieldType.EMPTY_FIELD:
                            result += EMPTY_CHAR;
                            break;
                    }

                    result += '|'; // it separates FieldType from DirectionType
                    switch (f.DirectionType)
                    {
                        case AdugoDirectionType.ALL_DIRECTIONS:
                            result += ALL_DIRECTIONS_STRING;
                            break;
                        case AdugoDirectionType.DOWN_DOWNLEFT_DOWNRIGHT_LEFT_RIGHT:
                            result += DOWN_DOWNLEFT_DOWNRIGHT_LEFT_RIGHT_STRING;
                            break;
                        case AdugoDirectionType.DOWN_DOWNLEFT_LEFT:
                            result += DOWN_DOWNLEFT_LEFT_STRING;
                            break;
                        case AdugoDirectionType.DOWN_DOWNRIGHT_RIGHT:
                            result += DOWN_DOWNRIGHT_RIGHT_STRING;
                            break;
                        case AdugoDirectionType.DOWN_LEFT_RIGHT:
                            result += DOWN_LEFT_RIGHT_STRING;
                            break;
                        case AdugoDirectionType.DOWN_UPLEFT_LEFT:
                            result += DOWN_UPLEFT_LEFT_STRING;
                            break;
                        case AdugoDirectionType.DOWN_UPRIGHT_RIGHT:
                            result += DOWN_UPRIGHT_RIGHT_STRING;
                            break;
                        case AdugoDirectionType.UPLEFT_LEFT:
                            result += UPLEFT_LEFT_STRING;
                            break;
                        case AdugoDirectionType.UPRIGHT_RIGHT:
                            result += UPRIGHT_RIGHT_STRING;
                            break;
                        case AdugoDirectionType.UP_DOWN_LEFT:
                            result += UP_DOWN_LEFT_STRING;
                            break;
                        case AdugoDirectionType.UP_DOWN_LEFT_RIGHT:
                            result += UP_DOWN_LEFT_RIGHT_STRING;
                            break;
                        case AdugoDirectionType.UP_DOWN_RIGHT:
                            result += UP_DOWN_RIGHT_STRING;
                            break;
                        case AdugoDirectionType.UP_DOWN_UPLEFT_DOWNLEFT_LEFT:
                            result += UP_DOWN_UPLEFT_DOWNLEFT_LEFT_STRING;
                            break;
                        case AdugoDirectionType.UP_DOWN_UPRIGHT_DOWNRIGHT_RIGHT:
                            result += UP_DOWN_UPRIGHT_DOWNRIGHT_RIGHT_STRING;
                            break;
                        case AdugoDirectionType.UP_LEFT:
                            result += UP_LEFT_STRING;
                            break;
                        case AdugoDirectionType.UP_LEFT_RIGHT:
                            result += UP_LEFT_RIGHT_STRING;
                            break;
                        case AdugoDirectionType.UP_RIGHT:
                            result += UP_RIGHT_STRING;
                            break;
                        case AdugoDirectionType.UP_UPLEFT_LEFT:
                            result += UP_UPLEFT_LEFT_STRING;
                            break;
                        case AdugoDirectionType.UP_UPRIGHT_RIGHT:
                            result += UP_UPRIGHT_RIGHT_STRING;
                            break;
                        case AdugoDirectionType.NONE:
                            result += NONE_STRING;
                            break;
                        default:
                            throw new NullReferenceException("There was field with NONE direction type while converting AdugoGameState to BoardRepresentation");
                    }

                    result += '%'; // it separates tuples (FieldType|DirectionType)
                }
            }
            return result;
        }

        public static TablutGameState ConvertStringToTablutGameState(string stringRepresentation, string playerPawnColor)
        {
            BoardState result = new BoardState(TablutGame.BOARD_WIDTH, TablutGame.BOARD_HEIGHT);
            string[] arguments = stringRepresentation.Split(',');
            FieldType playerType;
            if (playerPawnColor.Equals(BLACK_STRING))
            {
                playerType = FieldType.BLACK_PAWN;
            }
            else
            {
                playerType = FieldType.RED_PAWN;
            }
            //var enumerator = stringRepresentation.GetEnumerator();
            var enumerator = arguments[1].GetEnumerator();
            int horizontalIndex = 0, verticalIndex = 0;
            while (enumerator.MoveNext())
            {
                char character = enumerator.Current;
                if (character.Equals(BLACK_CHAR))
                {
                    result.BoardFields[verticalIndex, horizontalIndex].Type = FieldType.BLACK_PAWN;
                }
                else if (character.Equals(RED_CHAR))
                {
                    result.BoardFields[verticalIndex, horizontalIndex].Type = FieldType.RED_PAWN;
                }
                else if (character.Equals(KING_CHAR))
                {
                    result.BoardFields[verticalIndex, horizontalIndex].Type = FieldType.KING;
                }
                else
                {
                    result.BoardFields[verticalIndex, horizontalIndex].Type = FieldType.EMPTY_FIELD;
                }

                horizontalIndex++;
                if (horizontalIndex >= TablutGame.BOARD_WIDTH)
                {
                    horizontalIndex = 0;
                    verticalIndex++;
                }

                if (verticalIndex >= TablutGame.BOARD_HEIGHT)
                {
                    //throw new ArgumentOutOfRangeException("Exception thrown while parsing BoardState string representation - string is too long");
                    break;
                }
            }
            return new TablutGameState(playerType, result);
        }

        public static AdugoGameState ConvertStringToAdugoGameState(string stringRepresentation, string playerPawnColor)
        {
            AdugoBoardState result = new AdugoBoardState(AdugoGameState.BoardWidth, AdugoGameState.BoardHeight);
            string[] arguments = stringRepresentation.Split(',');
            FieldType playerType;
            if (playerPawnColor.Equals(JAGUAR_STRING))
            {
                playerType = FieldType.JAGUAR_PAWN;
            }
            else
            {
                playerType = FieldType.DOG_PAWN;
            }
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
                    if (horizontalIndex >= AdugoGameState.BoardWidth)
                    {
                        horizontalIndex = 0;
                        verticalIndex++;
                    }

                    if (verticalIndex >= AdugoGameState.BoardHeight)
                    {
                        //throw new ArgumentOutOfRangeException("Exception thrown while parsing BoardState string representation - string is too long");
                        break;
                    }

                    tupleFieldTypeDirectionType = string.Empty;
                    tupleInProgress = true; // new tuple will be built in the next iterations
                }
            }
            return new AdugoGameState(playerType, result);
        }

        //move syntax:
        //var message = "move " + selectedField.X.ToString() + " " + selectedField.Y.ToString()
        //                   + " " + selectedDirection.ToString().First() + " " + selectedNumOfFields.ToString();
        public static TablutMove ConvertStringToTablutMove(string moveInfo, TablutGameState gameState)
        {
            string[] moveParams = moveInfo.Split(' ');
            int y = Int32.Parse(moveParams[1]);
            int x = Int32.Parse(moveParams[2]);
            DirectionEnum direction = DirectionFromString(moveParams[3]);
            int numOfFields = Int32.Parse(moveParams[4]);
            return new TablutMove(x, y, direction, numOfFields, gameState);
        }

        //move syntax:
        //var message = "move " + selectedField.X.ToString() + " " + selectedField.Y.ToString()
        //  + " " + selectedDirection.ToString()
        public static AdugoMove ConvertStringToAdugoMove(string moveInfo, AdugoGameState gameState)
        {
            var moveParams = moveInfo.Split(' ');
            var y = Int32.Parse(moveParams[1]);
            var x = Int32.Parse(moveParams[2]);
            var direction = DirectionFromString(moveParams[3]);
            return new AdugoMove(x, y, direction, gameState);
        }

        public static PlayerEnum PlayerEnumFromString(string playerString)
        {
            if(playerString.Equals(HUMAN_STRING))
            {
                return PlayerEnum.HUMAN_PLAYER;
            }else
            {
                return PlayerEnum.BOT_PLAYER;
            }
        }

        private static DirectionEnum DirectionFromString(string directionString)
        {
            if(directionString.Equals(UP_STRING))
            {
                return DirectionEnum.UP;
            }else if (directionString.Equals(DOWN_STRING))
            {
                return DirectionEnum.DOWN;
            }
            else if (directionString.Equals(LEFT_STRING))
            {
                return DirectionEnum.LEFT;
            }
            else if (directionString.Equals(RIGHT_STRING))
            {
                return DirectionEnum.RIGHT;
            }
            else if (directionString.Contains(UP_STRING))
            {
                if(directionString.Contains(LEFT_STRING))
                    return DirectionEnum.UPLEFT;
                else if(directionString.Contains(RIGHT_STRING))
                    return DirectionEnum.UPRIGHT;
            }
            else if (directionString.Contains(DOWN_STRING))
            {
                if(directionString.Contains(LEFT_STRING))
                    return DirectionEnum.DOWNLEFT;
                else if(directionString.Contains(RIGHT_STRING))
                    return DirectionEnum.DOWNRIGHT;
            }

            return DirectionEnum.NONE;
        }
    }
}