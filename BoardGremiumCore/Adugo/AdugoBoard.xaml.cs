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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BoardGremiumCore.Adugo
{
    /// <summary>
    /// Logika interakcji dla klasy AdugoBoard.xaml
    /// </summary>
    public partial class AdugoBoard : UserControl
    {
        public DispatcherTimer DTforUpdatingView { get; set; }

        public AdugoBoard()
        {
            InitializeComponent();
            //  DispatcherTimer setup
            DTforUpdatingView = new DispatcherTimer();
            DTforUpdatingView.Tick += new EventHandler(DispatcherTimerForUpdatingTheView_Tick);
            DTforUpdatingView.Interval = new TimeSpan(0, 0, 1);
            DTforUpdatingView.Start();

        }

        private void DispatcherTimerForUpdatingTheView_Tick(object sender, EventArgs e)
        {
            AdugoViewModel vm = this.DataContext as AdugoViewModel;
            vm.UpdatePlayerTurnLabel();
            vm.MovePawn();
            vm.CheckIsGameWon();

            //if (vm.GameInfos.IsGameFinished && !this.StatisticsWindowCreated)
            //{
            //    this.StatisticsWindowCreated = true;
            //    var statsWindow = new StatisticsWindow(vm.GameInfos, "Tablut");
            //    statsWindow.Show();
            //}

            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }

        private void Item_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AdugoViewModel vm = this.DataContext as AdugoViewModel;

            if (!vm.GameInfos.IsBot2BotGame) // this is Human vs Bot mode and Clicked is needed to make a move 
            {
                ListBoxItem item = sender as ListBoxItem;
                AdugoField content = item.Content as AdugoField;

                //XXX sometimes if you click really fast you can end up clicking on what the debugger says is a "ListBoxItem {DisconnectedItem}
                //hunting down the exact cause would take ages, and might even be a bug in WPF or something
                if (content == null)
                    return;

                vm.Clicked(content);
            }
        }
    }
}
