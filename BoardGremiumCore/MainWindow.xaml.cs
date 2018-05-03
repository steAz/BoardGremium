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
        public Game game;
        /// <summary>
        /// Mapping, where images are reflected to pawns
        /// </summary>
        public Dictionary<Enum, Image> PawnsToImagesDict { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private Image GetImage(string imagePath)
        {
            Image image = new Image();
            BitmapImage btImage = new BitmapImage();
            btImage.BeginInit();
            btImage.UriSource = new Uri(imagePath, UriKind.Relative);
            btImage.EndInit();
            image.Source = btImage;
            return image;
        }

        private void PrepareGraphics(string boardPath, string redPath, string blackPath, string kingPath)
        {
            Image boardImage = new Image { Stretch = Stretch.Fill, HorizontalAlignment = HorizontalAlignment.Left };
            BitmapImage btImage = new BitmapImage();
            btImage.BeginInit();
            btImage.UriSource = new Uri(boardPath, UriKind.Relative);
            btImage.EndInit();
            boardImage.Source = btImage;
            MainGrid.Children.Add(boardImage);
        }

        private void SetCoordinates(Button button, Field field)
        {
            double xCoord = 0, yCoord = 0;
            switch (field.X)
            {
                case 0:
                    xCoord = 175;
                    break;
                case 1:
                    xCoord = 280;
                    break;
                case 2:
                    xCoord = 385;
                    break;
                case 3:
                    xCoord = 490;
                    break;
                case 4:
                    xCoord = 595;
                    break;
                case 5:
                    xCoord = 701;
                    break;
                case 6:
                    xCoord = 805;
                    break;
                case 7:
                    xCoord = 910;
                    break;
                case 8:
                    xCoord = 1015;
                    break;
            }

            switch (field.Y)
            {
                case 0:
                    yCoord = 103;
                    break;
                case 1:
                    yCoord = 164;
                    break;
                case 2:
                    yCoord = 225;
                    break;
                case 3:
                    yCoord = 286;
                    break;
                case 4:
                    yCoord = 348;
                    break;
                case 5:
                    yCoord = 409;
                    break;
                case 6:
                    yCoord = 470;
                    break;
                case 7:
                    yCoord = 532;
                    break;
                case 8:
                    yCoord = 593;
                    break;
            }

            button.Margin = new Thickness(xCoord, yCoord, 0, 0);
        }

        private void DisplayField(Field field, ref int counterOfButtons)
        {
            if (!field.Type.Equals(TablutFieldType.EMPTY_FIELD))
            {
                if (!game.ItemToGraphicsDict.TryGetValue(field.Type, out string path)) {
                    throw new Exception("Cannot find image's path with the given FieldType");
                }

                Image image = GetImage(path);
                Button button = new Button()
                {
                    Width = MainGrid.ActualWidth / 14,
                    Height = MainGrid.ActualHeight / 14,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Content = GetImage(path),
                    Name = "checker" + counterOfButtons,
                };
                counterOfButtons++;


                SetCoordinates(button, field);
                button.Click += new RoutedEventHandler(CheckerClick);
                MainGrid.Children.Add(button);
            }
        }

        private void CheckerClick(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            MessageBox.Show("Button's name is: " + clicked.Name);
            MoveWindow mw = new MoveWindow(game);
            mw.ShowDialog();
            MessageBox.Show("Dziala przeslanie info: " + mw.NumOfFields + mw.Direction.ToString());
            //TODO  Walniecie tutaj ruchu majac Direction i NumOfFields i jakis while, ktory 
            // w razie wybrania zlych danych znow odpali okno MoveWindow +
            // wyswietlanie komunikatu o zlym ruchu
        }

        private void LoadBoardForGame(string titleOfGame)
        {
            MainGrid.Children.Clear();
            this.Title = titleOfGame;

            if (titleOfGame == "Tablut")
            {
                string boardPath = "viewObjects\\Tablut.jpg";
                string kingPath = "viewObjects\\checker_king.gif";
                string blackPath = "viewObjects\\checker_black.gif";
                string redPath = "viewObjects\\checker_red.gif";
                game = new TablutGame(boardPath, redPath, blackPath, kingPath);

                PrepareGraphics(boardPath, redPath, blackPath, kingPath);

                int counterOfButtons = 0;
                foreach (Field field in game.currentBoardState.BoardFields)
                {
                    DisplayField(field, ref counterOfButtons);
                }

            }

        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if(GameSelectionCB.Text != "Select a game") LoadBoardForGame(GameSelectionCB.Text);
        }
    }
}
