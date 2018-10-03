using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumBotDecisions
{
    class BotClient : HttpClient
    {
        private string AddressIP;

        private static string PostGameRoute = "/api/GameEntitys";
        private static string PostMoveRoute = "/api/Move";

        public BotClient(string addressIP) : base()
        {
            this.AddressIP = addressIP;
        }

        public async Task<string> SendPostMove(string message)
        {
            var content = new StringContent(message, Encoding.UTF8, "application/json");
            var result = this.PostAsync(AddressIP + PostMoveRoute, content).Result;
            string resultContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine(resultContent);
            return resultContent;
        }

        public async Task<string> SendPostGame(string jsonMessage)
        {
            //gthis.BaseAddress = new Uri(AddressIP);
            var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
            var result = this.PostAsync(AddressIP + PostGameRoute, content).Result;
            string resultContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine(resultContent);
            return resultContent;
        }

        private async Task<string> SendGetCurrentPlayer(string gameName)
        {
            string uri = AddressIP + GetCurrentPlayerRoute(gameName);
            var result = this.GetAsync(uri).Result;
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine(resultContent);
                return resultContent;
            }
            else
            {
                Console.WriteLine("NOT FOUND");
                return "NOT FOUND";
            }
        }

        public bool HttpGet_IsBotMove(string gameName)
        {
            var currentPlayer = SendGetCurrentPlayer(gameName); 
            if (currentPlayer.Result.Contains("HUMAN"))
            {
                return false;
            }
            else if (currentPlayer.Result.Contains("BOT"))
            {
                return true;
            }
            else throw new HttpRequestException("Error with getting BotPawnColor as TablutFieldType");

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

        public TablutFieldType HttpGet_BotPawnColor(string gameName)
        {
            var botPawnColorTask = SendGetBotPawnColor(gameName);
            var botPawnColorString = botPawnColorTask.Result;
            return MessagesConverter.PlayerPawnFromMessage(botPawnColorString);              
        }

        public bool IsPlayerTurn(string gameName)
        {
            var getResult = SendGetCurrentPlayer(gameName);
            if (getResult.Result.Contains("HUMAN"))
            {
                return true;
            }
            else if (getResult.Result.Contains("BOT"))
            {
                return false;
            }
            else
            {
                throw new ServerResponseException("GET CurrentPlayer for game: " + gameName + " - server's response is not recognized");
            }
        }

        private async Task<string> SendGetCurrentBoardState(string gameName)
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

        public BoardState HttpGet_CurrentBoardState(string gameName)
        {
            var boardStateTask = SendGetCurrentBoardState(gameName);
            var boardStateString = boardStateTask.Result;
            return MessagesConverter.ConvertStringToTablutBoardState(boardStateString);
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

    }
}
