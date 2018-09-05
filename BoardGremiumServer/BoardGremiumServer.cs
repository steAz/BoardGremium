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
        private TcpListener Listener1;
        private TcpListener Listener2;
        private byte[] Buffer;

        public BoardGremiumServer(string ServerIp, int ServerPort)
        {
            this.ServerIp = ServerIp;
            this.ServerPort = ServerPort;
            IPAddress serverAddress = IPAddress.Parse(ServerIp);
            Listener1 = new TcpListener(serverAddress, ServerPort);
            //Listener2 = new TcpListener(serverAddress, ServerPort);
        }

        public void startListening()
        {
            Listener1.Start();
            //Listener2.Start();
            Buffer = new byte[256];
            ListeningLoop();
        }

        private void ListeningLoop()
        {
            Console.Write("Waiting for a connection... ");
            TcpClient playerClient = Listener1.AcceptTcpClient();
            Console.WriteLine("Player Connected!");
            TcpClient botClient = Listener1.AcceptTcpClient();
            Console.WriteLine("Bot Connected!");
            // Get a stream object for reading and writing
            NetworkStream playerStream = playerClient.GetStream();
            NetworkStream botStream = botClient.GetStream();
            string playerData, botData;
            while (true)
            {
                //dataRecivedAction(reciveMessage(stream));
                playerData = receiveMessage(playerStream);
                if (playerData != null)
                {
                    dataRecivedAction(playerData);
                }

                botData = receiveMessage(botStream);
                if (botData != null)
                {
                    dataRecivedAction(botData);
                }

            }
            // Shutdown and end connection
            playerClient.Close();
            botClient.Close();
        }

        /// <summary>
        /// Blocking call returning recived string message
        /// </summary>
        /// <param name="stream"> client stream</param>
        /// <returns></returns>
        private string receiveMessage(NetworkStream stream)
        {
            int i;
            string data;
            // Loop to receive all the data sent by the client.
            if ((i = stream.Read(Buffer, 0, Buffer.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                data = Encoding.ASCII.GetString(Buffer, 0, i);
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
        //move p 2 3 L2
        private void dataRecivedAction(string data)
        {
            Console.WriteLine("Received: {0}", data);
            if (data.StartsWith("move"))
            {

            }
        }



    }
}
