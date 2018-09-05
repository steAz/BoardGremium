using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BotDecisions
{
    class BotClient : TcpClient
    {
        public NetworkStream stream;

        public BotClient(string server, int port) : base(server, port)
        {
            try
            {
                stream = this.GetStream();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException in constructor: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException in constructor: {0}", e);
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                var data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
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
                Console.WriteLine("SocketException while sending message: {0}", e);
            }
        }

    }
}
