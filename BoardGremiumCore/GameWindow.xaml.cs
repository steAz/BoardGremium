using AbstractGame;
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
        public BoardState DisplayedBoardState;
        public TablutViewModel TablutBoard;
        public AdugoViewModel AdugoBoard;

        public GameWindow(string gameType, FieldType playerPawn, string gameName, Client httpClient, string gameMode)
        {
            InitializeComponent();
            if (gameType == "Tablut")
            {
                DisplayedBoardState = TablutUtils.StartingPosition();
                MainDockPanel.Children.Remove(adugoBoard);
                TablutBoard = new TablutViewModel(DisplayedBoardState, playerPawn, gameName, httpClient, PlayerTurnLabel, gameMode);
                tablutBoard.DataContext = TablutBoard;
            }
            else if (gameType == "Adugo")
            {
                DisplayedBoardState = AdugoUtils.StartingPosition();
                MainDockPanel.Children.Remove(tablutBoard);
                AdugoBoard = new AdugoViewModel(DisplayedBoardState);
                adugoBoard.DataContext = AdugoBoard;
            }
            
        }
    }
}
