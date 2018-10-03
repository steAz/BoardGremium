using BoardGremiumCore.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumCore
{
    public class Client : HttpClient
    {
        private string AddressIP;

        private static string PostGameRoute = "/api/GameEntitys";
        private static string PostMoveRoute = "/api/Move";

        public Client(string addressIP) : base()
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

        public async Task<string> SendGetCurrentPlayer(string gameName)
        {
            string uri = AddressIP + GetCurrentPlayerRoute(gameName);
            var result = this.GetAsync(uri).Result;
            if(result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine(resultContent);
                return resultContent;
            }else
            {
                Console.WriteLine("NOT FOUND");
                return "NOT FOUND";
            }
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

        public async Task<string> SendGetCurrentBoardState(string gameName)
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
                Console.WriteLine("Failed Status Code");
                return "NOT FOUND";
            }
        }

        public string GetCurrentPlayerRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/CurrentPlayer";
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
