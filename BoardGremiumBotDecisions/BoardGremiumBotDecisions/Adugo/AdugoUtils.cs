using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGremiumBotDecisions.AI.Adugo;
using BoardGremiumBotDecisions.AI.Tablut;

namespace BoardGremiumBotDecisions.Adugo
{
    static class AdugoUtils
    {
        public static int BOARD_WIDTH = 5;
        public static int BOARD_HEIGHT = 7;

        public static string EMPTY_CHAR = "E";
        public static string JAGUAR_CHAR = "J";
        public static string DOG_CHAR = "D";
        public static string LOCKED_CHAR = "L";

        public static string FIRST_JOINED_STRING = "First joined";
        public static string FIRST_NOT_JOINED_STRING = "First not joined";

        public static int INITIAL_DOG_PAWNS_NUMBER = 14;
        public static int INITIAL_JAGUAR_PAWNS_NUMBER = 1;

        public static string UP_STRING = "UP";
        public static string DOWN_STRING = "DOWN";
        public static string LEFT_STRING = "LEFT";
        public static string RIGHT_STRING = "RIGHT";

        public static string MINMAX_ALG_STRING = "MinMax";
        public static string NEGAMAX_ALG_STRING = "NegaMax";

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

        public static int InitialNumberOfPawns(FieldType forPlayer)
        {
            return forPlayer == FieldType.JAGUAR_PAWN ? INITIAL_JAGUAR_PAWNS_NUMBER : INITIAL_DOG_PAWNS_NUMBER;
        }

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

        public static AdugoBot BotInstanceFromAlg(BotAlgorithmsParameters botAlg, AdugoGame gameInstance, bool isFirstPlayerJoined)
        {
            string algName;
            int maxTreeDepth;
            if (!isFirstPlayerJoined)
            {
                algName = botAlg.FirstBotAlgorithmName;
                maxTreeDepth = botAlg.FirstBotMaxTreeDepth;
            }
            else
            {
                if (botAlg.IsBot2BotGame)
                {
                    algName = botAlg.SecBotAlgorithmName;
                    maxTreeDepth = botAlg.SecBotMaxTreeDepth;
                }
                else
                {
                    algName = botAlg.FirstBotAlgorithmName;
                    maxTreeDepth = botAlg.FirstBotMaxTreeDepth;
                }
            }

            if (algName.Contains(MINMAX_ALG_STRING))
            {
                return new MinMaxAdugoBot(gameInstance, maxTreeDepth);
            }
            else if (algName.Contains(NEGAMAX_ALG_STRING)) // ALFABETA ...
            {
                //return new NegaMaxBot(gameInstance, maxTreeDepth);
                return new MinMaxAdugoBot(gameInstance, maxTreeDepth);
            }
            
            Console.WriteLine("Wrong format og botAlgorithm string representation");
            return null;
           
        }

    }
}
