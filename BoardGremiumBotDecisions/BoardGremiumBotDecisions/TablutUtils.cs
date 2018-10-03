using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumBotDecisions
{
    public class TablutUtils
    {
        public static int BOARD_WIDTH = 9;
        public static int BOARD_HEIGHT = 9;

        public static char RED_CHAR = 'R';
        public static char BLACK_CHAR = 'B';
        public static char KING_CHAR = 'K';
        public static char EMPTY_CHAR = 'E';


        public static bool PawnsInSameTeam(TablutFieldType t1, TablutFieldType t2)
        {
            if ((t1.Equals(t2)) ||
               (t1.Equals(TablutFieldType.RED_PAWN) && t2.Equals(TablutFieldType.KING)) ||
               (t1.Equals(TablutFieldType.KING) && t2.Equals(TablutFieldType.RED_PAWN)))
            {
                return true;
            }
            else return false;
        }

        public static DirectionEnum GetDirectionFromMove(Field source, Field destination)
        {
            if (source.X > destination.X)
                return DirectionEnum.LEFT;
            if (source.X < destination.X)
                return DirectionEnum.RIGHT;
            if (source.Y > destination.Y)
                return DirectionEnum.UP;
            else return DirectionEnum.DOWN;
        }

        public static int GetNumOfFieldsFromMove(Field source, Field destination, DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.UP:
                    {
                        return source.Y - destination.Y;
                    }
                case DirectionEnum.DOWN:
                    {
                        return destination.Y - source.Y;
                    }
                case DirectionEnum.LEFT:
                    {
                        return source.X - destination.X;
                    }
                case DirectionEnum.RIGHT:
                    {
                        return destination.X - source.X;
                    }
            }
            //won't happen
            return 0;
        }
    }
}
