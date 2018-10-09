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
        private TextBoxOutputter outputter;
        private BoardState currentBoardState;
        private TextBox debugBox;
        Dictionary<Enum, string> ItemToGraphicsDict;
        public Game game;
        private Client client;
        /// <summary>
        /// Mapping, where images are reflected to pawns
        /// </summary>
        //public Dictionary<Enum, Image> PawnsToImagesDict { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetDictForGraphics(string redPawnPath, string blackPawnPath, string kingPath)
        {
            ItemToGraphicsDict = new Dictionary<Enum, string>
            {
                { TablutFieldType.RED_PAWN, redPawnPath},
                { TablutFieldType.BLACK_PAWN, blackPawnPath},
                { TablutFieldType.KING, kingPath },
                { TablutFieldType.EMPTY_FIELD, null }
            };
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
                if (!ItemToGraphicsDict.TryGetValue(field.Type, out path))
                {
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
              //  button.Click += new RoutedEventHandler(CheckerClick);
                MainGrid.Children.Add(button);
            }
        }

        private void CheckerClick(object sender, EventArgs e)
        {
            BoardButton clicked = (BoardButton)sender;
            //MessageBox.Show("Button's name is: " + clicked.Name);
            MoveWindow mw = new MoveWindow();
            mw.ShowDialog();
            //MessageBox.Show("Dziala przeslanie info: " + mw.NumOfFields + mw.Direction.ToString());
            //TODO  Walniecie tutaj ruchu majac Direction i NumOfFields i jakis while, ktory 
            // w razie wybrania zlych danych znow odpali okno MoveWindow +
            // wyswietlanie komunikatu o zlym ruchu

            
            
         //   if (IsChosenMoveValid(mw.NumOfFields, mw.Direction, clicked.RepresentedField))
        //    {
                //MakePlayerMove(clicked.RepresentedField, mw.Direction, mw.NumOfFields);
                //if(game.IsGameWon(game.currentBoardState, PlayerEnum.HUMAN_PLAYER))
                //{
                //    MessageBox.Show("Gracz wygrał, reset planszy.");
                //    game.currentBoardState = game.StartingPosition();
                //    MainGrid.Children.Clear();
                //    DisplayBoard();
                //    if((TablutFieldType)game.BotPlayerFieldType == TablutFieldType.BLACK_PAWN)
                //        return;
                //}
                //game.currentBoardState = bot.MakeMove(game.currentBoardState);
                //if (game.IsGameWon(game.currentBoardState, PlayerEnum.BOT_PLAYER))
                //{
                //    MessageBox.Show("Bot wygrał, reset planszy.");
                //    game.currentBoardState = game.StartingPosition();
                //}else
                //{
                //    MainGrid.Children.Clear();
                //    DisplayBoard();
                //}
                
          //  }else if (mw.NumOfFields != 0) {
                  //  MessageBox.Show("Nieprawidlowy ruch.");
          //  }
        }

        private Boolean IsChosenMoveValid(int numOfFields, DirectionEnum direction, Field field)
        {
            if (numOfFields <= 0)
                return false;
            else if (numOfFields > game.CalculateMaximumPossibleRange(game.currentBoardState, field, direction))
                return false;
            else if (((TablutFieldType)game.HumanPlayerFieldType == TablutFieldType.BLACK_PAWN && (TablutFieldType)field.Type != TablutFieldType.BLACK_PAWN)
                || ((TablutFieldType)game.HumanPlayerFieldType == TablutFieldType.RED_PAWN && (TablutFieldType)field.Type == TablutFieldType.BLACK_PAWN))
                return false;

            return true;
        }

        //private void MakePlayerMove(Field selectedField, DirectionEnum selectedDirection, int selectedNumOfFields)
        //{
        //    var message = "move p " + selectedField.X.ToString() + " " + selectedField.Y.ToString()
        //                    + " " + selectedDirection.ToString().First() + " " + selectedNumOfFields.ToString();
        //    client.SendPostMove(message, gameName);

        //    //server callback message
        //    bool isRightMove;
        //    ReceiveMessage(selectedField, selectedDirection, selectedNumOfFields, out isRightMove);
        //    //bot move message
        //    if (isRightMove) ReceiveMessage(selectedField, selectedDirection, selectedNumOfFields, out isRightMove);

        //    MainGrid.Children.Clear();
        //    DisplayBoard();
        //}

        private void ChangeBoardStateAfterMove(DirectionEnum direction, Field selectedField, int numOfFields)
        {
            int xCoord = 0;
            int yCoord = 0;

            switch (direction)
            {
                case DirectionEnum.UP:
                {
                    yCoord -= numOfFields;
                    break;
                }
                case DirectionEnum.DOWN:
                {
                    yCoord += numOfFields;
                    break;
                }
                case DirectionEnum.RIGHT:
                {
                    xCoord += numOfFields;
                    break;
                }
                case DirectionEnum.LEFT:
                {
                    xCoord -= numOfFields;
                    break;
                }
            }

            //currentBoardState.BoardFields[selectedField.Y + yCoord, selectedField.X + xCoord].Type = selectedField.Type; // firstly, changing the destination field
           // currentBoardState.BoardFields[selectedField.Y, selectedField.X].Type = TablutFieldType.EMPTY_FIELD; // then, changing source field to empty

        }

        static private DirectionEnum GetDirectionFromCode(string directionCode)
        {
            if (directionCode.Equals("L"))
            {
                return DirectionEnum.LEFT;
            }
            else if (directionCode.Equals("R"))
            {
                return DirectionEnum.RIGHT;
            }
            else if (directionCode.Equals("U"))
            {
                return DirectionEnum.UP;
            }
            else
            {
                return DirectionEnum.DOWN;
            }
        }

        private void LoadBoardForGame(string titleOfGame, out TablutFieldType gamerPawns)
        {
            //MainGrid.Children.Clear();
            this.Title = titleOfGame;
            gamerPawns = TablutFieldType.EMPTY_FIELD;

            if (titleOfGame == "Tablut")
            {
                string boardPath = "viewObjects\\Tablut.jpg";
                string kingPath = "viewObjects\\checker_king.gif";
                string blackPath = "viewObjects\\checker_black.gif";
                string redPath = "viewObjects\\checker_red.gif";
                if (PawnsSelectionCB.Text == "Red")
                    gamerPawns = TablutFieldType.RED_PAWN;
                else if(PawnsSelectionCB.Text == "Black")
                    gamerPawns = TablutFieldType.BLACK_PAWN;


              //  currentBoardState = StartingPosition(9, 9);
                //this.SetDictForGraphics(redPath, blackPath, kingPath);
                //PrepareGraphics(boardPath);
                PrepareDebugBox();
                DisplayBoard();

            }
        }

        private void PrepareDebugBox()
        {
            debugBox = new TextBox
            {
                Name = "debugBox",
                Width = 1000,
                Height = 850,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 10, 0, 0)
            };

            outputter = new TextBoxOutputter(debugBox);
            Console.SetOut(outputter);
            Console.WriteLine("Started");
        }

        private void DisplayBoard()
        {
            //PrepareGraphics("viewObjects\\Tablut.jpg");
            MainGrid.Children.Add(debugBox);
            int counterOfButtons = 0;

            //for (int i = 0; i != currentBoardState.Width; ++i)
            //{
            //    for (int j = 0; j != currentBoardState.Height; ++j)
            //    {
            // //       DisplayField(currentBoardState)
            //    }
            //}

          //  foreach (Field field in currentBoardState.BoardFields)
          //  {
           //     DisplayField(field, ref counterOfButtons);
          //  }
            
        }

        private void CreateGame_Click(object sender, RoutedEventArgs e)
        {
            if (GameSelectionCB.Text != "Select a game" && 
                !CreatedGameNameTB.Text.Equals(String.Empty) && !CreatedGameNameTB.Text.Equals("'Tutaj wpisz nazwę gry'"))
            {
                LoadBoardForGame(GameSelectionCB.Text, out TablutFieldType gamerPawns);

                client = new Client("http://localhost:54377");
                var message = "\"" + CreatedGameNameTB.Text + "," + PawnsSelectionCB.Text.ToUpper() + "\"";
                var postGameResult = client.SendPostGame(message);
                

                if(!postGameResult.Result.Contains("Error 400"))
                {
                    var gameWindow = new GameWindow(gamerPawns, CreatedGameNameTB.Text, client, GameModeSelectionCB.Text)
                    {
                        Owner = this
                    };

                    if(GameModeSelectionCB.Text.Equals("Human vs Bot"))
                        client.HumanPlayerJoinGame(CreatedGameNameTB.Text);

                    gameWindow.Show();

                }
 
            }
        }

      
    }
}
