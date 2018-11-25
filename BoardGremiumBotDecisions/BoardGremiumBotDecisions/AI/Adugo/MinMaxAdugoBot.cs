using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;
using BoardGremiumBotDecisions.Adugo;
using BoardGremiumBotDecisions.Tablut;

namespace BoardGremiumBotDecisions.AI.Adugo
{
    public class MinMaxAdugoBot : AdugoBot
    {
        public int MaxTreeDepth { get; set; }

        public MinMaxAdugoBot(AdugoGame game, int maxTreeDepth) :base(game)
        {
            MaxTreeDepth = maxTreeDepth;
        }

        private int MaxMinEvaluateState(AdugoBoardState currBoardState, int currDepth, int maxDepth, TraversingLevel typeOfLevel)
        {
            if (currDepth == maxDepth)
            {
                return Heuristic(currBoardState);
            }

            //Console.WriteLine("dzwierzenie " + currDepth + "na typie " + typeOfLevel.ToString());

            int bestHeuristic = 0;

            List<AdugoBoardState> possibleBoardStates = null;
            if (typeOfLevel == TraversingLevel.MAX)
            {
                possibleBoardStates = Game.GetPossibleAdugoBoardStates(currBoardState, this.Game.BotPlayerFieldType);
                bestHeuristic = -80000;
                int prevBestHeuristic = -80000;
                foreach (var possibleBoardState in possibleBoardStates.AsEnumerable())
                {
                    var heuristic = MaxMinEvaluateState(possibleBoardState, currDepth + 1, maxDepth, GetNextTraversingLevel(typeOfLevel));

                    if (heuristic >= bestHeuristic)
                    {
                        //if(heuristic != bestHeuristic)
                        prevBestHeuristic = bestHeuristic;
                        bestHeuristic = heuristic;
                        if (currDepth == 0)
                        {
                            this.TheBestBoardState = (AdugoBoardState)possibleBoardState.Clone();
                            this.TheBestDepth = currDepth;
                            if (bestHeuristic != prevBestHeuristic)
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
                possibleBoardStates = Game.GetPossibleAdugoBoardStates(currBoardState, this.Game.HumanPlayerFieldType);
                bestHeuristic = 80000;

                foreach (var possibleBoardState in possibleBoardStates)
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

        public override int Heuristic(AdugoBoardState bs)
        {
            FieldType enemyType = (FieldType)Game.HumanPlayerFieldType;

            if (enemyType == FieldType.JAGUAR_PAWN)
            {
                return HeuristicForDog(bs) - HeuristicForJaguar(bs);
            }
            else
            {
                return HeuristicForJaguar(bs) - HeuristicForDog(bs);
            }
        }

        protected int HeuristicForDog(AdugoBoardState bs)
        {
            int resultDog = 0;

            var numOfEmptyFieldsNextToJaguar = bs.NumOfEmptyFieldsNextToJaguar();


            if(numOfEmptyFieldsNextToJaguar == 0)
                resultDog += 500;
            else if (numOfEmptyFieldsNextToJaguar == 1)
                resultDog += 450;
            else if (numOfEmptyFieldsNextToJaguar == 2)
                resultDog += 400;
            else if (numOfEmptyFieldsNextToJaguar == 3)
                resultDog += 350;
            else if (numOfEmptyFieldsNextToJaguar == 4)
                resultDog += 300;
            else if (numOfEmptyFieldsNextToJaguar == 5)
                resultDog += 230;
            else if (numOfEmptyFieldsNextToJaguar == 6)
                resultDog += 180;
            else if (numOfEmptyFieldsNextToJaguar == 7)
                resultDog += 100;
            else if (numOfEmptyFieldsNextToJaguar == 8)
                resultDog += 0;


            return resultDog;
        }



        protected int HeuristicForJaguar(AdugoBoardState bs)
        {
            var resultJaguar = 0;
            var takenDogsNumber = AdugoUtils.InitialNumberOfPawns(FieldType.DOG_PAWN) - bs.NumberOfDogsOnBoard();
            resultJaguar = takenDogsNumber * 100;
            return resultJaguar;
        }

        public override AdugoBoardState MakeMove()
        {
            MaxMinEvaluateState(Game.CurrentBoardState, 0, MaxTreeDepth, TraversingLevel.MAX);  // on output BestBoardState is set
            Console.WriteLine("best depth: " + this.TheBestDepth);
            var rng = new Random();
            return this.AllTheBestBoardStates[rng.Next(AllTheBestBoardStates.Count)];
        }
    }
}
