using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;
using System.Windows.Controls;
using BoardGremiumCore.Communication;
using System.Net.Http;

namespace BoardGremiumCore.Tablut
{
    public class TablutViewModel
    {
        public BoardState MyBoardState { get; set; }
        public GameInfos GameInfos {get; set;}

        public TablutViewModel() { MyBoardState = new BoardState(TablutUtils.BOARD_WIDTH, TablutUtils.BOARD_HEIGHT);  }

        public TablutViewModel(BoardState bs, FieldType firstPlayerPawn, string gameName, Client httpClient, Label playerTurnLabel, string gameMode)
        {
            GameInfos = new GameInfos()
            {
                FirstPlayerPawn = firstPlayerPawn, //SecPlayerPawn is setting while setting FirstPlayerPawn in setter
                SecPlayerPawn = (firstPlayerPawn == FieldType.RED_PAWN) ? FieldType.BLACK_PAWN : FieldType.RED_PAWN,
                PlayerTurnLabel = playerTurnLabel,
                Client = httpClient,
                IsGameFinished = false,
                GameName = gameName,
                IsBot2BotGame =  (gameMode == "Bot vs Bot") ? true : false,
                GameFinishLogged = false,
            };
            MyBoardState = bs;
            UpdatePlayerTurnLabel();

            

        }

        public List<List<Field>> MyBoardFields
        {
            get
            {
                return MyBoardState.BoardFields;
            }
        }


        //returns true if game is over
        public bool Clicked(Field field)
        {
            if(GameInfos.Client.IsPlayerTurn(GameInfos.GameName) && !GameInfos.IsGameFinished)
            {
                CheckIsGameWon();
                if (TablutUtils.IsTargetSameType(GameInfos.FirstPlayerPawn, (FieldType)field.Type))
                {
                    MoveWindow mw = new MoveWindow();
                    mw.ShowDialog();
                    if (mw.IsFilled())  // it checks if each of parameters of the move filled 
                    {
                        MakePlayerMove(field, mw.Direction, mw.NumOfFields);
                        System.Threading.Thread.Sleep(1000);
                        if(GameInfos.IsGameFinished)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void MakePlayerMove(Field selectedField, DirectionEnum selectedDirection, int selectedNumOfFields)
        {
            string message = "\"" +  GameInfos.GameName + "|move " + selectedField.Y.ToString() + " " + selectedField.X.ToString()
                            + " " + selectedDirection.ToString().First() + " " + selectedNumOfFields.ToString() + "\"";

            var result = GameInfos.Client.SendPostMove(message);

            if(result.Result.Contains("ok"))
            {
                MovePawn();
                CheckIsGameWon();
                UpdatePlayerTurnLabel();
            }else
            {
                //we can create another label with information about e.g not valid move
            }
        }

        public void MovePawn()
        {
            var boardStateRepresentation = GameInfos.Client.SendGetCurrentBoardState(GameInfos.GameName); 
            var currentBoardState = TablutUtils.ConvertStringToTablutBoardState(boardStateRepresentation.Result);

            for (int i = 0; i != MyBoardState.Width; ++i)
            {
                for (int j = 0; j != MyBoardState.Height; ++j)
                {
                    MyBoardState.BoardFields[i][j].Type = currentBoardState.BoardFields[i][j].Type;
                }
            }

        }

        public void UpdatePlayerTurnLabel()
        {
            if (GameInfos.Client.IsPlayerTurn(GameInfos.GameName))
            {
                GameInfos.PlayerTurnLabel.Content = "Make your move, my friend";
            }
            else
            {
                GameInfos.PlayerTurnLabel.Content = "Enemy makes move.";
            }
        }

        public void CheckIsGameWon()
        {
            GameInfos.IsGameFinished = GameInfos.Client.IsGameWon(GameInfos.GameName);
            if(GameInfos.IsGameFinished)
            {
                if (!GameInfos.GameFinishLogged)
                {
                    try
                    {
                        string winnerColor = GameInfos.Client.GetWinnerColor(GameInfos.GameName);
                        Console.WriteLine("Game has finished. " + winnerColor + " pawns won.");
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine("Error while obtaining winner player: " + e.Message);
                    }

                    GameInfos.GameFinishLogged = true;
                }
            }
        }


    }
}
