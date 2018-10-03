using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;
using System.Windows.Controls;
using BoardGremiumCore.Communication;

namespace BoardGremiumCore.Tablut
{
    public class TablutViewModel
    {
        public BoardState MyBoardState { get; set; }
        public TablutFieldType PlayerPawn { get; set; }
        public string GameName { get; set; }
        public Client Client { get; }
        public Label PlayerTurnLabel; //label displaying information about current player

        public TablutViewModel() { MyBoardState = new BoardState(TablutUtils.BOARD_WIDTH, TablutUtils.BOARD_HEIGHT);  }

        public TablutViewModel(BoardState bs, TablutFieldType playerPawn, string gameName, Client httpClient, Label playerTurnLabel)
        {
            MyBoardState = bs;
            this.PlayerPawn = playerPawn;
            this.GameName = gameName;
            Client = httpClient;
            PlayerTurnLabel = playerTurnLabel;
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
            if(Client.IsPlayerTurn(GameName))
            {
                if (TablutUtils.IsTargetSameType(PlayerPawn, (TablutFieldType)field.Type))
                {
                    MoveWindow mw = new MoveWindow();
                    mw.ShowDialog();
                    if (mw.IsFilled())
                    {
                        MakePlayerMove(field, mw.Direction, mw.NumOfFields);
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                //updating gameState after enemy's move
                while(true)
                {
                    //player's turn
                    if(Client.IsPlayerTurn(GameName))  // ZALATWMY TO DISPATCHEREM JAK Z AKTUALIZOWANIEM PIERWSZEGO RUCHU BOTA
                    {
                        UpdatePlayerTurnLabel();
                        MovePawn();
                        break;
                    }
                }
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
    }
}
