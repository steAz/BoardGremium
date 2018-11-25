using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGremiumRESTservice.Adugo
{
    static class AdugoUtils
    {
        public static string UP_STRING = "UP";
        public static string DOWN_STRING = "DOWN";
        public static string LEFT_STRING = "LEFT";
        public static string RIGHT_STRING = "RIGHT";

        public static List<DirectionEnum> GetPossibleDirectionsFromDirectionType(AdugoField field)
        {
            var directions = new List<DirectionEnum>();
            if (field.DirectionType.ToString().Equals("ALL_DIRECTIONS"))
            {
                directions.AddRange(Enum.GetValues(typeof(DirectionEnum)).Cast<DirectionEnum>());
                directions.Remove(DirectionEnum.NONE);
            }
            else if (field.DirectionType.ToString().Equals("NONE"))
            {
                throw new ArgumentException("Pawn cannot stay on locked field");
            }

            var directionParams = field.DirectionType.ToString().Split('_');

            foreach (var directionString in directionParams)
            {
                if (directionString.Equals(UP_STRING))
                {
                    directions.Add(DirectionEnum.UP);
                }
                else if (directionString.Equals(DOWN_STRING))
                {
                    directions.Add(DirectionEnum.DOWN);
                }
                else if (directionString.Equals(LEFT_STRING))
                {
                    directions.Add(DirectionEnum.LEFT);
                }
                else if (directionString.Equals(RIGHT_STRING))
                {
                    directions.Add(DirectionEnum.RIGHT);
                }
                else if (directionString.Contains(UP_STRING))
                {
                    if (directionString.Contains(LEFT_STRING))
                        directions.Add(DirectionEnum.UPLEFT);
                    else if (directionString.Contains(RIGHT_STRING))
                        directions.Add(DirectionEnum.UPRIGHT);
                }
                else if (directionString.Contains(DOWN_STRING))
                {
                    if (directionString.Contains(LEFT_STRING))
                        directions.Add(DirectionEnum.DOWNLEFT);
                    else if (directionString.Contains(RIGHT_STRING))
                        directions.Add(DirectionEnum.DOWNRIGHT);
                }
            }

            return directions;
        }
    }
}