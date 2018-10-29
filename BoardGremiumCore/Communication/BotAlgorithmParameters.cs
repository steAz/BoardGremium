using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumCore
{
    public class BotAlgorithmsParameters
    {
        public string FirstBotAlgorithmName { get; set; }
        public int FirstBotMaxTreeDepth { get; set; }
        public string SecBotAlgorithmName { get; set; }
        public int SecBotMaxTreeDepth { get; set; }
        public bool IsBot2BotGame { get; set; }

        public BotAlgorithmsParameters(string firstName, string firstDepth, string secName, string secDepth, bool isBot2BotGame)
        {
            FirstBotAlgorithmName = firstName;
            FirstBotMaxTreeDepth = Int32.Parse(firstDepth);
            SecBotAlgorithmName = secName;
            SecBotMaxTreeDepth = Int32.Parse(secDepth);
            IsBot2BotGame = isBot2BotGame;
        }

        public BotAlgorithmsParameters()
        {
            FirstBotAlgorithmName = string.Empty;
            FirstBotMaxTreeDepth = 0;
            SecBotAlgorithmName = string.Empty;
            SecBotMaxTreeDepth = 0;
            IsBot2BotGame = false;
        }


    }
}
