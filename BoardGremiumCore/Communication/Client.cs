﻿using BoardGremiumCore.Communication;
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

        private async Task<string> SendGetCurrentPlayer(string gameName)
        {
            string uri = AddressIP + GetCurrentPlayerRoute(gameName);
            var result = this.GetAsync(uri).Result;
            if(result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                //Console.WriteLine(resultContent);
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

        private async Task<string> HttpPost_JoinGame(string gameName)
        {
            string uri = AddressIP + PostJoinGameRoute(gameName);
            var result = this.PostAsync(uri, null).Result;//POST without body
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                //Console.WriteLine(resultContent);
                return resultContent;
            }
            else
            {
                throw new HttpRequestException("Error while joining human player to game");
            }
        }

        public void HumanPlayerJoinGame(string gameName)
        {
            try
            {
                var postResult = HttpPost_JoinGame(gameName);
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("Error while joining human player to game (changing isFirstPlayerJoined)");
                Console.WriteLine(e.Message);
            }
            
            
        }

        public async Task<string> SendGetCurrentBoardState(string gameName)
        {
            string uri = AddressIP + GetCurrentBoardStateRoute(gameName);
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
            if(getResult.Result.Contains("True"))
            {
                return true;
            }else if(getResult.Result.Contains("False"))
            {
                return false;
            }else
            {
                throw new ServerResponseException("GET IsGameWon for game: " + gameName + " - server's response is not recognized");
            }
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
            if (currentPlayerString.Equals("HUMAN"))
            {
                var firstPlayerColorTask = HttpGet_FirstPlayerColor(gameName);
                var firstPlayerColorString = firstPlayerColorTask.Result;
                if (!firstPlayerColorString.Equals("RED") && !firstPlayerColorString.Equals("BLACK"))
                {
                    throw new HttpRequestException("Error with getting firstPlayerColor");
                }
                else
                {
                    return firstPlayerColorString;
                }
            }
            else if (currentPlayerString.Equals("BOT"))
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

        public string GetIsGameWonRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/IsWon";
        }

        public string PostJoinGameRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/HumanPlayerJoined";
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
