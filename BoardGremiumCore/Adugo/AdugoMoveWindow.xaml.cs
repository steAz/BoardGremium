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

namespace BoardGremiumCore.Adugo
{
    /// <summary>
    /// Interaction logic for AdugoMoveWindow.xaml
    /// </summary>
    public partial class AdugoMoveWindow : Window
    {
        public DirectionEnum Direction; 

        public AdugoMoveWindow()
        {
            InitializeComponent();
        }

        private void AcceptMove_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)WayUP.IsChecked)
                Direction = DirectionEnum.UP;
            else if ((bool)WayDOWN.IsChecked)
                Direction = DirectionEnum.DOWN;
            else if ((bool)WayLeft.IsChecked)
                Direction = DirectionEnum.LEFT;
            else if ((bool)WayRight.IsChecked)
                Direction = DirectionEnum.RIGHT;
            else if ((bool)WayUPRight.IsChecked)
                Direction = DirectionEnum.UPRIGHT;
            else if ((bool)WayUPLeft.IsChecked)
                Direction = DirectionEnum.UPLEFT;
            else if ((bool)WayDOWNRight.IsChecked)
                Direction = DirectionEnum.DOWNRIGHT;
            else if ((bool)WayDOWNLeft.IsChecked)
                Direction = DirectionEnum.DOWNLEFT;

            this.Close();
        }

        public bool IsFilled()
        {
            if (Direction.Equals(null))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
