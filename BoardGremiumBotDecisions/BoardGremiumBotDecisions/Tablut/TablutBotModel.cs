using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumBotDecisions.Tablut
{
    public class TablutBotModel : BotModel
    {
        public TablutBotModel(FieldType botPawn, string gameName, BotClient botClient, bool isFirstPlayerJoined, BotAlgorithmsParameters botAlgorithm)
                : base (botPawn, gameName, botClient, isFirstPlayerJoined, botAlgorithm)
        {
            FieldType humanPawnType;
            humanPawnType = botPawn.Equals(FieldType.RED_PAWN) ? FieldType.BLACK_PAWN : FieldType.RED_PAWN;
            var game = new TablutGame("", "", "", "", humanPawnType);
            var currentBoardState = (BoardState)botClient.HttpGet_CurrentBoardState(gameName);

            game.currentBoardState = currentBoardState;
            Bot = TablutUtils.BotInstanceFromAlg(BotAlgorithm, game, isFirstPlayerJoined);
        }

        protected override void UpdateCurrentBoardState()
        { 
            var bs = (BoardState)BotClient.HttpGet_CurrentBoardState(GameName);
            this.Bot.Game.currentBoardState = bs;
        }

        protected override void LogVictory()
        {
            var winnerColor = BotClient.GetWinnerColor(GameName);
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
                    if (TablutUtils.PawnsInSameTeam((FieldType)previous.BoardFields[i, j].Type, BotPawnColor)
                        && next.BoardFields[i, j].Type.Equals(FieldType.EMPTY_FIELD))
                    {
                        selectedField = previous.BoardFields[i, j];
                    }
                    else if (TablutUtils.PawnsInSameTeam((FieldType)next.BoardFields[i, j].Type, BotPawnColor)
                             && previous.BoardFields[i, j].Type.Equals(FieldType.EMPTY_FIELD))
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

                string message = "\"" + GameName + "|move " + selectedField.Y + " " + selectedField.X + " " + direction.ToString() + " " + numOfFields + "\"";

                return message;
            }
            return null;
        }

        /// <inheritdoc />
        /// <summary>
        /// It is method which makes move for bot.
        /// </summary>
        /// <returns>true if game has ended</returns>
        public override bool Play()
        {
            while (true)
            {
                if (BotClient.IsGameWon(GameName))
                {
                    LogVictory();
                    return true;
                }

                if (BotClient.HttpGet_IsBotMove(GameName, IsFirstPlayerJoined))
                {
                    Console.WriteLine("Making move...");
                    UpdateCurrentBoardState();
                    BotClient.SetHeuristic(GameName, Bot.Heuristic(Bot.Game.currentBoardState), BotPawnColor);
                    //wykonujemy ruch
                    BoardState botDecision = Bot.MakeMove();
                    var moveMessage = MoveMessage(Bot.Game.currentBoardState, botDecision);
                    Console.WriteLine("MoveMessage: " + moveMessage);
                    BotClient.SendPostMove(moveMessage);
                    BotClient.SetHeuristic(GameName, Bot.Heuristic(botDecision), BotPawnColor);
                    if (BotClient.IsGameWon(GameName))
                    {
                        try
                        {
                            LogVictory();
                        }
                        catch (HttpRequestException e)
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
    }
}
