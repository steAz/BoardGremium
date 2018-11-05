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

            InitializePlayerPawnsLabels(gameInfos.FirstPlayerPawn, gameInfos.SecPlayerPawn, gameInfos.IsBot2BotGame);
            this.Title = "Statistics of " + typeOfGame;
            this.ChartVM = new ChartViewModel(gameInfos.Client);
            lineChart.DataContext = ChartVM;
        }

        private void InitializePlayerPawnsLabels(FieldType firstPlayerPawn, 
                                        FieldType secPlayerPawn, bool isBot2BotGame)
        {
            string firstPlayerPawnString, secPlayerPawnString = string.Empty;
            if (firstPlayerPawn == FieldType.RED_PAWN)
            {
                firstPlayerPawnString = "Red";
                secPlayerPawnString = "Black";
            }
            else
            {
                firstPlayerPawnString = "Black";
                secPlayerPawnString = "Red";
            }

            if (isBot2BotGame)
            {
                this.LabelFirstBotParams.Content = firstPlayerPawnString + " bot parameters";
                this.LabelSecBotParams.Content = secPlayerPawnString + " bot parameters";
            }
            else
            {
                this.LabelFirstBotParams.Content = secPlayerPawnString + " bot parameters"; // In TablutViewModel on HumanVsBot mode we keep botPawn as SecondPlayerPawn 
                                                                                            //  and humanPawn as FirstPlayerPawnin
            }
        }

        private void RedHeuristicsButton_Click(object sender, RoutedEventArgs e)
        {
            ChartViewModel vm = lineChart.DataContext as ChartViewModel;
            vm.UpdateDataForChart(this.GameName, FieldType.RED_PAWN);
            lineChart.Title = "Red heuristics' chart";
        }

        private void BlackHeuristicsButton_Click(object sender, RoutedEventArgs e)
        {
            ChartViewModel vm = lineChart.DataContext as ChartViewModel;
            vm.UpdateDataForChart(this.GameName, FieldType.BLACK_PAWN);
            lineChart.Title = "Black heuristics' chart";
        }
    }
}
