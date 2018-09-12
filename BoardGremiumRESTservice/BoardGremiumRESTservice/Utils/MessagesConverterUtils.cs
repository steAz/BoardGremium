using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BoardGremiumRESTservice;

namespace BoardGremiumRESTservice.Utils
{
    public class MessagesConverterUtils
    {

        public static string RED_STRING = "RED";
        public static string BLACK_STRING = "BLACK";

        public static string HUMAN_STRING = "HUMAN";
        public static string BOT_STRING = "BOT";

        public static char RED_CHAR = 'R';
        public static char BLACK_CHAR = 'B';
        public static char KING_CHAR = 'K';
        public static char EMPTY_CHAR = 'E';

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
            foreach (Field f in tgs.game.currentBoardState.BoardFields)
            {
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
            var enumerator = stringRepresentation.GetEnumerator();
            int horizontalIndex = 0, verticalIndex = 0;
            do
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
                    throw new ArgumentOutOfRangeException("Exception thrown while parsing BoardState string representation - string is too long");
                }
            } while (enumerator.MoveNext());
            return new TablutGameState(playerType, result);
        }
    }
}