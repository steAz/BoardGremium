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

        public Client(string server, int port) : base()
        {
            this.server = server;
            this.port = port;
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

        public void SendPostGame(string message)
        {
      
        }
    }
}
