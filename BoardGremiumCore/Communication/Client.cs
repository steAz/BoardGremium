using AbstractGame;
using BoardGremiumCore.Communication;
using Newtonsoft.Json;
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
    public abstract class Client : HttpClient
    {
        protected string AddressIP;

        public Client(string addressIP) : base()
        {
            this.AddressIP = addressIP;
        }

        private async Task<string> SendGetGameType(string gameName)
        {
            var gameTypeRoute = GetGameTypeRoute(gameName);
            var uri = AddressIP + gameTypeRoute;
            var result = GetAsync(uri).Result;
            return null;
        }

        public string GetGameType(string gameName)
        {
            var result = SendGetGameType(gameName);
            return "Tablut";
        }

        public abstract Task<string> SendPostMove(string message);
        public abstract string GetWinnerColor(string gameName);

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
            if(result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                //Console.WriteLine(resultContent);
                return resultContent;
            }else
            {
                Console.WriteLine("Error from server-side while getting current player in client-side");
                return "Error from server-side while getting current player in client-side";
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
                throw new HttpRequestException("Error while finding out is player turn");
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

        private async Task<string> HttpPost_SetBotAlgorithms(string gameName, string botAlgorithmsParamsJSON)
        {
            string uri = AddressIP + PostSetBotAlgorithmsRoute(gameName);
            var content = new StringContent(botAlgorithmsParamsJSON, Encoding.UTF8, "application/json");
            var result = this.PostAsync(uri, content).Result; //POST
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                //Console.WriteLine(resultContent);
                return resultContent;
            }
            else
            {
                throw new HttpRequestException("Error while setting bot algorithms");
            }
        }

        public void SetBotAlgorithms(string gameName, BotAlgorithmsParameters botAlgParams)
        {
            try
            {
                string output = JsonConvert.SerializeObject(botAlgParams);

                output = "\"" + output.Replace('"', '\'') + "\"";
                var postResult = HttpPost_SetBotAlgorithms(gameName, output);

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Error while setting bot algorithms");
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
                throw new HttpRequestException("Error while finding out if game is won");
            }
        }

        protected async Task<string> HttpGet_FirstPlayerColor(string gameName)
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
                throw new HttpRequestException("Error while getting first player color");
            }
        }

        protected async Task<string> HttpGet_SecondPlayerColor(string gameName)
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
                throw new HttpRequestException("Error with getting SecondPlayerColor");
            }
        }

        private async Task<string> HttpGet_Heuristics(string gameName, FieldType playerColor)
        {
            string uri = AddressIP;
            if (playerColor == FieldType.RED_PAWN || playerColor == FieldType.JAGUAR_PAWN)
                uri += GetRedJaguarHeuristicsRoute(gameName);
            else if (playerColor == FieldType.DOG_PAWN || playerColor == FieldType.BLACK_PAWN)
                uri += GetBlackDogHeuristicsRoute(gameName);
            var result = this.GetAsync(uri).Result;
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                return resultContent;
            }
            else
            {
                throw new HttpRequestException("Error while getting heuristics");
            }
        }

        public List<int> GetHeuristics(string gameName, FieldType playerColor)
        {         
            try
            {
                var heuristicsTask = HttpGet_Heuristics(gameName, playerColor);
                var heuristicsString = heuristicsTask.Result; // example: 1,2,124,15 .. as strings
                var liOfHeuristics = new List<int>();
                var tabOfHeuristicsString = heuristicsString.Split(','); // tab of these strings that were above
                foreach(var heuristicString in tabOfHeuristicsString)
                {
                    if(Int32.TryParse(heuristicString, out var heuristicInt))
                        liOfHeuristics.Add(heuristicInt);
                }

                return liOfHeuristics; // list of heuristics as ints
                
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Error probably occured while converting string of heuristics to list of heuristics");
                Console.WriteLine(e.Message);
            }

            return null;
        }

        private async Task<string> HttpGet_TakenPawns(string gameName, FieldType playerColor)
        {
            string uri = AddressIP;
            if (playerColor == FieldType.RED_PAWN || playerColor == FieldType.JAGUAR_PAWN)
                uri += GetTakenPawnsByRedJaguarRoute(gameName);
            else if (playerColor == FieldType.BLACK_PAWN)
                uri += GetTakenPawnsByBlackRoute(gameName);
            var result = this.GetAsync(uri).Result;
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                return resultContent;
            }
            else
            {
                throw new HttpRequestException("Error while getting taken pawns");
            }
        }

        public List<int> GetTakenPawns(string gameName, FieldType playerColor)
        {
            try
            {
                var takenPawnsTask = HttpGet_TakenPawns(gameName, playerColor);
                var takenPawnsString = takenPawnsTask.Result; // example: 1,2,3 .. as strings
                var liOfTakenPawns = new List<int>();
                var tabOfTakenPawnsString = takenPawnsString.Split(','); // tab of these strings that were above
                foreach (var takenPawnString in tabOfTakenPawnsString)
                {
                    if (Int32.TryParse(takenPawnString, out var takenPawnInt))
                        liOfTakenPawns.Add(takenPawnInt);
                }

                return liOfTakenPawns; // list of taken pawns as ints

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Error probably occured while converting string of taken pawns to list of taken pawns");
                Console.WriteLine(e.Message);
            }

            return null;
        }

        protected static string PostGameRoute = "/api/GameEntitys";

        public string GetCurrentPlayerRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/CurrentPlayer";
        }

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

        public string PostSetBotAlgorithmsRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/SetBotAlgorithms";
        }

        public string GetFirstPlayerColorRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/FirstPlayerColor";
        }

        public string GetSecondPlayerColorRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/SecondPlayerColor";
        }

        public string GetRedJaguarHeuristicsRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/RedJaguarHeuristics";
        }

        public string GetBlackDogHeuristicsRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/BlackDogHeuristics";
        }

        public string GetTakenPawnsByRedJaguarRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/TakenRedJaguarPawns";
        }

        public string GetTakenPawnsByBlackRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/TakenBlackPawns";
        }

        public static string GetGameTypeRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/GameType";
        }

        public static string GetBotAlgParamsJSONRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/BotAlgorithmParamsJSON";
        }
    }
}
