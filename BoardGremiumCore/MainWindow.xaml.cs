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
        public Bot bot;
        /// <summary>
        /// Mapping, where images are reflected to pawns
        /// </summary>
        //public Dictionary<Enum, Image> PawnsToImagesDict { get; set; }

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

        private void PrepareGraphics(string boardPath)
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
                    xCoord = 210;//bylo 265 minus 55
                    break;
                case 1:
                    xCoord = 338;
                    break;
                case 2:
                    xCoord = 462;
                    break;
                case 3:
                    xCoord = 588;
                    break;
                case 4:
                    xCoord = 712;
                    break;
                case 5:
                    xCoord = 838;
                    break;
                case 6:
                    xCoord = 967;
                    break;
                case 7:
                    xCoord = 1090;
                    break;
                case 8:
                    xCoord = 1215;
                    break;
            }
            //xCoord = 210 + 70 * field.X;
            switch (field.Y)
            {
                case 0:
                    yCoord = 112;
                    break;
                case 1:
                    yCoord = 180;
                    break;
                case 2:
                    yCoord = 250;
                    break;
                case 3:
                    yCoord = 317;
                    break;
                case 4:
                    yCoord = 382;
                    break;
                case 5:
                    yCoord = 450;
                    break;
                case 6:
                    yCoord = 520;
                    break;
                case 7:
                    yCoord = 585;
                    break;
                case 8:
                    yCoord = 654;
                    break;
            }

            button.Margin = new Thickness(xCoord, yCoord, 0, 0);
        }

        private void DisplayField(Field field, ref int counterOfButtons)
        {
            if (!field.Type.Equals(TablutFieldType.EMPTY_FIELD))
            {
                string path;
                if (!game.ItemToGraphicsDict.TryGetValue(field.Type, out path)) {
                    throw new Exception("Cannot find image's path with the given FieldType");
                }

                Image image = GetImage(path);
                BoardButton button = new BoardButton()
                {
                    Width = MainGrid.ActualWidth / 14,
                    Height = MainGrid.ActualHeight / 14,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Content = GetImage(path),
                    Name = "checker" + counterOfButtons,
                    RepresentedField = field
                };
                counterOfButtons++;


                SetCoordinates(button, field);
                button.Click += new RoutedEventHandler(CheckerClick);
                MainGrid.Children.Add(button);
            }
        }

        private void CheckerClick(object sender, EventArgs e)
        {
            BoardButton clicked = (BoardButton)sender;
            //MessageBox.Show("Button's name is: " + clicked.Name);
            MoveWindow mw = new MoveWindow(game);
            mw.ShowDialog();
            //MessageBox.Show("Dziala przeslanie info: " + mw.NumOfFields + mw.Direction.ToString());
            //TODO  Walniecie tutaj ruchu majac Direction i NumOfFields i jakis while, ktory 
            // w razie wybrania zlych danych znow odpali okno MoveWindow +
            // wyswietlanie komunikatu o zlym ruchu

            
            
            if (IsChosenMoveValid(mw.NumOfFields, mw.Direction, clicked.RepresentedField))
            {
                MakePlayerMove(clicked.RepresentedField, mw.Direction, mw.NumOfFields);
                if(game.IsGameWon(game.currentBoardState, PlayerEnum.HUMAN_PLAYER))
                {
                    MessageBox.Show("Gracz wygrał, reset planszy.");
                    game.currentBoardState = game.StartingPosition();
                    MainGrid.Children.Clear();
                    DisplayBoard();
                    if((TablutFieldType)game.BotPlayerFieldType == TablutFieldType.BLACK_PAWN)
                        return;
                }
                game.currentBoardState = bot.MakeMove(game.currentBoardState);
                if (game.IsGameWon(game.currentBoardState, PlayerEnum.BOT_PLAYER))
                {
                    MessageBox.Show("Bot wygrał, reset planszy.");
                    game.currentBoardState = game.StartingPosition();
                }else
                {
                    MainGrid.Children.Clear();
                    DisplayBoard();
                }
                
            }else
            {
                if(mw.NumOfFields != 0)
                    MessageBox.Show("Nieprawidlowy ruch.");
            }
        }

        private Boolean IsChosenMoveValid(int numOfFields, DirectionEnum direction, Field field)
        {
            if (numOfFields <= 0)
                return false;
            else if (numOfFields > game.CalculateMaximumPossibleRange(game.currentBoardState, field, direction))
                return false;
            else if (((TablutFieldType)game.HumanPlayerFieldType == TablutFieldType.BLACK_PAWN && (TablutFieldType)field.Type != TablutFieldType.BLACK_PAWN)
                || ((TablutFieldType)game.HumanPlayerFieldType == TablutFieldType.WHITE_PAWN && (TablutFieldType)field.Type == TablutFieldType.BLACK_PAWN))
                return false;

            return true;
        }

        private void MakePlayerMove(Field selectedField, DirectionEnum selectedDirection, int selectedNumOfFields)
        {
            game.MovePawn(game.currentBoardState, selectedField, selectedDirection, selectedNumOfFields);
            MainGrid.Children.Clear();
            DisplayBoard();
        }

        private void LoadBoardForGame(string titleOfGame)
        {
            MainGrid.Children.Clear();
            this.Title = titleOfGame;

            if (titleOfGame == "Tablut")
            {
                TablutFieldType gamerPawns = TablutFieldType.EMPTY_FIELD;
                string boardPath = "viewObjects\\Tablut.jpg";
                string kingPath = "viewObjects\\checker_king.gif";
                string blackPath = "viewObjects\\checker_black.gif";
                string redPath = "viewObjects\\checker_red.gif";
                if (PawnsSelectionCB.Text == "Red")
                    gamerPawns = TablutFieldType.WHITE_PAWN;
                else if(PawnsSelectionCB.Text == "Black")
                    gamerPawns = TablutFieldType.BLACK_PAWN;
                game = new TablutGame(boardPath, redPath, blackPath, kingPath, gamerPawns);
                bot = new Bot(game);
                if((TablutFieldType)game.BotPlayerFieldType == TablutFieldType.WHITE_PAWN)
                {
                    game.currentBoardState = bot.MakeMove(game.currentBoardState);
                }
                PrepareGraphics(boardPath);

                DisplayBoard();

            }

        }

        private void DisplayBoard()
        {
            PrepareGraphics("viewObjects\\Tablut.jpg");
            int counterOfButtons = 0;
            foreach (Field field in game.currentBoardState.BoardFields)
            {
                DisplayField(field, ref counterOfButtons);
            }
            
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if(GameSelectionCB.Text != "Select a game") LoadBoardForGame(GameSelectionCB.Text);
        }
    }
}
