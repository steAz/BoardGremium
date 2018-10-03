using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumBotDecisions
{
    public class MessagesConverter
    {
        public static string RED_STRING = "RED";
        public static string BLACK_STRING = "BLACK";

        //message should be equal to "RED" or "BLACK"
        public static TablutFieldType PlayerPawnFromMessage(string message)
        {
            if (RED_STRING.Equals(message))
            {
                return TablutFieldType.RED_PAWN;
            }
            else if (BLACK_STRING.Equals(message))
            {
                return TablutFieldType.BLACK_PAWN;
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
                    result.BoardFields[horizontalIndex, verticalIndex].Type = TablutFieldType.BLACK_PAWN;
                }
                else if (character.Equals(TablutUtils.RED_CHAR))
                {
                    result.BoardFields[horizontalIndex, verticalIndex].Type = TablutFieldType.RED_PAWN;
                }
                else if (character.Equals(TablutUtils.KING_CHAR))
                {
                    result.BoardFields[horizontalIndex, verticalIndex].Type = TablutFieldType.KING;
                }
                else
                {
                    result.BoardFields[horizontalIndex, verticalIndex].Type = TablutFieldType.EMPTY_FIELD;
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



    }
}
