using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumBotDecisions.Adugo
{
    static class AdugoUtils
    {
        public static int BOARD_WIDTH = 5;
        public static int BOARD_HEIGHT = 8;

        public static string EMPTY_CHAR = "E";
        public static string JAGUAR_CHAR = "J";
        public static string DOG_CHAR = "D";
        public static string LOCKED_CHAR = "L";

        public static string FIRST_JOINED_STRING = "First joined";
        public static string FIRST_NOT_JOINED_STRING = "First not joined";

        public static bool PawnsInSameTeam(FieldType t1, FieldType t2)
        {
            return t1.Equals(t2);
        }

        public static DirectionEnum GetDirectionFromMove(AdugoField source, AdugoField destination)
        {
            if (source.X > destination.X && source.Y > destination.Y)
                return DirectionEnum.UPLEFT;
            else if (source.X > destination.X && source.Y < destination.Y)
                return DirectionEnum.DOWNLEFT;
            else if (source.X < destination.X && source.Y < destination.Y)
                return DirectionEnum.DOWNRIGHT;
            else if (source.X < destination.X && source.Y > destination.Y)
                return DirectionEnum.UPRIGHT;
            else if (source.X > destination.X && source.Y == destination.Y)
                return DirectionEnum.LEFT;
            else if (source.X < destination.X && source.Y == destination.Y)
                return DirectionEnum.RIGHT;
            else if (source.Y > destination.Y && source.X == destination.X)
                return DirectionEnum.UP;
            else 
                return DirectionEnum.DOWN;
        }
    }
}
