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
using Games;

namespace BoardGremiumCore
{
    /// <summary>
    /// Logika interakcji dla klasy MoveWindow.xaml
    /// </summary>
    public partial class MoveWindow : Window
    {
        public int NumOfFields;
        public DirectionEnum Direction;

        public MoveWindow(Game game)
        {
            InitializeComponent();
        }

        private void AcceptMove_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)MoveI.IsChecked)
                NumOfFields = 1;
            else if ((bool)MoveII.IsChecked)
                NumOfFields = 2;
            else if ((bool)MoveIII.IsChecked)
                NumOfFields = 3;
            else if ((bool)MoveIV.IsChecked)
                NumOfFields = 4;
            else if ((bool)MoveV.IsChecked)
                NumOfFields = 5;
            else if ((bool)MoveVI.IsChecked)
                NumOfFields = 6;
            else if ((bool)MoveVII.IsChecked)
                NumOfFields = 7;
            else if ((bool)MoveVIII.IsChecked)
                NumOfFields = 8;

            if ((bool)WayUP.IsChecked)
                Direction = DirectionEnum.UP;
            else if ((bool)WayDOWN.IsChecked)
                Direction = DirectionEnum.DOWN;
            else if ((bool)WayLeft.IsChecked)
                Direction = DirectionEnum.LEFT;
            else if ((bool)WayRight.IsChecked)
                Direction = DirectionEnum.RIGHT;

            this.Close();
        }
    }
}
