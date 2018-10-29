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

        public StatisticsWindow(string typeOfGame, BotAlgorithmsParameters botAlgParams,
                            TablutFieldType firstPlayerPawn, TablutFieldType secPlayerPawn, bool isBot2BotGame, Client client, string gameName) //create class with all game infos like these parameters
        {
            InitializeComponent();

            this.GameName = gameName;
            this.LabelFirstBotAlgName.Content = botAlgParams.FirstBotAlgorithmName;
            this.LabelFirstBotTreeDepth.Content = botAlgParams.FirstBotMaxTreeDepth;

            if (isBot2BotGame)
            {
                this.LabelSecBotParams.Visibility = Visibility.Visible;
                this.LabelSecBotAlgName.Content = botAlgParams.SecBotAlgorithmName;
                this.LabelSecBotAlgName.Visibility = Visibility.Visible;
                this.LabelSecBotTreeDepth.Content = botAlgParams.SecBotMaxTreeDepth;
                this.LabelSecBotTreeDepth.Visibility = Visibility.Visible;
            }

            InitializePlayerPawnsLabels(firstPlayerPawn, secPlayerPawn, isBot2BotGame);
            this.Title = "Statistics of " + typeOfGame;
            this.ChartVM = new ChartViewModel(client);
            heuristicsLineChart.DataContext = ChartVM;
        }

        private void InitializePlayerPawnsLabels(TablutFieldType firstPlayerPawn, 
                                        TablutFieldType secPlayerPawn, bool isBot2BotGame)
        {
            string firstPlayerPawnString, secPlayerPawnString = string.Empty;
            if (firstPlayerPawn == TablutFieldType.RED_PAWN)
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
            ChartViewModel vm = heuristicsLineChart.DataContext as ChartViewModel;
            vm.UpdateDataForChart(this.GameName);
        }
    }
}
