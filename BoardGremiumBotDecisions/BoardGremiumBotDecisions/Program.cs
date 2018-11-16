using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using AbstractGame;
using BoardGremiumBotDecisions.Adugo;
using BoardGremiumBotDecisions.Tablut;

namespace BoardGremiumBotDecisions
{
    class Program
    {

        static void Main(string[] args)
        {          
            Console.Write("Enter game's name: ");
            var gameName = Console.ReadLine();
            var gameType = new TablutBotClient("http://localhost:54377").HttpGet_GameType(gameName);

            BotClient botClient;
            if (gameType == MessagesConverter.ADUGO_STRING)
            {
               botClient = new AdugoBotClient("http://localhost:54377");
            }
            else
            {
                botClient = new TablutBotClient("http://localhost:54377");
            }

            bool isFirstPlayerJoined = botClient.HttpGet_IsFirstPlayerJoined(gameName);
            BotAlgorithmsParameters botAlgParams = botClient.BotAlgorithmsParams(gameName);

            var botPawnColor = botClient.HttpGet_BotPawnColor(gameName);

            BotModel bm;
            if (gameType == MessagesConverter.ADUGO_STRING)
            {
                bm = new AdugoBotModel(botPawnColor, gameName, (AdugoBotClient)botClient, isFirstPlayerJoined, botAlgParams);
            }
            else
            {
                bm = new TablutBotModel(botPawnColor, gameName, botClient, isFirstPlayerJoined, botAlgParams);
            }

            
            if (bm.Play())
            {
                Console.WriteLine("Press key to exit");
                Console.ReadKey();
            }             
        }
    }
}
