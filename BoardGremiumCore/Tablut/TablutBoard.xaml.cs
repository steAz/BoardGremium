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

namespace BoardGremiumCore.Tablut
{
    /// <summary>
    /// Logika interakcji dla klasy TablutBoard.xaml
    /// </summary>
    public partial class TablutBoard : UserControl
    {
        public TablutBoard()
        {
            InitializeComponent();
            
        }

        private void Item_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TablutViewModel vm = this.DataContext as TablutViewModel;
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
