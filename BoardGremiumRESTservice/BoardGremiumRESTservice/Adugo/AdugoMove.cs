using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGremiumRESTservice.Adugo
{
    public class AdugoMove
    {
        public int X { get; }
        public int Y { get; }
        public DirectionEnum Direction { get; }
        public AdugoField ChosenField { get; }

        public AdugoMove(int x, int y, DirectionEnum direction, AdugoGameState gameState)
        {
            this.X = x;
            this.Y = y;
            this.Direction = direction;
            this.ChosenField = gameState.Game.CurrentBoardState.BoardFields[Y, X];
        }
    }
}