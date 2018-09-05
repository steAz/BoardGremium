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
        public Bot bot;
        private Client client;
        private int currentX;
        private int currentY;
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

            
            
         //   if (IsChosenMoveValid(mw.NumOfFields, mw.Direction, clicked.RepresentedField))
        //    {
                MakePlayerMove(clicked.RepresentedField, mw.Direction, mw.NumOfFields);
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

        private void MakePlayerMove(Field selectedField, DirectionEnum selectedDirection, int selectedNumOfFields)
        {
            var message = "move p " + selectedField.X.ToString() + " " + selectedField.Y.ToString()
                            + " " + selectedDirection.ToString().First() + " " + selectedNumOfFields.ToString();
            client.SendPostMove(message);

            //server callback message
            bool isRightMove;
            ReceiveMessage(selectedField, selectedDirection, selectedNumOfFields, out isRightMove);
            //bot move message
            if (isRightMove) ReceiveMessage(selectedField, selectedDirection, selectedNumOfFields, out isRightMove);

            MainGrid.Children.Clear();
            DisplayBoard();
        }

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

            currentBoardState.BoardFields[selectedField.Y + yCoord, selectedField.X + xCoord].Type = selectedField.Type; // firstly, changing the destination field
            currentBoardState.BoardFields[selectedField.Y, selectedField.X].Type = TablutFieldType.EMPTY_FIELD; // then, changing source field to empty

        }

        private void ReceiveMessage(Field field, DirectionEnum direction, int numOfFields, out bool isRightMove)
        {
            var buffer = new byte[256];
            while (true)
            {
                if (client.stream.DataAvailable)
                {
                    var i = client.stream.Read(buffer, 0, buffer.Length);
                    var data = Encoding.ASCII.GetString(buffer, 0, i);

                    Console.WriteLine(data);

                    if (data.StartsWith("enemy"))
                    {
                        var arrayOfData = data.Split(' ');
                        var directionText = arrayOfData[3];
                        var botMovedDirection = GetDirectionFromCode(directionText);
                        var botMovedX = Int32.Parse(arrayOfData[1]);
                        var botMovedY = Int32.Parse(arrayOfData[2]);
                        var botMovedField = currentBoardState.BoardFields[botMovedY, botMovedX];
                        var botMovedNumOfFields = Int32.Parse(arrayOfData[4]);
                        ChangeBoardStateAfterMove(botMovedDirection, botMovedField, botMovedNumOfFields);

                        if (data.Contains("taken"))
                        {
                            var playerTakenX = Int32.Parse(arrayOfData[6]);
                            var playerTakenY = Int32.Parse(arrayOfData[7]);
                            currentBoardState.BoardFields[playerTakenX, playerTakenY].Type = TablutFieldType.EMPTY_FIELD;
                        }

                        isRightMove = true;
                        return;
                    }
                    else if(data.StartsWith("ok"))
                    {
                        var arrayOfData = data.Split(' ');
                        this.ChangeBoardStateAfterMove(direction, field, numOfFields);

                        if (data.Contains("taken")) // also enemy pawn is taken/beaten
                        {
                            var takenX = Int32.Parse(arrayOfData[2]);
                            var takenY = Int32.Parse(arrayOfData[3]);
                            currentBoardState.BoardFields[takenY, takenX].Type = TablutFieldType.EMPTY_FIELD;
                        }

                        isRightMove = true;
                        return;
                    }
                    else if (data.StartsWith("notOk"))
                    {
                        MessageBox.Show("Nieprawidłowy ruch");
                        isRightMove = false;
                        return;
                    }
                }
            }
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
            MainGrid.Children.Clear();
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

                currentBoardState = StartingPosition(9, 9);
                this.SetDictForGraphics(redPath, blackPath, kingPath);
                PrepareGraphics(boardPath);
                PrepareDebugBox();
                DisplayBoard();

            }
        }

        private void PrepareDebugBox()
        {
            debugBox = new TextBox
            {
                Name = "debugBox",
                Width = 195,
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
            PrepareGraphics("viewObjects\\Tablut.jpg");
            MainGrid.Children.Add(debugBox);
            int counterOfButtons = 0;
            foreach (Field field in currentBoardState.BoardFields)
            {
                DisplayField(field, ref counterOfButtons);
            }
            
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (GameSelectionCB.Text != "Select a game")
            {
                TablutFieldType gamerPawns;
                LoadBoardForGame(GameSelectionCB.Text, out gamerPawns);

                client = new Client("127.0.0.1", 13000);
                var message = "gamerPawns " + PawnsSelectionCB.Text.ToLower();
                client.SendPostGame(message);
                if (gamerPawns == TablutFieldType.BLACK_PAWN) // bot needs to make first move and player needs to update it on Board by getting message from server
                {
                    bool isRightMove;
                    ReceiveMessage(null, DirectionEnum.NONE, 0, out isRightMove); // first receiving message from server which gives first bot's move to the graphics -> enemy ...
                    MainGrid.Children.Clear();
                    DisplayBoard();
                }
            }
        }

        public BoardState StartingPosition(int BoardWidth, int BoardHeight)
        {
            BoardState startingBoardState = new BoardState(BoardWidth, BoardHeight);
            for (int i = 0; i < startingBoardState.Height; i++)
            {
                for (int j = 0; j < startingBoardState.Width; j++)
                {
                    //startingBoardState.SetField(j, i, TablutFieldType.EMPTY_FIELD);
                    startingBoardState.BoardFields[i, j] = new Field(j, i, TablutFieldType.EMPTY_FIELD);
                }
            }
            //black pawns
            startingBoardState.BoardFields[3, 0].Type = TablutFieldType.BLACK_PAWN;
            startingBoardState.BoardFields[4, 0].Type = TablutFieldType.BLACK_PAWN;
            startingBoardState.BoardFields[5, 0].Type = TablutFieldType.BLACK_PAWN;
            startingBoardState.BoardFields[4, 1].Type = TablutFieldType.BLACK_PAWN;

            startingBoardState.BoardFields[3, 8].Type = TablutFieldType.BLACK_PAWN;
            startingBoardState.BoardFields[4, 8].Type = TablutFieldType.BLACK_PAWN;
            startingBoardState.BoardFields[4, 7].Type = TablutFieldType.BLACK_PAWN;
            startingBoardState.BoardFields[5, 8].Type = TablutFieldType.BLACK_PAWN;

            startingBoardState.BoardFields[0, 3].Type = TablutFieldType.BLACK_PAWN;
            startingBoardState.BoardFields[0, 4].Type = TablutFieldType.BLACK_PAWN;
            startingBoardState.BoardFields[1, 4].Type = TablutFieldType.BLACK_PAWN;
            startingBoardState.BoardFields[0, 5].Type = TablutFieldType.BLACK_PAWN;

            startingBoardState.BoardFields[8, 3].Type = TablutFieldType.BLACK_PAWN;
            startingBoardState.BoardFields[8, 4].Type = TablutFieldType.BLACK_PAWN;
            startingBoardState.BoardFields[7, 4].Type = TablutFieldType.BLACK_PAWN;
            startingBoardState.BoardFields[8, 5].Type = TablutFieldType.BLACK_PAWN;
            //red pawns

            startingBoardState.BoardFields[4, 2].Type = TablutFieldType.RED_PAWN;
            startingBoardState.BoardFields[4, 3].Type = TablutFieldType.RED_PAWN;
            startingBoardState.BoardFields[4, 5].Type = TablutFieldType.RED_PAWN;
            startingBoardState.BoardFields[4, 6].Type = TablutFieldType.RED_PAWN;

            startingBoardState.BoardFields[2, 4].Type = TablutFieldType.RED_PAWN;
            startingBoardState.BoardFields[3, 4].Type = TablutFieldType.RED_PAWN;
            startingBoardState.BoardFields[5, 4].Type = TablutFieldType.RED_PAWN;
            startingBoardState.BoardFields[6, 4].Type = TablutFieldType.RED_PAWN;
            //king
            startingBoardState.BoardFields[4, 4].Type = TablutFieldType.KING;
            return startingBoardState;
        }
    }
}
