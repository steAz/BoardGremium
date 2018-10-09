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

        private async Task<string> HttpGet_FirstPlayerColor(string gameName)
        {
            string uri = AddressIP + GetFirstPlayerColorRoute(gameName);
            var result = this.GetAsync(uri).Result;
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

        private async Task<string> HttpGet_SecondPlayerColor(string gameName)
        {
            string uri = AddressIP + GetSecondPlayerColorRoute(gameName);
            var result = this.GetAsync(uri).Result;
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

        public string GetWinnerColor(string gameName)
        {
            var currentPlayerTask = SendGetCurrentPlayer(gameName);
            var currentPlayerString = currentPlayerTask.Result;
            if(currentPlayerString.Equals("HUMAN"))
            {
                var firstPlayerColorTask = HttpGet_FirstPlayerColor(gameName);
                var firstPlayerColorString = firstPlayerColorTask.Result;
                if(!firstPlayerColorString.Equals("RED") && !firstPlayerColorString.Equals("BLACK"))
                {
                    throw new HttpRequestException("Error with getting firstPlayerColor");
                }else
                {
                    return firstPlayerColorString;
                }
            }else if(currentPlayerString.Equals("BOT"))
            {
                var secondPlayerColorTask = HttpGet_SecondPlayerColor(gameName);
                var secondPlayerColorString = secondPlayerColorTask.Result;
                if (!secondPlayerColorString.Equals("RED") && !secondPlayerColorString.Equals("BLACK"))
                {
                    throw new HttpRequestException("Error with getting secondPlayerColor");
                }
                else
                {
                    return secondPlayerColorString;
                }
            }
            else
            {
                throw new HttpRequestException("Error with getting CurrentPlayer");
            }
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
    }
}
