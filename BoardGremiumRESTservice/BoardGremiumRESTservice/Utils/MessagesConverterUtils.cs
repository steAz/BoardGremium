﻿using System;
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

        public static string HUMAN_STRING = "HUMAN";
        public static string BOT_STRING = "BOT";

        public static char RED_CHAR = 'R';
        public static char BLACK_CHAR = 'B';
        public static char KING_CHAR = 'K';
        public static char EMPTY_CHAR = 'E';

        public static string UP_CHAR = "U";
        public static string DOWN_CHAR = "D";
        public static string LEFT_CHAR = "L";
        public static string RIGHT_CHAR = "R";

        //message should be equal to "RED" or "BLACK"
        public static TablutFieldType PlayerPawnFromMessage(string message)
        {
            if(RED_STRING.Equals(message))
            {
                return TablutFieldType.RED_PAWN;
            } else if(BLACK_STRING.Equals(message))
            {
                return TablutFieldType.BLACK_PAWN;
            }else
            {
                throw new ArgumentException("Wrong format of string message while converting to TablutFieldType.");
            }
        }

        public static TablutFieldType EnemyPawnFromMessage(string message)
        {
            if (RED_STRING.Equals(message))
            {
                return TablutFieldType.BLACK_PAWN;
            }
            else if (BLACK_STRING.Equals(message))
            {
                return TablutFieldType.RED_PAWN;
            }
            else
            {
                throw new ArgumentException("Wrong format of string message while converting to TablutFieldType.");
            }
        }

        public static string MessageFromPlayerPawn(TablutFieldType playerPawn)
        {
            if(playerPawn.Equals(TablutFieldType.RED_PAWN))
            {
                return RED_STRING;
            }else if(playerPawn.Equals(TablutFieldType.BLACK_PAWN))
            {
                return BLACK_STRING;
            }else
            {
                throw new ArgumentException("Wrong value of Enum while converting from TablutFieldType");
            }
        }

        public static string ConvertTablutGameStateToString(TablutGameState tgs)
        {

            string result = "";
            if ((TablutFieldType)tgs.game.HumanPlayerFieldType == TablutFieldType.BLACK_PAWN)
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
                    if ((TablutFieldType)f.Type == TablutFieldType.BLACK_PAWN)
                    {
                        result += BLACK_CHAR;
                    }
                    else if ((TablutFieldType)f.Type == TablutFieldType.RED_PAWN)
                    {
                        result += RED_CHAR;
                    }
                    else if ((TablutFieldType)f.Type == TablutFieldType.KING)
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
            TablutFieldType playerType;
            if (playerPawnColor.Equals(BLACK_STRING))
            {
                playerType = TablutFieldType.BLACK_PAWN;
            }
            else
            {
                playerType = TablutFieldType.RED_PAWN;
            }
            //var enumerator = stringRepresentation.GetEnumerator();
            var enumerator = arguments[1].GetEnumerator();
            int horizontalIndex = 0, verticalIndex = 0;
            while (enumerator.MoveNext())
            {
                char character = enumerator.Current;
                if (character.Equals(BLACK_CHAR))
                {
                    result.BoardFields[verticalIndex, horizontalIndex].Type = TablutFieldType.BLACK_PAWN;
                }
                else if (character.Equals(RED_CHAR))
                {
                    result.BoardFields[verticalIndex, horizontalIndex].Type = TablutFieldType.RED_PAWN;
                }
                else if (character.Equals(KING_CHAR))
                {
                    result.BoardFields[verticalIndex, horizontalIndex].Type = TablutFieldType.KING;
                }
                else
                {
                    result.BoardFields[verticalIndex, horizontalIndex].Type = TablutFieldType.EMPTY_FIELD;
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
            int x = Int32.Parse(moveParams[1]);
            int y = Int32.Parse(moveParams[2]);
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
            }else
            {
                return DirectionEnum.NONE;
            }
        }
    }
}