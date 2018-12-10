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
using BoardGremiumCore.Communication;

namespace BoardGremiumCore
{
    /// <summary>
    /// Logika interakcji dla klasy StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        private string GameName { get; set; }
        public ChartViewModel ChartVM;


        public StatisticsWindow()
        {
            InitializeComponent();
        }

        public StatisticsWindow(GameInfos gameInfos, string typeOfGame)
        {
            InitializeComponent();

            this.GameName = gameInfos.GameName;
            this.LabelFirstBotAlgName.Content = gameInfos.BotAlgParams.FirstBotAlgorithmName;
            this.LabelFirstBotTreeDepth.Content = gameInfos.BotAlgParams.FirstBotMaxTreeDepth;

            if (gameInfos.IsBot2BotGame)
            {
                this.LabelSecBotParams.Visibility = Visibility.Visible;
                this.LabelSecBotAlgName.Content = gameInfos.BotAlgParams.SecBotAlgorithmName;
                this.LabelSecBotAlgName.Visibility = Visibility.Visible;
                this.LabelSecBotTreeDepth.Content = gameInfos.BotAlgParams.SecBotMaxTreeDepth;
                this.LabelSecBotTreeDepth.Visibility = Visibility.Visible;
            }

            InitializePlayerPawnsLabelsAndButtons(gameInfos.FirstPlayerPawn, gameInfos.SecPlayerPawn, gameInfos.IsBot2BotGame, typeOfGame);
            this.Title = "Statistics of " + typeOfGame;
            this.ChartVM = new ChartViewModel(gameInfos.Client);
            lineChart.DataContext = ChartVM;
        }

        private void InitializePlayerPawnsLabelsAndButtons(FieldType firstPlayerPawn, 
                                        FieldType secPlayerPawn, bool isBot2BotGame, string typeOfGame)
        {
            string firstPlayerPawnString, secPlayerPawnString = string.Empty;
            switch (firstPlayerPawn)
            {
                case FieldType.RED_PAWN:
                    firstPlayerPawnString = "Red";
                    secPlayerPawnString = "Black";
                    FirstHeuristicsButton.Content = "Show red heuristics";
                    SecHeuristicsButton.Content = "Show black heuristics";
                    break;
                case FieldType.BLACK_PAWN:
                    firstPlayerPawnString = "Black";
                    secPlayerPawnString = "Red";
                    FirstHeuristicsButton.Content = "Show red heuristics";
                    SecHeuristicsButton.Content = "Show black heuristics";
                    break;
                case FieldType.DOG_PAWN:
                    firstPlayerPawnString = "Dog";
                    secPlayerPawnString = "Jaguar";
                    FirstHeuristicsButton.Content = "Show jaguar heuristics";
                    SecHeuristicsButton.Content = "Show dog heuristics";
                    break;
                default:
                    firstPlayerPawnString = "Jaguar";
                    secPlayerPawnString = "Dog";
                    FirstHeuristicsButton.Content = "Show jaguar heuristics";
                    SecHeuristicsButton.Content = "Show dog heuristics";
                    break;
            }

            switch (typeOfGame)
            {
                case "Adugo":
                    TakenPawnsByBlackButton.Visibility = Visibility.Hidden;
                    TakenPawnsByRedJaguarButton.Content = "Show taken pawns by jaguar";
                    break;
                case "Tablut":
                    TakenPawnsByRedJaguarButton.Content = "Show taken pawns by red";
                    TakenPawnsByBlackButton.Content = "Show taken pawns by black";
                    break;
                default:
                    throw new ArgumentException("Type of game has wrong value in StatisticsWindow");
            }

            if (isBot2BotGame)
            {
                this.LabelFirstBotParams.Content = firstPlayerPawnString + " bot parameters";
                this.LabelSecBotParams.Content = secPlayerPawnString + " bot parameters";
            }
            else
            {
                this.LabelFirstBotParams.Content = secPlayerPawnString + " bot parameters"; // In TablutViewModel on HumanVsBot mode we keep botPawn as SecondPlayerPawn                                                                                        //  and humanPawn as FirstPlayerPawnin
            }
        }

        private void FirstHeuristicsButton_Click(object sender, RoutedEventArgs e)
        {
            ChartViewModel vm = lineChart.DataContext as ChartViewModel;
            vm.DrawChartWithHeuristics(this.GameName, FieldType.RED_PAWN);
            switch (FirstHeuristicsButton.Content.ToString())
            {
                case "Show red heuristics":
                    lineChart.Title = "Red heuristics' chart";
                    break;
                case "Show jaguar heuristics":
                    lineChart.Title = "Jaguar heuristics' chart";
                    break;
                default:
                    throw new ArgumentException("FirstHeuristicsButtonContent had wrong value in StatisticsWindow");
            }

        }

        private void SecHeuristicsButton_Click(object sender, RoutedEventArgs e)
        {
            ChartViewModel vm = lineChart.DataContext as ChartViewModel;
            vm.DrawChartWithHeuristics(this.GameName, FieldType.BLACK_PAWN);
            switch (SecHeuristicsButton.Content.ToString())
            {
                case "Show black heuristics":
                    lineChart.Title = "Black heuristics' chart";
                    break;
                case "Show dog heuristics":
                    lineChart.Title = "Dog heuristics' chart";
                    break;
            }
        }

        private void TakenPawnsByRedJaguarButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = lineChart.DataContext as ChartViewModel;
            vm.DrawChartWithTakenPawns(this.GameName, FieldType.RED_PAWN);
            switch (TakenPawnsByRedJaguarButton.Content.ToString())
            {
                case "Show taken pawns by red":
                    lineChart.Title = "Taken pawns by red chart";
                    break;
                case "Show taken pawns by jaguar":
                    lineChart.Title = "Taken pawns by jaguar chart";
                    break;
            }
        }

        private void TakenPawnsByBlackButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = lineChart.DataContext as ChartViewModel;
            vm.DrawChartWithTakenPawns(this.GameName, FieldType.BLACK_PAWN);
            lineChart.Title = "Taken pawns by black chart";
        }
    }
}
