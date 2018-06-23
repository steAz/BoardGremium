using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Games;

namespace BoardGremiumServer
{
    public class BoardGremiumServer
    {
        private string ServerIp { get; }
        private int ServerPort { get; }
        private TcpListener Listener;
        private byte[] Buffer;

        public BoardGremiumServer(string ServerIp, int ServerPort)
        {
            this.ServerIp = ServerIp;
            this.ServerPort = ServerPort;
            IPAddress serverAddress = IPAddress.Parse(ServerIp);
            Listener = new TcpListener(serverAddress, ServerPort);
        }

        public void startListening()
        {
            Listener.Start();
            Buffer = new byte[256];
            ListeningLoop();
        }

        private void ListeningLoop()
        {
            Console.Write("Waiting for a connection... ");
            TcpClient client = Listener.AcceptTcpClient();
            Console.WriteLine("Connected!");
            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();
            while (true)
            {
                dataRecivedAction(reciveMessage(stream));
            }
            // Shutdown and end connection
            client.Close();
        }

        /// <summary>
        /// Blocking call returning recived string message
        /// </summary>
        /// <param name="stream"> client stream</param>
        /// <returns></returns>
        private string reciveMessage(NetworkStream stream)
        {
            int i;
            string data;
            // Loop to receive all the data sent by the client.
            while ((i = stream.Read(Buffer, 0, Buffer.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                data = System.Text.Encoding.ASCII.GetString(Buffer, 0, i);
                Console.WriteLine("Received: {0}", data);
                return data;
            }
            return null;
            /*
             * Original loop. Hint for future callback implementation
             *while ((i = stream.Read(Buffer, 0, Buffer.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(Buffer, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        dataRecivedAction(data);
                        //data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    }
             * */
        }
        //p 2 3 L2
        private void dataRecivedAction(string data)
        {
            Console.WriteLine(data);
        }

    }
}
