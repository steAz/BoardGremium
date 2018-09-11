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

        //message should be equal to "RED" or "BLACK"
        public static TablutFieldType playerPawnFromMessage(string message)
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

        public static string messageFromPlayerPawn(TablutFieldType playerPawn)
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
    }
}