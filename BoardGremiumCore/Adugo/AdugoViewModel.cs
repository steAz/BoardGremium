using AbstractGame;
using BoardGremiumCore.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BoardGremiumCore.Adugo
{
    public class AdugoViewModel
    {
        public AdugoBoardState MyBoardState { get; set; }
        public GameInfos GameInfos { get; set; }

        public List<List<AdugoField>> MyBoardFields
        {
            get
            {
                return MyBoardState.BoardFields;
            }
        }

        public AdugoViewModel() { MyBoardState = new AdugoBoardState(AdugoUtils.BOARD_WIDTH, AdugoUtils.BOARD_HEIGHT); }

        public AdugoViewModel(AdugoBoardState bs, FieldType firstPlayerPawn, string gameName, Client httpClient, Label playerTurnLabel, string gameMode, Label errorMoveLabel)
        {
            GameInfos = new GameInfos()
            {
                FirstPlayerPawn = firstPlayerPawn, //SecPlayerPawn is setting while setting FirstPlayerPawn in setter
                SecPlayerPawn = (firstPlayerPawn == FieldType.JAGUAR_PAWN) ? FieldType.DOG_PAWN : FieldType.JAGUAR_PAWN,
                PlayerTurnLabel = playerTurnLabel,
                ErrorMoveLabel = errorMoveLabel,
                Client = httpClient,
                IsGameFinished = false,
                GameName = gameName,
                IsBot2BotGame = (gameMode == "Bot vs Bot") ? true : false,
                GameFinishLogged = false,
            };

            MyBoardState = bs;
        }

        //returns true if game is over
        public bool Clicked(AdugoField field)
        {
            if (GameInfos.Client.IsPlayerTurn(GameInfos.GameName) && !GameInfos.IsGameFinished)
            {
                CheckIsGameWon();
                if (AdugoUtils.IsTargetSameType(GameInfos.FirstPlayerPawn, field.Type))
                {
                    var mw = new AdugoMoveWindow();
                    mw.ShowDialog();
                    if (mw.IsFilled())  // it checks if each of parameters of the move filled 
                    {
                        MakePlayerMove(field, mw.Direction);
                        System.Threading.Thread.Sleep(1000);
                        if (GameInfos.IsGameFinished)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void MakePlayerMove(AdugoField selectedField, DirectionEnum selectedDirection)
        {
            string message = "\"" + GameInfos.GameName + "|move " + selectedField.Y.ToString() + " " + selectedField.X.ToString()
                            + " " + selectedDirection.ToString() + "\""; 
                                    // tutaj wysylamy caly string Direction

            var result = GameInfos.Client.SendPostMove(message);

            if (result.Result.Contains("ok"))
            {
                MovePawn();
                CheckIsGameWon();
                UpdatePlayerTurnLabel();
                GameInfos.ErrorMoveLabel.Visibility = Visibility.Hidden;
            }
            else
            {
                GameInfos.ErrorMoveLabel.Content = "Move was not correct.";
                GameInfos.ErrorMoveLabel.Visibility = Visibility.Visible;
            }
        }

        public void MovePawn()
        {
            var boardStateRepresentation = GameInfos.Client.SendGetCurrentBoardState(GameInfos.GameName);
            var currentBoardState = AdugoUtils.ConvertStringToAdugoBoardState(boardStateRepresentation.Result);


            for (int x = 0; x != MyBoardState.Width; ++x)
            {
                for (int y = 0; y != MyBoardState.Height; ++y)
                {
                    MyBoardState.BoardFields[y][x].Type = currentBoardState.BoardFields[y][x].Type;
                }
            }

        }

        public void UpdatePlayerTurnLabel()
        {
            if (GameInfos.Client.IsPlayerTurn(GameInfos.GameName))
            {
                GameInfos.PlayerTurnLabel.Content = "Make your move, my friend.";
                GameInfos.PlayerTurnLabel.Visibility = Visibility.Visible;
            }
            else
            {
                GameInfos.PlayerTurnLabel.Content = "Enemy is making move.";
                GameInfos.PlayerTurnLabel.Visibility = Visibility.Visible;
            }
        }

        public void CheckIsGameWon()
        {
            GameInfos.IsGameFinished = GameInfos.Client.IsGameWon(GameInfos.GameName);
            if (GameInfos.IsGameFinished)
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
