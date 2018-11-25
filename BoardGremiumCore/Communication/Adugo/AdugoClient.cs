using AbstractGame;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumCore.Communication.Adugo
{
    public class AdugoClient : Client
    {
        // private string AddressIP;

        private static string PostAdugoMoveRoute = "/api/AdugoMove";

        public AdugoClient(string addressIP) : base(addressIP)
        {
            this.AddressIP = addressIP;
        }

        public override string GetWinnerColor(string gameName)
        {
            var currentPlayerTask = base.SendGetCurrentPlayer(gameName);
            var currentPlayerString = currentPlayerTask.Result;
            if (currentPlayerString.Equals("HUMAN"))
            {
                var firstPlayerColorTask = HttpGet_FirstPlayerColor(gameName);
                var firstPlayerColorString = firstPlayerColorTask.Result;
                if (!firstPlayerColorString.Equals("JAGUAR") && !firstPlayerColorString.Equals("DOG"))
                {
                    throw new HttpRequestException("Error with getting firstPlayerColor from AdugoClient");
                }

                return firstPlayerColorString;
            }
            else if (currentPlayerString.Equals("BOT"))
            {
                var secondPlayerColorTask = HttpGet_SecondPlayerColor(gameName);
                var secondPlayerColorString = secondPlayerColorTask.Result;
                if (!secondPlayerColorString.Equals("JAGUAR") && !secondPlayerColorString.Equals("DOG"))
                {
                    throw new HttpRequestException("Error with getting secondPlayerColor from AdugoClient");
                }

                return secondPlayerColorString;
            }

            throw new HttpRequestException("Error with getting CurrentPlayer");
        }

        public override async Task<string> SendPostMove(string message)
        {
            var content = new StringContent(message, Encoding.UTF8, "application/json");
            var result = this.PostAsync(AddressIP + PostAdugoMoveRoute, content).Result;
            string resultContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine(resultContent);
            return resultContent;
        }
    }
}
