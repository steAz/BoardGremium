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
    class Client : HttpClient
    {
        private string server;
        private int port;
        private string AddressIP;

        private static string PostGameRoute = "/api/GameEntitys";


        public Client(string addressIP) : base()
        {
            this.AddressIP = addressIP;
        }

        public void SendPostMove(string message)
        {
            try
            {
                var data = Encoding.ASCII.GetBytes(message);
                Console.WriteLine("Sent: {0}", message);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException while sending message: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException while sending message: {0}", e);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("NullReferenceException while sending message: {0}", e);
            }
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

        public string GetCurrentPlayerRoute(string gameName)
        {
            return "/api/GameEntitys/" + gameName + "/CurrentPlayer";
        }
    }
}
