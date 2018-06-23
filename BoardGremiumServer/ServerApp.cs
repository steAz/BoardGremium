using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumServer
{
    class ServerApp
    {
        static void Main(string[] args)
        {
            BoardGremiumServer server = new BoardGremiumServer("127.0.0.1", 13000);
            server.startListening();
        }
    }
}
