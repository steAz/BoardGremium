using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGremiumBotDecisions.AI.Tablut;
using BoardGremiumBotDecisions.Tablut;

namespace BoardGremiumBotDecisions
{
    public class NegaMaxBot : MinMaxBot
    {
        public NegaMaxBot(Game game, int maxTreeDepth) :base(game, maxTreeDepth)
        {

        }

        public int NegaMaxEvaluateState(TablutBoardState currTablutBoardState, int currDepth, int maxDepth)
        {
            if (currDepth == maxDepth)
            {
                return Heuristic(currTablutBoardState);
            }

            int bestHeuristic = 0;

            List<TablutBoardState> possibleBoardStates = null;

            possibleBoardStates = Game.GetPossibleBoardStates(currTablutBoardState, this.Game.BotPlayerFieldType);
            bestHeuristic = -80000;
            int prevBestHeuristic = -80000;
            foreach (TablutBoardState possibleBoardState in possibleBoardStates.AsEnumerable())
            {
                var heuristic = - NegaMaxEvaluateState(possibleBoardState, currDepth + 1, maxDepth);

                if (heuristic >= bestHeuristic)
                {
                    //if(heuristic != bestHeuristic)
                    prevBestHeuristic = bestHeuristic;
                    bestHeuristic = heuristic;
                    if (currDepth == 0)
                    {
                        this.TheBestTablutBoardState = (TablutBoardState)possibleBoardState.Clone();
                        this.TheBestDepth = currDepth;
                        if (bestHeuristic != prevBestHeuristic)
                        {
                            this.AllTheBestBoardStates.Clear();
                        }
                        this.AllTheBestBoardStates.Add(TheBestTablutBoardState);

                    }
                }
            }

            return bestHeuristic;
        }

        public int NegaMaxEvaluateState2(TablutBoardState currTablutBoardState, int currDepth, int maxDepth, int pointOfView)
        {
            if (currDepth == maxDepth)
            {
                return pointOfView * Heuristic(currTablutBoardState);
            }

            int bestHeuristic = 0;

            List<TablutBoardState> possibleBoardStates = null;
            FieldType playerFieldType;
            if (pointOfView == 1)
                playerFieldType = this.Game.BotPlayerFieldType;
            else
                playerFieldType = this.Game.HumanPlayerFieldType;

            possibleBoardStates = Game.GetPossibleBoardStates(currTablutBoardState, playerFieldType);
            bestHeuristic = -80000;
            int prevBestHeuristic = -80000;
            foreach (TablutBoardState possibleBoardState in possibleBoardStates.AsEnumerable().Reverse())
            {
                var heuristic = - NegaMaxEvaluateState2(possibleBoardState, currDepth + 1, maxDepth, pointOfView * (-1));

                if (heuristic >= bestHeuristic)
                {
                    //if(heuristic != bestHeuristic)
                    prevBestHeuristic = bestHeuristic;
                    bestHeuristic = heuristic;
                    if (currDepth == 0)
                    {
                        this.TheBestTablutBoardState = (TablutBoardState)possibleBoardState.Clone();
                        this.TheBestDepth = currDepth;
                        if (bestHeuristic != prevBestHeuristic)
                        {
                            this.AllTheBestBoardStates.Clear();
                        }
                        this.AllTheBestBoardStates.Add(TheBestTablutBoardState);

                    }
                }
            }
            return bestHeuristic;
        }

        

        public override TablutBoardState MakeMove()
        {
            NegaMaxEvaluateState2(Game.CurrentTablutBoardState, 0, MaxTreeDepth, 1);
            Random rng = new Random();
            return this.AllTheBestBoardStates[rng.Next(AllTheBestBoardStates.Count)];
        }
    }
}
