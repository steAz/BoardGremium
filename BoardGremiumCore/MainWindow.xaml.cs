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

namespace BoardGremiumCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            drawMainWindow();
        }

        private void drawMainWindow()
        {
            this.WindowState = WindowState.Maximized;
            this.Title = "BoardGremium";
            //gameSelection combobox
            ComboBox gameSelection = new ComboBox();
            gameSelection.Name = "GameSelection";
            gameSelection.Width = 200;
            gameSelection.Height = 20;
            gameSelection.IsEditable = true;
            gameSelection.IsReadOnly = true;
            gameSelection.Text = "Select a Game";
            gameSelection.Margin = new Thickness(300, 182, 261, 305);
            //set games to choose
            ComboBoxItem cbi = new ComboBoxItem();
            cbi.Content = "Tablut";
            gameSelection.Items.Add(cbi);
            cbi = new ComboBoxItem();
            cbi.Content = "NextGame";
            gameSelection.Items.Add(cbi);
            MainGrid.Children.Add(gameSelection);
            //create start game button
            Button btn = new Button();
            btn.Width = 80;
            btn.Height = 20;
            //534,182,147.6,305.2
            btn.Margin = new Thickness(534, 182, 147, 305);
            TextBlock tb = new TextBlock();
            tb.Text = "Start";
            btn.Content = tb;
            MainGrid.Children.Add(btn);
        }
    }
}
