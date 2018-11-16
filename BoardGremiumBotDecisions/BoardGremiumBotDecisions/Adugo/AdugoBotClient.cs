using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace BoardGremiumBotDecisions.Adugo
{
    public class AdugoBotClient : BotClient
    {
        private static string PostAdugoMoveRoute = "/api/AdugoMove";

        public AdugoBotClient(string addressIP) : base(addressIP)
        {
            this.AddressIP = addressIP;
        }

        public override async Task<string> SendPostMove(string message)
        {
            var content = new StringContent(message, Encoding.UTF8, "application/json");
            var result = this.PostAsync(AddressIP + PostAdugoMoveRoute, content).Result;
            string resultContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine(resultContent);
            return resultContent;
        }

        public override string GetWinnerColor(string gameName)
        {
            throw new NotImplementedException();
        }

        public override object HttpGet_CurrentBoardState(string gameName)
        {
            var boardStateTask = SendGetCurrentBoardState(gameName);
            var boardStateString = boardStateTask.Result;
            return MessagesConverter.ConvertStringToAdugoBoardState(boardStateString);
        }
    }
}
