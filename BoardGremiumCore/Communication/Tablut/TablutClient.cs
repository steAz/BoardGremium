using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumCore.Communication.Tablut
{
    public class TablutClient : Client
    {
        private static string PostTablutMoveRoute = "/api/Move";

        public TablutClient(string addressIP) : base(addressIP)
        {
            this.AddressIP = addressIP;
        }

        public override async Task<string> SendPostMove(string message)
        {
            var content = new StringContent(message, Encoding.UTF8, "application/json");
            var result = this.PostAsync(AddressIP + PostTablutMoveRoute, content).Result;
            string resultContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine(resultContent);
            return resultContent;
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
                throw new HttpRequestException("Error with getting SecondPlayerColor");
            }
        }

        public override string GetWinnerColor(string gameName)
        {
            var currentPlayerTask = base.SendGetCurrentPlayer(gameName);
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
    }
}
