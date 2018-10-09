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
        public TablutFieldType PlayerPawn { get; set; }
        public string GameName { get; set; }
        public Client Client { get; }
        public Label PlayerTurnLabel; //label displaying information about current player
        public bool IsGameFinished;
        public bool IsBot2BotGame { get; set; }
        public bool GameFinishLogged { get; set; }

        public TablutViewModel() { MyBoardState = new BoardState(TablutUtils.BOARD_WIDTH, TablutUtils.BOARD_HEIGHT);  }

        public TablutViewModel(BoardState bs, TablutFieldType playerPawn, string gameName, Client httpClient, Label playerTurnLabel, string gameMode)
        {
            MyBoardState = bs;
            this.PlayerPawn = playerPawn;
            this.GameName = gameName;
            this.GameFinishLogged = false;
            Client = httpClient;
            PlayerTurnLabel = playerTurnLabel;
            UpdatePlayerTurnLabel();

            if (gameMode == "Bot vs Bot")
                this.IsBot2BotGame = true;
            else
                this.IsBot2BotGame = false;

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
            if(Client.IsPlayerTurn(GameName) && !IsGameFinished)
            {
                CheckIsGameWon();
                if (TablutUtils.IsTargetSameType(PlayerPawn, (TablutFieldType)field.Type))
                {
                    MoveWindow mw = new MoveWindow();
                    mw.ShowDialog();
                    if (mw.IsFilled())  // it checks if each of parameters of the move filled 
                    {
                        MakePlayerMove(field, mw.Direction, mw.NumOfFields);
                        System.Threading.Thread.Sleep(1000);
                        if(IsGameFinished)
                        {
                            return true;
                        }
                    }
                }
                //updating gameState after enemy's move
                //while(true)
                //{
                //    //player's turn
                //    if(Client.IsPlayerTurn(GameName))  // ZALATWMY TO DISPATCHEREM JAK Z AKTUALIZOWANIEM PIERWSZEGO RUCHU BOTA
                //    {
                //        UpdatePlayerTurnLabel();
                //        MovePawn();
                //        break;
                //    }
                //}
            }
            return false;
        }

        private void MakePlayerMove(Field selectedField, DirectionEnum selectedDirection, int selectedNumOfFields)
        {
            string message = "\"" +  GameName + "|move " + selectedField.Y.ToString() + " " + selectedField.X.ToString()
                            + " " + selectedDirection.ToString().First() + " " + selectedNumOfFields.ToString() + "\"";

            var result = Client.SendPostMove(message);

            if(result.Result.Contains("ok"))
            {
                MovePawn();
                CheckIsGameWon();
                UpdatePlayerTurnLabel();
            }else
            {
                //we can create another label with information about e.g not valid move
            }

            //ReceiveMessage(selectedField, selectedDirection, selectedNumOfFields, out isRightMove);
            //bot move message
            //if (isRightMove) ReceiveMessage(selectedField, selectedDirection, selectedNumOfFields, out isRightMove);

            //MainGrid.Children.Clear();
            //DisplayBoard();
        }

        public void MovePawn()
        {
            var boardStateRepresentation = Client.SendGetCurrentBoardState(GameName); 
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
            if (Client.IsPlayerTurn(GameName))
            {
                PlayerTurnLabel.Content = "Make your move, my friend";
            }
            else
            {
                PlayerTurnLabel.Content = "Enemy makes move.";
            }
        }

        public void CheckIsGameWon()
        {
            IsGameFinished = Client.IsGameWon(GameName);
            if(IsGameFinished)
            {
                if (!GameFinishLogged)
                {
                    try
                    {
                        string winnerColor = Client.GetWinnerColor(GameName);
                        Console.WriteLine("Game has finished. " + winnerColor + " pawns won.");
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine("Error while obtaining winner player: " + e.Message);
                    }

                    GameFinishLogged = true;
                }
            }
        }
    }
}
