using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using AbstractGame;

namespace BoardGremiumBotDecisions
{
    class Program
    {

        static void Main(string[] args)
        {
            GameState gameState = null;
            var botClient = new BotClient("http://localhost:54377");

            
            Console.Write("Enter game's name: ");
            var gameName = Console.ReadLine();

            bool isFirstPlayerJoined = botClient.HttpGet_IsFirstPlayerJoined(gameName);
            BotAlgorithmsParameters botAlgParams = botClient.BotAlgorithmsParams(gameName);

            var botPawnColor = botClient.HttpGet_BotPawnColor(gameName);

            

            TablutBotModel tbm = new TablutBotModel(botPawnColor, gameName, botClient, isFirstPlayerJoined, botAlgParams);
            if (tbm.Play())
            {
                Console.WriteLine("Press key to exit");
                Console.ReadKey();
            }             
        }
    }
}
