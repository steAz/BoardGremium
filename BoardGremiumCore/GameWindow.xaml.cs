﻿using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BoardGremiumCore.Tablut;
using BoardGremiumCore.Adugo;

namespace BoardGremiumCore
{
    /// <summary>
    /// Logika interakcji dla klasy GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public TablutBoardState DisplayedTablutBoardState;
        public AdugoBoardState DisplayedAdugoBoardState;
        public TablutViewModel TablutBoard;
        public AdugoViewModel AdugoBoard;

        public GameWindow(string gameType, FieldType playerPawn, string gameName, Client httpClient, string gameMode)
        {
            InitializeComponent();
            gameType = gameType.ToLower();
            switch (gameType)
            {
                case "tablut":
                    DisplayedTablutBoardState = TablutUtils.StartingPosition();
                    adugoBoard.DTforUpdatingView.Stop();
                    MainDockPanel.Children.Remove(adugoBoard);
                    TablutBoard = new TablutViewModel(DisplayedTablutBoardState, playerPawn, gameName, httpClient, PlayerTurnLabel, gameMode, ErrorMoveLabel);
                    tablutBoard.DataContext = TablutBoard;
                    break;
                case "adugo":
                    DisplayedAdugoBoardState = AdugoUtils.StartingPosition();
                    tablutBoard.DTforUpdatingView.Stop();
                    MainDockPanel.Children.Remove(tablutBoard);
                    AdugoBoard = new AdugoViewModel(DisplayedAdugoBoardState, playerPawn, gameName, httpClient, PlayerTurnLabel, gameMode, ErrorMoveLabel);
                    adugoBoard.DataContext = AdugoBoard;
                    break;
            }
        }
    }
}
