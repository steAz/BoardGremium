using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumBotDecisions
{
    public class NegaMaxBot : MinMaxBot
    {
        public NegaMaxBot(Game game) :base(game)
        {

        }

        public int NegaMaxEvaluateState(BoardState currBoardState, int currDepth, int maxDepth, int pointOfView)
        {
            if (currDepth == maxDepth)
            {
                return pointOfView * Heuristic(currBoardState);
            }

            int bestHeuristic = 0;

            List<BoardState> possibleBoardStates = null;

            possibleBoardStates = Game.GetPossibleBoardStates(currBoardState, this.Game.BotPlayerFieldType);
            bestHeuristic = -80000;
            int prevBestHeuristic = -80000;
            foreach (BoardState possibleBoardState in possibleBoardStates.AsEnumerable().Reverse())
            {
                var heuristic = - NegaMaxEvaluateState(possibleBoardState, currDepth + 1, maxDepth, pointOfView * (-1) );

                if (heuristic >= bestHeuristic)
                {
                    //if(heuristic != bestHeuristic)
                    prevBestHeuristic = bestHeuristic;
                    bestHeuristic = heuristic;
                    if (currDepth == 0)
                    {
                        this.TheBestBoardState = (BoardState)possibleBoardState.Clone();
                        this.TheBestDepth = currDepth;
                        if (bestHeuristic != prevBestHeuristic)
                        {
                            this.AllTheBestBoardStates.Clear();
                        }
                        this.AllTheBestBoardStates.Add(TheBestBoardState);

                    }
                }
            }

            return bestHeuristic;
        }

        public override BoardState MakeMove()
        {
            NegaMaxEvaluateState(Game.currentBoardState, 0, 3, 1);
            Random rng = new Random();
            return this.AllTheBestBoardStates[rng.Next(AllTheBestBoardStates.Count)];
        }
    }
}
