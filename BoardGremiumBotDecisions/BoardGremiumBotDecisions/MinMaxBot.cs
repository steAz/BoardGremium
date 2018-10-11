using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumBotDecisions
{


    public class MinMaxBot : Bot
    {


        public enum TraversingLevel
        {
            MAX,
            MIN
        }


        public MinMaxBot(Game game) :base(game)
        {
        }

        private int MaxMinEvaluateState(BoardState currBoardState, int currDepth, int maxDepth, TraversingLevel typeOfLevel)
        {
            if (currDepth == maxDepth)
            {
                return Heuristic(currBoardState);
            }

            //Console.WriteLine("dzwierzenie " + currDepth + "na typie " + typeOfLevel.ToString());

            int bestHeuristic = 0;

            List<BoardState> possibleBoardStates = null;
            if (typeOfLevel == TraversingLevel.MAX)
            {
                possibleBoardStates = Game.GetPossibleBoardStates(currBoardState, this.Game.BotPlayerFieldType);
                bestHeuristic = -80000;
                int prevBestHeuristic = -80000;
                foreach (BoardState possibleBoardState in possibleBoardStates.AsEnumerable().Reverse())
                {
                    var heuristic = MaxMinEvaluateState(possibleBoardState, currDepth + 1, maxDepth, GetNextTraversingLevel(typeOfLevel));
                    
                    if (heuristic >= bestHeuristic)
                    {
                        //if(heuristic != bestHeuristic)
                            prevBestHeuristic = bestHeuristic;
                        bestHeuristic = heuristic;
                        if(currDepth == 0)
                        {
                            this.TheBestBoardState = (BoardState)possibleBoardState.Clone();
                            this.TheBestDepth = currDepth;
                            if(bestHeuristic != prevBestHeuristic)
                            {
                                this.AllTheBestBoardStates.Clear();
                            }
                            this.AllTheBestBoardStates.Add(TheBestBoardState);

                        }
                    }
                }
            }
            else if (typeOfLevel == TraversingLevel.MIN)
            {
                possibleBoardStates = Game.GetPossibleBoardStates(currBoardState, this.Game.HumanPlayerFieldType);
                bestHeuristic = 80000;

                foreach (BoardState possibleBoardState in possibleBoardStates)
                {
                    var heuristic = MaxMinEvaluateState(possibleBoardState, currDepth + 1, maxDepth, GetNextTraversingLevel(typeOfLevel));

                    if (heuristic <= bestHeuristic)
                    {
                        bestHeuristic = heuristic;
                        //this.TheBestBoardState = (BoardState)possibleBoardState.Clone();
                    }
                }
            }

            return bestHeuristic;
        }

        private TraversingLevel GetNextTraversingLevel(TraversingLevel typeOfLevel)
        {
            if (typeOfLevel == TraversingLevel.MAX) return TraversingLevel.MIN;
            else return TraversingLevel.MAX;
        }

        public override int Heuristic(BoardState bs)
        {
            TablutFieldType enemyType = (TablutFieldType)Game.HumanPlayerFieldType;
            
            if(enemyType == TablutFieldType.RED_PAWN)
            {
                return HeuristicForBlack(bs) - HeuristicForRed(bs);
            }else
            {
                return HeuristicForRed(bs) - HeuristicForBlack(bs);
            }
        }

        protected int HeuristicForBlack(BoardState bs)
        {
            int resultBlack = 0;

            resultBlack = TablutUtils.InitialNumberOfPawns(TablutFieldType.RED_PAWN) * 150 - (bs.NumberOfPawnsForPlayer(TablutFieldType.RED_PAWN) * 150); // the less enemy'pawns on the board, the better black's heuristic is

            var kingField = bs.GetKingTablutField();

            var fieldNextToKingDirectionUP = bs.AdjacentField(kingField, DirectionEnum.UP);
            var fieldNextToKingDirectionRIGHT = bs.AdjacentField(kingField, DirectionEnum.RIGHT);
            var fieldNextToKingDirectionLEFT = bs.AdjacentField(kingField, DirectionEnum.LEFT);
            var fieldNextToKingDirectionDOWN = bs.AdjacentField(kingField, DirectionEnum.DOWN);

            int blacksNextToKing = 0;
            if (fieldNextToKingDirectionUP != null && fieldNextToKingDirectionUP.Type == TablutFieldType.BLACK_PAWN)
            {
                blacksNextToKing++;
            }
            if (fieldNextToKingDirectionDOWN != null && fieldNextToKingDirectionDOWN.Type == TablutFieldType.BLACK_PAWN)
            {
                blacksNextToKing++;
            }
            if (fieldNextToKingDirectionLEFT != null && fieldNextToKingDirectionLEFT.Type == TablutFieldType.BLACK_PAWN)
            {
                blacksNextToKing++;
            }
            if (fieldNextToKingDirectionRIGHT != null && fieldNextToKingDirectionRIGHT.Type == TablutFieldType.BLACK_PAWN)
            {
                blacksNextToKing++;
            }

            if (blacksNextToKing == 1)
                resultBlack += 50;
            else if (blacksNextToKing == 2)
                resultBlack += 80;
            else if (blacksNextToKing == 3)
                resultBlack += 200;
            else if (blacksNextToKing == 4)
                resultBlack += 500;


            return resultBlack;
        }

        protected int HeuristicForRed(BoardState bs)
        {
            var resultRed = 0;
            resultRed = TablutUtils.InitialNumberOfPawns(TablutFieldType.BLACK_PAWN) * 100 - (bs.NumberOfPawnsForPlayer(TablutFieldType.BLACK_PAWN) * 100); // the less enemy'pawns on the board, the better black's heuristic is

            int minimumDistance = bs.ClosestCornerDistanceFromKing();

            if (minimumDistance == 7)
                resultRed += 20;
            else if (minimumDistance == 6)
                resultRed += 50;
            else if (minimumDistance == 5)
                resultRed += 90;
            else if (minimumDistance == 4)
                resultRed += 150;
            else if (minimumDistance == 3)
                resultRed += 220;
            else if (minimumDistance == 2)
                resultRed += 300;
            else if (minimumDistance == 1)
                resultRed += 400;
            else if (minimumDistance == 0)
                resultRed += 500;

            return resultRed;
        }

        public override BoardState MakeMove()
        {
            MaxMinEvaluateState(Game.currentBoardState, 0, 3, TraversingLevel.MAX);  // on output BestBoardState is set
            Console.WriteLine("best depth: " + this.TheBestDepth);
            Random rng = new Random();
            return this.AllTheBestBoardStates[rng.Next(AllTheBestBoardStates.Count)];
        }
    }
}
