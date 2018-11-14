using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BoardGremiumRESTservice;
using BoardGremiumRESTservice.Tablut;

namespace BoardGremiumRESTservice.Utils
{
    public static class MessagesConverterUtils
    {

        public static string RED_STRING = "RED";
        public static string BLACK_STRING = "BLACK";
        public static string JAGUAR_STRING = "JAGUAR";
        public static string DOG_STRING = "DOG";

        public static string HUMAN_STRING = "HUMAN";
        public static string BOT_STRING = "BOT";

        public static char RED_CHAR = 'R';
        public static char BLACK_CHAR = 'B';
        public static char KING_CHAR = 'K';
        public static char EMPTY_CHAR = 'E';
        public static char JAGUAR_CHAR = 'J';
        public static char DOG_CHAR = 'D';

        public static string UP_CHAR = "U";
        public static string DOWN_CHAR = "D";
        public static string LEFT_CHAR = "L";
        public static string RIGHT_CHAR = "R";
        public static string UP_LEFT_CHAR = "UL";
        public static string UP_RIGHT_CHAR = "UR";
        public static string DOWN_LEFT_CHAR = "DL";
        public static string DOWN_RIGHT_CHAR = "DR";

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
                    if ((FieldType)f.FieldType == FieldType.BLACK_PAWN)
                    {
                        result += BLACK_CHAR;
                    }
                    else if ((FieldType)f.FieldType == FieldType.RED_PAWN)
                    {
                        result += RED_CHAR;
                    }
                    else if ((FieldType)f.FieldType == FieldType.KING)
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
                    result.BoardFields[verticalIndex, horizontalIndex].FieldType = FieldType.BLACK_PAWN;
                }
                else if (character.Equals(RED_CHAR))
                {
                    result.BoardFields[verticalIndex, horizontalIndex].FieldType = FieldType.RED_PAWN;
                }
                else if (character.Equals(KING_CHAR))
                {
                    result.BoardFields[verticalIndex, horizontalIndex].FieldType = FieldType.KING;
                }
                else
                {
                    result.BoardFields[verticalIndex, horizontalIndex].FieldType = FieldType.EMPTY_FIELD;
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

        //move syntax:
        //var message = "move " + selectedField.X.ToString() + " " + selectedField.Y.ToString()
        //                   + " " + selectedDirection.ToString().First() + " " + selectedNumOfFields.ToString();
        public static TablutMove ConvertStringToTablutMove(string moveInfo, TablutGameState gameState)
        {
            string[] moveParams = moveInfo.Split(' ');
            int y = Int32.Parse(moveParams[1]);
            int x = Int32.Parse(moveParams[2]);
            DirectionEnum direction = DirectionFromChar(moveParams[3]);
            int numOfFields = Int32.Parse(moveParams[4]);
            return new TablutMove(x, y, direction, numOfFields, gameState);
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

        private static DirectionEnum DirectionFromChar(string directionChar)
        {
            if(directionChar.Equals(UP_CHAR))
            {
                return DirectionEnum.UP;
            }else if (directionChar.Equals(DOWN_CHAR))
            {
                return DirectionEnum.DOWN;
            }
            else if (directionChar.Equals(LEFT_CHAR))
            {
                return DirectionEnum.LEFT;
            }
            else if (directionChar.Equals(RIGHT_CHAR))
            {
                return DirectionEnum.RIGHT;
            }
            else if (directionChar.Equals(UP_LEFT_CHAR))
            {
                return DirectionEnum.UPLEFT;
            }
            else if (directionChar.Equals(UP_RIGHT_CHAR))
            {
                return DirectionEnum.UPRIGHT;
            }
            else if (directionChar.Equals(DOWN_LEFT_CHAR))
            {
                return DirectionEnum.DOWNLEFT;
            }
            else if (directionChar.Equals(DOWN_RIGHT_CHAR))
            {
                return DirectionEnum.DOWNRIGHT;
            }
            else
            {
                return DirectionEnum.NONE;
            }
        }
    }
}