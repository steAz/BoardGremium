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

namespace BoardGremiumCore
{
    /// <summary>
    /// Logika interakcji dla klasy GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public BoardState DisplayedBoardState;
        public TablutViewModel TablutBoard;

        public GameWindow(TablutFieldType playerPawn, string gameName, Client httpClient)
        {
            DisplayedBoardState = TablutUtils.StartingPosition();

            

            InitializeComponent();
            TablutBoard = new TablutViewModel(DisplayedBoardState, playerPawn, gameName, httpClient, PlayerTurnLabel);
            tablutBoard.DataContext = TablutBoard;
            
        }
    }
}
