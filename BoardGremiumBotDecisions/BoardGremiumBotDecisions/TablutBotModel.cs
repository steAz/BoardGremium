using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;
using System.Net.Http;

namespace BoardGremiumBotDecisions
{
    class TablutBotModel
    {
        public TablutFieldType BotPawnColor { get; set; }
        public string GameName { get; set; }
        public BotClient BotClient { get; }
        public Bot Bot { get; }
        public bool IsFirstPlayerJoined{ get; set; }
        public BotAlgorithmsParameters BotAlgorithm { get; set; }

        public TablutBotModel(TablutFieldType botPawn, string gameName, BotClient botClient, bool isFirstPlayerJoined, BotAlgorithmsParameters botAlgorithm)
        {
            BotPawnColor = botPawn;
            GameName = gameName;
            BotClient = botClient;
            IsFirstPlayerJoined = isFirstPlayerJoined;
            BotAlgorithm = botAlgorithm;
            TablutFieldType humanPawnType;
            if(botPawn.Equals(TablutFieldType.RED_PAWN))
            {
                humanPawnType = TablutFieldType.BLACK_PAWN;
            }else
            {
                humanPawnType = TablutFieldType.RED_PAWN;
            }
            Game game = new TablutGame("", "", "", "", humanPawnType);
            BoardState currentBoardState = botClient.HttpGet_CurrentBoardState(gameName);

            game.currentBoardState = currentBoardState;
            Bot = TablutUtils.BotInstanceFromAlg(BotAlgorithm, game, isFirstPlayerJoined);   
        }

        /// <summary>
        /// It is method which makes move for bot.
        /// </summary>
        /// <returns>true if game has ended</returns>
        public bool Play()
        {
            while(true)
            {
                if (BotClient.IsGameWon(GameName))
                {
                    LogVictory();
                    return true;
                }

                if(BotClient.HttpGet_IsBotMove(GameName, IsFirstPlayerJoined))
                {
                    Console.WriteLine("Making move...");
                    UpdateCurrentBoardState();
                    BotClient.SetHeuristic(GameName, Bot.Heuristic(Bot.Game.currentBoardState), BotPawnColor);
                    //wykonujemy ruch
                    BoardState botDecision = Bot.MakeMove();
                    var moveMessage = MoveMessage(Bot.Game.currentBoardState, botDecision);
                    Console.WriteLine("MoveMessage: " + moveMessage);
                    var result = BotClient.SendPostMove(moveMessage);
                    BotClient.SetHeuristic(GameName, Bot.Heuristic(botDecision), BotPawnColor);
                    if (BotClient.IsGameWon(GameName))
                    {
                        try
                        {
                            LogVictory();
                        }catch(HttpRequestException e)
                        {
                            Console.WriteLine("Error while obtaining winner player: " + e.Message);
                        }
                        
                        Console.WriteLine("");
                        return true;
                    }

                }
                else
                {
                    System.Threading.Thread.Sleep(500);
                }
            }
        }

        private void LogVictory()
        {
            string winnerColor = BotClient.GetWinnerColor(GameName);
            Console.WriteLine("Game has finished. " + winnerColor + " pawns won.");
        }

        private string MoveMessage(BoardState previous, BoardState next)
        {
            //This will be called only for bot player
            Field selectedField = null, targetField = null;
            for (int i = 0; i < TablutUtils.BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < TablutUtils.BOARD_WIDTH; j++)
                {
                    if (TablutUtils.PawnsInSameTeam((TablutFieldType)previous.BoardFields[i, j].Type, BotPawnColor)
                        && next.BoardFields[i, j].Type.Equals(TablutFieldType.EMPTY_FIELD))
                    {
                        selectedField = previous.BoardFields[i, j];
                    }
                    else if (TablutUtils.PawnsInSameTeam((TablutFieldType)next.BoardFields[i, j].Type, BotPawnColor)
                           && previous.BoardFields[i, j].Type.Equals(TablutFieldType.EMPTY_FIELD))
                    {
                        targetField = next.BoardFields[i, j];
                    }
                }
            }
            if (selectedField != null && targetField != null)
            {
                //string message = "" + selectedField.X + " " + selectedField.Y + " " + targetField.X + " " + targetField.Y;
                DirectionEnum direction = TablutUtils.GetDirectionFromMove(selectedField, targetField);
                int numOfFields = TablutUtils.GetNumOfFieldsFromMove(selectedField, targetField, direction);

                //string message = "\"" + GameName + "|move " + selectedField.Y.ToString() + " " + selectedField.X.ToString()
                          //  + " " + selectedDirection.ToString().First() + " " + selectedNumOfFields.ToString() + "\"";

                string message = "\"" + GameName + "|move " + selectedField.Y + " " + selectedField.X + " " + direction.ToString().First() + " " + numOfFields + "\"";

                return message;
            }
            return null;
        }

        private void UpdateCurrentBoardState()
        {
            BoardState bs = BotClient.HttpGet_CurrentBoardState(GameName);
            this.Bot.Game.currentBoardState = bs;
        }
    }
}
