using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;
using System.Net.Http;
using BoardGremiumBotDecisions.AI;
using BoardGremiumBotDecisions.Tablut;

namespace BoardGremiumBotDecisions
{
    public abstract class BotModel
    {
        public FieldType BotPawnColor { get; set; }
        public string GameName { get; set; }
        public BotClient BotClient { get; set; }
        public Bot Bot { get; set; }
        public bool IsFirstPlayerJoined{ get; set; }
        public BotAlgorithmsParameters BotAlgorithm { get; set; }

        protected BotModel()
        {

        }

        protected BotModel(FieldType botPawn, string gameName, BotClient botClient, bool isFirstPlayerJoined, BotAlgorithmsParameters botAlgorithm)
        {
            BotPawnColor = botPawn;
            GameName = gameName;
            BotClient = botClient;
            IsFirstPlayerJoined = isFirstPlayerJoined;
            BotAlgorithm = botAlgorithm;
        }

        /// <summary>
        /// It is method which makes move for bot.
        /// </summary>
        /// <returns>true if game has ended</returns>
        public abstract bool Play();
        protected abstract void LogVictory();
        protected abstract void UpdateCurrentBoardState();
    }
}
