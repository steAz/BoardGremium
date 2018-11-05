using AbstractGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BoardGremiumCore.Communication
{
    public class GameInfos
    {
        public FieldType FirstPlayerPawn { get; set; }
        public FieldType SecPlayerPawn { get; set; }
        public string GameName { get; set; }
        public Client Client { get; set; }
        public Label PlayerTurnLabel { get; set; } //label displaying information about current player
        public bool IsGameFinished { get; set; }
        public bool IsBot2BotGame { get; set; }
        public bool GameFinishLogged { get; set; }
        public BotAlgorithmsParameters BotAlgParams { get; set; }

        public GameInfos()
        {

        }
    }
}
