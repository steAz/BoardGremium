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

        public static string FIRST_JOINED_STRING = "First joined";
        public static string FIRST_NOT_JOINED_STRING = "First not joined";

        public static string MINMAX_ALG_STRING = "MinMax";
        public static string NEGAMAX_ALG_STRING = "NegaMax";

        public static int INITIAL_BLACK_PAWNS_NUMBER = 16;
        public static int INITIAL_RED_PAWNS_NUMBER = 8;


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

        public static TablutFieldType EnemyType(TablutFieldType playerType)
        {
            if(playerType.Equals(TablutFieldType.BLACK_PAWN))
            {
                return TablutFieldType.RED_PAWN;
            }else if(playerType.Equals(TablutFieldType.RED_PAWN) || playerType.Equals(TablutFieldType.KING))
            {
                return TablutFieldType.BLACK_PAWN;
            }else
            {
                throw new ArgumentException("EMPTY_FIELD type in get EnemyType.");
            }
        }

        public static int InitialNumberOfPawns(TablutFieldType forPlayer)
        {
            if(forPlayer == TablutFieldType.BLACK_PAWN)
            {
                return INITIAL_BLACK_PAWNS_NUMBER;
            }else
            {
                return INITIAL_RED_PAWNS_NUMBER;
            }
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

        public static Bot BotInstanceFromAlg(BotAlgorithmsParameters botAlg, Game gameInstance, bool isFirstPlayerJoined)
        {
            string algName;
            int maxTreeDepth;
            if(!isFirstPlayerJoined)
            {
                algName = botAlg.FirstBotAlgorithmName;
                maxTreeDepth = botAlg.FirstBotMaxTreeDepth;
            }else
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
                return new MinMaxBot(gameInstance, maxTreeDepth);
            }
            else if (algName.Contains(NEGAMAX_ALG_STRING))
            {
                return new NegaMaxBot(gameInstance, maxTreeDepth);
            }
            else
            {
                Console.WriteLine("Wrong format og botAlgorithm string representation");
                return null;
            }

        }
    }
}
