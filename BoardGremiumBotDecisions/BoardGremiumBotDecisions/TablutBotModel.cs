using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;

namespace BoardGremiumBotDecisions
{
    class TablutBotModel
    {
        public TablutFieldType BotPawnColor { get; set; }
        public string GameName { get; set; }
        public BotClient BotClient { get; }
        public Bot Bot { get; }

        public TablutBotModel(TablutFieldType botPawn, string gameName, BotClient botClient)
        {
            BotPawnColor = botPawn;
            GameName = gameName;
            BotClient = botClient;
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
            Bot = new Bot(game);
        }

        //Playing game in loop
        public void Play()
        {
            while(true)
            {
                if(BotClient.HttpGet_IsBotMove(GameName))
                {
                    Console.WriteLine("Making move...");
                    //wykonujemy ruch
                    BoardState botDecision = Bot.MakeMove();
                    var moveMessage = MoveMessage(Bot.Game.currentBoardState, botDecision);
                    Console.WriteLine("MoveMessage: " + moveMessage);
                    var result = BotClient.SendPostMove(moveMessage);
                }else
                {
                    System.Threading.Thread.Sleep(5000);
                    
                    //śpimy albo i nie
                }
            }
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
    }
}
