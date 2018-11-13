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
            throw new NotImplementedException();
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
