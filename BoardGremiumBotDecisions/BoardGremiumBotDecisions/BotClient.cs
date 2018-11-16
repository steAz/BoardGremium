using AbstractGame;
using BoardGremiumBotDecisions.Tablut;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumBotDecisions
{
    public abstract class BotClient : HttpClient
    {
        protected string AddressIP;

        private static string PostGameRoute = "/api/GameEntitys";

        public BotClient(string addressIP) : base()
        {
            this.AddressIP = addressIP;
        }

        public abstract string GetWinnerColor(string gameName);
        public abstract Task<string> SendPostMove(string message);


        protected async Task<string> SendGetCurrentBoardState(string gameName)
        {
            string uri = AddressIP + GetCurrentBoardStateRoute(gameName);
            var result = this.GetAsync(uri).Result;
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine(resultContent);
                return resultContent;
            }
            else
            {
                throw new HttpRequestException("Failed Status Code while getting CurrentBoardState");
            }
        }

        public abstract object HttpGet_CurrentBoardState(string gameName);

        public async Task<string> SendPostGame(string jsonMessage)
        {
            //gthis.BaseAddress = new Uri(AddressIP);
            var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
            var result = this.PostAsync(AddressIP + PostGameRoute, content).Result;
            string resultContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine(resultContent);
            return resultContent;
        }

        protected async Task<string> SendGetCurrentPlayer(string gameName)
        {
            string uri = AddressIP + GetCurrentPlayerRoute(gameName);
            var result = this.GetAsync(uri).Result;
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                return resultContent;
            }
            else
            {
                Console.WriteLine("NOT FOUND");
                return "NOT FOUND";
            }
        }


        public bool HttpGet_IsBotMove(string gameName, bool isFirstPlayerJoined)
        {
            var currentPlayer = SendGetCurrentPlayer(gameName); 
            if ((currentPlayer.Result.Contains("HUMAN") && !isFirstPlayerJoined) || (currentPlayer.Result.Contains("BOT") && isFirstPlayerJoined))
            {
                return true;
            }
            else if ((currentPlayer.Result.Contains("HUMAN") && isFirstPlayerJoined) || (currentPlayer.Result.Contains("BOT") && !isFirstPlayerJoined))
            {
                return false;
            }
            else throw new HttpRequestException("Error with getting BotPawnColor as FieldType");

        }

        private async Task<string> SendGetBotPawnColor(string gameName)
        {
            string uri = AddressIP + GetBotPawnColorRoute(gameName);
            var result = this.GetAsync(uri).Result;
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine(resultContent);
                return resultContent;
            }
            else
            {
                throw new HttpRequestException("Error with getting BotPawnColor as TablutFieldType");
            }
        }

        public FieldType HttpGet_BotPawnColor(string gameName)
        {
            var botPawnColorTask = SendGetBotPawnColor(gameName);
            var botPawnColorString = botPawnColorTask.Result;
            return MessagesConverter.PlayerPawnFromMessage(botPawnColorString);              
        }

        private async Task<string> SendGetIsFirstPlayerJoined(string gameName)
        {
            string uri = AddressIP + GetIsFirstPlayerJoinedRoute(gameName);
            var result = this.GetAsync(uri).Result;
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine(resultContent);
                return resultContent;
            }
            else
            {
                throw new HttpRequestException("Error with getting BotPawnColor as TablutFieldType");
            }
        }

        public bool HttpGet_IsFirstPlayerJoined(string gameName)
        {
            var isFirstPlayerJoinedTask = SendGetIsFirstPlayerJoined(gameName);
            var isFirstPlayerJoinedString = isFirstPlayerJoinedTask.Result;

            if(isFirstPlayerJoinedString.Contains(TablutUtils.FIRST_JOINED_STRING))
            {
                return true;
            }else if(isFirstPlayerJoinedString.Contains(TablutUtils.FIRST_NOT_JOINED_STRING))
            {
                return false;
            }else
            {
                throw new HttpRequestException("Error with getting isFirstPlayerJoined");
            }
        }

        private async Task<string> SendGetGameType(string gameName)
        {
            string uri = AddressIP + GetGameTypeRoute(gameName);
            var result = this.GetAsync(uri).Result;
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine(resultContent);
                return resultContent;
            }
            else
            {
                throw new HttpRequestException("Failed Status Code while getting GameType from Bot");
            }
        }

        public string HttpGet_GameType(string gameName)
        {
            var gameTypeTask = SendGetGameType(gameName);
            var gameTypeString = gameTypeTask.Result;
            return gameTypeString;
        }

        private async Task<string> HttpGet_IsGameWon(string gameName)
        {
            string uri = AddressIP + GetIsGameWonRoute(gameName);
            var result = this.GetAsync(uri).Result;
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                //Console.WriteLine(resultContent);
                return resultContent;
            }
            else
            {
                Console.WriteLine("Failed Status Code");
                return "NOT FOUND";
            }
        }

        public bool IsGameWon(string gameName)
        {
            var getResult = HttpGet_IsGameWon(gameName);
            if (getResult.Result.Contains("True"))
            {
                return true;
            }
            else if (getResult.Result.Contains("False"))
            {
                return false;
            }
            else
            {
                throw new ServerResponseException("GET IsGameWon for game: " + gameName + " - server's response is not recognized");
            }
        }

        private async Task<string> HttpGet_BotAlgorithmsParamsJSON(string gameName)
        {
            string uri = AddressIP + GetBotAlgParamsJSONRoute(gameName);
            var result = this.GetAsync(uri).Result;
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

        public string BotAlgorithmsParamsJSON(string gameName)
        {
            try
            {
                var getResult = HttpGet_BotAlgorithmsParamsJSON(gameName);
                return getResult.Result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Error while getting bot algorithm params JSON");
                Console.WriteLine(e.Message);
                return null;
            }
            
        }


        public BotAlgorithmsParameters BotAlgorithmsParams(string gameName)
        {
            var botAlgorithmsParamsJSON = BotAlgorithmsParamsJSON(gameName);
            var deserializedBotAlgParams = JsonConvert.DeserializeObject<BotAlgorithmsParameters>(botAlgorithmsParamsJSON);
            return deserializedBotAlgParams;
        }

        private async Task<string> HttpPost_SetHeuristic(string gameName, int heuristic, FieldType pawnType)
        {
            string uri = AddressIP + PostHeuristicRoute(gameName, pawnType);
            var content = new StringContent(heuristic.ToString(), Encoding.UTF8, "application/json");
            var result = this.PostAsync(uri, content).Result; //POST
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                return resultContent;
            }
            else
            {
                throw new HttpRequestException("Error while setting heuristic");
            }
        }

        public void SetHeuristic(string gameName, int heuristic, FieldType pawnType)
        {
            try
            {
                var postResult = HttpPost_SetHeuristic(gameName, heuristic, pawnType);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Error while setting heuristic");
                Console.WriteLine(e.Message);
            }
        }

        public string GetCurrentPlayerRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/CurrentPlayer";
        }

        public string GetBotPawnColorRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/BotPawnColor";
        }

        //public string PostMoveRoute(string gameName)
        //{
        //   return "/api/GameEntitys/" + gameName + "/Move";
        //  }

        public string GetCurrentBoardStateRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/CurrentBoardState";
        }

        public string GetGameTypeRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/GameType";
        }

        public string GetIsGameWonRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/IsWon";
        }

        public string GetIsFirstPlayerJoinedRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/IsFirstPlayerJoined";
        }

        public string GetFirstPlayerColorRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/FirstPlayerColor";
        }

        public string GetSecondPlayerColorRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/SecondPlayerColor";
        }

        public string GetBotAlgParamsJSONRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/BotAlgorithmParamsJSON";
        }

        public string PostHeuristicRoute(string gameName, FieldType botPawn)
        {
            if(botPawn.Equals(FieldType.RED_PAWN))
            {
                return PostRedHeuristicRoute(gameName);
            }else if (botPawn.Equals(FieldType.BLACK_PAWN))
            {
                return PostBlackHeuristicRoute(gameName);
            }
            else if (botPawn.Equals(FieldType.JAGUAR_PAWN)) // TODO
            {
                return PostRedHeuristicRoute(gameName);
            }
            else if(botPawn.Equals(FieldType.DOG_PAWN)) // TODO

            {
                return PostBlackHeuristicRoute(gameName);
            }
            else
            {
                throw new NullReferenceException("Cannot specific route for heuristic");
            }

        }

        private string PostRedHeuristicRoute(string gameName)
        {
            return "/api/Move/" + gameName + "/RedHeuristics";
        }

        private string PostBlackHeuristicRoute(string gameName)
        {
            return "/api/Move/" + gameName + "/BlackHeuristics";
        }
    }
}
