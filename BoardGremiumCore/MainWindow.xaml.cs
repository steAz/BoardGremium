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
using BoardGremiumCore.Communication.Adugo;
using BoardGremiumCore.Communication.Tablut;

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
                { FieldType.RED_PAWN, redPawnPath},
                { FieldType.BLACK_PAWN, blackPawnPath},
                { FieldType.KING, kingPath },
                { FieldType.EMPTY_FIELD, null }
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

        private void CheckerClick(object sender, EventArgs e)
        {
            BoardButton clicked = (BoardButton)sender;
            MoveWindow mw = new MoveWindow();
            mw.ShowDialog();
        }

        private Boolean IsChosenMoveValid(int numOfFields, DirectionEnum direction, Field field)
        {
            if (numOfFields <= 0)
                return false;
            else if (numOfFields > game.CalculateMaximumPossibleRange(game.currentBoardState, field, direction))
                return false;
            else if (((FieldType)game.HumanPlayerFieldType == FieldType.BLACK_PAWN && (FieldType)field.Type != FieldType.BLACK_PAWN)
                || ((FieldType)game.HumanPlayerFieldType == FieldType.RED_PAWN && (FieldType)field.Type == FieldType.BLACK_PAWN))
                return false;

            return true;
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

        private void LoadBoardForGame(string titleOfGame, out FieldType gamerPawns)
        {
            //MainGrid.Children.Clear();
            this.Title = titleOfGame;
            gamerPawns = FieldType.EMPTY_FIELD;

            if (titleOfGame == "Tablut")
            {
                if (PawnsSelectionCB.Text == "Red")
                    gamerPawns = FieldType.RED_PAWN;
                else if (PawnsSelectionCB.Text == "Black")
                    gamerPawns = FieldType.BLACK_PAWN;


                //  currentBoardState = StartingPosition(9, 9);
                //this.SetDictForGraphics(redPath, blackPath, kingPath);
                //PrepareGraphics(boardPath);       
            }
            else if(titleOfGame == "Adugo")
            {
                if (PawnsSelectionCB.Text == "Jaguar")
                    gamerPawns = FieldType.JAGUAR_PAWN;
                else if (PawnsSelectionCB.Text == "Dogs")
                    gamerPawns = FieldType.DOG_PAWN;
            }

            PrepareDebugBox();
        }

        private void PrepareDebugBox()
        {
            debugBox = new TextBox
            {
                Name = "debugBox",
                Width = 500,
                Height = 850,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 10, 0, 0)
            };

            outputter = new TextBoxOutputter(debugBox);
            Console.SetOut(outputter);
            Console.WriteLine("Started");
            MainGrid.Children.Add(debugBox);
        }

        private void CreateGame_Click(object sender, RoutedEventArgs e)
        {
            if (GameSelectionCB.Text != "Select a game" &&
                !CreatedGameNameTB.Text.Equals(String.Empty) && !CreatedGameNameTB.Text.Equals("'Write name of game here'"))
            {
                LoadBoardForGame(GameSelectionCB.Text, out FieldType gamerPawns);

                client = new TablutClient("http://localhost:54377");
                var message = "\"" + CreatedGameNameTB.Text + "," + PawnsSelectionCB.Text.ToUpper() + "\"";
                var postGameResult = client.SendPostGame(message);


                if (!postGameResult.Result.Contains("Error 400"))
                {
                    var gameWindow = new GameWindow(GameSelectionCB.Text, gamerPawns, CreatedGameNameTB.Text, client, GameModeSelectionCB.Text)
                    {
                        Owner = this
                    };

                    string botAlgorithms = string.Empty;
                    var botAlgParams = new BotAlgorithmsParameters
                    {
                        FirstBotAlgorithmName = FirstBotAlgoSelectionCB.Text,
                        FirstBotMaxTreeDepth = Int32.Parse(FirstBotMaxTreeDepth.Text)
                    };
                    if (GameModeSelectionCB.Text.Equals("Human vs Bot"))
                    {
                        client.HumanPlayerJoinGame(CreatedGameNameTB.Text);
                        botAlgParams.IsBot2BotGame = false;
                    }
                    else if (GameModeSelectionCB.Text.Equals("Bot vs Bot"))
                    {
                        botAlgParams.SecBotAlgorithmName = SecBotAlgoSelectionCB.Text;
                        botAlgParams.SecBotMaxTreeDepth = Int32.Parse(SecBotMaxTreeDepth.Text);
                        botAlgParams.IsBot2BotGame = true;
                    }

                    if (GameSelectionCB.Text.Equals("Tablut"))
                    {
                        gameWindow.TablutBoard.GameInfos.BotAlgParams = botAlgParams;
                        client.SetBotAlgorithms(CreatedGameNameTB.Text, botAlgParams);
                    }
                    else if (GameSelectionCB.Text.Equals("Adugo"))
                    {

                    }
                    gameWindow.Show();

                }

            }
        }

        private void GameSelectionCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PawnsSelectionCB.Items.Clear();
            if(GameSelectionCB.SelectedItem.ToString().Contains("Tablut"))
            {
                PawnsSelectionCB.Items.Add("Red");
                PawnsSelectionCB.Items.Add("Black");
            }
            else if(GameSelectionCB.SelectedItem.ToString().Contains("Adugo"))
            {
                PawnsSelectionCB.Items.Add("Jaguar");
                PawnsSelectionCB.Items.Add("Dogs");
            }
            FirstBotAlgoSelectionCB.Items.Add("MinMax");
            FirstBotAlgoSelectionCB.Items.Add("NegaMax");
            SecBotAlgoSelectionCB.Items.Add("MinMax");
            SecBotAlgoSelectionCB.Items.Add("NegaMax");
        }

        private void GameModeSelectionCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GameModeSelectionCB.SelectedItem.ToString().Contains("Bot vs Bot"))
            {
                SecBotAlgoSelectionCB.Visibility = Visibility.Visible;
                SecBotMaxTreeDepth.Visibility = Visibility.Visible;
            }
            else if (GameModeSelectionCB.SelectedItem.ToString().Contains("Human vs Bot"))
            {
                SecBotAlgoSelectionCB.Visibility = Visibility.Hidden;
                SecBotMaxTreeDepth.Visibility = Visibility.Hidden;
            }
                
        }

        private void FirstBotAlgoSelectionCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FirstBotMaxTreeDepth.Items.Clear();
            FirstBotMaxTreeDepth.Visibility = Visibility.Visible;
            FirstBotMaxTreeDepth.Items.Add("1");
            FirstBotMaxTreeDepth.Items.Add("2");
            FirstBotMaxTreeDepth.Items.Add("3");
            FirstBotMaxTreeDepth.Items.Add("4");
        }

        private void SecBotAlgoSelectionCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SecBotMaxTreeDepth.Items.Clear();
            SecBotMaxTreeDepth.Visibility = Visibility.Visible;
            SecBotMaxTreeDepth.Items.Add("1");
            SecBotMaxTreeDepth.Items.Add("2");
            SecBotMaxTreeDepth.Items.Add("3");
            SecBotMaxTreeDepth.Items.Add("4");
        }
    }
}
