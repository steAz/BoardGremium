using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;
using BoardGremiumBotDecisions.Adugo;

namespace BoardGremiumBotDecisions.AI.Adugo
{
    public class AlfaBetaAdugoBot : MinMaxAdugoBot
    {
        public AlfaBetaAdugoBot(AdugoGame game, int maxTreeDepth) : base(game, maxTreeDepth)
        {

        }

        private int AlphaBetaEvaluateState(AdugoBoardState currBoardState, int currDepth, int maxDepth,
            TraversingLevel typeOfLevel, int alpha, int beta)
        {
            if (currDepth == maxDepth)
            {
                return Heuristic(currBoardState);
            }

            int bestHeuristic = 0;

            List<AdugoBoardState> possibleBoardStates = null;
            if (typeOfLevel == TraversingLevel.MAX)
            {
                possibleBoardStates = Game.GetPossibleAdugoBoardStates(currBoardState, this.Game.BotPlayerFieldType);
                bestHeuristic = -80000;
                int prevBestHeuristic = -80000;
                foreach (AdugoBoardState possibleBoardState in possibleBoardStates.AsEnumerable())
                {
                    if (currDepth == 0)
                    {
                        if (Game.IsGameWon(possibleBoardState, PlayerEnum.BOT_PLAYER))
                        {
                            this.AllTheBestBoardStates.Clear();
                            this.AllTheBestBoardStates.Add((AdugoBoardState) possibleBoardState.Clone());
                            return 100000;
                        }
                    }
                    var heuristic = AlphaBetaEvaluateState(possibleBoardState, currDepth + 1, maxDepth, GetNextTraversingLevel(typeOfLevel), alpha, beta);
                    alpha = Math.Max(alpha, heuristic);
                    if (alpha >= beta)
                    {
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
                        return alpha;
                    }
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
                    var heuristic = AlphaBetaEvaluateState(possibleBoardState, currDepth + 1, maxDepth, GetNextTraversingLevel(typeOfLevel), alpha, beta);
                    beta = Math.Min(beta, heuristic);
                    if (alpha >= beta)
                    {//we cut off an alpha branch
                        return beta;
                    }
                    
                    if (heuristic <= bestHeuristic)
                    {
                        bestHeuristic = heuristic;
                        //this.TheBestTablutBoardState = (TablutBoardState)possibleBoardState.Clone();
                    }
                }
            }

            return bestHeuristic;
        }

        public override AdugoBoardState MakeMove()
        {
            AlphaBetaEvaluateState(Game.CurrentBoardState, 0, MaxTreeDepth, TraversingLevel.MAX, -80000, 80000);  // on output BestBoardState is set
            Console.WriteLine("best depth: " + this.TheBestDepth);
            var rng = new Random();
            return this.AllTheBestBoardStates[rng.Next(AllTheBestBoardStates.Count)];
        }
    }
}
