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
using AbstractGame;
using System.Windows.Threading;

namespace BoardGremiumCore.Tablut
{
    /// <summary>
    /// Logika interakcji dla klasy TablutBoard.xaml
    /// </summary>
    public partial class TablutBoard : UserControl
    {
        bool StatisticsWindowCreated = false;

        public TablutBoard()
        {
            InitializeComponent();
            //  DispatcherTimer setup
            var dTForUpdatingView = new DispatcherTimer();
            dTForUpdatingView.Tick += new EventHandler(DispatcherTimerForUpdatingTheView_Tick);
            dTForUpdatingView.Interval = new TimeSpan(0, 0, 1);
            dTForUpdatingView.Start();

        }

        private void DispatcherTimerForUpdatingTheView_Tick(object sender, EventArgs e)
        {
            TablutViewModel vm = this.DataContext as TablutViewModel;
            vm.UpdatePlayerTurnLabel();
            vm.MovePawn();
            vm.CheckIsGameWon();

            if (vm.IsGameFinished && !this.StatisticsWindowCreated)
            {
                this.StatisticsWindowCreated = true;
                var statsWindow = new StatisticsWindow("Tablut", vm.BotAlgParams, vm.FirstPlayerPawn, vm.SecPlayerPawn, vm.IsBot2BotGame, vm.Client, vm.GameName);
                statsWindow.Show();
            }

            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }

        private void Item_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TablutViewModel vm = this.DataContext as TablutViewModel;

            if (!vm.IsBot2BotGame) // this is Human vs Bot mode and Clicked is needed to make a move 
            {
                ListBoxItem item = sender as ListBoxItem;
                Field content = item.Content as Field;

                //XXX sometimes if you click really fast you can end up clicking on what the debugger says is a "ListBoxItem {DisconnectedItem}
                //hunting down the exact cause would take ages, and might even be a bug in WPF or something
                if (content == null)
                    return;

                vm.Clicked(content);
            }
        }
    }
}
