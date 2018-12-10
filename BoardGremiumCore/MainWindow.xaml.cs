using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using AbstractGame;
using BoardGremiumCore.Communication.Adugo;
using BoardGremiumCore.Communication.Tablut;
using BoardGremiumCore.Tablut;
using Newtonsoft.Json;

namespace BoardGremiumCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextBoxOutputter outputter;
        private TablutBoardState _currentTablutBoardState;
        private TextBox debugBox;
        private Client client;
        /// <summary>
        /// Mapping, where images are reflected to pawns
        /// </summary>
        //public Dictionary<Enum, Image> PawnsToImagesDict { get; set; }

        public MainWindow()
        {
            InitializeComponent();
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


                //  _currentTablutBoardState = StartingPosition(9, 9);
                //this.SetDictForGraphics(redPath, blackPath, kingPath);
                //PrepareGraphics(boardPath);       
            }
            else if(titleOfGame == "Adugo")
            {
                if (PawnsSelectionCB.Text == "Jaguar")
                    gamerPawns = FieldType.JAGUAR_PAWN;
                else if (PawnsSelectionCB.Text == "Dog")
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

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private void CreateGame_Click(object sender, RoutedEventArgs e)
        {
            if (GameSelectionCB.Text != "Select a game" &&
                !CreatedGameNameTB.Text.Equals(String.Empty) && !CreatedGameNameTB.Text.Equals("'Write name of game here'")
                && !ServerIpTb.Text.Equals("'Write server IP here'") && !ServerPortTb.Text.Equals("'Write server port here"))
            {
                if (!IsDigitsOnly(ServerPortTb.Text))
                {
                    MessageBox.Show("Port needs to have only numbers");
                    return;
                }


                LoadBoardForGame(GameSelectionCB.Text, out FieldType gamerPawns);

                if (GameSelectionCB.Text.Equals("Tablut"))
                    client = new TablutClient("http://" + ServerIpTb.Text + ":" + ServerPortTb.Text);
                else if(GameSelectionCB.Text.Equals("Adugo"))
                    client = new AdugoClient("http://" + ServerIpTb.Text + ":" + ServerPortTb.Text);
                var message = "\"" + CreatedGameNameTB.Text + "," +
                              PawnsSelectionCB.Text.ToUpper() + "," +
                              GameSelectionCB.Text.ToUpper() + "\"";
                var postGameResult = client.SendPostGame(message);


                if (!postGameResult.Result.Contains("Error 400"))
                {
                    var gameWindow = new GameWindow(GameSelectionCB.Text, gamerPawns, CreatedGameNameTB.Text, client, GameModeSelectionCB.Text)
                    {
                        Owner = this
                    };

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
                        gameWindow.AdugoBoard.GameInfos.BotAlgParams = botAlgParams;
                        client.SetBotAlgorithms(CreatedGameNameTB.Text, botAlgParams);
                    }
                    gameWindow.Show();

                }

            }
        }

        private void GameSelectionCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PawnsSelectionCB.Items.Clear();
            FirstBotAlgoSelectionCB.Items.Clear();
            SecBotAlgoSelectionCB.Items.Clear();
            if(GameSelectionCB.SelectedItem.ToString().Contains("Tablut"))
            {
                PawnsSelectionCB.Items.Add("Red");
                PawnsSelectionCB.Items.Add("Black");
                FirstBotAlgoSelectionCB.Items.Add("MinMax");
                FirstBotAlgoSelectionCB.Items.Add("NegaMax");
                SecBotAlgoSelectionCB.Items.Add("MinMax");
                SecBotAlgoSelectionCB.Items.Add("NegaMax");
            }
            else if(GameSelectionCB.SelectedItem.ToString().Contains("Adugo"))
            {
                PawnsSelectionCB.Items.Add("Jaguar");
                PawnsSelectionCB.Items.Add("Dog");
                FirstBotAlgoSelectionCB.Items.Add("MinMax");
                FirstBotAlgoSelectionCB.Items.Add("AlphaBeta");
                SecBotAlgoSelectionCB.Items.Add("MinMax");
                SecBotAlgoSelectionCB.Items.Add("AlphaBeta");
            }
            
            
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
            if (GameSelectionCB.SelectedItem.ToString().Contains("Adugo"))
            {
                FirstBotMaxTreeDepth.Items.Add("5");
                FirstBotMaxTreeDepth.Items.Add("6");
                FirstBotMaxTreeDepth.Items.Add("7");
                FirstBotMaxTreeDepth.Items.Add("8");
            }
        }

        private void SecBotAlgoSelectionCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SecBotMaxTreeDepth.Items.Clear();
            SecBotMaxTreeDepth.Visibility = Visibility.Visible;
            SecBotMaxTreeDepth.Items.Add("1");
            SecBotMaxTreeDepth.Items.Add("2");
            SecBotMaxTreeDepth.Items.Add("3");
            SecBotMaxTreeDepth.Items.Add("4");
            if (GameSelectionCB.SelectedItem.ToString().Contains("Adugo"))
            {
                SecBotMaxTreeDepth.Items.Add("5");
                SecBotMaxTreeDepth.Items.Add("6");
                SecBotMaxTreeDepth.Items.Add("7");
                SecBotMaxTreeDepth.Items.Add("8");
            }
        }

        private void JoinGame_Click(object sender, RoutedEventArgs e)
        {
            if (!CreatedGameNameTB.Text.Equals("'Write name of game here'")
                && !ServerIpTb.Text.Equals("'Write server IP here'") &&
                !ServerPortTb.Text.Equals("'Write server port here"))
            {
                if (!IsDigitsOnly(ServerPortTb.Text))
                {
                    MessageBox.Show("Port needs to have only numbers");
                    return;
                }

                var gameName = CreatedGameNameTB.Text;
                var uri = "http://" + ServerIpTb.Text + ":" + ServerPortTb.Text;
                var gameType = GetGameType(uri + Client.GetGameTypeRoute(CreatedGameNameTB.Text)).Result;
                //Console.WriteLine("Read: " + gameType);
                if (gameType.Equals("Tablut")) //Tablut
                {
                    client = new TablutClient(uri);
                }
                else //Adugo
                {
                    client = new AdugoClient(uri);
                }

                //potrzeba gamerpawns i gameMode
                var botAlgParams = BotAlgorithmsParams(gameName, uri);
                var gameMode = botAlgParams.IsBot2BotGame ? "Bot vs Bot" : "Human vs Bot";
                var playerPawnColorUri = uri + "/api/GameEntitys/" + gameName + "/FirstPlayerColor";
                var firstPlayerColor = HttpGet_FirstPlayerColor(gameName, playerPawnColorUri).Result;
                FieldType gamerPawns = TablutUtils.PlayerPawnFromMessage(firstPlayerColor);

                var gameWindow = new GameWindow(gameType, gamerPawns, CreatedGameNameTB.Text, client, gameMode)
                {
                    Owner = this
                };

                if (gameType.Equals("Tablut"))
                {
                    gameWindow.TablutBoard.GameInfos.BotAlgParams = botAlgParams;
                }
                else if (gameType.Equals("Adugo"))
                {
                    gameWindow.AdugoBoard.GameInfos.BotAlgParams = botAlgParams;
                }
                gameWindow.Show();

                Console.WriteLine(gameMode);
            }
        }

        private async Task<string> GetGameType(string uri)
        {
            var client = new HttpClient();
            // client.GetAsync(uri).Result;
            var result = client.GetAsync(uri).Result;
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                return resultContent;
            }
            else
            {
                Console.WriteLine("Error from server-side while getting gameType of game");
                return "Error from server-side while getting gameType of game";
            }
        }

        private async Task<string> HttpGet_BotAlgorithmsParamsJSON(string gameName, string uri)
        {
            var client = new HttpClient();
            var result = client.GetAsync(uri).Result;
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                //Console.WriteLine(resultContent);
                return resultContent;
            }
            else
            {
                throw new HttpRequestException("Error while getting bot algorithm params JSON");
            }
        }

        public string BotAlgorithmsParamsJSON(string gameName, string uri)
        {
            try
            {
                var serverResourceUri = uri + Client.GetBotAlgParamsJSONRoute(gameName);
                var getResult = HttpGet_BotAlgorithmsParamsJSON(gameName, serverResourceUri);
                return getResult.Result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Error while getting bot algorithm params JSON");
                Console.WriteLine(e.Message);
                return null;
            }

        }


        public BotAlgorithmsParameters BotAlgorithmsParams(string gameName, string uri)
        {
            var botAlgorithmsParamsJSON = BotAlgorithmsParamsJSON(gameName, uri);
            var deserializedBotAlgParams = JsonConvert.DeserializeObject<BotAlgorithmsParameters>(botAlgorithmsParamsJSON);
            return deserializedBotAlgParams;
        }

        private async Task<string> HttpGet_FirstPlayerColor(string gameName, string uri)
        {
            var client = new HttpClient();
            var result = client.GetAsync(uri).Result;
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine(resultContent);
                return resultContent;
            }
            else
            {
                throw new HttpRequestException("Error with getting FirstPlayerColor");
            }
        }
    }
}
