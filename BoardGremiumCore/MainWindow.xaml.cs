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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Games;
using AbstractGame;

namespace BoardGremiumCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;

        
        public MainWindow()
        {
            InitializeComponent();
            DrawMainWindow();
        }

        private void DrawMainWindow()
        {
            
        }

        private void LoadBoardForGame(string titleOfGame)
        {
            MainGrid.Children.Clear();
            this.Title = titleOfGame;
            
            if(titleOfGame == "Tablut") 
            {
                game = new TablutGame("F:\\Informatyka\\Repositories\\BoardGremium\\BoardGremiumCore\\viewObjects", "", "", "");
                Image image = new Image();
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri("F:\\Informatyka\\Repositories\\BoardGremium\\BoardGremiumCore\\viewObjects", UriKind.Absolute);
                bi3.EndInit();
                image.Stretch = Stretch.Fill;
                image.Source = bi3;
            }

        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if(GameSelectionCB.Text != "Select a game") LoadBoardForGame(GameSelectionCB.Text);
        }
    }
}
