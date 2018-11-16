using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BoardGremiumBotDecisions.AI;
using BoardGremiumBotDecisions.AI.Adugo;
using BoardGremiumBotDecisions.Tablut;

namespace BoardGremiumBotDecisions.Adugo
{
    public class AdugoBotModel : BotModel
    {
        public new AdugoBot Bot { get; set; }
        public new AdugoBotClient BotClient { get; set; }

        public AdugoBotModel(FieldType botPawn, string gameName, AdugoBotClient botClient, bool isFirstPlayerJoined, BotAlgorithmsParameters botAlgorithm)
        {
            BotPawnColor = botPawn;
            GameName = gameName;
            BotClient = botClient;
            IsFirstPlayerJoined = isFirstPlayerJoined;
            BotAlgorithm = botAlgorithm;
            FieldType humanPawnType;
            humanPawnType = botPawn.Equals(FieldType.JAGUAR_PAWN) ? FieldType.DOG_PAWN : FieldType.JAGUAR_PAWN;
            var game = new AdugoGame(humanPawnType, AdugoUtils.BOARD_WIDTH, AdugoUtils.BOARD_HEIGHT);
            var currentBoardState = (AdugoBoardState)botClient.HttpGet_CurrentBoardState(gameName);

            game.CurrentBoardState =  currentBoardState;
            Bot = new AdugoBot(game);
        }

        protected override void UpdateCurrentBoardState()
        {
            var bs = (AdugoBoardState)BotClient.HttpGet_CurrentBoardState(GameName);
            this.Bot.Game.CurrentBoardState = bs;
        }

        protected override void LogVictory()
        {
            var winnerColor = BotClient.GetWinnerColor(GameName);
            Console.WriteLine("Game has finished. " + winnerColor + " pawns won.");
        }

        private string MoveMessage(AdugoBoardState previous, AdugoBoardState next)
        {
            //This will be called only for bot player
            AdugoField selectedField = null, targetField = null;
            for (var y = 0; y < AdugoUtils.BOARD_HEIGHT; y++)
            {
                for (var x = 0; x < AdugoUtils.BOARD_WIDTH; x++)
                {
                    if (AdugoUtils.PawnsInSameTeam(previous.BoardFields[y, x].Type, this.BotPawnColor)
                        && next.BoardFields[y, x].Type.Equals(FieldType.EMPTY_FIELD))
                    {
                        selectedField = previous.BoardFields[y, x];
                    }
                    else if (AdugoUtils.PawnsInSameTeam(next.BoardFields[y, x].Type, this.BotPawnColor)
                             && previous.BoardFields[y, x].Type.Equals(FieldType.EMPTY_FIELD))
                    {
                        targetField = next.BoardFields[y, x];
                    }
                }
            }
            if (selectedField != null && targetField != null)
            {
                var direction = AdugoUtils.GetDirectionFromMove(selectedField, targetField);

                var message = "\"" + GameName + "|move " + selectedField.Y + " " + selectedField.X + " " + direction.ToString() + "\"";

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
                    BotClient.SetHeuristic(GameName, Bot.Heuristic(Bot.Game.CurrentBoardState), BotPawnColor);
                    //wykonujemy ruch
                    var botDecision = Bot.MakeMove();
                    var moveMessage = MoveMessage(Bot.Game.CurrentBoardState, botDecision);
                    Console.WriteLine("MoveMessage: " + moveMessage);
                    var result = BotClient.SendPostMove(moveMessage);
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
